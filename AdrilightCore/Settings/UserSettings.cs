using adrilight.Settings;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace adrilight
{
    internal class UserSettings : ViewModelBase, IUserSettings
    {
        private bool _autostart = false;
        private int _borderDistanceX = 0;
        private int _borderDistanceY = 100;
        private string? _comPort = null;
        private string _adrilightVersion = "2.0.7";
        private bool _mirrorX = false;
        private bool _mirrorY = false;
        private int _offsetLed = 0;
        private bool _isPreviewEnabled = false;
        private byte _saturationTreshold = 10;
        private int _spotHeight = 50;
        private int _spotsX = 5;
        private int _spotsY = 7;
        private int _spotWidth = 50;
        private bool _startMinimized = false;
        private bool _transferActive = false;
        private bool _useLinearLighting = false;

        private byte _whitebalanceRed = 100;
        private byte _whitebalanceGreen = 100;
        private byte _whitebalanceBlue = 100;

        private byte _altWhitebalanceRed = 100;
        private byte _altWhitebalanceGreen = 100;
        private byte _altWhitebalanceBlue = 80;

        private bool _sendRandomColors = false;
        private int _limitFps = 60;
        private int _configFileVersion = 2;
        private AlternateWhiteBalanceModeEnum _alternateWhiteBalanceMode = AlternateWhiteBalanceModeEnum.Off;

        //support future config file migration
        public int ConfigFileVersion { get => _configFileVersion; set { Set(() => ConfigFileVersion, ref _configFileVersion, value); } }


        public bool Autostart { get => _autostart; set { Set(() => Autostart, ref _autostart, value); } }
        public int BorderDistanceX { get => _borderDistanceX; set { Set(() => BorderDistanceX, ref _borderDistanceX, value); } }
        public int BorderDistanceY { get => _borderDistanceY; set { Set(() => BorderDistanceY, ref _borderDistanceY, value); } }
        public string? ComPort { get => _comPort; set { Set(() => ComPort, ref _comPort, value); } }

        public string AdrilightVersion { get => _adrilightVersion; set { Set(() => AdrilightVersion, ref _adrilightVersion, value); } }

        public bool MirrorX { get => _mirrorX; set { Set(() => MirrorX, ref _mirrorX, value); } }
        public bool MirrorY { get => _mirrorY; set { Set(() => MirrorY, ref _mirrorY, value); } }
        public int OffsetLed { get => _offsetLed; set { Set(() => OffsetLed, ref _offsetLed, value); } }

        public int LimitFps { get => _limitFps; set { Set(() => LimitFps, ref _limitFps, value); } }

        public bool IsPreviewEnabled { get => _isPreviewEnabled; set { Set(() => IsPreviewEnabled, ref _isPreviewEnabled, value); } }
        public byte SaturationTreshold { get => _saturationTreshold; set { Set(() => SaturationTreshold, ref _saturationTreshold, value); } }
        public int SpotHeight { get => _spotHeight; set { Set(() => SpotHeight, ref _spotHeight, value); } }
        public int SpotsX { get => _spotsX; set { Set(() => SpotsX, ref _spotsX, value); } }
        public int SpotsY { get => _spotsY; set { Set(() => SpotsY, ref _spotsY, value); } }
        public int SpotWidth { get => _spotWidth; set { Set(() => SpotWidth, ref _spotWidth, value); } }
        public bool StartMinimized { get => _startMinimized; set { Set(() => StartMinimized, ref _startMinimized, value); } }
        public bool TransferActive { get => _transferActive; set { Set(() => TransferActive, ref _transferActive, value); } }
        public bool UseLinearLighting { get => _useLinearLighting; set { Set(() => UseLinearLighting, ref _useLinearLighting, value); } }

        public byte WhitebalanceRed { get => _whitebalanceRed; set { Set(() => WhitebalanceRed, ref _whitebalanceRed, value); } }
        public byte WhitebalanceGreen { get => _whitebalanceGreen; set { Set(() => WhitebalanceGreen, ref _whitebalanceGreen, value); } }
        public byte WhitebalanceBlue { get => _whitebalanceBlue; set { Set(() => WhitebalanceBlue, ref _whitebalanceBlue, value); } }

        public byte AltWhitebalanceRed { get => _altWhitebalanceRed; set { Set(() => AltWhitebalanceRed, ref _altWhitebalanceRed, value); } }
        public byte AltWhitebalanceGreen { get => _altWhitebalanceGreen; set { Set(() => AltWhitebalanceGreen, ref _altWhitebalanceGreen, value); } }
        public byte AltWhitebalanceBlue { get => _altWhitebalanceBlue; set { Set(() => AltWhitebalanceBlue, ref _altWhitebalanceBlue, value); } }

        public bool SendRandomColors { get => _sendRandomColors; set { Set(() => SendRandomColors, ref _sendRandomColors, value); } }

        public Guid InstallationId { get; set; } = Guid.NewGuid();
        public AlternateWhiteBalanceModeEnum AlternateWhiteBalanceMode { get => _alternateWhiteBalanceMode; set { Set(() => AlternateWhiteBalanceMode, ref _alternateWhiteBalanceMode, value); } }
    }
}
