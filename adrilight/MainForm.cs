/* See the file "LICENSE" for the full license governing this code. */

using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Bambilight {

    public partial class MainForm : Form {

        private NotifyIcon mNotifyIcon;
        private ContextMenu mContextMenu;

        DxScreenCapture mDxScreenCapture;
        SerialStream mSerialStream;
        Overlay mOverlay;

        public MainForm() {
            InitializeComponent();
            initTrayIcon();

            Application.ApplicationExit += OnApplicationExit;
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
        }

        private void initTrayIcon() {
            mContextMenu = new ContextMenu();
            mContextMenu.MenuItems.Add("Exit", NotifyIcon_ExitClick);

            mNotifyIcon = new NotifyIcon();
            mNotifyIcon.Text = "Bambilight";
            mNotifyIcon.Icon = new Icon(Properties.Resources.deer, 40, 40);
            mNotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

            mNotifyIcon.ContextMenu = mContextMenu;
        }

        private void MainForm_Load(object sender, EventArgs e) {
            initTags();
            initFieldLimits();

            refreshFields();

            if (Settings.StartMinimized) {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            if (FormWindowState.Minimized == WindowState) {
                mNotifyIcon.Visible = true;
                ShowInTaskbar = false;
            } else if (FormWindowState.Normal == WindowState) {
                mNotifyIcon.Visible = false;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void NotifyIcon_ExitClick(object sender, EventArgs e) {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            mNotifyIcon.Dispose();
        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e) {
            if (e.Mode == PowerModes.Resume) {
                RefreshTransferState();
                RefreshOverlayState();
                RefreshCapturingState();
            } else if(e.Mode == PowerModes.Suspend) {
                StopBackgroundWorkers();
            }
        }

        // for universal change of track bar value labels => see setTrackBarValue
        private void initTags() {
            trackBarSpotWidth.Tag = labelSpotWidth;
            labelSpotWidth.Tag = labelSpotWidth.Text;

            trackBarSpotHeight.Tag = labelSpotHeight;
            labelSpotHeight.Tag = labelSpotHeight.Text;

            trackBarOffsetX.Tag = labelOffsetX;
            labelOffsetX.Tag = labelOffsetX.Text;

            trackBarOffsetY.Tag = labelOffsetY;
            labelOffsetY.Tag = labelOffsetY.Text;

            trackBarBorderDistanceX.Tag = labelBorderDistanceX;
            labelBorderDistanceX.Tag = labelBorderDistanceX.Text;

            trackBarBorderDistanceY.Tag = labelBorderDistanceY;
            labelBorderDistanceY.Tag = labelBorderDistanceY.Text;
        }

        // TODO min and max values should be dynamic
        private void initFieldLimits() {
            numericUpDownSpotsX.Minimum = 1;
            numericUpDownSpotsX.Maximum = 100;

            numericUpDownSpotsY.Minimum = 1;
            numericUpDownSpotsY.Maximum = 100;

            trackBarSpotWidth.Minimum = 10;
            trackBarSpotWidth.Maximum = Program.ScreenWidth;

            trackBarSpotHeight.Minimum = 10;
            trackBarSpotHeight.Maximum = Program.ScreenHeight;

            trackBarBorderDistanceX.Minimum = 0;
            trackBarBorderDistanceX.Maximum = Program.ScreenWidth / 2;

            trackBarBorderDistanceY.Minimum = 0;
            trackBarBorderDistanceY.Maximum = Program.ScreenHeight / 2;

            trackBarOffsetX.Minimum = -Program.ScreenWidth;
            trackBarOffsetX.Maximum = Program.ScreenWidth;

            trackBarOffsetY.Minimum = -Program.ScreenHeight;
            trackBarOffsetY.Maximum = Program.ScreenHeight;

            numericUpDownLedsPerSpot.Minimum = 1;
            numericUpDownLedsPerSpot.Maximum = 100;

            numericUpDownSaturationTreshold.Minimum = 0;
            numericUpDownSaturationTreshold.Maximum = 255;

            numericUpDownLedOffset.Minimum = 0;
            numericUpDownLedOffset.Maximum = 500;

            numericUpDownMinimumRefreshRateMs.Minimum = 0;
            numericUpDownMinimumRefreshRateMs.Maximum = 1000;
        }

        private void refreshFields() {
            numericUpDownSpotsX.Value = Settings.SpotsX;

            numericUpDownSpotsY.Value = Settings.SpotsY;

            numericUpDownLedsPerSpot.Value = Settings.LedsPerSpot;

            setTrackBarValue(trackBarOffsetX, Settings.OffsetX);

            setTrackBarValue(trackBarOffsetY, Settings.OffsetY);

            checkBoxTransferActive.Checked = Settings.TransferActive;

            checkBoxOverlayActive.Checked = Settings.OverlayActive;

            comboBoxComPort.Text = Settings.ComPort;
            comboBoxComPort.Items.Clear();
            foreach (string s in SerialPort.GetPortNames()) {
                comboBoxComPort.Items.Add(s);
            }
            if (comboBoxComPort.Text.Equals("") && comboBoxComPort.Items.Count > 0) {
                comboBoxComPort.Text = (string)comboBoxComPort.Items[0];
            }

            numericUpDownSaturationTreshold.Value = Settings.SaturationTreshold;

            setTrackBarValue(trackBarSpotWidth, Settings.SpotWidth);

            setTrackBarValue(trackBarSpotHeight, Settings.SpotHeight);

            checkBoxMirrorX.Checked = Settings.MirrorX;

            checkBoxMirrorY.Checked = Settings.MirrorY;

            numericUpDownLedOffset.Value = Settings.OffsetLed;

            setTrackBarValue(trackBarBorderDistanceX, Settings.BorderDistanceX);

            setTrackBarValue(trackBarBorderDistanceY, Settings.BorderDistanceY);

            checkBoxAutostart.Checked = Settings.Autostart;

            checkBoxStartMinimized.Checked = Settings.StartMinimized;

            numericUpDownMinimumRefreshRateMs.Value = Settings.MinimumRefreshRateMs;

            groupBoxLEDs.Text = "LEDs (" + (SpotSet.CountSpots(Settings.SpotsX, Settings.SpotsY) * Settings.LedsPerSpot) + ")";
        }

        private void setTrackBarValue(TrackBar trackBar, int value) {
            if (value < trackBar.Minimum) value = (int)trackBar.Minimum;
            if (value > trackBar.Maximum) value = (int)trackBar.Maximum;

            trackBar.Value = value;
            Label label = (Label)trackBar.Tag;
            label.Text = (string)label.Tag + ":" + System.Environment.NewLine + "   " + value;
        }

        private void refreshAll() {
            SpotSet.Refresh();
            refreshFields();
            RefreshOverlay();
        }

        private void numericUpDownSpotsX_ValueChanged(object sender, EventArgs e) {
            Settings.SpotsX = (int)numericUpDownSpotsX.Value;
            refreshAll();
        }

        private void numericUpDownSpotsY_ValueChanged(object sender, EventArgs e) {
            Settings.SpotsY = (int)numericUpDownSpotsY.Value;
            refreshAll();
        }

        private void numericUpDownLedsPerSpot_ValueChanged(object sender, EventArgs e) {
            Settings.LedsPerSpot = (int)numericUpDownLedsPerSpot.Value;
            refreshAll();
        }

        private void trackBarOffsetX_Scroll(object sender, EventArgs e) {
            Settings.OffsetX = trackBarOffsetX.Value;
            refreshAll();
        }

        private void trackBarOffsetY_Scroll(object sender, EventArgs e) {
            Settings.OffsetY = trackBarOffsetY.Value;
            refreshAll();
        }

        private void comboBoxComPort_SelectedIndexChanged(object sender, EventArgs e) {
            Settings.ComPort = comboBoxComPort.Text;
            // can be changed on the fly without refreshing
        }

        private void numericUpDownSaturationTreshold_ValueChanged(object sender, EventArgs e) {
            Settings.SaturationTreshold = Convert.ToByte((int)numericUpDownSaturationTreshold.Value);
            // can be changed on the fly without refreshing
        }

        private void trackBarSpotWidth_Scroll(object sender, EventArgs e) {
            Settings.SpotWidth = trackBarSpotWidth.Value;
            refreshAll();
        }

        private void trackBarSpotHeight_Scroll(object sender, EventArgs e) {
            Settings.SpotHeight = trackBarSpotHeight.Value;
            refreshAll();
        }

        private void buttonResetPosition_Click(object sender, EventArgs e) {
            trackBarOffsetX.Value = 0;
            trackBarOffsetX_Scroll(null, null);
            trackBarOffsetY.Value = 0;
            trackBarOffsetY_Scroll(null, null);
        }

        private void checkBoxMirrorX_CheckedChanged(object sender, EventArgs e) {
            Settings.MirrorX = checkBoxMirrorX.Checked;
            refreshAll();
        }

        private void checkBoxMirrorY_CheckedChanged(object sender, EventArgs e) {
            Settings.MirrorY = checkBoxMirrorY.Checked;
            refreshAll();
        }

        private void checkBoxTransferActive_CheckedChanged(object sender, EventArgs e) {
            Settings.TransferActive = checkBoxTransferActive.Checked;
            refreshAll();

            RefreshCapturingState();
            RefreshTransferState();
        }

        private void checkBoxOverlayActive_CheckedChanged(object sender, EventArgs e) {
            Settings.OverlayActive = checkBoxOverlayActive.Checked;
            refreshAll();

            RefreshCapturingState();
            RefreshOverlayState();
        }

        private void numericUpDownLedOffset_ValueChanged(object sender, EventArgs e) {
            Settings.OffsetLed = (int)numericUpDownLedOffset.Value;
            refreshAll();
        }

        private void trackBarBorderDistanceX_Scroll(object sender, EventArgs e) {
            Settings.BorderDistanceX = trackBarBorderDistanceX.Value;
            refreshAll();
        }

        private void trackBarBorderDistanceY_Scroll(object sender, EventArgs e) {
            Settings.BorderDistanceY = trackBarBorderDistanceY.Value;
            refreshAll();
        }

        private void checkBoxAutostart_CheckedChanged(object sender, EventArgs e) {
            Settings.Autostart = checkBoxAutostart.Checked;
            if (Settings.Autostart) {
                Autostart.Add();
            } else {
                Autostart.Remove();
            }
        }

        private void checkBoxStartMinimized_CheckedChanged(object sender, EventArgs e) {
            Settings.StartMinimized = checkBoxStartMinimized.Checked;
            // can be changed on the fly without refreshing
        }

        private void numericUpDownMinimumRefreshRateMs_ValueChanged(object sender, EventArgs e) {
            Settings.MinimumRefreshRateMs = (int)numericUpDownMinimumRefreshRateMs.Value;
            // can be changed on the fly without refreshing
        }

        private void RefreshCapturingState() {
            if (null == mDxScreenCapture) {
                mDxScreenCapture = new DxScreenCapture();
            }

            if (Settings.TransferActive || Settings.OverlayActive) {
                mDxScreenCapture.Start();
            } else {
                mDxScreenCapture.Stop();
                mDxScreenCapture = null;
            }
        }

        private void RefreshTransferState() {
            if (null == mSerialStream) {
                mSerialStream = new SerialStream();
            }

            if (Settings.TransferActive) {
                comboBoxComPort.Enabled = false;
                mSerialStream.Start();
            } else {
                mSerialStream.Stop();
                mSerialStream = null;
                comboBoxComPort.Enabled = true;
            }
        }

        private void RefreshOverlayState() {
            if (null == mOverlay) {
                mOverlay = new Overlay();
            }

            if (Settings.OverlayActive) {
                TopMost = true;
                mOverlay.Start();
            } else {
                mOverlay.Stop();
                mOverlay = null;
                TopMost = false;
            }
        }

        private void RefreshOverlay() {
            if (null != mOverlay) {
                mOverlay.RefreshGraphics();
            }
        }

        private void StopBackgroundWorkers() {
            if (null != mOverlay) {
                mOverlay.Stop();
                mOverlay = null;
            }
            if (null != mSerialStream) {
                mSerialStream.Stop();
                mSerialStream = null;
            }
            if (null != mDxScreenCapture) {
                mDxScreenCapture.Stop();
                mDxScreenCapture = null;
            }
        }
    }
}
