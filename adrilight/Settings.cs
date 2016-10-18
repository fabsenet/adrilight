/* See the file "LICENSE" for the full license governing this code. */

using System;

namespace Bambilight {

    static class Settings {

        private static int mSpotsX;
        private static int mSpotsY;
        private static int mLedsPerSpot;
        private static int mOffsetX;
        private static int mOffsetY;
        private static bool mTransferActive;
        private static bool mOverlayActive;
        private static string mComPort;
        private static byte mSaturationTreshold;
        private static int mSpotWidth;
        private static int mSpotHeight;
        private static bool mMirrorX;
        private static bool mMirrorY;
        private static int mOffsetLed;
        private static int mBorderDistanceX;
        private static int mBorderDistanceY;
        private static bool mAutostart;
        private static bool mStartMinimized;
        private static int mMinimumRefreshRateMs;

        public static void Refresh() {
            mSpotsX = Properties.Settings.Default.SPOTS_X;
            mSpotsY = Properties.Settings.Default.SPOTS_Y;
            mLedsPerSpot = Properties.Settings.Default.LEDS_PER_SPOT;
            mOffsetX = Properties.Settings.Default.OFFSET_X;
            mOffsetY = Properties.Settings.Default.OFFSET_Y;
            mTransferActive = Properties.Settings.Default.TRANSFER_ACTIVE;
            mOverlayActive = Properties.Settings.Default.OVERLAY_ACTIVE;
            mComPort = Properties.Settings.Default.COM_PORT;
            mSaturationTreshold = Convert.ToByte(Properties.Settings.Default.SATURATION_TRESHOLD);
            mSpotWidth = Properties.Settings.Default.SPOT_WIDTH;
            mSpotHeight = Properties.Settings.Default.SPOT_HEIGHT;
            mMirrorX = Properties.Settings.Default.MIRROR_X;
            mMirrorY = Properties.Settings.Default.MIRROR_Y;
            mOffsetLed = Properties.Settings.Default.OFFSET_LED;
            mBorderDistanceX = Properties.Settings.Default.BORDER_DISTANCE_X;
            mBorderDistanceY = Properties.Settings.Default.BORDER_DISTANCE_Y;
            mAutostart = Properties.Settings.Default.AUTOSTART;
            mStartMinimized = Properties.Settings.Default.START_MINIMIZED;
            mMinimumRefreshRateMs = Properties.Settings.Default.MINIMUM_REFRESH_RATE_MS;
        }

        public static int SpotsX {
            get { return mSpotsX; }
            set {
                mSpotsX = value;
                Properties.Settings.Default.SPOTS_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotsY {
            get { return mSpotsY; }
            set {
                mSpotsY = value;
                Properties.Settings.Default.SPOTS_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int LedsPerSpot {
            get { return mLedsPerSpot; }
            set {
                mLedsPerSpot = value;
                Properties.Settings.Default.LEDS_PER_SPOT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetX {
            get { return mOffsetX; }
            set {
                mOffsetX = value;
                Properties.Settings.Default.OFFSET_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetY {
            get { return mOffsetY; }
            set {
                mOffsetY = value;
                Properties.Settings.Default.OFFSET_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool TransferActive {
            get { return mTransferActive; }
            set {
                mTransferActive = value;
                Properties.Settings.Default.TRANSFER_ACTIVE = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool OverlayActive {
            get { return mOverlayActive; }
            set {
                mOverlayActive = value;
                Properties.Settings.Default.OVERLAY_ACTIVE = value;
                Properties.Settings.Default.Save();
            }
        }

        public static string ComPort {
            get { return mComPort; }
            set {
                mComPort = value;
                Properties.Settings.Default.COM_PORT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static byte SaturationTreshold {
            get { return mSaturationTreshold; }
            set {
                mSaturationTreshold = value;
                Properties.Settings.Default.SATURATION_TRESHOLD = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotWidth {
            get { return mSpotWidth; }
            set {
                mSpotWidth = value;
                Properties.Settings.Default.SPOT_WIDTH = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotHeight {
            get { return mSpotHeight; }
            set {
                mSpotHeight = value;
                Properties.Settings.Default.SPOT_HEIGHT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool MirrorX {
            get { return mMirrorX; }
            set {
                mMirrorX = value;
                Properties.Settings.Default.MIRROR_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool MirrorY {
            get { return mMirrorY; }
            set {
                mMirrorY = value;
                Properties.Settings.Default.MIRROR_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetLed {
            get { return mOffsetLed; }
            set {
                mOffsetLed = value;
                Properties.Settings.Default.OFFSET_LED = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int BorderDistanceX {
            get { return mBorderDistanceX; }
            set {
                mBorderDistanceX = value;
                Properties.Settings.Default.BORDER_DISTANCE_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int BorderDistanceY {
            get { return mBorderDistanceY; }
            set {
                mBorderDistanceY = value;
                Properties.Settings.Default.BORDER_DISTANCE_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool Autostart {
            get { return mAutostart; }
            set {
                mAutostart = value;
                Properties.Settings.Default.AUTOSTART = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool StartMinimized {
            get { return mStartMinimized; }
            set {
                mStartMinimized = value;
                Properties.Settings.Default.START_MINIMIZED = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int MinimumRefreshRateMs {
            get { return mMinimumRefreshRateMs; }
            set {
                mMinimumRefreshRateMs = value;
                Properties.Settings.Default.MINIMUM_REFRESH_RATE_MS = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
