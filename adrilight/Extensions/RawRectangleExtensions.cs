using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rectangle = SharpDX.Mathematics.Interop.RawRectangle;

namespace adrilight.Extensions
{
    static class RawRectangleExtensions
    {
        public static int GetWidth(this Rectangle rect)
        {
            return rect.Right - rect.Left;
        }
        public static int GetHeight(this Rectangle rect)
        {
            return rect.Bottom - rect.Top;
        }
    }
}
