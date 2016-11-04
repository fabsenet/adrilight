using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Polly;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Rectangle = SharpDX.Rectangle;

namespace adrilight.DesktopDuplication
{
    /// <summary>
    /// Provides access to frame-by-frame updates of a particular desktop (i.e. one monitor), with image and cursor information.
    /// </summary>
    public class DesktopDuplicator : IDisposable
    {
        private Device _mDevice;
        private Texture2DDescription _mTextureDesc;
        private OutputDescription _mOutputDesc;
        private OutputDuplication _mDeskDupl;

        private Texture2D _desktopImageTexture = null;
        private OutputDuplicateFrameInformation _frameInfo = new OutputDuplicateFrameInformation();
        private readonly int _whichGraphicsCardAdapter;
        private int _mWhichOutputDevice = -1;

        private Bitmap _finalImage1, _finalImage2;
        private bool _isFinalImage1 = false;
        private Bitmap FinalImage
        {
            get
            {
                return _isFinalImage1 ? _finalImage1 : _finalImage2;
            }
            set
            {
                if (_isFinalImage1)
                {
                    _finalImage2 = value;
                    if (_finalImage1 != null) _finalImage1.Dispose();
                }
                else
                {
                    _finalImage1 = value;
                    if (_finalImage2 != null) _finalImage2.Dispose();
                }
                _isFinalImage1 = !_isFinalImage1;
            }
        }

        /// <summary>
        /// Duplicates the output of the specified monitor.
        /// </summary>
        /// <param name="whichMonitor">The output device to duplicate (i.e. monitor). Begins with zero, which seems to correspond to the primary monitor.</param>
        public DesktopDuplicator(int whichMonitor)
            : this(0, whichMonitor) { }

        /// <summary>
        /// Duplicates the output of the specified monitor on the specified graphics adapter.
        /// </summary>
        /// <param name="whichGraphicsCardAdapter">The adapter which contains the desired outputs.</param>
        /// <param name="whichOutputDevice">The output device to duplicate (i.e. monitor). Begins with zero, which seems to correspond to the primary monitor.</param>
        public DesktopDuplicator(int whichGraphicsCardAdapter, int whichOutputDevice)
        {
            _whichGraphicsCardAdapter = whichGraphicsCardAdapter;
            this._mWhichOutputDevice = whichOutputDevice;
            
            Adapter1 adapter = null;
            try
            {
                adapter = new Factory1().GetAdapter1(_whichGraphicsCardAdapter);
            }
            catch (SharpDXException ex)
            {
                throw new DesktopDuplicationException("Could not find the specified graphics card adapter.", ex);
            }
            this._mDevice = new Device(adapter);
            Output output = null;
            try
            {
                output = adapter.GetOutput(_mWhichOutputDevice);
            }
            catch (SharpDXException ex)
            {
                throw new DesktopDuplicationException("Could not find the specified output device.", ex);
            }
            var output1 = output.QueryInterface<Output1>();
            this._mOutputDesc = output.Description;
            this._mTextureDesc = new Texture2DDescription()
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = this._mOutputDesc.DesktopBounds.Width,
                Height = this._mOutputDesc.DesktopBounds.Height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = {Count = 1, Quality = 0},
                Usage = ResourceUsage.Staging
            };

            try
            {
                this._mDeskDupl = output1.DuplicateOutput(_mDevice);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.NotCurrentlyAvailable.Result.Code)
                {
                    throw new DesktopDuplicationException(
                        "There is already the maximum number of applications using the Desktop Duplication API running, please close one of the applications and try again.");
                }
            }
        }

        /// <summary>
        /// Retrieves the latest desktop image and associated metadata.
        /// </summary>
        public DesktopFrame GetLatestFrame()
        {
            var frame = new DesktopFrame();
            // Try to get the latest frame; this may timeout
            bool retrievalTimedOut = RetrieveFrame();
            if (retrievalTimedOut)
                return null;
            try
            {
                RetrieveFrameMetadata(frame);
                RetrieveCursorMetadata(frame);
                ProcessFrame(frame);
            }
            finally
            {
                try
                {
                    ReleaseFrame();
                }
                catch
                {
                   //ignored
                }
            }
            return frame;
        }

        private bool RetrieveFrame()
        {
            if (_desktopImageTexture == null)
                _desktopImageTexture = new Texture2D(_mDevice, _mTextureDesc);
            SharpDX.DXGI.Resource desktopResource = null;
            _frameInfo = new OutputDuplicateFrameInformation();
            try
            {
                _mDeskDupl.AcquireNextFrame(500, out _frameInfo, out desktopResource);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                {
                    return true;
                }

                throw new DesktopDuplicationException("Failed to acquire next frame.", ex);
            }

            using (var tempTexture = desktopResource.QueryInterface<Texture2D>())
                _mDevice.ImmediateContext.CopyResource(tempTexture, _desktopImageTexture);
            desktopResource.Dispose();
            return false;
        }

        private void RetrieveFrameMetadata(DesktopFrame frame)
        {

            if (_frameInfo.TotalMetadataBufferSize > 0)
            {
                // Get moved regions
                int movedRegionsLength = 0;
                OutputDuplicateMoveRectangle[] movedRectangles = new OutputDuplicateMoveRectangle[_frameInfo.TotalMetadataBufferSize];
                _mDeskDupl.GetFrameMoveRects(movedRectangles.Length, movedRectangles, out movedRegionsLength);
                frame.MovedRegions = new MovedRegion[movedRegionsLength / Marshal.SizeOf(typeof(OutputDuplicateMoveRectangle))];
                for (int i = 0; i < frame.MovedRegions.Length; i++)
                {
                    frame.MovedRegions[i] = new MovedRegion()
                    {
                        Source = new System.Drawing.Point(movedRectangles[i].SourcePoint.X, movedRectangles[i].SourcePoint.Y),
                        Destination = new System.Drawing.Rectangle(movedRectangles[i].DestinationRect.X, movedRectangles[i].DestinationRect.Y, movedRectangles[i].DestinationRect.Width, movedRectangles[i].DestinationRect.Height)
                    };
                }

                // Get dirty regions
                int dirtyRegionsLength = 0;
                Rectangle[] dirtyRectangles = new Rectangle[_frameInfo.TotalMetadataBufferSize];
                _mDeskDupl.GetFrameDirtyRects(dirtyRectangles.Length, dirtyRectangles, out dirtyRegionsLength);
                frame.UpdatedRegions = new System.Drawing.Rectangle[dirtyRegionsLength / Marshal.SizeOf(typeof(Rectangle))];
                for (int i = 0; i < frame.UpdatedRegions.Length; i++)
                {
                    frame.UpdatedRegions[i] = new System.Drawing.Rectangle(dirtyRectangles[i].X, dirtyRectangles[i].Y, dirtyRectangles[i].Width, dirtyRectangles[i].Height);
                }
            }
            else
            {
                frame.MovedRegions = new MovedRegion[0];
                frame.UpdatedRegions = new System.Drawing.Rectangle[0];
            }
        }

        private void RetrieveCursorMetadata(DesktopFrame frame)
        {
            var pointerInfo = new PointerInfo();

            // A non-zero mouse update timestamp indicates that there is a mouse position update and optionally a shape change
            if (_frameInfo.LastMouseUpdateTime == 0)
                return;

            bool updatePosition = true;

            // Make sure we don't update pointer position wrongly
            // If pointer is invisible, make sure we did not get an update from another output that the last time that said pointer
            // was visible, if so, don't set it to invisible or update.

            if (!_frameInfo.PointerPosition.Visible && (pointerInfo.WhoUpdatedPositionLast != this._mWhichOutputDevice))
                updatePosition = false;

            // If two outputs both say they have a visible, only update if new update has newer timestamp
            if (_frameInfo.PointerPosition.Visible && pointerInfo.Visible && (pointerInfo.WhoUpdatedPositionLast != this._mWhichOutputDevice) && (pointerInfo.LastTimeStamp > _frameInfo.LastMouseUpdateTime))
                updatePosition = false;

            // Update position
            if (updatePosition)
            {
                pointerInfo.Position = new SharpDX.Point(_frameInfo.PointerPosition.Position.X, _frameInfo.PointerPosition.Position.Y);
                pointerInfo.WhoUpdatedPositionLast = _mWhichOutputDevice;
                pointerInfo.LastTimeStamp = _frameInfo.LastMouseUpdateTime;
                pointerInfo.Visible = _frameInfo.PointerPosition.Visible;
            }
                        
            // No new shape
            if (_frameInfo.PointerShapeBufferSize == 0)
                return;

            if (_frameInfo.PointerShapeBufferSize > pointerInfo.BufferSize)
            {
                pointerInfo.PtrShapeBuffer = new byte[_frameInfo.PointerShapeBufferSize];
                pointerInfo.BufferSize = _frameInfo.PointerShapeBufferSize;
            }

            try
            {
                unsafe
                {
                    fixed (byte* ptrShapeBufferPtr = pointerInfo.PtrShapeBuffer)
                    {
                        _mDeskDupl.GetFramePointerShape(_frameInfo.PointerShapeBufferSize, (IntPtr)ptrShapeBufferPtr, out pointerInfo.BufferSize, out pointerInfo.ShapeInfo);
                    }
                }
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Failure)
                {
                    throw new DesktopDuplicationException("Failed to get frame pointer shape.");
                }
            }
        }
        
        private void ProcessFrame(DesktopFrame frame)
        {
            // Get the desktop capture texture
            var mapSource = _mDevice.ImmediateContext.MapSubresource(_desktopImageTexture, 0, MapMode.Read, MapFlags.None);

            FinalImage = new System.Drawing.Bitmap(_mOutputDesc.DesktopBounds.Width, _mOutputDesc.DesktopBounds.Height, PixelFormat.Format32bppRgb);
            var boundsRect = new System.Drawing.Rectangle(0, 0, _mOutputDesc.DesktopBounds.Width, _mOutputDesc.DesktopBounds.Height);
            // Copy pixels from screen capture Texture to GDI bitmap
            var mapDest = FinalImage.LockBits(boundsRect, ImageLockMode.WriteOnly, FinalImage.PixelFormat);
            var sourcePtr = mapSource.DataPointer;
            var destPtr = mapDest.Scan0;
            for (int y = 0; y < _mOutputDesc.DesktopBounds.Height; y++)
            {
                // Copy a single line 
                Utilities.CopyMemory(destPtr, sourcePtr, _mOutputDesc.DesktopBounds.Width * 4);

                // Advance pointers
                sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                destPtr = IntPtr.Add(destPtr, mapDest.Stride);
            }

            // Release source and dest locks
            FinalImage.UnlockBits(mapDest);
            _mDevice.ImmediateContext.UnmapSubresource(_desktopImageTexture, 0);
            frame.DesktopImage = FinalImage;
        }

        private void ReleaseFrame()
        {
            try
            {
                _mDeskDupl.ReleaseFrame();
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Failure)
                {
                    throw new DesktopDuplicationException("Failed to release frame.");
                }
            }
        }

        public void Dispose()
        {
            _desktopImageTexture?.Dispose();
            _mDeskDupl?.Dispose();
            _mDevice?.Dispose();
            _finalImage1?.Dispose();
            _finalImage2?.Dispose();
        }
    }
}
