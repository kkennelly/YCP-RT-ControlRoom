using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.GUI;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using System.Net;
using ControlRoomApplication.Entities.WeatherStation;
using log4net.Appender;
using ControlRoomApplication.Documentation;
using ControlRoomApplication.Validation;
using ControlRoomApplication.GUI.Data;
using ControlRoomApplication.Util;
using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.GUI.DropDownEnums;
using System.Threading.Tasks;

namespace ControlRoomApplication.Main
{
    public partial class MainForm : Form, IAppender
    {
        private static int current_rt_id;
        public List<KeyValuePair<RadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList { get; set; }
        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
        public List<AbstractPLCDriver> ProgramPLCDriverList { get; set; }
        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }
        public ControlRoomController MainControlRoomController { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AbstractWeatherStation lastCreatedProductionWeatherStation = null;

        public bool finalSettings = true;
        public LoggerQueue log = new LoggerQueue();

        // Booleans for user validation of input forms
        public bool MCUIPValid = false;
        public bool MCUPortValid = false;
        public bool PLCPortValid = false;
        public bool WCOMPortValid = false;
        public bool RLPortValid = false;
        public bool SpectraPortValid = false;
        public bool SensorNetworkServerIPBool = false;
        public bool SensorNetworkServerPortBool = false;
        public bool SensorNetworkClientIPBool = false;
        public bool SensorNetworkClientPortBool = false;
        public bool PNEnabled = false; 
        
        // form
        RTControlFormData formData;

        RadioTelescopeControllerManagementThread ManagementThread; 

        enum TempSensorType
        {
            Production,
            Simulation
        }

        enum MCUType
        {
            Production,
            Simulation
        }

        enum PLCType
        {
            Production,
            Scale,
            Simulation,
            Test

        }

        enum MicrocontrollerType
        {
            Production,
            Simulation
        }

        /// <summary>
        /// Constructor for the main GUI form. Initializes the GUI form by calling the
        /// initialization method in another partial class. Initializes the datagridview
        /// and the lists that track telescope configurations.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] v4_list = new IPAddress[ipHostInfo.AddressList.Length / 2];
            System.Array.Copy(ipHostInfo.AddressList, ipHostInfo.AddressList.Length / 2, v4_list, 0, ipHostInfo.AddressList.Length / 2);
            this.LocalIPCombo.Items.AddRange(v4_list);
            
            logger.Info(Utilities.GetTimeStamp() + ": <--------------- Control Room Application Started --------------->");
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "PLC IP";
            dataGridView1.Columns[2].HeaderText = "PLC Port";
            dataGridView1.Columns[3].HeaderText = "MCU Port";
            dataGridView1.Columns[4].HeaderText = "WS Port";
            //dataGridView1.Columns[3].HeaderText = "MCU Port";

            AbstractRTDriverPairList = new List<KeyValuePair<RadioTelescope, AbstractPLCDriver>>();
            ProgramRTControllerList = new List<RadioTelescopeController>();
            ProgramPLCDriverList = new List<AbstractPLCDriver>();
            ProgramControlRoomControllerList = new List<ControlRoomController>();
            current_rt_id = 0;

            // Initialize Button Settings 
            startRTGroupbox.BackColor = System.Drawing.Color.DarkGray;
            acceptSettings.Enabled = false;
            startButton.BackColor = System.Drawing.Color.Gainsboro;
            startButton.Enabled = false;
            shutdownButton.BackColor = System.Drawing.Color.Gainsboro;
            shutdownButton.Enabled = false;
            loopBackBox.Enabled = true;
            btnApptControl.BackColor = System.Drawing.Color.DarkGray;
            btnApptControl.Enabled = false; 

            comboSensorNetworkBox.SelectedIndex = (int)SensorNetworkDropdown.SimulatedSensorNetwork;
            comboSpectraCyberBox.SelectedIndex = (int)SpectraCyberDropdown.SimulatedSpectraCyber;
            comboWeatherStationBox.SelectedIndex = (int)WeatherStationDropdown.SimulatedWeatherStation;
            comboPLCType.SelectedIndex = (int)PLCDropdown.SimulatedPLC;
            LocalIPCombo.SelectedIndex = 0;

            sensorNetworkServerIPAddress.Text = "IP Address";
            // initialize formData struct
            formData = new RTControlFormData();
            sensorNetworkServerPort.Text = "Port";
            sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Gray;
            sensorNetworkServerPort.ForeColor = System.Drawing.Color.Gray;
            // initialize formData struct
            formData = new RTControlFormData();

            sensorNetworkClientIPAddress.Text = "IP Address";
            sensorNetworkClientPort.Text = "Port";
            sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Gray;
            sensorNetworkClientPort.ForeColor = System.Drawing.Color.Gray;

            txtSpectraPort.Text = "COM";
            txtSpectraPort.ForeColor = System.Drawing.Color.Gray;
            txtWSCOMPort.Text = "COM";
            txtWSCOMPort.ForeColor = System.Drawing.Color.Gray;
            txtRemoteListenerCOMPort.Text = "COM";
            txtRemoteListenerCOMPort.ForeColor = System.Drawing.Color.Gray;

            logger.Info(Utilities.GetTimeStamp() + ": MainForm Initalized");
        }

        public void DoAppend(log4net.Core.LoggingEvent loggingEvent)
        {
            log.loggerQueue += loggingEvent.Level.Name + ": " + loggingEvent.MessageObject.ToString() + Environment.NewLine;
        }

        /// <summary>
        /// Eventhandler for the start button on the main GUI form. This method creates and
        /// initializes the configuration that is specified on the main GUI form if the correct
        /// fields are populated.
        /// </summary>
        /// <param name="sender"> Object specifying the sender of this Event. </param>
        /// <param name="e"> The eventargs from the button being clicked on the GUI. </param>
        private void startButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Start Telescope Button Clicked");

            // This will tell us whether or not the RT is safe to start.
            // It may not be safe to start if, for example, there are
            // validation errors, or no Telescope is found in the DB
            bool runRt = false;

            // retrirve contents of JSON file
            RadioTelescopeConfig RTConfig = RadioTelescopeConfig.DeserializeRTConfig();

            // this will be null if an error occurs in parsing JSON from the file, if the expected types do not match, i.e. a string
            // was given where an integer was expected, or if any of the inputs were null.
            if (RTConfig == null)
            {
                DialogResult result = MessageBox.Show("An error occured while parsing the RTConfig JSON file. Would you like to recreate the JSON " +
                    "file?", "Error Parsing JSON", MessageBoxButtons.YesNo);
                // If yes, recreate the file and remind the user to set the ID and change the flag back to false
                if (result == DialogResult.Yes)
                {
                    RadioTelescopeConfig.CreateAndWriteToNewJSONFile(RadioTelescopeConfig.DEFAULT_JSON_CONTENTS);
                    MessageBox.Show("JSON file successfully recreated! Do not forget to specify the ID of telescope you want to run inside the file, " +
                        "and set the newTelescope flag to false.",
                        "JSON File Sucessfully Created", MessageBoxButtons.OK);
                }
            }
            // retrieve RT by specified ID, if newTelescope flag set to false (meaning the user is trying to run a pre-existing telescope)
            else if (!RTConfig.newTelescope)
            {
                RadioTelescope RT = DatabaseOperations.FetchRadioTelescopeByID(RTConfig.telescopeID);
                if (RT == null)
                {
                    DialogResult result = MessageBox.Show("The ID of " + RTConfig.telescopeID +
                        " was not found in the database. Would you like to create a new one?", "No Telescope found", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        runRt = true;
                    }
                }
                // we cannot run a second telescope if the selected one is already running.
                else if (RT.online == 1)
                {
                    DialogResult result = MessageBox.Show(
                        $"Telescope {RT.Id} is already in use, or the program crashed. Would you like to override this check and run the telescope anyway?",
                        "Telescope in use",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        runRt = true;
                        current_rt_id = RTConfig.telescopeID;
                    }
                }
                // else the telescope entered by the user is valid and is currently not running. Start it up
                else
                {
                    Console.WriteLine("The Selected RT with id " + RTConfig.telescopeID + " was not null, and is not running. Starting telescope " + RTConfig.telescopeID);
                    current_rt_id = RTConfig.telescopeID;
                    runRt = true;
                }

            }
            // else the user is trying to create a new telescope. Discard the RadioTelescope ID from the file, and ask
            // the user to confirm they would like to create a new one.
            else
            {
                DialogResult result = MessageBox.Show(
                    "The new telescope flag was set to true in the RTConfig File. Please confirm you like to create " +
                    "a new telescope, or go back and input the ID of an existing telescope in the database.",
                    "New Telescope Flag Set to True",
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    runRt = true;
                }
            }

            /** MCU IP VALIDATION **/

            // We want the IP to be marked as alive if we are running the simulation
            // Otherwise, we call the validation
            bool mcuIpAlive = true;
            if (comboPLCType.SelectedIndex != 2)
            {
                mcuIpAlive = Validator.ServerRunningOnIp(txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text));
            }
            else if (!txtPLCIP.Text.Contains("127.0.0."))
            {
                MessageBox.Show(
                    $"{txtPLCIP.Text} is not a valid IP address for the simulation MCU to run on.\n\n" +
                    $"Please make sure that the IP is a local host address (Ex: 127.0.0.xxx).",

                    "Invalid Simulation MCU IP"
                );
                runRt = false;
            }

            // Verify that the MCU's IP address is alive
            // This is skipped if we are simulating, since the server is run internally
            if (!mcuIpAlive)
            {
                MessageBox.Show(
                    $"The MCU was not found on {txtPLCIP.Text}:{txtMcuCOMPort.Text}, or the system is still booting up.\n\n" +
                    $"If this problem persists, please power-cycle the PLC and MCU and verify all connections are firmly in place.",

                    "MCU Not Found"
                );
                runRt = false;
            }


            if (runRt)
            {
                shutdownButton.BackColor = System.Drawing.Color.Red;
                shutdownButton.Enabled = true;
                simulationSettingsGroupbox.BackColor = System.Drawing.Color.Gray;
                comboSensorNetworkBox.Enabled = true;
                comboWeatherStationBox.Enabled = true;
                comboSpectraCyberBox.Enabled = true;
                comboPLCType.Enabled = true;
                LocalIPCombo.Enabled = true;

                portGroupbox.BackColor = System.Drawing.Color.Gray;
                txtPLCIP.Enabled = true;
                txtMcuCOMPort.Enabled = true;
                txtWSCOMPort.Enabled = true;
                txtRemoteListenerCOMPort.Enabled = true;
                txtPLCPort.Enabled = true;
                txtSpectraPort.Enabled = true;

                btnApptControl.BackColor = System.Drawing.Color.LightGray;
                btnApptControl.Enabled = true;

                if (txtPLCPort.Text != null
                    && txtPLCIP.Text != null
                    && comboSpectraCyberBox.SelectedIndex > -1)
                {

                    // If the main control room controller hasn't been initialized, initialize it.
                    if (MainControlRoomController == null)
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Initializing ControlRoomController");

                        int rlPort=0;
                        try
                        {
                            rlPort = Int32.Parse(txtRemoteListenerCOMPort.Text);
                        }catch(Exception ex)
                        {
                            logger.Error("There was an error parsing the Remote Listener port to an integer"+ex);

                        }
                        if (lastCreatedProductionWeatherStation == null)
                            MainControlRoomController = new ControlRoomController(new ControlRoom(BuildWeatherStation(), rlPort));
                        else
                            MainControlRoomController = new ControlRoomController(new ControlRoom(lastCreatedProductionWeatherStation, rlPort));
                    }

                    bool isSensorNetworkServerSimulated = false;
                    if (comboSensorNetworkBox.SelectedIndex == (int)SensorNetworkDropdown.SimulatedSensorNetwork)
                    {
                        isSensorNetworkServerSimulated = true;
                    }

                    //current_rt_id++;
                    AbstractPLCDriver APLCDriver = BuildPLCDriver();
                    SensorNetworkServer sensorNetworkServer = new SensorNetworkServer(IPAddress.Parse(sensorNetworkServerIPAddress.Text), int.Parse(sensorNetworkServerPort.Text),
                    sensorNetworkClientIPAddress.Text, int.Parse(sensorNetworkClientPort.Text), RTConfig.telescopeID, isSensorNetworkServerSimulated);

                    sensorNetworkServer.StartSensorMonitoringRoutine();
                    RadioTelescope ARadioTelescope = BuildRT(APLCDriver, sensorNetworkServer);
                    ARadioTelescope.WeatherStation = MainControlRoomController.ControlRoom.WeatherStation;

                    // Add the RT/PLC driver pair and the RT controller to their respective lists
                    AbstractRTDriverPairList.Add(new KeyValuePair<RadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
                    ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[AbstractRTDriverPairList.Count - 1].Key));
                    ProgramPLCDriverList.Add(APLCDriver);

                    // Start plc server and attempt to connect to it.
                    logger.Info(Utilities.GetTimeStamp() + ": Starting plc server and attempting to connect to it");
                    ProgramPLCDriverList[ProgramPLCDriverList.Count - 1].StartAsyncAcceptingClients();
                    //ProgramRTControllerList[current_rt_id - 1].RadioTelescope.PLCClient.ConnectToServer();//////####################################

                    logger.Info(Utilities.GetTimeStamp() + ": Adding RadioTelescope Controller");
                    MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);
                    ARadioTelescope.SetParent(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);

                    // linking radio telescope controller to tcp listener
                    MainControlRoomController.ControlRoom.mobileControlServer.rtController = ARadioTelescope.GetParent();

                    logger.Info(Utilities.GetTimeStamp() + ": Starting Spectra Cyber Controller");
                    ARadioTelescope.SpectraCyberController.BringUp();

                    logger.Info(Utilities.GetTimeStamp() + ": Setting Default Values for Spectra Cyber Controller");
                    ARadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
                    ARadioTelescope.SpectraCyberController.SetSpectralIntegrationTime(SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN);
                    ARadioTelescope.SpectraCyberController.SetContinuumOffsetVoltage(2.0);

                    // Start RT controller's threaded management
                    logger.Info(Utilities.GetTimeStamp() + ": Starting RT controller's threaded management");
                    ManagementThread = MainControlRoomController.ControlRoom.RTControllerManagementThreads[MainControlRoomController.ControlRoom.RTControllerManagementThreads.Count - 1];

                    // add telescope to database
                    //DatabaseOperations.AddRadioTelescope(ARadioTelescope);

                    int RT_ID = ManagementThread.RadioTelescopeID;
                    List<Appointment> AllAppointments = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(RT_ID);

                    logger.Info(Utilities.GetTimeStamp() + ": Attempting to queue " + AllAppointments.Count.ToString() + " appointments for RT with ID " + RT_ID.ToString());
                    foreach (Appointment appt in AllAppointments)
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": \t[" + appt.Id + "] " + appt.start_time.ToString() + " -> " + appt.end_time.ToString());
                    }

                    if (ManagementThread.Start())
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Successfully started RT controller management thread [" + RT_ID.ToString() + "]");

                        ProgramRTControllerList[ProgramRTControllerList.Count - 1].ConfigureRadioTelescope(.06, .06, 300, 300);
                    }
                    else
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": ERROR starting RT controller management thread [" + RT_ID.ToString() + "]");
                    }

                    AddConfigurationToDataGrid();

                    // Set PLC override bits because they may be different than what's in the database
                    bool gateOvr = ProgramRTControllerList[ProgramRTControllerList.Count - 1].overrides.overrideGate;

                    APLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.GATE_OVERRIDE, Convert.ToUInt16(gateOvr));

                    logger.Info(Utilities.GetTimeStamp() + ": Enabling ManualControl and FreeControl");
                    FreeControl.Enabled = true;

                    Task.Run(() =>
                    {
                        // Occasionally, during the initial connection (because things are being powered up, yet), there may be input errors present on the MCU.
                        // We aren't concerned with these errors, so clearing them is a part of the startup procedure.
                        Thread.Sleep(5000);
                        while (ProgramRTControllerList[ProgramRTControllerList.Count - 1].RadioTelescope.PLCDriver.CheckMCUErrors().Count > 0)
                        {
                            ProgramRTControllerList[ProgramRTControllerList.Count - 1].RadioTelescope.PLCDriver.ResetMCUErrors();
                            Thread.Sleep(3000);
                        }
                    });
                }
            }



        }

        /// <summary>
        /// Adds the configuration specified with the main GUI form to the datagridview.
        /// </summary>
        private void AddConfigurationToDataGrid()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Adding Configuration To DataGrid");
            string[] row = { (current_rt_id).ToString(), txtPLCIP.Text, txtPLCPort.Text, txtMcuCOMPort.Text, txtWSCOMPort.Text };

            dataGridView1.Rows.Add(row);
            dataGridView1.Update();
        }

        /// <summary>
        /// Brings down each telescope instance and exits from the GUI cleanly.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            HandleShutDown(); 
        }

        /// <summary>
        /// Handles shut down routine for Control Room App.
        /// </summary>
        private void HandleShutDown()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Shut Down Telescope Button Clicked");

            // Loop through the list of telescope controllers and call their respective bring down sequences.
            for (int i = 0; i < ProgramRTControllerList.Count; i++)
            {
                if (MainControlRoomController.RemoveRadioTelescopeControllerAt(0, false))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Successfully brought down RT controller at index " + i.ToString());
                }
                else
                {
                    logger.Info(Utilities.GetTimeStamp() + ": ERROR killing RT controller at index " + i.ToString());
                }

                //Turn off Telescope in database
                ProgramRTControllerList[i].RadioTelescope.online = 0;
                ProgramRTControllerList[i].RadioTelescope.SensorNetworkServer.EndSensorMonitoringRoutine();
                DatabaseOperations.UpdateTelescope(ProgramRTControllerList[i].RadioTelescope);

                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringDown();
                ProgramRTControllerList[i].ShutdownRadioTelescope();
            }

            // End logging
            logger.Info(Utilities.GetTimeStamp() + ": <--------------- Control Room Application Terminated --------------->");
            Environment.Exit(0);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            HandleShutDown();
        }

        /// <summary>
        /// Generates a diagnostic form for whichever telescope configuration is chosen
        /// from the GUI.
        /// </summary>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": dataGridView1_CellContent Clicked");
            try
            {
                DiagnosticsForm diagnosticForm = new DiagnosticsForm(MainControlRoomController.ControlRoom, AbstractRTDriverPairList[dataGridView1.CurrentCell.RowIndex].Key.Id, this);
                diagnosticForm.Show();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Builds a radio telescope instance based off of the input from the GUI form.
        /// </summary>
        /// <returns> A radio telescope instance representing the configuration chosen. </returns>
        public RadioTelescope BuildRT(AbstractPLCDriver abstractPLCDriver, SensorNetworkServer sns)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Building RadioTelescope");

            // if this is set to 0, it has not been updated with an existing ID from the database. Therefore, we must create one.
            if (current_rt_id == 0)
            {
                RadioTelescope newRT = new RadioTelescope();
                newRT.Location = new Location(0, 0, 0, "");
                newRT.CalibrationOrientation = new Entities.Orientation(0, 0);
                newRT.CurrentOrientation = new Entities.Orientation(0, 0);

                // This is the TELESCOPE TYPE
                // It is now set to SLIP_RING because we finally removed the hard stops. Isn't that exciting?!
                newRT._TeleType = RadioTelescopeTypeEnum.SLIP_RING;

                DatabaseOperations.AddRadioTelescope(newRT);

                newRT.Id = DatabaseOperations.FetchLastRadioTelescope().Id;

                // Set software stops
                newRT.maxElevationDegrees = MiscellaneousConstants.MAX_SOFTWARE_STOP_EL_DEGREES;
                newRT.minElevationDegrees = MiscellaneousConstants.MIN_SOFTWARE_STOP_EL_DEGREES;

                //Turn telescope on in database 
                newRT.online = 1;
                DatabaseOperations.UpdateTelescope(newRT);

                // These settings are not stored in the database, so they are new every time
                newRT.PLCDriver = abstractPLCDriver;
                newRT.PLCDriver.setTelescopeType(newRT._TeleType);
                newRT.SpectraCyberController = BuildSpectraCyber();
                newRT.SensorNetworkServer = sns;
                logger.Info(Utilities.GetTimeStamp() + ": New RadioTelescope built successfully");

                current_rt_id = newRT.Id;

                // update the JSON config file to reflect the newly created telescope
                RadioTelescopeConfig.SerializeRTConfig(new RadioTelescopeConfig(newRT.Id, false));

                return newRT;
            }
            else
            // else there has been a specified RT instance we are retrieving from the database. Do that and build that specific telescope here
            {
                RadioTelescope existingRT = DatabaseOperations.FetchRadioTelescopeByID(current_rt_id);

                // Turn on telescope in database
                existingRT.online = 1;
                DatabaseOperations.UpdateTelescope(existingRT);

                // These settings are not stored in the database, so they are new every time
                existingRT.PLCDriver = abstractPLCDriver;
                existingRT.PLCDriver.setTelescopeType(existingRT._TeleType);
                existingRT.SpectraCyberController = BuildSpectraCyber();
                existingRT.SensorNetworkServer = sns;
                logger.Info(Utilities.GetTimeStamp() + ": Existing RadioTelescope with ID " + current_rt_id + " retrieved and built successfully");

                return existingRT;

            }

        }

        /// <summary>
        /// Builds a spectracyber instance based off of the input from the GUI.
        /// </summary>
        /// <returns> A spectracyber instance based off of the configuration specified by the GUI. </returns>
        public AbstractSpectraCyberController BuildSpectraCyber()
        {
            switch (comboSpectraCyberBox.SelectedIndex)
            {
                case 0:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SpectraCyber");
                    return new SpectraCyberController(new SpectraCyber("COM" + txtSpectraPort.Text));

                case 1:
                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SpectraCyberSimulator");
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            }
        }

        /// <summary>
        /// Builds a PLC driver based off of the input from the GUI.
        /// </summary>
        /// <returns> A plc driver based off of the configuration specified by the GUI. </returns>
        public AbstractPLCDriver BuildPLCDriver()
        {
            switch (comboPLCType.SelectedIndex)
            {
                case 0:
                    logger.Info(Utilities.GetTimeStamp() + ": Building ProductionPLCDriver");
                    return new ProductionPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text));

                case 1:
                    logger.Info(Utilities.GetTimeStamp() + ": Building ScaleModelPLCDriver");
                    return new ScaleModelPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text));

                case 3:
                    logger.Info(Utilities.GetTimeStamp() + ": Building TestPLCDriver");
                    return new TestPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text), false);
                case 2:
                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SimulationPLCDriver");
                    return new SimulationPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text), false, false);
            }
        }

        /// <summary>
        /// Build a weather station based off of the input from the GUI form.
        /// </summary>
        /// <returns> A weather station instance based off of the configuration specified. </returns>
        public AbstractWeatherStation BuildWeatherStation()
        {
            switch (comboWeatherStationBox.SelectedIndex)
            {
                case 0:
                    logger.Info(Utilities.GetTimeStamp() + ": Building ProductionWeatherStation");
                    try
                    {
                    return new WeatherStation(1000, int.Parse(txtWSCOMPort.Text));
                    }catch(Exception e)
                    {
                        return null;
                    }
                    
                case 2:
                    logger.Error(Utilities.GetTimeStamp() + ": The test weather station is not yet supported.");
                    throw new NotImplementedException("The test weather station is not yet supported.");

                case 1:
                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SimulationWeatherStation");
                    return new SimulationWeatherStation(1000);
            }
        }


        /// <summary>
        /// Generates a free control form that allows free control access to a radio telescope
        /// instance through the generated form.
        /// </summary>
        private void FreeControl_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Free Control Button Clicked");
            int rtIDforControl = AbstractRTDriverPairList[dataGridView1.CurrentCell.RowIndex].Key.Id;
            FreeControlForm freeControlWindow = new FreeControlForm(MainControlRoomController.ControlRoom, rtIDforControl, formData);
            // Create free control thread
            Thread FreeControlThread = new Thread(() => freeControlWindow.ShowDialog())
            {
                Name = "Free Control Thread"
            };
            FreeControlThread.Start();
        }

        /// <summary>
        /// Generates a manual control form that allows manual control access to a radio telescope
        /// instance through the generated form.
        /// </summary>
        //private void ManualControl_Click(object sender, EventArgs e)
        //{
        //    logger.Info(Utilities.GetTimeStamp() + ": Manual Control Button Clicked");
        //    ProgramRTControllerList[current_rt_id - 1].ConfigureRadioTelescope( .1 , .1 , 0 , 0 );
        //    ManualControlForm manualControlWindow = new ManualControlForm(MainControlRoomController.ControlRoom, current_rt_id);
        //    // Create free control thread
        //    Thread ManualControlThread = new Thread(() => manualControlWindow.ShowDialog())
        //    {
        //        Name = "Manual Control Thread"
        //    };
        //    ManualControlThread.Start();
        //}

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void loopBackBox_CheckedChanged(object sender, EventArgs e)
        {

            if (loopBackBox.Checked)
            {
                ProdcheckBox.Checked = false;
                this.txtWSCOMPort.Text = "222"; //default WS COM port # is 221
                this.txtSpectraPort.Text = "777";
                this.txtRemoteListenerCOMPort.Text = "80";
                this.txtMcuCOMPort.Text = ((int)(8083 + ProgramPLCDriverList.Count * 3)).ToString(); ; //default MCU Port
                this.txtPLCIP.Text = "127.0.0.1";//default IP address

                this.sensorNetworkServerIPAddress.Text = "127.0.0.1";
                this.sensorNetworkClientIPAddress.Text = "127.0.0.1";
                this.sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Black;

                this.sensorNetworkServerPort.Text = "1600";
                this.sensorNetworkClientPort.Text = "1680";
                this.sensorNetworkServerPort.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientPort.ForeColor = System.Drawing.Color.Black;

                comboSensorNetworkBox.SelectedIndex = (int)SensorNetworkDropdown.SimulatedSensorNetwork;
                comboSpectraCyberBox.SelectedIndex = (int)SpectraCyberDropdown.SimulatedSpectraCyber;
                comboWeatherStationBox.SelectedIndex = (int)WeatherStationDropdown.SimulatedWeatherStation;
                comboPLCType.SelectedIndex = (int)PLCType.Simulation;

                if (LocalIPCombo.FindStringExact("127.0.0.1") == -1)
                {
                    this.LocalIPCombo.Items.Add(IPAddress.Parse("127.0.0.1"));
                }
                this.LocalIPCombo.SelectedIndex = LocalIPCombo.FindStringExact("127.0.0.1");
            }
            this.txtPLCPort.Text = ((int)(8082 + ProgramPLCDriverList.Count * 3)).ToString();

        }

        private void ProdcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ProdcheckBox.Checked)
            {
                loopBackBox.Checked = false;
                this.txtWSCOMPort.Text = "222"; //default WS COM port # is 221
                this.txtSpectraPort.Text = "8"; // Value used to be 777, but the USB's port is currently set to 8
                this.txtMcuCOMPort.Text = "502"; //default MCU Port
                this.txtPLCIP.Text = "192.168.0.50";//default IP address
                this.txtRemoteListenerCOMPort.Text = "80";
                this.comboPLCType.SelectedIndex = (int)PLCDropdown.ProductionPLC;
                if (LocalIPCombo.FindStringExact("192.168.0.70") == -1)
                {
                    this.LocalIPCombo.Items.Add(IPAddress.Parse("192.168.0.70"));
                }
                this.LocalIPCombo.SelectedIndex = LocalIPCombo.FindStringExact("192.168.0.70");
                this.txtPLCPort.Text = "502";
                comboSensorNetworkBox.SelectedIndex = (int)SensorNetworkDropdown.ProductionSensorNetwork;
                comboSpectraCyberBox.SelectedIndex = (int)SpectraCyberDropdown.ProductionSpectraCyber;
                comboWeatherStationBox.SelectedIndex = (int)WeatherStationDropdown.ProductionWeatherStation;

                // SensorNetwork and Server IP/Ports

                this.sensorNetworkServerIPAddress.Text = "192.168.0.10";
                this.sensorNetworkClientIPAddress.Text = "192.168.0.197";
                this.sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Black;

                this.sensorNetworkServerPort.Text = "1600";
                this.sensorNetworkClientPort.Text = "1680";
                this.sensorNetworkServerPort.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void comboSensorNetworkBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboSensorNetworkBox.SelectedIndex == (int)SensorNetworkDropdown.ProductionSensorNetwork)
            {
                this.sensorNetworkServerIPAddress.Text = "192.168.0.10";
                this.sensorNetworkClientIPAddress.Text = "192.168.0.197";
                this.sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Black;

                this.sensorNetworkServerPort.Text = "1600";
                this.sensorNetworkClientPort.Text = "1680";
                this.sensorNetworkServerPort.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientPort.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                this.sensorNetworkServerIPAddress.Text = "127.0.0.1";
                this.sensorNetworkClientIPAddress.Text = "127.0.0.1";
                this.sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Black;

                this.sensorNetworkServerPort.Text = "1600";
                this.sensorNetworkClientPort.Text = "1680";
                this.sensorNetworkServerPort.ForeColor = System.Drawing.Color.Black;
                this.sensorNetworkClientPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtPLCPort_TextChanged(object sender, EventArgs e)
        {
            PLCPortValid = Validator.ValidatePort(txtPLCPort.Text);
            if (!PLCPortValid)
            {
                acceptSettings.Enabled = false;
                txtPLCPort.BackColor = System.Drawing.Color.Yellow;
                this.PLCPortToolTip.Show("Enter a valid port number\n" +
                    " between 1 and 65536", label3);
            }
            else
            {
                txtPLCPort.BackColor = System.Drawing.Color.White;
                this.PLCPortToolTip.Hide(label3);
            }
            if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
            {
                acceptSettings.Enabled = true;
            }

        }

        private void txtPLCIP_TextChanged(object sender, EventArgs e)
        {
            MCUIPValid = Validator.ValidateIPAddress(txtPLCIP.Text);
            if (!MCUIPValid)
            {
                acceptSettings.Enabled = false;
                txtPLCIP.BackColor = System.Drawing.Color.Yellow;
                this.MCUIPToolTip.Show("Enter a valid IP Address\n" +
                    " (xxx.xxx.xxx.xxx)", label4);
            }
            else
            {
                txtPLCIP.BackColor = System.Drawing.Color.White;
                this.MCUIPToolTip.Hide(label4);
            }
            if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
            {
                acceptSettings.Enabled = true;
            }
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            //this.WCOMPortToolTip.Show("Enter a valid port number, between 1 and 65536", label2);
        }
        private void label3_MouseHover(object sender, EventArgs e)
        {
            //this.PLCPortToolTip.Show("Enter a valid port number, between 1 and 65536", label3);
        }

        private void label4_MouseHover(object sender, EventArgs e)
        {
            //this.MCUIPToolTip.Show("Enter a valid IP Address (xxx.xxx.xxx.xxx)", label4);
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            //this.MCUPortToolTip.Show("Enter a valid port number, between 1 and 65536", label5);
        }

        private void txtWSCOMPort_TextChanged(object sender, EventArgs e)
        {

            if (txtWSCOMPort.Text != "COM")
            {
                WCOMPortValid = Validator.ValidatePort(txtWSCOMPort.Text);
                if (!WCOMPortValid)
                {
                    acceptSettings.Enabled = false;
                    txtWSCOMPort.BackColor = System.Drawing.Color.Yellow;
                    this.WCOMPortToolTip.Show("Enter a valid port number\n" +
                        " between 1 and 65536", label2);
                }
                else
                {
                    txtWSCOMPort.BackColor = System.Drawing.Color.White;
                    this.WCOMPortToolTip.Hide(label2);

                }
                if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = true;
                }

            }
            else
            {
                txtWSCOMPort.BackColor = System.Drawing.Color.LightGray;
                this.WCOMPortToolTip.Hide(label2);
            }
        }

        private void txtRemoteListenerCOMPort_TextChanged(object sender, EventArgs e)
        {

            if (txtRemoteListenerCOMPort.Text != "COM")
            {
                RLPortValid = Validator.ValidatePort(txtRemoteListenerCOMPort.Text);
                if (!RLPortValid)
                {
                    acceptSettings.Enabled = false;
                    txtRemoteListenerCOMPort.BackColor = System.Drawing.Color.Yellow;
                    this.RLPortToolTip.Show("Enter a valid port number\n" +" between 1 and 65536", RLLabel);
                }
                else
                {
                    txtRemoteListenerCOMPort.BackColor = System.Drawing.Color.White;
                    this.RLPortToolTip.Hide(RLLabel);

                }
                if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = true;
                }

            }
            else
            {
                txtRemoteListenerCOMPort.BackColor = System.Drawing.Color.LightGray;
                this.RLPortToolTip.Hide(RLLabel);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void acceptSettings_Click(object sender, EventArgs e)
        {
            finalSettings = !finalSettings;

            if (finalSettings == false)
            {
                if (comboWeatherStationBox.Text == "Production Weather Station")
                {
                    lastCreatedProductionWeatherStation = BuildWeatherStation();
                   
                    if (lastCreatedProductionWeatherStation != null)
                    {
                        startButton.BackColor = System.Drawing.Color.LimeGreen;
                        startButton.Enabled = true;
                    }
                    else
                    {
                        //if there is an error with the production weather station, display a tooltip & do not let the user start the telescope
                        this.WCOMPortToolTip.Show("Could not create production weather station on this port.", label8);
                        txtWSCOMPort.BackColor = System.Drawing.Color.Yellow;
                    }

                }
                else
                {
                    startButton.BackColor = System.Drawing.Color.LimeGreen;
                    startButton.Enabled = true;
                }

                //Editing text to relflect state -- When the settings are being edited, the button will need to say 'finalize'
                acceptSettings.Text = "Edit Settings";

                startRTGroupbox.BackColor = System.Drawing.Color.Gray;
                loopBackBox.Enabled = false;

                simulationSettingsGroupbox.BackColor = System.Drawing.Color.DarkGray;
                comboSensorNetworkBox.Enabled = false;
                comboWeatherStationBox.Enabled = false;
                comboSpectraCyberBox.Enabled = false;
                comboPLCType.Enabled = false;
                LocalIPCombo.Enabled = false;

                portGroupbox.BackColor = System.Drawing.Color.DarkGray;
                ProdcheckBox.Enabled = false;
                txtPLCIP.Enabled = false;
                txtMcuCOMPort.Enabled = false;
                txtWSCOMPort.Enabled = false;
                txtPLCPort.Enabled = false;
                txtSpectraPort.Enabled = false;

                sensorNetworkServerIPAddress.Enabled = false;
                sensorNetworkServerPort.Enabled = false;
                sensorNetworkClientIPAddress.Enabled = false;
                sensorNetworkClientPort.Enabled = false;

            }
            else if (finalSettings == true)
            {
                // Editing Text to reflect state -- when finalized, you can click "edit settings"
                acceptSettings.Text = "Finalize Settings";
                
                //hide WS error
                this.WCOMPortToolTip.Hide(label8);
                txtWSCOMPort.BackColor = System.Drawing.Color.White;


                startRTGroupbox.BackColor = System.Drawing.Color.DarkGray;
                startButton.Enabled = false;
                startButton.BackColor = System.Drawing.Color.LightGray;
                loopBackBox.Enabled = true;

                simulationSettingsGroupbox.BackColor = System.Drawing.Color.Gray;
                comboSensorNetworkBox.Enabled = true;
                comboWeatherStationBox.Enabled = true;
                comboSpectraCyberBox.Enabled = true;
                comboPLCType.Enabled = true;
                LocalIPCombo.Enabled = true;

                portGroupbox.BackColor = System.Drawing.Color.Gray;
                ProdcheckBox.Enabled = true;
                txtPLCIP.Enabled = true;
                txtMcuCOMPort.Enabled = true;
                txtWSCOMPort.Enabled = true;
                txtPLCPort.Enabled = true;
                txtSpectraPort.Enabled = true;

                sensorNetworkServerIPAddress.Enabled = true;
                sensorNetworkServerPort.Enabled = true;
                sensorNetworkClientIPAddress.Enabled = true;
                sensorNetworkClientPort.Enabled = true;
            }

        }

        //Help button clicked ( user interface documentation PDF)
        private void helpButton_click(object sender, EventArgs e)
        {
            string filename = Directory.GetCurrentDirectory() + "\\" + "UIDoc.pdf";
            if (File.Exists(filename))
                System.Diagnostics.Process.Start(filename);
        }

        private void simulationSettingsGroupbox_Enter(object sender, EventArgs e)
        {

        }

        private void portGroupbox_Enter(object sender, EventArgs e)
        {

        }

        // Get and set Weather Station override status

        public void setWSOverride(bool WSO)
        {
            MainControlRoomController.ControlRoom.weatherStationOverride = WSO;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.WEATHER_STATION, WSO);
        }

        public bool getWSOverride()
        {
            return MainControlRoomController.ControlRoom.weatherStationOverride;
        }

        private void txtMcuCOMPort_TextChanged(object sender, EventArgs e)
        {
            MCUPortValid = Validator.ValidatePort(txtMcuCOMPort.Text);
            if (!MCUPortValid)
            {
                acceptSettings.Enabled = false;
                txtMcuCOMPort.BackColor = System.Drawing.Color.Yellow;
                this.MCUIPToolTip.Show("Enter a valid port number\n" +
                    "between 1 and 65536", label5);
            }
            else
            {
                txtMcuCOMPort.BackColor = System.Drawing.Color.White;
                this.MCUIPToolTip.Hide(label5);
            }
            if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
            {
                acceptSettings.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboEncoderType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboPLCType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sensorNetworkServerIPAddress_TextChanged(object sender, EventArgs e)
        {
            if (sensorNetworkServerIPAddress.Text != "IP Address")
            {
                SensorNetworkServerIPBool = Validator.ValidateIPAddress(sensorNetworkServerIPAddress.Text);
                if (!SensorNetworkServerIPBool)
                {
                    acceptSettings.Enabled = false;
                    sensorNetworkServerIPAddress.BackColor = System.Drawing.Color.Yellow;
                    this.MCUIPToolTip.Show("Enter a valid IP Address\n" +
                        " (xxx.xxx.xxx.xxx)", label6);
                }
                else
                {
                    sensorNetworkServerIPAddress.BackColor = System.Drawing.Color.White;
                    this.MCUIPToolTip.Hide(label6);
                }
                if (RLPortValid &&  PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = true;
                }
            }
            else
            {
                sensorNetworkServerIPAddress.BackColor = System.Drawing.Color.LightGray;
                this.MCUIPToolTip.Hide(label6);
            }
        }

        //Sets removes the temp value and sets text to black 
        private void sensorNetworkServerIPAddress_Enter(object sender, EventArgs e)
        {
            if (sensorNetworkServerIPAddress.Text == "IP Address")
            {
                sensorNetworkServerIPAddress.Text = "";
                sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void sensorNetworkServerIPAddress_Leave(object sender, EventArgs e)
        {
            if (sensorNetworkServerIPAddress.Text == "")
            {
                sensorNetworkServerIPAddress.Text = "IP Address";
                sensorNetworkServerIPAddress.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void sensorNetworkClientIPAddress_TextChanged(object sender, EventArgs e)
        {
            if (sensorNetworkClientIPAddress.Text != "IP Address")
            {
                SensorNetworkClientIPBool = Validator.ValidateIPAddress(sensorNetworkClientIPAddress.Text);
                if (!SensorNetworkClientIPBool)
                {
                    acceptSettings.Enabled = false;
                    sensorNetworkClientIPAddress.BackColor = System.Drawing.Color.Yellow;
                    this.MCUIPToolTip.Show("Enter a valid IP Address\n" +
                        " (xxx.xxx.xxx.xxx)", label7);
                }
                else
                {
                    sensorNetworkClientIPAddress.BackColor = System.Drawing.Color.White;
                    this.MCUIPToolTip.Hide(label7);
                }
                if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = true;
                }
            }
            else
            {
                sensorNetworkClientIPAddress.BackColor = System.Drawing.Color.LightGray;
                this.MCUIPToolTip.Hide(label7);
            }
        }

        //Sets removes the temp value and sets text to black 
        private void sensorNetworkClientIPAddress_Enter(object sender, EventArgs e)
        {
            if (sensorNetworkClientIPAddress.Text == "IP Address")
            {
                sensorNetworkClientIPAddress.Text = "";
                sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void sensorNetworkClientIPAddress_Leave(object sender, EventArgs e)
        {
            if (sensorNetworkClientIPAddress.Text == "")
            {
                sensorNetworkClientIPAddress.Text = "IP Address";
                sensorNetworkClientIPAddress.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void sensorNetworkServerPort_TextChanged(object sender, EventArgs e)
        {
            if (sensorNetworkServerPort.Text != "Port")
            {
                SensorNetworkServerPortBool = Validator.ValidatePort(sensorNetworkServerPort.Text);
                if (!SensorNetworkServerPortBool)
                {
                    acceptSettings.Enabled = false;
                    sensorNetworkServerPort.BackColor = System.Drawing.Color.Yellow;
                    this.WCOMPortToolTip.Show("Enter a valid port number\n" +
                        " between 1 and 65536", label6);
                }
                else
                {
                    sensorNetworkServerPort.BackColor = System.Drawing.Color.White;
                    this.WCOMPortToolTip.Hide(label6);

                }
                if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = true;
                }
            }
            else
            {
                sensorNetworkServerPort.BackColor = System.Drawing.Color.LightGray;
                this.WCOMPortToolTip.Hide(label6);
            }
        }

        //Sets removes the temp value and sets text to black 
        private void sensorNetworkServerPort_Enter(object sender, EventArgs e)
        {
            if (sensorNetworkServerPort.Text == "Port")
            {
                sensorNetworkServerPort.Text = "";
                sensorNetworkServerPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void sensorNetworkServerPort_Leave(object sender, EventArgs e)
        {
            if (sensorNetworkServerPort.Text == "")
            {
                sensorNetworkServerPort.Text = "Port";
                sensorNetworkServerPort.ForeColor = System.Drawing.Color.Gray;
            }
        }

        //Sets removes the temp value and sets text to black 
        private void txtSpectraPort_Enter(object sender, EventArgs e)
        {
            if (txtSpectraPort.Text == "COM")
            {
                txtSpectraPort.Text = "";
                txtSpectraPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void txtSpectraPort_Leave(object sender, EventArgs e)
        {
            if (txtSpectraPort.Text == "")
            {
                txtSpectraPort.Text = "COM";
                txtSpectraPort.ForeColor = System.Drawing.Color.Gray;
            }
        }

        //Sets removes the temp value and sets text to black 
        private void txtWSCOMPort_Enter(object sender, EventArgs e)
        {
            if (txtWSCOMPort.Text == "COM")
            {
                txtWSCOMPort.Text = "";
                txtWSCOMPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void txtWSCOMPort_Leave(object sender, EventArgs e)
        {
            if (txtWSCOMPort.Text == "")
            {
                txtWSCOMPort.Text = "COM";
                txtWSCOMPort.ForeColor = System.Drawing.Color.Gray;
            }
        }


        //Sets removes the temp value and sets text to black 
        private void txtRemoteListenerCOMPort_Enter(object sender, EventArgs e)
        {
            if (txtRemoteListenerCOMPort.Text == "COM")
            {
                txtRemoteListenerCOMPort.Text = "";
                txtRemoteListenerCOMPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void txtRemoteListenerCOMPort_Leave(object sender, EventArgs e)
        {
            if (txtRemoteListenerCOMPort.Text == "")
            {
                txtRemoteListenerCOMPort.Text = "COM";
                txtRemoteListenerCOMPort.ForeColor = System.Drawing.Color.Gray;
            }
        }


        private void sensorNetworkClientPort_TextChanged(object sender, EventArgs e)
        {
            if (sensorNetworkClientPort.Text != "Port")
            {
                SensorNetworkClientPortBool = Validator.ValidatePort(sensorNetworkClientPort.Text);
                if (!SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = false;
                    sensorNetworkClientPort.BackColor = System.Drawing.Color.Yellow;
                    this.WCOMPortToolTip.Show("Enter a valid port number\n" +
                        " between 1 and 65536", label7);
                }
                else
                {
                    sensorNetworkClientPort.BackColor = System.Drawing.Color.White;
                    this.WCOMPortToolTip.Hide(label7);

                }
                if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool)
                {
                    acceptSettings.Enabled = true;
                }
            }
            else
            {
                sensorNetworkClientPort.BackColor = System.Drawing.Color.LightGray;
                this.WCOMPortToolTip.Hide(label7);
            }
        }

        //Sets removes the temp value and sets text to black 
        private void sensorNetworkClientPort_Enter(object sender, EventArgs e)
        {
            if (sensorNetworkClientPort.Text == "Port")
            {
                sensorNetworkClientPort.Text = "";
                sensorNetworkClientPort.ForeColor = System.Drawing.Color.Black;
            }
        }

        //Sets sets the temp value and sets text to gray 
        private void sensorNetworkClientPort_Leave(object sender, EventArgs e)
        {
            if (sensorNetworkClientPort.Text == "")
            {
                sensorNetworkClientPort.Text = "Port";
                sensorNetworkClientPort.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void WCOMPortToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtSpectraPort.Text != "COM")
            {
                SpectraPortValid = Validator.ValidatePort(txtSpectraPort.Text);
                if (!SpectraPortValid)
                {
                    acceptSettings.Enabled = false;
                    txtSpectraPort.BackColor = System.Drawing.Color.Yellow;
                    this.WCOMPortToolTip.Show("Enter a valid port number\n" +
                        " between 1 and 65536", label8);
                }
                else
                {
                    txtSpectraPort.BackColor = System.Drawing.Color.White;
                    this.WCOMPortToolTip.Hide(label8);

                }
                if (RLPortValid && PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid && SensorNetworkServerIPBool && SensorNetworkServerPortBool && SensorNetworkClientIPBool && SensorNetworkClientPortBool && SpectraPortValid)
                {
                    acceptSettings.Enabled = true;
                }
            }
            else
            {
                txtSpectraPort.BackColor = System.Drawing.Color.LightGray;
                this.WCOMPortToolTip.Hide(label8);
            }
        }

        public void PNBox_CheckedChanged(bool PNE)
        {
            PNEnabled = PNE;

            // Update PLC, MCU, RadioTelescopeController, and SensorNetwork Push Notification values            
            KeyValuePair< RadioTelescope, AbstractPLCDriver > rtAPLC = AbstractRTDriverPairList[0]; 
            RadioTelescope currentRT = rtAPLC.Key;
            RadioTelescopeController rtc = ProgramRTControllerList[0]; 

            try
            {
                // Only update values if using ProductionPLC 
                var rttype = currentRT.PLCDriver.GetType(); 
                if (currentRT.PLCDriver.GetType().Name is "ProductionPLCDriver")
                {
                    ProductionPLCDriver p = (ProductionPLCDriver) currentRT.PLCDriver;
                    p.SetPushNotificationEnabled(PNEnabled); 
                }
                currentRT.SensorNetworkServer.PNEnabled = PNEnabled; 
                rtc.PNEnabled = PNEnabled;

                string PNStatus = PNEnabled ? "enabled." : "disabled."; 
                logger.Info(Utilities.GetTimeStamp() + ": Push/Email Notifications have been " + PNStatus);

            } catch (Exception ex)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Error updating Push Notification values in PLC, MCU, RadioTelescopeController, and/or SensorNetwork.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var apptControlForm = new AppointmentControlForm(current_rt_id);

            apptControlForm.Show();
        }
    }
}