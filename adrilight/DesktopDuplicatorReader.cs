using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using adrilight.DesktopDuplication;
using NLog;
using Polly;

namespace adrilight
{
    public class DesktopDuplicatorReader
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public bool IsRunning { get; private set; } = false;

        private readonly Policy _retryPolicy = Policy.Handle<Exception>().
            WaitAndRetryForever(ProvideDelayDuration);

        private static TimeSpan ProvideDelayDuration(int index)
        {
            if (index < 10)
            {
                //first second
                return TimeSpan.FromMilliseconds(100);
            }

            if (index < 10 + 256)
            {
                //steps where there is also led dimming
                SpotSet.IndicateMissingValues();
                return TimeSpan.FromMilliseconds(5000d/256);
            }
            return TimeSpan.FromMilliseconds(1000);
        }

        private DesktopDuplicator _desktopDuplicator;

        public void Run(CancellationToken token)
        {
            if (IsRunning) throw new Exception(nameof(DesktopDuplicatorReader) + " is already running!");

            IsRunning = true;
            Debug.WriteLine("Started Desktop Duplication Reader.");
            try
            {
                BitmapData bitmapData = new BitmapData();

                while (!token.IsCancellationRequested)
                {

                    var frame = _retryPolicy.Execute<DesktopFrame>(GetNextFrame);
                    TraceFrameDetails(frame);
                    if (frame == null)
                    {
                        //there was a timeout before there was the next frame, simply retry!
                        continue;
                    }


                    var image = frame.DesktopImage;
                    image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb, bitmapData);
                    lock (SpotSet.Lock)
                    {
                        var useLinearLighting = Settings.UseLinearLighting;

                        var imageRectangle = new Rectangle(0, 0, image.Width, image.Height);
                        Parallel.ForEach(SpotSet.Spots
                            , spot =>
                            {
                                const int numberOfSteps = 15;
                                int stepx = Math.Max(1, spot.Rectangle.Width/numberOfSteps);
                                int stepy = Math.Max(1, spot.Rectangle.Height/numberOfSteps);

                                int sumR;
                                int sumG;
                                int sumB;
                                int count;
                                if (imageRectangle.Width != SpotSet.ExpectedScreenBound.Width || imageRectangle.Height != SpotSet.ExpectedScreenBound.Height)
                                {
                                    //the screen was resized or this is some kind of powersaving state
                                    SpotSet.IndicateMissingValues();
                                    return;
                                }
                                GetAverageColorOfRectangularRegion(spot.Rectangle, stepy, stepx, bitmapData, out sumR, out sumG, out sumB, out count);

                                var countInverse = 1f/count;
                                byte finalR, finalG, finalB;
                                ApplyColorCorrections(sumR*countInverse, sumG*countInverse, sumB*countInverse, out finalR, out finalG, out finalB, useLinearLighting);

                                spot.SetColor(finalR, finalG, finalB);

                            });
                    }
                    image.UnlockBits(bitmapData);
                }
            }
            finally
            {
                _desktopDuplicator?.Dispose();
                _desktopDuplicator = null;

                Debug.WriteLine("Stopped Desktop Duplication Reader.");
                IsRunning = false;
            }
        }

        private DateTime? _lastNotNullFrameDateTime;
        private DateTime? _lastNotNullFrameLoggedDateTime;
        private bool _framesAreNullMessagePrinted;
        private int? _lastObservedHeight;
        private int? _lastObservedWidth;

        private void TraceFrameDetails(DesktopFrame frame)
        {
            //there are many frames per second and we need to extract useful information and only log those!
            if (frame == null)
            {
                //if the frame is null, this can mean two things. the timeout from the desktop duplication api was reached
                //before the monitor content changed or there was some other error.
                if (!_lastNotNullFrameDateTime.HasValue) _lastNotNullFrameDateTime = DateTime.UtcNow;

                if (!_lastNotNullFrameLoggedDateTime.HasValue 
                    || DateTime.UtcNow - _lastNotNullFrameLoggedDateTime.Value > TimeSpan.FromSeconds(30))
                {
                    _lastNotNullFrameLoggedDateTime = DateTime.UtcNow;
                    _log.Debug("The frames are null since {0}", _lastNotNullFrameDateTime);
                    _framesAreNullMessagePrinted = true;
                }
            }
            else
            {
                if (_lastNotNullFrameDateTime.HasValue)
                {
                    if (_framesAreNullMessagePrinted)
                    {
                        _log.Debug("There is again a frame which is not null!");
                        _framesAreNullMessagePrinted = false;
                    }
                    _lastNotNullFrameDateTime = null;
                }

                if (_lastObservedHeight == null || _lastObservedWidth == null
                    || _lastObservedHeight != frame.DesktopImage.Height
                    || _lastObservedWidth != frame.DesktopImage.Width)
                {
                    _log.Debug("The frame size changed from {0}x{1} to {2}x{3}"
                        , _lastObservedWidth, _lastObservedHeight
                        , frame.DesktopImage.Width, frame.DesktopImage.Height);

                    _lastObservedWidth = frame.DesktopImage.Width;
                    _lastObservedHeight = frame.DesktopImage.Height;
                }
            }

        }

        private void ApplyColorCorrections(float r, float g, float b, out byte finalR, out byte finalG, out byte finalB, bool useLinearLighting)
        {
            if (r <= Settings.SaturationTreshold && g <= Settings.SaturationTreshold && b <= Settings.SaturationTreshold)
            {
                //black
                finalR = finalG = finalB = 0;
                return;
            }

            if (!useLinearLighting)
            {
                //apply non linear LED fading ( http://www.mikrocontroller.net/articles/LED-Fading )
                r = FadeNonLinear(r);
                g = FadeNonLinear(g);
                b = FadeNonLinear(b);
            }

            //white balance
            //todo: introduce settings for white balance adjustments
            r = 1f*r;
            g = 0.8730f*g;
            b = 0.7453f*b;

            //output
            finalR = (byte) r;
            finalG = (byte) g;
            finalB = (byte) b;
        }

        private float FadeNonLinear(float color)
        {
            const float factor = 80f;
            //todo: cache values, return directly byte!
            return 256f*((float) Math.Pow(factor, color/256f) - 1f)/(factor - 1);
        }


        private DesktopFrame GetNextFrame()
        {
            if (_desktopDuplicator == null)
            {
                _desktopDuplicator = new DesktopDuplicator(0, 0);
            }

            try
            {
                var frame = _desktopDuplicator.GetLatestFrame();
                return frame;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "GetNextFrame() failed.");

                _desktopDuplicator?.Dispose();
                _desktopDuplicator = null;
                throw;
            }
        }

        public static unsafe void GetAverageColorOfRectangularRegion(Rectangle spotRectangle, int stepy, int stepx, BitmapData bitmapData, out int sumR, out int sumG,
            out int sumB, out int count)
        {
            sumR = 0;
            sumG = 0;
            sumB = 0;
            count = 0;

            var stepCount = spotRectangle.Width/stepx;
            var stepxTimes4 = stepx*4;
            for (var y = spotRectangle.Top; y < spotRectangle.Bottom; y += stepy)
            {
                byte* pointer = (byte*) bitmapData.Scan0 + bitmapData.Stride*y + 4*spotRectangle.Left;
                for (int i = 0; i < stepCount; i++)
                {
                    sumR += pointer[2];
                    sumG += pointer[1];
                    sumB += pointer[0];

                    pointer += stepxTimes4;
                }
                count += stepCount;
            }
        }

    }
}
