/* See the file "LICENSE" for the full license governing this code. */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopDuplication;

namespace Bambilight
{
    class DesktopDuplicatorReader
    {
        private static DesktopDuplicator _desktopDuplicator;
        public bool IsRunning { get; private set; } = false;

        public void Run(CancellationToken token)
        {
            if(IsRunning) throw new Exception(nameof(DesktopDuplicatorReader) +" is already running!");

            IsRunning = true;
            try
            {
                if (_desktopDuplicator == null)
                {
                    _desktopDuplicator = new DesktopDuplicator(0);
                }
                var desktopDuplicator = _desktopDuplicator; //main window
                while (!token.IsCancellationRequested)
                {
                        Thread.Sleep(16);
                    var frame = desktopDuplicator.GetLatestFrame();
                    if (frame == null)
                    {
                        continue;
                    }
                    lock (SpotSet.Lock)
                    {
                        foreach (Spot spot in SpotSet.Spots)
                        {
                            if (spot.TopLeft.DxPos >= 0
                                && spot.TopRight.DxPos >= 0
                                && spot.BottomLeft.DxPos >= 0
                                && spot.BottomRight.DxPos >= 0)
                            {
                                var minX = spot.TopLeft.X;
                                var minY = spot.TopLeft.Y;
                                var dx = spot.Width;
                                var dy = spot.Height;

                                int count = 0;
                                int r = 0, g = 0, b = 0;
                                for (int x = 0; x < dx; x+=4)
                                {
                                    for (int y = 0; y < dy; y+=4)
                                    {
                                        var color = frame.DesktopImage.GetPixel(minX + x, minY + y);
                                        r += color.R;
                                        g += color.G;
                                        b += color.B;
                                        count++;
                                    }
                                }
//                            White balance    1,0000  0,8730  0,7453

                                var avgColor = Color.FromArgb((int) (r*1.0f/count), (int) (g* 0.8730f/ count), (int) (b* 0.7453f/ count));
                                spot.SetColor(avgColor);

                                //dataStream.Position = spot.TopLeft.DxPos;
                                //dataStream.Read(mColorBufferTopLeft, 0, 4);

                                //dataStream.Position = spot.TopRight.DxPos;
                                //dataStream.Read(mColorBufferTopRight, 0, 4);

                                //dataStream.Position = spot.Center.DxPos;
                                //dataStream.Read(mColorBufferCenter, 0, 4);

                                //dataStream.Position = spot.BottomLeft.DxPos;
                                //dataStream.Read(mColorBufferBottomLeft, 0, 4);

                                //dataStream.Position = spot.BottomRight.DxPos;
                                //dataStream.Read(mColorBufferBottomRight, 0, 4);

                                //averageValues();

                                //if (mColorBuffer[0] <= Settings.SaturationTreshold) { mColorBuffer[0] = 0x00; } //blue
                                //if (mColorBuffer[1] <= Settings.SaturationTreshold) { mColorBuffer[1] = 0x00; } // green
                                //if (mColorBuffer[2] <= Settings.SaturationTreshold) { mColorBuffer[2] = 0x00; } // red

                                //spot.SetColor(mColorBuffer[2] /* red */, mColorBuffer[1] /* green */, mColorBuffer[0] /* blue */);
                            }
                        }
                    }
                }
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}


//using System;
//using System.ComponentModel;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Bambilight {

//    public class DxScreenCapture : IDisposable {

//        private const int COLORS_PER_LED = 3;
//        private const int DATASTREAM_BYTES_PER_PIXEL = 4;

//        private Task mBackgroundWorker = null;
//        private CancellationTokenSource _tokenSource = null;
//        private byte[] mColorBufferTopLeft = new byte[DATASTREAM_BYTES_PER_PIXEL];
//        private byte[] mColorBufferTopRight = new byte[DATASTREAM_BYTES_PER_PIXEL];
//        private byte[] mColorBufferCenter = new byte[DATASTREAM_BYTES_PER_PIXEL];
//        private byte[] mColorBufferBottomLeft = new byte[DATASTREAM_BYTES_PER_PIXEL];
//        private byte[] mColorBufferBottomRight = new byte[DATASTREAM_BYTES_PER_PIXEL];
//        private int[] mColorBuffer = new int[COLORS_PER_LED];

//        public DxScreenCapture() {
//        }

//        public void Start() {
//            if (_tokenSource == null) {
//                _tokenSource = new CancellationTokenSource();
//                mBackgroundWorker = Task.Run(() => mBackgroundWorker_DoWork(_tokenSource.Token));
//            }
//        }

//        public void Stop() {
//            if (_tokenSource != null) {
//                _tokenSource.Cancel();
//                mBackgroundWorker = null;
//                _tokenSource = null;
//            }
//        }

//        private void mBackgroundWorker_DoWork(CancellationToken token) {

//            Direct3D direct3D = new Direct3D();

//            PresentParameters present_params = new PresentParameters();
//            present_params.Windowed = true;
//            present_params.SwapEffect = SwapEffect.Discard;
//            Device device = new Device(direct3D, 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.SoftwareVertexProcessing, present_params);

//            while (!token.IsCancellationRequested) {

//                Surface surface = Surface.CreateOffscreenPlain(device,
//                    Screen.PrimaryScreen.Bounds.Width,
//                    Screen.PrimaryScreen.Bounds.Height,
//                    Format.A8R8G8B8,
//                    Pool.Scratch);

//                device.GetFrontBufferData(0, surface);

//                DataRectangle dataRectangle = surface.LockRectangle(LockFlags.None);
//                DataStream dataStream = dataRectangle.Data;

//                lock (SpotSet.Lock) {
//                    foreach (Spot spot in SpotSet.Spots) {
//                        if (spot.TopLeft.DxPos >= 0 
//                            && spot.TopRight.DxPos >= 0 
//                            && spot.BottomLeft.DxPos >= 0 
//                            && spot.BottomRight.DxPos >= 0) {

//                            dataStream.Position = spot.TopLeft.DxPos;
//                            dataStream.Read(mColorBufferTopLeft, 0, 4);

//                            dataStream.Position = spot.TopRight.DxPos;
//                            dataStream.Read(mColorBufferTopRight, 0, 4);

//                            dataStream.Position = spot.Center.DxPos;
//                            dataStream.Read(mColorBufferCenter, 0, 4);

//                            dataStream.Position = spot.BottomLeft.DxPos;
//                            dataStream.Read(mColorBufferBottomLeft, 0, 4);

//                            dataStream.Position = spot.BottomRight.DxPos;
//                            dataStream.Read(mColorBufferBottomRight, 0, 4);

//                            averageValues();

//                            if (mColorBuffer[0] <= Settings.SaturationTreshold) { mColorBuffer[0] = 0x00; } //blue
//                            if (mColorBuffer[1] <= Settings.SaturationTreshold) { mColorBuffer[1] = 0x00; } // green
//                            if (mColorBuffer[2] <= Settings.SaturationTreshold) { mColorBuffer[2] = 0x00; } // red

//                            spot.SetColor(mColorBuffer[2] /* red */, mColorBuffer[1] /* green */, mColorBuffer[0] /* blue */);
//                        }
//                    }
//                }

//                surface.UnlockRectangle();
//                surface.Dispose();

//            }

//            device.Dispose();
//            direct3D.Dispose();
//        }

//        private void averageValues() {
//            for (int i = 0; i < COLORS_PER_LED; i++) {
//                int temp = (int)(mColorBufferTopLeft[i] + mColorBufferTopRight[i] + mColorBufferCenter[i] + mColorBufferBottomLeft[i] + mColorBufferBottomRight[i]) / 5;
//                mColorBuffer[i] = (byte)temp;
//            }
//        }

//        public void Dispose() {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing) {
//            if (disposing) {
//                Stop();

//                mBackgroundWorker.Dispose();
//            }
//        }
//    }
//}
