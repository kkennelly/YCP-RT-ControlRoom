﻿namespace ControlRoomApplication.Main
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
            this.PosDecButton = new System.Windows.Forms.Button();
            this.NegDecButton = new System.Windows.Forms.Button();
            this.NegRAButton = new System.Windows.Forms.Button();
            this.PosRAButton = new System.Windows.Forms.Button();
            this.ActualRATextBox = new System.Windows.Forms.TextBox();
            this.ActualDecTextBox = new System.Windows.Forms.TextBox();
            this.ActualPositionLabel = new System.Windows.Forms.Label();
            this.ActualRALabel = new System.Windows.Forms.Label();
            this.ActualDecLabel = new System.Windows.Forms.Label();
            this.TargetDecLabel = new System.Windows.Forms.Label();
            this.TargetRALabel = new System.Windows.Forms.Label();
            this.TargetPositionLabel = new System.Windows.Forms.Label();
            this.TargetDecTextBox = new System.Windows.Forms.TextBox();
            this.TargetRATextBox = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RAIncGroupbox = new System.Windows.Forms.GroupBox();
            this.tenButton = new System.Windows.Forms.Button();
            this.fiveButton = new System.Windows.Forms.Button();
            this.oneButton = new System.Windows.Forms.Button();
            this.oneForthButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.decIncGroupbox = new System.Windows.Forms.GroupBox();
            this.tenButtonDec = new System.Windows.Forms.Button();
            this.fiveButtonDec = new System.Windows.Forms.Button();
            this.oneButtonDec = new System.Windows.Forms.Button();
            this.oneForthButtonDec = new System.Windows.Forms.Button();
            this.freeControlGroupbox = new System.Windows.Forms.GroupBox();
            this.controlScriptsCombo = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.manualControlButton = new System.Windows.Forms.Button();
            this.immediateRadioButton = new System.Windows.Forms.RadioButton();
            this.ControledButtonRadio = new System.Windows.Forms.RadioButton();
            this.speedComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.subJogButton = new System.Windows.Forms.Button();
            this.plusElaButton = new System.Windows.Forms.Button();
            this.subElaButton = new System.Windows.Forms.Button();
            this.plusJogButton = new System.Windows.Forms.Button();
            this.RAIncGroupbox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.decIncGroupbox.SuspendLayout();
            this.freeControlGroupbox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.manualGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosDecButton.BackColor = System.Drawing.Color.DarkGray;
            this.PosDecButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PosDecButton.Location = new System.Drawing.Point(297, 56);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(40, 40);
            this.PosDecButton.TabIndex = 0;
            this.PosDecButton.Text = "+ Dec";
            this.PosDecButton.UseVisualStyleBackColor = false;
            this.PosDecButton.Click += new System.EventHandler(this.PosDecButton_Click);
            // 
            // NegDecButton
            // 
            this.NegDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegDecButton.BackColor = System.Drawing.Color.DarkGray;
            this.NegDecButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NegDecButton.Location = new System.Drawing.Point(296, 151);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(40, 40);
            this.NegDecButton.TabIndex = 1;
            this.NegDecButton.Text = "- Dec";
            this.NegDecButton.UseVisualStyleBackColor = false;
            this.NegDecButton.Click += new System.EventHandler(this.NegDecButton_Click);
            // 
            // NegRAButton
            // 
            this.NegRAButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegRAButton.BackColor = System.Drawing.Color.DarkGray;
            this.NegRAButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NegRAButton.Location = new System.Drawing.Point(250, 105);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(40, 40);
            this.NegRAButton.TabIndex = 2;
            this.NegRAButton.Text = "- RA";
            this.NegRAButton.UseVisualStyleBackColor = false;
            this.NegRAButton.Click += new System.EventHandler(this.NegRAButton_Click);
            // 
            // PosRAButton
            // 
            this.PosRAButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosRAButton.BackColor = System.Drawing.Color.DarkGray;
            this.PosRAButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PosRAButton.Location = new System.Drawing.Point(341, 105);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(40, 40);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = false;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRATextBox.Location = new System.Drawing.Point(125, 68);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecTextBox.Location = new System.Drawing.Point(125, 121);
            this.ActualDecTextBox.Name = "ActualDecTextBox";
            this.ActualDecTextBox.ReadOnly = true;
            this.ActualDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualDecTextBox.TabIndex = 6;
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ActualPositionLabel.Location = new System.Drawing.Point(127, 16);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(114, 20);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(123, 50);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(84, 13);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "Right Ascension";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(123, 103);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(60, 13);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Declination";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(6, 103);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(60, 13);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Declination";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(7, 50);
            this.TargetRALabel.Name = "TargetRALabel";
            this.TargetRALabel.Size = new System.Drawing.Size(84, 13);
            this.TargetRALabel.TabIndex = 13;
            this.TargetRALabel.Text = "Right Ascension";
            // 
            // TargetPositionLabel
            // 
            this.TargetPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetPositionLabel.AutoSize = true;
            this.TargetPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TargetPositionLabel.Location = new System.Drawing.Point(6, 16);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(115, 20);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecTextBox.Location = new System.Drawing.Point(8, 121);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRATextBox.Location = new System.Drawing.Point(8, 68);
            this.TargetRATextBox.Name = "TargetRATextBox";
            this.TargetRATextBox.ReadOnly = true;
            this.TargetRATextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetRATextBox.TabIndex = 10;
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.Title.Location = new System.Drawing.Point(11, 9);
            this.Title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(462, 46);
            this.Title.TabIndex = 15;
            this.Title.Text = "Radio Telescope Control";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // RAIncGroupbox
            // 
            this.RAIncGroupbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.RAIncGroupbox.BackColor = System.Drawing.Color.Gray;
            this.RAIncGroupbox.Controls.Add(this.tenButton);
            this.RAIncGroupbox.Controls.Add(this.fiveButton);
            this.RAIncGroupbox.Controls.Add(this.oneButton);
            this.RAIncGroupbox.Controls.Add(this.oneForthButton);
            this.RAIncGroupbox.Location = new System.Drawing.Point(6, 54);
            this.RAIncGroupbox.Margin = new System.Windows.Forms.Padding(2);
            this.RAIncGroupbox.Name = "RAIncGroupbox";
            this.RAIncGroupbox.Padding = new System.Windows.Forms.Padding(2);
            this.RAIncGroupbox.Size = new System.Drawing.Size(230, 55);
            this.RAIncGroupbox.TabIndex = 16;
            this.RAIncGroupbox.TabStop = false;
            this.RAIncGroupbox.Text = "Right Ascension Increment";
            this.RAIncGroupbox.Enter += new System.EventHandler(this.RAIncGroupbox_Enter);
            // 
            // tenButton
            // 
            this.tenButton.BackColor = System.Drawing.Color.DarkGray;
            this.tenButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.tenButton.Location = new System.Drawing.Point(182, 18);
            this.tenButton.Margin = new System.Windows.Forms.Padding(2);
            this.tenButton.Name = "tenButton";
            this.tenButton.Size = new System.Drawing.Size(40, 30);
            this.tenButton.TabIndex = 3;
            this.tenButton.Text = "10";
            this.tenButton.UseVisualStyleBackColor = false;
            this.tenButton.Click += new System.EventHandler(this.tenButton_Click);
            // 
            // fiveButton
            // 
            this.fiveButton.BackColor = System.Drawing.Color.DarkGray;
            this.fiveButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fiveButton.Location = new System.Drawing.Point(124, 18);
            this.fiveButton.Margin = new System.Windows.Forms.Padding(2);
            this.fiveButton.Name = "fiveButton";
            this.fiveButton.Size = new System.Drawing.Size(40, 30);
            this.fiveButton.TabIndex = 2;
            this.fiveButton.Text = "5";
            this.fiveButton.UseVisualStyleBackColor = false;
            this.fiveButton.Click += new System.EventHandler(this.fiveButton_Click);
            // 
            // oneButton
            // 
            this.oneButton.BackColor = System.Drawing.Color.DarkGray;
            this.oneButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneButton.Location = new System.Drawing.Point(65, 18);
            this.oneButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneButton.Name = "oneButton";
            this.oneButton.Size = new System.Drawing.Size(40, 30);
            this.oneButton.TabIndex = 1;
            this.oneButton.Text = "1";
            this.oneButton.UseVisualStyleBackColor = false;
            this.oneButton.Click += new System.EventHandler(this.oneButton_Click);
            // 
            // oneForthButton
            // 
            this.oneForthButton.BackColor = System.Drawing.Color.DarkGray;
            this.oneForthButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneForthButton.Location = new System.Drawing.Point(7, 18);
            this.oneForthButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneForthButton.Name = "oneForthButton";
            this.oneForthButton.Size = new System.Drawing.Size(40, 30);
            this.oneForthButton.TabIndex = 0;
            this.oneForthButton.Text = "0.25";
            this.oneForthButton.UseVisualStyleBackColor = false;
            this.oneForthButton.Click += new System.EventHandler(this.oneForthButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.BackColor = System.Drawing.Color.OrangeRed;
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.editButton.Location = new System.Drawing.Point(250, 10);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(131, 25);
            this.editButton.TabIndex = 17;
            this.editButton.Text = "Edit Position";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(283, 422);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(230, 19);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.ActualRATextBox);
            this.groupBox1.Controls.Add(this.ActualDecTextBox);
            this.groupBox1.Controls.Add(this.ActualPositionLabel);
            this.groupBox1.Controls.Add(this.ActualRALabel);
            this.groupBox1.Controls.Add(this.ActualDecLabel);
            this.groupBox1.Controls.Add(this.TargetDecLabel);
            this.groupBox1.Controls.Add(this.TargetRATextBox);
            this.groupBox1.Controls.Add(this.TargetRALabel);
            this.groupBox1.Controls.Add(this.TargetDecTextBox);
            this.groupBox1.Controls.Add(this.TargetPositionLabel);
            this.groupBox1.Location = new System.Drawing.Point(19, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 149);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position Information";
            // 
            // decIncGroupbox
            // 
            this.decIncGroupbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.decIncGroupbox.BackColor = System.Drawing.Color.Gray;
            this.decIncGroupbox.Controls.Add(this.tenButtonDec);
            this.decIncGroupbox.Controls.Add(this.fiveButtonDec);
            this.decIncGroupbox.Controls.Add(this.oneButtonDec);
            this.decIncGroupbox.Controls.Add(this.oneForthButtonDec);
            this.decIncGroupbox.Location = new System.Drawing.Point(6, 128);
            this.decIncGroupbox.Margin = new System.Windows.Forms.Padding(2);
            this.decIncGroupbox.Name = "decIncGroupbox";
            this.decIncGroupbox.Padding = new System.Windows.Forms.Padding(2);
            this.decIncGroupbox.Size = new System.Drawing.Size(230, 55);
            this.decIncGroupbox.TabIndex = 20;
            this.decIncGroupbox.TabStop = false;
            this.decIncGroupbox.Text = "Declanation Increment";
            // 
            // tenButtonDec
            // 
            this.tenButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.tenButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.tenButtonDec.Location = new System.Drawing.Point(182, 18);
            this.tenButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.tenButtonDec.Name = "tenButtonDec";
            this.tenButtonDec.Size = new System.Drawing.Size(40, 30);
            this.tenButtonDec.TabIndex = 3;
            this.tenButtonDec.Text = "10";
            this.tenButtonDec.UseVisualStyleBackColor = false;
            // 
            // fiveButtonDec
            // 
            this.fiveButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.fiveButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fiveButtonDec.Location = new System.Drawing.Point(124, 18);
            this.fiveButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.fiveButtonDec.Name = "fiveButtonDec";
            this.fiveButtonDec.Size = new System.Drawing.Size(40, 30);
            this.fiveButtonDec.TabIndex = 2;
            this.fiveButtonDec.Text = "5";
            this.fiveButtonDec.UseVisualStyleBackColor = false;
            // 
            // oneButtonDec
            // 
            this.oneButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.oneButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneButtonDec.Location = new System.Drawing.Point(65, 18);
            this.oneButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.oneButtonDec.Name = "oneButtonDec";
            this.oneButtonDec.Size = new System.Drawing.Size(40, 30);
            this.oneButtonDec.TabIndex = 1;
            this.oneButtonDec.Text = "1";
            this.oneButtonDec.UseVisualStyleBackColor = false;
            // 
            // oneForthButtonDec
            // 
            this.oneForthButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.oneForthButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneForthButtonDec.Location = new System.Drawing.Point(7, 18);
            this.oneForthButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.oneForthButtonDec.Name = "oneForthButtonDec";
            this.oneForthButtonDec.Size = new System.Drawing.Size(40, 30);
            this.oneForthButtonDec.TabIndex = 0;
            this.oneForthButtonDec.Text = "0.25";
            this.oneForthButtonDec.UseVisualStyleBackColor = false;
            // 
            // freeControlGroupbox
            // 
            this.freeControlGroupbox.BackColor = System.Drawing.Color.Gainsboro;
            this.freeControlGroupbox.Controls.Add(this.NegRAButton);
            this.freeControlGroupbox.Controls.Add(this.PosDecButton);
            this.freeControlGroupbox.Controls.Add(this.decIncGroupbox);
            this.freeControlGroupbox.Controls.Add(this.NegDecButton);
            this.freeControlGroupbox.Controls.Add(this.PosRAButton);
            this.freeControlGroupbox.Controls.Add(this.RAIncGroupbox);
            this.freeControlGroupbox.Controls.Add(this.editButton);
            this.freeControlGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.freeControlGroupbox.Location = new System.Drawing.Point(19, 213);
            this.freeControlGroupbox.Name = "freeControlGroupbox";
            this.freeControlGroupbox.Size = new System.Drawing.Size(405, 201);
            this.freeControlGroupbox.TabIndex = 22;
            this.freeControlGroupbox.TabStop = false;
            this.freeControlGroupbox.Text = "Edit Target Position";
            // 
            // controlScriptsCombo
            // 
            this.controlScriptsCombo.BackColor = System.Drawing.Color.DarkGray;
            this.controlScriptsCombo.FormattingEnabled = true;
            this.controlScriptsCombo.Items.AddRange(new object[] {
            "Snow Dump",
            "Stow Telescope",
            "Full 360 Clockwise",
            "Full 360 Counter-Clockwise",
            "Calibrate Telescope"});
            this.controlScriptsCombo.Location = new System.Drawing.Point(20, 27);
            this.controlScriptsCombo.Name = "controlScriptsCombo";
            this.controlScriptsCombo.Size = new System.Drawing.Size(260, 21);
            this.controlScriptsCombo.TabIndex = 23;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.controlScriptsCombo);
            this.groupBox4.Location = new System.Drawing.Point(353, 77);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(404, 69);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control Scripts";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkGray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(308, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 24;
            this.button1.Text = "Edit Scripts";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.manualGroupBox.Controls.Add(this.label4);
            this.manualGroupBox.Controls.Add(this.label5);
            this.manualGroupBox.Controls.Add(this.label3);
            this.manualGroupBox.Controls.Add(this.manualControlButton);
            this.manualGroupBox.Controls.Add(this.immediateRadioButton);
            this.manualGroupBox.Controls.Add(this.ControledButtonRadio);
            this.manualGroupBox.Controls.Add(this.speedComboBox);
            this.manualGroupBox.Controls.Add(this.label2);
            this.manualGroupBox.Controls.Add(this.label1);
            this.manualGroupBox.Controls.Add(this.subJogButton);
            this.manualGroupBox.Controls.Add(this.plusElaButton);
            this.manualGroupBox.Controls.Add(this.subElaButton);
            this.manualGroupBox.Controls.Add(this.plusJogButton);
            this.manualGroupBox.Location = new System.Drawing.Point(455, 179);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Size = new System.Drawing.Size(302, 235);
            this.manualGroupBox.TabIndex = 25;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "Manual Control";
            this.manualGroupBox.Enter += new System.EventHandler(this.manualGroupBox_Enter);
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
            this.label3.Location = new System.Drawing.Point(3, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Speed";
            // 
            // manualControlButton
            // 
            this.manualControlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.manualControlButton.BackColor = System.Drawing.Color.OrangeRed;
            this.manualControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.manualControlButton.Location = new System.Drawing.Point(140, 26);
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
            this.immediateRadioButton.Location = new System.Drawing.Point(6, 146);
            this.immediateRadioButton.Name = "immediateRadioButton";
            this.immediateRadioButton.Size = new System.Drawing.Size(98, 17);
            this.immediateRadioButton.TabIndex = 24;
            this.immediateRadioButton.Text = "Immediate Stop";
            this.immediateRadioButton.UseVisualStyleBackColor = true;
            // 
            // ControledButtonRadio
            // 
            this.ControledButtonRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ControledButtonRadio.AutoSize = true;
            this.ControledButtonRadio.Checked = true;
            this.ControledButtonRadio.Location = new System.Drawing.Point(6, 123);
            this.ControledButtonRadio.Name = "ControledButtonRadio";
            this.ControledButtonRadio.Size = new System.Drawing.Size(97, 17);
            this.ControledButtonRadio.TabIndex = 23;
            this.ControledButtonRadio.TabStop = true;
            this.ControledButtonRadio.Text = "Controlled Stop";
            this.ControledButtonRadio.UseVisualStyleBackColor = true;
            // 
            // speedComboBox
            // 
            this.speedComboBox.FormattingEnabled = true;
            this.speedComboBox.Items.AddRange(new object[] {
            "0.1 RPM",
            "2 RPM"});
            this.speedComboBox.Location = new System.Drawing.Point(6, 189);
            this.speedComboBox.Name = "speedComboBox";
            this.speedComboBox.Size = new System.Drawing.Size(121, 21);
            this.speedComboBox.TabIndex = 10;
            this.speedComboBox.SelectedIndexChanged += new System.EventHandler(this.speedComboBox_SelectedIndexChanged);
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
            this.label1.Text = "Current Elavation: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // subJogButton
            // 
            this.subJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.subJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subJogButton.Location = new System.Drawing.Point(155, 139);
            this.subJogButton.Name = "subJogButton";
            this.subJogButton.Size = new System.Drawing.Size(40, 40);
            this.subJogButton.TabIndex = 6;
            this.subJogButton.Text = "- Jog";
            this.subJogButton.UseVisualStyleBackColor = false;
            this.subJogButton.Click += new System.EventHandler(this.subJogButton_Click);
            // 
            // plusElaButton
            // 
            this.plusElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusElaButton.Location = new System.Drawing.Point(196, 88);
            this.plusElaButton.Name = "plusElaButton";
            this.plusElaButton.Size = new System.Drawing.Size(40, 40);
            this.plusElaButton.TabIndex = 4;
            this.plusElaButton.Text = "+ Ela";
            this.plusElaButton.UseVisualStyleBackColor = false;
            // 
            // subElaButton
            // 
            this.subElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.subElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subElaButton.Location = new System.Drawing.Point(196, 185);
            this.subElaButton.Name = "subElaButton";
            this.subElaButton.Size = new System.Drawing.Size(40, 40);
            this.subElaButton.TabIndex = 5;
            this.subElaButton.Text = "- Ela";
            this.subElaButton.UseVisualStyleBackColor = false;
            // 
            // plusJogButton
            // 
            this.plusJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusJogButton.Location = new System.Drawing.Point(241, 139);
            this.plusJogButton.Name = "plusJogButton";
            this.plusJogButton.Size = new System.Drawing.Size(40, 40);
            this.plusJogButton.TabIndex = 7;
            this.plusJogButton.Text = "+ Jog";
            this.plusJogButton.UseVisualStyleBackColor = false;
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.manualGroupBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.freeControlGroupbox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.Title);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "FreeControlForm";
            this.Text = "FreeControlForm";
            this.RAIncGroupbox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.decIncGroupbox.ResumeLayout(false);
            this.freeControlGroupbox.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.manualGroupBox.ResumeLayout(false);
            this.manualGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PosDecButton;
        private System.Windows.Forms.Button NegDecButton;
        private System.Windows.Forms.Button NegRAButton;
        private System.Windows.Forms.Button PosRAButton;
        private System.Windows.Forms.TextBox ActualRATextBox;
        private System.Windows.Forms.TextBox ActualDecTextBox;
        private System.Windows.Forms.Label ActualPositionLabel;
        private System.Windows.Forms.Label ActualRALabel;
        private System.Windows.Forms.Label ActualDecLabel;
        private System.Windows.Forms.Label TargetDecLabel;
        private System.Windows.Forms.Label TargetRALabel;
        private System.Windows.Forms.Label TargetPositionLabel;
        private System.Windows.Forms.TextBox TargetDecTextBox;
        private System.Windows.Forms.TextBox TargetRATextBox;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox RAIncGroupbox;
        private System.Windows.Forms.Button tenButton;
        private System.Windows.Forms.Button fiveButton;
        private System.Windows.Forms.Button oneButton;
        private System.Windows.Forms.Button oneForthButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox decIncGroupbox;
        private System.Windows.Forms.Button tenButtonDec;
        private System.Windows.Forms.Button fiveButtonDec;
        private System.Windows.Forms.Button oneButtonDec;
        private System.Windows.Forms.Button oneForthButtonDec;
        private System.Windows.Forms.GroupBox freeControlGroupbox;
        private System.Windows.Forms.ComboBox controlScriptsCombo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button subJogButton;
        private System.Windows.Forms.Button plusElaButton;
        private System.Windows.Forms.Button subElaButton;
        private System.Windows.Forms.Button plusJogButton;
        private System.Windows.Forms.ComboBox speedComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton immediateRadioButton;
        private System.Windows.Forms.RadioButton ControledButtonRadio;
        private System.Windows.Forms.Button manualControlButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}