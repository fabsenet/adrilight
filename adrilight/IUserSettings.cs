using System;
using System.ComponentModel;

namespace adrilight
{
    public interface IUserSettings : INotifyPropertyChanged
    {
        bool Autostart { get; set; }
        int BorderDistanceX { get; set; }
        int BorderDistanceY { get; set; }
        string ComPort { get; set; }
        DateTime? LastUpdateCheck { get; set; }
        int LedsPerSpot { get; set; }
        bool MirrorX { get; set; }
        bool MirrorY { get; set; }
        int OffsetLed { get; set; }
        int OffsetX { get; set; }
        int OffsetY { get; set; }
        bool IsPreviewEnabled { get; set; }
        byte SaturationTreshold { get; set; }
        int SpotHeight { get; set; }
        int SpotsX { get; set; }
        int SpotsY { get; set; }
        int SpotWidth { get; set; }
        bool StartMinimized { get; set; }
        bool TransferActive { get; set; }
        bool UseLinearLighting { get; set; }
    }
}