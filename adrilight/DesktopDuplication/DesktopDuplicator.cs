using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using adrilight.Extensions;

using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Rectangle = SharpDX.Mathematics.Interop.RawRectangle;

namespace adrilight.DesktopDuplication
{
    /// <summary>
    /// Provides access to frame-by-frame updates of a particular desktop (i.e. one monitor), with image and cursor information.
    /// </summary>
    public class DesktopDuplicator : IDisposable
    {
        private readonly Device _device;
        private OutputDescription _outputDescription;
        private readonly OutputDuplication _outputDuplication;

        private Texture2D _desktopImageTexture;
        private OutputDuplicateFrameInformation _frameInfo;

        /// <summary>
        /// Duplicates the output of the specified monitor on the specified graphics adapter.
        /// </summary>
        /// <param name="whichGraphicsCardAdapter">The adapter which contains the desired outputs.</param>
        /// <param name="whichOutputDevice">The output device to duplicate (i.e. monitor). Begins with zero, which seems to correspond to the primary monitor.</param>
        public DesktopDuplicator(int whichGraphicsCardAdapter, int whichOutputDevice)
        {
            Adapter1 adapter;
            try
            {
                adapter = new Factory1().GetAdapter1(whichGraphicsCardAdapter);
            }
            catch (SharpDXException ex)
            {
                throw new DesktopDuplicationException("Could not find the specified graphics card adapter.", ex);
            }
            _device = new Device(adapter);
            Output output;
            try
            {
                output = adapter.GetOutput(whichOutputDevice);
            }
            catch (SharpDXException ex)
            {
                throw new DesktopDuplicationException("Could not find the specified output device.", ex);
            }
            var output1 = output.QueryInterface<Output1>();
            _outputDescription = output.Description;

            try
            {
                _outputDuplication = output1.DuplicateOutput(_device);
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
        public Bitmap GetLatestFrame(Bitmap reusableImage)
        {
            // Try to get the latest frame; this may timeout
            bool succeeded = RetrieveFrame();
            if (!succeeded)
                return null;
            try
            {
                return ProcessFrame(reusableImage);
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
        }

        private bool RetrieveFrame()
        {
            if (_desktopImageTexture == null)
            {
                _desktopImageTexture = new Texture2D(_device, new Texture2DDescription()
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = _outputDescription.DesktopBounds.GetWidth(),
                    Height = _outputDescription.DesktopBounds.GetHeight(),
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging
                });
            }
            SharpDX.DXGI.Resource desktopResource;
            _frameInfo = new OutputDuplicateFrameInformation();
            try
            {
                if (_outputDuplication == null) throw new Exception("_outputDuplication is null");
                _outputDuplication.AcquireNextFrame(500, out _frameInfo, out desktopResource);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                {
                    return false;
                }

                throw new DesktopDuplicationException("Failed to acquire next frame.", ex);
            }

            if (desktopResource == null) throw new Exception("desktopResource is null");
            using (var tempTexture = desktopResource.QueryInterface<Texture2D>())
            {
                if (_device == null) throw new Exception("_device is null");
                if (_device.ImmediateContext == null) throw new Exception("_device.ImmediateContext is null");

                _device.ImmediateContext.CopyResource(tempTexture, _desktopImageTexture);
            }
            desktopResource.Dispose();
            return true;
        }

        private Bitmap ProcessFrame(Bitmap reusableImage)
        {
            // Get the desktop capture texture
            var mapSource = _device.ImmediateContext.MapSubresource(_desktopImageTexture, 0, MapMode.Read, MapFlags.None);
            
            Bitmap image;
            var width = _outputDescription.DesktopBounds.GetWidth();
            var height = _outputDescription.DesktopBounds.GetHeight();

            if (reusableImage!=null && reusableImage.Width==width && reusableImage.Height==height)
            {
                image = reusableImage;
            }
            else
            {
                image = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            }

            var boundsRect = new System.Drawing.Rectangle(0, 0, width, height);

            // Copy pixels from screen capture Texture to GDI bitmap
            var mapDest = image.LockBits(boundsRect, ImageLockMode.WriteOnly, image.PixelFormat);
            var sourcePtr = mapSource.DataPointer;
            var destPtr = mapDest.Scan0;

            Utilities.CopyMemory(destPtr, sourcePtr, height * width * 4);


          
            // Release source and dest locks
            image.UnlockBits(mapDest);
            _device.ImmediateContext.UnmapSubresource(_desktopImageTexture, 0);
            return image;
        }

        private void ReleaseFrame()
        {
            try
            {
                _outputDuplication.ReleaseFrame();
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Failure)
                {
                    throw new DesktopDuplicationException("Failed to release frame.");
                }
            }
        }

        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            IsDisposed = true;
            _desktopImageTexture?.Dispose();
            _outputDuplication?.Dispose();
            _device?.Dispose();
        }
    }
}
