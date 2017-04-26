using System;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO.Ports;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NLog;
using Semver;
using System.Reflection;
using System.IO;

namespace adrilight {

    public partial class MainForm : Form
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        SerialStream _mSerialStream;
        Overlay _mOverlay;

        public MainForm() {
            _log.Debug("Creating Mainform");
            InitializeComponent();
            Text += " " + Program.VersionNumber;

            Settings.OverlayActive = checkBoxOverlayActive.Checked;

            Task.Run(() => StartVersionCheck());
            _log.Debug("Created Mainform");
        }

        private async Task StartVersionCheck()
        {
            //avoid too many checks
            if (Settings.LastUpdateCheck.HasValue && Settings.LastUpdateCheck > DateTime.UtcNow.AddHours(-8)) return;
            
            try
            {
                var latestRelease = await TryGetLatestReleaseData();
                if (latestRelease == null) return;

                string tagName = latestRelease.tag_name;
                var latestVersionNumber = SemVersion.Parse(tagName.TrimStart('v', 'V'));

                Settings.LastUpdateCheck = DateTime.UtcNow;

                if (latestVersionNumber > Program.VersionNumber)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        var url = latestRelease.html_url as string;
                        var shouldOpenUrl = MessageBox.Show($"New version of adrilight is available! The new version is {latestVersionNumber} (you are running {Program.VersionNumber}). Press OK to open the download page!"
                             , "New Adrilight Version!", MessageBoxButtons.OKCancel) == DialogResult.OK;

                        if (url != null && shouldOpenUrl)
                        {
                            Process.Start(url);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _log.Warn(ex, "Something failed in StartVersionCheck()");
                throw;
            }
        }
        private class GithubReleaseData
        {
            public string tag_name { get; set; }
            public string html_url { get; set; }
        }

        private async Task<GithubReleaseData> TryGetLatestReleaseData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "fabsenet/adrilight");

                    var jsonString = await client.GetStringAsync("https://api.github.com/repos/fabsenet/adrilight/releases/latest");
                    var data = JsonConvert.DeserializeObject<GithubReleaseData>(jsonString);
                    return data;
                }
            }
            catch (Exception ex)
            {
                _log.Info(ex, "Check for latest release failed.");
                return null;
            }
        }
        
        private void MainForm_Load(object sender, EventArgs e) {
            InitTags();
            InitFieldLimits();

            RefreshFields();
            RefreshAll();

            RefreshCapturingState();
            RefreshOverlayState();
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
        
        private DesktopDuplicatorReader _desktopDuplicatorReader;
        private CancellationTokenSource _cancellationTokenSource;

        private void RefreshCapturingState()
        {

            var isRunning = _cancellationTokenSource!=null && _desktopDuplicatorReader != null && _desktopDuplicatorReader.IsRunning;
            var shouldBeRunning = Settings.TransferActive || Settings.OverlayActive;

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
