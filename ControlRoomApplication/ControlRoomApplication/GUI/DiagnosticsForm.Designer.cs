namespace ControlRoomApplication.GUI
{
    partial class DiagnosticsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series22 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series23 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series24 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series25 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series26 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.windSpeedLabel = new System.Windows.Forms.Label();
            this.windDirLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.startTimeTextBox = new System.Windows.Forms.TextBox();
            this.endTimeTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblAzimuthTemp = new System.Windows.Forms.Label();
            this.lblElevationTemp = new System.Windows.Forms.Label();
            this.fldAzTemp = new System.Windows.Forms.Label();
            this.fldElTemp = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.selectDemo = new System.Windows.Forms.CheckBox();
            this.lblAbsEncoder = new System.Windows.Forms.Label();
            this.lblEncoderDegrees = new System.Windows.Forms.Label();
            this.lblAzEncoderDegrees = new System.Windows.Forms.Label();
            this.lblEncoderTicks = new System.Windows.Forms.Label();
            this.lblAzEncoderTicks = new System.Windows.Forms.Label();
            this.btnAddOneEncoder = new System.Windows.Forms.Button();
            this.btnAddFiveEncoder = new System.Windows.Forms.Button();
            this.btnAddXEncoder = new System.Windows.Forms.Button();
            this.btnSubtractOneEncoder = new System.Windows.Forms.Button();
            this.btnSubtractFiveEncoder = new System.Windows.Forms.Button();
            this.btnSubtractXEncoder = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCustEncoderVal = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.lblElEncoderTicks = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblElEncoderDegrees = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.runDiagScriptsButton = new System.Windows.Forms.Button();
            this.diagnosticScriptCombo = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grpAmbTempHumid = new System.Windows.Forms.GroupBox();
            this.lblAmbientDewPoint = new System.Windows.Forms.Label();
            this.lblAmbientDewPointUnit = new System.Windows.Forms.Label();
            this.fldAmbientDewPoint = new System.Windows.Forms.Label();
            this.lblAmbientTempUnit = new System.Windows.Forms.Label();
            this.lblAmbientHumidityUnit = new System.Windows.Forms.Label();
            this.lblAmbientTemp = new System.Windows.Forms.Label();
            this.fldAmbientHumidity = new System.Windows.Forms.Label();
            this.fldAmbientTemp = new System.Windows.Forms.Label();
            this.lblAmbientHumidity = new System.Windows.Forms.Label();
            this.grpAccelerometerSensorData = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.pnlCounterbalanceAccelerometer = new System.Windows.Forms.Panel();
            this.lblCbDisabled = new System.Windows.Forms.Label();
            this.counterBalanceAccChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label50 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.pnlElevationMotorAccelerometer = new System.Windows.Forms.Panel();
            this.lblElDisabled = new System.Windows.Forms.Label();
            this.elevationAccChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label47 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.pnlAzimuthMotorAccelerometer = new System.Windows.Forms.Panel();
            this.lblAzDisabled = new System.Windows.Forms.Label();
            this.azimuthAccChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.grpSensorData = new System.Windows.Forms.GroupBox();
            this.lbGateStat = new System.Windows.Forms.Label();
            this.lbEstopStat = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.lblAzHome1 = new System.Windows.Forms.Label();
            this.lblElLimStatus2 = new System.Windows.Forms.Label();
            this.lblElLimStatus1 = new System.Windows.Forms.Label();
            this.lblElHome = new System.Windows.Forms.Label();
            this.lblELHomeStatus = new System.Windows.Forms.Label();
            this.lblAzHomeStatus1 = new System.Windows.Forms.Label();
            this.lblAzLimit1 = new System.Windows.Forms.Label();
            this.lblAzLimit2 = new System.Windows.Forms.Label();
            this.grpMcuStatus = new System.Windows.Forms.GroupBox();
            this.lblMCUStatus = new System.Windows.Forms.Label();
            this.lblMCUStatusText = new System.Windows.Forms.Label();
            this.lblMCUErrors = new System.Windows.Forms.Label();
            this.btnResetMcuErrors = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.farTempConvert = new System.Windows.Forms.Button();
            this.celTempConvert = new System.Windows.Forms.Button();
            this.grpAbsoluteMotorPositionsTemperatures = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label32 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.lblCurrentAzOrientation = new System.Windows.Forms.Label();
            this.lblCurrentElOrientation = new System.Windows.Forms.Label();
            this.lblElAbsPos = new System.Windows.Forms.Label();
            this.lblAzAbsPos = new System.Windows.Forms.Label();
            this.AZTempUnitLabel = new System.Windows.Forms.Label();
            this.ElTempUnitLabel = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.InsideTempUnits = new System.Windows.Forms.Label();
            this.rainRateUnits = new System.Windows.Forms.Label();
            this.pressUnits = new System.Windows.Forms.Label();
            this.dailyRainfallUnits = new System.Windows.Forms.Label();
            this.outTempUnits = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.windSpeedUnits = new System.Windows.Forms.Label();
            this.insideTempLabel = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.rainRateLabel = new System.Windows.Forms.Label();
            this.barometricPressureLabel = new System.Windows.Forms.Label();
            this.dailyRainfallLabel = new System.Windows.Forms.Label();
            this.outsideTempLabel = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Encoders = new System.Windows.Forms.GroupBox();
            this.ElecationAbsoluteEncoder_lbl = new System.Windows.Forms.Label();
            this.btnElevationAbsoluteEncoder = new System.Windows.Forms.Button();
            this.btnAzimuthAbsoluteEncoder = new System.Windows.Forms.Button();
            this.AzimuthAbsoluteEncoder_lbl = new System.Windows.Forms.Label();
            this.SensorNetworkSensorInitialization = new System.Windows.Forms.GroupBox();
            this.AmbientTempHumid = new System.Windows.Forms.CheckBox();
            this.lblInitTimeout = new System.Windows.Forms.Label();
            this.lblDataTimeout = new System.Windows.Forms.Label();
            this.txtDataTimeout = new System.Windows.Forms.TextBox();
            this.txtInitTimeout = new System.Windows.Forms.TextBox();
            this.lblSNStatus = new System.Windows.Forms.Label();
            this.UpdateSensorInitiliazation = new System.Windows.Forms.Button();
            this.AzimuthEncoder = new System.Windows.Forms.CheckBox();
            this.ElevationEncoder = new System.Windows.Forms.CheckBox();
            this.CounterbalanceAccelerometer = new System.Windows.Forms.CheckBox();
            this.ElevationAccelerometer = new System.Windows.Forms.CheckBox();
            this.AzimuthAccelerometer = new System.Windows.Forms.CheckBox();
            this.AzimuthTemperature1 = new System.Windows.Forms.CheckBox();
            this.ElevationTemperature1 = new System.Windows.Forms.CheckBox();
            this.ThresholdsGroup = new System.Windows.Forms.GroupBox();
            this.txtLowerHumidLimit = new System.Windows.Forms.TextBox();
            this.txtUpperHumidLimit = new System.Windows.Forms.TextBox();
            this.txtLowerTempLimit = new System.Windows.Forms.TextBox();
            this.txtUpperTempLimit = new System.Windows.Forms.TextBox();
            this.lblLower = new System.Windows.Forms.Label();
            this.lblUpper = new System.Windows.Forms.Label();
            this.lblAmbHumid = new System.Windows.Forms.Label();
            this.lblAmbTemp = new System.Windows.Forms.Label();
            this.lblSWStop = new System.Windows.Forms.Label();
            this.UpdateThresholdsButton = new System.Windows.Forms.Button();
            this.txtLowerSWStopsLimit = new System.Windows.Forms.TextBox();
            this.txtUpperSWStopsLimit = new System.Windows.Forms.TextBox();
            this.grpProximitySensors = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.ElivationLimitSwitch0 = new System.Windows.Forms.Button();
            this.ElevationLimitSwitch90 = new System.Windows.Forms.Button();
            this.TemperatureSensors = new System.Windows.Forms.GroupBox();
            this.AmbTempHumidSensOverride = new System.Windows.Forms.Button();
            this.ElMotTempSensOverride = new System.Windows.Forms.Button();
            this.lblTempHumidOverride = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.AzMotTempSensOverride = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.GatesSensors = new System.Windows.Forms.GroupBox();
            this.MGOverride = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.Accelerometers = new System.Windows.Forms.GroupBox();
            this.btnElevationMotorAccelerometerOverride = new System.Windows.Forms.Button();
            this.btnCounterbalanceMotorAccelerometerOverride = new System.Windows.Forms.Button();
            this.btnAzimuthMotorAccelerometerOverride = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.WeatherStation = new System.Windows.Forms.GroupBox();
            this.WSOverride = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lblModeType = new System.Windows.Forms.Label();
            this.spectraModeTypeVal = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.frequencyVal = new System.Windows.Forms.Label();
            this.spectraCyberScanChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblIntegrationStep = new System.Windows.Forms.Label();
            this.IntegrationStepVal = new System.Windows.Forms.Label();
            this.lblDCGain = new System.Windows.Forms.Label();
            this.DCGainVal = new System.Windows.Forms.Label();
            this.lblIFGain = new System.Windows.Forms.Label();
            this.IFGainVal = new System.Windows.Forms.Label();
            this.lblBandwidth = new System.Windows.Forms.Label();
            this.BandwidthVal = new System.Windows.Forms.Label();
            this.lblOffsetVoltage = new System.Windows.Forms.Label();
            this.OffsetVoltageVal = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.consoleLogBox = new System.Windows.Forms.TextBox();
            this.DataTimeoutValidation = new System.Windows.Forms.ToolTip(this.components);
            this.InitTimeoutValidation = new System.Windows.Forms.ToolTip(this.components);
            this.UpperSWStopsValidation = new System.Windows.Forms.ToolTip(this.components);
            this.LowerSWStopsValidation = new System.Windows.Forms.ToolTip(this.components);
            this.AmbTempValidation = new System.Windows.Forms.ToolTip(this.components);
            this.AmbHumidValidation = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grpAmbTempHumid.SuspendLayout();
            this.grpAccelerometerSensorData.SuspendLayout();
            this.pnlCounterbalanceAccelerometer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.counterBalanceAccChart)).BeginInit();
            this.pnlElevationMotorAccelerometer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.elevationAccChart)).BeginInit();
            this.pnlAzimuthMotorAccelerometer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.azimuthAccChart)).BeginInit();
            this.grpSensorData.SuspendLayout();
            this.grpMcuStatus.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.grpAbsoluteMotorPositionsTemperatures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.Encoders.SuspendLayout();
            this.SensorNetworkSensorInitialization.SuspendLayout();
            this.ThresholdsGroup.SuspendLayout();
            this.grpProximitySensors.SuspendLayout();
            this.TemperatureSensors.SuspendLayout();
            this.GatesSensors.SuspendLayout();
            this.Accelerometers.SuspendLayout();
            this.WeatherStation.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectraCyberScanChart)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(3, 6);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(331, 178);
            this.dataGridView1.TabIndex = 0;
            // 
            // windSpeedLabel
            // 
            this.windSpeedLabel.AutoSize = true;
            this.windSpeedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windSpeedLabel.Location = new System.Drawing.Point(203, 46);
            this.windSpeedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windSpeedLabel.Name = "windSpeedLabel";
            this.windSpeedLabel.Size = new System.Drawing.Size(22, 18);
            this.windSpeedLabel.TabIndex = 3;
            this.windSpeedLabel.Text = " --";
            // 
            // windDirLabel
            // 
            this.windDirLabel.AutoSize = true;
            this.windDirLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windDirLabel.Location = new System.Drawing.Point(202, 22);
            this.windDirLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windDirLabel.Name = "windDirLabel";
            this.windDirLabel.Size = new System.Drawing.Size(23, 20);
            this.windDirLabel.TabIndex = 4;
            this.windDirLabel.Text = " --";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(2, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Start Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(2, 57);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "End Time";
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(5, 35);
            this.startTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(135, 20);
            this.startTimeTextBox.TabIndex = 8;
            // 
            // endTimeTextBox
            // 
            this.endTimeTextBox.Location = new System.Drawing.Point(5, 76);
            this.endTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.endTimeTextBox.Name = "endTimeTextBox";
            this.endTimeTextBox.Size = new System.Drawing.Size(135, 20);
            this.endTimeTextBox.TabIndex = 9;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(161, 57);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(103, 20);
            this.statusTextBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(158, 35);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 11;
            this.label8.Text = "Status";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblAzimuthTemp
            // 
            this.lblAzimuthTemp.AutoSize = true;
            this.lblAzimuthTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzimuthTemp.Location = new System.Drawing.Point(10, 9);
            this.lblAzimuthTemp.Name = "lblAzimuthTemp";
            this.lblAzimuthTemp.Size = new System.Drawing.Size(152, 16);
            this.lblAzimuthTemp.TabIndex = 14;
            this.lblAzimuthTemp.Text = "Azimuth Motor Temp:";
            // 
            // lblElevationTemp
            // 
            this.lblElevationTemp.AutoSize = true;
            this.lblElevationTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElevationTemp.Location = new System.Drawing.Point(11, 26);
            this.lblElevationTemp.Name = "lblElevationTemp";
            this.lblElevationTemp.Size = new System.Drawing.Size(164, 16);
            this.lblElevationTemp.TabIndex = 15;
            this.lblElevationTemp.Text = "Elevation Motor Temp:";
            // 
            // fldAzTemp
            // 
            this.fldAzTemp.AutoSize = true;
            this.fldAzTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAzTemp.Location = new System.Drawing.Point(202, 7);
            this.fldAzTemp.Name = "fldAzTemp";
            this.fldAzTemp.Size = new System.Drawing.Size(18, 18);
            this.fldAzTemp.TabIndex = 18;
            this.fldAzTemp.Text = "--";
            // 
            // fldElTemp
            // 
            this.fldElTemp.AutoSize = true;
            this.fldElTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldElTemp.Location = new System.Drawing.Point(202, 29);
            this.fldElTemp.Name = "fldElTemp";
            this.fldElTemp.Size = new System.Drawing.Size(18, 18);
            this.fldElTemp.TabIndex = 19;
            this.fldElTemp.Text = "--";
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.LimeGreen;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTest.Location = new System.Drawing.Point(115, 16);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(69, 27);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "Test ";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // selectDemo
            // 
            this.selectDemo.AutoSize = true;
            this.selectDemo.Location = new System.Drawing.Point(13, 22);
            this.selectDemo.Name = "selectDemo";
            this.selectDemo.Size = new System.Drawing.Size(77, 17);
            this.selectDemo.TabIndex = 29;
            this.selectDemo.Text = "Run Demo";
            this.selectDemo.UseVisualStyleBackColor = true;
            // 
            // lblAbsEncoder
            // 
            this.lblAbsEncoder.AutoSize = true;
            this.lblAbsEncoder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbsEncoder.Location = new System.Drawing.Point(12, 2);
            this.lblAbsEncoder.Name = "lblAbsEncoder";
            this.lblAbsEncoder.Size = new System.Drawing.Size(115, 15);
            this.lblAbsEncoder.TabIndex = 36;
            this.lblAbsEncoder.Text = "Azimuth Encoder";
            // 
            // lblEncoderDegrees
            // 
            this.lblEncoderDegrees.AutoSize = true;
            this.lblEncoderDegrees.Location = new System.Drawing.Point(13, 29);
            this.lblEncoderDegrees.Name = "lblEncoderDegrees";
            this.lblEncoderDegrees.Size = new System.Drawing.Size(47, 13);
            this.lblEncoderDegrees.TabIndex = 37;
            this.lblEncoderDegrees.Text = "Degrees";
            // 
            // lblAzEncoderDegrees
            // 
            this.lblAzEncoderDegrees.AutoSize = true;
            this.lblAzEncoderDegrees.Location = new System.Drawing.Point(61, 29);
            this.lblAzEncoderDegrees.Name = "lblAzEncoderDegrees";
            this.lblAzEncoderDegrees.Size = new System.Drawing.Size(13, 13);
            this.lblAzEncoderDegrees.TabIndex = 38;
            this.lblAzEncoderDegrees.Text = "0";
            // 
            // lblEncoderTicks
            // 
            this.lblEncoderTicks.AutoSize = true;
            this.lblEncoderTicks.Location = new System.Drawing.Point(12, 52);
            this.lblEncoderTicks.Name = "lblEncoderTicks";
            this.lblEncoderTicks.Size = new System.Drawing.Size(33, 13);
            this.lblEncoderTicks.TabIndex = 39;
            this.lblEncoderTicks.Text = "Ticks";
            // 
            // lblAzEncoderTicks
            // 
            this.lblAzEncoderTicks.AutoSize = true;
            this.lblAzEncoderTicks.Location = new System.Drawing.Point(61, 52);
            this.lblAzEncoderTicks.Name = "lblAzEncoderTicks";
            this.lblAzEncoderTicks.Size = new System.Drawing.Size(13, 13);
            this.lblAzEncoderTicks.TabIndex = 40;
            this.lblAzEncoderTicks.Text = "0";
            // 
            // btnAddOneEncoder
            // 
            this.btnAddOneEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddOneEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddOneEncoder.Location = new System.Drawing.Point(113, 23);
            this.btnAddOneEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddOneEncoder.Name = "btnAddOneEncoder";
            this.btnAddOneEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnAddOneEncoder.TabIndex = 41;
            this.btnAddOneEncoder.Text = "+1";
            this.btnAddOneEncoder.UseVisualStyleBackColor = false;
            this.btnAddOneEncoder.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnAddFiveEncoder
            // 
            this.btnAddFiveEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddFiveEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddFiveEncoder.Location = new System.Drawing.Point(162, 23);
            this.btnAddFiveEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddFiveEncoder.Name = "btnAddFiveEncoder";
            this.btnAddFiveEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnAddFiveEncoder.TabIndex = 42;
            this.btnAddFiveEncoder.Text = "+5";
            this.btnAddFiveEncoder.UseVisualStyleBackColor = false;
            this.btnAddFiveEncoder.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnAddXEncoder
            // 
            this.btnAddXEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddXEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddXEncoder.Location = new System.Drawing.Point(209, 23);
            this.btnAddXEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddXEncoder.Name = "btnAddXEncoder";
            this.btnAddXEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnAddXEncoder.TabIndex = 43;
            this.btnAddXEncoder.Text = "+X";
            this.btnAddXEncoder.UseVisualStyleBackColor = false;
            this.btnAddXEncoder.Click += new System.EventHandler(this.btnAddXEncoder_Click);
            // 
            // btnSubtractOneEncoder
            // 
            this.btnSubtractOneEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnSubtractOneEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractOneEncoder.Location = new System.Drawing.Point(114, 53);
            this.btnSubtractOneEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubtractOneEncoder.Name = "btnSubtractOneEncoder";
            this.btnSubtractOneEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnSubtractOneEncoder.TabIndex = 44;
            this.btnSubtractOneEncoder.Text = "-1";
            this.btnSubtractOneEncoder.UseVisualStyleBackColor = false;
            this.btnSubtractOneEncoder.Click += new System.EventHandler(this.btnSubtractOneEncoder_Click);
            // 
            // btnSubtractFiveEncoder
            // 
            this.btnSubtractFiveEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnSubtractFiveEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractFiveEncoder.Location = new System.Drawing.Point(162, 53);
            this.btnSubtractFiveEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubtractFiveEncoder.Name = "btnSubtractFiveEncoder";
            this.btnSubtractFiveEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnSubtractFiveEncoder.TabIndex = 45;
            this.btnSubtractFiveEncoder.Text = "-5";
            this.btnSubtractFiveEncoder.UseVisualStyleBackColor = false;
            this.btnSubtractFiveEncoder.Click += new System.EventHandler(this.btnSubtractFiveEncoder_Click);
            // 
            // btnSubtractXEncoder
            // 
            this.btnSubtractXEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnSubtractXEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractXEncoder.Location = new System.Drawing.Point(209, 53);
            this.btnSubtractXEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubtractXEncoder.Name = "btnSubtractXEncoder";
            this.btnSubtractXEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnSubtractXEncoder.TabIndex = 46;
            this.btnSubtractXEncoder.Text = "-X";
            this.btnSubtractXEncoder.UseVisualStyleBackColor = false;
            this.btnSubtractXEncoder.Click += new System.EventHandler(this.btnSubtractXEncoder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(265, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "Custom Value";
            // 
            // txtCustEncoderVal
            // 
            this.txtCustEncoderVal.Location = new System.Drawing.Point(268, 48);
            this.txtCustEncoderVal.Name = "txtCustEncoderVal";
            this.txtCustEncoderVal.Size = new System.Drawing.Size(51, 20);
            this.txtCustEncoderVal.TabIndex = 48;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(269, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 56;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(266, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 55;
            this.label10.Text = "Custom Value";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkGray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(209, 57);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 24);
            this.button1.TabIndex = 54;
            this.button1.Text = "-X";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkGray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(162, 57);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 24);
            this.button2.TabIndex = 53;
            this.button2.Text = "-5";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DarkGray;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(112, 57);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 24);
            this.button3.TabIndex = 52;
            this.button3.Text = "-1";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.DarkGray;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(209, 25);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(33, 24);
            this.button4.TabIndex = 51;
            this.button4.Text = "+X";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.DarkGray;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(162, 25);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 24);
            this.button5.TabIndex = 50;
            this.button5.Text = "+5";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.DarkGray;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(113, 25);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(33, 24);
            this.button6.TabIndex = 49;
            this.button6.Text = "+1";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // lblElEncoderTicks
            // 
            this.lblElEncoderTicks.AutoSize = true;
            this.lblElEncoderTicks.Location = new System.Drawing.Point(61, 59);
            this.lblElEncoderTicks.Name = "lblElEncoderTicks";
            this.lblElEncoderTicks.Size = new System.Drawing.Size(13, 13);
            this.lblElEncoderTicks.TabIndex = 61;
            this.lblElEncoderTicks.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 60;
            this.label14.Text = "Ticks";
            // 
            // lblElEncoderDegrees
            // 
            this.lblElEncoderDegrees.AutoSize = true;
            this.lblElEncoderDegrees.Location = new System.Drawing.Point(60, 36);
            this.lblElEncoderDegrees.Name = "lblElEncoderDegrees";
            this.lblElEncoderDegrees.Size = new System.Drawing.Size(13, 13);
            this.lblElEncoderDegrees.TabIndex = 59;
            this.lblElEncoderDegrees.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 36);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 13);
            this.label16.TabIndex = 58;
            this.label16.Text = "Degrees";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(4, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(123, 15);
            this.label17.TabIndex = 57;
            this.label17.Text = "Elevation Encoder";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Set Bits of Precision";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(137, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(39, 20);
            this.textBox2.TabIndex = 63;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 13);
            this.label13.TabIndex = 64;
            this.label13.Text = "Set Error";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1158, 74);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 13);
            this.label15.TabIndex = 65;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(137, 72);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(40, 20);
            this.textBox3.TabIndex = 66;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(138, 102);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(39, 20);
            this.textBox4.TabIndex = 67;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 72);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(63, 13);
            this.label18.TabIndex = 68;
            this.label18.Text = "Set Position";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(2, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(653, 531);
            this.tabControl1.TabIndex = 74;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gray;
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Controls.Add(this.groupBox9);
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(645, 505);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Appointment Control";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Gainsboro;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button7.Location = new System.Drawing.Point(627, 0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(22, 22);
            this.button7.TabIndex = 78;
            this.button7.Text = "?";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox9.Controls.Add(this.btnTest);
            this.groupBox9.Controls.Add(this.selectDemo);
            this.groupBox9.Location = new System.Drawing.Point(416, 388);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(210, 66);
            this.groupBox9.TabIndex = 77;
            this.groupBox9.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox8.Controls.Add(this.runDiagScriptsButton);
            this.groupBox8.Controls.Add(this.diagnosticScriptCombo);
            this.groupBox8.Location = new System.Drawing.Point(339, 127);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(287, 71);
            this.groupBox8.TabIndex = 76;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Diagnostic Scripts";
            // 
            // runDiagScriptsButton
            // 
            this.runDiagScriptsButton.BackColor = System.Drawing.Color.DarkGray;
            this.runDiagScriptsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.runDiagScriptsButton.Location = new System.Drawing.Point(197, 19);
            this.runDiagScriptsButton.Name = "runDiagScriptsButton";
            this.runDiagScriptsButton.Size = new System.Drawing.Size(75, 24);
            this.runDiagScriptsButton.TabIndex = 24;
            this.runDiagScriptsButton.Text = "Run Script";
            this.runDiagScriptsButton.UseVisualStyleBackColor = false;
            this.runDiagScriptsButton.Click += new System.EventHandler(this.editDiagScriptsButton_Click);
            // 
            // diagnosticScriptCombo
            // 
            this.diagnosticScriptCombo.BackColor = System.Drawing.Color.DarkGray;
            this.diagnosticScriptCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.diagnosticScriptCombo.FormattingEnabled = true;
            this.diagnosticScriptCombo.Items.AddRange(new object[] {
            "Select a Script",
            "Hit Elevation Lower Limit Switch",
            "Hit Elevation Upper Limit Switch"});
            this.diagnosticScriptCombo.Location = new System.Drawing.Point(6, 22);
            this.diagnosticScriptCombo.Name = "diagnosticScriptCombo";
            this.diagnosticScriptCombo.Size = new System.Drawing.Size(180, 21);
            this.diagnosticScriptCombo.TabIndex = 23;
            this.diagnosticScriptCombo.SelectedIndexChanged += new System.EventHandler(this.diagnosticScriptCombo_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Location = new System.Drawing.Point(416, 246);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 133);
            this.groupBox3.TabIndex = 75;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.Controls.Add(this.splitContainer2);
            this.groupBox2.Location = new System.Drawing.Point(6, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 208);
            this.groupBox2.TabIndex = 74;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encoder Simulation";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Location = new System.Drawing.Point(-6, 19);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnAddFiveEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.lblAbsEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.txtCustEncoderVal);
            this.splitContainer2.Panel1.Controls.Add(this.lblEncoderDegrees);
            this.splitContainer2.Panel1.Controls.Add(this.label9);
            this.splitContainer2.Panel1.Controls.Add(this.lblAzEncoderDegrees);
            this.splitContainer2.Panel1.Controls.Add(this.btnSubtractXEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.lblEncoderTicks);
            this.splitContainer2.Panel1.Controls.Add(this.btnSubtractFiveEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.btnSubtractOneEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.lblAzEncoderTicks);
            this.splitContainer2.Panel1.Controls.Add(this.btnAddXEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.btnAddOneEncoder);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label17);
            this.splitContainer2.Panel2.Controls.Add(this.label14);
            this.splitContainer2.Panel2.Controls.Add(this.lblElEncoderTicks);
            this.splitContainer2.Panel2.Controls.Add(this.label16);
            this.splitContainer2.Panel2.Controls.Add(this.lblElEncoderDegrees);
            this.splitContainer2.Panel2.Controls.Add(this.button5);
            this.splitContainer2.Panel2.Controls.Add(this.label10);
            this.splitContainer2.Panel2.Controls.Add(this.textBox1);
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.Controls.Add(this.button3);
            this.splitContainer2.Panel2.Controls.Add(this.button6);
            this.splitContainer2.Panel2.Controls.Add(this.button4);
            this.splitContainer2.Size = new System.Drawing.Size(410, 191);
            this.splitContainer2.SplitterDistance = 91;
            this.splitContainer2.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.startTimeTextBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.statusTextBox);
            this.groupBox1.Controls.Add(this.endTimeTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(339, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 115);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Appointment";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gray;
            this.tabPage2.Controls.Add(this.grpAmbTempHumid);
            this.tabPage2.Controls.Add(this.grpAccelerometerSensorData);
            this.tabPage2.Controls.Add(this.grpSensorData);
            this.tabPage2.Controls.Add(this.grpMcuStatus);
            this.tabPage2.Controls.Add(this.groupBox14);
            this.tabPage2.Controls.Add(this.grpAbsoluteMotorPositionsTemperatures);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(645, 505);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sensor Data";
            // 
            // grpAmbTempHumid
            // 
            this.grpAmbTempHumid.BackColor = System.Drawing.Color.Gainsboro;
            this.grpAmbTempHumid.Controls.Add(this.lblAmbientDewPoint);
            this.grpAmbTempHumid.Controls.Add(this.lblAmbientDewPointUnit);
            this.grpAmbTempHumid.Controls.Add(this.fldAmbientDewPoint);
            this.grpAmbTempHumid.Controls.Add(this.lblAmbientTempUnit);
            this.grpAmbTempHumid.Controls.Add(this.lblAmbientHumidityUnit);
            this.grpAmbTempHumid.Controls.Add(this.lblAmbientTemp);
            this.grpAmbTempHumid.Controls.Add(this.fldAmbientHumidity);
            this.grpAmbTempHumid.Controls.Add(this.fldAmbientTemp);
            this.grpAmbTempHumid.Controls.Add(this.lblAmbientHumidity);
            this.grpAmbTempHumid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAmbTempHumid.Location = new System.Drawing.Point(314, 385);
            this.grpAmbTempHumid.Margin = new System.Windows.Forms.Padding(2);
            this.grpAmbTempHumid.Name = "grpAmbTempHumid";
            this.grpAmbTempHumid.Padding = new System.Windows.Forms.Padding(2);
            this.grpAmbTempHumid.Size = new System.Drawing.Size(324, 74);
            this.grpAmbTempHumid.TabIndex = 44;
            this.grpAmbTempHumid.TabStop = false;
            this.grpAmbTempHumid.Text = "Elevation Ambient Temperature and Humidity";
            // 
            // lblAmbientDewPoint
            // 
            this.lblAmbientDewPoint.AutoSize = true;
            this.lblAmbientDewPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientDewPoint.Location = new System.Drawing.Point(6, 52);
            this.lblAmbientDewPoint.Name = "lblAmbientDewPoint";
            this.lblAmbientDewPoint.Size = new System.Drawing.Size(141, 16);
            this.lblAmbientDewPoint.TabIndex = 42;
            this.lblAmbientDewPoint.Text = "Ambient Dew Point:";
            // 
            // lblAmbientDewPointUnit
            // 
            this.lblAmbientDewPointUnit.AutoSize = true;
            this.lblAmbientDewPointUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientDewPointUnit.Location = new System.Drawing.Point(252, 54);
            this.lblAmbientDewPointUnit.Name = "lblAmbientDewPointUnit";
            this.lblAmbientDewPointUnit.Size = new System.Drawing.Size(57, 13);
            this.lblAmbientDewPointUnit.TabIndex = 41;
            this.lblAmbientDewPointUnit.Text = "Fahrenheit";
            // 
            // fldAmbientDewPoint
            // 
            this.fldAmbientDewPoint.AutoSize = true;
            this.fldAmbientDewPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAmbientDewPoint.Location = new System.Drawing.Point(197, 50);
            this.fldAmbientDewPoint.Name = "fldAmbientDewPoint";
            this.fldAmbientDewPoint.Size = new System.Drawing.Size(18, 18);
            this.fldAmbientDewPoint.TabIndex = 40;
            this.fldAmbientDewPoint.Text = "--";
            // 
            // lblAmbientTempUnit
            // 
            this.lblAmbientTempUnit.AutoSize = true;
            this.lblAmbientTempUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientTempUnit.Location = new System.Drawing.Point(252, 17);
            this.lblAmbientTempUnit.Name = "lblAmbientTempUnit";
            this.lblAmbientTempUnit.Size = new System.Drawing.Size(57, 13);
            this.lblAmbientTempUnit.TabIndex = 38;
            this.lblAmbientTempUnit.Text = "Fahrenheit";
            // 
            // lblAmbientHumidityUnit
            // 
            this.lblAmbientHumidityUnit.AutoSize = true;
            this.lblAmbientHumidityUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientHumidityUnit.Location = new System.Drawing.Point(252, 36);
            this.lblAmbientHumidityUnit.Name = "lblAmbientHumidityUnit";
            this.lblAmbientHumidityUnit.Size = new System.Drawing.Size(15, 13);
            this.lblAmbientHumidityUnit.TabIndex = 37;
            this.lblAmbientHumidityUnit.Text = "%";
            // 
            // lblAmbientTemp
            // 
            this.lblAmbientTemp.AutoSize = true;
            this.lblAmbientTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientTemp.Location = new System.Drawing.Point(6, 15);
            this.lblAmbientTemp.Name = "lblAmbientTemp";
            this.lblAmbientTemp.Size = new System.Drawing.Size(112, 16);
            this.lblAmbientTemp.TabIndex = 33;
            this.lblAmbientTemp.Text = "Ambient Temp:";
            // 
            // fldAmbientHumidity
            // 
            this.fldAmbientHumidity.AutoSize = true;
            this.fldAmbientHumidity.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAmbientHumidity.Location = new System.Drawing.Point(197, 31);
            this.fldAmbientHumidity.Name = "fldAmbientHumidity";
            this.fldAmbientHumidity.Size = new System.Drawing.Size(18, 18);
            this.fldAmbientHumidity.TabIndex = 36;
            this.fldAmbientHumidity.Text = "--";
            // 
            // fldAmbientTemp
            // 
            this.fldAmbientTemp.AutoSize = true;
            this.fldAmbientTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAmbientTemp.Location = new System.Drawing.Point(197, 13);
            this.fldAmbientTemp.Name = "fldAmbientTemp";
            this.fldAmbientTemp.Size = new System.Drawing.Size(18, 18);
            this.fldAmbientTemp.TabIndex = 35;
            this.fldAmbientTemp.Text = "--";
            // 
            // lblAmbientHumidity
            // 
            this.lblAmbientHumidity.AutoSize = true;
            this.lblAmbientHumidity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientHumidity.Location = new System.Drawing.Point(6, 33);
            this.lblAmbientHumidity.Name = "lblAmbientHumidity";
            this.lblAmbientHumidity.Size = new System.Drawing.Size(132, 16);
            this.lblAmbientHumidity.TabIndex = 34;
            this.lblAmbientHumidity.Text = "Ambient Humidity:";
            // 
            // grpAccelerometerSensorData
            // 
            this.grpAccelerometerSensorData.BackColor = System.Drawing.Color.Gainsboro;
            this.grpAccelerometerSensorData.Controls.Add(this.label26);
            this.grpAccelerometerSensorData.Controls.Add(this.label25);
            this.grpAccelerometerSensorData.Controls.Add(this.label45);
            this.grpAccelerometerSensorData.Controls.Add(this.label38);
            this.grpAccelerometerSensorData.Controls.Add(this.label46);
            this.grpAccelerometerSensorData.Controls.Add(this.label48);
            this.grpAccelerometerSensorData.Controls.Add(this.pnlCounterbalanceAccelerometer);
            this.grpAccelerometerSensorData.Controls.Add(this.label50);
            this.grpAccelerometerSensorData.Controls.Add(this.label37);
            this.grpAccelerometerSensorData.Controls.Add(this.label49);
            this.grpAccelerometerSensorData.Controls.Add(this.pnlElevationMotorAccelerometer);
            this.grpAccelerometerSensorData.Controls.Add(this.label47);
            this.grpAccelerometerSensorData.Controls.Add(this.label36);
            this.grpAccelerometerSensorData.Controls.Add(this.pnlAzimuthMotorAccelerometer);
            this.grpAccelerometerSensorData.Location = new System.Drawing.Point(9, 127);
            this.grpAccelerometerSensorData.Name = "grpAccelerometerSensorData";
            this.grpAccelerometerSensorData.Size = new System.Drawing.Size(295, 374);
            this.grpAccelerometerSensorData.TabIndex = 43;
            this.grpAccelerometerSensorData.TabStop = false;
            this.grpAccelerometerSensorData.Text = "Accelerometer Sensor Data";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(210, 21);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(66, 13);
            this.label26.TabIndex = 102;
            this.label26.Text = "Acceleration";
            // 
            // label25
            // 
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(177, 16);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(37, 13);
            this.label25.TabIndex = 101;
            this.label25.Text = "__________";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(96, 21);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(14, 13);
            this.label45.TabIndex = 52;
            this.label45.Text = "Y";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(2, 263);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(153, 13);
            this.label38.TabIndex = 10;
            this.label38.Text = "Counterbalance Accelerometer";
            // 
            // label46
            // 
            this.label46.ForeColor = System.Drawing.Color.Blue;
            this.label46.Location = new System.Drawing.Point(2, 16);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(37, 13);
            this.label46.TabIndex = 100;
            this.label46.Text = "__________";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(154, 21);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(14, 13);
            this.label48.TabIndex = 51;
            this.label48.Text = "Z";
            // 
            // pnlCounterbalanceAccelerometer
            // 
            this.pnlCounterbalanceAccelerometer.Controls.Add(this.lblCbDisabled);
            this.pnlCounterbalanceAccelerometer.Controls.Add(this.counterBalanceAccChart);
            this.pnlCounterbalanceAccelerometer.Location = new System.Drawing.Point(0, 279);
            this.pnlCounterbalanceAccelerometer.Name = "pnlCounterbalanceAccelerometer";
            this.pnlCounterbalanceAccelerometer.Size = new System.Drawing.Size(295, 96);
            this.pnlCounterbalanceAccelerometer.TabIndex = 4;
            // 
            // lblCbDisabled
            // 
            this.lblCbDisabled.AutoSize = true;
            this.lblCbDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCbDisabled.Location = new System.Drawing.Point(17, 41);
            this.lblCbDisabled.Name = "lblCbDisabled";
            this.lblCbDisabled.Size = new System.Drawing.Size(75, 18);
            this.lblCbDisabled.TabIndex = 33;
            this.lblCbDisabled.Text = "CB Status";
            this.lblCbDisabled.Visible = false;
            // 
            // counterBalanceAccChart
            // 
            this.counterBalanceAccChart.BackColor = System.Drawing.Color.Gainsboro;
            chartArea5.AxisX.IsLabelAutoFit = false;
            chartArea5.AxisX.LabelStyle.Enabled = false;
            chartArea5.AxisX.MajorGrid.Enabled = false;
            chartArea5.AxisY.MajorGrid.Enabled = false;
            chartArea5.BackColor = System.Drawing.Color.Gainsboro;
            chartArea5.Name = "ChartArea1";
            this.counterBalanceAccChart.ChartAreas.Add(chartArea5);
            this.counterBalanceAccChart.Location = new System.Drawing.Point(-20, 0);
            this.counterBalanceAccChart.Name = "counterBalanceAccChart";
            series14.ChartArea = "ChartArea1";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Color = System.Drawing.Color.Blue;
            series14.Name = "x";
            series14.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series14.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series15.ChartArea = "ChartArea1";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series15.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series15.Name = "y";
            series16.ChartArea = "ChartArea1";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series16.Color = System.Drawing.Color.Red;
            series16.Name = "z";
            series17.ChartArea = "ChartArea1";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series17.Color = System.Drawing.Color.Black;
            series17.Name = "accel";
            this.counterBalanceAccChart.Series.Add(series14);
            this.counterBalanceAccChart.Series.Add(series15);
            this.counterBalanceAccChart.Series.Add(series16);
            this.counterBalanceAccChart.Series.Add(series17);
            this.counterBalanceAccChart.Size = new System.Drawing.Size(315, 98);
            this.counterBalanceAccChart.TabIndex = 0;
            this.counterBalanceAccChart.Text = "chart1";
            // 
            // label50
            // 
            this.label50.ForeColor = System.Drawing.Color.Red;
            this.label50.Location = new System.Drawing.Point(118, 16);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(37, 13);
            this.label50.TabIndex = 48;
            this.label50.Text = "__________";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(3, 140);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(152, 13);
            this.label37.TabIndex = 10;
            this.label37.Text = "Elevation Motor Accelerometer";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(36, 21);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(14, 13);
            this.label49.TabIndex = 50;
            this.label49.Text = "X";
            // 
            // pnlElevationMotorAccelerometer
            // 
            this.pnlElevationMotorAccelerometer.Controls.Add(this.lblElDisabled);
            this.pnlElevationMotorAccelerometer.Controls.Add(this.elevationAccChart);
            this.pnlElevationMotorAccelerometer.Location = new System.Drawing.Point(-3, 156);
            this.pnlElevationMotorAccelerometer.Name = "pnlElevationMotorAccelerometer";
            this.pnlElevationMotorAccelerometer.Size = new System.Drawing.Size(298, 98);
            this.pnlElevationMotorAccelerometer.TabIndex = 2;
            // 
            // lblElDisabled
            // 
            this.lblElDisabled.AutoSize = true;
            this.lblElDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElDisabled.Location = new System.Drawing.Point(20, 43);
            this.lblElDisabled.Name = "lblElDisabled";
            this.lblElDisabled.Size = new System.Drawing.Size(72, 18);
            this.lblElDisabled.TabIndex = 32;
            this.lblElDisabled.Text = "EL Status";
            this.lblElDisabled.Visible = false;
            // 
            // elevationAccChart
            // 
            this.elevationAccChart.BackColor = System.Drawing.Color.Gainsboro;
            this.elevationAccChart.BorderlineColor = System.Drawing.Color.Gainsboro;
            this.elevationAccChart.BorderSkin.PageColor = System.Drawing.Color.Gainsboro;
            chartArea6.AxisX.IsLabelAutoFit = false;
            chartArea6.AxisX.LabelStyle.Enabled = false;
            chartArea6.AxisX.MajorGrid.Enabled = false;
            chartArea6.AxisY.IsLabelAutoFit = false;
            chartArea6.AxisY.MajorGrid.Enabled = false;
            chartArea6.BackColor = System.Drawing.Color.Gainsboro;
            chartArea6.Name = "ChartArea1";
            this.elevationAccChart.ChartAreas.Add(chartArea6);
            this.elevationAccChart.Location = new System.Drawing.Point(-10, 0);
            this.elevationAccChart.Name = "elevationAccChart";
            series18.ChartArea = "ChartArea1";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series18.Color = System.Drawing.Color.Blue;
            series18.Name = "x";
            series18.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series18.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series19.ChartArea = "ChartArea1";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series19.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series19.Name = "y";
            series20.ChartArea = "ChartArea1";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series20.Color = System.Drawing.Color.Red;
            series20.Name = "z";
            series21.ChartArea = "ChartArea1";
            series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series21.Color = System.Drawing.Color.Black;
            series21.Name = "accel";
            this.elevationAccChart.Series.Add(series18);
            this.elevationAccChart.Series.Add(series19);
            this.elevationAccChart.Series.Add(series20);
            this.elevationAccChart.Series.Add(series21);
            this.elevationAccChart.Size = new System.Drawing.Size(308, 101);
            this.elevationAccChart.TabIndex = 0;
            this.elevationAccChart.Text = "elevationAccChart";
            // 
            // label47
            // 
            this.label47.ForeColor = System.Drawing.Color.Green;
            this.label47.Location = new System.Drawing.Point(63, 16);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(37, 13);
            this.label47.TabIndex = 49;
            this.label47.Text = "__________";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(3, 34);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(145, 13);
            this.label36.TabIndex = 10;
            this.label36.Text = "Azimuth Motor Accelerometer";
            // 
            // pnlAzimuthMotorAccelerometer
            // 
            this.pnlAzimuthMotorAccelerometer.Controls.Add(this.lblAzDisabled);
            this.pnlAzimuthMotorAccelerometer.Controls.Add(this.azimuthAccChart);
            this.pnlAzimuthMotorAccelerometer.Location = new System.Drawing.Point(-4, 50);
            this.pnlAzimuthMotorAccelerometer.Name = "pnlAzimuthMotorAccelerometer";
            this.pnlAzimuthMotorAccelerometer.Size = new System.Drawing.Size(299, 86);
            this.pnlAzimuthMotorAccelerometer.TabIndex = 0;
            // 
            // lblAzDisabled
            // 
            this.lblAzDisabled.AutoSize = true;
            this.lblAzDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzDisabled.Location = new System.Drawing.Point(21, 35);
            this.lblAzDisabled.Name = "lblAzDisabled";
            this.lblAzDisabled.Size = new System.Drawing.Size(72, 18);
            this.lblAzDisabled.TabIndex = 31;
            this.lblAzDisabled.Text = "AZ Status";
            this.lblAzDisabled.Visible = false;
            // 
            // azimuthAccChart
            // 
            this.azimuthAccChart.BackColor = System.Drawing.Color.Gainsboro;
            this.azimuthAccChart.BorderlineColor = System.Drawing.Color.Gainsboro;
            chartArea7.AxisX.IsLabelAutoFit = false;
            chartArea7.AxisX.LabelStyle.Enabled = false;
            chartArea7.AxisX.MajorGrid.Enabled = false;
            chartArea7.AxisY.IsLabelAutoFit = false;
            chartArea7.AxisY.MajorGrid.Enabled = false;
            chartArea7.BackColor = System.Drawing.Color.Gainsboro;
            chartArea7.Name = "ChartArea1";
            this.azimuthAccChart.ChartAreas.Add(chartArea7);
            this.azimuthAccChart.Location = new System.Drawing.Point(-9, 0);
            this.azimuthAccChart.Name = "azimuthAccChart";
            series22.ChartArea = "ChartArea1";
            series22.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series22.Color = System.Drawing.Color.Blue;
            series22.Name = "x";
            series22.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series22.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series23.ChartArea = "ChartArea1";
            series23.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series23.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series23.Name = "y";
            series24.ChartArea = "ChartArea1";
            series24.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series24.Color = System.Drawing.Color.Red;
            series24.Name = "z";
            series25.ChartArea = "ChartArea1";
            series25.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series25.Color = System.Drawing.Color.Black;
            series25.Name = "accel";
            this.azimuthAccChart.Series.Add(series22);
            this.azimuthAccChart.Series.Add(series23);
            this.azimuthAccChart.Series.Add(series24);
            this.azimuthAccChart.Series.Add(series25);
            this.azimuthAccChart.Size = new System.Drawing.Size(308, 86);
            this.azimuthAccChart.TabIndex = 0;
            this.azimuthAccChart.Text = "chart1";
            // 
            // grpSensorData
            // 
            this.grpSensorData.BackColor = System.Drawing.Color.Gainsboro;
            this.grpSensorData.Controls.Add(this.lbGateStat);
            this.grpSensorData.Controls.Add(this.lbEstopStat);
            this.grpSensorData.Controls.Add(this.label31);
            this.grpSensorData.Controls.Add(this.label30);
            this.grpSensorData.Controls.Add(this.lblAzHome1);
            this.grpSensorData.Controls.Add(this.lblElLimStatus2);
            this.grpSensorData.Controls.Add(this.lblElLimStatus1);
            this.grpSensorData.Controls.Add(this.lblElHome);
            this.grpSensorData.Controls.Add(this.lblELHomeStatus);
            this.grpSensorData.Controls.Add(this.lblAzHomeStatus1);
            this.grpSensorData.Controls.Add(this.lblAzLimit1);
            this.grpSensorData.Controls.Add(this.lblAzLimit2);
            this.grpSensorData.Location = new System.Drawing.Point(9, 4);
            this.grpSensorData.Name = "grpSensorData";
            this.grpSensorData.Size = new System.Drawing.Size(295, 117);
            this.grpSensorData.TabIndex = 42;
            this.grpSensorData.TabStop = false;
            this.grpSensorData.Text = "Sensor Data";
            // 
            // lbGateStat
            // 
            this.lbGateStat.AutoSize = true;
            this.lbGateStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGateStat.Location = new System.Drawing.Point(220, 93);
            this.lbGateStat.Name = "lbGateStat";
            this.lbGateStat.Size = new System.Drawing.Size(56, 15);
            this.lbGateStat.TabIndex = 21;
            this.lbGateStat.Text = "Inactive";
            // 
            // lbEstopStat
            // 
            this.lbEstopStat.AutoSize = true;
            this.lbEstopStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEstopStat.Location = new System.Drawing.Point(220, 78);
            this.lbEstopStat.Name = "lbEstopStat";
            this.lbEstopStat.Size = new System.Drawing.Size(56, 15);
            this.lbEstopStat.TabIndex = 20;
            this.lbEstopStat.Text = "Inactive";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(6, 93);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(44, 15);
            this.label31.TabIndex = 19;
            this.label31.Text = "Gates";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(6, 78);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(43, 15);
            this.label30.TabIndex = 18;
            this.label30.Text = "Estop";
            // 
            // lblAzHome1
            // 
            this.lblAzHome1.AutoSize = true;
            this.lblAzHome1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzHome1.Location = new System.Drawing.Point(6, 16);
            this.lblAzHome1.Name = "lblAzHome1";
            this.lblAzHome1.Size = new System.Drawing.Size(149, 15);
            this.lblAzHome1.TabIndex = 0;
            this.lblAzHome1.Text = "Azimuth Home Sensor";
            // 
            // lblElLimStatus2
            // 
            this.lblElLimStatus2.AutoSize = true;
            this.lblElLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus2.Location = new System.Drawing.Point(220, 63);
            this.lblElLimStatus2.Name = "lblElLimStatus2";
            this.lblElLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus2.TabIndex = 15;
            this.lblElLimStatus2.Text = "Inactive";
            // 
            // lblElLimStatus1
            // 
            this.lblElLimStatus1.AutoSize = true;
            this.lblElLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus1.Location = new System.Drawing.Point(220, 48);
            this.lblElLimStatus1.Name = "lblElLimStatus1";
            this.lblElLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus1.TabIndex = 14;
            this.lblElLimStatus1.Text = "Inactive";
            // 
            // lblElHome
            // 
            this.lblElHome.AutoSize = true;
            this.lblElHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElHome.Location = new System.Drawing.Point(6, 31);
            this.lblElHome.Name = "lblElHome";
            this.lblElHome.Size = new System.Drawing.Size(157, 15);
            this.lblElHome.TabIndex = 2;
            this.lblElHome.Text = "Elevation Home Sensor";
            // 
            // lblELHomeStatus
            // 
            this.lblELHomeStatus.AutoSize = true;
            this.lblELHomeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblELHomeStatus.Location = new System.Drawing.Point(220, 31);
            this.lblELHomeStatus.Name = "lblELHomeStatus";
            this.lblELHomeStatus.Size = new System.Drawing.Size(56, 15);
            this.lblELHomeStatus.TabIndex = 5;
            this.lblELHomeStatus.Text = "Inactive";
            // 
            // lblAzHomeStatus1
            // 
            this.lblAzHomeStatus1.AutoSize = true;
            this.lblAzHomeStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzHomeStatus1.Location = new System.Drawing.Point(220, 16);
            this.lblAzHomeStatus1.Name = "lblAzHomeStatus1";
            this.lblAzHomeStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzHomeStatus1.TabIndex = 3;
            this.lblAzHomeStatus1.Text = "Inactive";
            // 
            // lblAzLimit1
            // 
            this.lblAzLimit1.AutoSize = true;
            this.lblAzLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit1.Location = new System.Drawing.Point(6, 48);
            this.lblAzLimit1.Name = "lblAzLimit1";
            this.lblAzLimit1.Size = new System.Drawing.Size(160, 15);
            this.lblAzLimit1.TabIndex = 10;
            this.lblAzLimit1.Text = "Elevation Limit Switch 1";
            // 
            // lblAzLimit2
            // 
            this.lblAzLimit2.AutoSize = true;
            this.lblAzLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit2.Location = new System.Drawing.Point(6, 63);
            this.lblAzLimit2.Name = "lblAzLimit2";
            this.lblAzLimit2.Size = new System.Drawing.Size(160, 15);
            this.lblAzLimit2.TabIndex = 11;
            this.lblAzLimit2.Text = "Elevation Limit Switch 2";
            // 
            // grpMcuStatus
            // 
            this.grpMcuStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.grpMcuStatus.Controls.Add(this.lblMCUStatus);
            this.grpMcuStatus.Controls.Add(this.lblMCUStatusText);
            this.grpMcuStatus.Controls.Add(this.lblMCUErrors);
            this.grpMcuStatus.Controls.Add(this.btnResetMcuErrors);
            this.grpMcuStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMcuStatus.Location = new System.Drawing.Point(314, 463);
            this.grpMcuStatus.Margin = new System.Windows.Forms.Padding(2);
            this.grpMcuStatus.Name = "grpMcuStatus";
            this.grpMcuStatus.Padding = new System.Windows.Forms.Padding(2);
            this.grpMcuStatus.Size = new System.Drawing.Size(324, 38);
            this.grpMcuStatus.TabIndex = 38;
            this.grpMcuStatus.TabStop = false;
            this.grpMcuStatus.Text = "Motor Controller Status";
            // 
            // lblMCUStatus
            // 
            this.lblMCUStatus.AutoSize = true;
            this.lblMCUStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMCUStatus.ForeColor = System.Drawing.Color.Black;
            this.lblMCUStatus.Location = new System.Drawing.Point(95, 17);
            this.lblMCUStatus.Name = "lblMCUStatus";
            this.lblMCUStatus.Size = new System.Drawing.Size(34, 16);
            this.lblMCUStatus.TabIndex = 76;
            this.lblMCUStatus.Text = "N/A";
            // 
            // lblMCUStatusText
            // 
            this.lblMCUStatusText.AutoSize = true;
            this.lblMCUStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMCUStatusText.Location = new System.Drawing.Point(6, 17);
            this.lblMCUStatusText.Name = "lblMCUStatusText";
            this.lblMCUStatusText.Size = new System.Drawing.Size(92, 16);
            this.lblMCUStatusText.TabIndex = 33;
            this.lblMCUStatusText.Text = "MCU Status:";
            // 
            // lblMCUErrors
            // 
            this.lblMCUErrors.AutoSize = true;
            this.lblMCUErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMCUErrors.Location = new System.Drawing.Point(17, 24);
            this.lblMCUErrors.Name = "lblMCUErrors";
            this.lblMCUErrors.Size = new System.Drawing.Size(0, 13);
            this.lblMCUErrors.TabIndex = 75;
            // 
            // btnResetMcuErrors
            // 
            this.btnResetMcuErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetMcuErrors.Location = new System.Drawing.Point(210, 14);
            this.btnResetMcuErrors.Name = "btnResetMcuErrors";
            this.btnResetMcuErrors.Size = new System.Drawing.Size(110, 23);
            this.btnResetMcuErrors.TabIndex = 0;
            this.btnResetMcuErrors.Text = "Reset MCU Errors";
            this.btnResetMcuErrors.UseVisualStyleBackColor = true;
            this.btnResetMcuErrors.Click += new System.EventHandler(this.btnResetMcuErrors_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox14.Controls.Add(this.farTempConvert);
            this.groupBox14.Controls.Add(this.celTempConvert);
            this.groupBox14.Location = new System.Drawing.Point(314, 4);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(324, 54);
            this.groupBox14.TabIndex = 39;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Temperature Conversion";
            // 
            // farTempConvert
            // 
            this.farTempConvert.BackColor = System.Drawing.Color.Silver;
            this.farTempConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.farTempConvert.Location = new System.Drawing.Point(183, 19);
            this.farTempConvert.Name = "farTempConvert";
            this.farTempConvert.Size = new System.Drawing.Size(130, 23);
            this.farTempConvert.TabIndex = 1;
            this.farTempConvert.Text = "Fahrenheit";
            this.farTempConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.farTempConvert.UseVisualStyleBackColor = false;
            this.farTempConvert.Click += new System.EventHandler(this.farTempConvert_Click);
            // 
            // celTempConvert
            // 
            this.celTempConvert.BackColor = System.Drawing.Color.Silver;
            this.celTempConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.celTempConvert.Location = new System.Drawing.Point(9, 19);
            this.celTempConvert.Name = "celTempConvert";
            this.celTempConvert.Size = new System.Drawing.Size(130, 23);
            this.celTempConvert.TabIndex = 0;
            this.celTempConvert.Text = "Celsius";
            this.celTempConvert.UseVisualStyleBackColor = false;
            this.celTempConvert.Click += new System.EventHandler(this.celTempConvert_Click);
            // 
            // grpAbsoluteMotorPositionsTemperatures
            // 
            this.grpAbsoluteMotorPositionsTemperatures.BackColor = System.Drawing.Color.Gainsboro;
            this.grpAbsoluteMotorPositionsTemperatures.Controls.Add(this.splitContainer1);
            this.grpAbsoluteMotorPositionsTemperatures.Location = new System.Drawing.Point(314, 252);
            this.grpAbsoluteMotorPositionsTemperatures.Margin = new System.Windows.Forms.Padding(2);
            this.grpAbsoluteMotorPositionsTemperatures.Name = "grpAbsoluteMotorPositionsTemperatures";
            this.grpAbsoluteMotorPositionsTemperatures.Padding = new System.Windows.Forms.Padding(2);
            this.grpAbsoluteMotorPositionsTemperatures.Size = new System.Drawing.Size(324, 129);
            this.grpAbsoluteMotorPositionsTemperatures.TabIndex = 38;
            this.grpAbsoluteMotorPositionsTemperatures.TabStop = false;
            this.grpAbsoluteMotorPositionsTemperatures.Text = "Absolute Motor Positions and Temperatures";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(-7, 18);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label32);
            this.splitContainer1.Panel1.Controls.Add(this.label34);
            this.splitContainer1.Panel1.Controls.Add(this.lblCurrentAzOrientation);
            this.splitContainer1.Panel1.Controls.Add(this.lblCurrentElOrientation);
            this.splitContainer1.Panel1.Controls.Add(this.lblElAbsPos);
            this.splitContainer1.Panel1.Controls.Add(this.lblAzAbsPos);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AZTempUnitLabel);
            this.splitContainer1.Panel2.Controls.Add(this.ElTempUnitLabel);
            this.splitContainer1.Panel2.Controls.Add(this.lblAzimuthTemp);
            this.splitContainer1.Panel2.Controls.Add(this.fldElTemp);
            this.splitContainer1.Panel2.Controls.Add(this.fldAzTemp);
            this.splitContainer1.Panel2.Controls.Add(this.lblElevationTemp);
            this.splitContainer1.Size = new System.Drawing.Size(333, 123);
            this.splitContainer1.SplitterDistance = 48;
            this.splitContainer1.TabIndex = 20;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(257, 27);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(47, 13);
            this.label32.TabIndex = 27;
            this.label32.Text = "Degrees";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(257, 10);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(47, 13);
            this.label34.TabIndex = 26;
            this.label34.Text = "Degrees";
            // 
            // lblCurrentAzOrientation
            // 
            this.lblCurrentAzOrientation.AutoSize = true;
            this.lblCurrentAzOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAzOrientation.Location = new System.Drawing.Point(10, 7);
            this.lblCurrentAzOrientation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentAzOrientation.Name = "lblCurrentAzOrientation";
            this.lblCurrentAzOrientation.Size = new System.Drawing.Size(129, 16);
            this.lblCurrentAzOrientation.TabIndex = 7;
            this.lblCurrentAzOrientation.Text = "Azimuth Position: ";
            // 
            // lblCurrentElOrientation
            // 
            this.lblCurrentElOrientation.AutoSize = true;
            this.lblCurrentElOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentElOrientation.Location = new System.Drawing.Point(9, 25);
            this.lblCurrentElOrientation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentElOrientation.Name = "lblCurrentElOrientation";
            this.lblCurrentElOrientation.Size = new System.Drawing.Size(141, 16);
            this.lblCurrentElOrientation.TabIndex = 8;
            this.lblCurrentElOrientation.Text = "Elevation Position: ";
            // 
            // lblElAbsPos
            // 
            this.lblElAbsPos.AutoSize = true;
            this.lblElAbsPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElAbsPos.Location = new System.Drawing.Point(198, 27);
            this.lblElAbsPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblElAbsPos.Name = "lblElAbsPos";
            this.lblElAbsPos.Size = new System.Drawing.Size(28, 18);
            this.lblElAbsPos.TabIndex = 10;
            this.lblElAbsPos.Text = "0.0";
            // 
            // lblAzAbsPos
            // 
            this.lblAzAbsPos.AutoSize = true;
            this.lblAzAbsPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzAbsPos.Location = new System.Drawing.Point(198, 6);
            this.lblAzAbsPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAzAbsPos.Name = "lblAzAbsPos";
            this.lblAzAbsPos.Size = new System.Drawing.Size(28, 18);
            this.lblAzAbsPos.TabIndex = 9;
            this.lblAzAbsPos.Text = "0.0";
            // 
            // AZTempUnitLabel
            // 
            this.AZTempUnitLabel.AutoSize = true;
            this.AZTempUnitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AZTempUnitLabel.Location = new System.Drawing.Point(257, 12);
            this.AZTempUnitLabel.Name = "AZTempUnitLabel";
            this.AZTempUnitLabel.Size = new System.Drawing.Size(57, 13);
            this.AZTempUnitLabel.TabIndex = 32;
            this.AZTempUnitLabel.Text = "Fahrenheit";
            // 
            // ElTempUnitLabel
            // 
            this.ElTempUnitLabel.AutoSize = true;
            this.ElTempUnitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElTempUnitLabel.Location = new System.Drawing.Point(257, 33);
            this.ElTempUnitLabel.Name = "ElTempUnitLabel";
            this.ElTempUnitLabel.Size = new System.Drawing.Size(57, 13);
            this.ElTempUnitLabel.TabIndex = 31;
            this.ElTempUnitLabel.Text = "Fahrenheit";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Controls.Add(this.InsideTempUnits);
            this.groupBox5.Controls.Add(this.rainRateUnits);
            this.groupBox5.Controls.Add(this.pressUnits);
            this.groupBox5.Controls.Add(this.dailyRainfallUnits);
            this.groupBox5.Controls.Add(this.outTempUnits);
            this.groupBox5.Controls.Add(this.label35);
            this.groupBox5.Controls.Add(this.windSpeedUnits);
            this.groupBox5.Controls.Add(this.insideTempLabel);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.rainRateLabel);
            this.groupBox5.Controls.Add(this.barometricPressureLabel);
            this.groupBox5.Controls.Add(this.dailyRainfallLabel);
            this.groupBox5.Controls.Add(this.outsideTempLabel);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.windDirLabel);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.windSpeedLabel);
            this.groupBox5.Location = new System.Drawing.Point(314, 63);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(324, 185);
            this.groupBox5.TabIndex = 37;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Weather Sensor Data";
            // 
            // InsideTempUnits
            // 
            this.InsideTempUnits.AutoSize = true;
            this.InsideTempUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InsideTempUnits.Location = new System.Drawing.Point(257, 117);
            this.InsideTempUnits.Name = "InsideTempUnits";
            this.InsideTempUnits.Size = new System.Drawing.Size(57, 13);
            this.InsideTempUnits.TabIndex = 30;
            this.InsideTempUnits.Text = "Fahrenheit";
            // 
            // rainRateUnits
            // 
            this.rainRateUnits.AutoSize = true;
            this.rainRateUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rainRateUnits.Location = new System.Drawing.Point(257, 91);
            this.rainRateUnits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rainRateUnits.Name = "rainRateUnits";
            this.rainRateUnits.Size = new System.Drawing.Size(39, 13);
            this.rainRateUnits.TabIndex = 27;
            this.rainRateUnits.Text = "Inches";
            // 
            // pressUnits
            // 
            this.pressUnits.AutoSize = true;
            this.pressUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressUnits.Location = new System.Drawing.Point(257, 164);
            this.pressUnits.Name = "pressUnits";
            this.pressUnits.Size = new System.Drawing.Size(58, 13);
            this.pressUnits.TabIndex = 29;
            this.pressUnits.Text = "Inches/Hg";
            // 
            // dailyRainfallUnits
            // 
            this.dailyRainfallUnits.AutoSize = true;
            this.dailyRainfallUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dailyRainfallUnits.Location = new System.Drawing.Point(257, 70);
            this.dailyRainfallUnits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dailyRainfallUnits.Name = "dailyRainfallUnits";
            this.dailyRainfallUnits.Size = new System.Drawing.Size(63, 13);
            this.dailyRainfallUnits.TabIndex = 26;
            this.dailyRainfallUnits.Text = "Inches/Day";
            // 
            // outTempUnits
            // 
            this.outTempUnits.AutoSize = true;
            this.outTempUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outTempUnits.Location = new System.Drawing.Point(257, 139);
            this.outTempUnits.Name = "outTempUnits";
            this.outTempUnits.Size = new System.Drawing.Size(57, 13);
            this.outTempUnits.TabIndex = 28;
            this.outTempUnits.Text = "Fahrenheit";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(256, 20);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(23, 20);
            this.label35.TabIndex = 25;
            this.label35.Text = " --";
            // 
            // windSpeedUnits
            // 
            this.windSpeedUnits.AutoSize = true;
            this.windSpeedUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windSpeedUnits.Location = new System.Drawing.Point(257, 48);
            this.windSpeedUnits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windSpeedUnits.Name = "windSpeedUnits";
            this.windSpeedUnits.Size = new System.Drawing.Size(31, 13);
            this.windSpeedUnits.TabIndex = 24;
            this.windSpeedUnits.Text = "MPH";
            // 
            // insideTempLabel
            // 
            this.insideTempLabel.AutoSize = true;
            this.insideTempLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insideTempLabel.Location = new System.Drawing.Point(202, 115);
            this.insideTempLabel.Name = "insideTempLabel";
            this.insideTempLabel.Size = new System.Drawing.Size(22, 18);
            this.insideTempLabel.TabIndex = 23;
            this.insideTempLabel.Text = " --";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(6, 115);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(136, 15);
            this.label23.TabIndex = 22;
            this.label23.Text = "Inside Temperature ";
            // 
            // rainRateLabel
            // 
            this.rainRateLabel.AutoSize = true;
            this.rainRateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rainRateLabel.Location = new System.Drawing.Point(203, 91);
            this.rainRateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rainRateLabel.Name = "rainRateLabel";
            this.rainRateLabel.Size = new System.Drawing.Size(22, 18);
            this.rainRateLabel.TabIndex = 19;
            this.rainRateLabel.Text = " --";
            // 
            // barometricPressureLabel
            // 
            this.barometricPressureLabel.AutoSize = true;
            this.barometricPressureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barometricPressureLabel.Location = new System.Drawing.Point(203, 159);
            this.barometricPressureLabel.Name = "barometricPressureLabel";
            this.barometricPressureLabel.Size = new System.Drawing.Size(22, 18);
            this.barometricPressureLabel.TabIndex = 21;
            this.barometricPressureLabel.Text = " --";
            // 
            // dailyRainfallLabel
            // 
            this.dailyRainfallLabel.AutoSize = true;
            this.dailyRainfallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dailyRainfallLabel.Location = new System.Drawing.Point(203, 68);
            this.dailyRainfallLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dailyRainfallLabel.Name = "dailyRainfallLabel";
            this.dailyRainfallLabel.Size = new System.Drawing.Size(22, 18);
            this.dailyRainfallLabel.TabIndex = 18;
            this.dailyRainfallLabel.Text = " --";
            // 
            // outsideTempLabel
            // 
            this.outsideTempLabel.AutoSize = true;
            this.outsideTempLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outsideTempLabel.Location = new System.Drawing.Point(202, 139);
            this.outsideTempLabel.Name = "outsideTempLabel";
            this.outsideTempLabel.Size = new System.Drawing.Size(22, 18);
            this.outsideTempLabel.TabIndex = 20;
            this.outsideTempLabel.Text = " --";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(5, 164);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(142, 15);
            this.label19.TabIndex = 19;
            this.label19.Text = "Barometric Pressure ";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(6, 142);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(146, 15);
            this.label20.TabIndex = 18;
            this.label20.Text = "Outside Temperature ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Rain Rate ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Daily Rainfall ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Wind Speed";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 15);
            this.label11.TabIndex = 15;
            this.label11.Text = "Wind Direction:";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Gray;
            this.tabPage3.Controls.Add(this.Encoders);
            this.tabPage3.Controls.Add(this.SensorNetworkSensorInitialization);
            this.tabPage3.Controls.Add(this.ThresholdsGroup);
            this.tabPage3.Controls.Add(this.grpProximitySensors);
            this.tabPage3.Controls.Add(this.TemperatureSensors);
            this.tabPage3.Controls.Add(this.GatesSensors);
            this.tabPage3.Controls.Add(this.Accelerometers);
            this.tabPage3.Controls.Add(this.WeatherStation);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(645, 505);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Sensor Overrides/Init";
            // 
            // Encoders
            // 
            this.Encoders.BackColor = System.Drawing.Color.Gainsboro;
            this.Encoders.Controls.Add(this.ElecationAbsoluteEncoder_lbl);
            this.Encoders.Controls.Add(this.btnElevationAbsoluteEncoder);
            this.Encoders.Controls.Add(this.btnAzimuthAbsoluteEncoder);
            this.Encoders.Controls.Add(this.AzimuthAbsoluteEncoder_lbl);
            this.Encoders.Location = new System.Drawing.Point(3, 307);
            this.Encoders.Name = "Encoders";
            this.Encoders.Padding = new System.Windows.Forms.Padding(2);
            this.Encoders.Size = new System.Drawing.Size(338, 80);
            this.Encoders.TabIndex = 16;
            this.Encoders.TabStop = false;
            this.Encoders.Text = "Encoders";
            // 
            // ElecationAbsoluteEncoder_lbl
            // 
            this.ElecationAbsoluteEncoder_lbl.AutoSize = true;
            this.ElecationAbsoluteEncoder_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElecationAbsoluteEncoder_lbl.Location = new System.Drawing.Point(9, 51);
            this.ElecationAbsoluteEncoder_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ElecationAbsoluteEncoder_lbl.Name = "ElecationAbsoluteEncoder_lbl";
            this.ElecationAbsoluteEncoder_lbl.Size = new System.Drawing.Size(164, 13);
            this.ElecationAbsoluteEncoder_lbl.TabIndex = 10;
            this.ElecationAbsoluteEncoder_lbl.Text = "Elevation Absolute Encoder";
            // 
            // btnElevationAbsoluteEncoder
            // 
            this.btnElevationAbsoluteEncoder.BackColor = System.Drawing.Color.Yellow;
            this.btnElevationAbsoluteEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnElevationAbsoluteEncoder.Location = new System.Drawing.Point(228, 46);
            this.btnElevationAbsoluteEncoder.Name = "btnElevationAbsoluteEncoder";
            this.btnElevationAbsoluteEncoder.Size = new System.Drawing.Size(89, 23);
            this.btnElevationAbsoluteEncoder.TabIndex = 14;
            this.btnElevationAbsoluteEncoder.Text = "NOT LOADED";
            this.btnElevationAbsoluteEncoder.UseVisualStyleBackColor = false;
            this.btnElevationAbsoluteEncoder.Click += new System.EventHandler(this.btnElevationAbsoluteEncoder_Click);
            // 
            // btnAzimuthAbsoluteEncoder
            // 
            this.btnAzimuthAbsoluteEncoder.BackColor = System.Drawing.Color.Yellow;
            this.btnAzimuthAbsoluteEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAzimuthAbsoluteEncoder.Location = new System.Drawing.Point(228, 14);
            this.btnAzimuthAbsoluteEncoder.Name = "btnAzimuthAbsoluteEncoder";
            this.btnAzimuthAbsoluteEncoder.Size = new System.Drawing.Size(89, 23);
            this.btnAzimuthAbsoluteEncoder.TabIndex = 13;
            this.btnAzimuthAbsoluteEncoder.Text = "NOT LOADED";
            this.btnAzimuthAbsoluteEncoder.UseVisualStyleBackColor = false;
            this.btnAzimuthAbsoluteEncoder.Click += new System.EventHandler(this.btnAzimuthAbsoluteEncoder_Click);
            // 
            // AzimuthAbsoluteEncoder_lbl
            // 
            this.AzimuthAbsoluteEncoder_lbl.AutoSize = true;
            this.AzimuthAbsoluteEncoder_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AzimuthAbsoluteEncoder_lbl.Location = new System.Drawing.Point(9, 19);
            this.AzimuthAbsoluteEncoder_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AzimuthAbsoluteEncoder_lbl.Name = "AzimuthAbsoluteEncoder_lbl";
            this.AzimuthAbsoluteEncoder_lbl.Size = new System.Drawing.Size(155, 13);
            this.AzimuthAbsoluteEncoder_lbl.TabIndex = 9;
            this.AzimuthAbsoluteEncoder_lbl.Text = "Azimuth Absolute Encoder";
            // 
            // SensorNetworkSensorInitialization
            // 
            this.SensorNetworkSensorInitialization.BackColor = System.Drawing.Color.Gainsboro;
            this.SensorNetworkSensorInitialization.Controls.Add(this.AmbientTempHumid);
            this.SensorNetworkSensorInitialization.Controls.Add(this.lblInitTimeout);
            this.SensorNetworkSensorInitialization.Controls.Add(this.lblDataTimeout);
            this.SensorNetworkSensorInitialization.Controls.Add(this.txtDataTimeout);
            this.SensorNetworkSensorInitialization.Controls.Add(this.txtInitTimeout);
            this.SensorNetworkSensorInitialization.Controls.Add(this.lblSNStatus);
            this.SensorNetworkSensorInitialization.Controls.Add(this.UpdateSensorInitiliazation);
            this.SensorNetworkSensorInitialization.Controls.Add(this.AzimuthEncoder);
            this.SensorNetworkSensorInitialization.Controls.Add(this.ElevationEncoder);
            this.SensorNetworkSensorInitialization.Controls.Add(this.CounterbalanceAccelerometer);
            this.SensorNetworkSensorInitialization.Controls.Add(this.ElevationAccelerometer);
            this.SensorNetworkSensorInitialization.Controls.Add(this.AzimuthAccelerometer);
            this.SensorNetworkSensorInitialization.Controls.Add(this.AzimuthTemperature1);
            this.SensorNetworkSensorInitialization.Controls.Add(this.ElevationTemperature1);
            this.SensorNetworkSensorInitialization.Location = new System.Drawing.Point(344, 136);
            this.SensorNetworkSensorInitialization.Name = "SensorNetworkSensorInitialization";
            this.SensorNetworkSensorInitialization.Padding = new System.Windows.Forms.Padding(2);
            this.SensorNetworkSensorInitialization.Size = new System.Drawing.Size(296, 364);
            this.SensorNetworkSensorInitialization.TabIndex = 17;
            this.SensorNetworkSensorInitialization.TabStop = false;
            this.SensorNetworkSensorInitialization.Text = "Sensor Network Sensor Initialization";
            // 
            // AmbientTempHumid
            // 
            this.AmbientTempHumid.AutoSize = true;
            this.AmbientTempHumid.Location = new System.Drawing.Point(5, 220);
            this.AmbientTempHumid.Name = "AmbientTempHumid";
            this.AmbientTempHumid.Size = new System.Drawing.Size(191, 17);
            this.AmbientTempHumid.TabIndex = 79;
            this.AmbientTempHumid.Text = "Ambient Temperature and Humidity";
            this.AmbientTempHumid.UseVisualStyleBackColor = true;
            // 
            // lblInitTimeout
            // 
            this.lblInitTimeout.AutoSize = true;
            this.lblInitTimeout.ForeColor = System.Drawing.Color.Black;
            this.lblInitTimeout.Location = new System.Drawing.Point(74, 314);
            this.lblInitTimeout.Name = "lblInitTimeout";
            this.lblInitTimeout.Size = new System.Drawing.Size(154, 13);
            this.lblInitTimeout.TabIndex = 78;
            this.lblInitTimeout.Text = "Initialization Timeout (seconds):";
            // 
            // lblDataTimeout
            // 
            this.lblDataTimeout.AutoSize = true;
            this.lblDataTimeout.ForeColor = System.Drawing.Color.Black;
            this.lblDataTimeout.Location = new System.Drawing.Point(60, 294);
            this.lblDataTimeout.Name = "lblDataTimeout";
            this.lblDataTimeout.Size = new System.Drawing.Size(168, 13);
            this.lblDataTimeout.TabIndex = 77;
            this.lblDataTimeout.Text = "Data Retrieval Timeout (seconds):";
            // 
            // txtDataTimeout
            // 
            this.txtDataTimeout.Location = new System.Drawing.Point(242, 291);
            this.txtDataTimeout.Name = "txtDataTimeout";
            this.txtDataTimeout.Size = new System.Drawing.Size(44, 20);
            this.txtDataTimeout.TabIndex = 76;
            this.txtDataTimeout.TextChanged += new System.EventHandler(this.txtDataTimeout_TextChanged);
            // 
            // txtInitTimeout
            // 
            this.txtInitTimeout.Location = new System.Drawing.Point(242, 312);
            this.txtInitTimeout.Name = "txtInitTimeout";
            this.txtInitTimeout.Size = new System.Drawing.Size(44, 20);
            this.txtInitTimeout.TabIndex = 75;
            this.txtInitTimeout.TextChanged += new System.EventHandler(this.txtInitTimeout_TextChanged);
            // 
            // lblSNStatus
            // 
            this.lblSNStatus.AutoSize = true;
            this.lblSNStatus.ForeColor = System.Drawing.Color.Black;
            this.lblSNStatus.Location = new System.Drawing.Point(10, 330);
            this.lblSNStatus.Name = "lblSNStatus";
            this.lblSNStatus.Size = new System.Drawing.Size(40, 13);
            this.lblSNStatus.TabIndex = 10;
            this.lblSNStatus.Text = "Status:";
            // 
            // UpdateSensorInitiliazation
            // 
            this.UpdateSensorInitiliazation.Location = new System.Drawing.Point(123, 334);
            this.UpdateSensorInitiliazation.Name = "UpdateSensorInitiliazation";
            this.UpdateSensorInitiliazation.Size = new System.Drawing.Size(164, 23);
            this.UpdateSensorInitiliazation.TabIndex = 9;
            this.UpdateSensorInitiliazation.Text = "Update Sensor Configuration";
            this.UpdateSensorInitiliazation.UseVisualStyleBackColor = true;
            this.UpdateSensorInitiliazation.Click += new System.EventHandler(this.UpdateSensorInitiliazation_Click);
            // 
            // AzimuthEncoder
            // 
            this.AzimuthEncoder.AutoSize = true;
            this.AzimuthEncoder.ForeColor = System.Drawing.Color.Red;
            this.AzimuthEncoder.Location = new System.Drawing.Point(5, 169);
            this.AzimuthEncoder.Name = "AzimuthEncoder";
            this.AzimuthEncoder.Size = new System.Drawing.Size(106, 17);
            this.AzimuthEncoder.TabIndex = 8;
            this.AzimuthEncoder.Text = "Azimuth Encoder";
            this.AzimuthEncoder.UseVisualStyleBackColor = true;
            // 
            // ElevationEncoder
            // 
            this.ElevationEncoder.AutoSize = true;
            this.ElevationEncoder.ForeColor = System.Drawing.Color.Red;
            this.ElevationEncoder.Location = new System.Drawing.Point(5, 151);
            this.ElevationEncoder.Name = "ElevationEncoder";
            this.ElevationEncoder.Size = new System.Drawing.Size(113, 17);
            this.ElevationEncoder.TabIndex = 7;
            this.ElevationEncoder.Text = "Elevation Encoder";
            this.ElevationEncoder.UseVisualStyleBackColor = true;
            // 
            // CounterbalanceAccelerometer
            // 
            this.CounterbalanceAccelerometer.AutoSize = true;
            this.CounterbalanceAccelerometer.Location = new System.Drawing.Point(5, 273);
            this.CounterbalanceAccelerometer.Name = "CounterbalanceAccelerometer";
            this.CounterbalanceAccelerometer.Size = new System.Drawing.Size(172, 17);
            this.CounterbalanceAccelerometer.TabIndex = 6;
            this.CounterbalanceAccelerometer.Text = "Counterbalance Accelerometer";
            this.CounterbalanceAccelerometer.UseVisualStyleBackColor = true;
            // 
            // ElevationAccelerometer
            // 
            this.ElevationAccelerometer.AutoSize = true;
            this.ElevationAccelerometer.Location = new System.Drawing.Point(5, 255);
            this.ElevationAccelerometer.Name = "ElevationAccelerometer";
            this.ElevationAccelerometer.Size = new System.Drawing.Size(141, 17);
            this.ElevationAccelerometer.TabIndex = 5;
            this.ElevationAccelerometer.Text = "Elevation Accelerometer";
            this.ElevationAccelerometer.UseVisualStyleBackColor = true;
            // 
            // AzimuthAccelerometer
            // 
            this.AzimuthAccelerometer.AutoSize = true;
            this.AzimuthAccelerometer.Location = new System.Drawing.Point(5, 237);
            this.AzimuthAccelerometer.Name = "AzimuthAccelerometer";
            this.AzimuthAccelerometer.Size = new System.Drawing.Size(134, 17);
            this.AzimuthAccelerometer.TabIndex = 4;
            this.AzimuthAccelerometer.Text = "Azimuth Accelerometer";
            this.AzimuthAccelerometer.UseVisualStyleBackColor = true;
            // 
            // AzimuthTemperature1
            // 
            this.AzimuthTemperature1.AutoSize = true;
            this.AzimuthTemperature1.Location = new System.Drawing.Point(5, 203);
            this.AzimuthTemperature1.Name = "AzimuthTemperature1";
            this.AzimuthTemperature1.Size = new System.Drawing.Size(156, 17);
            this.AzimuthTemperature1.TabIndex = 2;
            this.AzimuthTemperature1.Text = "Azimuth Motor Temperature";
            this.AzimuthTemperature1.UseVisualStyleBackColor = true;
            // 
            // ElevationTemperature1
            // 
            this.ElevationTemperature1.AutoSize = true;
            this.ElevationTemperature1.Location = new System.Drawing.Point(5, 186);
            this.ElevationTemperature1.Name = "ElevationTemperature1";
            this.ElevationTemperature1.Size = new System.Drawing.Size(163, 17);
            this.ElevationTemperature1.TabIndex = 0;
            this.ElevationTemperature1.Text = "Elevation Motor Temperature";
            this.ElevationTemperature1.UseVisualStyleBackColor = true;
            // 
            // ThresholdsGroup
            // 
            this.ThresholdsGroup.BackColor = System.Drawing.Color.Gainsboro;
            this.ThresholdsGroup.Controls.Add(this.txtLowerHumidLimit);
            this.ThresholdsGroup.Controls.Add(this.txtUpperHumidLimit);
            this.ThresholdsGroup.Controls.Add(this.txtLowerTempLimit);
            this.ThresholdsGroup.Controls.Add(this.txtUpperTempLimit);
            this.ThresholdsGroup.Controls.Add(this.lblLower);
            this.ThresholdsGroup.Controls.Add(this.lblUpper);
            this.ThresholdsGroup.Controls.Add(this.lblAmbHumid);
            this.ThresholdsGroup.Controls.Add(this.lblAmbTemp);
            this.ThresholdsGroup.Controls.Add(this.lblSWStop);
            this.ThresholdsGroup.Controls.Add(this.UpdateThresholdsButton);
            this.ThresholdsGroup.Controls.Add(this.txtLowerSWStopsLimit);
            this.ThresholdsGroup.Controls.Add(this.txtUpperSWStopsLimit);
            this.ThresholdsGroup.Location = new System.Drawing.Point(344, 3);
            this.ThresholdsGroup.Name = "ThresholdsGroup";
            this.ThresholdsGroup.Size = new System.Drawing.Size(296, 130);
            this.ThresholdsGroup.TabIndex = 0;
            this.ThresholdsGroup.TabStop = false;
            this.ThresholdsGroup.Text = "Thresholds";
            // 
            // txtLowerHumidLimit
            // 
            this.txtLowerHumidLimit.AccessibleName = "";
            this.txtLowerHumidLimit.Location = new System.Drawing.Point(228, 78);
            this.txtLowerHumidLimit.Name = "txtLowerHumidLimit";
            this.txtLowerHumidLimit.Size = new System.Drawing.Size(60, 20);
            this.txtLowerHumidLimit.TabIndex = 14;
            this.txtLowerHumidLimit.TextChanged += new System.EventHandler(this.txtLowerHumidLimit_TextChanged);
            // 
            // txtUpperHumidLimit
            // 
            this.txtUpperHumidLimit.AccessibleName = "";
            this.txtUpperHumidLimit.Location = new System.Drawing.Point(157, 78);
            this.txtUpperHumidLimit.Name = "txtUpperHumidLimit";
            this.txtUpperHumidLimit.Size = new System.Drawing.Size(60, 20);
            this.txtUpperHumidLimit.TabIndex = 13;
            this.txtUpperHumidLimit.TextChanged += new System.EventHandler(this.txtUpperHumidLimit_TextChanged);
            // 
            // txtLowerTempLimit
            // 
            this.txtLowerTempLimit.AccessibleName = "";
            this.txtLowerTempLimit.Location = new System.Drawing.Point(228, 51);
            this.txtLowerTempLimit.Name = "txtLowerTempLimit";
            this.txtLowerTempLimit.Size = new System.Drawing.Size(60, 20);
            this.txtLowerTempLimit.TabIndex = 12;
            this.txtLowerTempLimit.TextChanged += new System.EventHandler(this.txtLowerTempLimit_TextChanged);
            // 
            // txtUpperTempLimit
            // 
            this.txtUpperTempLimit.AccessibleName = "";
            this.txtUpperTempLimit.Location = new System.Drawing.Point(157, 51);
            this.txtUpperTempLimit.Name = "txtUpperTempLimit";
            this.txtUpperTempLimit.Size = new System.Drawing.Size(60, 20);
            this.txtUpperTempLimit.TabIndex = 11;
            this.txtUpperTempLimit.TextChanged += new System.EventHandler(this.txtUpperTempLimit_TextChanged);
            // 
            // lblLower
            // 
            this.lblLower.AutoSize = true;
            this.lblLower.Location = new System.Drawing.Point(239, 6);
            this.lblLower.Name = "lblLower";
            this.lblLower.Size = new System.Drawing.Size(36, 13);
            this.lblLower.TabIndex = 10;
            this.lblLower.Text = "Lower";
            // 
            // lblUpper
            // 
            this.lblUpper.AutoSize = true;
            this.lblUpper.Location = new System.Drawing.Point(168, 6);
            this.lblUpper.Name = "lblUpper";
            this.lblUpper.Size = new System.Drawing.Size(36, 13);
            this.lblUpper.TabIndex = 9;
            this.lblUpper.Text = "Upper";
            // 
            // lblAmbHumid
            // 
            this.lblAmbHumid.AutoSize = true;
            this.lblAmbHumid.Location = new System.Drawing.Point(11, 78);
            this.lblAmbHumid.Name = "lblAmbHumid";
            this.lblAmbHumid.Size = new System.Drawing.Size(108, 13);
            this.lblAmbHumid.TabIndex = 8;
            this.lblAmbHumid.Text = "Ambient Humidity (%):";
            // 
            // lblAmbTemp
            // 
            this.lblAmbTemp.AutoSize = true;
            this.lblAmbTemp.Location = new System.Drawing.Point(11, 51);
            this.lblAmbTemp.Name = "lblAmbTemp";
            this.lblAmbTemp.Size = new System.Drawing.Size(133, 13);
            this.lblAmbTemp.TabIndex = 7;
            this.lblAmbTemp.Text = "Ambient Temperature (°F ):";
            // 
            // lblSWStop
            // 
            this.lblSWStop.AutoSize = true;
            this.lblSWStop.Location = new System.Drawing.Point(11, 24);
            this.lblSWStop.Name = "lblSWStop";
            this.lblSWStop.Size = new System.Drawing.Size(111, 13);
            this.lblSWStop.TabIndex = 3;
            this.lblSWStop.Text = "Software-Stops Limits:";
            // 
            // UpdateThresholdsButton
            // 
            this.UpdateThresholdsButton.Location = new System.Drawing.Point(89, 103);
            this.UpdateThresholdsButton.Name = "UpdateThresholdsButton";
            this.UpdateThresholdsButton.Size = new System.Drawing.Size(116, 20);
            this.UpdateThresholdsButton.TabIndex = 2;
            this.UpdateThresholdsButton.Text = "Update Thresholds";
            this.UpdateThresholdsButton.UseVisualStyleBackColor = true;
            this.UpdateThresholdsButton.Click += new System.EventHandler(this.UpdateThresholdsButton_Click);
            // 
            // txtLowerSWStopsLimit
            // 
            this.txtLowerSWStopsLimit.AccessibleName = "SoftwareStopLowerLimitText";
            this.txtLowerSWStopsLimit.Location = new System.Drawing.Point(228, 24);
            this.txtLowerSWStopsLimit.Name = "txtLowerSWStopsLimit";
            this.txtLowerSWStopsLimit.Size = new System.Drawing.Size(60, 20);
            this.txtLowerSWStopsLimit.TabIndex = 1;
            this.txtLowerSWStopsLimit.TextChanged += new System.EventHandler(this.LowerSWStopsLimitText_TextChanged);
            // 
            // txtUpperSWStopsLimit
            // 
            this.txtUpperSWStopsLimit.AccessibleName = "SoftwareStopUpperLimitText";
            this.txtUpperSWStopsLimit.Location = new System.Drawing.Point(157, 24);
            this.txtUpperSWStopsLimit.Name = "txtUpperSWStopsLimit";
            this.txtUpperSWStopsLimit.Size = new System.Drawing.Size(60, 20);
            this.txtUpperSWStopsLimit.TabIndex = 0;
            this.txtUpperSWStopsLimit.TextChanged += new System.EventHandler(this.UpperSWStopsLimitText_TextChanged);
            // 
            // grpProximitySensors
            // 
            this.grpProximitySensors.BackColor = System.Drawing.Color.Gainsboro;
            this.grpProximitySensors.Controls.Add(this.label4);
            this.grpProximitySensors.Controls.Add(this.label21);
            this.grpProximitySensors.Controls.Add(this.ElivationLimitSwitch0);
            this.grpProximitySensors.Controls.Add(this.ElevationLimitSwitch90);
            this.grpProximitySensors.Location = new System.Drawing.Point(3, 219);
            this.grpProximitySensors.Name = "grpProximitySensors";
            this.grpProximitySensors.Padding = new System.Windows.Forms.Padding(2);
            this.grpProximitySensors.Size = new System.Drawing.Size(338, 85);
            this.grpProximitySensors.TabIndex = 15;
            this.grpProximitySensors.TabStop = false;
            this.grpProximitySensors.Text = " Proximity Sensors";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Elevation Limit Switch 90°";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(9, 23);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(148, 13);
            this.label21.TabIndex = 9;
            this.label21.Text = "Elevation Limit Switch 0°";
            // 
            // ElivationLimitSwitch0
            // 
            this.ElivationLimitSwitch0.BackColor = System.Drawing.Color.Yellow;
            this.ElivationLimitSwitch0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ElivationLimitSwitch0.Location = new System.Drawing.Point(228, 18);
            this.ElivationLimitSwitch0.Name = "ElivationLimitSwitch0";
            this.ElivationLimitSwitch0.Size = new System.Drawing.Size(89, 23);
            this.ElivationLimitSwitch0.TabIndex = 13;
            this.ElivationLimitSwitch0.Text = "NOT LOADED";
            this.ElivationLimitSwitch0.UseVisualStyleBackColor = false;
            this.ElivationLimitSwitch0.Click += new System.EventHandler(this.ElevationProximityOverideButton1_Click);
            // 
            // ElevationLimitSwitch90
            // 
            this.ElevationLimitSwitch90.BackColor = System.Drawing.Color.Yellow;
            this.ElevationLimitSwitch90.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ElevationLimitSwitch90.Location = new System.Drawing.Point(228, 50);
            this.ElevationLimitSwitch90.Name = "ElevationLimitSwitch90";
            this.ElevationLimitSwitch90.Size = new System.Drawing.Size(89, 23);
            this.ElevationLimitSwitch90.TabIndex = 14;
            this.ElevationLimitSwitch90.Text = "NOT LOADED";
            this.ElevationLimitSwitch90.UseVisualStyleBackColor = false;
            this.ElevationLimitSwitch90.Click += new System.EventHandler(this.ElevationProximityOverideButton2_Click);
            // 
            // TemperatureSensors
            // 
            this.TemperatureSensors.BackColor = System.Drawing.Color.Gainsboro;
            this.TemperatureSensors.Controls.Add(this.AmbTempHumidSensOverride);
            this.TemperatureSensors.Controls.Add(this.ElMotTempSensOverride);
            this.TemperatureSensors.Controls.Add(this.lblTempHumidOverride);
            this.TemperatureSensors.Controls.Add(this.label29);
            this.TemperatureSensors.Controls.Add(this.AzMotTempSensOverride);
            this.TemperatureSensors.Controls.Add(this.label28);
            this.TemperatureSensors.Location = new System.Drawing.Point(3, 97);
            this.TemperatureSensors.Name = "TemperatureSensors";
            this.TemperatureSensors.Size = new System.Drawing.Size(338, 119);
            this.TemperatureSensors.TabIndex = 29;
            this.TemperatureSensors.TabStop = false;
            this.TemperatureSensors.Text = "Temperature Sensors";
            // 
            // AmbTempHumidSensOverride
            // 
            this.AmbTempHumidSensOverride.BackColor = System.Drawing.Color.Yellow;
            this.AmbTempHumidSensOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AmbTempHumidSensOverride.Location = new System.Drawing.Point(228, 82);
            this.AmbTempHumidSensOverride.Margin = new System.Windows.Forms.Padding(2);
            this.AmbTempHumidSensOverride.Name = "AmbTempHumidSensOverride";
            this.AmbTempHumidSensOverride.Size = new System.Drawing.Size(89, 23);
            this.AmbTempHumidSensOverride.TabIndex = 13;
            this.AmbTempHumidSensOverride.Text = "NOT LOADED";
            this.AmbTempHumidSensOverride.UseVisualStyleBackColor = false;
            this.AmbTempHumidSensOverride.Click += new System.EventHandler(this.AmbTempHumidSensOverride_Click);
            // 
            // ElMotTempSensOverride
            // 
            this.ElMotTempSensOverride.BackColor = System.Drawing.Color.Yellow;
            this.ElMotTempSensOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ElMotTempSensOverride.Location = new System.Drawing.Point(228, 50);
            this.ElMotTempSensOverride.Margin = new System.Windows.Forms.Padding(2);
            this.ElMotTempSensOverride.Name = "ElMotTempSensOverride";
            this.ElMotTempSensOverride.Size = new System.Drawing.Size(89, 23);
            this.ElMotTempSensOverride.TabIndex = 15;
            this.ElMotTempSensOverride.Text = "NOT LOADED";
            this.ElMotTempSensOverride.UseVisualStyleBackColor = false;
            this.ElMotTempSensOverride.Click += new System.EventHandler(this.ElMotTempSensOverride_Click);
            // 
            // lblTempHumidOverride
            // 
            this.lblTempHumidOverride.AutoSize = true;
            this.lblTempHumidOverride.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempHumidOverride.Location = new System.Drawing.Point(9, 86);
            this.lblTempHumidOverride.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTempHumidOverride.Name = "lblTempHumidOverride";
            this.lblTempHumidOverride.Size = new System.Drawing.Size(222, 13);
            this.lblTempHumidOverride.TabIndex = 12;
            this.lblTempHumidOverride.Text = "Ambient Temperature Humidity Sensor";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(9, 55);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(214, 13);
            this.label29.TabIndex = 14;
            this.label29.Text = "Elevation Motor Temperature Sensor";
            // 
            // AzMotTempSensOverride
            // 
            this.AzMotTempSensOverride.BackColor = System.Drawing.Color.Yellow;
            this.AzMotTempSensOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AzMotTempSensOverride.Location = new System.Drawing.Point(228, 18);
            this.AzMotTempSensOverride.Margin = new System.Windows.Forms.Padding(2);
            this.AzMotTempSensOverride.Name = "AzMotTempSensOverride";
            this.AzMotTempSensOverride.Size = new System.Drawing.Size(89, 23);
            this.AzMotTempSensOverride.TabIndex = 13;
            this.AzMotTempSensOverride.Text = "NOT LOADED";
            this.AzMotTempSensOverride.UseVisualStyleBackColor = false;
            this.AzMotTempSensOverride.Click += new System.EventHandler(this.AzMotTempSensOverride_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(9, 23);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(205, 13);
            this.label28.TabIndex = 12;
            this.label28.Text = "Azimuth Motor Temperature Sensor";
            // 
            // GatesSensors
            // 
            this.GatesSensors.BackColor = System.Drawing.Color.Gainsboro;
            this.GatesSensors.Controls.Add(this.MGOverride);
            this.GatesSensors.Controls.Add(this.label27);
            this.GatesSensors.Location = new System.Drawing.Point(3, 50);
            this.GatesSensors.Name = "GatesSensors";
            this.GatesSensors.Size = new System.Drawing.Size(338, 44);
            this.GatesSensors.TabIndex = 28;
            this.GatesSensors.TabStop = false;
            this.GatesSensors.Text = "Gates Sensors";
            // 
            // MGOverride
            // 
            this.MGOverride.BackColor = System.Drawing.Color.Yellow;
            this.MGOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MGOverride.Location = new System.Drawing.Point(228, 13);
            this.MGOverride.Margin = new System.Windows.Forms.Padding(2);
            this.MGOverride.Name = "MGOverride";
            this.MGOverride.Size = new System.Drawing.Size(89, 23);
            this.MGOverride.TabIndex = 13;
            this.MGOverride.Text = "NOT LOADED";
            this.MGOverride.UseVisualStyleBackColor = false;
            this.MGOverride.Click += new System.EventHandler(this.MGOverride_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(9, 18);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(120, 13);
            this.label27.TabIndex = 12;
            this.label27.Text = "Main Gates Sensors";
            // 
            // Accelerometers
            // 
            this.Accelerometers.BackColor = System.Drawing.Color.Gainsboro;
            this.Accelerometers.Controls.Add(this.btnElevationMotorAccelerometerOverride);
            this.Accelerometers.Controls.Add(this.btnCounterbalanceMotorAccelerometerOverride);
            this.Accelerometers.Controls.Add(this.btnAzimuthMotorAccelerometerOverride);
            this.Accelerometers.Controls.Add(this.label33);
            this.Accelerometers.Controls.Add(this.label3);
            this.Accelerometers.Controls.Add(this.label22);
            this.Accelerometers.Location = new System.Drawing.Point(3, 390);
            this.Accelerometers.Name = "Accelerometers";
            this.Accelerometers.Padding = new System.Windows.Forms.Padding(2);
            this.Accelerometers.Size = new System.Drawing.Size(338, 110);
            this.Accelerometers.TabIndex = 0;
            this.Accelerometers.TabStop = false;
            this.Accelerometers.Text = "Accelerometers";
            // 
            // btnElevationMotorAccelerometerOverride
            // 
            this.btnElevationMotorAccelerometerOverride.BackColor = System.Drawing.Color.Yellow;
            this.btnElevationMotorAccelerometerOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnElevationMotorAccelerometerOverride.Location = new System.Drawing.Point(228, 49);
            this.btnElevationMotorAccelerometerOverride.Name = "btnElevationMotorAccelerometerOverride";
            this.btnElevationMotorAccelerometerOverride.Size = new System.Drawing.Size(89, 23);
            this.btnElevationMotorAccelerometerOverride.TabIndex = 14;
            this.btnElevationMotorAccelerometerOverride.Text = "NOT LOADED";
            this.btnElevationMotorAccelerometerOverride.UseVisualStyleBackColor = false;
            this.btnElevationMotorAccelerometerOverride.Click += new System.EventHandler(this.btnElevationMotorAccelerometerOverride_Click);
            // 
            // btnCounterbalanceMotorAccelerometerOverride
            // 
            this.btnCounterbalanceMotorAccelerometerOverride.BackColor = System.Drawing.Color.Yellow;
            this.btnCounterbalanceMotorAccelerometerOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCounterbalanceMotorAccelerometerOverride.Location = new System.Drawing.Point(228, 81);
            this.btnCounterbalanceMotorAccelerometerOverride.Name = "btnCounterbalanceMotorAccelerometerOverride";
            this.btnCounterbalanceMotorAccelerometerOverride.Size = new System.Drawing.Size(89, 23);
            this.btnCounterbalanceMotorAccelerometerOverride.TabIndex = 16;
            this.btnCounterbalanceMotorAccelerometerOverride.Text = "NOT LOADED";
            this.btnCounterbalanceMotorAccelerometerOverride.UseVisualStyleBackColor = false;
            this.btnCounterbalanceMotorAccelerometerOverride.Click += new System.EventHandler(this.btnCounterbalanceMotorAccelerometerOverride_Click);
            // 
            // btnAzimuthMotorAccelerometerOverride
            // 
            this.btnAzimuthMotorAccelerometerOverride.BackColor = System.Drawing.Color.Yellow;
            this.btnAzimuthMotorAccelerometerOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAzimuthMotorAccelerometerOverride.Location = new System.Drawing.Point(228, 17);
            this.btnAzimuthMotorAccelerometerOverride.Name = "btnAzimuthMotorAccelerometerOverride";
            this.btnAzimuthMotorAccelerometerOverride.Size = new System.Drawing.Size(89, 23);
            this.btnAzimuthMotorAccelerometerOverride.TabIndex = 13;
            this.btnAzimuthMotorAccelerometerOverride.Text = "NOT LOADED";
            this.btnAzimuthMotorAccelerometerOverride.UseVisualStyleBackColor = false;
            this.btnAzimuthMotorAccelerometerOverride.Click += new System.EventHandler(this.btnAzimuthMotorAccelerometerOverride_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(5, 86);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(181, 13);
            this.label33.TabIndex = 15;
            this.label33.Text = "Counterbalance Accelerometer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Elevation Motor Accelerometer";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(5, 22);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(172, 13);
            this.label22.TabIndex = 9;
            this.label22.Text = "Azimuth Motor Accelerometer";
            // 
            // WeatherStation
            // 
            this.WeatherStation.BackColor = System.Drawing.Color.Gainsboro;
            this.WeatherStation.Controls.Add(this.WSOverride);
            this.WeatherStation.Controls.Add(this.label24);
            this.WeatherStation.Location = new System.Drawing.Point(3, 3);
            this.WeatherStation.Margin = new System.Windows.Forms.Padding(2);
            this.WeatherStation.Name = "WeatherStation";
            this.WeatherStation.Padding = new System.Windows.Forms.Padding(2);
            this.WeatherStation.Size = new System.Drawing.Size(338, 44);
            this.WeatherStation.TabIndex = 27;
            this.WeatherStation.TabStop = false;
            this.WeatherStation.Text = "Weather Station";
            // 
            // WSOverride
            // 
            this.WSOverride.BackColor = System.Drawing.Color.Yellow;
            this.WSOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.WSOverride.Location = new System.Drawing.Point(228, 13);
            this.WSOverride.Margin = new System.Windows.Forms.Padding(2);
            this.WSOverride.Name = "WSOverride";
            this.WSOverride.Size = new System.Drawing.Size(89, 23);
            this.WSOverride.TabIndex = 13;
            this.WSOverride.Text = "NOT LOADED";
            this.WSOverride.UseVisualStyleBackColor = false;
            this.WSOverride.Click += new System.EventHandler(this.WSOverride_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(9, 18);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(99, 13);
            this.label24.TabIndex = 12;
            this.label24.Text = "Weather Station";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lblModeType);
            this.tabPage4.Controls.Add(this.spectraModeTypeVal);
            this.tabPage4.Controls.Add(this.lblFrequency);
            this.tabPage4.Controls.Add(this.frequencyVal);
            this.tabPage4.Controls.Add(this.spectraCyberScanChart);
            this.tabPage4.Controls.Add(this.lblIntegrationStep);
            this.tabPage4.Controls.Add(this.IntegrationStepVal);
            this.tabPage4.Controls.Add(this.lblDCGain);
            this.tabPage4.Controls.Add(this.DCGainVal);
            this.tabPage4.Controls.Add(this.lblIFGain);
            this.tabPage4.Controls.Add(this.IFGainVal);
            this.tabPage4.Controls.Add(this.lblBandwidth);
            this.tabPage4.Controls.Add(this.BandwidthVal);
            this.tabPage4.Controls.Add(this.lblOffsetVoltage);
            this.tabPage4.Controls.Add(this.OffsetVoltageVal);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(645, 505);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "RFData";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lblModeType
            // 
            this.lblModeType.AutoSize = true;
            this.lblModeType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModeType.Location = new System.Drawing.Point(155, 333);
            this.lblModeType.Name = "lblModeType";
            this.lblModeType.Size = new System.Drawing.Size(128, 15);
            this.lblModeType.TabIndex = 19;
            this.lblModeType.Text = "SpectraCyberMode";
            // 
            // spectraModeTypeVal
            // 
            this.spectraModeTypeVal.AutoSize = true;
            this.spectraModeTypeVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spectraModeTypeVal.Location = new System.Drawing.Point(369, 333);
            this.spectraModeTypeVal.Name = "spectraModeTypeVal";
            this.spectraModeTypeVal.Size = new System.Drawing.Size(35, 15);
            this.spectraModeTypeVal.TabIndex = 20;
            this.spectraModeTypeVal.Text = "NaN";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrequency.Location = new System.Drawing.Point(300, 369);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(73, 15);
            this.lblFrequency.TabIndex = 17;
            this.lblFrequency.Text = "Frequency";
            // 
            // frequencyVal
            // 
            this.frequencyVal.AutoSize = true;
            this.frequencyVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencyVal.Location = new System.Drawing.Point(514, 369);
            this.frequencyVal.Name = "frequencyVal";
            this.frequencyVal.Size = new System.Drawing.Size(35, 15);
            this.frequencyVal.TabIndex = 18;
            this.frequencyVal.Text = "NaN";
            // 
            // spectraCyberScanChart
            // 
            chartArea8.AxisX.Title = "Time";
            chartArea8.AxisY.Title = "RF Data";
            chartArea8.Name = "ChartArea1";
            this.spectraCyberScanChart.ChartAreas.Add(chartArea8);
            legend2.Name = "Legend1";
            this.spectraCyberScanChart.Legends.Add(legend2);
            this.spectraCyberScanChart.Location = new System.Drawing.Point(36, 0);
            this.spectraCyberScanChart.Name = "spectraCyberScanChart";
            series26.ChartArea = "ChartArea1";
            series26.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series26.Legend = "Legend1";
            series26.Name = "Data/Time";
            this.spectraCyberScanChart.Series.Add(series26);
            this.spectraCyberScanChart.Size = new System.Drawing.Size(571, 279);
            this.spectraCyberScanChart.TabIndex = 16;
            this.spectraCyberScanChart.Text = "spectraCyberScanChart";
            // 
            // lblIntegrationStep
            // 
            this.lblIntegrationStep.AutoSize = true;
            this.lblIntegrationStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntegrationStep.Location = new System.Drawing.Point(300, 399);
            this.lblIntegrationStep.Name = "lblIntegrationStep";
            this.lblIntegrationStep.Size = new System.Drawing.Size(105, 15);
            this.lblIntegrationStep.TabIndex = 14;
            this.lblIntegrationStep.Text = "IntegrationStep";
            // 
            // IntegrationStepVal
            // 
            this.IntegrationStepVal.AutoSize = true;
            this.IntegrationStepVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IntegrationStepVal.Location = new System.Drawing.Point(514, 399);
            this.IntegrationStepVal.Name = "IntegrationStepVal";
            this.IntegrationStepVal.Size = new System.Drawing.Size(35, 15);
            this.IntegrationStepVal.TabIndex = 15;
            this.IntegrationStepVal.Text = "NaN";
            // 
            // lblDCGain
            // 
            this.lblDCGain.AutoSize = true;
            this.lblDCGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDCGain.Location = new System.Drawing.Point(6, 425);
            this.lblDCGain.Name = "lblDCGain";
            this.lblDCGain.Size = new System.Drawing.Size(56, 15);
            this.lblDCGain.TabIndex = 12;
            this.lblDCGain.Text = "DCGain";
            // 
            // DCGainVal
            // 
            this.DCGainVal.AutoSize = true;
            this.DCGainVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DCGainVal.Location = new System.Drawing.Point(220, 425);
            this.DCGainVal.Name = "DCGainVal";
            this.DCGainVal.Size = new System.Drawing.Size(35, 15);
            this.DCGainVal.TabIndex = 13;
            this.DCGainVal.Text = "NaN";
            // 
            // lblIFGain
            // 
            this.lblIFGain.AutoSize = true;
            this.lblIFGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIFGain.Location = new System.Drawing.Point(6, 399);
            this.lblIFGain.Name = "lblIFGain";
            this.lblIFGain.Size = new System.Drawing.Size(53, 15);
            this.lblIFGain.TabIndex = 10;
            this.lblIFGain.Text = "IF Gain";
            // 
            // IFGainVal
            // 
            this.IFGainVal.AutoSize = true;
            this.IFGainVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IFGainVal.Location = new System.Drawing.Point(220, 399);
            this.IFGainVal.Name = "IFGainVal";
            this.IFGainVal.Size = new System.Drawing.Size(35, 15);
            this.IFGainVal.TabIndex = 11;
            this.IFGainVal.Text = "NaN";
            // 
            // lblBandwidth
            // 
            this.lblBandwidth.AutoSize = true;
            this.lblBandwidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBandwidth.Location = new System.Drawing.Point(6, 369);
            this.lblBandwidth.Name = "lblBandwidth";
            this.lblBandwidth.Size = new System.Drawing.Size(74, 15);
            this.lblBandwidth.TabIndex = 8;
            this.lblBandwidth.Text = "Bandwidth";
            // 
            // BandwidthVal
            // 
            this.BandwidthVal.AutoSize = true;
            this.BandwidthVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BandwidthVal.Location = new System.Drawing.Point(220, 369);
            this.BandwidthVal.Name = "BandwidthVal";
            this.BandwidthVal.Size = new System.Drawing.Size(35, 15);
            this.BandwidthVal.TabIndex = 9;
            this.BandwidthVal.Text = "NaN";
            // 
            // lblOffsetVoltage
            // 
            this.lblOffsetVoltage.AutoSize = true;
            this.lblOffsetVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffsetVoltage.Location = new System.Drawing.Point(300, 425);
            this.lblOffsetVoltage.Name = "lblOffsetVoltage";
            this.lblOffsetVoltage.Size = new System.Drawing.Size(92, 15);
            this.lblOffsetVoltage.TabIndex = 6;
            this.lblOffsetVoltage.Text = "OffsetVoltage";
            // 
            // OffsetVoltageVal
            // 
            this.OffsetVoltageVal.AutoSize = true;
            this.OffsetVoltageVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OffsetVoltageVal.Location = new System.Drawing.Point(514, 425);
            this.OffsetVoltageVal.Name = "OffsetVoltageVal";
            this.OffsetVoltageVal.Size = new System.Drawing.Size(35, 15);
            this.OffsetVoltageVal.TabIndex = 7;
            this.OffsetVoltageVal.Text = "NaN";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.consoleLogBox);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(645, 505);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Console Log";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // consoleLogBox
            // 
            this.consoleLogBox.AcceptsReturn = true;
            this.consoleLogBox.AcceptsTab = true;
            this.consoleLogBox.AllowDrop = true;
            this.consoleLogBox.Location = new System.Drawing.Point(6, 6);
            this.consoleLogBox.Multiline = true;
            this.consoleLogBox.Name = "consoleLogBox";
            this.consoleLogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleLogBox.Size = new System.Drawing.Size(631, 494);
            this.consoleLogBox.TabIndex = 0;
            // 
            // DiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 547);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label15);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(352, 175);
            this.Name = "DiagnosticsForm";
            this.Text = "DiagnosticsForm";
            this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.grpAmbTempHumid.ResumeLayout(false);
            this.grpAmbTempHumid.PerformLayout();
            this.grpAccelerometerSensorData.ResumeLayout(false);
            this.grpAccelerometerSensorData.PerformLayout();
            this.pnlCounterbalanceAccelerometer.ResumeLayout(false);
            this.pnlCounterbalanceAccelerometer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.counterBalanceAccChart)).EndInit();
            this.pnlElevationMotorAccelerometer.ResumeLayout(false);
            this.pnlElevationMotorAccelerometer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.elevationAccChart)).EndInit();
            this.pnlAzimuthMotorAccelerometer.ResumeLayout(false);
            this.pnlAzimuthMotorAccelerometer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.azimuthAccChart)).EndInit();
            this.grpSensorData.ResumeLayout(false);
            this.grpSensorData.PerformLayout();
            this.grpMcuStatus.ResumeLayout(false);
            this.grpMcuStatus.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.grpAbsoluteMotorPositionsTemperatures.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.Encoders.ResumeLayout(false);
            this.Encoders.PerformLayout();
            this.SensorNetworkSensorInitialization.ResumeLayout(false);
            this.SensorNetworkSensorInitialization.PerformLayout();
            this.ThresholdsGroup.ResumeLayout(false);
            this.ThresholdsGroup.PerformLayout();
            this.grpProximitySensors.ResumeLayout(false);
            this.grpProximitySensors.PerformLayout();
            this.TemperatureSensors.ResumeLayout(false);
            this.TemperatureSensors.PerformLayout();
            this.GatesSensors.ResumeLayout(false);
            this.GatesSensors.PerformLayout();
            this.Accelerometers.ResumeLayout(false);
            this.Accelerometers.PerformLayout();
            this.WeatherStation.ResumeLayout(false);
            this.WeatherStation.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectraCyberScanChart)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label windSpeedLabel;
        private System.Windows.Forms.Label windDirLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox startTimeTextBox;
        private System.Windows.Forms.TextBox endTimeTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblAzimuthTemp;
        private System.Windows.Forms.Label lblElevationTemp;
        private System.Windows.Forms.Label fldAzTemp;
        private System.Windows.Forms.Label fldElTemp;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.CheckBox selectDemo;
        private System.Windows.Forms.Label lblAbsEncoder;
        private System.Windows.Forms.Label lblEncoderDegrees;
        private System.Windows.Forms.Label lblAzEncoderDegrees;
        private System.Windows.Forms.Label lblEncoderTicks;
        private System.Windows.Forms.Label lblAzEncoderTicks;
        private System.Windows.Forms.Button btnAddOneEncoder;
        private System.Windows.Forms.Button btnAddFiveEncoder;
        private System.Windows.Forms.Button btnAddXEncoder;
        private System.Windows.Forms.Button btnSubtractOneEncoder;
        private System.Windows.Forms.Button btnSubtractFiveEncoder;
        private System.Windows.Forms.Button btnSubtractXEncoder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCustEncoderVal;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label lblElEncoderTicks;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblElEncoderDegrees;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox grpAbsoluteMotorPositionsTemperatures;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label barometricPressureLabel;
        private System.Windows.Forms.Label outsideTempLabel;
        private System.Windows.Forms.Label rainRateLabel;
        private System.Windows.Forms.Label dailyRainfallLabel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblElAbsPos;
        private System.Windows.Forms.Label lblAzAbsPos;
        private System.Windows.Forms.Label lblCurrentAzOrientation;
        private System.Windows.Forms.Label lblCurrentElOrientation;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button runDiagScriptsButton;
        private System.Windows.Forms.ComboBox diagnosticScriptCombo;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label insideTempLabel;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox Accelerometers;
        private System.Windows.Forms.Button ElevationLimitSwitch90;
        private System.Windows.Forms.Button ElivationLimitSwitch0;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox WeatherStation;
        private System.Windows.Forms.GroupBox TemperatureSensors;
        private System.Windows.Forms.Button ElMotTempSensOverride;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button AzMotTempSensOverride;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox GatesSensors;
        private System.Windows.Forms.Button MGOverride;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button WSOverride;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button farTempConvert;
        private System.Windows.Forms.Button celTempConvert;
        private System.Windows.Forms.Label InsideTempUnits;
        private System.Windows.Forms.Label rainRateUnits;
        private System.Windows.Forms.Label pressUnits;
        private System.Windows.Forms.Label dailyRainfallUnits;
        private System.Windows.Forms.Label outTempUnits;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label windSpeedUnits;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label AZTempUnitLabel;
        private System.Windows.Forms.Label ElTempUnitLabel;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label lblDCGain;
        private System.Windows.Forms.Label DCGainVal;
        private System.Windows.Forms.Label lblIFGain;
        private System.Windows.Forms.Label IFGainVal;
        private System.Windows.Forms.Label lblBandwidth;
        private System.Windows.Forms.Label BandwidthVal;
        private System.Windows.Forms.Label lblOffsetVoltage;
        private System.Windows.Forms.Label OffsetVoltageVal;
        private System.Windows.Forms.Label lblIntegrationStep;
        private System.Windows.Forms.Label IntegrationStepVal;
        private System.Windows.Forms.TextBox consoleLogBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart spectraCyberScanChart;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label frequencyVal;
        private System.Windows.Forms.Label lblModeType;
        private System.Windows.Forms.Label spectraModeTypeVal;
        private System.Windows.Forms.GroupBox SensorNetworkSensorInitialization;
        private System.Windows.Forms.GroupBox ThresholdsGroup;
        private System.Windows.Forms.GroupBox grpProximitySensors;
        private System.Windows.Forms.Button btnElevationMotorAccelerometerOverride;
        private System.Windows.Forms.Button btnAzimuthMotorAccelerometerOverride;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnCounterbalanceMotorAccelerometerOverride;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button UpdateSensorInitiliazation;
        private System.Windows.Forms.CheckBox AzimuthEncoder;
        private System.Windows.Forms.CheckBox ElevationEncoder;
        private System.Windows.Forms.CheckBox CounterbalanceAccelerometer;
        private System.Windows.Forms.CheckBox ElevationAccelerometer;
        private System.Windows.Forms.CheckBox AzimuthAccelerometer;
        private System.Windows.Forms.CheckBox AzimuthTemperature1;
        private System.Windows.Forms.CheckBox ElevationTemperature1;
        private System.Windows.Forms.GroupBox Encoders;
        private System.Windows.Forms.Button btnElevationAbsoluteEncoder;
        private System.Windows.Forms.Button btnAzimuthAbsoluteEncoder;
        private System.Windows.Forms.Label ElecationAbsoluteEncoder_lbl;
        private System.Windows.Forms.Label AzimuthAbsoluteEncoder_lbl;
        private System.Windows.Forms.Label lblSNStatus;
        private System.Windows.Forms.GroupBox grpMcuStatus;
        private System.Windows.Forms.Label lblMCUStatus;
        private System.Windows.Forms.Label lblMCUStatusText;
        private System.Windows.Forms.Label lblMCUErrors;
        private System.Windows.Forms.Button btnResetMcuErrors;
        private System.Windows.Forms.GroupBox grpAccelerometerSensorData;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Panel pnlCounterbalanceAccelerometer;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Panel pnlElevationMotorAccelerometer;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Panel pnlAzimuthMotorAccelerometer;
        private System.Windows.Forms.GroupBox grpSensorData;
        private System.Windows.Forms.Label lbGateStat;
        private System.Windows.Forms.Label lbEstopStat;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label lblAzHome1;
        private System.Windows.Forms.Label lblElLimStatus2;
        private System.Windows.Forms.Label lblElLimStatus1;
        private System.Windows.Forms.Label lblElHome;
        private System.Windows.Forms.Label lblELHomeStatus;
        private System.Windows.Forms.Label lblAzHomeStatus1;
        private System.Windows.Forms.Label lblAzLimit1;
        private System.Windows.Forms.Label lblAzLimit2;
        private System.Windows.Forms.DataVisualization.Charting.Chart elevationAccChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart counterBalanceAccChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart azimuthAccChart;
        private System.Windows.Forms.Label lblCbDisabled;
        private System.Windows.Forms.Label lblElDisabled;
        private System.Windows.Forms.Label lblAzDisabled;
        private System.Windows.Forms.Label lblInitTimeout;
        private System.Windows.Forms.Label lblDataTimeout;
        private System.Windows.Forms.TextBox txtDataTimeout;
        private System.Windows.Forms.TextBox txtInitTimeout;
        private System.Windows.Forms.ToolTip DataTimeoutValidation;
        private System.Windows.Forms.ToolTip InitTimeoutValidation;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtLowerSWStopsLimit;
        private System.Windows.Forms.TextBox txtUpperSWStopsLimit;
        private System.Windows.Forms.Button UpdateThresholdsButton;
        private System.Windows.Forms.Label lblSWStop;
        private System.Windows.Forms.ToolTip UpperSWStopsValidation;
        private System.Windows.Forms.ToolTip LowerSWStopsValidation;
        private System.Windows.Forms.GroupBox grpAmbTempHumid;
        private System.Windows.Forms.Label lblAmbientTempUnit;
        private System.Windows.Forms.Label lblAmbientHumidityUnit;
        private System.Windows.Forms.Label lblAmbientTemp;
        private System.Windows.Forms.Label fldAmbientHumidity;
        private System.Windows.Forms.Label fldAmbientTemp;
        private System.Windows.Forms.Label lblAmbientHumidity;
        private System.Windows.Forms.CheckBox AmbientTempHumid;
        private System.Windows.Forms.Label lblAmbientDewPointUnit;
        private System.Windows.Forms.Label fldAmbientDewPoint;
        private System.Windows.Forms.Label lblAmbientDewPoint;
        private System.Windows.Forms.Button AmbTempHumidSensOverride;
        private System.Windows.Forms.Label lblTempHumidOverride;
        private System.Windows.Forms.Label lblAmbHumid;
        private System.Windows.Forms.Label lblAmbTemp;
        private System.Windows.Forms.TextBox txtLowerHumidLimit;
        private System.Windows.Forms.TextBox txtUpperHumidLimit;
        private System.Windows.Forms.TextBox txtLowerTempLimit;
        private System.Windows.Forms.TextBox txtUpperTempLimit;
        private System.Windows.Forms.Label lblLower;
        private System.Windows.Forms.Label lblUpper;
        private System.Windows.Forms.ToolTip AmbTempValidation;
        private System.Windows.Forms.ToolTip AmbHumidValidation;
    }
}