

using System;
using System.Diagnostics;
using System.Drawing;

namespace adrilight {

    [DebuggerDisplay("Spot: Rectangle={Rectangle}, Color={Red},{Green},{Blue}")]
    sealed class Spot : IDisposable {

        public Spot(int x, int y, int aWidth, int aHeight)
        {
            var topLeft = new DxPoint(x - aWidth/2, y - aHeight/2);
            Rectangle = new Rectangle(topLeft.X, topLeft.Y, aWidth, aHeight);

            RectangleOverlayBorder = new Rectangle(topLeft.X + 2, topLeft.Y + 2, aWidth - 4, aHeight - 4);
            RectangleOverlayFilling = new Rectangle(topLeft.X + 4, topLeft.Y + 4, aWidth - 8, aHeight - 8);
        }




        public Rectangle Rectangle { get; private set; }
        public Rectangle RectangleOverlayBorder { get; private set; }
        public Rectangle RectangleOverlayFilling { get; private set; }

        private readonly SolidBrush _brush = new SolidBrush(Color.Black);

        public SolidBrush OnDemandBrush
        {
            get
            {
                _brush.Color = Color.FromArgb(Red, Green, Blue);
                 return _brush;
            }
        }
        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }

        public void SetColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            _lastMissingValueIndication = null;
        }

        public void Dispose() {
            _brush?.Dispose();
        }

        private DateTime? _lastMissingValueIndication;
        private readonly double _dimToBlackIntervalInMs = TimeSpan.FromMilliseconds(10000).TotalMilliseconds;

        private float _dimR, _dimG, _dimB;

        public void IndicateMissingValue()
        {
            //trhis method might be called while another thread is calling setcolor() and we need the local copy to have a fixed value
            var localCopyLastMissingValueIndication = _lastMissingValueIndication;

            if (!localCopyLastMissingValueIndication.HasValue)
            {
                //a new period of missing values starts, copy last values
                _dimR = Red;
                _dimG = Green;
                _dimB = Blue;
                localCopyLastMissingValueIndication = _lastMissingValueIndication = DateTime.UtcNow;
            }

            var dimFactor =(float) (1 - (DateTime.UtcNow - localCopyLastMissingValueIndication.Value).TotalMilliseconds / _dimToBlackIntervalInMs);
            dimFactor = Math.Max(0, Math.Min(1, dimFactor));

            Red = (byte) (dimFactor*_dimR);
            Green = (byte) (dimFactor*_dimG);
            Blue = (byte) (dimFactor*_dimB);
        }
    }
}
