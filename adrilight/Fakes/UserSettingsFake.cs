using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Fakes
{
    class UserSettingsFake : IUserSettings
    {
        public bool Autostart { get; set; } = true;
        public int BorderDistanceX { get; set; } = 33;
        public int BorderDistanceY { get; set; } = 44;
        public string ComPort { get; set; } = "COM7";
        public DateTime? LastUpdateCheck { get; set; } = DateTime.Now;
        public int LedsPerSpot { get; set; } = 1;
        public bool MirrorX { get; set; } = true;
        public bool MirrorY { get; set; } = false;
        public int OffsetLed { get; set; } = 22;
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
        public bool IsPreviewEnabled { get; set; } = false;
        public byte SaturationTreshold { get; set; } = 4;
        public int SpotsX { get; set; } = 80;
        public int SpotsY { get; set; } = 55;
        public int SpotHeight { get; set; } = 40;
        public int SpotWidth { get; set; } = 40;
        public bool StartMinimized { get; set; } = true;
        public bool TransferActive { get; set; } = false;
        public bool UseLinearLighting { get; set; } = false;

        public byte WhitebalanceRed { get; set; } = 100;
        public byte WhitebalanceGreen { get; set; } = 100;
        public byte WhitebalanceBlue { get; set; } = 100;

        public Guid InstallationId { get; set; } = Guid.NewGuid();

        public bool SendRandomColors { get; set; }
        public int LimitFps { get; set; } = 60;
        public string AdrilightVersion { get; set; } = "2.0.6";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
