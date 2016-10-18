/* See the file "LICENSE" for the full license governing this code. */

using System;
using System.Drawing;

namespace Bambilight {

    class Spot : IDisposable {

        public Spot(int x, int y, int aWidth, int aHeight) : this(new DxPoint(x, y), aWidth, aHeight) { }

        public Spot(DxPoint aCenter, int aWidth, int aHeight) {
            Center = aCenter;
            Width = aWidth;
            Height = aHeight;

            int distanceX = aWidth / 2;
            int distanceY = aHeight / 2;
            TopLeft = new DxPoint(Center.X - distanceX, Center.Y - distanceY);
            TopRight = new DxPoint(Center.X + distanceX, Center.Y - distanceY);
            BottomLeft = new DxPoint(Center.X - distanceX, Center.Y + distanceY);
            BottomRight = new DxPoint(Center.X + distanceX, Center.Y + distanceY);

            Rectangle = new Rectangle(TopLeft.X, TopLeft.Y, Width, Height);
            RectangleOverlayBorder = new Rectangle(TopLeft.X + 2, TopLeft.Y + 2, Width - 4, Height - 4);
            RectangleOverlayFilling = new Rectangle(TopLeft.X + 4, TopLeft.Y + 4, Width - 8, Height - 8);

            Brush = new SolidBrush(Color.Black);
        }

        public DxPoint Center { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public DxPoint TopLeft { get; private set; }
        public DxPoint TopRight { get; private set; }
        public DxPoint BottomLeft { get; private set; }
        public DxPoint BottomRight { get; private set; }

        public Rectangle Rectangle { get; private set; }
        public Rectangle RectangleOverlayBorder { get; private set; }
        public Rectangle RectangleOverlayFilling { get; private set; }

        public SolidBrush Brush { get; private set; }

        public void SetColor(int red, int green, int blue) {
            Brush.Color = Color.FromArgb(red, green, blue);
        }

        public void Dispose() {
            Brush.Dispose();
        }
    }
}
