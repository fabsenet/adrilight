using adrilight.Extensions;
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


            UserSettings.PropertyChanged += (_, e) => DecideRefresh(e.PropertyName);
            Refresh();

            _log.Info($"SpotSet created.");
        }

        private void DecideRefresh(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(UserSettings.BorderDistanceX):
                case nameof(UserSettings.BorderDistanceY):
                case nameof(UserSettings.MirrorX):
                case nameof(UserSettings.MirrorY):
                case nameof(UserSettings.OffsetLed):
                case nameof(UserSettings.OffsetX):
                case nameof(UserSettings.OffsetY):
                case nameof(UserSettings.SpotHeight):
                case nameof(UserSettings.SpotsX):
                case nameof(UserSettings.SpotsY):
                case nameof(UserSettings.SpotWidth):
                    Refresh();
                    break;
            }
        }

        public ISpot[] Spots { get; set; }

        public object Lock { get; } = new object();

        /// <summary>
        /// returns the number of leds
        /// </summary>
        public int CountLeds(int spotsX, int spotsY)
        {
            if (spotsX <= 1 || spotsY <= 1)
            {
                //special case because it is not really a rectangle of lights but a single light or a line of lights
                return spotsX * spotsY;
            }

            //normal case
            return 2 * spotsX + 2 * spotsY - 4;
        }

        public Rectangle ExpectedScreenBound { get; private set; } = Screen.PrimaryScreen.Bounds;

        private IUserSettings UserSettings { get; }


        private void Refresh()
        {
            lock (Lock)
            {
                var screen = ExpectedScreenBound = Screen.PrimaryScreen.Bounds;
                var userSettings = UserSettings;
                Spots = BuildSpots(screen, userSettings);
            }
        }

        internal ISpot[] BuildSpots(Rectangle screen, IUserSettings userSettings)
        {
            ISpot[] spots = new Spot[CountLeds(userSettings.SpotsX, userSettings.SpotsY)];


            var canvasSizeX = screen.Width - 2 * userSettings.BorderDistanceX;
            var canvasSizeY = screen.Height - 2 * userSettings.BorderDistanceY;

            var xResolution = userSettings.SpotsX > 1 ? (canvasSizeX - userSettings.SpotWidth) / (userSettings.SpotsX - 1) : 0;
            var xRemainingOffset = userSettings.SpotsX > 1 ? ((canvasSizeX - userSettings.SpotWidth) % (userSettings.SpotsX - 1)) / 2 : 0;
            var yResolution = userSettings.SpotsY > 1 ? (canvasSizeY - userSettings.SpotHeight) / (userSettings.SpotsY - 1) : 0;
            var yRemainingOffset = userSettings.SpotsY > 1 ? ((canvasSizeY - userSettings.SpotHeight) % (userSettings.SpotsY - 1)) / 2 : 0;

            var counter = 0;
            var relationIndex = userSettings.SpotsX - userSettings.SpotsY + 1;

            for (var j = 0; j < userSettings.SpotsY; j++)
            {
                for (var i = 0; i < userSettings.SpotsX; i++)
                {
                    var isFirstColumn = i == 0;
                    var isLastColumn = i == userSettings.SpotsX - 1;
                    var isFirstRow = j == 0;
                    var isLastRow = j == userSettings.SpotsY - 1;

                    if (isFirstColumn || isLastColumn || isFirstRow || isLastRow) // needing only outer spots
                    {
                        var x = (xRemainingOffset + userSettings.BorderDistanceX + userSettings.OffsetX + i * xResolution)
                                .Clamp(0, screen.Width);

                        var y = (yRemainingOffset + userSettings.BorderDistanceY + userSettings.OffsetY + j * yResolution)
                                .Clamp(0, screen.Height);

                        var index = counter++; // in first row index is always counter

                        if (userSettings.SpotsX > 1 && userSettings.SpotsY > 1)
                        {
                            if (!isFirstRow && !isLastRow)
                            {
                                if (isFirstColumn)
                                {
                                    index += relationIndex + ((userSettings.SpotsY - 1 - j) * 3);
                                }
                                else if (isLastColumn)
                                {
                                    index -= j;
                                }
                            }

                            if (!isFirstRow && isLastRow)
                            {
                                index += relationIndex - i * 2;
                            }
                        }

                        spots[index] = new Spot(x, y, userSettings.SpotWidth, userSettings.SpotHeight);
                    }
                }
            }

            //TODO totally broken :(

            if (userSettings.OffsetLed != 0) Offset(ref spots, userSettings.OffsetLed);
            if (userSettings.SpotsY > 1 && userSettings.MirrorX) MirrorX(spots, userSettings.SpotsX, userSettings.SpotsY);
            if (userSettings.SpotsX > 1 && userSettings.MirrorY) MirrorY(spots, userSettings.SpotsX, userSettings.SpotsY);

            spots[0].IsFirst = true;
            return spots;
        }

        private static void Mirror(ISpot[] spots, int startIndex, int length)
        {
            var halfLength = (length/2);
            var endIndex = startIndex + length - 1;

            for (var i = 0; i < halfLength; i++)
            {
                spots.Swap(startIndex + i, endIndex - i);
            }
        }

        private static void MirrorX(ISpot[] spots, int spotsX, int spotsY)
        {
            // copy swap last row to first row inverse
            for (var i = 0; i < spotsX; i++)
            {
                var index1 = i;
                var index2 = (spots.Length - 1) - (spotsY - 2) - i;
                spots.Swap(index1, index2);
            }

            // mirror first column
            Mirror(spots, spotsX, spotsY - 2);

            // mirror last column
            if (spotsX > 1)
                Mirror(spots, 2 * spotsX + spotsY - 2, spotsY - 2);
        }

        private static void MirrorY(ISpot[] spots, int spotsX, int spotsY)
        {
            // copy swap last row to first row inverse
            for (var i = 0; i < spotsY - 2; i++)
            {
                var index1 = spotsX + i;
                var index2 = (spots.Length - 1) - i;
                spots.Swap(index1, index2);
            }

            // mirror first row
            Mirror(spots, 0, spotsX);

            // mirror last row
            if (spotsY > 1)
                Mirror(spots, spotsX + spotsY - 2, spotsX);
        }

        private static void Offset(ref ISpot[] spots, int offset)
        {
            ISpot[] temp = new Spot[spots.Length];
            for (var i = 0; i < spots.Length; i++)
            {
                temp[(i + temp.Length + offset)%temp.Length] = spots[i];
            }
            spots = temp;
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
