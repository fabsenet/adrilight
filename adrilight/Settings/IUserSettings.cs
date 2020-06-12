using adrilight.Settings;
using System;
using System.ComponentModel;

namespace adrilight
{
    public interface IUserSettings : INotifyPropertyChanged
    {
        int ConfigFileVersion { get; set; }
        bool Autostart { get; set; }
        int BorderDistanceX { get; set; }
        int BorderDistanceY { get; set; }
        string ComPort { get; set; }

        bool MirrorX { get; set; }
        bool MirrorY { get; set; }
        int OffsetLed { get; set; }

        bool IsPreviewEnabled { get; set; }
        byte SaturationTreshold { get; set; }
        int SpotHeight { get; set; }
        int SpotsX { get; set; }
        int SpotsY { get; set; }
        int SpotWidth { get; set; }
        bool StartMinimized { get; set; }
        bool TransferActive { get; set; }
        bool UseLinearLighting { get; set; }

        Guid InstallationId { get; set; }

        byte WhitebalanceRed { get; set; }
        byte WhitebalanceGreen { get; set; }
        byte WhitebalanceBlue { get; set; }

        byte AltWhitebalanceRed { get; set; }
        byte AltWhitebalanceGreen { get; set; }
        byte AltWhitebalanceBlue { get; set; }

        byte RedtoWhite { get; set; }
        byte GreentoWhite { get; set; }
        byte BluetoWhite { get; set; }
        byte WhitetoColors { get; set; }

        bool StripType { get; set; }

        bool SendRandomColors { get; set; }

        int LimitFps { get; set; }

        string AdrilightVersion { get; set; }
        AlternateWhiteBalanceModeEnum AlternateWhiteBalanceMode { get; set; }
    }
}