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
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.speedTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.manualControlButton = new System.Windows.Forms.Button();
            this.immediateRadioButton = new System.Windows.Forms.RadioButton();
            this.ControlledButtonRadio = new System.Windows.Forms.RadioButton();
            this.speedTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ccwAzJogButton = new System.Windows.Forms.Button();
            this.plusElaButton = new System.Windows.Forms.Button();
            this.subElaButton = new System.Windows.Forms.Button();
            this.cwAzJogButton = new System.Windows.Forms.Button();
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
            this.overRideGroupbox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.manualGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
            this.spectraCyberGroupBox.SuspendLayout();
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
            this.errorLabel.Location = new System.Drawing.Point(583, -193);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(107, 57);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overRideGroupbox
            // 
            this.overRideGroupbox.BackColor = System.Drawing.Color.Gainsboro;
            this.overRideGroupbox.Controls.Add(this.stopRT);
            this.overRideGroupbox.Controls.Add(this.SoftwareStopsCheckBox);
            this.overRideGroupbox.Location = new System.Drawing.Point(299, 11);
            this.overRideGroupbox.Name = "overRideGroupbox";
            this.overRideGroupbox.Size = new System.Drawing.Size(279, 54);
            this.overRideGroupbox.TabIndex = 19;
            this.overRideGroupbox.TabStop = false;
            this.overRideGroupbox.Text = "Stop Telescope";
            // 
            // stopRT
            // 
            this.stopRT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopRT.BackColor = System.Drawing.Color.Red;
            this.stopRT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopRT.Location = new System.Drawing.Point(154, 15);
            this.stopRT.Margin = new System.Windows.Forms.Padding(2);
            this.stopRT.Name = "stopRT";
            this.stopRT.Size = new System.Drawing.Size(100, 31);
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
            this.SoftwareStopsCheckBox.Location = new System.Drawing.Point(16, 15);
            this.SoftwareStopsCheckBox.Margin = new System.Windows.Forms.Padding(2);
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
            this.controlScriptsCombo.Location = new System.Drawing.Point(4, 28);
            this.controlScriptsCombo.Name = "controlScriptsCombo";
            this.controlScriptsCombo.Size = new System.Drawing.Size(260, 21);
            this.controlScriptsCombo.TabIndex = 23;
            this.controlScriptsCombo.SelectedIndexChanged += new System.EventHandler(this.controlScriptsCombo_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.runControlScriptButton);
            this.groupBox4.Controls.Add(this.controlScriptsCombo);
            this.groupBox4.Location = new System.Drawing.Point(299, 71);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(418, 65);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control Scripts and Spectra";
            // 
            // runControlScriptButton
            // 
            this.runControlScriptButton.BackColor = System.Drawing.Color.DarkGray;
            this.runControlScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.runControlScriptButton.Location = new System.Drawing.Point(288, 16);
            this.runControlScriptButton.Margin = new System.Windows.Forms.Padding(2);
            this.runControlScriptButton.Name = "runControlScriptButton";
            this.runControlScriptButton.Size = new System.Drawing.Size(126, 33);
            this.runControlScriptButton.TabIndex = 24;
            this.runControlScriptButton.Text = "Run Script";
            this.runControlScriptButton.UseVisualStyleBackColor = false;
            this.runControlScriptButton.Click += new System.EventHandler(this.runControlScript_Click);
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.manualGroupBox.Controls.Add(this.speedTrackBar);
            this.manualGroupBox.Controls.Add(this.label4);
            this.manualGroupBox.Controls.Add(this.label5);
            this.manualGroupBox.Controls.Add(this.label3);
            this.manualGroupBox.Controls.Add(this.manualControlButton);
            this.manualGroupBox.Controls.Add(this.immediateRadioButton);
            this.manualGroupBox.Controls.Add(this.ControlledButtonRadio);
            this.manualGroupBox.Controls.Add(this.speedTextBox);
            this.manualGroupBox.Controls.Add(this.label2);
            this.manualGroupBox.Controls.Add(this.label1);
            this.manualGroupBox.Controls.Add(this.ccwAzJogButton);
            this.manualGroupBox.Controls.Add(this.plusElaButton);
            this.manualGroupBox.Controls.Add(this.subElaButton);
            this.manualGroupBox.Controls.Add(this.cwAzJogButton);
            this.manualGroupBox.Location = new System.Drawing.Point(6, 11);
            this.manualGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.manualGroupBox.Size = new System.Drawing.Size(288, 212);
            this.manualGroupBox.TabIndex = 25;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "Manual Control";
            // 
            // speedTrackBar
            // 
            this.speedTrackBar.Location = new System.Drawing.Point(108, 161);
            this.speedTrackBar.Name = "speedTrackBar";
            this.speedTrackBar.Size = new System.Drawing.Size(150, 45);
            this.speedTrackBar.SmallChange = 3;
            this.speedTrackBar.TabIndex = 20;
            this.speedTrackBar.Scroll += new System.EventHandler(this.speedTrackBar_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(105, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(105, 60);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 155);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Speed (RPMs)";
            // 
            // manualControlButton
            // 
            this.manualControlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.manualControlButton.BackColor = System.Drawing.Color.OrangeRed;
            this.manualControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualControlButton.Location = new System.Drawing.Point(14, 22);
            this.manualControlButton.Margin = new System.Windows.Forms.Padding(2);
            this.manualControlButton.Name = "manualControlButton";
            this.manualControlButton.Size = new System.Drawing.Size(150, 28);
            this.manualControlButton.TabIndex = 25;
            this.manualControlButton.Text = "Activate Manual Control";
            this.manualControlButton.UseVisualStyleBackColor = false;
            this.manualControlButton.Click += new System.EventHandler(this.manualControlButton_Click);
            // 
            // immediateRadioButton
            // 
            this.immediateRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.immediateRadioButton.AutoSize = true;
            this.immediateRadioButton.Location = new System.Drawing.Point(14, 123);
            this.immediateRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.immediateRadioButton.Name = "immediateRadioButton";
            this.immediateRadioButton.Size = new System.Drawing.Size(98, 17);
            this.immediateRadioButton.TabIndex = 24;
            this.immediateRadioButton.Text = "Immediate Stop";
            this.immediateRadioButton.UseVisualStyleBackColor = true;
            // 
            // ControlledButtonRadio
            // 
            this.ControlledButtonRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ControlledButtonRadio.AutoSize = true;
            this.ControlledButtonRadio.Checked = true;
            this.ControlledButtonRadio.Location = new System.Drawing.Point(14, 102);
            this.ControlledButtonRadio.Margin = new System.Windows.Forms.Padding(2);
            this.ControlledButtonRadio.Name = "ControlledButtonRadio";
            this.ControlledButtonRadio.Size = new System.Drawing.Size(97, 17);
            this.ControlledButtonRadio.TabIndex = 23;
            this.ControlledButtonRadio.TabStop = true;
            this.ControlledButtonRadio.Text = "Controlled Stop";
            this.ControlledButtonRadio.UseVisualStyleBackColor = true;
            // 
            // speedTextBox
            // 
            this.speedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.speedTextBox.Location = new System.Drawing.Point(14, 175);
            this.speedTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.speedTextBox.Name = "speedTextBox";
            this.speedTextBox.ReadOnly = true;
            this.speedTextBox.Size = new System.Drawing.Size(69, 20);
            this.speedTextBox.TabIndex = 10;
            this.speedTextBox.TextChanged += new System.EventHandler(this.speedTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current Azimuth: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Elevation: ";
            // 
            // ccwAzJogButton
            // 
            this.ccwAzJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ccwAzJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.ccwAzJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ccwAzJogButton.Location = new System.Drawing.Point(151, 60);
            this.ccwAzJogButton.Margin = new System.Windows.Forms.Padding(2);
            this.ccwAzJogButton.Name = "ccwAzJogButton";
            this.ccwAzJogButton.Size = new System.Drawing.Size(40, 40);
            this.ccwAzJogButton.TabIndex = 6;
            this.ccwAzJogButton.Text = "CCW Jog";
            this.ccwAzJogButton.UseVisualStyleBackColor = false;
            this.ccwAzJogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ccwAzJogButton_Down);
            this.ccwAzJogButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ccwAzJogButton_Up);
            // 
            // plusElaButton
            // 
            this.plusElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusElaButton.Location = new System.Drawing.Point(195, 17);
            this.plusElaButton.Margin = new System.Windows.Forms.Padding(2);
            this.plusElaButton.Name = "plusElaButton";
            this.plusElaButton.Size = new System.Drawing.Size(40, 40);
            this.plusElaButton.TabIndex = 4;
            this.plusElaButton.Text = "+ Ela";
            this.plusElaButton.UseVisualStyleBackColor = false;
            this.plusElaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plusElaButton_Down);
            this.plusElaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plusElaButton_Up);
            // 
            // subElaButton
            // 
            this.subElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.subElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subElaButton.Location = new System.Drawing.Point(195, 104);
            this.subElaButton.Margin = new System.Windows.Forms.Padding(2);
            this.subElaButton.Name = "subElaButton";
            this.subElaButton.Size = new System.Drawing.Size(40, 40);
            this.subElaButton.TabIndex = 5;
            this.subElaButton.Text = "- Ela";
            this.subElaButton.UseVisualStyleBackColor = false;
            this.subElaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.subElaButton_Down);
            this.subElaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.subElaButton_Up);
            // 
            // cwAzJogButton
            // 
            this.cwAzJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cwAzJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.cwAzJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cwAzJogButton.Location = new System.Drawing.Point(239, 60);
            this.cwAzJogButton.Margin = new System.Windows.Forms.Padding(2);
            this.cwAzJogButton.Name = "cwAzJogButton";
            this.cwAzJogButton.Size = new System.Drawing.Size(40, 40);
            this.cwAzJogButton.TabIndex = 7;
            this.cwAzJogButton.Text = "CW Jog";
            this.cwAzJogButton.UseVisualStyleBackColor = false;
            this.cwAzJogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cwAzJogButton_Down);
            this.cwAzJogButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwAzJogButton_UP);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(695, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 23);
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
            this.spectraCyberGroupBox.Location = new System.Drawing.Point(299, 140);
            this.spectraCyberGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.spectraCyberGroupBox.Name = "spectraCyberGroupBox";
            this.spectraCyberGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.spectraCyberGroupBox.Size = new System.Drawing.Size(418, 84);
            this.spectraCyberGroupBox.TabIndex = 29;
            this.spectraCyberGroupBox.TabStop = false;
            this.spectraCyberGroupBox.Text = "Spectra Cyber";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(133, 43);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
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
            this.integrationStepCombo.Location = new System.Drawing.Point(215, 56);
            this.integrationStepCombo.Margin = new System.Windows.Forms.Padding(2);
            this.integrationStepCombo.MaxDropDownItems = 6;
            this.integrationStepCombo.Name = "integrationStepCombo";
            this.integrationStepCombo.Size = new System.Drawing.Size(79, 21);
            this.integrationStepCombo.TabIndex = 44;
            this.integrationStepCombo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 41);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 43;
            this.label10.Text = "DCGain (dB)";
            // 
            // IFGainVal
            // 
            this.IFGainVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IFGainVal.Location = new System.Drawing.Point(77, 57);
            this.IFGainVal.Margin = new System.Windows.Forms.Padding(2);
            this.IFGainVal.Name = "IFGainVal";
            this.IFGainVal.Size = new System.Drawing.Size(44, 20);
            this.IFGainVal.TabIndex = 42;
            this.IFGainVal.TextChanged += new System.EventHandler(this.IFGainVal_TextChanged);
            // 
            // lblIFGain
            // 
            this.lblIFGain.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblIFGain.AutoSize = true;
            this.lblIFGain.Location = new System.Drawing.Point(74, 43);
            this.lblIFGain.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIFGain.Name = "lblIFGain";
            this.lblIFGain.Size = new System.Drawing.Size(60, 13);
            this.lblIFGain.TabIndex = 41;
            this.lblIFGain.Text = "IFGain (dB)";
            // 
            // finalizeSettingsButton
            // 
            this.finalizeSettingsButton.BackColor = System.Drawing.Color.DarkGray;
            this.finalizeSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.finalizeSettingsButton.Location = new System.Drawing.Point(309, 10);
            this.finalizeSettingsButton.Margin = new System.Windows.Forms.Padding(2);
            this.finalizeSettingsButton.Name = "finalizeSettingsButton";
            this.finalizeSettingsButton.Size = new System.Drawing.Size(97, 27);
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
            this.DCGain.Location = new System.Drawing.Point(4, 56);
            this.DCGain.Margin = new System.Windows.Forms.Padding(2);
            this.DCGain.MaxDropDownItems = 6;
            this.DCGain.Name = "DCGain";
            this.DCGain.Size = new System.Drawing.Size(57, 21);
            this.DCGain.TabIndex = 39;
            this.DCGain.SelectedIndexChanged += new System.EventHandler(this.DCGain_SelectedIndexChanged);
            // 
            // stopScanButton
            // 
            this.stopScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopScanButton.BackColor = System.Drawing.Color.OrangeRed;
            this.stopScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopScanButton.Location = new System.Drawing.Point(361, 44);
            this.stopScanButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopScanButton.Name = "stopScanButton";
            this.stopScanButton.Size = new System.Drawing.Size(45, 36);
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
            this.startScanButton.Location = new System.Drawing.Point(309, 44);
            this.startScanButton.Margin = new System.Windows.Forms.Padding(2);
            this.startScanButton.Name = "startScanButton";
            this.startScanButton.Size = new System.Drawing.Size(48, 36);
            this.startScanButton.TabIndex = 36;
            this.startScanButton.Text = "Start Scan";
            this.startScanButton.UseVisualStyleBackColor = false;
            this.startScanButton.Click += new System.EventHandler(this.startScan_Click);
            // 
            // frequency
            // 
            this.frequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.frequency.Location = new System.Drawing.Point(158, 18);
            this.frequency.Margin = new System.Windows.Forms.Padding(2);
            this.frequency.Name = "frequency";
            this.frequency.Size = new System.Drawing.Size(76, 20);
            this.frequency.TabIndex = 35;
            this.frequency.TextChanged += new System.EventHandler(this.frequency_TextChanged);
            // 
            // lblFrequency
            // 
            this.lblFrequency.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(155, 0);
            this.lblFrequency.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(85, 13);
            this.lblFrequency.TabIndex = 34;
            this.lblFrequency.Text = "Frequency (kHz)";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(212, 44);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Integration Step";
            // 
            // offsetVoltage
            // 
            this.offsetVoltage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.offsetVoltage.Location = new System.Drawing.Point(136, 57);
            this.offsetVoltage.Margin = new System.Windows.Forms.Padding(2);
            this.offsetVoltage.Name = "offsetVoltage";
            this.offsetVoltage.Size = new System.Drawing.Size(44, 20);
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
            this.scanTypeComboBox.Location = new System.Drawing.Point(4, 18);
            this.scanTypeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.scanTypeComboBox.MaxDropDownItems = 2;
            this.scanTypeComboBox.Name = "scanTypeComboBox";
            this.scanTypeComboBox.Size = new System.Drawing.Size(83, 21);
            this.scanTypeComboBox.TabIndex = 25;
            this.scanTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.scanTypeComboBox_SelectedIndexChanged);
            // 
            // FreeControlForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(4F, 8.7F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(724, 233);
            this.Controls.Add(this.spectraCyberGroupBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.manualGroupBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.overRideGroupbox);
            this.Controls.Add(this.errorLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(600, 100);
            this.Name = "FreeControlForm";
            this.Text = "Control Form";
            this.overRideGroupbox.ResumeLayout(false);
            this.overRideGroupbox.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.manualGroupBox.ResumeLayout(false);
            this.manualGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
            this.spectraCyberGroupBox.ResumeLayout(false);
            this.spectraCyberGroupBox.PerformLayout();
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
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ccwAzJogButton;
        private System.Windows.Forms.Button plusElaButton;
        private System.Windows.Forms.Button subElaButton;
        private System.Windows.Forms.Button cwAzJogButton;
        private System.Windows.Forms.TextBox speedTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton immediateRadioButton;
        private System.Windows.Forms.RadioButton ControlledButtonRadio;
        private System.Windows.Forms.Button manualControlButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
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
        private System.Windows.Forms.TrackBar speedTrackBar;
        private System.Windows.Forms.CheckBox SoftwareStopsCheckBox;
        private System.Windows.Forms.Button stopRT;
    }
}