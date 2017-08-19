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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBoxSpots = new System.Windows.Forms.GroupBox();
            this.resetOffsetYButton = new System.Windows.Forms.Button();
            this.trackBarOffsetY = new System.Windows.Forms.TrackBar();
            this.resetOffsetXButton = new System.Windows.Forms.Button();
            this.numericUpDownSpotsY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSpotsX = new System.Windows.Forms.NumericUpDown();
            this.labelOffsetY = new System.Windows.Forms.Label();
            this.labelOffsetX = new System.Windows.Forms.Label();
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
            this.checkBoxReverse = new System.Windows.Forms.CheckBox();
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
            this.rbNonLinearLighting = new System.Windows.Forms.RadioButton();
            this.rbLinearLighting = new System.Windows.Forms.RadioButton();
            this.timerFPS = new System.Windows.Forms.Timer(this.components);
            this.groupBoxDebug = new System.Windows.Forms.GroupBox();
            this.checkBoxLogging = new System.Windows.Forms.CheckBox();
            this.groupBoxWhiteBalance = new System.Windows.Forms.GroupBox();
            this.trackBarWhiteBalanceR = new System.Windows.Forms.TrackBar();
            this.trackBarWhiteBalanceG = new System.Windows.Forms.TrackBar();
            this.trackBarWhiteBalanceB = new System.Windows.Forms.TrackBar();
            this.labelWhiteBalanceR = new System.Windows.Forms.Label();
            this.labelWhiteBalanceG = new System.Windows.Forms.Label();
            this.labelWhiteBalanceB = new System.Windows.Forms.Label();
            this.labelBrightness = new System.Windows.Forms.Label();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.buttonWhiteZero = new System.Windows.Forms.Button();
            this.buttonWhiteSave = new System.Windows.Forms.Button();
            this.groupBoxSpots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedsPerSpot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSaturationTreshold)).BeginInit();
            this.groupBoxLEDs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedOffset)).BeginInit();
            this.groupBoxTransfer.SuspendLayout();
            this.groupBoxRun.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxDebug.SuspendLayout();
            this.groupBoxWhiteBalance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWhiteBalanceR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWhiteBalanceG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWhiteBalanceB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
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
            this.groupBoxSpots.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSpots.Name = "groupBoxSpots";
            this.groupBoxSpots.Size = new System.Drawing.Size(241, 323);
            this.groupBoxSpots.TabIndex = 6;
            this.groupBoxSpots.TabStop = false;
            this.groupBoxSpots.Text = "Spots";
            // 
            // resetOffsetYButton
            // 
            this.resetOffsetYButton.Location = new System.Drawing.Point(129, 247);
            this.resetOffsetYButton.Margin = new System.Windows.Forms.Padding(2);
            this.resetOffsetYButton.Name = "resetOffsetYButton";
            this.resetOffsetYButton.Size = new System.Drawing.Size(18, 25);
            this.resetOffsetYButton.TabIndex = 38;
            this.resetOffsetYButton.Text = "0";
            this.resetOffsetYButton.UseVisualStyleBackColor = true;
            this.resetOffsetYButton.Click += new System.EventHandler(this.resetOffsetYButton_Click);
            // 
            // trackBarOffsetY
            // 
            this.trackBarOffsetY.LargeChange = 10;
            this.trackBarOffsetY.Location = new System.Drawing.Point(142, 253);
            this.trackBarOffsetY.Maximum = 0;
            this.trackBarOffsetY.Name = "trackBarOffsetY";
            this.trackBarOffsetY.Size = new System.Drawing.Size(93, 45);
            this.trackBarOffsetY.TabIndex = 8;
            this.trackBarOffsetY.Scroll += new System.EventHandler(this.trackBarOffsetY_Scroll);
            // 
            // resetOffsetXButton
            // 
            this.resetOffsetXButton.Location = new System.Drawing.Point(129, 211);
            this.resetOffsetXButton.Margin = new System.Windows.Forms.Padding(2);
            this.resetOffsetXButton.Name = "resetOffsetXButton";
            this.resetOffsetXButton.Size = new System.Drawing.Size(18, 25);
            this.resetOffsetXButton.TabIndex = 38;
            this.resetOffsetXButton.Text = "0";
            this.resetOffsetXButton.UseVisualStyleBackColor = true;
            this.resetOffsetXButton.Click += new System.EventHandler(this.resetOffsetXButton_Click);
            // 
            // numericUpDownSpotsY
            // 
            this.numericUpDownSpotsY.Location = new System.Drawing.Point(142, 47);
            this.numericUpDownSpotsY.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownSpotsY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpotsY.Name = "numericUpDownSpotsY";
            this.numericUpDownSpotsY.Size = new System.Drawing.Size(93, 20);
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
            this.numericUpDownSpotsX.Location = new System.Drawing.Point(142, 21);
            this.numericUpDownSpotsX.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownSpotsX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpotsX.Name = "numericUpDownSpotsX";
            this.numericUpDownSpotsX.Size = new System.Drawing.Size(93, 20);
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
            this.labelOffsetY.Location = new System.Drawing.Point(9, 253);
            this.labelOffsetY.Name = "labelOffsetY";
            this.labelOffsetY.Size = new System.Drawing.Size(45, 13);
            this.labelOffsetY.TabIndex = 37;
            this.labelOffsetY.Text = "Offset Y";
            // 
            // labelOffsetX
            // 
            this.labelOffsetX.AutoSize = true;
            this.labelOffsetX.Location = new System.Drawing.Point(9, 217);
            this.labelOffsetX.Name = "labelOffsetX";
            this.labelOffsetX.Size = new System.Drawing.Size(45, 13);
            this.labelOffsetX.TabIndex = 36;
            this.labelOffsetX.Text = "Offset X";
            // 
            // trackBarOffsetX
            // 
            this.trackBarOffsetX.LargeChange = 10;
            this.trackBarOffsetX.Location = new System.Drawing.Point(142, 217);
            this.trackBarOffsetX.Maximum = 0;
            this.trackBarOffsetX.Name = "trackBarOffsetX";
            this.trackBarOffsetX.Size = new System.Drawing.Size(93, 45);
            this.trackBarOffsetX.TabIndex = 7;
            this.trackBarOffsetX.Scroll += new System.EventHandler(this.trackBarOffsetX_Scroll);
            // 
            // trackBarBorderDistanceY
            // 
            this.trackBarBorderDistanceY.LargeChange = 10;
            this.trackBarBorderDistanceY.Location = new System.Drawing.Point(142, 181);
            this.trackBarBorderDistanceY.Maximum = 250;
            this.trackBarBorderDistanceY.Name = "trackBarBorderDistanceY";
            this.trackBarBorderDistanceY.Size = new System.Drawing.Size(93, 45);
            this.trackBarBorderDistanceY.TabIndex = 6;
            this.trackBarBorderDistanceY.Value = 32;
            this.trackBarBorderDistanceY.Scroll += new System.EventHandler(this.trackBarBorderDistanceY_Scroll);
            // 
            // labelBorderDistanceY
            // 
            this.labelBorderDistanceY.AutoSize = true;
            this.labelBorderDistanceY.Location = new System.Drawing.Point(6, 181);
            this.labelBorderDistanceY.Name = "labelBorderDistanceY";
            this.labelBorderDistanceY.Size = new System.Drawing.Size(93, 13);
            this.labelBorderDistanceY.TabIndex = 35;
            this.labelBorderDistanceY.Text = "Border Distance Y";
            // 
            // trackBarBorderDistanceX
            // 
            this.trackBarBorderDistanceX.LargeChange = 10;
            this.trackBarBorderDistanceX.Location = new System.Drawing.Point(142, 145);
            this.trackBarBorderDistanceX.Maximum = 250;
            this.trackBarBorderDistanceX.Name = "trackBarBorderDistanceX";
            this.trackBarBorderDistanceX.Size = new System.Drawing.Size(93, 45);
            this.trackBarBorderDistanceX.TabIndex = 5;
            this.trackBarBorderDistanceX.Value = 32;
            this.trackBarBorderDistanceX.Scroll += new System.EventHandler(this.trackBarBorderDistanceX_Scroll);
            // 
            // labelBorderDistanceX
            // 
            this.labelBorderDistanceX.AutoSize = true;
            this.labelBorderDistanceX.Location = new System.Drawing.Point(6, 145);
            this.labelBorderDistanceX.Name = "labelBorderDistanceX";
            this.labelBorderDistanceX.Size = new System.Drawing.Size(93, 13);
            this.labelBorderDistanceX.TabIndex = 33;
            this.labelBorderDistanceX.Text = "Border Distance X";
            // 
            // trackBarSpotHeight
            // 
            this.trackBarSpotHeight.LargeChange = 10;
            this.trackBarSpotHeight.Location = new System.Drawing.Point(142, 109);
            this.trackBarSpotHeight.Maximum = 32;
            this.trackBarSpotHeight.Minimum = 32;
            this.trackBarSpotHeight.Name = "trackBarSpotHeight";
            this.trackBarSpotHeight.Size = new System.Drawing.Size(93, 45);
            this.trackBarSpotHeight.TabIndex = 4;
            this.trackBarSpotHeight.Value = 32;
            this.trackBarSpotHeight.Scroll += new System.EventHandler(this.trackBarSpotHeight_Scroll);
            // 
            // labelSpotHeight
            // 
            this.labelSpotHeight.AutoSize = true;
            this.labelSpotHeight.Location = new System.Drawing.Point(6, 109);
            this.labelSpotHeight.Name = "labelSpotHeight";
            this.labelSpotHeight.Size = new System.Drawing.Size(63, 13);
            this.labelSpotHeight.TabIndex = 27;
            this.labelSpotHeight.Text = "Spot Height";
            // 
            // trackBarSpotWidth
            // 
            this.trackBarSpotWidth.LargeChange = 10;
            this.trackBarSpotWidth.Location = new System.Drawing.Point(142, 73);
            this.trackBarSpotWidth.Maximum = 32;
            this.trackBarSpotWidth.Minimum = 32;
            this.trackBarSpotWidth.Name = "trackBarSpotWidth";
            this.trackBarSpotWidth.Size = new System.Drawing.Size(93, 45);
            this.trackBarSpotWidth.TabIndex = 3;
            this.trackBarSpotWidth.Value = 32;
            this.trackBarSpotWidth.Scroll += new System.EventHandler(this.trackBarSpotWidth_Scroll);
            // 
            // labelSpotWidth
            // 
            this.labelSpotWidth.AutoSize = true;
            this.labelSpotWidth.Location = new System.Drawing.Point(6, 73);
            this.labelSpotWidth.Name = "labelSpotWidth";
            this.labelSpotWidth.Size = new System.Drawing.Size(60, 13);
            this.labelSpotWidth.TabIndex = 26;
            this.labelSpotWidth.Text = "Spot Width";
            // 
            // labelSpotsY
            // 
            this.labelSpotsY.AutoSize = true;
            this.labelSpotsY.Location = new System.Drawing.Point(6, 49);
            this.labelSpotsY.Name = "labelSpotsY";
            this.labelSpotsY.Size = new System.Drawing.Size(44, 13);
            this.labelSpotsY.TabIndex = 11;
            this.labelSpotsY.Text = "Spots Y";
            // 
            // labelSpotsX
            // 
            this.labelSpotsX.AutoSize = true;
            this.labelSpotsX.Location = new System.Drawing.Point(6, 23);
            this.labelSpotsX.Name = "labelSpotsX";
            this.labelSpotsX.Size = new System.Drawing.Size(44, 13);
            this.labelSpotsX.TabIndex = 10;
            this.labelSpotsX.Text = "Spots X";
            // 
            // numericUpDownLedsPerSpot
            // 
            this.numericUpDownLedsPerSpot.Location = new System.Drawing.Point(139, 20);
            this.numericUpDownLedsPerSpot.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLedsPerSpot.Name = "numericUpDownLedsPerSpot";
            this.numericUpDownLedsPerSpot.Size = new System.Drawing.Size(93, 20);
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
            this.labelLedsPerSpot.Location = new System.Drawing.Point(6, 22);
            this.labelLedsPerSpot.Name = "labelLedsPerSpot";
            this.labelLedsPerSpot.Size = new System.Drawing.Size(76, 13);
            this.labelLedsPerSpot.TabIndex = 13;
            this.labelLedsPerSpot.Text = "LEDs per Spot";
            // 
            // labelComPort
            // 
            this.labelComPort.AutoSize = true;
            this.labelComPort.Location = new System.Drawing.Point(6, 20);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(53, 13);
            this.labelComPort.TabIndex = 17;
            this.labelComPort.Text = "COM Port";
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(142, 13);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(93, 21);
            this.comboBoxComPort.TabIndex = 9;
            this.comboBoxComPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPort_SelectedIndexChanged);
            // 
            // numericUpDownSaturationTreshold
            // 
            this.numericUpDownSaturationTreshold.Location = new System.Drawing.Point(139, 47);
            this.numericUpDownSaturationTreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSaturationTreshold.Name = "numericUpDownSaturationTreshold";
            this.numericUpDownSaturationTreshold.Size = new System.Drawing.Size(93, 20);
            this.numericUpDownSaturationTreshold.TabIndex = 11;
            this.numericUpDownSaturationTreshold.ValueChanged += new System.EventHandler(this.numericUpDownSaturationTreshold_ValueChanged);
            // 
            // groupBoxLEDs
            // 
            this.groupBoxLEDs.Controls.Add(this.checkBoxReverse);
            this.groupBoxLEDs.Controls.Add(this.labelSaturationTreshold);
            this.groupBoxLEDs.Controls.Add(this.labelLedOffset);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownLedOffset);
            this.groupBoxLEDs.Controls.Add(this.checkBoxMirrorY);
            this.groupBoxLEDs.Controls.Add(this.checkBoxMirrorX);
            this.groupBoxLEDs.Controls.Add(this.labelLedsPerSpot);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownLedsPerSpot);
            this.groupBoxLEDs.Controls.Add(this.numericUpDownSaturationTreshold);
            this.groupBoxLEDs.Location = new System.Drawing.Point(259, 57);
            this.groupBoxLEDs.Name = "groupBoxLEDs";
            this.groupBoxLEDs.Size = new System.Drawing.Size(241, 145);
            this.groupBoxLEDs.TabIndex = 23;
            this.groupBoxLEDs.TabStop = false;
            this.groupBoxLEDs.Text = "LEDs";
            // 
            // checkBoxReverse
            // 
            this.checkBoxReverse.AutoSize = true;
            this.checkBoxReverse.Location = new System.Drawing.Point(9, 120);
            this.checkBoxReverse.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxReverse.Name = "checkBoxReverse";
            this.checkBoxReverse.Size = new System.Drawing.Size(134, 17);
            this.checkBoxReverse.TabIndex = 28;
            this.checkBoxReverse.Text = "Reverse order of LEDs";
            this.checkBoxReverse.UseVisualStyleBackColor = true;
            this.checkBoxReverse.CheckedChanged += new System.EventHandler(this.checkBoxReverse_CheckedChanged);
            // 
            // labelSaturationTreshold
            // 
            this.labelSaturationTreshold.AutoSize = true;
            this.labelSaturationTreshold.Location = new System.Drawing.Point(6, 49);
            this.labelSaturationTreshold.Name = "labelSaturationTreshold";
            this.labelSaturationTreshold.Size = new System.Drawing.Size(99, 13);
            this.labelSaturationTreshold.TabIndex = 27;
            this.labelSaturationTreshold.Text = "Saturation Treshold";
            // 
            // labelLedOffset
            // 
            this.labelLedOffset.AutoSize = true;
            this.labelLedOffset.Location = new System.Drawing.Point(6, 75);
            this.labelLedOffset.Name = "labelLedOffset";
            this.labelLedOffset.Size = new System.Drawing.Size(35, 13);
            this.labelLedOffset.TabIndex = 26;
            this.labelLedOffset.Text = "Offset";
            // 
            // numericUpDownLedOffset
            // 
            this.numericUpDownLedOffset.Location = new System.Drawing.Point(139, 73);
            this.numericUpDownLedOffset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownLedOffset.Name = "numericUpDownLedOffset";
            this.numericUpDownLedOffset.Size = new System.Drawing.Size(93, 20);
            this.numericUpDownLedOffset.TabIndex = 12;
            this.numericUpDownLedOffset.ValueChanged += new System.EventHandler(this.numericUpDownLedOffset_ValueChanged);
            // 
            // checkBoxMirrorY
            // 
            this.checkBoxMirrorY.AutoSize = true;
            this.checkBoxMirrorY.Location = new System.Drawing.Point(138, 98);
            this.checkBoxMirrorY.Name = "checkBoxMirrorY";
            this.checkBoxMirrorY.Size = new System.Drawing.Size(84, 17);
            this.checkBoxMirrorY.TabIndex = 15;
            this.checkBoxMirrorY.Text = "Mirror Y-Axis";
            this.checkBoxMirrorY.UseVisualStyleBackColor = true;
            this.checkBoxMirrorY.CheckedChanged += new System.EventHandler(this.checkBoxMirrorY_CheckedChanged);
            // 
            // checkBoxMirrorX
            // 
            this.checkBoxMirrorX.AutoSize = true;
            this.checkBoxMirrorX.Location = new System.Drawing.Point(9, 98);
            this.checkBoxMirrorX.Name = "checkBoxMirrorX";
            this.checkBoxMirrorX.Size = new System.Drawing.Size(84, 17);
            this.checkBoxMirrorX.TabIndex = 14;
            this.checkBoxMirrorX.Text = "Mirror X-Axis";
            this.checkBoxMirrorX.UseVisualStyleBackColor = true;
            this.checkBoxMirrorX.CheckedChanged += new System.EventHandler(this.checkBoxMirrorX_CheckedChanged);
            // 
            // groupBoxTransfer
            // 
            this.groupBoxTransfer.Controls.Add(this.comboBoxComPort);
            this.groupBoxTransfer.Controls.Add(this.labelComPort);
            this.groupBoxTransfer.Location = new System.Drawing.Point(259, 12);
            this.groupBoxTransfer.Name = "groupBoxTransfer";
            this.groupBoxTransfer.Size = new System.Drawing.Size(241, 41);
            this.groupBoxTransfer.TabIndex = 24;
            this.groupBoxTransfer.TabStop = false;
            this.groupBoxTransfer.Text = "Serial Transfer";
            // 
            // checkBoxTransferActive
            // 
            this.checkBoxTransferActive.AutoSize = true;
            this.checkBoxTransferActive.Location = new System.Drawing.Point(9, 19);
            this.checkBoxTransferActive.Name = "checkBoxTransferActive";
            this.checkBoxTransferActive.Size = new System.Drawing.Size(82, 17);
            this.checkBoxTransferActive.TabIndex = 16;
            this.checkBoxTransferActive.Text = "LED-Output";
            this.checkBoxTransferActive.UseVisualStyleBackColor = true;
            this.checkBoxTransferActive.CheckedChanged += new System.EventHandler(this.checkBoxTransferActive_CheckedChanged);
            // 
            // checkBoxOverlayActive
            // 
            this.checkBoxOverlayActive.AutoSize = true;
            this.checkBoxOverlayActive.Location = new System.Drawing.Point(138, 19);
            this.checkBoxOverlayActive.Name = "checkBoxOverlayActive";
            this.checkBoxOverlayActive.Size = new System.Drawing.Size(86, 17);
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
            this.groupBoxRun.Location = new System.Drawing.Point(12, 341);
            this.groupBoxRun.Name = "groupBoxRun";
            this.groupBoxRun.Size = new System.Drawing.Size(241, 72);
            this.groupBoxRun.TabIndex = 25;
            this.groupBoxRun.TabStop = false;
            this.groupBoxRun.Text = "Control";
            // 
            // checkBoxAutostart
            // 
            this.checkBoxAutostart.AutoSize = true;
            this.checkBoxAutostart.Location = new System.Drawing.Point(138, 42);
            this.checkBoxAutostart.Name = "checkBoxAutostart";
            this.checkBoxAutostart.Size = new System.Drawing.Size(68, 17);
            this.checkBoxAutostart.TabIndex = 19;
            this.checkBoxAutostart.Text = "Autostart";
            this.checkBoxAutostart.UseVisualStyleBackColor = true;
            this.checkBoxAutostart.CheckedChanged += new System.EventHandler(this.checkBoxAutostart_CheckedChanged);
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(9, 42);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(97, 17);
            this.checkBoxStartMinimized.TabIndex = 18;
            this.checkBoxStartMinimized.Text = "Start Minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            this.checkBoxStartMinimized.CheckedChanged += new System.EventHandler(this.checkBoxStartMinimized_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNonLinearLighting);
            this.groupBox1.Controls.Add(this.rbLinearLighting);
            this.groupBox1.Location = new System.Drawing.Point(12, 418);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(241, 63);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lighting Mode";
            // 
            // rbNonLinearLighting
            // 
            this.rbNonLinearLighting.AutoSize = true;
            this.rbNonLinearLighting.Location = new System.Drawing.Point(129, 27);
            this.rbNonLinearLighting.Margin = new System.Windows.Forms.Padding(2);
            this.rbNonLinearLighting.Name = "rbNonLinearLighting";
            this.rbNonLinearLighting.Size = new System.Drawing.Size(105, 17);
            this.rbNonLinearLighting.TabIndex = 1;
            this.rbNonLinearLighting.TabStop = true;
            this.rbNonLinearLighting.Text = "Non-linear fading";
            this.rbNonLinearLighting.UseVisualStyleBackColor = true;
            this.rbNonLinearLighting.CheckedChanged += new System.EventHandler(this.rbLinearLighting_CheckedChanged);
            // 
            // rbLinearLighting
            // 
            this.rbLinearLighting.AutoSize = true;
            this.rbLinearLighting.Location = new System.Drawing.Point(12, 27);
            this.rbLinearLighting.Margin = new System.Windows.Forms.Padding(2);
            this.rbLinearLighting.Name = "rbLinearLighting";
            this.rbLinearLighting.Size = new System.Drawing.Size(94, 17);
            this.rbLinearLighting.TabIndex = 0;
            this.rbLinearLighting.TabStop = true;
            this.rbLinearLighting.Text = "Linear Lighting";
            this.rbLinearLighting.UseVisualStyleBackColor = true;
            this.rbLinearLighting.CheckedChanged += new System.EventHandler(this.rbLinearLighting_CheckedChanged);
            // 
            // timerFPS
            // 
            this.timerFPS.Enabled = true;
            this.timerFPS.Interval = 1000;
            this.timerFPS.Tick += new System.EventHandler(this.timerFPS_Tick);
            // 
            // groupBoxDebug
            // 
            this.groupBoxDebug.Controls.Add(this.checkBoxLogging);
            this.groupBoxDebug.Location = new System.Drawing.Point(259, 418);
            this.groupBoxDebug.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxDebug.Name = "groupBoxDebug";
            this.groupBoxDebug.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxDebug.Size = new System.Drawing.Size(241, 63);
            this.groupBoxDebug.TabIndex = 27;
            this.groupBoxDebug.TabStop = false;
            this.groupBoxDebug.Text = "Debug";
            // 
            // checkBoxLogging
            // 
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Location = new System.Drawing.Point(9, 29);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new System.Drawing.Size(100, 17);
            this.checkBoxLogging.TabIndex = 0;
            this.checkBoxLogging.Text = "Enable Logging";
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            this.checkBoxLogging.CheckedChanged += new System.EventHandler(this.checkBoxLogging_CheckedChanged);
            // 
            // groupBoxWhiteBalance
            // 
            this.groupBoxWhiteBalance.Controls.Add(this.buttonWhiteSave);
            this.groupBoxWhiteBalance.Controls.Add(this.buttonWhiteZero);
            this.groupBoxWhiteBalance.Controls.Add(this.trackBarBrightness);
            this.groupBoxWhiteBalance.Controls.Add(this.labelBrightness);
            this.groupBoxWhiteBalance.Controls.Add(this.labelWhiteBalanceB);
            this.groupBoxWhiteBalance.Controls.Add(this.labelWhiteBalanceG);
            this.groupBoxWhiteBalance.Controls.Add(this.labelWhiteBalanceR);
            this.groupBoxWhiteBalance.Controls.Add(this.trackBarWhiteBalanceB);
            this.groupBoxWhiteBalance.Controls.Add(this.trackBarWhiteBalanceG);
            this.groupBoxWhiteBalance.Controls.Add(this.trackBarWhiteBalanceR);
            this.groupBoxWhiteBalance.Location = new System.Drawing.Point(259, 207);
            this.groupBoxWhiteBalance.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxWhiteBalance.Name = "groupBoxWhiteBalance";
            this.groupBoxWhiteBalance.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxWhiteBalance.Size = new System.Drawing.Size(241, 206);
            this.groupBoxWhiteBalance.TabIndex = 28;
            this.groupBoxWhiteBalance.TabStop = false;
            this.groupBoxWhiteBalance.Text = "White balance";
            // 
            // trackBarWhiteBalanceR
            // 
            this.trackBarWhiteBalanceR.LargeChange = 10;
            this.trackBarWhiteBalanceR.Location = new System.Drawing.Point(140, 18);
            this.trackBarWhiteBalanceR.Maximum = 100;
            this.trackBarWhiteBalanceR.Minimum = -100;
            this.trackBarWhiteBalanceR.Name = "trackBarWhiteBalanceR";
            this.trackBarWhiteBalanceR.Size = new System.Drawing.Size(93, 45);
            this.trackBarWhiteBalanceR.TabIndex = 29;
            this.trackBarWhiteBalanceR.Scroll += new System.EventHandler(this.trackBarWhiteBalanceR_Scroll);
            // 
            // trackBarWhiteBalanceG
            // 
            this.trackBarWhiteBalanceG.LargeChange = 10;
            this.trackBarWhiteBalanceG.Location = new System.Drawing.Point(140, 52);
            this.trackBarWhiteBalanceG.Maximum = 100;
            this.trackBarWhiteBalanceG.Minimum = -100;
            this.trackBarWhiteBalanceG.Name = "trackBarWhiteBalanceG";
            this.trackBarWhiteBalanceG.Size = new System.Drawing.Size(93, 45);
            this.trackBarWhiteBalanceG.TabIndex = 30;
            this.trackBarWhiteBalanceG.Scroll += new System.EventHandler(this.trackBarWhiteBalanceG_Scroll);
            // 
            // trackBarWhiteBalanceB
            // 
            this.trackBarWhiteBalanceB.LargeChange = 10;
            this.trackBarWhiteBalanceB.Location = new System.Drawing.Point(140, 88);
            this.trackBarWhiteBalanceB.Maximum = 100;
            this.trackBarWhiteBalanceB.Minimum = -100;
            this.trackBarWhiteBalanceB.Name = "trackBarWhiteBalanceB";
            this.trackBarWhiteBalanceB.Size = new System.Drawing.Size(93, 45);
            this.trackBarWhiteBalanceB.TabIndex = 31;
            this.trackBarWhiteBalanceB.Scroll += new System.EventHandler(this.trackBarWhiteBalanceB_Scroll);
            // 
            // labelWhiteBalanceR
            // 
            this.labelWhiteBalanceR.AutoSize = true;
            this.labelWhiteBalanceR.Location = new System.Drawing.Point(7, 21);
            this.labelWhiteBalanceR.Name = "labelWhiteBalanceR";
            this.labelWhiteBalanceR.Size = new System.Drawing.Size(27, 13);
            this.labelWhiteBalanceR.TabIndex = 39;
            this.labelWhiteBalanceR.Text = "Red";
            // 
            // labelWhiteBalanceG
            // 
            this.labelWhiteBalanceG.AutoSize = true;
            this.labelWhiteBalanceG.Location = new System.Drawing.Point(7, 55);
            this.labelWhiteBalanceG.Name = "labelWhiteBalanceG";
            this.labelWhiteBalanceG.Size = new System.Drawing.Size(36, 13);
            this.labelWhiteBalanceG.TabIndex = 40;
            this.labelWhiteBalanceG.Text = "Green";
            // 
            // labelWhiteBalanceB
            // 
            this.labelWhiteBalanceB.AutoSize = true;
            this.labelWhiteBalanceB.Location = new System.Drawing.Point(7, 91);
            this.labelWhiteBalanceB.Name = "labelWhiteBalanceB";
            this.labelWhiteBalanceB.Size = new System.Drawing.Size(28, 13);
            this.labelWhiteBalanceB.TabIndex = 41;
            this.labelWhiteBalanceB.Text = "Blue";
            // 
            // labelBrightness
            // 
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.Location = new System.Drawing.Point(7, 134);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new System.Drawing.Size(56, 13);
            this.labelBrightness.TabIndex = 39;
            this.labelBrightness.Text = "Brightness";
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.LargeChange = 10;
            this.trackBarBrightness.Location = new System.Drawing.Point(140, 131);
            this.trackBarBrightness.Maximum = 100;
            this.trackBarBrightness.Minimum = -100;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(93, 45);
            this.trackBarBrightness.TabIndex = 29;
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
            // 
            // buttonWhiteZero
            // 
            this.buttonWhiteZero.Location = new System.Drawing.Point(165, 178);
            this.buttonWhiteZero.Name = "buttonWhiteZero";
            this.buttonWhiteZero.Size = new System.Drawing.Size(39, 22);
            this.buttonWhiteZero.TabIndex = 42;
            this.buttonWhiteZero.Text = "Zero";
            this.buttonWhiteZero.UseVisualStyleBackColor = true;
            this.buttonWhiteZero.Click += new System.EventHandler(this.buttonWhiteZero_Click);
            // 
            // buttonWhiteSave
            // 
            this.buttonWhiteSave.Location = new System.Drawing.Point(10, 178);
            this.buttonWhiteSave.Name = "buttonWhiteSave";
            this.buttonWhiteSave.Size = new System.Drawing.Size(119, 22);
            this.buttonWhiteSave.TabIndex = 43;
            this.buttonWhiteSave.Text = "Save as default";
            this.buttonWhiteSave.UseVisualStyleBackColor = true;
            this.buttonWhiteSave.Click += new System.EventHandler(this.buttonWhiteSave_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(512, 491);
            this.Controls.Add(this.groupBoxWhiteBalance);
            this.Controls.Add(this.groupBoxDebug);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxRun);
            this.Controls.Add(this.groupBoxTransfer);
            this.Controls.Add(this.groupBoxLEDs);
            this.Controls.Add(this.groupBoxSpots);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 530);
            this.MinimumSize = new System.Drawing.Size(528, 530);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "adrilight";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxSpots.ResumeLayout(false);
            this.groupBoxSpots.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpotsX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBorderDistanceX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpotWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedsPerSpot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSaturationTreshold)).EndInit();
            this.groupBoxLEDs.ResumeLayout(false);
            this.groupBoxLEDs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLedOffset)).EndInit();
            this.groupBoxTransfer.ResumeLayout(false);
            this.groupBoxTransfer.PerformLayout();
            this.groupBoxRun.ResumeLayout(false);
            this.groupBoxRun.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxDebug.ResumeLayout(false);
            this.groupBoxDebug.PerformLayout();
            this.groupBoxWhiteBalance.ResumeLayout(false);
            this.groupBoxWhiteBalance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWhiteBalanceR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWhiteBalanceG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWhiteBalanceB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
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
        private System.Windows.Forms.Button resetOffsetXButton;
        private System.Windows.Forms.Button resetOffsetYButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbNonLinearLighting;
        private System.Windows.Forms.RadioButton rbLinearLighting;
        private System.Windows.Forms.Timer timerFPS;
        private System.Windows.Forms.CheckBox checkBoxReverse;
        private System.Windows.Forms.GroupBox groupBoxDebug;
        private System.Windows.Forms.CheckBox checkBoxLogging;
        private System.Windows.Forms.GroupBox groupBoxWhiteBalance;
        private System.Windows.Forms.Label labelWhiteBalanceB;
        private System.Windows.Forms.Label labelWhiteBalanceG;
        private System.Windows.Forms.Label labelWhiteBalanceR;
        private System.Windows.Forms.TrackBar trackBarWhiteBalanceB;
        private System.Windows.Forms.TrackBar trackBarWhiteBalanceG;
        private System.Windows.Forms.TrackBar trackBarWhiteBalanceR;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.Label labelBrightness;
        private System.Windows.Forms.Button buttonWhiteZero;
        private System.Windows.Forms.Button buttonWhiteSave;
    }
}

