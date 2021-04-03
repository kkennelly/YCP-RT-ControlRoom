﻿namespace ControlRoomApplication.Main
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Create error provider objects
       // private System.Windows.Forms.ErrorProvider txtPLCPortErrorProvider = new System.Windows.Forms.ErrorProvider();
       // txtPLCPortErrorProvider.SetIconAlignment(this.txtPLCPort, 2);


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
            this.startButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.txtPLCPort = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtPLCIP = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboPLCType = new System.Windows.Forms.ComboBox();
            this.FreeControl = new System.Windows.Forms.Button();
            this.comboSensorNetworkBox = new System.Windows.Forms.ComboBox();
            this.loopBackBox = new System.Windows.Forms.CheckBox();
            this.LocalIPCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.simulationSettingsGroupbox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWSCOMPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.portGroupbox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.sensorNetworkClientPort = new System.Windows.Forms.TextBox();
            this.sensorNetworkClientIPAddress = new System.Windows.Forms.TextBox();
            this.sensorNetworkServerPort = new System.Windows.Forms.TextBox();
            this.sensorNetworkServerIPAddress = new System.Windows.Forms.TextBox();
            this.txtMcuCOMPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.createWSButton = new System.Windows.Forms.Button();
            this.acceptSettings = new System.Windows.Forms.Button();
            this.startRTGroupbox = new System.Windows.Forms.GroupBox();
            this.helpButton = new System.Windows.Forms.Button();
            this.ProdcheckBox = new System.Windows.Forms.CheckBox();
            this.MCUIPToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.MCUPortToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PLCPortToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.WCOMPortToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.simulationSettingsGroupbox.SuspendLayout();
            this.portGroupbox.SuspendLayout();
            this.startRTGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.BackColor = System.Drawing.Color.LimeGreen;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.startButton.ForeColor = System.Drawing.Color.Black;
            this.startButton.Location = new System.Drawing.Point(197, 68);
            this.startButton.Margin = new System.Windows.Forms.Padding(2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(170, 40);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start RT";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView1.Location = new System.Drawing.Point(11, 22);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(509, 230);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // shutdownButton
            // 
            this.shutdownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shutdownButton.BackColor = System.Drawing.Color.Red;
            this.shutdownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.shutdownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.shutdownButton.Location = new System.Drawing.Point(13, 68);
            this.shutdownButton.Margin = new System.Windows.Forms.Padding(2);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(170, 40);
            this.shutdownButton.TabIndex = 7;
            this.shutdownButton.Text = "Shutdown RT";
            this.shutdownButton.UseVisualStyleBackColor = false;
            this.shutdownButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtPLCPort
            // 
            this.txtPLCPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPLCPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtPLCPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPLCPort.Location = new System.Drawing.Point(249, 42);
            this.txtPLCPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtPLCPort.Name = "txtPLCPort";
            this.txtPLCPort.Size = new System.Drawing.Size(118, 29);
            this.txtPLCPort.TabIndex = 5;
            this.txtPLCPort.TextChanged += new System.EventHandler(this.txtPLCPort_TextChanged);
            this.txtPLCPort.GotFocus += new System.EventHandler(this.textBox1_Focus);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Production SpectraCyber",
            "Simulated SpectraCyber"});
            this.comboBox1.Location = new System.Drawing.Point(260, 30);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(234, 28);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtPLCIP
            // 
            this.txtPLCIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPLCIP.BackColor = System.Drawing.Color.Gainsboro;
            this.txtPLCIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPLCIP.Location = new System.Drawing.Point(249, 13);
            this.txtPLCIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtPLCIP.Name = "txtPLCIP";
            this.txtPLCIP.Size = new System.Drawing.Size(118, 29);
            this.txtPLCIP.TabIndex = 4;
            this.txtPLCIP.TextChanged += new System.EventHandler(this.txtPLCIP_TextChanged);
            this.txtPLCIP.GotFocus += new System.EventHandler(this.textBox2_Focus);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Production Weather Station",
            "Simulated Weather Station",
            "Test Weather Station"});
            this.comboBox2.Location = new System.Drawing.Point(5, 76);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(247, 28);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(555, 232);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(140, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Populate local database";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboPLCType
            // 
            this.comboPLCType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboPLCType.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboPLCType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPLCType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboPLCType.FormattingEnabled = true;
            this.comboPLCType.Items.AddRange(new object[] {
            "Production PLC",
            "Scale PLC",
            "Simulated PLC",
            "Test PLC"});
            this.comboPLCType.Location = new System.Drawing.Point(260, 76);
            this.comboPLCType.Margin = new System.Windows.Forms.Padding(2);
            this.comboPLCType.Name = "comboPLCType";
            this.comboPLCType.Size = new System.Drawing.Size(234, 28);
            this.comboPLCType.TabIndex = 3;
            this.comboPLCType.SelectedIndexChanged += new System.EventHandler(this.comboPLCType_SelectedIndexChanged);
            // 
            // FreeControl
            // 
            this.FreeControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FreeControl.BackColor = System.Drawing.Color.LightGray;
            this.FreeControl.Enabled = false;
            this.FreeControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FreeControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.FreeControl.Location = new System.Drawing.Point(13, 15);
            this.FreeControl.Margin = new System.Windows.Forms.Padding(2);
            this.FreeControl.Name = "FreeControl";
            this.FreeControl.Size = new System.Drawing.Size(354, 44);
            this.FreeControl.TabIndex = 9;
            this.FreeControl.Text = "Radio Telescope Control";
            this.FreeControl.UseVisualStyleBackColor = false;
            this.FreeControl.Click += new System.EventHandler(this.FreeControl_Click);
            // 
            // comboSensorNetworkBox
            // 
            this.comboSensorNetworkBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboSensorNetworkBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSensorNetworkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSensorNetworkBox.FormattingEnabled = true;
            this.comboSensorNetworkBox.Items.AddRange(new object[] {
            "Production Sensor Network",
            "Simulated Sensor Network"});
            this.comboSensorNetworkBox.Location = new System.Drawing.Point(6, 30);
            this.comboSensorNetworkBox.Name = "comboSensorNetworkBox";
            this.comboSensorNetworkBox.Size = new System.Drawing.Size(246, 28);
            this.comboSensorNetworkBox.TabIndex = 14;
            this.comboSensorNetworkBox.SelectedIndexChanged += new System.EventHandler(this.comboSensorNetworkBox_SelectedIndexChanged);
            // 
            // loopBackBox
            // 
            this.loopBackBox.AutoSize = true;
            this.loopBackBox.Location = new System.Drawing.Point(699, 219);
            this.loopBackBox.Margin = new System.Windows.Forms.Padding(2);
            this.loopBackBox.Name = "loopBackBox";
            this.loopBackBox.Size = new System.Drawing.Size(93, 43);
            this.loopBackBox.TabIndex = 28;
            this.loopBackBox.Text = "Loop back \r\n(for simulation)\r\n ";
            this.loopBackBox.UseVisualStyleBackColor = true;
            this.loopBackBox.CheckedChanged += new System.EventHandler(this.loopBackBox_CheckedChanged);
            // 
            // LocalIPCombo
            // 
            this.LocalIPCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocalIPCombo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.LocalIPCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LocalIPCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocalIPCombo.FormattingEnabled = true;
            this.LocalIPCombo.Items.AddRange(new object[] {
            "RT IP Address",
            "127.0.0.1"});
            this.LocalIPCombo.Location = new System.Drawing.Point(140, 120);
            this.LocalIPCombo.Margin = new System.Windows.Forms.Padding(2);
            this.LocalIPCombo.Name = "LocalIPCombo";
            this.LocalIPCombo.Size = new System.Drawing.Size(234, 28);
            this.LocalIPCombo.TabIndex = 16;
            this.LocalIPCombo.SelectedIndexChanged += new System.EventHandler(this.LocalIPCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Click on the IP adress of the RT to open diagnostic form";
            // 
            // simulationSettingsGroupbox
            // 
            this.simulationSettingsGroupbox.BackColor = System.Drawing.Color.Gray;
            this.simulationSettingsGroupbox.Controls.Add(this.comboBox2);
            this.simulationSettingsGroupbox.Controls.Add(this.LocalIPCombo);
            this.simulationSettingsGroupbox.Controls.Add(this.comboSensorNetworkBox);
            this.simulationSettingsGroupbox.Controls.Add(this.comboPLCType);
            this.simulationSettingsGroupbox.Controls.Add(this.comboBox1);
            this.simulationSettingsGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.simulationSettingsGroupbox.Location = new System.Drawing.Point(12, 258);
            this.simulationSettingsGroupbox.Name = "simulationSettingsGroupbox";
            this.simulationSettingsGroupbox.Size = new System.Drawing.Size(499, 170);
            this.simulationSettingsGroupbox.TabIndex = 18;
            this.simulationSettingsGroupbox.TabStop = false;
            this.simulationSettingsGroupbox.Text = "Individual Component Simulation settings";
            this.simulationSettingsGroupbox.Enter += new System.EventHandler(this.simulationSettingsGroupbox_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "Weather station COM port:";
            this.WCOMPortToolTip.SetToolTip(this.label2, "Enter a valid port number, between 1 and 65536");
            this.label2.Click += new System.EventHandler(this.label2_MouseHover);
            // 
            // txtWSCOMPort
            // 
            this.txtWSCOMPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtWSCOMPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtWSCOMPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtWSCOMPort.Location = new System.Drawing.Point(249, 100);
            this.txtWSCOMPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtWSCOMPort.Name = "txtWSCOMPort";
            this.txtWSCOMPort.Size = new System.Drawing.Size(118, 29);
            this.txtWSCOMPort.TabIndex = 22;
            this.txtWSCOMPort.TextChanged += new System.EventHandler(this.txtWSCOMPort_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 18);
            this.label3.TabIndex = 19;
            this.label3.Text = " PLC port:";
            this.PLCPortToolTip.SetToolTip(this.label3, "Enter a valid port number, between 1 and 65536");
            this.label3.MouseHover += new System.EventHandler(this.label3_MouseHover);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 18);
            this.label4.TabIndex = 20;
            this.label4.Text = "MCU IP Address:";
            this.MCUIPToolTip.SetToolTip(this.label4, "Enter a valid IP Address (in the form xxx.xxx.xxx.xxx)");
            this.label4.MouseHover += new System.EventHandler(this.label4_MouseHover);
            // 
            // portGroupbox
            // 
            this.portGroupbox.Controls.Add(this.label7);
            this.portGroupbox.Controls.Add(this.label6);
            this.portGroupbox.Controls.Add(this.sensorNetworkClientPort);
            this.portGroupbox.Controls.Add(this.sensorNetworkClientIPAddress);
            this.portGroupbox.Controls.Add(this.sensorNetworkServerPort);
            this.portGroupbox.Controls.Add(this.sensorNetworkServerIPAddress);
            this.portGroupbox.Controls.Add(this.txtMcuCOMPort);
            this.portGroupbox.Controls.Add(this.label5);
            this.portGroupbox.Controls.Add(this.txtPLCPort);
            this.portGroupbox.Controls.Add(this.label4);
            this.portGroupbox.Controls.Add(this.txtPLCIP);
            this.portGroupbox.Controls.Add(this.label3);
            this.portGroupbox.Controls.Add(this.label2);
            this.portGroupbox.Controls.Add(this.txtWSCOMPort);
            this.portGroupbox.Location = new System.Drawing.Point(526, 25);
            this.portGroupbox.Name = "portGroupbox";
            this.portGroupbox.Size = new System.Drawing.Size(373, 195);
            this.portGroupbox.TabIndex = 21;
            this.portGroupbox.TabStop = false;
            this.portGroupbox.Text = "System IP Address and Port Numbers";
            this.portGroupbox.Enter += new System.EventHandler(this.portGroupbox_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 18);
            this.label7.TabIndex = 28;
            this.label7.Text = "Sensor Network Client:";
            this.WCOMPortToolTip.SetToolTip(this.label7, "Enter a valid port number, between 1 and 65536");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 18);
            this.label6.TabIndex = 27;
            this.label6.Text = "Sensor Network Sever:";
            this.WCOMPortToolTip.SetToolTip(this.label6, "Enter a valid port number, between 1 and 65536");
            // 
            // sensorNetworkClientPort
            // 
            this.sensorNetworkClientPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sensorNetworkClientPort.BackColor = System.Drawing.Color.Gainsboro;
            this.sensorNetworkClientPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.sensorNetworkClientPort.Location = new System.Drawing.Point(303, 159);
            this.sensorNetworkClientPort.Margin = new System.Windows.Forms.Padding(2);
            this.sensorNetworkClientPort.Name = "sensorNetworkClientPort";
            this.sensorNetworkClientPort.Size = new System.Drawing.Size(64, 29);
            this.sensorNetworkClientPort.TabIndex = 26;
            this.sensorNetworkClientPort.TextChanged += new System.EventHandler(this.sensorNetworkClientPort_TextChanged);
            this.sensorNetworkClientPort.Enter += new System.EventHandler(this.sensorNetworkClientPort_Enter);
            this.sensorNetworkClientPort.Leave += new System.EventHandler(this.sensorNetworkClientPort_Leave);
            // 
            // sensorNetworkClientIPAddress
            // 
            this.sensorNetworkClientIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sensorNetworkClientIPAddress.BackColor = System.Drawing.Color.Gainsboro;
            this.sensorNetworkClientIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.sensorNetworkClientIPAddress.Location = new System.Drawing.Point(197, 159);
            this.sensorNetworkClientIPAddress.Margin = new System.Windows.Forms.Padding(2);
            this.sensorNetworkClientIPAddress.Name = "sensorNetworkClientIPAddress";
            this.sensorNetworkClientIPAddress.Size = new System.Drawing.Size(104, 29);
            this.sensorNetworkClientIPAddress.TabIndex = 25;
            this.sensorNetworkClientIPAddress.TextChanged += new System.EventHandler(this.sensorNetworkClientIPAddress_TextChanged);
            this.sensorNetworkClientIPAddress.Enter += new System.EventHandler(this.sensorNetworkClientIPAddress_Enter);
            this.sensorNetworkClientIPAddress.Leave += new System.EventHandler(this.sensorNetworkClientIPAddress_Leave);
            // 
            // sensorNetworkServerPort
            // 
            this.sensorNetworkServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sensorNetworkServerPort.BackColor = System.Drawing.Color.Gainsboro;
            this.sensorNetworkServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.sensorNetworkServerPort.Location = new System.Drawing.Point(303, 130);
            this.sensorNetworkServerPort.Margin = new System.Windows.Forms.Padding(2);
            this.sensorNetworkServerPort.Name = "sensorNetworkServerPort";
            this.sensorNetworkServerPort.Size = new System.Drawing.Size(64, 29);
            this.sensorNetworkServerPort.TabIndex = 24;
            this.sensorNetworkServerPort.TextChanged += new System.EventHandler(this.sensorNetworkServerPort_TextChanged);
            this.sensorNetworkServerPort.Enter += new System.EventHandler(this.sensorNetworkServerPort_Enter);
            this.sensorNetworkServerPort.Leave += new System.EventHandler(this.sensorNetworkServerPort_Leave);
            // 
            // sensorNetworkServerIPAddress
            // 
            this.sensorNetworkServerIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sensorNetworkServerIPAddress.BackColor = System.Drawing.Color.Gainsboro;
            this.sensorNetworkServerIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.sensorNetworkServerIPAddress.Location = new System.Drawing.Point(197, 130);
            this.sensorNetworkServerIPAddress.Margin = new System.Windows.Forms.Padding(2);
            this.sensorNetworkServerIPAddress.Name = "sensorNetworkServerIPAddress";
            this.sensorNetworkServerIPAddress.Size = new System.Drawing.Size(104, 29);
            this.sensorNetworkServerIPAddress.TabIndex = 23;
            this.sensorNetworkServerIPAddress.TextChanged += new System.EventHandler(this.sensorNetworkServerIPAddress_TextChanged);
            this.sensorNetworkServerIPAddress.Enter += new System.EventHandler(this.sensorNetworkServerIPAddress_Enter);
            this.sensorNetworkServerIPAddress.Leave += new System.EventHandler(this.sensorNetworkServerIPAddress_Leave);
            // 
            // txtMcuCOMPort
            // 
            this.txtMcuCOMPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMcuCOMPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtMcuCOMPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtMcuCOMPort.Location = new System.Drawing.Point(249, 71);
            this.txtMcuCOMPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtMcuCOMPort.Name = "txtMcuCOMPort";
            this.txtMcuCOMPort.Size = new System.Drawing.Size(118, 29);
            this.txtMcuCOMPort.TabIndex = 18;
            this.txtMcuCOMPort.TextChanged += new System.EventHandler(this.txtMcuCOMPort_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 18);
            this.label5.TabIndex = 21;
            this.label5.Text = "MCU Port: ";
            this.MCUPortToolTip.SetToolTip(this.label5, "Enter a valid port number, between 1 and 65536");
            this.label5.Click += new System.EventHandler(this.label5_MouseHover);
            // 
            // createWSButton
            // 
            this.createWSButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createWSButton.BackColor = System.Drawing.Color.LightGray;
            this.createWSButton.Enabled = false;
            this.createWSButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.createWSButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createWSButton.Location = new System.Drawing.Point(723, 262);
            this.createWSButton.Margin = new System.Windows.Forms.Padding(2);
            this.createWSButton.Name = "createWSButton";
            this.createWSButton.Size = new System.Drawing.Size(170, 51);
            this.createWSButton.TabIndex = 22;
            this.createWSButton.Text = "Create Production Weather Station";
            this.createWSButton.UseVisualStyleBackColor = false;
            this.createWSButton.Click += new System.EventHandler(this.createWSButton_Click);
            // 
            // acceptSettings
            // 
            this.acceptSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptSettings.BackColor = System.Drawing.Color.LightGray;
            this.acceptSettings.Enabled = false;
            this.acceptSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.acceptSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptSettings.Location = new System.Drawing.Point(539, 262);
            this.acceptSettings.Margin = new System.Windows.Forms.Padding(2);
            this.acceptSettings.Name = "acceptSettings";
            this.acceptSettings.Size = new System.Drawing.Size(170, 51);
            this.acceptSettings.TabIndex = 23;
            this.acceptSettings.Text = "Finalize settings";
            this.acceptSettings.UseVisualStyleBackColor = false;
            this.acceptSettings.Click += new System.EventHandler(this.acceptSettings_Click);
            // 
            // startRTGroupbox
            // 
            this.startRTGroupbox.Controls.Add(this.FreeControl);
            this.startRTGroupbox.Controls.Add(this.shutdownButton);
            this.startRTGroupbox.Controls.Add(this.startButton);
            this.startRTGroupbox.Location = new System.Drawing.Point(526, 315);
            this.startRTGroupbox.Name = "startRTGroupbox";
            this.startRTGroupbox.Size = new System.Drawing.Size(381, 113);
            this.startRTGroupbox.TabIndex = 24;
            this.startRTGroupbox.TabStop = false;
            this.startRTGroupbox.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.Gainsboro;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.helpButton.Location = new System.Drawing.Point(879, 2);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(22, 24);
            this.helpButton.TabIndex = 27;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.helpButton_click);
            // 
            // ProdcheckBox
            // 
            this.ProdcheckBox.AutoSize = true;
            this.ProdcheckBox.Location = new System.Drawing.Point(801, 225);
            this.ProdcheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.ProdcheckBox.Name = "ProdcheckBox";
            this.ProdcheckBox.Size = new System.Drawing.Size(97, 30);
            this.ProdcheckBox.TabIndex = 28;
            this.ProdcheckBox.Text = "Default Vals \r\n(for production)";
            this.ProdcheckBox.UseVisualStyleBackColor = true;
            this.ProdcheckBox.CheckedChanged += new System.EventHandler(this.ProdcheckBox_CheckedChanged);
            // 
            // WCOMPortToolTip
            // 
            this.WCOMPortToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.WCOMPortToolTip_Popup);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(911, 432);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.startRTGroupbox);
            this.Controls.Add(this.acceptSettings);
            this.Controls.Add(this.createWSButton);
            this.Controls.Add(this.portGroupbox);
            this.Controls.Add(this.simulationSettingsGroupbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.loopBackBox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.ProdcheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(920, 365);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.simulationSettingsGroupbox.ResumeLayout(false);
            this.portGroupbox.ResumeLayout(false);
            this.portGroupbox.PerformLayout();
            this.startRTGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.TextBox txtPLCPort;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtPLCIP;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboPLCType;
        private System.Windows.Forms.Button FreeControl;
        private System.Windows.Forms.ComboBox comboSensorNetworkBox;
        private System.Windows.Forms.CheckBox loopBackBox;
        private System.Windows.Forms.ComboBox LocalIPCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox simulationSettingsGroupbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWSCOMPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox portGroupbox;
        private System.Windows.Forms.TextBox txtMcuCOMPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button createWSButton;
        private System.Windows.Forms.Button acceptSettings;
        private System.Windows.Forms.GroupBox startRTGroupbox;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.CheckBox ProdcheckBox;
        private System.Windows.Forms.ToolTip MCUIPToolTip;
        private System.Windows.Forms.ToolTip MCUPortToolTip;
        private System.Windows.Forms.ToolTip PLCPortToolTip;
        private System.Windows.Forms.ToolTip WCOMPortToolTip;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sensorNetworkClientPort;
        private System.Windows.Forms.TextBox sensorNetworkClientIPAddress;
        private System.Windows.Forms.TextBox sensorNetworkServerPort;
        private System.Windows.Forms.TextBox sensorNetworkServerIPAddress;







        //private void txtPLCPort_Validated(object sender, System.EventArgs e)
        //{
        //    if (IsPLCPortValid()){
        //        txtPLCPortErrorProvider.SetError(this.txtPLCPort, string.Empty);
        //    }
        //    else
        //    {
        //        txtPLCPortErrorProvider.SetError(this.txtPLCPort, "PLC Port is required.");
        //    }
        //}

        //private bool IsPLCPortValid()
        //{
        //    if((txtPLCPort.Text == "5012" || txtPLCPort.Text == "8080" || txtPLCPort.Text == "58006") && IsEmpty(txtPLCPort))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        //private bool IsEmpty (System.Windows.Forms.TextBox text)
        //{
        //    return (text.Text.Length == 0);
        //}

    }
}