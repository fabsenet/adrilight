

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using NLog;

namespace adrilight {

    public partial class MainForm : Form
    {

        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        private NotifyIcon _mNotifyIcon;
        private ContextMenu _mContextMenu;
        SerialStream _mSerialStream;
        Overlay _mOverlay;

        public MainForm() {
            _log.Debug("Creating Mainform");
            InitializeComponent();
            InitTrayIcon();

            Application.ApplicationExit += OnApplicationExit;
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
            Settings.OverlayActive = checkBoxOverlayActive.Checked;
            _log.Debug("Created Mainform");
        }

        private void InitTrayIcon() {
            _mContextMenu = new ContextMenu();
            _mContextMenu.MenuItems.Add("Exit", NotifyIcon_ExitClick);

            _mNotifyIcon = new NotifyIcon
            {
                Text = "adrilight",
                Icon = this.Icon,
                Visible = true,
                ContextMenu = _mContextMenu
            };
            _mNotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }

        private void MainForm_Load(object sender, EventArgs e) {
            InitTags();
            InitFieldLimits();

            RefreshFields();

            if (Settings.StartMinimized) {
                ShowInTaskbar = Visible = false;
                WindowState = FormWindowState.Minimized;
            }
            RefreshAll();

            RefreshCapturingState();
            RefreshOverlayState();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
            Visible = WindowState != FormWindowState.Minimized;
            Trace.WriteLine("WindowState is "+WindowState);
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Visible = true;
            WindowState = FormWindowState.Normal;
        }

        private void NotifyIcon_ExitClick(object sender, EventArgs e) {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            _log.Debug("Application exit!");
            _mNotifyIcon.Dispose();
        }

        private bool _isSuspending;

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            _log.Debug("Changing Powermode to {0}", e.Mode);

            //if (e.Mode == PowerModes.Resume)
            //{
            //    _isSuspending = false;
            //    RefreshTransferState();
            //    RefreshOverlayState();
            //    RefreshCapturingState();
            //}
            //else if (e.Mode == PowerModes.Suspend)
            //{
            //    _isSuspending = true;
            //    StopBackgroundWorkers();
            //    RefreshCapturingState();
            //}
        }

        // for universal change of track bar value labels => see setTrackBarValue
        private void InitTags() {
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
        private void InitFieldLimits() {
            numericUpDownSpotsX.Minimum = 1;
            numericUpDownSpotsX.Maximum = 100;

            numericUpDownSpotsY.Minimum = 1;
            numericUpDownSpotsY.Maximum = 100;

            trackBarSpotWidth.Minimum = 10;
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            trackBarSpotWidth.Maximum = screenWidth;

            trackBarSpotHeight.Minimum = 10;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;
            trackBarSpotHeight.Maximum = screenHeight;

            trackBarBorderDistanceX.Minimum = 0;
            trackBarBorderDistanceX.Maximum = Screen.PrimaryScreen.Bounds.Width / 2;

            trackBarBorderDistanceY.Minimum = 0;
            trackBarBorderDistanceY.Maximum = screenHeight / 2;

            trackBarOffsetX.Minimum = -Screen.PrimaryScreen.Bounds.Width;
            trackBarOffsetX.Maximum = screenWidth;

            trackBarOffsetY.Minimum = -screenHeight;
            trackBarOffsetY.Maximum = screenHeight;

            numericUpDownLedsPerSpot.Minimum = 1;
            numericUpDownLedsPerSpot.Maximum = 100;

            numericUpDownSaturationTreshold.Minimum = 0;
            numericUpDownSaturationTreshold.Maximum = 255;

            numericUpDownLedOffset.Minimum = 0;
            numericUpDownLedOffset.Maximum = 500;

            numericUpDownMinimumRefreshRateMs.Minimum = 0;
            numericUpDownMinimumRefreshRateMs.Maximum = 1000;
        }

        private void RefreshFields()
        {
            rbLinearLighting.Checked = Settings.UseLinearLighting;
            rbNonLinearLighting.Checked = !Settings.UseLinearLighting;

            numericUpDownSpotsX.Value = Settings.SpotsX;

            numericUpDownSpotsY.Value = Settings.SpotsY;

            numericUpDownLedsPerSpot.Value = Settings.LedsPerSpot;

            SetTrackBarValue(trackBarOffsetX, Settings.OffsetX);

            SetTrackBarValue(trackBarOffsetY, Settings.OffsetY);

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

            SetTrackBarValue(trackBarSpotWidth, Settings.SpotWidth);

            SetTrackBarValue(trackBarSpotHeight, Settings.SpotHeight);

            checkBoxMirrorX.Checked = Settings.MirrorX;

            checkBoxMirrorY.Checked = Settings.MirrorY;

            numericUpDownLedOffset.Value = Settings.OffsetLed;

            SetTrackBarValue(trackBarBorderDistanceX, Settings.BorderDistanceX);

            SetTrackBarValue(trackBarBorderDistanceY, Settings.BorderDistanceY);

            checkBoxAutostart.Checked = Settings.Autostart;

            checkBoxStartMinimized.Checked = Settings.StartMinimized;

            numericUpDownMinimumRefreshRateMs.Value = Settings.MinimumRefreshRateMs;

            groupBoxLEDs.Text = "LEDs (" + (SpotSet.CountLeds(Settings.SpotsX, Settings.SpotsY) * Settings.LedsPerSpot) + ")";
        }

        private void SetTrackBarValue(TrackBar trackBar, int value) {
            if (value < trackBar.Minimum) value = (int)trackBar.Minimum;
            if (value > trackBar.Maximum) value = (int)trackBar.Maximum;

            trackBar.Value = value;
            Label label = (Label)trackBar.Tag;
            label.Text = (string)label.Tag + ":" + System.Environment.NewLine + "   " + value;
        }

        private void RefreshAll() {
            SpotSet.Refresh();
            RefreshFields();
            RefreshOverlay();
        }

        private void numericUpDownSpotsX_ValueChanged(object sender, EventArgs e) {
            Settings.SpotsX = (int)numericUpDownSpotsX.Value;
            RefreshAll();
        }

        private void numericUpDownSpotsY_ValueChanged(object sender, EventArgs e) {
            Settings.SpotsY = (int)numericUpDownSpotsY.Value;
            RefreshAll();
        }

        private void numericUpDownLedsPerSpot_ValueChanged(object sender, EventArgs e) {
            Settings.LedsPerSpot = (int)numericUpDownLedsPerSpot.Value;
            RefreshAll();
        }

        private void trackBarOffsetX_Scroll(object sender, EventArgs e) {
            Settings.OffsetX = trackBarOffsetX.Value;
            RefreshAll();
        }

        private void trackBarOffsetY_Scroll(object sender, EventArgs e) {
            Settings.OffsetY = trackBarOffsetY.Value;
            RefreshAll();
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
            RefreshAll();
        }

        private void trackBarSpotHeight_Scroll(object sender, EventArgs e) {
            Settings.SpotHeight = trackBarSpotHeight.Value;
            RefreshAll();
        }

        private void buttonResetPosition_Click(object sender, EventArgs e) {
            trackBarOffsetX.Value = 0;
            trackBarOffsetX_Scroll(null, null);
            trackBarOffsetY.Value = 0;
            trackBarOffsetY_Scroll(null, null);
        }

        private void checkBoxMirrorX_CheckedChanged(object sender, EventArgs e) {
            Settings.MirrorX = checkBoxMirrorX.Checked;
            RefreshAll();
        }

        private void checkBoxMirrorY_CheckedChanged(object sender, EventArgs e) {
            Settings.MirrorY = checkBoxMirrorY.Checked;
            RefreshAll();
        }

        private void checkBoxTransferActive_CheckedChanged(object sender, EventArgs e) {
            Settings.TransferActive = checkBoxTransferActive.Checked;
            RefreshAll();

            RefreshCapturingState();
            RefreshTransferState();
        }

        private void checkBoxOverlayActive_CheckedChanged(object sender, EventArgs e) {
            Settings.OverlayActive = checkBoxOverlayActive.Checked;
            RefreshAll();

            RefreshCapturingState();
            RefreshOverlayState();
        }

        private void numericUpDownLedOffset_ValueChanged(object sender, EventArgs e) {
            Settings.OffsetLed = (int)numericUpDownLedOffset.Value;
            RefreshAll();
        }

        private void trackBarBorderDistanceX_Scroll(object sender, EventArgs e) {
            Settings.BorderDistanceX = trackBarBorderDistanceX.Value;
            RefreshAll();
        }

        private void trackBarBorderDistanceY_Scroll(object sender, EventArgs e) {
            Settings.BorderDistanceY = trackBarBorderDistanceY.Value;
            RefreshAll();
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

        private void NumericUpDownMinimumRefreshRateMs_ValueChanged(object sender, EventArgs e) {
            Settings.MinimumRefreshRateMs = (int)numericUpDownMinimumRefreshRateMs.Value;
            // can be changed on the fly without refreshing
        }

        private DesktopDuplicatorReader _desktopDuplicatorReader;
        private CancellationTokenSource _cancellationTokenSource;

        private void RefreshCapturingState()
        {

            var isRunning = _cancellationTokenSource!=null && _desktopDuplicatorReader != null && _desktopDuplicatorReader.IsRunning;
            var shouldBeRunning = !_isSuspending && ( Settings.TransferActive || Settings.OverlayActive);

            if (isRunning && !shouldBeRunning)
            {
                //stop it!
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
                _desktopDuplicatorReader = null;
            }

            if (!isRunning && shouldBeRunning)
            {
                //start it
                _cancellationTokenSource = new CancellationTokenSource();
                _desktopDuplicatorReader = new DesktopDuplicatorReader();
                var thread = new Thread(() => _desktopDuplicatorReader.Run(_cancellationTokenSource.Token))
                {
                    IsBackground = true,Priority = ThreadPriority.BelowNormal
                };
                thread.Start();
            }
        }

        private void RefreshTransferState() {
            if (null == _mSerialStream) {
                _mSerialStream = new SerialStream();
            }

            if (Settings.TransferActive) {
                comboBoxComPort.Enabled = false;
                _mSerialStream.Start();
            } else {
                _mSerialStream.Stop();
                _mSerialStream = null;
                comboBoxComPort.Enabled = true;
            }
        }

        private void RefreshOverlayState() {
            if (null == _mOverlay) {
                _mOverlay = new Overlay();
            }

            if (Settings.OverlayActive) {
                TopMost = true;
                _mOverlay.Start();
            } else {
                _mOverlay.Stop();
                _mOverlay = null;
                TopMost = false;
            }
        }

        private void RefreshOverlay() {
            if (null != _mOverlay) {
                _mOverlay.RefreshGraphics();
            }
        }

        private void StopBackgroundWorkers() {
            if (null != _mOverlay) {
                _mOverlay.Stop();
                _mOverlay = null;
            }
            if (null != _mSerialStream) {
                _mSerialStream.Stop();
                _mSerialStream = null;
            }
        }

        private void resetOffsetXButton_Click(object sender, EventArgs e)
        {
            Settings.OffsetX = 0;
            RefreshAll();
        }

        private void resetOffsetYButton_Click(object sender, EventArgs e)
        {
            Settings.OffsetY = 0;
            RefreshAll();
        }

        private void rbLinearLighting_CheckedChanged(object sender, EventArgs e)
        {
            Settings.UseLinearLighting = rbLinearLighting.Checked;
            _log.Debug("UseLinearLighting changed to {0}", Settings.UseLinearLighting);
        }
    }
}
