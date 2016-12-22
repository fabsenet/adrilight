namespace adrilight
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                //todo
                //mDxScreenCapture.Dispose();
                _mSerialStream.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBoxSpots = new System.Windows.Forms.GroupBox();
            this.resetOffsetXButton = new System.Windows.Forms.Button();
            this.resetOffsetYButton = new System.Windows.Forms.Button();
            this.numericUpDownSpotsY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSpotsX = new System.Windows.Forms.NumericUpDown();
            this.labelOffsetY = new System.Windows.Forms.Label();
            this.labelOffsetX = new System.Windows.Forms.Label();
            this.trackBarOffsetY = new System.Windows.Forms.TrackBar();
            this.trackBarOffsetX = new System.Windows.Forms.TrackBar();
            this.trackBarBorderDistanceY = new System.Windows.Forms.TrackBar();
            this.labelBorderDistanceY = new System.Windows.Forms.Label();
            this.trackBarBorderDistanceX = new System.Windows.Forms.TrackBar();
            this.labelBorderDistanceX = new System.Windows.Forms.Label();
            this.trackBarSpotHeight = new System.Windows.Forms.TrackBar();
            this.labelSpotHeight = new System.Windows.Forms.Label();
            this.trackBarSpotWidth = new System.Windows.Forms.TrackBar();
            this.labelSpotWidth = new System.Windows.Forms.Label();
            this.labelSpotsY = new System.Windows.Forms.Label();
            this.labelSpotsX = new System.Windows.Forms.Label();
            this.numericUpDownLedsPerSpot = new System.Windows.Forms.NumericUpDown();
            this.labelLedsPerSpot = new System.Windows.Forms.Label();
            this.labelComPort = new System.Windows.Forms.Label();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.numericUpDownSaturationTreshold = new System.Windows.Forms.NumericUpDown();
            this.groupBoxLEDs = new System.Windows.Forms.GroupBox();
            this.labelMinimumRefreshRateMs = new System.Windows.Forms.Label();
            this.numericUpDownMinimumRefreshRateMs = new System.Windows.Forms.NumericUpDown();
            this.labelSaturationTreshold = new System.Windows.Forms.Label();
            this.labelLedOffset = new System.Windows.Forms.Label();
            this.numericUpDownLedOffset = new System.Windows.Forms.NumericUpDown();
            this.checkBoxMirrorY = new System.Windows.Forms.CheckBox();
            this.checkBoxMirrorX = new System.Windows.Forms.CheckBox();
            this.groupBoxTransfer = new System.Windows.Forms.GroupBox();
            this.checkBoxTransferActive = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlayActive = new System.Windows.Forms.CheckBox();
            this.groupBoxRun = new System.Windows.Forms.GroupBox();
            this.checkBoxAutostart = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbLinearLighting = new System.Windows.Forms.RadioButton();
            this.rbNonLinearLighting = new System.Windows.Forms.RadioButton();
            this.groupBoxSpots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedsPerSpot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSaturationTreshold)).BeginInit();
            this.groupBoxLEDs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumRefreshRateMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedOffset)).BeginInit();
            this.groupBoxTransfer.SuspendLayout();
            this.groupBoxRun.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSpots
            // 
            this.groupBoxSpots.Controls.Add(this.resetOffsetYButton);
            this.groupBoxSpots.Controls.Add(this.trackBarOffsetY);
            this.groupBoxSpots.Controls.Add(this.resetOffsetXButton);
            this.groupBoxSpots.Controls.Add(this.numericUpDownSpotsY);
            this.groupBoxSpots.Controls.Add(this.numericUpDownSpotsX);
            this.groupBoxSpots.Controls.Add(this.labelOffsetY);
            this.groupBoxSpots.Controls.Add(this.labelOffsetX);
            this.groupBoxSpots.Controls.Add(this.trackBarOffsetX);
            this.groupBoxSpots.Controls.Add(this.trackBarBorderDistanceY);
            this.groupBoxSpots.Controls.Add(this.labelBorderDistanceY);
            this.groupBoxSpots.Controls.Add(this.trackBarBorderDistanceX);
            this.groupBoxSpots.Controls.Add(this.labelBorderDistanceX);
            this.groupBoxSpots.Controls.Add(this.trackBarSpotHeight);
            this.groupBoxSpots.Controls.Add(this.labelSpotHeight);
            this.groupBoxSpots.Controls.Add(this.trackBarSpotWidth);
            this.groupBoxSpots.Controls.Add(this.labelSpotWidth);
            this.groupBoxSpots.Controls.Add(this.labelSpotsY);
            this.groupBoxSpots.Controls.Add(this.labelSpotsX);
            this.groupBoxSpots.Location = new System.Drawing.Point(18, 18);
            this.groupBoxSpots.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSpots.Name = "groupBoxSpots";
            this.groupBoxSpots.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSpots.Size = new System.Drawing.Size(362, 456);
            this.groupBoxSpots.TabIndex = 6;
            this.groupBoxSpots.TabStop = false;
            this.groupBoxSpots.Text = "Spots";
            // 
            // resetOffsetXButton
            // 
            this.resetOffsetXButton.Location = new System.Drawing.Point(194, 325);
            this.resetOffsetXButton.Name = "resetOffsetXButton";
            this.resetOffsetXButton.Size = new System.Drawing.Size(27, 38);
            this.resetOffsetXButton.TabIndex = 38;
            this.resetOffsetXButton.Text = "0";
            this.resetOffsetXButton.UseVisualStyleBackColor = true;
            this.resetOffsetXButton.Click += new System.EventHandler(this.resetOffsetXButton_Click);
            // 
            // resetOffsetYButton
            // 
            this.resetOffsetYButton.Location = new System.Drawing.Point(194, 380);
            this.resetOffsetYButton.Name = "resetOffsetYButton";
            this.resetOffsetYButton.Size = new System.Drawing.Size(27, 38);
            this.resetOffsetYButton.TabIndex = 38;
            this.resetOffsetYButton.Text = "0";
            this.resetOffsetYButton.UseVisualStyleBackColor = true;
            this.resetOffsetYButton.Click += new System.EventHandler(this.resetOffsetYButton_Click);
            // 
            // numericUpDownSpotsY
            // 
            this.numericUpDownSpotsY.Location = new System.Drawing.Point(213, 72);
            this.numericUpDownSpotsY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownSpotsY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpotsY.Name = "numericUpDownSpotsY";
            this.numericUpDownSpotsY.Size = new System.Drawing.Size(140, 26);
            this.numericUpDownSpotsY.TabIndex = 2;
            this.numericUpDownSpotsY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpotsY.ValueChanged += new System.EventHandler(this.numericUpDownSpotsY_ValueChanged);
            // 
            // numericUpDownSpotsX
            // 
            this.numericUpDownSpotsX.Location = new System.Drawing.Point(213, 32);
            this.numericUpDownSpotsX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownSpotsX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpotsX.Name = "numericUpDownSpotsX";
            this.numericUpDownSpotsX.Size = new System.Drawing.Size(140, 26);
            this.numericUpDownSpotsX.TabIndex = 1;
            this.numericUpDownSpotsX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpotsX.ValueChanged += new System.EventHandler(this.numericUpDownSpotsX_ValueChanged);
            // 
            // labelOffsetY
            // 
            this.labelOffsetY.AutoSize = true;
            this.labelOffsetY.Location = new System.Drawing.Point(14, 389);
            this.labelOffsetY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOffsetY.Name = "labelOffsetY";
            this.labelOffsetY.Size = new System.Drawing.Size(68, 20);
            this.labelOffsetY.TabIndex = 37;
            this.labelOffsetY.Text = "Offset Y";
            // 
            // labelOffsetX
            // 
            this.labelOffsetX.AutoSize = true;
            this.labelOffsetX.Location = new System.Drawing.Point(14, 334);
            this.labelOffsetX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOffsetX.Name = "labelOffsetX";
            this.labelOffsetX.Size = new System.Drawing.Size(68, 20);
            this.labelOffsetX.TabIndex = 36;
            this.labelOffsetX.Text = "Offset X";
            // 
            // trackBarOffsetY
            // 
            this.trackBarOffsetY.LargeChange = 10;
            this.trackBarOffsetY.Location = new System.Drawing.Point(213, 389);
            this.trackBarOffsetY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarOffsetY.Maximum = 0;
            this.trackBarOffsetY.Name = "trackBarOffsetY";
            this.trackBarOffsetY.Size = new System.Drawing.Size(140, 69);
            this.trackBarOffsetY.TabIndex = 8;
            this.trackBarOffsetY.Scroll += new System.EventHandler(this.trackBarOffsetY_Scroll);
            // 
            // trackBarOffsetX
            // 
            this.trackBarOffsetX.LargeChange = 10;
            this.trackBarOffsetX.Location = new System.Drawing.Point(213, 334);
            this.trackBarOffsetX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarOffsetX.Maximum = 0;
            this.trackBarOffsetX.Name = "trackBarOffsetX";
            this.trackBarOffsetX.Size = new System.Drawing.Size(140, 69);
            this.trackBarOffsetX.TabIndex = 7;
            this.trackBarOffsetX.Scroll += new System.EventHandler(this.trackBarOffsetX_Scroll);
            // 
            // trackBarBorderDistanceY
            // 
            this.trackBarBorderDistanceY.LargeChange = 10;
            this.trackBarBorderDistanceY.Location = new System.Drawing.Point(213, 278);
            this.trackBarBorderDistanceY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarBorderDistanceY.Maximum = 250;
            this.trackBarBorderDistanceY.Name = "trackBarBorderDistanceY";
            this.trackBarBorderDistanceY.Size = new System.Drawing.Size(140, 69);
            this.trackBarBorderDistanceY.TabIndex = 6;
            this.trackBarBorderDistanceY.Value = 32;
            this.trackBarBorderDistanceY.Scroll += new System.EventHandler(this.trackBarBorderDistanceY_Scroll);
            // 
            // labelBorderDistanceY
            // 
            this.labelBorderDistanceY.AutoSize = true;
            this.labelBorderDistanceY.Location = new System.Drawing.Point(9, 278);
            this.labelBorderDistanceY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBorderDistanceY.Name = "labelBorderDistanceY";
            this.labelBorderDistanceY.Size = new System.Drawing.Size(139, 20);
            this.labelBorderDistanceY.TabIndex = 35;
            this.labelBorderDistanceY.Text = "Border Distance Y";
            // 
            // trackBarBorderDistanceX
            // 
            this.trackBarBorderDistanceX.LargeChange = 10;
            this.trackBarBorderDistanceX.Location = new System.Drawing.Point(213, 223);
            this.trackBarBorderDistanceX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarBorderDistanceX.Maximum = 250;
            this.trackBarBorderDistanceX.Name = "trackBarBorderDistanceX";
            this.trackBarBorderDistanceX.Size = new System.Drawing.Size(140, 69);
            this.trackBarBorderDistanceX.TabIndex = 5;
            this.trackBarBorderDistanceX.Value = 32;
            this.trackBarBorderDistanceX.Scroll += new System.EventHandler(this.trackBarBorderDistanceX_Scroll);
            // 
            // labelBorderDistanceX
            // 
            this.labelBorderDistanceX.AutoSize = true;
            this.labelBorderDistanceX.Location = new System.Drawing.Point(9, 223);
            this.labelBorderDistanceX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBorderDistanceX.Name = "labelBorderDistanceX";
            this.labelBorderDistanceX.Size = new System.Drawing.Size(139, 20);
            this.labelBorderDistanceX.TabIndex = 33;
            this.labelBorderDistanceX.Text = "Border Distance X";
            // 
            // trackBarSpotHeight
            // 
            this.trackBarSpotHeight.LargeChange = 10;
            this.trackBarSpotHeight.Location = new System.Drawing.Point(213, 168);
            this.trackBarSpotHeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarSpotHeight.Maximum = 32;
            this.trackBarSpotHeight.Minimum = 32;
            this.trackBarSpotHeight.Name = "trackBarSpotHeight";
            this.trackBarSpotHeight.Size = new System.Drawing.Size(140, 69);
            this.trackBarSpotHeight.TabIndex = 4;
            this.trackBarSpotHeight.Value = 32;
            this.trackBarSpotHeight.Scroll += new System.EventHandler(this.trackBarSpotHeight_Scroll);
            // 
            // labelSpotHeight
            // 
            this.labelSpotHeight.AutoSize = true;
            this.labelSpotHeight.Location = new System.Drawing.Point(9, 168);
            this.labelSpotHeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSpotHeight.Name = "labelSpotHeight";
            this.labelSpotHeight.Size = new System.Drawing.Size(94, 20);
            this.labelSpotHeight.TabIndex = 27;
            this.labelSpotHeight.Text = "Spot Height";
            // 
            // trackBarSpotWidth
            // 
            this.trackBarSpotWidth.LargeChange = 10;
            this.trackBarSpotWidth.Location = new System.Drawing.Point(213, 112);
            this.trackBarSpotWidth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarSpotWidth.Maximum = 32;
            this.trackBarSpotWidth.Minimum = 32;
            this.trackBarSpotWidth.Name = "trackBarSpotWidth";
            this.trackBarSpotWidth.Size = new System.Drawing.Size(140, 69);
            this.trackBarSpotWidth.TabIndex = 3;
            this.trackBarSpotWidth.Value = 32;
            this.trackBarSpotWidth.Scroll += new System.EventHandler(this.trackBarSpotWidth_Scroll);
            // 
            // labelSpotWidth
            // 
            this.labelSpotWidth.AutoSize = true;
            this.labelSpotWidth.Location = new System.Drawing.Point(9, 112);
            this.labelSpotWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSpotWidth.Name = "labelSpotWidth";
            this.labelSpotWidth.Size = new System.Drawing.Size(88, 20);
            this.labelSpotWidth.TabIndex = 26;
            this.labelSpotWidth.Text = "Spot Width";
            // 
            // labelSpotsY
            // 
            this.labelSpotsY.AutoSize = true;
            this.labelSpotsY.Location = new System.Drawing.Point(9, 75);
            this.labelSpotsY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSpotsY.Name = "labelSpotsY";
            this.labelSpotsY.Size = new System.Drawing.Size(66, 20);
            this.labelSpotsY.TabIndex = 11;
            this.labelSpotsY.Text = "Spots Y";
            // 
            // labelSpotsX
            // 
            this.labelSpotsX.AutoSize = true;
            this.labelSpotsX.Location = new System.Drawing.Point(9, 35);
            this.labelSpotsX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSpotsX.Name = "labelSpotsX";
            this.labelSpotsX.Size = new System.Drawing.Size(66, 20);
            this.labelSpotsX.TabIndex = 10;
            this.labelSpotsX.Text = "Spots X";
            // 
            // numericUpDownLedsPerSpot
            // 
            this.numericUpDownLedsPerSpot.Location = new System.Drawing.Point(208, 31);
            this.numericUpDownLedsPerSpot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownLedsPerSpot.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLedsPerSpot.Name = "numericUpDownLedsPerSpot";
            this.numericUpDownLedsPerSpot.Size = new System.Drawing.Size(140, 26);
            this.numericUpDownLedsPerSpot.TabIndex = 10;
            this.numericUpDownLedsPerSpot.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLedsPerSpot.ValueChanged += new System.EventHandler(this.numericUpDownLedsPerSpot_ValueChanged);
            // 
            // labelLedsPerSpot
            // 
            this.labelLedsPerSpot.AutoSize = true;
            this.labelLedsPerSpot.Location = new System.Drawing.Point(9, 34);
            this.labelLedsPerSpot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLedsPerSpot.Name = "labelLedsPerSpot";
            this.labelLedsPerSpot.Size = new System.Drawing.Size(114, 20);
            this.labelLedsPerSpot.TabIndex = 13;
            this.labelLedsPerSpot.Text = "LEDs per Spot";
            // 
            // labelComPort
            // 
            this.labelComPort.AutoSize = true;
            this.labelComPort.Location = new System.Drawing.Point(9, 40);
            this.labelComPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(78, 20);
            this.labelComPort.TabIndex = 17;
            this.labelComPort.Text = "COM Port";
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(213, 32);
            this.comboBoxComPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(138, 28);
            this.comboBoxComPort.TabIndex = 9;
            this.comboBoxComPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPort_SelectedIndexChanged);
            // 
            // numericUpDownSaturationTreshold
            // 
            this.numericUpDownSaturationTreshold.Location = new System.Drawing.Point(208, 72);
            this.numericUpDownSaturationTreshold.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownSaturationTreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSaturationTreshold.Name = "numericUpDownSaturationTreshold";
            this.numericUpDownSaturationTreshold.Size = new System.Drawing.Size(140, 26);
            this.numericUpDownSaturationTreshold.TabIndex = 11;
            this.numericUpDownSaturationTreshold.ValueChanged += new System.EventHandler(this.numericUpDownSaturationTreshold_ValueChanged);
            // 
            // groupBoxLEDs
            // 
            this.groupBoxLEDs.Controls.Add(this.labelMinimumRefreshRateMs);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownMinimumRefreshRateMs);
            this.groupBoxLEDs.Controls.Add(this.labelSaturationTreshold);
            this.groupBoxLEDs.Controls.Add(this.labelLedOffset);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownLedOffset);
            this.groupBoxLEDs.Controls.Add(this.checkBoxMirrorY);
            this.groupBoxLEDs.Controls.Add(this.checkBoxMirrorX);
            this.groupBoxLEDs.Controls.Add(this.labelLedsPerSpot);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownLedsPerSpot);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownSaturationTreshold);
            this.groupBoxLEDs.Location = new System.Drawing.Point(388, 115);
            this.groupBoxLEDs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxLEDs.Name = "groupBoxLEDs";
            this.groupBoxLEDs.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxLEDs.Size = new System.Drawing.Size(362, 238);
            this.groupBoxLEDs.TabIndex = 23;
            this.groupBoxLEDs.TabStop = false;
            this.groupBoxLEDs.Text = "LEDs";
            // 
            // labelMinimumRefreshRateMs
            // 
            this.labelMinimumRefreshRateMs.AutoSize = true;
            this.labelMinimumRefreshRateMs.Location = new System.Drawing.Point(9, 155);
            this.labelMinimumRefreshRateMs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinimumRefreshRateMs.Name = "labelMinimumRefreshRateMs";
            this.labelMinimumRefreshRateMs.Size = new System.Drawing.Size(173, 20);
            this.labelMinimumRefreshRateMs.TabIndex = 29;
            this.labelMinimumRefreshRateMs.Text = "Min. Refresh Rate (ms)";
            // 
            // numericUpDownMinimumRefreshRateMs
            // 
            this.numericUpDownMinimumRefreshRateMs.Location = new System.Drawing.Point(208, 152);
            this.numericUpDownMinimumRefreshRateMs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownMinimumRefreshRateMs.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinimumRefreshRateMs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMinimumRefreshRateMs.Name = "numericUpDownMinimumRefreshRateMs";
            this.numericUpDownMinimumRefreshRateMs.Size = new System.Drawing.Size(140, 26);
            this.numericUpDownMinimumRefreshRateMs.TabIndex = 13;
            this.numericUpDownMinimumRefreshRateMs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMinimumRefreshRateMs.ValueChanged += new System.EventHandler(this.NumericUpDownMinimumRefreshRateMs_ValueChanged);
            // 
            // labelSaturationTreshold
            // 
            this.labelSaturationTreshold.AutoSize = true;
            this.labelSaturationTreshold.Location = new System.Drawing.Point(9, 75);
            this.labelSaturationTreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSaturationTreshold.Name = "labelSaturationTreshold";
            this.labelSaturationTreshold.Size = new System.Drawing.Size(148, 20);
            this.labelSaturationTreshold.TabIndex = 27;
            this.labelSaturationTreshold.Text = "Saturation Treshold";
            // 
            // labelLedOffset
            // 
            this.labelLedOffset.AutoSize = true;
            this.labelLedOffset.Location = new System.Drawing.Point(9, 115);
            this.labelLedOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLedOffset.Name = "labelLedOffset";
            this.labelLedOffset.Size = new System.Drawing.Size(53, 20);
            this.labelLedOffset.TabIndex = 26;
            this.labelLedOffset.Text = "Offset";
            // 
            // numericUpDownLedOffset
            // 
            this.numericUpDownLedOffset.Location = new System.Drawing.Point(208, 112);
            this.numericUpDownLedOffset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownLedOffset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownLedOffset.Name = "numericUpDownLedOffset";
            this.numericUpDownLedOffset.Size = new System.Drawing.Size(140, 26);
            this.numericUpDownLedOffset.TabIndex = 12;
            this.numericUpDownLedOffset.ValueChanged += new System.EventHandler(this.numericUpDownLedOffset_ValueChanged);
            // 
            // checkBoxMirrorY
            // 
            this.checkBoxMirrorY.AutoSize = true;
            this.checkBoxMirrorY.Location = new System.Drawing.Point(208, 203);
            this.checkBoxMirrorY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxMirrorY.Name = "checkBoxMirrorY";
            this.checkBoxMirrorY.Size = new System.Drawing.Size(124, 24);
            this.checkBoxMirrorY.TabIndex = 15;
            this.checkBoxMirrorY.Text = "Mirror Y-Axis";
            this.checkBoxMirrorY.UseVisualStyleBackColor = true;
            this.checkBoxMirrorY.CheckedChanged += new System.EventHandler(this.checkBoxMirrorY_CheckedChanged);
            // 
            // checkBoxMirrorX
            // 
            this.checkBoxMirrorX.AutoSize = true;
            this.checkBoxMirrorX.Location = new System.Drawing.Point(14, 203);
            this.checkBoxMirrorX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxMirrorX.Name = "checkBoxMirrorX";
            this.checkBoxMirrorX.Size = new System.Drawing.Size(124, 24);
            this.checkBoxMirrorX.TabIndex = 14;
            this.checkBoxMirrorX.Text = "Mirror X-Axis";
            this.checkBoxMirrorX.UseVisualStyleBackColor = true;
            this.checkBoxMirrorX.CheckedChanged += new System.EventHandler(this.checkBoxMirrorX_CheckedChanged);
            // 
            // groupBoxTransfer
            // 
            this.groupBoxTransfer.Controls.Add(this.comboBoxComPort);
            this.groupBoxTransfer.Controls.Add(this.labelComPort);
            this.groupBoxTransfer.Location = new System.Drawing.Point(388, 18);
            this.groupBoxTransfer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxTransfer.Name = "groupBoxTransfer";
            this.groupBoxTransfer.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxTransfer.Size = new System.Drawing.Size(362, 88);
            this.groupBoxTransfer.TabIndex = 24;
            this.groupBoxTransfer.TabStop = false;
            this.groupBoxTransfer.Text = "Serial Transfer";
            // 
            // checkBoxTransferActive
            // 
            this.checkBoxTransferActive.AutoSize = true;
            this.checkBoxTransferActive.Location = new System.Drawing.Point(14, 29);
            this.checkBoxTransferActive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxTransferActive.Name = "checkBoxTransferActive";
            this.checkBoxTransferActive.Size = new System.Drawing.Size(121, 24);
            this.checkBoxTransferActive.TabIndex = 16;
            this.checkBoxTransferActive.Text = "LED-Output";
            this.checkBoxTransferActive.UseVisualStyleBackColor = true;
            this.checkBoxTransferActive.CheckedChanged += new System.EventHandler(this.checkBoxTransferActive_CheckedChanged);
            // 
            // checkBoxOverlayActive
            // 
            this.checkBoxOverlayActive.AutoSize = true;
            this.checkBoxOverlayActive.Location = new System.Drawing.Point(207, 29);
            this.checkBoxOverlayActive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxOverlayActive.Name = "checkBoxOverlayActive";
            this.checkBoxOverlayActive.Size = new System.Drawing.Size(123, 24);
            this.checkBoxOverlayActive.TabIndex = 17;
            this.checkBoxOverlayActive.Text = "Test-Overlay";
            this.checkBoxOverlayActive.UseVisualStyleBackColor = true;
            this.checkBoxOverlayActive.CheckedChanged += new System.EventHandler(this.checkBoxOverlayActive_CheckedChanged);
            // 
            // groupBoxRun
            // 
            this.groupBoxRun.Controls.Add(this.checkBoxAutostart);
            this.groupBoxRun.Controls.Add(this.checkBoxStartMinimized);
            this.groupBoxRun.Controls.Add(this.checkBoxTransferActive);
            this.groupBoxRun.Controls.Add(this.checkBoxOverlayActive);
            this.groupBoxRun.Location = new System.Drawing.Point(388, 363);
            this.groupBoxRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxRun.Name = "groupBoxRun";
            this.groupBoxRun.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxRun.Size = new System.Drawing.Size(360, 111);
            this.groupBoxRun.TabIndex = 25;
            this.groupBoxRun.TabStop = false;
            this.groupBoxRun.Text = "Control";
            // 
            // checkBoxAutostart
            // 
            this.checkBoxAutostart.AutoSize = true;
            this.checkBoxAutostart.Location = new System.Drawing.Point(207, 65);
            this.checkBoxAutostart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxAutostart.Name = "checkBoxAutostart";
            this.checkBoxAutostart.Size = new System.Drawing.Size(101, 24);
            this.checkBoxAutostart.TabIndex = 19;
            this.checkBoxAutostart.Text = "Autostart";
            this.checkBoxAutostart.UseVisualStyleBackColor = true;
            this.checkBoxAutostart.CheckedChanged += new System.EventHandler(this.checkBoxAutostart_CheckedChanged);
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(14, 65);
            this.checkBoxStartMinimized.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(144, 24);
            this.checkBoxStartMinimized.TabIndex = 18;
            this.checkBoxStartMinimized.Text = "Start Minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            this.checkBoxStartMinimized.CheckedChanged += new System.EventHandler(this.checkBoxStartMinimized_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNonLinearLighting);
            this.groupBox1.Controls.Add(this.rbLinearLighting);
            this.groupBox1.Location = new System.Drawing.Point(388, 482);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 74);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lighting Mode";
            // 
            // rbLinearLighting
            // 
            this.rbLinearLighting.AutoSize = true;
            this.rbLinearLighting.Location = new System.Drawing.Point(14, 26);
            this.rbLinearLighting.Name = "rbLinearLighting";
            this.rbLinearLighting.Size = new System.Drawing.Size(138, 24);
            this.rbLinearLighting.TabIndex = 0;
            this.rbLinearLighting.TabStop = true;
            this.rbLinearLighting.Text = "Linear Lighting";
            this.rbLinearLighting.UseVisualStyleBackColor = true;
            this.rbLinearLighting.CheckedChanged += new System.EventHandler(this.rbLinearLighting_CheckedChanged);
            // 
            // rbNonLinearLighting
            // 
            this.rbNonLinearLighting.AutoSize = true;
            this.rbNonLinearLighting.Location = new System.Drawing.Point(194, 25);
            this.rbNonLinearLighting.Name = "rbNonLinearLighting";
            this.rbNonLinearLighting.Size = new System.Drawing.Size(154, 24);
            this.rbNonLinearLighting.TabIndex = 1;
            this.rbNonLinearLighting.TabStop = true;
            this.rbNonLinearLighting.Text = "Non-linear fading";
            this.rbNonLinearLighting.UseVisualStyleBackColor = true;
            this.rbNonLinearLighting.CheckedChanged += new System.EventHandler(this.rbLinearLighting_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(762, 568);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxRun);
            this.Controls.Add(this.groupBoxTransfer);
            this.Controls.Add(this.groupBoxLEDs);
            this.Controls.Add(this.groupBoxSpots);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(784, 624);
            this.MinimumSize = new System.Drawing.Size(784, 524);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "adrilight";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBoxSpots.ResumeLayout(false);
            this.groupBoxSpots.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedsPerSpot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSaturationTreshold)).EndInit();
            this.groupBoxLEDs.ResumeLayout(false);
            this.groupBoxLEDs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumRefreshRateMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedOffset)).EndInit();
            this.groupBoxTransfer.ResumeLayout(false);
            this.groupBoxTransfer.PerformLayout();
            this.groupBoxRun.ResumeLayout(false);
            this.groupBoxRun.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxSpots;
        private System.Windows.Forms.Label labelSpotsY;
        private System.Windows.Forms.Label labelSpotsX;
        private System.Windows.Forms.NumericUpDown numericUpDownLedsPerSpot;
        private System.Windows.Forms.Label labelLedsPerSpot;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label labelComPort;
        private System.Windows.Forms.NumericUpDown numericUpDownSaturationTreshold;
        private System.Windows.Forms.Label labelSpotHeight;
        private System.Windows.Forms.Label labelSpotWidth;
        private System.Windows.Forms.TrackBar trackBarSpotWidth;
        private System.Windows.Forms.TrackBar trackBarSpotHeight;
        private System.Windows.Forms.GroupBox groupBoxLEDs;
        private System.Windows.Forms.GroupBox groupBoxTransfer;
        private System.Windows.Forms.Label labelSaturationTreshold;
        private System.Windows.Forms.Label labelLedOffset;
        private System.Windows.Forms.NumericUpDown numericUpDownLedOffset;
        private System.Windows.Forms.CheckBox checkBoxMirrorY;
        private System.Windows.Forms.CheckBox checkBoxMirrorX;
        private System.Windows.Forms.CheckBox checkBoxTransferActive;
        private System.Windows.Forms.CheckBox checkBoxOverlayActive;
        private System.Windows.Forms.TrackBar trackBarBorderDistanceX;
        private System.Windows.Forms.Label labelBorderDistanceX;
        private System.Windows.Forms.GroupBox groupBoxRun;
        private System.Windows.Forms.NumericUpDown numericUpDownSpotsY;
        private System.Windows.Forms.NumericUpDown numericUpDownSpotsX;
        private System.Windows.Forms.Label labelOffsetY;
        private System.Windows.Forms.Label labelOffsetX;
        private System.Windows.Forms.TrackBar trackBarOffsetY;
        private System.Windows.Forms.TrackBar trackBarOffsetX;
        private System.Windows.Forms.TrackBar trackBarBorderDistanceY;
        private System.Windows.Forms.Label labelBorderDistanceY;
        private System.Windows.Forms.CheckBox checkBoxAutostart;
        private System.Windows.Forms.CheckBox checkBoxStartMinimized;
        private System.Windows.Forms.Label labelMinimumRefreshRateMs;
        private System.Windows.Forms.NumericUpDown numericUpDownMinimumRefreshRateMs;
        private System.Windows.Forms.Button resetOffsetXButton;
        private System.Windows.Forms.Button resetOffsetYButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbNonLinearLighting;
        private System.Windows.Forms.RadioButton rbLinearLighting;
    }
}

