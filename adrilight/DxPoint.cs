

using System.Diagnostics;

namespace adrilight {

    [DebuggerDisplay("DxPoint {X}, {Y}")]
    class DxPoint {

        public DxPoint(int x, int y) {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

    }
}
