/* See the file "LICENSE" for the full license governing this code. */

namespace Bambilight {

    class SpotSet {

        public static Spot[] Spots { get; set; }
        public static readonly object Lock = new object();

        public static int CountSpots(int spotsX, int spotsY) {
            int counter = spotsX * spotsY;

            if (spotsX > 1 && spotsY > 1) {
                counter -= ((spotsX - 2) * (spotsY - 2));
            }

            return counter;
        }

        public static void Refresh() {
            lock (Lock) {
                Spots = new Spot[CountSpots(Settings.SpotsX, Settings.SpotsY)];

                int canvasSizeX = (Program.ScreenWidth - 2 * Settings.BorderDistanceX);
                int canvasSizeY = (Program.ScreenHeight - 2 * Settings.BorderDistanceY);

                int xResolution = Settings.SpotsX > 1 ? (canvasSizeX - Settings.SpotWidth) / (Settings.SpotsX - 1) : 0;
                int xRemainingOffset = Settings.SpotsX > 1 ? ((canvasSizeX - Settings.SpotWidth) % (Settings.SpotsX - 1)) / 2 : 0;
                int yResolution = Settings.SpotsY > 1 ? (canvasSizeY - Settings.SpotHeight) / (Settings.SpotsY - 1) : 0;
                int yRemainingOffset = Settings.SpotsY > 1 ? ((canvasSizeY - Settings.SpotHeight) % (Settings.SpotsY - 1)) / 2 : 0;

                int x, y, counter = 0;
                int relationIndex = Settings.SpotsX - Settings.SpotsY + 1;

                for (int j = 0; j < Settings.SpotsY; j++) {
                    for (int i = 0; i < Settings.SpotsX; i++) {
                        bool isFirstColumn = (i == 0);
                        bool isLastColumn = (i == Settings.SpotsX - 1);
                        bool isFirstRow = (j == 0);
                        bool isLastRow = (j == Settings.SpotsY - 1);

                        if (isFirstColumn || isLastColumn || isFirstRow || isLastRow) // needing only outer spots
                        {
                            x = xRemainingOffset + Settings.BorderDistanceX + Settings.OffsetX + i * (xResolution) + Settings.SpotWidth / 2;
                            y = yRemainingOffset + Settings.BorderDistanceY + Settings.OffsetY + j * (yResolution) + Settings.SpotHeight / 2;

                            int index = counter++; // in first row index is always counter

                            if (Settings.SpotsX > 1 && Settings.SpotsY > 1) {
                                if (!isFirstRow && !isLastRow) {
                                    if (isFirstColumn) {
                                        index += relationIndex + ((Settings.SpotsY - 1 - j) * 3);
                                    } else if (isLastColumn) {
                                        index -= j;
                                    }
                                }

                                if (!isFirstRow && isLastRow) {
                                    index += relationIndex - (i * 2);
                                }
                            }

                            SpotSet.Spots[index] = new Spot(x, y, Settings.SpotWidth, Settings.SpotHeight);
                        }
                    }
                }


                if (Settings.OffsetLed != 0) offset(Settings.OffsetLed);
                if (Settings.SpotsY > 1 && Settings.MirrorX) mirrorX();
                if (Settings.SpotsX > 1 && Settings.MirrorY) mirrorY();
            }
        }

        private static void mirror(int startIndex, int length) {
            int halfLength = (length / 2);
            int endIndex = startIndex + length - 1;

            for (int i = 0; i < halfLength; i++) {
                swap(startIndex + i, endIndex - i);
            }
        }

        private static void swap(int index1, int index2) {
            Spot temp = Spots[index1];
            Spots[index1] = Spots[index2];
            Spots[index2] = temp;
        }

        private static void mirrorX() {
            // copy swap last row to first row inverse
            for (int i = 0; i < Settings.SpotsX; i++) {
                int index1 = i;
                int index2 = (Spots.Length - 1) - (Settings.SpotsY - 2) - i;
                swap(index1, index2);
            }

            // mirror first column
            mirror(Settings.SpotsX, Settings.SpotsY - 2);

            // mirror last column
            if (Settings.SpotsX > 1)
                mirror(2 * Settings.SpotsX + Settings.SpotsY - 2, Settings.SpotsY - 2);
        }

        private static void mirrorY() {
            // copy swap last row to first row inverse
            for (int i = 0; i < Settings.SpotsY - 2; i++) {
                int index1 = Settings.SpotsX + i;
                int index2 = (Spots.Length - 1) - i;
                swap(index1, index2);
            }

            // mirror first row
            mirror(0, Settings.SpotsX);

            // mirror last row
            if (Settings.SpotsY > 1)
                mirror(Settings.SpotsX + Settings.SpotsY - 2, Settings.SpotsX);
        }

        private static void offset(int offset) {
            Spot[] temp = new Spot[Spots.Length];
            for (int i = 0; i < Spots.Length; i++) {
                temp[(i + temp.Length + offset) % temp.Length] = Spots[i];
            }
            Spots = temp;
        }
    }


}
