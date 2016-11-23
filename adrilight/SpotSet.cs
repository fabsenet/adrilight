using System;
using System.Drawing;
using System.Windows.Forms;

namespace adrilight
{
    internal static class SpotSet
    {

        public static Spot[] Spots { get; set; }
        public static readonly object Lock = new object();

        /// <summary>
        /// returns the number of leds
        /// </summary>
        public static int CountLeds(int spotsX, int spotsY)
        {
            if (spotsX <= 1 || spotsY <= 1)
            {
                //special case because it is not really a rectangle of lights but a single light or a line of lights
                return spotsX*spotsY;
            }

            //normal case
            return 2*spotsX + 2*spotsY - 4;
        }

        public static Rectangle ExpectedScreenBound { get; private set; }
        public static void Refresh()
        {
            lock (Lock)
            {
                Spots = new Spot[CountLeds(Settings.SpotsX, Settings.SpotsY)];

                var rectangle = ExpectedScreenBound = Screen.PrimaryScreen.Bounds;

                var canvasSizeX = (rectangle.Width - 2*Settings.BorderDistanceX);
                var screenHeight = rectangle.Height;
                var canvasSizeY = (screenHeight - 2*Settings.BorderDistanceY);

                var xResolution = Settings.SpotsX > 1 ? (canvasSizeX - Settings.SpotWidth)/(Settings.SpotsX - 1) : 0;
                var xRemainingOffset = Settings.SpotsX > 1 ? ((canvasSizeX - Settings.SpotWidth)%(Settings.SpotsX - 1))/2 : 0;
                var yResolution = Settings.SpotsY > 1 ? (canvasSizeY - Settings.SpotHeight)/(Settings.SpotsY - 1) : 0;
                var yRemainingOffset = Settings.SpotsY > 1 ? ((canvasSizeY - Settings.SpotHeight)%(Settings.SpotsY - 1))/2 : 0;

                var counter = 0;
                var relationIndex = Settings.SpotsX - Settings.SpotsY + 1;

                for (var j = 0; j < Settings.SpotsY; j++)
                {
                    for (var i = 0; i < Settings.SpotsX; i++)
                    {
                        var isFirstColumn = i == 0;
                        var isLastColumn = i == Settings.SpotsX - 1;
                        var isFirstRow = j == 0;
                        var isLastRow = j == Settings.SpotsY - 1;

                        if (isFirstColumn || isLastColumn || isFirstRow || isLastRow) // needing only outer spots
                        {
                            var x = Math.Max(0,
                                Math.Min(rectangle.Width,
                                    xRemainingOffset + Settings.BorderDistanceX + Settings.OffsetX + i*(xResolution) + Settings.SpotWidth/2));
                            var y = Math.Max(0,
                                Math.Min(screenHeight, yRemainingOffset + Settings.BorderDistanceY + Settings.OffsetY + j*(yResolution) + Settings.SpotHeight/2));

                            var index = counter++; // in first row index is always counter

                            if (Settings.SpotsX > 1 && Settings.SpotsY > 1)
                            {
                                if (!isFirstRow && !isLastRow)
                                {
                                    if (isFirstColumn)
                                    {
                                        index += relationIndex + ((Settings.SpotsY - 1 - j)*3);
                                    }
                                    else if (isLastColumn)
                                    {
                                        index -= j;
                                    }
                                }

                                if (!isFirstRow && isLastRow)
                                {
                                    index += relationIndex - (i*2);
                                }
                            }
                            var spotWidth = Math.Min(Settings.SpotWidth, Math.Min(x, rectangle.Width - x));
                            var spotHeight = Math.Min(Settings.SpotHeight, Math.Min(y, screenHeight - y));
                            SpotSet.Spots[index] = new Spot(x, y, spotWidth, spotHeight);
                        }
                    }
                }


                if (Settings.OffsetLed != 0) Offset(Settings.OffsetLed);
                if (Settings.SpotsY > 1 && Settings.MirrorX) MirrorX();
                if (Settings.SpotsX > 1 && Settings.MirrorY) MirrorY();
            }
        }

        private static void Mirror(int startIndex, int length)
        {
            var halfLength = (length/2);
            var endIndex = startIndex + length - 1;

            for (var i = 0; i < halfLength; i++)
            {
                Swap(startIndex + i, endIndex - i);
            }
        }

        private static void Swap(int index1, int index2)
        {
            var temp = Spots[index1];
            Spots[index1] = Spots[index2];
            Spots[index2] = temp;
        }

        private static void MirrorX()
        {
            // copy swap last row to first row inverse
            for (var i = 0; i < Settings.SpotsX; i++)
            {
                var index1 = i;
                var index2 = (Spots.Length - 1) - (Settings.SpotsY - 2) - i;
                Swap(index1, index2);
            }

            // mirror first column
            Mirror(Settings.SpotsX, Settings.SpotsY - 2);

            // mirror last column
            if (Settings.SpotsX > 1)
                Mirror(2*Settings.SpotsX + Settings.SpotsY - 2, Settings.SpotsY - 2);
        }

        private static void MirrorY()
        {
            // copy swap last row to first row inverse
            for (var i = 0; i < Settings.SpotsY - 2; i++)
            {
                var index1 = Settings.SpotsX + i;
                var index2 = (Spots.Length - 1) - i;
                Swap(index1, index2);
            }

            // mirror first row
            Mirror(0, Settings.SpotsX);

            // mirror last row
            if (Settings.SpotsY > 1)
                Mirror(Settings.SpotsX + Settings.SpotsY - 2, Settings.SpotsX);
        }

        private static void Offset(int offset)
        {
            var temp = new Spot[Spots.Length];
            for (var i = 0; i < Spots.Length; i++)
            {
                temp[(i + temp.Length + offset)%temp.Length] = Spots[i];
            }
            Spots = temp;
        }

        public static void IndicateMissingValues()
        {
            foreach (var spot in Spots)
            {
                spot.IndicateMissingValue();
            }
        }
    }


}
