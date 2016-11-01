using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace adrilight.benchmarks
{
    public class GetpixelsBenchmarks :IDisposable
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<GetpixelsBenchmarks>();
        }

        private readonly BitmapData _bitmapData;
        private Bitmap _image;

        public GetpixelsBenchmarks()
        {
            _image = (Bitmap) Image.FromFile("sample.jpg");
            _bitmapData = new BitmapData();
            _image.LockBits(new Rectangle(0, 0, _image.Width, _image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb, _bitmapData);
        }

        [Benchmark]
        public void GetAverageColorOfRectangularRegionBenchmark()
        {
            int r, g, b, count;
            GetAverageColorOfRectangularRegion(new Rectangle(20, 30, 100, 100), 10, 10, _bitmapData, out r, out g, out b, out count);
        }

        [Benchmark]
        public void GetAverageColorOfRectangularRegionInlinedBenchmark()
        {
            int r, g, b, count;
            GetAverageColorOfRectangularRegion2(new Rectangle(20, 30, 100, 100), 10, 10, _bitmapData, out r, out g, out b, out count);
        }

        [Benchmark]
        public void GetAverageColorOfRectangularRegionInlinedReducedLocalVarsBenchmark()
        {
            int r, g, b, count;
            GetAverageColorOfRectangularRegion2_5(new Rectangle(20, 30, 100, 100), 10, 10, _bitmapData, out r, out g, out b, out count);
        }
        [Benchmark]
        public void GetAverageColorOfRectangularRegionRunningPointerBenchmark()
        {
            int r, g, b, count;
            GetAverageColorOfRectangularRegion3(new Rectangle(20, 30, 100, 100), 10, 10, _bitmapData, out r, out g, out b, out count);
        }

        public void Dispose()
        {
            _image?.Dispose();
            _image = null;
        }



        public static void GetAverageColorOfRectangularRegion(Rectangle spotRectangle, int stepy, int stepx, BitmapData bitmapData, out int sumR, out int sumG, out int sumB, out int count)
        {
            sumR = 0;
            sumG = 0;
            sumB = 0;
            count = 0;
            for (var y = spotRectangle.Top; y < spotRectangle.Bottom; y += stepy)
            {
                for (var x = spotRectangle.Left; x < spotRectangle.Right; x += stepx)
                {
                    byte r;
                    byte g;
                    byte b;

                    GetColor(bitmapData, y, x, out r, out g, out b);

                    sumR += r;
                    sumG += g;
                    sumB += b;
                    count++;
                }
            }
        }

        public unsafe static void GetAverageColorOfRectangularRegion2(Rectangle spotRectangle, int stepy, int stepx, BitmapData bitmapData, out int sumR, out int sumG, out int sumB, out int count)
        {
            sumR = 0;
            sumG = 0;
            sumB = 0;
            count = 0;
            for (var y = spotRectangle.Top; y < spotRectangle.Bottom; y += stepy)
            {
                for (var x = spotRectangle.Left; x < spotRectangle.Right; x += stepx)
                {
                    byte r;
                    byte g;
                    byte b;

                    byte* pointer = (byte*)bitmapData.Scan0 + bitmapData.Stride * y;

                    var offsetX = x * 4;

                    r = pointer[offsetX + 2];
                    g = pointer[offsetX + 1];
                    b = pointer[offsetX + 0];

                    sumR += r;
                    sumG += g;
                    sumB += b;
                    count++;
                }
            }
        }
        public unsafe static void GetAverageColorOfRectangularRegion2_5(Rectangle spotRectangle, int stepy, int stepx, BitmapData bitmapData, out int sumR, out int sumG, out int sumB, out int count)
        {
            sumR = 0;
            sumG = 0;
            sumB = 0;
            count = 0;
            for (var y = spotRectangle.Top; y < spotRectangle.Bottom; y += stepy)
            {
                for (var x = spotRectangle.Left; x < spotRectangle.Right; x += stepx)
                {
                    byte* pointer = (byte*)bitmapData.Scan0 + bitmapData.Stride * y;
                    var offsetX = x * 4;

                    sumR += pointer[offsetX + 2];
                    sumG += pointer[offsetX + 1];
                    sumB += pointer[offsetX + 0];
                    count++;
                }
            }
        }
        public unsafe static void GetAverageColorOfRectangularRegion3(Rectangle spotRectangle, int stepy, int stepx, BitmapData bitmapData, out int sumR, out int sumG, out int sumB, out int count)
        {
            sumR = 0;
            sumG = 0;
            sumB = 0;
            count = 0;

            var stepCount = spotRectangle.Width/stepx;
            var stepxTimes4 = stepx*4;
            for (var y = spotRectangle.Top; y < spotRectangle.Bottom; y += stepy)
            {
                byte* pointer = (byte*)bitmapData.Scan0 + bitmapData.Stride * y + 4*spotRectangle.Left;
                for (int i = 0; i < stepCount; i++)
                {
                    sumR += pointer[2];
                    sumG += pointer[1];
                    sumB += pointer[0];

                    pointer += stepxTimes4;
                }
                    count+=stepCount;
            }
        }

        private static unsafe void GetColor(BitmapData bitmapData, int py, int px, out byte r, out byte g, out byte b)
        {
            Debug.Assert(py >= 0 && py < bitmapData.Height);
            Debug.Assert(px >= 0 && px < bitmapData.Width);

            byte* pointer = (byte*)bitmapData.Scan0 + bitmapData.Stride * (py);

            var offsetX = (px) * 4;

            r = pointer[offsetX + 2];
            g = pointer[offsetX + 1];
            b = pointer[offsetX + 0];
        }
    }
}
