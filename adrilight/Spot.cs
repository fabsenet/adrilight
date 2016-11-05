

using System;
using System.Diagnostics;
using System.Drawing;

namespace adrilight {

    [DebuggerDisplay("Spot: TopLeft={TopLeft}, BottomRight={BottomRight}, Color={Red},{Green},{Blue}")]
    sealed class Spot : IDisposable {

        public Spot(int x, int y, int aWidth, int aHeight) : this(new DxPoint(x, y), aWidth, aHeight) { }

        public Spot(DxPoint aCenter, int aWidth, int aHeight) {
            Width = aWidth;
            Height = aHeight;

            int distanceX = aWidth / 2;
            int distanceY = aHeight / 2;
            TopLeft = new DxPoint(aCenter.X - distanceX, aCenter.Y - distanceY);
            BottomRight = new DxPoint(aCenter.X + distanceX, aCenter.Y + distanceY);

            Rectangle = new Rectangle(TopLeft.X, TopLeft.Y, Width, Height);
            RectangleOverlayBorder = new Rectangle(TopLeft.X + 2, TopLeft.Y + 2, Width - 4, Height - 4);
            RectangleOverlayFilling = new Rectangle(TopLeft.X + 4, TopLeft.Y + 4, Width - 8, Height - 8);

            Brush = new SolidBrush(Color.Black);
        }


        public int Width { get; private set; }
        public int Height { get; private set; }

        public DxPoint TopLeft { get; private set; }
        public DxPoint BottomRight { get; private set; }

        public Rectangle Rectangle { get; private set; }
        public Rectangle RectangleOverlayBorder { get; private set; }
        public Rectangle RectangleOverlayFilling { get; private set; }

        private SolidBrush Brush { get; set; }

        public SolidBrush OnDemandBrush
        {
            get
            {
                 Brush.Color = Color.FromArgb(Red, Green, Blue);
                 return Brush;
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
        }

        public void Dispose() {
            Brush?.Dispose();
        }
    }
}
