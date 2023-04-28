using System;

namespace ControlRoomApplication.Main
{
    partial class FreeControlForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.errorLabel = new System.Windows.Forms.Label();
            this.overRideGroupbox = new System.Windows.Forms.GroupBox();
            this.stopRT = new System.Windows.Forms.Button();
            this.SoftwareStopsCheckBox = new System.Windows.Forms.CheckBox();
            this.controlScriptsCombo = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.runControlScriptButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.spectraCyberGroupBox = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.integrationStepCombo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.IFGainVal = new System.Windows.Forms.TextBox();
            this.lblIFGain = new System.Windows.Forms.Label();
            this.finalizeSettingsButton = new System.Windows.Forms.Button();
            this.DCGain = new System.Windows.Forms.ComboBox();
            this.stopScanButton = new System.Windows.Forms.Button();
            this.startScanButton = new System.Windows.Forms.Button();
            this.frequency = new System.Windows.Forms.TextBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.offsetVoltage = new System.Windows.Forms.TextBox();
            this.scanTypeComboBox = new System.Windows.Forms.ComboBox();
            this.IFGainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.frequencyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.offsetVoltageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cwAzJogButton = new System.Windows.Forms.Button();
            this.subElaButton = new System.Windows.Forms.Button();
            this.plusElaButton = new System.Windows.Forms.Button();
            this.ccwAzJogButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AzSpeedTextbox = new System.Windows.Forms.TextBox();
            this.ControlledButtonRadio = new System.Windows.Forms.RadioButton();
            this.immediateRadioButton = new System.Windows.Forms.RadioButton();
            this.manualControlButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AzSpeedTrackbar = new System.Windows.Forms.TrackBar();
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.ElSpeedTrackbar = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.ElSpeedTextbox = new System.Windows.Forms.TextBox();
            this.UseCounterbalanceCheckbox = new System.Windows.Forms.CheckBox();
           // this.RAIncGroupbox.SuspendLayout();
            this.overRideGroupbox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.spectraCyberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AzSpeedTrackbar)).BeginInit();
            this.manualGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ElSpeedTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(874, -288);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(160, 84);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overRideGroupbox
            // 
            this.overRideGroupbox.BackColor = System.Drawing.Color.Gainsboro;
            this.overRideGroupbox.Controls.Add(this.UseCounterbalanceCheckbox);
            this.overRideGroupbox.Controls.Add(this.stopRT);
            this.overRideGroupbox.Controls.Add(this.SoftwareStopsCheckBox);
            this.overRideGroupbox.Location = new System.Drawing.Point(448, 16);
            this.overRideGroupbox.Margin = new System.Windows.Forms.Padding(4);
            this.overRideGroupbox.Name = "overRideGroupbox";
            this.overRideGroupbox.Padding = new System.Windows.Forms.Padding(4);
            this.overRideGroupbox.Size = new System.Drawing.Size(418, 81);
            this.overRideGroupbox.TabIndex = 19;
            this.overRideGroupbox.TabStop = false;
            this.overRideGroupbox.Text = "Stop Telescope";
            // 
            // stopRT
            // 
            this.stopRT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopRT.BackColor = System.Drawing.Color.Red;
            this.stopRT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopRT.Location = new System.Drawing.Point(231, 22);
            this.stopRT.Name = "stopRT";
            this.stopRT.Size = new System.Drawing.Size(150, 46);
            this.stopRT.TabIndex = 38;
            this.stopRT.Text = "STOP Telescope";
            this.stopRT.UseVisualStyleBackColor = false;
            this.stopRT.Click += new System.EventHandler(this.stopRT_click);
            // 
            // SoftwareStopsCheckBox
            // 
            this.SoftwareStopsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SoftwareStopsCheckBox.AutoSize = true;
            this.SoftwareStopsCheckBox.Checked = true;
            this.SoftwareStopsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SoftwareStopsCheckBox.Location = new System.Drawing.Point(24, 30);
            this.SoftwareStopsCheckBox.Name = "SoftwareStopsCheckBox";
            this.SoftwareStopsCheckBox.Size = new System.Drawing.Size(134, 17);
            this.SoftwareStopsCheckBox.TabIndex = 17;
            this.SoftwareStopsCheckBox.Text = "Enable Software Stops";
            this.SoftwareStopsCheckBox.UseVisualStyleBackColor = true;
            this.SoftwareStopsCheckBox.CheckedChanged += new System.EventHandler(this.SoftwareStopsCheckBox_CheckedChanged);
            // 
            // controlScriptsCombo
            // 
            this.controlScriptsCombo.BackColor = System.Drawing.Color.DarkGray;
            this.controlScriptsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlScriptsCombo.FormattingEnabled = true;
            this.controlScriptsCombo.Items.AddRange(new object[] {
            "Radio Telescope Control Scripts",
            "Stow Telescope",
            "Full Elevation",
            "Full 360 Clockwise Rotation",
            "Full 360 Counter-Clockwise Rotation",
            "Thermal Calibration",
            "Snow Dump",
            "Home Telescope",
            "Custom Orientation Movement",
            "Endless Azimuth Rotation",
            "Hardware Movement Script"});
            this.controlScriptsCombo.Location = new System.Drawing.Point(6, 42);
            this.controlScriptsCombo.Margin = new System.Windows.Forms.Padding(4);
            this.controlScriptsCombo.Name = "controlScriptsCombo";
            this.controlScriptsCombo.Size = new System.Drawing.Size(388, 21);
            this.controlScriptsCombo.TabIndex = 23;
            this.controlScriptsCombo.SelectedIndexChanged += new System.EventHandler(this.controlScriptsCombo_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.runControlScriptButton);
            this.groupBox4.Controls.Add(this.controlScriptsCombo);
            this.groupBox4.Location = new System.Drawing.Point(448, 106);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(627, 97);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control Scripts and Spectra";
            // 
            // runControlScriptButton
            // 
            this.runControlScriptButton.BackColor = System.Drawing.Color.DarkGray;
            this.runControlScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.runControlScriptButton.Location = new System.Drawing.Point(432, 24);
            this.runControlScriptButton.Name = "runControlScriptButton";
            this.runControlScriptButton.Size = new System.Drawing.Size(189, 49);
            this.runControlScriptButton.TabIndex = 24;
            this.runControlScriptButton.Text = "Run Script";
            this.runControlScriptButton.UseVisualStyleBackColor = false;
            this.runControlScriptButton.Click += new System.EventHandler(this.runControlScript_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(1042, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 34);
            this.button1.TabIndex = 26;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.helpButton_click);
            // 
            // spectraCyberGroupBox
            // 
            this.spectraCyberGroupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.spectraCyberGroupBox.Controls.Add(this.label12);
            this.spectraCyberGroupBox.Controls.Add(this.integrationStepCombo);
            this.spectraCyberGroupBox.Controls.Add(this.label10);
            this.spectraCyberGroupBox.Controls.Add(this.IFGainVal);
            this.spectraCyberGroupBox.Controls.Add(this.lblIFGain);
            this.spectraCyberGroupBox.Controls.Add(this.finalizeSettingsButton);
            this.spectraCyberGroupBox.Controls.Add(this.DCGain);
            this.spectraCyberGroupBox.Controls.Add(this.stopScanButton);
            this.spectraCyberGroupBox.Controls.Add(this.startScanButton);
            this.spectraCyberGroupBox.Controls.Add(this.frequency);
            this.spectraCyberGroupBox.Controls.Add(this.lblFrequency);
            this.spectraCyberGroupBox.Controls.Add(this.label9);
            this.spectraCyberGroupBox.Controls.Add(this.offsetVoltage);
            this.spectraCyberGroupBox.Controls.Add(this.scanTypeComboBox);
            this.spectraCyberGroupBox.Location = new System.Drawing.Point(448, 209);
            this.spectraCyberGroupBox.Name = "spectraCyberGroupBox";
            this.spectraCyberGroupBox.Size = new System.Drawing.Size(627, 126);
            this.spectraCyberGroupBox.TabIndex = 29;
            this.spectraCyberGroupBox.TabStop = false;
            this.spectraCyberGroupBox.Text = "Spectra Cyber";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(200, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Offset Voltage";
            // 
            // integrationStepCombo
            // 
            this.integrationStepCombo.BackColor = System.Drawing.Color.DarkGray;
            this.integrationStepCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.integrationStepCombo.FormattingEnabled = true;
            this.integrationStepCombo.Items.AddRange(new object[] {
            "Int Step",
            "0.3",
            "0.5(S)/1.00(C) ",
            "1.00(S)/10.00(C)"});
            this.integrationStepCombo.Location = new System.Drawing.Point(322, 84);
            this.integrationStepCombo.MaxDropDownItems = 6;
            this.integrationStepCombo.Name = "integrationStepCombo";
            this.integrationStepCombo.Size = new System.Drawing.Size(116, 21);
            this.integrationStepCombo.TabIndex = 44;
            this.integrationStepCombo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 43;
            this.label10.Text = "DCGain (dB)";
            // 
            // IFGainVal
            // 
            this.IFGainVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IFGainVal.Location = new System.Drawing.Point(116, 85);
            this.IFGainVal.Name = "IFGainVal";
            this.IFGainVal.Size = new System.Drawing.Size(64, 20);
            this.IFGainVal.TabIndex = 42;
            this.IFGainVal.TextChanged += new System.EventHandler(this.IFGainVal_TextChanged);
            // 
            // lblIFGain
            // 
            this.lblIFGain.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblIFGain.AutoSize = true;
            this.lblIFGain.Location = new System.Drawing.Point(111, 64);
            this.lblIFGain.Name = "lblIFGain";
            this.lblIFGain.Size = new System.Drawing.Size(60, 13);
            this.lblIFGain.TabIndex = 41;
            this.lblIFGain.Text = "IFGain (dB)";
            // 
            // finalizeSettingsButton
            // 
            this.finalizeSettingsButton.BackColor = System.Drawing.Color.DarkGray;
            this.finalizeSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.finalizeSettingsButton.Location = new System.Drawing.Point(464, 15);
            this.finalizeSettingsButton.Name = "finalizeSettingsButton";
            this.finalizeSettingsButton.Size = new System.Drawing.Size(146, 40);
            this.finalizeSettingsButton.TabIndex = 40;
            this.finalizeSettingsButton.Text = "Finalize Settings";
            this.finalizeSettingsButton.UseVisualStyleBackColor = false;
            this.finalizeSettingsButton.Click += new System.EventHandler(this.finalizeSettings_Click);
            // 
            // DCGain
            // 
            this.DCGain.BackColor = System.Drawing.Color.DarkGray;
            this.DCGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DCGain.FormattingEnabled = true;
            this.DCGain.Items.AddRange(new object[] {
            "Gain",
            "x1",
            "X5",
            "X10",
            "X20",
            "X50",
            "X60"});
            this.DCGain.Location = new System.Drawing.Point(6, 84);
            this.DCGain.MaxDropDownItems = 6;
            this.DCGain.Name = "DCGain";
            this.DCGain.Size = new System.Drawing.Size(84, 21);
            this.DCGain.TabIndex = 39;
            this.DCGain.SelectedIndexChanged += new System.EventHandler(this.DCGain_SelectedIndexChanged);
            // 
            // stopScanButton
            // 
            this.stopScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopScanButton.BackColor = System.Drawing.Color.OrangeRed;
            this.stopScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopScanButton.Location = new System.Drawing.Point(542, 66);
            this.stopScanButton.Name = "stopScanButton";
            this.stopScanButton.Size = new System.Drawing.Size(68, 54);
            this.stopScanButton.TabIndex = 37;
            this.stopScanButton.Text = "Stop Scan";
            this.stopScanButton.UseVisualStyleBackColor = false;
            this.stopScanButton.Click += new System.EventHandler(this.stopScan_Click);
            // 
            // startScanButton
            // 
            this.startScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startScanButton.BackColor = System.Drawing.Color.LimeGreen;
            this.startScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startScanButton.Location = new System.Drawing.Point(464, 66);
            this.startScanButton.Name = "startScanButton";
            this.startScanButton.Size = new System.Drawing.Size(72, 54);
            this.startScanButton.TabIndex = 36;
            this.startScanButton.Text = "Start Scan";
            this.startScanButton.UseVisualStyleBackColor = false;
            this.startScanButton.Click += new System.EventHandler(this.startScan_Click);
            // 
            // frequency
            // 
            this.frequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.frequency.Location = new System.Drawing.Point(237, 27);
            this.frequency.Name = "frequency";
            this.frequency.Size = new System.Drawing.Size(112, 20);
            this.frequency.TabIndex = 35;
            this.frequency.TextChanged += new System.EventHandler(this.frequency_TextChanged);
            // 
            // lblFrequency
            // 
            this.lblFrequency.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(232, 0);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(85, 13);
            this.lblFrequency.TabIndex = 34;
            this.lblFrequency.Text = "Frequency (kHz)";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(318, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Integration Step";
            // 
            // offsetVoltage
            // 
            this.offsetVoltage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.offsetVoltage.Location = new System.Drawing.Point(204, 85);
            this.offsetVoltage.Name = "offsetVoltage";
            this.offsetVoltage.Size = new System.Drawing.Size(64, 20);
            this.offsetVoltage.TabIndex = 27;
            this.offsetVoltage.TextChanged += new System.EventHandler(this.offsetVoltage_TextChanged);
            // 
            // scanTypeComboBox
            // 
            this.scanTypeComboBox.BackColor = System.Drawing.Color.DarkGray;
            this.scanTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scanTypeComboBox.FormattingEnabled = true;
            this.scanTypeComboBox.Items.AddRange(new object[] {
            "Scan Type",
            "Continuum",
            "Spectral"});
            this.scanTypeComboBox.Location = new System.Drawing.Point(6, 27);
            this.scanTypeComboBox.MaxDropDownItems = 2;
            this.scanTypeComboBox.Name = "scanTypeComboBox";
            this.scanTypeComboBox.Size = new System.Drawing.Size(122, 21);
            this.scanTypeComboBox.TabIndex = 25;
            this.scanTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.scanTypeComboBox_SelectedIndexChanged);
            // 
            // cwAzJogButton
            // 
            this.cwAzJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cwAzJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.cwAzJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cwAzJogButton.Location = new System.Drawing.Point(358, 90);
            this.cwAzJogButton.Name = "cwAzJogButton";
            this.cwAzJogButton.Size = new System.Drawing.Size(60, 60);
            this.cwAzJogButton.TabIndex = 7;
            this.cwAzJogButton.Text = "CW Jog";
            this.cwAzJogButton.UseVisualStyleBackColor = false;
            this.cwAzJogButton.Click += new System.EventHandler(this.cwAzJogButton_Click);
            this.cwAzJogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cwAzJogButton_Down);
            this.cwAzJogButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwAzJogButton_UP);
            // 
            // subElaButton
            // 
            this.subElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.subElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subElaButton.Location = new System.Drawing.Point(292, 155);
            this.subElaButton.Name = "subElaButton";
            this.subElaButton.Size = new System.Drawing.Size(60, 60);
            this.subElaButton.TabIndex = 5;
            this.subElaButton.Text = "- Ela";
            this.subElaButton.UseVisualStyleBackColor = false;
            this.subElaButton.Click += new System.EventHandler(this.subElaButton_Click);
            this.subElaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.subElaButton_Down);
            this.subElaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.subElaButton_Up);
            // 
            // plusElaButton
            // 
            this.plusElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusElaButton.Location = new System.Drawing.Point(292, 25);
            this.plusElaButton.Name = "plusElaButton";
            this.plusElaButton.Size = new System.Drawing.Size(60, 60);
            this.plusElaButton.TabIndex = 4;
            this.plusElaButton.Text = "+ Ela";
            this.plusElaButton.UseVisualStyleBackColor = false;
            this.plusElaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plusElaButton_Down);
            this.plusElaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plusElaButton_Up);
            // 
            // ccwAzJogButton
            // 
            this.ccwAzJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ccwAzJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.ccwAzJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ccwAzJogButton.Location = new System.Drawing.Point(226, 90);
            this.ccwAzJogButton.Name = "ccwAzJogButton";
            this.ccwAzJogButton.Size = new System.Drawing.Size(60, 60);
            this.ccwAzJogButton.TabIndex = 6;
            this.ccwAzJogButton.Text = "CCW Jog";
            this.ccwAzJogButton.UseVisualStyleBackColor = false;
            this.ccwAzJogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ccwAzJogButton_Down);
            this.ccwAzJogButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ccwAzJogButton_Up);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Elevation: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 120);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current Azimuth: ";
            // 
            // AzSpeedTextBox
            // 
            this.AzSpeedTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AzSpeedTextbox.Location = new System.Drawing.Point(91, 235);
            this.AzSpeedTextbox.Name = "AzSpeedTextBox";
            this.AzSpeedTextbox.ReadOnly = true;
            this.AzSpeedTextbox.Size = new System.Drawing.Size(102, 20);
            this.AzSpeedTextbox.TabIndex = 10;
            this.AzSpeedTextbox.TextChanged += new System.EventHandler(this.speedTextBox_TextChanged);
            // 
            // ControlledButtonRadio
            // 
            this.ControlledButtonRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ControlledButtonRadio.AutoSize = true;
            this.ControlledButtonRadio.Checked = true;
            this.ControlledButtonRadio.Location = new System.Drawing.Point(21, 160);
            this.ControlledButtonRadio.Name = "ControlledButtonRadio";
            this.ControlledButtonRadio.Size = new System.Drawing.Size(97, 17);
            this.ControlledButtonRadio.TabIndex = 23;
            this.ControlledButtonRadio.TabStop = true;
            this.ControlledButtonRadio.Text = "Controlled Stop";
            this.ControlledButtonRadio.UseVisualStyleBackColor = true;
            // 
            // immediateRadioButton
            // 
            this.immediateRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.immediateRadioButton.AutoSize = true;
            this.immediateRadioButton.Location = new System.Drawing.Point(21, 192);
            this.immediateRadioButton.Name = "immediateRadioButton";
            this.immediateRadioButton.Size = new System.Drawing.Size(98, 17);
            this.immediateRadioButton.TabIndex = 24;
            this.immediateRadioButton.Text = "Immediate Stop";
            this.immediateRadioButton.UseVisualStyleBackColor = true;
            // 
            // manualControlButton
            // 
            this.manualControlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.manualControlButton.BackColor = System.Drawing.Color.OrangeRed;
            this.manualControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualControlButton.Location = new System.Drawing.Point(21, 33);
            this.manualControlButton.Name = "manualControlButton";
            this.manualControlButton.Size = new System.Drawing.Size(225, 42);
            this.manualControlButton.TabIndex = 25;
            this.manualControlButton.Text = "Activate Manual Control";
            this.manualControlButton.UseVisualStyleBackColor = false;
            this.manualControlButton.Click += new System.EventHandler(this.manualControlButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Azimuth RPM";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(158, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "0.0";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(158, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "0.0";
            // 
            // AzSpeedTrackbar
            // 
            this.AzSpeedTrackbar.Location = new System.Drawing.Point(200, 223);
            this.AzSpeedTrackbar.Margin = new System.Windows.Forms.Padding(4);
            this.AzSpeedTrackbar.Name = "AzSpeedTrackbar";
            this.AzSpeedTrackbar.Size = new System.Drawing.Size(225, 45);
            this.AzSpeedTrackbar.SmallChange = 3;
            this.AzSpeedTrackbar.TabIndex = 20;
            this.AzSpeedTrackbar.Scroll += new System.EventHandler(this.AzSpeedTrackbarScroll);
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.manualGroupBox.Controls.Add(this.ElSpeedTrackbar);
            this.manualGroupBox.Controls.Add(this.label6);
            this.manualGroupBox.Controls.Add(this.ElSpeedTextbox);
            this.manualGroupBox.Controls.Add(this.AzSpeedTrackbar);
            this.manualGroupBox.Controls.Add(this.label4);
            this.manualGroupBox.Controls.Add(this.label5);
            this.manualGroupBox.Controls.Add(this.label3);
            this.manualGroupBox.Controls.Add(this.manualControlButton);
            this.manualGroupBox.Controls.Add(this.immediateRadioButton);
            this.manualGroupBox.Controls.Add(this.ControlledButtonRadio);
            this.manualGroupBox.Controls.Add(this.AzSpeedTextbox);
            this.manualGroupBox.Controls.Add(this.label2);
            this.manualGroupBox.Controls.Add(this.label1);
            this.manualGroupBox.Controls.Add(this.ccwAzJogButton);
            this.manualGroupBox.Controls.Add(this.plusElaButton);
            this.manualGroupBox.Controls.Add(this.subElaButton);
            this.manualGroupBox.Controls.Add(this.cwAzJogButton);
            this.manualGroupBox.Location = new System.Drawing.Point(9, 16);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Size = new System.Drawing.Size(432, 317);
            this.manualGroupBox.TabIndex = 25;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "Manual Control";
            // 
            // ElSpeedTrackbar
            // 
            this.ElSpeedTrackbar.Location = new System.Drawing.Point(200, 268);
            this.ElSpeedTrackbar.Margin = new System.Windows.Forms.Padding(4);
            this.ElSpeedTrackbar.Name = "ElSpeedTrackbar";
            this.ElSpeedTrackbar.Size = new System.Drawing.Size(225, 45);
            this.ElSpeedTrackbar.SmallChange = 3;
            this.ElSpeedTrackbar.TabIndex = 30;
            this.ElSpeedTrackbar.Scroll += new System.EventHandler(this.ElSpeedTrackbarScroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Elevation RPM";
            // 
            // ElSpeedTextbox
            // 
            this.ElSpeedTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ElSpeedTextbox.Location = new System.Drawing.Point(91, 280);
            this.ElSpeedTextbox.Name = "ElSpeedTextbox";
            this.ElSpeedTextbox.ReadOnly = true;
            this.ElSpeedTextbox.Size = new System.Drawing.Size(102, 20);
            this.ElSpeedTextbox.TabIndex = 29;
            // 
            // UseCounterbalanceCheckbox
            // 
            this.UseCounterbalanceCheckbox.AutoSize = true;
            this.UseCounterbalanceCheckbox.Location = new System.Drawing.Point(16, 175);
            this.UseCounterbalanceCheckbox.Name = "UseCounterbalanceCheckbox";
            this.UseCounterbalanceCheckbox.Size = new System.Drawing.Size(132, 17);
            this.UseCounterbalanceCheckbox.TabIndex = 39;
            this.UseCounterbalanceCheckbox.Text = "Use CB accelerometer";
            this.UseCounterbalanceCheckbox.UseVisualStyleBackColor = true;
            this.UseCounterbalanceCheckbox.Click += new System.EventHandler(this.useCounterbalanceCheckbox_Click);
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1086, 348);
            this.Controls.Add(this.spectraCyberGroupBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.manualGroupBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.overRideGroupbox);
            this.Controls.Add(this.errorLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(892, 130);
            this.Name = "FreeControlForm";
            this.Text = "Control Form";
            this.overRideGroupbox.ResumeLayout(false);
            this.overRideGroupbox.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.spectraCyberGroupBox.ResumeLayout(false);
            this.spectraCyberGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AzSpeedTrackbar)).EndInit();
            this.manualGroupBox.ResumeLayout(false);
            this.manualGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ElSpeedTrackbar)).EndInit();
            this.ResumeLayout(false);

        }

      
        #endregion
        /*private System.Windows.Forms.TextBox ActualRATextBox;
        private System.Windows.Forms.Label ActualPositionLabel;
        private System.Windows.Forms.Label ActualRALabel;
        private System.Windows.Forms.Label ActualDecLabel;
        private System.Windows.Forms.Label TargetDecLabel;
        private System.Windows.Forms.Label TargetRALabel;
        private System.Windows.Forms.Label TargetPositionLabel;
        private System.Windows.Forms.TextBox TargetDecTextBox;
        private System.Windows.Forms.TextBox TargetRATextBox;
        */
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox overRideGroupbox;
        private System.Windows.Forms.ComboBox controlScriptsCombo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button runControlScriptButton;
        private System.Windows.Forms.Button button1;
        //private System.Windows.Forms.Label label8;
        //private System.Windows.Forms.TextBox statusTextBox;
        //private System.Windows.Forms.TextBox ActualDecTextBox;
        private System.Windows.Forms.GroupBox spectraCyberGroupBox;
        private System.Windows.Forms.Button finalizeSettingsButton;
        private System.Windows.Forms.ComboBox DCGain;
        private System.Windows.Forms.Button stopScanButton;
        private System.Windows.Forms.Button startScanButton;
        private System.Windows.Forms.TextBox frequency;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox offsetVoltage;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox scanTypeComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox IFGainVal;
        private System.Windows.Forms.Label lblIFGain;
        private System.Windows.Forms.ComboBox integrationStepCombo;
        private System.Windows.Forms.ToolTip IFGainToolTip;
        private System.Windows.Forms.ToolTip offsetVoltageToolTip;
        private System.Windows.Forms.ToolTip frequencyToolTip;
        private System.Windows.Forms.CheckBox SoftwareStopsCheckBox;
        private System.Windows.Forms.Button stopRT;
        private System.Windows.Forms.Button cwAzJogButton;
        private System.Windows.Forms.Button subElaButton;
        private System.Windows.Forms.Button plusElaButton;
        private System.Windows.Forms.Button ccwAzJogButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AzSpeedTextbox;
        private System.Windows.Forms.RadioButton ControlledButtonRadio;
        private System.Windows.Forms.RadioButton immediateRadioButton;
        private System.Windows.Forms.Button manualControlButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar AzSpeedTrackbar;
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.TrackBar ElSpeedTrackbar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ElSpeedTextbox;
        private System.Windows.Forms.CheckBox UseCounterbalanceCheckbox;
    }
}