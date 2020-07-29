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
using adrilight.Util;

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

        private Texture2D _stagingTexture;
        private Texture2D _smallerTexture;
        private ShaderResourceView _smallerTextureView;

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

        private static readonly FpsLogger _desktopFrameLogger = new FpsLogger("DesktopDuplication");


        /// <summary>
        /// Retrieves the latest desktop image and associated metadata.
        /// </summary>
        public Bitmap GetLatestFrame(Bitmap reusableImage)
        {
            // Try to get the latest frame; this may timeout
            var succeeded = RetrieveFrame();
            if (!succeeded)
                return null;

            _desktopFrameLogger.TrackSingleFrame();

            return ProcessFrame(reusableImage);

        }

        private const int mipMapLevel = 3;
        private const int scalingFactor = 1 << mipMapLevel;

        private bool RetrieveFrame()
        {

            var desktopWidth = _outputDescription.DesktopBounds.GetWidth();
            var desktopHeight = _outputDescription.DesktopBounds.GetHeight();

            if (_stagingTexture == null)
            {
                _stagingTexture = new Texture2D(_device, new Texture2DDescription()
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = desktopWidth / scalingFactor,
                    Height = desktopHeight / scalingFactor,
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging // << can be read by CPU
                });
            }
            SharpDX.DXGI.Resource desktopResource;
            try
            {
                if (_outputDuplication == null) throw new Exception("_outputDuplication is null");
                _outputDuplication.TryAcquireNextFrame(500, out var frameInformation, out desktopResource); //todo TRY semantic!!
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

            if (_smallerTexture == null)
            {
                _smallerTexture = new Texture2D(_device, new Texture2DDescription {
                    CpuAccessFlags = CpuAccessFlags.None,
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = desktopWidth,
                    Height = desktopHeight,
                    OptionFlags = ResourceOptionFlags.GenerateMipMaps,
                    MipLevels = mipMapLevel + 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Default
                });
                _smallerTextureView = new ShaderResourceView(_device, _smallerTexture);
            }


            using (var tempTexture = desktopResource.QueryInterface<Texture2D>())
            {
                if (_device == null) throw new Exception("_device is null");
                if (_device.ImmediateContext == null) throw new Exception("_device.ImmediateContext is null");

                _device.ImmediateContext.CopySubresourceRegion(tempTexture, 0, null, _smallerTexture, 0);
            }
            _outputDuplication.ReleaseFrame();

            // Generates the mipmap of the screen
            _device.ImmediateContext.GenerateMips(_smallerTextureView);

            // Copy the mipmap 1 of smallerTexture (size/2) to the staging texture
            _device.ImmediateContext.CopySubresourceRegion(_smallerTexture, mipMapLevel, null, _stagingTexture, 0);

            desktopResource.Dispose(); //perf?
            return true;
        }

        private Bitmap ProcessFrame(Bitmap reusableImage)
        {
            // Get the desktop capture texture
            var mapSource = _device.ImmediateContext.MapSubresource(_stagingTexture, 0, MapMode.Read, MapFlags.None);
            
            Bitmap image;
            var width = _outputDescription.DesktopBounds.GetWidth() / scalingFactor;
            var height = _outputDescription.DesktopBounds.GetHeight() / scalingFactor;

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

            if (mapSource.RowPitch == mapDest.Stride)
            {
                //fast copy
                Utilities.CopyMemory(destPtr, sourcePtr, height * mapDest.Stride);
            }
            else
            {
                //safe copy
                for (int y = 0; y < height; y++)
                {
                    // Copy a single line 
                    Utilities.CopyMemory(destPtr, sourcePtr, width * 4);

                    // Advance pointers
                    sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                    destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                }
            }

            // Release source and dest locks
            image.UnlockBits(mapDest);
            _device.ImmediateContext.UnmapSubresource(_stagingTexture, 0);
            return image;
        }


        public bool IsDisposed { get; private set; }

        public static int ScalingFactor => scalingFactor;

        public void Dispose()
        {
            IsDisposed = true;
            _smallerTexture?.Dispose();
            _smallerTextureView?.Dispose();
            _stagingTexture?.Dispose();
            _outputDuplication?.Dispose();
            _device?.Dispose();

            GC.Collect();
        }
    }
}
