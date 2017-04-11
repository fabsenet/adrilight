

using NLog;
using System;

namespace adrilight {

    static class Settings {

        private static ILogger _log = LogManager.GetCurrentClassLogger();

        private static int _spotsX;
        private static int _spotsY;
        private static int _ledsPerSpot;
        private static int _offsetX;
        private static int _offsetY;
        private static bool _transferActive;
        private static bool _overlayActive;
        private static string _comPort;
        private static byte _saturationTreshold;
        private static int _spotWidth;
        private static int _spotHeight;
        private static bool _mirrorX;
        private static bool _mirrorY;
        private static int _offsetLed;
        private static int _borderDistanceX;
        private static int _borderDistanceY;
        private static bool _autostart;
        private static bool _startMinimized;

        public static void Load() {
            var settings = Properties.Settings.Default;

            settings.SettingChanging += Default_SettingChanging;
            settings.PropertyChanged += Default_PropertyChanged;

            _spotsX = settings.SPOTS_X;
            _spotsY = settings.SPOTS_Y;
            _ledsPerSpot = settings.LEDS_PER_SPOT;
            _offsetX = settings.OFFSET_X;
            _offsetY = settings.OFFSET_Y;
            _transferActive = settings.TRANSFER_ACTIVE;
            _overlayActive = settings.OVERLAY_ACTIVE;
            _comPort = settings.COM_PORT;
            _saturationTreshold = settings.SATURATION_TRESHOLD;
            _spotWidth = settings.SPOT_WIDTH;
            _spotHeight = settings.SPOT_HEIGHT;
            _mirrorX = settings.MIRROR_X;
            _mirrorY = settings.MIRROR_Y;
            _offsetLed = settings.OFFSET_LED;
            _borderDistanceX = settings.BORDER_DISTANCE_X;
            _borderDistanceY = settings.BORDER_DISTANCE_Y;
            _autostart = settings.AUTOSTART;
            _startMinimized = settings.START_MINIMIZED;
        }

        private static void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _log.Info($"PropertyChanged: {e.PropertyName}");
        }

        private static void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            _log.Info($"SettingChanging: {e.SettingName} will get the value {e.NewValue}");
        }

        public static bool UseLinearLighting {
            get => Properties.Settings.Default.USE_LINEAR_LIGHTING;
            set {
                Properties.Settings.Default.USE_LINEAR_LIGHTING = value;
                Properties.Settings.Default.Save();
            }
        }
        public static int SpotsX {
            get { return _spotsX; }
            set {
                _spotsX = value;
                Properties.Settings.Default.SPOTS_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotsY {
            get { return _spotsY; }
            set {
                _spotsY = value;
                Properties.Settings.Default.SPOTS_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int LedsPerSpot {
            get { return _ledsPerSpot; }
            set {
                _ledsPerSpot = value;
                Properties.Settings.Default.LEDS_PER_SPOT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetX {
            get { return _offsetX; }
            set {
                _offsetX = value;
                Properties.Settings.Default.OFFSET_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetY {
            get { return _offsetY; }
            set {
                _offsetY = value;
                Properties.Settings.Default.OFFSET_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool TransferActive {
            get { return _transferActive; }
            set {
                _transferActive = value;
                Properties.Settings.Default.TRANSFER_ACTIVE = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool OverlayActive {
            get { return _overlayActive; }
            set {
                _overlayActive = value;
                Properties.Settings.Default.OVERLAY_ACTIVE = value;
                Properties.Settings.Default.Save();
            }
        }

        public static string ComPort {
            get { return _comPort; }
            set {
                _comPort = value;
                Properties.Settings.Default.COM_PORT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static byte SaturationTreshold {
            get { return _saturationTreshold; }
            set {
                _saturationTreshold = value;
                Properties.Settings.Default.SATURATION_TRESHOLD = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotWidth {
            get { return _spotWidth; }
            set {
                _spotWidth = value;
                Properties.Settings.Default.SPOT_WIDTH = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int SpotHeight {
            get { return _spotHeight; }
            set {
                _spotHeight = value;
                Properties.Settings.Default.SPOT_HEIGHT = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool MirrorX {
            get { return _mirrorX; }
            set {
                _mirrorX = value;
                Properties.Settings.Default.MIRROR_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool MirrorY {
            get { return _mirrorY; }
            set {
                _mirrorY = value;
                Properties.Settings.Default.MIRROR_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OffsetLed {
            get { return _offsetLed; }
            set {
                _offsetLed = value;
                Properties.Settings.Default.OFFSET_LED = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int BorderDistanceX {
            get { return _borderDistanceX; }
            set {
                _borderDistanceX = value;
                Properties.Settings.Default.BORDER_DISTANCE_X = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int BorderDistanceY {
            get { return _borderDistanceY; }
            set {
                _borderDistanceY = value;
                Properties.Settings.Default.BORDER_DISTANCE_Y = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool Autostart {
            get { return _autostart; }
            set {
                _autostart = value;
                Properties.Settings.Default.AUTOSTART = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool StartMinimized {
            get { return _startMinimized; }
            set {
                _startMinimized = value;
                Properties.Settings.Default.START_MINIMIZED = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
