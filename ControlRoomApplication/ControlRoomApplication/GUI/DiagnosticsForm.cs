﻿using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;
using ControlRoomApplication.Simulators.Hardware.MCU;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;
using ControlRoomApplication.Database;
using ControlRoomApplication.Constants;
using System;
using ControlRoomApplication.Main;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Controllers.Communications;
using System.Threading;
using System.ComponentModel;
using ControlRoomApplication.Util;
using System.Linq;
using ControlRoomApplication.Controllers.SensorNetwork;
using System.Drawing.Printing;

namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
        EncoderReader encoderReader = new EncoderReader("192.168.7.2", 1602);
        ControlRoomApplication.Entities.Orientation azimuthOrientation = new ControlRoomApplication.Entities.Orientation();
        private RadioTelescopeController rtController { get; set; }

        // Thread that monitors the overrides, and updates the buttons as necessary
        BackgroundWorker updateOverride;


        private int demoIndex = 0;
        //private PLC PLC; This needs to be defined once I can get find the currect import

        FakeEncoderSensor myEncoder = new FakeEncoderSensor();
        /***********DEMO MODE VARIABLES**************/
        DateTime currentEncodDate = DateTime.Now;

        private bool graphClear = true;

        /***********DEMO MODE VARIABLES END*********/
        // Encoder Variables
        double _azEncoderDegrees = 0;
        double _elEncoderDegrees = 0;
        double _elevationTemp = 0;
        int _azEncoderTicks = 0;
        int _elEncoderTicks = 0;

        // Azimuth Limit Switch Variables
        bool _azCCWLimitChange = false;
        bool _azCWLimitOld = false;

        bool _azCWLimitChange = false;
        bool _azCCWLimitOld = false;

        // Elevation Limit Switch Variables
        bool _elLowerLimitChange = false;
        bool _elLowerLimitOld = false;

        bool _elUpperLimitChange = false;
        bool _elUpperLimitOld = false;

        // Azimuth Proximity Sensor Variables
        bool _azCCWProxOld = false;
        bool _azCCWProxChange = false;

        bool _azCWProxOld = false;
        bool _azCWProxChange = false;

        bool _azCloserUpperProx = false;

        // Elevation Proximity Sensor Variables
        bool _elLowerProxOld = false;
        bool _elLowerProxChange = false;

        bool _elUpperProxOld = false;
        bool _elUpperProxChange = false;

        // Alert Flags
        bool farenheit = true;

        private int rtId;


        // Config file for the sensor network server to use
        SensorNetworkConfig SensorNetworkConfig;
        

        // This is being passed through so the Weather Station override bool can be modified
        private readonly MainForm mainF;

        private string[] statuses = { "Offline", "Offline", "Offline", "Offline" };
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes the diagnostic form based off of the specified configuration.
        /// </summary>
        /// 
        public DiagnosticsForm(ControlRoom controlRoom, int new_rtId, MainForm mainF)
        {
            InitializeComponent();

            this.controlRoom = controlRoom;

            rtId = new_rtId;

            this.mainF = mainF;
            rtController = controlRoom.RadioTelescopeControllers.Find(x => x.RadioTelescope.Id == rtId);


            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "Hardware";
            dataGridView1.Columns[1].HeaderText = "Status";

            GetHardwareStatuses();
            string[] spectraCyberRow = { "SpectraCyber", statuses[0] };
            string[] weatherStationRow = { "Weather Station", statuses[1] };
            string[] mcuRow = { "MCU", statuses[2] };
            string[] tempSensorRow = { "Temp Sensor", statuses[3] };

            dataGridView1.Rows.Add(spectraCyberRow);
            dataGridView1.Rows.Add(weatherStationRow);
            dataGridView1.Rows.Add(mcuRow);
            dataGridView1.Update();

            //MCU_Statui.ColumnCount = 2;
            //MCU_Statui.Columns[0].HeaderText = "Status name";
            //MCU_Statui.Columns[1].HeaderText = "value";

            SetCurrentWeatherData();
            runDiagScriptsButton.Enabled = false;

            // Updates the override buttons so they reflect what the actual override values are
            bool currMain = rtController.overrides.overrideGate;
            bool currWS = controlRoom.weatherStationOverride;
            bool currAZ = rtController.overrides.overrideAzimuthMotTemp;
            bool currEL = rtController.overrides.overrideElevatMotTemp;
            bool currElProx0 = rtController.overrides.overrideElevatProx0;
            bool currElProx90 = rtController.overrides.overrideElevatProx90;
            bool currAzimuthAbsEncoder = rtController.overrides.overrideAzimuthAbsEncoder;
            bool currElevationAbsEncoder = rtController.overrides.overrideElevationAbsEncoder;
            bool currAzimuthAccelerometer = rtController.overrides.overrideAzimuthAccelerometer;
            bool currElevationAccelerometer = rtController.overrides.overrideElevationAccelerometer;
            bool currCounterbalanceAccelerometer = rtController.overrides.overrideCounterbalanceAccelerometer;
            updateButtons(currMain, currWS, currAZ, currEL, currElProx0, currElProx90, 
                currAzimuthAbsEncoder, currElevationAbsEncoder, currAzimuthAccelerometer, currElevationAccelerometer, currCounterbalanceAccelerometer);
            
            updateOverride = new BackgroundWorker();
            updateOverride.DoWork += new DoWorkEventHandler(checkOverrideVars);
            updateOverride.RunWorkerAsync();

            //Initialize Color
            celTempConvert.BackColor = System.Drawing.Color.DarkGray;
            farTempConvert.BackColor = System.Drawing.Color.LimeGreen;

            lblSNStatus.Text = "";

            // Set sensor initialization checkboxes to reflect what is stored in the database
            SensorNetworkConfig = rtController.RadioTelescope.SensorNetworkServer.InitializationClient.config;

            AzimuthTemperature1.Checked = SensorNetworkConfig.AzimuthTemp1Init;
            AzimuthTemperature2.Checked = SensorNetworkConfig.AzimuthTemp2Init;
            ElevationTemperature1.Checked = SensorNetworkConfig.ElevationTemp1Init;
            ElevationTemperature2.Checked = SensorNetworkConfig.ElevationTemp2Init;
            AzimuthAccelerometer.Checked = SensorNetworkConfig.AzimuthAccelerometerInit;
            ElevationAccelerometer.Checked = SensorNetworkConfig.ElevationAccelerometerInit;
            CounterbalanceAccelerometer.Checked = SensorNetworkConfig.CounterbalanceAccelerometerInit;
            ElevationEncoder.Checked = SensorNetworkConfig.ElevationEncoderInit;
            AzimuthEncoder.Checked = SensorNetworkConfig.AzimuthEncoderInit;

            logger.Info(Utilities.GetTimeStamp() + ": DiagnosticsForm Initalized");
        }

        private void SetCurrentWeatherData()
        {
            windSpeedLabel.Text = Math.Round(controlRoom.WeatherStation.GetWindSpeed(), 2).ToString();
            windDirLabel.Text = controlRoom.WeatherStation.GetWindDirection();
            dailyRainfallLabel.Text = Math.Round(controlRoom.WeatherStation.GetDailyRain(), 2).ToString();
            rainRateLabel.Text = Math.Round(controlRoom.WeatherStation.GetRainRate(), 2).ToString();
            //outsideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetOutsideTemp(), 2).ToString();
            //insideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetInsideTemp(), 2).ToString();
            barometricPressureLabel.Text = Math.Round(controlRoom.WeatherStation.GetBarometricPressure(), 2).ToString();
        }

        /// <summary>
        /// Gets and displays the current statuses of the hardware components for the specified configuration.
        /// </summary>
        private void GetHardwareStatuses() {
            if (rtController.RadioTelescope.SpectraCyberController.IsConsideredAlive()) {
                statuses[0] = "Online";
            }

            if (controlRoom.WeatherStation.IsConsideredAlive()) {
                statuses[1] = "Online";
            }
        }

        public delegate void SetStartTimeTextCallback(string text);
        public void SetStartTimeText(string text)
        {
            if (startTimeTextBox.InvokeRequired)
            {
                SetStartTimeTextCallback d = new SetStartTimeTextCallback(SetStartTimeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                startTimeTextBox.Text = text;
            }
        }

        public delegate void SetEndTimeTextCallback(string text);
        public void SetEndTimeText(string text)
        {
            if (endTimeTextBox.InvokeRequired)
            {
                SetEndTimeTextCallback d = new SetEndTimeTextCallback(SetEndTimeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                endTimeTextBox.Text = text;
            }
        }

        public delegate void SetApptStatusTextCallback(string text);
        public void SetApptStatusText(string text)
        {
            if (statusTextBox.InvokeRequired)
            {
                SetApptStatusTextCallback d = new SetApptStatusTextCallback(SetApptStatusText);
                Invoke(d, new object[] { text });
            }
            else
            {
                statusTextBox.Text = text;
            }
        }


        /***************************************************************
         * ***************TIMER TICK FUNCTION STARTS HERE***************
         * ******this comment was added to make this easier to find*****
         * ************************************************************/
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            double currWindSpeed = controlRoom.WeatherStation.GetWindSpeed();//wind speed

            //double testVal = rtController.RadioTelescope.Encoders.GetCurentOrientation().Azimuth;

            _azEncoderDegrees = rtController.RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Azimuth;
            _elEncoderDegrees = rtController.RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Elevation;
            lblAzAbsPos.Text = Math.Round(_azEncoderDegrees, 2).ToString();
            lblElAbsPos.Text = Math.Round(_elEncoderDegrees, 2).ToString();

            timer1.Interval = 200;

            //TODO: Investigate if needed 
            /*
            if (selectDemo.Checked == true)
            {
                // Simulating Encoder Sensors
                TimeSpan elapsedEncodTime = DateTime.Now - currentEncodDate;

                if (elapsedEncodTime.TotalSeconds > 15)
                {
                    if (myEncoder.getLeftOrRight() == true && (myEncoder.GetAzimuthAngle() < 340 && myEncoder.GetAzimuthAngle() > 15))
                        myEncoder.SetAzimuthAngle(340);
                    else if (myEncoder.getLeftOrRight() == false && (myEncoder.GetAzimuthAngle() > 15 && myEncoder.GetAzimuthAngle() < 340))
                        myEncoder.SetAzimuthAngle(15);

                    if (myEncoder.getUpOrDown() == true && (myEncoder.GetElevationAngle() < 80 && myEncoder.GetElevationAngle() > 0))
                        myEncoder.SetElevationAngle(80);
                    else if (myEncoder.getUpOrDown() == false && (myEncoder.GetElevationAngle() > 0 && myEncoder.GetElevationAngle() < 80))
                        myEncoder.SetElevationAngle(0);

                    currentEncodDate = DateTime.Now;
                }

                _azEncoderDegrees = myEncoder.GetAzimuthAngle();
                _elEncoderDegrees = myEncoder.GetElevationAngle();

                _azEncoderTicks = (int)(_azEncoderDegrees * 11.38);
                _elEncoderTicks = (int)(_elEncoderDegrees * 2.8);


            }
            */

            Temperature[] ElMotTemps = rtController.RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp;
            Temperature[] AzMotTemps = rtController.RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp;

            // these come in as celsius
            double ElMotTemp = ElMotTemps[ElMotTemps.Length - 1].temp;
            double AzMotTemp = AzMotTemps[AzMotTemps.Length - 1].temp;

            float insideTemp = controlRoom.WeatherStation.GetInsideTemp();
            float outsideTemp = controlRoom.WeatherStation.GetOutsideTemp();

            double insideTempCel = (insideTemp - 32) * (5.0 / 9);
            double outsideTempCel = (outsideTemp - 32) * (5.0 / 9);

            // farenheit conversion
            double ElMotTempFahrenheit = (ElMotTemp * (5.0 / 9)) + 32;
            double AzMotTempFahrenheit = (AzMotTemp * (5.0 / 9)) + 32;



            //Celsius
            if (farenheit == false)
            {
                InsideTempUnits.Text = "Celsius";
                outTempUnits.Text = "Celsius";
                AZTempUnitLabel.Text = "Celsius";
                ElTempUnitLabel.Text = "Celsius";
                outsideTempLabel.Text = Math.Round(insideTempCel, 2).ToString();
                insideTempLabel.Text = Math.Round(outsideTempCel, 2).ToString();

                if (SensorNetworkConfig.ElevationTemp1Init)
                {
                    fldElTemp.Text = Math.Round(ElMotTemp, 2).ToString();
                }
                else
                {
                    fldElTemp.Text = "--";
                }

                if (SensorNetworkConfig.AzimuthTemp1Init)
                {
                    fldAzTemp.Text = Math.Round(AzMotTemp, 2).ToString();
                }
                else
                {
                    fldAzTemp.Text = "--";
                }
            }
            //Farenheit
            else if (farenheit == true)
            {
                InsideTempUnits.Text = "Farenheit";
                outTempUnits.Text = "Farenheit";
                AZTempUnitLabel.Text = "Farenheit";
                ElTempUnitLabel.Text = "Farenheit";
                outsideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetOutsideTemp(), 2).ToString();
                insideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetInsideTemp(), 2).ToString();

                if (SensorNetworkConfig.ElevationTemp1Init)
                {
                    fldElTemp.Text = Math.Round(ElMotTempFahrenheit, 2).ToString();
                }
                else
                {
                    fldElTemp.Text = "--";
                }

                if (SensorNetworkConfig.AzimuthTemp1Init)
                {
                    fldAzTemp.Text = Math.Round(AzMotTempFahrenheit, 2).ToString();
                }
                else
                {
                    fldAzTemp.Text = "--";
                }
            }

            // Encoder Position in both degrees and motor ticks
            lblAzEncoderDegrees.Text = Math.Round(_azEncoderDegrees, 2).ToString();
            lblAzEncoderTicks.Text = _azEncoderTicks.ToString();

            // lblElEncoderDegrees.Text = _elEncoderDegrees.ToString();
            lblElEncoderDegrees.Text =Math.Round(_elEncoderDegrees, 2).ToString();
            lblElEncoderTicks.Text = _elEncoderTicks.ToString();

            // Proximity and Limit Switches
            lblElLimStatus1.Text = rtController.RadioTelescope.PLCDriver.limitSwitchData.Elevation_Lower_Limit.ToString();
            lblElLimStatus2.Text = rtController.RadioTelescope.PLCDriver.limitSwitchData.Elevation_Upper_Limit.ToString();

            lblAzHomeStatus1.Text = rtController.RadioTelescope.PLCDriver.homeSensorData.Azimuth_Home_One.ToString();
            lblELHomeStatus.Text = rtController.RadioTelescope.PLCDriver.homeSensorData.Elevation_Home.ToString();

            lbEstopStat.Text = rtController.RadioTelescope.PLCDriver.plcInput.Estop.ToString();
            lbGateStat.Text = rtController.RadioTelescope.PLCDriver.plcInput.Gate_Sensor.ToString();

            GetHardwareStatuses();

            SetCurrentWeatherData();

            dataGridView1.Update();

            // Spectra Cyber Tab Updates
            spectraModeTypeVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.spectraCyberMode.ToString();

            BandwidthVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.bandwidth.GetValue();
            frequencyVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.frequency.ToString();
            IFGainVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.IFGain.ToString();

            if(rtController.RadioTelescope.SpectraCyberController.configVals.spectraCyberMode == SpectraCyberModeTypeEnum.SPECTRAL)
                DCGainVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.specGain.GetValue();
            else if(rtController.RadioTelescope.SpectraCyberController.configVals.spectraCyberMode == SpectraCyberModeTypeEnum.CONTINUUM)
                DCGainVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.contGain.GetValue();

            IntegrationStepVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.integrationStep.ToString();

            OffsetVoltageVal.Text = rtController.RadioTelescope.SpectraCyberController.configVals.offsetVoltage.ToString();

            // Spectra Cyber Graph Update
            if (rtController.RadioTelescope.SpectraCyberController.Schedule.GetMode() == SpectraCyberScanScheduleMode.CONTINUOUS_SCAN
                || rtController.RadioTelescope.SpectraCyberController.Schedule.GetMode() == SpectraCyberScanScheduleMode.SCHEDULED_SCAN
                || rtController.RadioTelescope.SpectraCyberController.Schedule.GetMode() == SpectraCyberScanScheduleMode.SINGLE_SCAN)
            {
                if (rtController.RadioTelescope.SpectraCyberController.configVals.spectraCyberMode == SpectraCyberModeTypeEnum.SPECTRAL)
                {
                    if(graphClear == true)
                    {
                        spectraCyberScanChart.Series.Clear();
                        spectraCyberScanChart.Series.Add("Spectral");
                        spectraCyberScanChart.Series["Spectral"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                        spectraCyberScanChart.ChartAreas["ChartArea1"].AxisX.Title = "Frequency";
                        graphClear = false;
                    }

                    double intensity = rtController.RadioTelescope.SpectraCyberController.configVals.rfData;
                    double frequency = rtController.RadioTelescope.SpectraCyberController.configVals.bandscan;

                    spectraCyberScanChart.Series["Spectral"].Points.AddXY(frequency, intensity);

                    if (frequency >= rtController.RadioTelescope.SpectraCyberController.configVals.frequency / 2)
                        spectraCyberScanChart.Series["Spectral"].Points.Clear();
                }
                else if (rtController.RadioTelescope.SpectraCyberController.configVals.spectraCyberMode == SpectraCyberModeTypeEnum.CONTINUUM)
                {
                    if (graphClear == true)
                    {
                        spectraCyberScanChart.Series.Clear();
                        spectraCyberScanChart.Series.Add("Continuum");
                        spectraCyberScanChart.Series["Continuum"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                        spectraCyberScanChart.ChartAreas["ChartArea1"].AxisX.Title = "Time";
                        graphClear = false;
                    }

                    double intensity = rtController.RadioTelescope.SpectraCyberController.configVals.rfData;
                    double time = rtController.RadioTelescope.SpectraCyberController.configVals.scanTime;

                    spectraCyberScanChart.Series["Continuum"].Points.AddXY(time, intensity);

                }
            }
            else
            {
                graphClear = true;
            }

            // Update MCU error status

            // First retrieve errors
            String errors = string.Join("\n", rtController.RadioTelescope.PLCDriver.CheckMCUErrors().
                Select(s =>
                    s.Item1.ToString() + ": " + s.Item2.ToString()
                ).ToArray());

            if(!errors.Equals(""))
            {
                lblMCUStatus.ForeColor = Color.Red;
                lblMCUStatus.Text = "Contains Errors";

            }
            else
            {
                lblMCUStatus.ForeColor = Color.Green;
                lblMCUStatus.Text = "Running";
            }

            // Display errors
            lblMCUErrors.Text = errors;

            // Console Log Output Update
            consoleLogBox.Text = mainF.log.loggerQueue;

            if (!consoleLogBox.Focused)
            {
                consoleLogBox.SelectionStart = consoleLogBox.TextLength;
                consoleLogBox.ScrollToCaret();
            }


            // FFT transformations -- currently not in use
            //double[] fftX = FftSharp.Transform.FFTpower(eleAccelerometerX);
            //double[]fft
            //double SAMPLE_RATE = 0.8;

            // Create an array of frequencies for each point of the FFT
            //double[] freqs = FftSharp.Transform.FFTfreq(SAMPLE_RATE , fftX.Length);

            // Azimuth Accelerometer Chart /////////////////////////////////////////////
            if (SensorNetworkConfig.AzimuthAccelerometerInit)
            {
                lblAzDisabled.Visible = false;
                Acceleration[] azimuthAccel = rtController.RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorAccl;
                azimuthAccChart.ChartAreas[0].AxisX.Minimum = double.NaN;
                azimuthAccChart.ChartAreas[0].AxisX.Maximum = double.NaN;

                for (int i = 0; i < azimuthAccel.Length; i++)
                {
                    azimuthAccChart.Series["x"].Points.AddY(azimuthAccel[i].x);
                    azimuthAccChart.Series["y"].Points.AddY(azimuthAccel[i].y);
                    azimuthAccChart.Series["z"].Points.AddY(azimuthAccel[i].z);


                    if (azimuthAccChart.Series["x"].Points.Count > 500)
                    {
                        azimuthAccChart.Series["x"].Points.RemoveAt(0);
                        azimuthAccChart.Series["y"].Points.RemoveAt(0);
                        azimuthAccChart.Series["z"].Points.RemoveAt(0);
                    }
                    azimuthAccChart.ChartAreas[0].RecalculateAxesScale();
                }
            }
            else
            {
                // This all only needs to be executed once when it is reached. This if statement
                // blocks these five lines from being executed during every single tick
                if (!lblAzDisabled.Visible)
                {
                    lblAzDisabled.Visible = true;
                    lblAzDisabled.Text = "Azimuth Motor Accelerometer Disabled";
                    azimuthAccChart.Series["x"].Points.Clear();
                    azimuthAccChart.Series["y"].Points.Clear();
                    azimuthAccChart.Series["z"].Points.Clear();
                }
            }
            ///////////////////////////////////////////////////////////////////////////////

            // Elevation Accelerometer Chart /////////////////////////////////////////////
            if (SensorNetworkConfig.ElevationAccelerometerInit)
            {
                lblElDisabled.Visible = false;
                Acceleration[] eleAccel = rtController.RadioTelescope.SensorNetworkServer.CurrentElevationMotorAccl;

                elevationAccChart.ChartAreas[0].AxisX.Minimum = double.NaN;
                elevationAccChart.ChartAreas[0].AxisX.Maximum = double.NaN;

                for (int i = 0; i < eleAccel.Length; i++)
                {
                    elevationAccChart.Series["x"].Points.AddY(eleAccel[i].x);
                    elevationAccChart.Series["y"].Points.AddY(eleAccel[i].y);
                    elevationAccChart.Series["z"].Points.AddY(eleAccel[i].z);


                    if (elevationAccChart.Series["x"].Points.Count > 500)
                    {
                        elevationAccChart.Series["x"].Points.RemoveAt(0);
                        elevationAccChart.Series["y"].Points.RemoveAt(0);
                        elevationAccChart.Series["z"].Points.RemoveAt(0);
                    }
                    elevationAccChart.ChartAreas[0].RecalculateAxesScale();
                }
            }
            else
            {
                // This all only needs to be executed once when it is reached. This if statement
                // blocks these five lines from being executed during every single tick
                if (!lblElDisabled.Visible)
                {
                    lblElDisabled.Visible = true;
                    lblElDisabled.Text = "Elevation Motor Accelerometer Disabled";
                    elevationAccChart.Series["x"].Points.Clear();
                    elevationAccChart.Series["y"].Points.Clear();
                    elevationAccChart.Series["z"].Points.Clear();
                }
            }
            ///////////////////////////////////////////////////////////////////////////////

            // CounterBalance Accelerometer Chart /////////////////////////////////////////////
            if (SensorNetworkConfig.CounterbalanceAccelerometerInit)
            {
                lblCbDisabled.Visible = false;
                Acceleration[] cbAccel = rtController.RadioTelescope.SensorNetworkServer.CurrentCounterbalanceAccl;
                counterBalanceAccChart.ChartAreas[0].AxisX.Minimum = double.NaN;
                counterBalanceAccChart.ChartAreas[0].AxisX.Maximum = double.NaN;

                for (int i = 0; i < cbAccel.Length; i++)
                {
                    counterBalanceAccChart.Series["x"].Points.AddY(cbAccel[i].x);
                    counterBalanceAccChart.Series["y"].Points.AddY(cbAccel[i].y);
                    counterBalanceAccChart.Series["z"].Points.AddY(cbAccel[i].z);


                    if (counterBalanceAccChart.Series["x"].Points.Count > 500)
                    {
                        counterBalanceAccChart.Series["x"].Points.RemoveAt(0);
                        counterBalanceAccChart.Series["y"].Points.RemoveAt(0);
                        counterBalanceAccChart.Series["z"].Points.RemoveAt(0);
                    }
                    counterBalanceAccChart.ChartAreas[0].RecalculateAxesScale();
                }
            }
            else
            {
                // This all only needs to be executed once when it is reached. This if statement
                // blocks these five lines from being executed during every single tick
                if (!lblCbDisabled.Visible)
                {
                    lblCbDisabled.Visible = true;
                    lblCbDisabled.Text = "Counterbalance Accelerometer Disabled";
                    counterBalanceAccChart.Series["x"].Points.Clear();
                    counterBalanceAccChart.Series["y"].Points.Clear();
                    counterBalanceAccChart.Series["z"].Points.Clear();
                }
            }
            ///////////////////////////////////////////////////////////////////////////////

            // Update the Sensor Network status
            lblSNStatus.Text = "Sensor Network Status: " + rtController.RadioTelescope.SensorNetworkServer.Status.ToString();
        }

        private void DiagnosticsForm_Load(object sender, System.EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            double temperature = 0;
        }

        private void btnAddOneTemp_Click(object sender, System.EventArgs e)
        {
            _elevationTemp += 1;
        }

        private void btnAddFiveTemp_Click(object sender, System.EventArgs e)
        {
            _elevationTemp += 5;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            _elevationTemp -= 5;
        }

        //private void btnAddXTemp_Click(object sender, System.EventArgs e)
        //{
        //    double tempVal; //temperature value


        //    if (double.TryParse(txtCustTemp.Text, out tempVal))
        //    {
        //        _elevationTemp += tempVal;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void button3_Click(object sender, System.EventArgs e)
        {
           // double tempVal; //temperature value


            //if (double.TryParse(txtCustTemp.Text, out tempVal))
            //{
            //    _elevationTemp -= tempVal;
            //}
            //else
            //{
            //    MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void timer2_Tick(object sender, System.EventArgs e)
        {

        }


        private void button4_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees += 1;

        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees += 5;
        }

        private void btnSubtractOneEncoder_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees -= 1;

        }

        private void btnSubtractFiveEncoder_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees -= 5;
        }

        private void btnAddXEncoder_Click(object sender, System.EventArgs e)
        {
            double encVal; //encoder value


            if (double.TryParse(txtCustEncoderVal.Text, out encVal))
            {
                _azEncoderDegrees += encVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Encoder field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubtractXEncoder_Click(object sender, System.EventArgs e)
        {
            double encVal; //encoder value


            if (double.TryParse(txtCustEncoderVal.Text, out encVal))
            {
                _azEncoderDegrees -= encVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Encoder field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editDiagScriptsButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Edit Scripts Button Clicked");
            int caseSwitch = diagnosticScriptCombo.SelectedIndex;

            switch (caseSwitch)
            {
                case 0:
                    rtController.ExecuteRadioTelescopeControlledStop();
                    rtController.RadioTelescope.PLCDriver.HitAzimuthLeftLimitSwitch();//Change left to CCW
                    //Hit Azimuth Counter-Clockwise Limit Switch (index 0 of control script combo)
                    break;
                case 1:
                    rtController.ExecuteRadioTelescopeControlledStop();
                    rtController.RadioTelescope.PLCDriver.HitAzimuthRightLimitSwitch();
                    //Hit Azimuth Clockwise Limit Switch (index 1 of control script combo)
                    break;
                case 2:
                    rtController.ExecuteRadioTelescopeControlledStop();
                    rtController.RadioTelescope.PLCDriver.HitElevationLowerLimitSwitch();
                    //Elevation Lower Limit Switch (index 2 of control script combo)
                    break;
                case 3:
                    rtController.ExecuteRadioTelescopeControlledStop();
                    rtController.RadioTelescope.PLCDriver.HitElevationUpperLimitSwitch();
                    //Elevation Upper Limit Switch (index 3 of control script combo)
                    break;
                case 4:
                    rtController.ExecuteRadioTelescopeControlledStop();
                    rtController.RadioTelescope.PLCDriver.Hit_CW_Hardstop();
                    //Hit Clockwise Hardstop (index 4 of control script combo)
                    break;
                case 5:
                    rtController.ExecuteRadioTelescopeControlledStop();
                    rtController.RadioTelescope.PLCDriver.Hit_CCW_Hardstop();
                    //Hit Counter-Clockwise Hardstop (index 4 of control script combo)
                    break;
                default:

                    //Script cannot be run
                    break;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string filename = Directory.GetCurrentDirectory() + "\\" + "UIDoc.pdf";
            if (File.Exists(filename))
                System.Diagnostics.Process.Start(filename);
        }


        private void ElevationProximityOverideButton1_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideElevatProx0)
            {
                ElivationLimitSwitch0.Text = "OVERRIDING";
                ElivationLimitSwitch0.BackColor = System.Drawing.Color.Red;
                rtController.setOverride("elevation proximity (1)", true);
            }
            else if (rtController.overrides.overrideElevatProx0)
            {
                ElivationLimitSwitch0.Text = "ENABLED";
                ElivationLimitSwitch0.BackColor = System.Drawing.Color.LimeGreen;
                rtController.setOverride("elevation proximity (1)", false);
            }
        }

        private void ElevationProximityOverideButton2_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideElevatProx90)
            {
                ElevationLimitSwitch90.Text = "OVERRIDING";
                ElevationLimitSwitch90.BackColor = System.Drawing.Color.Red;
                rtController.setOverride("elevation proximity (2)", true);
            }
            else
            {
                ElevationLimitSwitch90.Text = "ENABLED";
                ElevationLimitSwitch90.BackColor = System.Drawing.Color.LimeGreen;
                rtController.setOverride("elevation proximity (2)", false);
            }
        }

        private void diagnosticScriptCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diagnosticScriptCombo.SelectedIndex >= 0)
            {
                runDiagScriptsButton.Enabled = true;
                runDiagScriptsButton.BackColor = System.Drawing.Color.LimeGreen;
            }
        }

        private void warningLabel_Click(object sender, EventArgs e)
        {

        }

        /** Conversion from fahrenheit to celsius (Currently not being used) 
            if(celOrFar)
            {
                elevationTemperature = (elevationTemperature - 32) * (5.0 / 9);
                azimuthTemperature = (azimuthTemperature - 32) * (5.0 / 9);
            }**/

        private void celTempConvert_Click(object sender, EventArgs e)
        {

            

            if (farenheit == true)
            {
                farenheit = false;
                celTempConvert.BackColor = System.Drawing.Color.LimeGreen;
                farTempConvert.BackColor = System.Drawing.Color.DarkGray;
                
            }
        }

        private void farTempConvert_Click(object sender, EventArgs e)
        {
          
            

            if (farenheit == false)
            {
                farenheit = true;
                celTempConvert.BackColor = System.Drawing.Color.DarkGray;
                farTempConvert.BackColor = System.Drawing.Color.LimeGreen;
            
            }
        }

        private void WSOverride_Click(object sender, EventArgs e)
        {
            if (!mainF.getWSOverride())
            {
                WSOverride.Text = "OVERRIDING";
                WSOverride.BackColor = System.Drawing.Color.Red;
                mainF.setWSOverride(true);

                // We are only calling this to send the push notification and email, it does not actually set the override
                rtController.setOverride("weather station", true);
            }
            else
            {
                WSOverride.Text = "ENABLED";
                WSOverride.BackColor = System.Drawing.Color.LimeGreen;
                mainF.setWSOverride(false);

                // We are only calling this to send the push notification and email, it does not actually set the override
                rtController.setOverride("weather station", false);
            }
        }

        private void MGOverride_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideGate)
            {
                MGOverride.Text = "OVERRIDING";
                MGOverride.BackColor = System.Drawing.Color.Red;
                rtController.setOverride("main gate", true);
            }
            else if (rtController.overrides.overrideGate)
            {
                MGOverride.Text = "ENABLED";
                MGOverride.BackColor = System.Drawing.Color.LimeGreen;
                rtController.setOverride("main gate", false);
            }
        }

        private void lblElEncoderDegrees_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void AzMotTempSensOverride_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideAzimuthMotTemp)
            {
                AzMotTempSensOverride.Text = "OVERRIDING";
                AzMotTempSensOverride.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("azimuth motor temperature", true);
            }
            else if (rtController.overrides.overrideAzimuthMotTemp)
            {
                AzMotTempSensOverride.Text = "ENABLED";
                AzMotTempSensOverride.BackColor = System.Drawing.Color.LimeGreen;

                rtController.setOverride("azimuth motor temperature", false);
            }
        }

        private void ElMotTempSensOverride_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideElevatMotTemp)
            {
                ElMotTempSensOverride.Text = "OVERRIDING";
                ElMotTempSensOverride.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("elevation motor temperature", true);
            }
            else if (rtController.overrides.overrideElevatMotTemp)
            {
                ElMotTempSensOverride.Text = "ENABLED";
                ElMotTempSensOverride.BackColor = System.Drawing.Color.LimeGreen;

                rtController.setOverride("elevation motor temperature", false);
            }
        }

        private void buttonWS_Click(object sender, EventArgs e)
        {
            // create a override by the control room computer
            controlRoom.RTControllerManagementThreads[0].ActiveOverrides.Add(new Override(SensorItemEnum.WIND, "Control Room Computer"));
            controlRoom.RTControllerManagementThreads[0].checkCurrentSensorAndOverrideStatus();
          
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        // Getter for RadioTelescopeController
        public RadioTelescopeController getRTController()
        {
            return rtController;
        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {
        }

        // Runs a check on the override variables, and if there is a change, updates the buttons appropriately
        private void checkOverrideVars(object sender, DoWorkEventArgs e)
        {
            // Current overrides
            bool currMain = rtController.overrides.overrideGate;
            bool currWS = controlRoom.weatherStationOverride;
            bool currAZ = rtController.overrides.overrideAzimuthMotTemp;
            bool currEL = rtController.overrides.overrideElevatMotTemp;
            bool currElProx0 = rtController.overrides.overrideElevatProx0;
            bool currElProx90 = rtController.overrides.overrideElevatProx90;
            bool currAzimuthAbsEncoder = rtController.overrides.overrideAzimuthAbsEncoder;
            bool currElevationAbsEncoder = rtController.overrides.overrideElevationAbsEncoder;
            bool currAzimuthAccelerometer = rtController.overrides.overrideAzimuthAccelerometer;
            bool currElevationAccelerometer = rtController.overrides.overrideElevationAccelerometer;
            bool currCounterbalanceAccelerometer = rtController.overrides.overrideCounterbalanceAccelerometer;

            bool newMain, newWS, newAZ, newEL, newElProx0, newElProx90, 
                newAzimuthAbsEncoder, newElevationAbsEncoder, newAzimuthAccelerometer, newElevationAccelerometer, newCounterbalanceAccelerometer;


            while (true)
            {
                newMain = rtController.overrides.overrideGate;
                newWS = controlRoom.weatherStationOverride;
                newAZ = rtController.overrides.overrideAzimuthMotTemp;
                newEL = rtController.overrides.overrideElevatMotTemp;
                newElProx0 = rtController.overrides.overrideElevatProx0;
                newElProx90 = rtController.overrides.overrideElevatProx90;
                newAzimuthAbsEncoder = rtController.overrides.overrideAzimuthAbsEncoder;
                newElevationAbsEncoder = rtController.overrides.overrideElevationAbsEncoder;
                newAzimuthAccelerometer = rtController.overrides.overrideAzimuthAccelerometer;
                newElevationAccelerometer = rtController.overrides.overrideElevationAccelerometer;
                newCounterbalanceAccelerometer = rtController.overrides.overrideCounterbalanceAccelerometer;

                if (currWS != newWS || 
                    currMain != newMain || 
                    currAZ != newAZ || 
                    currEL != newEL ||
                    currElProx0 != newElProx0 ||
                    currElProx90 != newElProx90 ||
                    currAzimuthAbsEncoder != newAzimuthAbsEncoder ||
                    currElevationAbsEncoder != newElevationAbsEncoder ||
                    currAzimuthAccelerometer != newAzimuthAccelerometer ||
                    currElevationAccelerometer != newElevationAccelerometer ||
                    currCounterbalanceAccelerometer != newCounterbalanceAccelerometer)
                {
                    currMain = newMain;
                    currWS = newWS;
                    currAZ = newAZ;
                    currEL = newEL;
                    currElProx0 = newElProx0;
                    currElProx90 = newElProx90;
                    currAzimuthAbsEncoder = newAzimuthAbsEncoder;
                    currElevationAbsEncoder = newElevationAbsEncoder;
                    currAzimuthAccelerometer = newAzimuthAccelerometer;
                    currElevationAccelerometer = newElevationAccelerometer;
                    currCounterbalanceAccelerometer = newCounterbalanceAccelerometer;

                    if (IsHandleCreated)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            updateButtons(currMain, currWS, currAZ, currEL, currElProx0, currElProx90, 
                                currAzimuthAbsEncoder, currElevationAbsEncoder, currAzimuthAccelerometer, currElevationAccelerometer, currCounterbalanceAccelerometer);
                        });
                    }

                }
                Thread.Sleep(1000);
            }
        }

        // Loads the override buttons
        public void updateButtons(bool currMain, bool currWS, bool currAZ, bool currEL, bool currElProx0, bool currElProx90,
            bool azimuthAbsEncoder, bool elevationAbsEncoder, bool azimuthAccelerometer, bool elevationAccelerometer, bool counterbalanceAccelerometer)
        {
            // Weather Station Override
            if(currWS)
            {
                WSOverride.Text = "OVERRIDING";
                WSOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                WSOverride.Text = "ENABLED";
                WSOverride.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Main Gate Override
            if(currMain)
            {
                MGOverride.Text = "OVERRIDING";
                MGOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                MGOverride.Text = "ENABLED";
                MGOverride.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Azimuth Motor Override
            if(currAZ)
            {
                AzMotTempSensOverride.Text = "OVERRIDING";
                AzMotTempSensOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                AzMotTempSensOverride.Text = "ENABLED";
                AzMotTempSensOverride.BackColor = System.Drawing.Color.LimeGreen;
            }
            
            // Elevation Motor Override
            if(currEL)
            {
                ElMotTempSensOverride.Text = "OVERRIDING";
                ElMotTempSensOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                ElMotTempSensOverride.Text = "ENABLED";
                ElMotTempSensOverride.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Elevation Limit Switch 0 Degrees Override
            if(currElProx0)
            {
                ElivationLimitSwitch0.Text = "OVERRIDING";
                ElivationLimitSwitch0.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                ElivationLimitSwitch0.Text = "ENABLED";
                ElivationLimitSwitch0.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Elevation Limit Switch 90 Degrees Override
            if (currElProx90)
            {
                ElevationLimitSwitch90.Text = "OVERRIDING";
                ElevationLimitSwitch90.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                ElevationLimitSwitch90.Text = "ENABLED";
                ElevationLimitSwitch90.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Azimuth ABS Encoder Override
            if (azimuthAbsEncoder)
            {
                btnAzimuthAbsoluteEncoder.Text = "OVERRIDING";
                btnAzimuthAbsoluteEncoder.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                btnAzimuthAbsoluteEncoder.Text = "ENABLED";
                btnAzimuthAbsoluteEncoder.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Elevation ABS Encoder Override
            if (elevationAbsEncoder)
            {
                btnElevationAbsoluteEncoder.Text = "OVERRIDING";
                btnElevationAbsoluteEncoder.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                btnElevationAbsoluteEncoder.Text = "ENABLED";
                btnElevationAbsoluteEncoder.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Azimuth Accelerometer Override
            if (azimuthAccelerometer)
            {
                btnAzimuthMotorAccelerometerOverride.Text = "OVERRIDING";
                btnAzimuthMotorAccelerometerOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                btnAzimuthMotorAccelerometerOverride.Text = "ENABLED";
                btnAzimuthMotorAccelerometerOverride.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Elevation Accelerometer Override
            if (elevationAccelerometer)
            {
                btnElevationMotorAccelerometerOverride.Text = "OVERRIDING";
                btnElevationMotorAccelerometerOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                btnElevationMotorAccelerometerOverride.Text = "ENABLED";
                btnElevationMotorAccelerometerOverride.BackColor = System.Drawing.Color.LimeGreen;
            }

            // Counterbalce Accelerometer Override
            if (counterbalanceAccelerometer)
            {
                btnCounterbalanceMotorAccelerometerOverride.Text = "OVERRIDING";
                btnCounterbalanceMotorAccelerometerOverride.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                btnCounterbalanceMotorAccelerometerOverride.Text = "ENABLED";
                btnCounterbalanceMotorAccelerometerOverride.BackColor = System.Drawing.Color.LimeGreen;
            }
        }

        private void btnResetMcuErrors_Click(object sender, EventArgs e)
        {
            rtController.RadioTelescope.PLCDriver.ResetMCUErrors();
        }

        private void lblAzLimStatus1_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }


        private void btnAzimuthAbsoluteEncoder_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideAzimuthAbsEncoder)
            {
                btnAzimuthAbsoluteEncoder.Text = "OVERRIDING";
                btnAzimuthAbsoluteEncoder.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("azimuth absolute encoder", true);
            }
            else if (rtController.overrides.overrideAzimuthAbsEncoder)
            {
                btnAzimuthAbsoluteEncoder.Text = "ENABLED";
                btnAzimuthAbsoluteEncoder.BackColor = System.Drawing.Color.LimeGreen;

                rtController.setOverride("azimuth absolute encoder", false);
            }
        }

        private void btnElevationAbsoluteEncoder_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideElevationAbsEncoder)
            {
                btnElevationAbsoluteEncoder.Text = "OVERRIDING";
                btnElevationAbsoluteEncoder.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("elevation absolute encoder", true);
            }
            else if (rtController.overrides.overrideElevationAbsEncoder)
            {
                btnElevationAbsoluteEncoder.Text = "ENABLED";
                btnElevationAbsoluteEncoder.BackColor = System.Drawing.Color.LimeGreen;

                rtController.setOverride("elevation absolute encoder", false);
            }
        }

        private void btnAzimuthMotorAccelerometerOverride_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideAzimuthAccelerometer)
            {
                btnAzimuthMotorAccelerometerOverride.Text = "OVERRIDING";
                btnAzimuthMotorAccelerometerOverride.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("azimuth motor accelerometer", true);
            }
            else if (rtController.overrides.overrideAzimuthAccelerometer)
            {
                btnAzimuthMotorAccelerometerOverride.Text = "ENABLED";
                btnAzimuthMotorAccelerometerOverride.BackColor = System.Drawing.Color.LimeGreen;

                rtController.setOverride("azimuth motor accelerometer", false);
            }
        }

        private void btnElevationMotorAccelerometerOverride_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideElevationAccelerometer)
            {
                btnElevationMotorAccelerometerOverride.Text = "OVERRIDING";
                btnElevationMotorAccelerometerOverride.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("elevation motor accelerometer", true);
            }
            else if (rtController.overrides.overrideElevationAccelerometer)
            {
                btnElevationMotorAccelerometerOverride.Text = "ENABLED";
                btnElevationMotorAccelerometerOverride.BackColor = System.Drawing.Color.LimeGreen;

                rtController.setOverride("elevation motor accelerometer", false);
            }
        }

        private void btnCounterbalanceMotorAccelerometerOverride_Click(object sender, EventArgs e)
        {
            if (!rtController.overrides.overrideCounterbalanceAccelerometer)
            {
                btnCounterbalanceMotorAccelerometerOverride.Text = "OVERRIDING";
                btnCounterbalanceMotorAccelerometerOverride.BackColor = System.Drawing.Color.Red;

                rtController.setOverride("counterbalance accelerometer", true);
            }
            else if (rtController.overrides.overrideCounterbalanceAccelerometer)
            {
                btnCounterbalanceMotorAccelerometerOverride.Text = "ENABLED";
                btnCounterbalanceMotorAccelerometerOverride.BackColor = System.Drawing.Color.LimeGreen;
                
                rtController.setOverride("counterbalance accelerometer", false);
            }
        }

        private void UpdateSensorInitiliazation_Click(object sender, EventArgs e)
        {
            // First set all the checkboxes equal to the sensor network config
            SensorNetworkConfig.AzimuthTemp1Init = AzimuthTemperature1.Checked;
            SensorNetworkConfig.AzimuthTemp2Init = AzimuthTemperature2.Checked;
            SensorNetworkConfig.ElevationTemp1Init = ElevationTemperature1.Checked;
            SensorNetworkConfig.ElevationTemp2Init = ElevationTemperature2.Checked;
            SensorNetworkConfig.AzimuthAccelerometerInit = AzimuthAccelerometer.Checked;
            SensorNetworkConfig.ElevationAccelerometerInit = ElevationAccelerometer.Checked;
            SensorNetworkConfig.CounterbalanceAccelerometerInit = CounterbalanceAccelerometer.Checked;
            SensorNetworkConfig.ElevationEncoderInit = ElevationEncoder.Checked;
            SensorNetworkConfig.AzimuthEncoderInit = AzimuthEncoder.Checked;

            // Update the config in the DB
            DatabaseOperations.UpdateSensorNetworkConfig(SensorNetworkConfig);
            
            // reboot
            rtController.RadioTelescope.SensorNetworkServer.RebootSensorNetwork();
        }
    }
}
