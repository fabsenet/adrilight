/* See the file "LICENSE" for the full license governing this code. */

using System.Diagnostics;

namespace Bambilight {

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
