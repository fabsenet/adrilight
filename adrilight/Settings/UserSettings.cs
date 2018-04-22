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
        private string _comPort = null;
        private DateTime? _lastUpdateCheck=DateTime.UtcNow;
        private int _ledsPerSpot = 1;
        private bool _mirrorX = false;
        private bool _mirrorY = false;
        private int _offsetLed = 0;
        private int _offsetX = 0;
        private int _offsetY = 0;
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

        //support future config file migration
        public int ConfigFileVersion { get; set; } = 1;


        public bool Autostart { get => _autostart; set { Set(() => Autostart, ref _autostart, value); } }
        public int BorderDistanceX { get => _borderDistanceX; set { Set(() => BorderDistanceX, ref _borderDistanceX, value); } }
        public int BorderDistanceY { get => _borderDistanceY; set { Set(() => BorderDistanceY, ref _borderDistanceY, value); } }
        public string ComPort { get => _comPort; set { Set(() => ComPort, ref _comPort, value); } }
        public DateTime? LastUpdateCheck { get => _lastUpdateCheck; set { Set(() => LastUpdateCheck, ref _lastUpdateCheck, value); } }
        public int LedsPerSpot { get => _ledsPerSpot; set { Set(() => LedsPerSpot, ref _ledsPerSpot, value); } }
        public bool MirrorX { get => _mirrorX; set { Set(() => MirrorX, ref _mirrorX, value); } }
        public bool MirrorY { get => _mirrorY; set { Set(() => MirrorY, ref _mirrorY, value); } }
        public int OffsetLed { get => _offsetLed; set { Set(() => OffsetLed, ref _offsetLed, value); } }
        public int OffsetX { get => _offsetX; set { Set(() => OffsetX, ref _offsetX, value); } }
        public int OffsetY { get => _offsetY; set { Set(() => OffsetY, ref _offsetY, value); } }
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

        public Guid InstallationId { get; set; } = Guid.NewGuid();

    }
}
