

using System;

namespace adrilight {

    static class Settings {

        private static int _mSpotsX;
        private static int _mSpotsY;
        private static int _mLedsPerSpot;
        private static int _mOffsetX;
        private static int _mOffsetY;
        private static bool _mTransferActive;
        private static bool _mOverlayActive;
        private static string _mComPort;
        private static byte _mSaturationTreshold;
        private static int _mSpotWidth;
        private static int _mSpotHeight;
        private static bool _mMirrorX;
        private static bool _mMirrorY;
        private static int _mOffsetLed;
        private static int _mBorderDistanceX;
        private static int _mBorderDistanceY;
        private static bool _mAutostart;
        private static bool _mStartMinimized;
        private static int _mMinimumRefreshRateMs;

        public static void Refresh() {
            _mSpotsX = Properties.Settings.Default.SPOTS_X;
            _mSpotsY = Properties.Settings.Default.SPOTS_Y;
            _mLedsPerSpot = Properties.Settings.Default.LEDS_PER_SPOT;
            _mOffsetX = Properties.Settings.Default.OFFSET_X;
            _mOffsetY = Properties.Settings.Default.OFFSET_Y;
            _mTransferActive = Properties.Settings.Default.TRANSFER_ACTIVE;
            _mOverlayActive = Properties.Settings.Default.OVERLAY_ACTIVE;
            _mComPort = Properties.Settings.Default.COM_PORT;
            _mSaturationTreshold = Convert.ToByte(Properties.Settings.Default.SATURATION_TRESHOLD);
            _mSpotWidth = Properties.Settings.Default.SPOT_WIDTH;
            _mSpotHeight = Properties.Settings.Default.SPOT_HEIGHT;
            _mMirrorX = Properties.Settings.Default.MIRROR_X;
            _mMirrorY = Properties.Settings.Default.MIRROR_Y;
            _mOffsetLed = Properties.Settings.Default.OFFSET_LED;
            _mBorderDistanceX = Properties.Settings.Default.BORDER_DISTANCE_X;
            _mBorderDistanceY = Properties.Settings.Default.BORDER_DISTANCE_Y;
            _mAutostart = Properties.Settings.Default.AUTOSTART;
            _mStartMinimized = Properties.Settings.Default.START_MINIMIZED;
            _mMinimumRefreshRateMs = Properties.Settings.Default.MINIMUM_REFRESH_RATE_MS;
        }

        public static int SpotsX {
            get { return _mSpotsX; }
            set {
                _mSpotsX = value;
                Properties.Settings.Default.SPOTS_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotsY {
            get { return _mSpotsY; }
            set {
                _mSpotsY = value;
                Properties.Settings.Default.SPOTS_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int LedsPerSpot {
            get { return _mLedsPerSpot; }
            set {
                _mLedsPerSpot = value;
                Properties.Settings.Default.LEDS_PER_SPOT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetX {
            get { return _mOffsetX; }
            set {
                _mOffsetX = value;
                Properties.Settings.Default.OFFSET_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetY {
            get { return _mOffsetY; }
            set {
                _mOffsetY = value;
                Properties.Settings.Default.OFFSET_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool TransferActive {
            get { return _mTransferActive; }
            set {
                _mTransferActive = value;
                Properties.Settings.Default.TRANSFER_ACTIVE = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool OverlayActive {
            get { return _mOverlayActive; }
            set {
                _mOverlayActive = value;
                Properties.Settings.Default.OVERLAY_ACTIVE = value;
                Properties.Settings.Default.Save();
            }
        }

        public static string ComPort {
            get { return _mComPort; }
            set {
                _mComPort = value;
                Properties.Settings.Default.COM_PORT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static byte SaturationTreshold {
            get { return _mSaturationTreshold; }
            set {
                _mSaturationTreshold = value;
                Properties.Settings.Default.SATURATION_TRESHOLD = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotWidth {
            get { return _mSpotWidth; }
            set {
                _mSpotWidth = value;
                Properties.Settings.Default.SPOT_WIDTH = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotHeight {
            get { return _mSpotHeight; }
            set {
                _mSpotHeight = value;
                Properties.Settings.Default.SPOT_HEIGHT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool MirrorX {
            get { return _mMirrorX; }
            set {
                _mMirrorX = value;
                Properties.Settings.Default.MIRROR_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool MirrorY {
            get { return _mMirrorY; }
            set {
                _mMirrorY = value;
                Properties.Settings.Default.MIRROR_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetLed {
            get { return _mOffsetLed; }
            set {
                _mOffsetLed = value;
                Properties.Settings.Default.OFFSET_LED = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int BorderDistanceX {
            get { return _mBorderDistanceX; }
            set {
                _mBorderDistanceX = value;
                Properties.Settings.Default.BORDER_DISTANCE_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int BorderDistanceY {
            get { return _mBorderDistanceY; }
            set {
                _mBorderDistanceY = value;
                Properties.Settings.Default.BORDER_DISTANCE_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool Autostart {
            get { return _mAutostart; }
            set {
                _mAutostart = value;
                Properties.Settings.Default.AUTOSTART = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool StartMinimized {
            get { return _mStartMinimized; }
            set {
                _mStartMinimized = value;
                Properties.Settings.Default.START_MINIMIZED = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int MinimumRefreshRateMs {
            get { return _mMinimumRefreshRateMs; }
            set {
                _mMinimumRefreshRateMs = value;
                Properties.Settings.Default.MINIMUM_REFRESH_RATE_MS = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
