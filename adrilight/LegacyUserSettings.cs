using GalaSoft.MvvmLight;
using NLog;
using System;

namespace adrilight
{

    sealed class LegacyUserSettings : ViewModelBase, IUserSettings
    {
        private ILogger _log = LogManager.GetCurrentClassLogger();

        private int _spotsX;
        private int _spotsY;
        private int _ledsPerSpot;
        private int _offsetX;
        private int _offsetY;
        private bool _transferActive;
        private bool _overlayActive;
        private string _comPort;
        private byte _saturationTreshold;
        private int _spotWidth;
        private int _spotHeight;
        private bool _mirrorX;
        private bool _mirrorY;
        private int _offsetLed;
        private int _borderDistanceX;
        private int _borderDistanceY;
        private bool _autostart;
        private bool _startMinimized;
        private DateTime? _lastUpdateCheck;

        private byte _whitebalanceRed;
        private byte _whitebalanceGreen;
        private byte _whitebalanceBlue;

        public LegacyUserSettings()
        {
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
            _lastUpdateCheck = settings.LAST_UPDATE_CHECKDATE_UTC;
            _whitebalanceRed = settings.WhitebalanceRed;
            _whitebalanceGreen = settings.WhitebalanceGreen;
            _whitebalanceBlue = settings.WhitebalanceBlue;

            _log.Info($"UserSettings created.");
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _log.Info($"PropertyChanged: {e.PropertyName}");
        }

        private void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            _log.Info($"SettingChanging: {e.SettingName} will get the new value {e.NewValue}");
        }

        public bool UseLinearLighting
        {
            get => Properties.Settings.Default.USE_LINEAR_LIGHTING;
            set
            {
                Properties.Settings.Default.USE_LINEAR_LIGHTING = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(() => UseLinearLighting);
            }
        }
        public int SpotsX
        {
            get { return _spotsX; }
            set
            {
                _spotsX = value;
                Properties.Settings.Default.SPOTS_X = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(() => SpotsX);
            }
        }

        public int SpotsY
        {
            get { return _spotsY; }
            set
            {
                _spotsY = value;
                Properties.Settings.Default.SPOTS_Y = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => SpotsY);
            }
        }

        public int LedsPerSpot
        {
            get { return _ledsPerSpot; }
            set
            {
                _ledsPerSpot = value;
                Properties.Settings.Default.LEDS_PER_SPOT = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => LedsPerSpot);
            }
        }

        public int OffsetX
        {
            get { return _offsetX; }
            set
            {
                _offsetX = value;
                Properties.Settings.Default.OFFSET_X = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => OffsetX);
            }
        }

        public int OffsetY
        {
            get { return _offsetY; }
            set
            {
                _offsetY = value;
                Properties.Settings.Default.OFFSET_Y = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => OffsetY);
            }
        }

        public bool TransferActive
        {
            get { return _transferActive; }
            set
            {
                _transferActive = value;
                Properties.Settings.Default.TRANSFER_ACTIVE = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => TransferActive);
            }
        }

        public bool IsPreviewEnabled
        {
            get { return _overlayActive; }
            set
            {
                _overlayActive = value;
                Properties.Settings.Default.OVERLAY_ACTIVE = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => IsPreviewEnabled);
            }
        }

        public string ComPort
        {
            get { return _comPort; }
            set
            {
                _comPort = value;
                Properties.Settings.Default.COM_PORT = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => ComPort);
            }
        }

        public byte SaturationTreshold
        {
            get { return _saturationTreshold; }
            set
            {
                _saturationTreshold = value;
                Properties.Settings.Default.SATURATION_TRESHOLD = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => SaturationTreshold);
            }
        }

        public int SpotWidth
        {
            get { return _spotWidth; }
            set
            {
                _spotWidth = value;
                Properties.Settings.Default.SPOT_WIDTH = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => SpotWidth);
            }
        }

        public int SpotHeight
        {
            get { return _spotHeight; }
            set
            {
                _spotHeight = value;
                Properties.Settings.Default.SPOT_HEIGHT = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => SpotHeight);
            }
        }

        public bool MirrorX
        {
            get { return _mirrorX; }
            set
            {
                _mirrorX = value;
                Properties.Settings.Default.MIRROR_X = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => MirrorX);
            }
        }

        public bool MirrorY
        {
            get { return _mirrorY; }
            set
            {
                _mirrorY = value;
                Properties.Settings.Default.MIRROR_Y = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => MirrorY);
            }
        }

        public int OffsetLed
        {
            get { return _offsetLed; }
            set
            {
                _offsetLed = value;
                Properties.Settings.Default.OFFSET_LED = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => OffsetLed);
            }
        }

        public int BorderDistanceX
        {
            get { return _borderDistanceX; }
            set
            {
                _borderDistanceX = value;
                Properties.Settings.Default.BORDER_DISTANCE_X = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => BorderDistanceX);
            }
        }

        public int BorderDistanceY
        {
            get { return _borderDistanceY; }
            set
            {
                _borderDistanceY = value;
                Properties.Settings.Default.BORDER_DISTANCE_Y = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => BorderDistanceY);
            }
        }

        public bool Autostart
        {
            get { return _autostart; }
            set
            {
                _autostart = value;
                Properties.Settings.Default.AUTOSTART = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => Autostart);
            }
        }

        public bool StartMinimized
        {
            get { return _startMinimized; }
            set
            {
                _startMinimized = value;
                Properties.Settings.Default.START_MINIMIZED = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => StartMinimized);
            }
        }

        public DateTime? LastUpdateCheck
        {
            get { return _lastUpdateCheck; }
            set
            {
                _lastUpdateCheck = value;
                Properties.Settings.Default.LAST_UPDATE_CHECKDATE_UTC = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => LastUpdateCheck);
            }
        }


        public byte WhitebalanceRed
        {
            get => _whitebalanceRed;
            set
            {
                _whitebalanceRed = value;
                Properties.Settings.Default.WhitebalanceRed = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => WhitebalanceRed);
            }
        }

        public byte WhitebalanceGreen
        {
            get => _whitebalanceGreen;
            set
            {
                _whitebalanceGreen = value;
                Properties.Settings.Default.WhitebalanceGreen = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => WhitebalanceGreen);
            }
        }

        public byte WhitebalanceBlue
        {
            get => _whitebalanceBlue;
            set
            {
                _whitebalanceBlue = value;
                Properties.Settings.Default.WhitebalanceBlue = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged(() => WhitebalanceBlue);
            }
        }
    }
}
