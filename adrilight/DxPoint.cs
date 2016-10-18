/* See the file "LICENSE" for the full license governing this code. */

namespace Bambilight {

    class DxPoint {

        public DxPoint(int x, int y) {
            X = x;
            Y = y;
            DxPos = getPos(x, y);
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public long DxPos { get; private set; }

        private static long getPos(int x, int y) {
            long pos = (y * Program.ScreenWidth + x);
            return (pos <= Program.ScreenPixel) ? (pos * 4) : -1;
        }
    }
}
