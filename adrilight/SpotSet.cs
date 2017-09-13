using NLog;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace adrilight
{
    internal sealed class SpotSet : ISpotSet
    {
        private ILogger _log = LogManager.GetCurrentClassLogger();

        public SpotSet(IUserSettings userSettings)
        {
            UserSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));


            UserSettings.PropertyChanged += (_, __) => Refresh();
            Refresh();

            _log.Info($"SpotSet created.");
        }

        public ISpot[] Spots { get; set; }

        public object Lock => @lock;
        private readonly object @lock = new object();

        /// <summary>
        /// returns the number of leds
        /// </summary>
        public int CountLeds(int spotsX, int spotsY)
        {
            if (spotsX <= 1 || spotsY <= 1)
            {
                //special case because it is not really a rectangle of lights but a single light or a line of lights
                return spotsX*spotsY;
            }

            //normal case
            return 2*spotsX + 2*spotsY - 4;
        }

        public Rectangle ExpectedScreenBound { get; private set; }
        private IUserSettings UserSettings { get; }


        private void Refresh()
        {
            lock (Lock)
            {
                Spots = new Spot[CountLeds(UserSettings.SpotsX, UserSettings.SpotsY)];

                var rectangle = ExpectedScreenBound = Screen.PrimaryScreen.Bounds;

                var canvasSizeX = (rectangle.Width - 2* UserSettings.BorderDistanceX);
                var screenHeight = rectangle.Height;
                var canvasSizeY = (screenHeight - 2* UserSettings.BorderDistanceY);

                var xResolution = UserSettings.SpotsX > 1 ? (canvasSizeX - UserSettings.SpotWidth)/(UserSettings.SpotsX - 1) : 0;
                var xRemainingOffset = UserSettings.SpotsX > 1 ? ((canvasSizeX - UserSettings.SpotWidth)%(UserSettings.SpotsX - 1))/2 : 0;
                var yResolution = UserSettings.SpotsY > 1 ? (canvasSizeY - UserSettings.SpotHeight)/(UserSettings.SpotsY - 1) : 0;
                var yRemainingOffset = UserSettings.SpotsY > 1 ? ((canvasSizeY - UserSettings.SpotHeight)%(UserSettings.SpotsY - 1))/2 : 0;

                var counter = 0;
                var relationIndex = UserSettings.SpotsX - UserSettings.SpotsY + 1;

                for (var j = 0; j < UserSettings.SpotsY; j++)
                {
                    for (var i = 0; i < UserSettings.SpotsX; i++)
                    {
                        var isFirstColumn = i == 0;
                        var isLastColumn = i == UserSettings.SpotsX - 1;
                        var isFirstRow = j == 0;
                        var isLastRow = j == UserSettings.SpotsY - 1;

                        if (isFirstColumn || isLastColumn || isFirstRow || isLastRow) // needing only outer spots
                        {
                            var x = Math.Max(0,
                                Math.Min(rectangle.Width,
                                    xRemainingOffset + UserSettings.BorderDistanceX + UserSettings.OffsetX + i*(xResolution) + UserSettings.SpotWidth/2));
                            var y = Math.Max(0,
                                Math.Min(screenHeight, yRemainingOffset + UserSettings.BorderDistanceY + UserSettings.OffsetY + j*(yResolution) + UserSettings.SpotHeight/2));

                            var index = counter++; // in first row index is always counter

                            if (UserSettings.SpotsX > 1 && UserSettings.SpotsY > 1)
                            {
                                if (!isFirstRow && !isLastRow)
                                {
                                    if (isFirstColumn)
                                    {
                                        index += relationIndex + ((UserSettings.SpotsY - 1 - j)*3);
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
                            var spotWidth = Math.Min(UserSettings.SpotWidth, Math.Min(x, rectangle.Width - x));
                            var spotHeight = Math.Min(UserSettings.SpotHeight, Math.Min(y, screenHeight - y));
                            Spots[index] = new Spot(x, y, spotWidth, spotHeight);
                        }
                    }
                }


                if (UserSettings.OffsetLed != 0) Offset(UserSettings.OffsetLed);
                if (UserSettings.SpotsY > 1 && UserSettings.MirrorX) MirrorX();
                if (UserSettings.SpotsX > 1 && UserSettings.MirrorY) MirrorY();
            }
        }

        private void Mirror(int startIndex, int length)
        {
            var halfLength = (length/2);
            var endIndex = startIndex + length - 1;

            for (var i = 0; i < halfLength; i++)
            {
                Swap(startIndex + i, endIndex - i);
            }
        }

        private void Swap(int index1, int index2)
        {
            var temp = Spots[index1];
            Spots[index1] = Spots[index2];
            Spots[index2] = temp;
        }

        private void MirrorX()
        {
            // copy swap last row to first row inverse
            for (var i = 0; i < UserSettings.SpotsX; i++)
            {
                var index1 = i;
                var index2 = (Spots.Length - 1) - (UserSettings.SpotsY - 2) - i;
                Swap(index1, index2);
            }

            // mirror first column
            Mirror(UserSettings.SpotsX, UserSettings.SpotsY - 2);

            // mirror last column
            if (UserSettings.SpotsX > 1)
                Mirror(2* UserSettings.SpotsX + UserSettings.SpotsY - 2, UserSettings.SpotsY - 2);
        }

        private void MirrorY()
        {
            // copy swap last row to first row inverse
            for (var i = 0; i < UserSettings.SpotsY - 2; i++)
            {
                var index1 = UserSettings.SpotsX + i;
                var index2 = (Spots.Length - 1) - i;
                Swap(index1, index2);
            }

            // mirror first row
            Mirror(0, UserSettings.SpotsX);

            // mirror last row
            if (UserSettings.SpotsY > 1)
                Mirror(UserSettings.SpotsX + UserSettings.SpotsY - 2, UserSettings.SpotsX);
        }

        private void Offset(int offset)
        {
            ISpot[] temp = new Spot[Spots.Length];
            for (var i = 0; i < Spots.Length; i++)
            {
                temp[(i + temp.Length + offset)%temp.Length] = Spots[i];
            }
            Spots = temp;
        }

        public void IndicateMissingValues()
        {
            foreach (var spot in Spots)
            {
                spot.IndicateMissingValue();
            }
        }
    }


}
