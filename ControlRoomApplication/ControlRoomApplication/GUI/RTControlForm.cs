using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Validation;
using ControlRoomApplication.GUI.Data;
using ControlRoomApplication.Util;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.GUI;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public Appointment CurrentAppointment { get; set; }
        public User ControlRoomUser { get; set; }
        public Coordinate TargetCoordinate { get; set; }
        public double Increment { get; set; }
        public CoordinateCalculationController CoordCalc { set; get; }
        public ControlRoom controlRoom { get; set; }
        private RadioTelescopeController rtController { get; set; }
        // private ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        public int rtId { get; set; }

        
        private static int current_rt_id;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool freeEditActive;
        public bool manualControlActive;
        public bool spectraEditActive;
        bool manual_save_state;
        bool save_state;
        public SpectraCyberDCGainEnum DCGainInput;
        public SpectraCyberIntegrationTimeEnum IntTimeInput;
        public SpectraCyberBandwidthEnum BandwidthInput;
        public bool acceptSettings = false;
        public bool frequencyValid = false;
        public bool offsetVoltValid = false;
        public bool IFGainValid = false;
        RTControlFormData formData;

        public FreeControlForm(ControlRoom new_controlRoom, int new_rtId, RTControlFormData controlFormData)
        {
            InitializeComponent();
            // Set ControlRoom
            controlRoom = new_controlRoom;
            // Set RT id
            rtId = new_rtId;
            // Make coordCalc
            rtController = controlRoom.RadioTelescopeControllers.Find(x => x.RadioTelescope.Id == rtId);
            CoordCalc = rtController.CoordinateController;
            // Set increment
            Increment = 1;
            UpdateIncrementButtons();

            // set form data object
            formData = controlFormData;
            formData.speed = 1;
        
            // set form data
            controlScriptsCombo.SelectedIndex = formData.controlScriptIndex;
            scanTypeComboBox.SelectedIndex = formData.spectraCyberScanIndex;
            frequency.Text = formData.frequency;
            DCGain.SelectedIndex = formData.DCGainIndex;
            integrationStepCombo.SelectedIndex = formData.integrationStepIndex;
            speedTextBox.Text = formData.speed.ToString();
            offsetVoltage.Text = formData.offsetVoltage;
            IFGainVal.Text = formData.IFGain;
            immediateRadioButton.Checked = formData.immediateStopBool;
            ControlledButtonRadio.Checked = formData.controlledStopBool;
            manual_save_state = formData.manualControlEnabled;
            save_state = formData.freeControlEnabled;

            ControlRoomUser = DatabaseOperations.GetControlRoomUser();

            // Add free control appt
            CurrentAppointment = new Appointment();
            CurrentAppointment.start_time = DateTime.UtcNow.AddSeconds(5);
            CurrentAppointment.end_time = DateTime.UtcNow.AddMinutes(15);
            CurrentAppointment._Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment._Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment._Priority = AppointmentPriorityEnum.MANUAL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.SPECTRAL);
            CurrentAppointment.CelestialBody = new CelestialBody();
            CurrentAppointment.CelestialBody.Coordinate = new Coordinate(0, 0);
            
            CurrentAppointment.Orientation = rtController.GetCurrentOrientation();
            CurrentAppointment.Telescope = controlRoom.RadioTelescopes.Find(x => x.Id == rtId);
            CurrentAppointment.User = ControlRoomUser;

            rtController.RadioTelescope.SpectraCyberController.Schedule.Mode = SpectraCyberScanScheduleMode.OFF;

            DatabaseOperations.AddAppointment(CurrentAppointment);

            //Calibrate Move
            CalibrateMove();
            runControlScriptButton.Enabled = false;

            //Initialize Free control Box based on manual control
            if (!save_state)
            {
                editButton.Text = "Edit Position";
                if (manual_save_state)
                {
                   editButton.BackColor = System.Drawing.Color.DarkGray;
                   freeControlGroupbox.BackColor = System.Drawing.Color.DarkGray;
                    editButton.Enabled = false;
                }
                else
                {
                    editButton.BackColor = System.Drawing.Color.Red;
                    freeControlGroupbox.BackColor = System.Drawing.Color.DarkGray;
                    editButton.Enabled = true;
                }
               
                decIncGroupbox.BackColor = System.Drawing.Color.DarkGray;
                RAIncGroupbox.BackColor = System.Drawing.Color.DarkGray;
               
            }
            else
            {
                editButton.Text = "Save Position";
                editButton.BackColor = System.Drawing.Color.LimeGreen;
                freeControlGroupbox.BackColor = System.Drawing.Color.Gainsboro;
                decIncGroupbox.BackColor = System.Drawing.Color.Gray;
                RAIncGroupbox.BackColor = System.Drawing.Color.Gray;
                manualControlButton.Enabled = !formData.freeControlEnabled;
                manualControlButton.BackColor = System.Drawing.Color.DarkGray;
            }
            PosDecButton.Enabled = formData.freeControlEnabled;
            NegDecButton.Enabled = formData.freeControlEnabled;
            PosRAButton.Enabled = formData.freeControlEnabled;
            NegRAButton.Enabled = formData.freeControlEnabled;
            oneForthButton.Enabled = formData.freeControlEnabled;
            oneForthButtonDec.Enabled = formData.freeControlEnabled;
            oneButton.Enabled = formData.freeControlEnabled;
            oneButtonDec.Enabled = formData.freeControlEnabled;
            fiveButton.Enabled = formData.freeControlEnabled;
            fiveButtonDec.Enabled = formData.freeControlEnabled;
            tenButton.Enabled = formData.freeControlEnabled;
            tenButtonDec.Enabled = formData.freeControlEnabled;

            //Initialize Manual control Box based on previous data (false by default)
            if (!manual_save_state)
            {
                manualControlButton.Text = "Activate Manual Control";
                // if free control is active, this button will be disabled. Gray it out if so
                if (save_state)
                {
                    manualControlButton.BackColor = System.Drawing.Color.DarkGray;
                    manualControlButton.Enabled = false;
                }
                else
                {
                    manualControlButton.BackColor = System.Drawing.Color.Red;
                    manualControlButton.Enabled = true;
                }
                manualGroupBox.BackColor = System.Drawing.Color.DarkGray;
            }
            else if (manual_save_state)
            {
                manualControlButton.Text = "Deactivate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.LimeGreen;
                manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;

            }
            plusElaButton.Enabled = formData.manualControlEnabled;
            cwAzJogButton.Enabled = formData.manualControlEnabled;
            ccwAzJogButton.Enabled = formData.manualControlEnabled;
            subElaButton.Enabled = formData.manualControlEnabled;
            ControlledButtonRadio.Enabled = formData.manualControlEnabled;
            immediateRadioButton.Enabled = formData.manualControlEnabled;
            speedTextBox.Enabled = formData.manualControlEnabled;
            speedTrackBar.Enabled = formData.manualControlEnabled;
            speedTrackBar.Value = 10;


            //Initialize Start and Stop Scan buttons as disabled
            spectraEditActive = true;
            startScanButton.BackColor = System.Drawing.Color.DarkGray;
            startScanButton.Enabled = false;
            stopScanButton.BackColor = System.Drawing.Color.DarkGray;
            stopScanButton.Enabled = false;

            // finalize settings should be based on values in scan combo box
            finalizeSettingsButton.Enabled = allScanInputsValid();

            this.FormClosing += FreeControlForm_Closing;

            logger.Info(Utilities.GetTimeStamp() + ": Radio Telescope Control Form Initalized");
        }

        void FreeControlForm_Closing(object sender, CancelEventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Radio Telescope Control Form Closing");
            CurrentAppointment._Status = AppointmentStatusEnum.COMPLETED;
            DatabaseOperations.UpdateAppointment(CurrentAppointment);

            var threads = controlRoom.RTControllerManagementThreads.Where<RadioTelescopeControllerManagementThread>(t => t.RTController == rtController).ToList<RadioTelescopeControllerManagementThread>();

            //Done in a new thread so the UI does not freeze while waiting for the move to finish
            new Thread(() =>
            {
                threads[0].EndAppointment();
            }).Start();

            // Disables manual control on page close
            formData.manualControlEnabled = false; 

            timer1.Enabled = false;
        }

        private void PosDecButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Positive declination Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.right_ascension, TargetCoordinate.declination + Increment);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if(test_orientation.azimuth > 0 && test_orientation.elevation > 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void NegDecButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Negitive declination Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.right_ascension, TargetCoordinate.declination - Increment);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if (test_orientation.azimuth > 0 && test_orientation.elevation > 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void NegRAButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Negitive Right Ascension Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.right_ascension - Increment, TargetCoordinate.declination);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if (test_orientation.azimuth > 0 && test_orientation.elevation > 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void PosRAButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Positive Right Ascension Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.right_ascension + Increment, TargetCoordinate.declination);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if (test_orientation.azimuth >= 0 && test_orientation.elevation >= 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Calibrate Button Clicked");
            CalibrateMove();
        }

        public void CalibrateMove()
        {
            logger.Info(Utilities.GetTimeStamp() + ": CalibrateMove ");
            TargetCoordinate = CoordCalc.OrientationToCoordinate(CurrentAppointment.Orientation, DateTime.UtcNow);
            UpdateText();
        }

        private void CoordMove()
        {
            logger.Info(Utilities.GetTimeStamp() + ": CoordMove ");
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.UpdateAppointment(CurrentAppointment);
            UpdateText();
        }

        private void UpdateText()
        {
            string RA = TargetCoordinate.right_ascension.ToString("0.##");
            string Dec = TargetCoordinate.declination.ToString("0.##");
            logger.Info(Utilities.GetTimeStamp() + ": UpdateText, Target Coordinate = RA:" + RA + ", Dec:" + Dec);
            
            TargetRATextBox.Text = RA;
            TargetDecTextBox.Text = Dec;

            errorLabel.Text = "Free Control for Radio Telescope " + rtId.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Entities.Orientation currentOrienation = rtController.GetCurrentOrientation();
            Coordinate ConvertedPosition = CoordCalc.OrientationToCoordinate(currentOrienation, DateTime.UtcNow);

            Utilities.WriteToGUIFromThread(this, () => {
                label4.Text = String.Format("{0:N2}", currentOrienation.azimuth);
                label5.Text = String.Format("{0:N2}", currentOrienation.elevation);

                ActualRATextBox.Text = ConvertedPosition.right_ascension.ToString("0.##");
                ActualDecTextBox.Text = ConvertedPosition.declination.ToString("0.##");
            });
        }

        private void oneForthButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Increment = 0.25 Button Clicked");
            Increment = 0.25;
            UpdateIncrementButtons();
        }

        private void oneButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Increment = 1 Button Clicked");
            Increment = 1;
            UpdateIncrementButtons();
        }

        private void fiveButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Increment = 5 Button Clicked");
            Increment = 5;
            UpdateIncrementButtons();
        }

        private void tenButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Increment = 10 Button Clicked");
            Increment = 10;
            UpdateIncrementButtons();
        }

        private void UpdateIncrementButtons()
        {

            switch (Increment)
            {
                case 0.25:
                    oneForthButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                case 1:
                    oneButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                case 5:
                    fiveButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                case 10:
                    tenButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                default:
                    throw new ArgumentException("Invalid Increment");
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Edit Button Clicked");
            save_state = !save_state;

            formData.freeControlEnabled = save_state;
            if (!save_state)
            {
                editButton.Text = "Edit Position";
                editButton.BackColor = System.Drawing.Color.Red;
                freeControlGroupbox.BackColor = System.Drawing.Color.DarkGray;
                manualControlButton.BackColor = System.Drawing.Color.Red;
                decIncGroupbox.BackColor = System.Drawing.Color.DarkGray;
                RAIncGroupbox.BackColor = System.Drawing.Color.DarkGray;
                double newRA;
                double newDec;
                double.TryParse(TargetRATextBox.Text, out newRA);
                double.TryParse(TargetDecTextBox.Text, out newDec);
                Coordinate new_coord = new Coordinate(newRA, newDec);
                Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
                if (test_orientation.azimuth >= 0 && test_orientation.elevation >= 0)
                {
                    TargetCoordinate = new_coord;
                    CoordMove();
                }
                else
                {
                    errorLabel.Text = "Invalid Coordinate: orienation out of range";
                }
            }
            else
            {
                editButton.Text = "Save Position";
                manualControlButton.BackColor = System.Drawing.Color.DarkGray;
                editButton.BackColor = System.Drawing.Color.LimeGreen;
                freeControlGroupbox.BackColor = System.Drawing.Color.Gainsboro;
                decIncGroupbox.BackColor = System.Drawing.Color.Gray;
                RAIncGroupbox.BackColor = System.Drawing.Color.Gray;
            }

            PosDecButton.Enabled = save_state;
            NegDecButton.Enabled = save_state;
            PosRAButton.Enabled = save_state;
            NegRAButton.Enabled = save_state;
            oneForthButton.Enabled = save_state;
            oneForthButtonDec.Enabled = save_state;
            oneButton.Enabled = save_state;
            oneButtonDec.Enabled = save_state;
            fiveButton.Enabled = save_state;
            fiveButtonDec.Enabled = save_state;
            tenButton.Enabled = save_state;
            tenButtonDec.Enabled = save_state;
            TargetRATextBox.ReadOnly = save_state;
            TargetDecTextBox.ReadOnly = save_state;

            manualControlButton.Enabled = !save_state;
        }
        //
        private void manualControlButton_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Activate Manual Control Clicked");
            manual_save_state = !manual_save_state;
            formData.manualControlEnabled = manual_save_state;
            
            if (!manual_save_state)
            {
                manualControlButton.Text = "Activate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.Red;
                manualGroupBox.BackColor = System.Drawing.Color.DarkGray;
                editButton.BackColor = System.Drawing.Color.Red;
            }
            else if(manual_save_state)
            {
                manualControlButton.Text = "Deactivate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.LimeGreen;
                manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
                editButton.BackColor = System.Drawing.Color.DarkGray;

            }
            plusElaButton.Enabled = manual_save_state;
            cwAzJogButton.Enabled = manual_save_state;
            ccwAzJogButton.Enabled = manual_save_state;
            subElaButton.Enabled = manual_save_state;
            ControlledButtonRadio.Enabled = manual_save_state;
            immediateRadioButton.Enabled = manual_save_state;
            speedTextBox.Enabled = manual_save_state;
            speedTrackBar.Enabled = manual_save_state;

            editButton.Enabled = !manual_save_state;
        }

   
        //Run Script Button Functionality
        //Case Depends on which script is currently selected 
        private async void runControlScript_Click(object sender, EventArgs e)
        {
            int index = controlScriptsCombo.SelectedIndex + 0;
            string indexName = controlScriptsCombo.SelectedItem.ToString();

            // We must run this async so it doesn't hold up the UI
            await Task.Run(() => {
                logger.Info($"{Utilities.GetTimeStamp()}: Starting script {indexName}.");

                MovementResult movementResult = MovementResult.None;

                switch (index)
                {
                    case 1:
                        movementResult = rtController.StowRadioTelescope(MovementPriority.Manual);
                        break;

                    case 2:
                        movementResult = rtController.FullElevationMove(MovementPriority.Manual);
                        break;

                    case 3:
                        movementResult = rtController.MoveRadioTelescopeByXDegrees(new Entities.Orientation(360, 0), MovementPriority.Manual);
                        break;

                    case 4:
                        movementResult = rtController.MoveRadioTelescopeByXDegrees(new Entities.Orientation(-360, 0), MovementPriority.Manual);
                        break;

                    case 5:
                        movementResult = rtController.ThermalCalibrateRadioTelescope(MovementPriority.Manual);
                        break;

                    case 6:
                        movementResult = rtController.SnowDump(MovementPriority.Manual);
                        break;

                    case 7:
                        movementResult = rtController.HomeTelescope(MovementPriority.Manual);
                        break;

                    case 8:
                        // Create a new CustomOrientationInputDialog instance to allow the user to enter data 

                        CustomOrientationInputDialog id = new CustomOrientationInputDialog(rtController.EnableSoftwareStops, rtController.RadioTelescope.teleType, rtController.RadioTelescope.maxElevationDegrees, rtController.RadioTelescope.minElevationDegrees);
                        
                        Entities.Orientation currentOrientation = rtController.GetCurrentOrientation();

                        id.Text = "Custom Orientation Movement";    // Set the title of the input form 

                        if (id.ShowDialog() == DialogResult.OK)     // Use the data entered when the user clicks OK. (OK cannot be clicked unless the input is valid) 
                        {
                            Entities.Orientation moveTo = new Entities.Orientation(id.AzimuthPos, id.ElevationPos);
                            movementResult = rtController.MoveRadioTelescopeToOrientation(moveTo, MovementPriority.Manual);
                        }

                        break;

                    case 9:
                        rtController.StartRadioTelescopeJog(1, RadioTelescopeDirectionEnum.ClockwiseOrNegative, RadioTelescopeAxisEnum.AZIMUTH);
                        MessageBox.Show("Currently spinning Azimuth. Press OK to stop spinning.", "Azimuth Moving");
                        ExecuteCorrectStop();
                        movementResult = MovementResult.Success;
                        break;

                    // Hardware movement script
                    case 10:
                        movementResult = rtController.ExecuteHardwareMovementScript(MovementPriority.Manual);
                        break;
                    default:
                        // Script does not exist
                        break;
                }
                
                if(movementResult == MovementResult.Success)
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Successfully finished script {indexName}.");
                }
                else if (movementResult != MovementResult.None)
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Script {indexName} FAILED with error message: {movementResult.ToString()}");
                    PushNotification.sendToAllAdmins("Script Failed", $"Script {indexName} FAILED with error message: {movementResult.ToString()}", rtController.PNEnabled);
                    EmailNotifications.sendToAllAdmins("Script Failed", $"Script {indexName} FAILED with error message: {movementResult.ToString()}", rtController.PNEnabled);
                }
            });
        }

        //Control Script combo box enables run button when a script has been selected
        private void controlScriptsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controlScriptsCombo.SelectedIndex > 0)
            {
                runControlScriptButton.Enabled = true;
                runControlScriptButton.BackColor = System.Drawing.Color.LimeGreen;
            }
            if(controlScriptsCombo.SelectedIndex == 0)
            {
                runControlScriptButton.Enabled = false;
                runControlScriptButton.BackColor = System.Drawing.Color.Gray;
            }
            
            formData.controlScriptIndex = controlScriptsCombo.SelectedIndex;

        }

        /// <summary>
        /// Functionality for when the STOP Telescope button is pressed. Creates a confirmation pop up box to prevent accidental presses of the stop button before sending any stop script.
        /// </summary>
        private void stopRT_click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to stop the telescope?", "Telescope Stop Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // Run the stop script for the telescope
                MovementResult moveResult = rtController.InterruptRadioTelescope();
                
                if (moveResult == MovementResult.Success)
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Telescope movement stopped.");
                }
                else
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Failed to stop telescope movement.");
                }
            }
        }

        private void ccwAzJogButton_Down( object sender , MouseEventArgs e ) {
            if (Validator.ValidateSpeedTextOnly(speedTextBox.Text))
            {
                double speed = Convert.ToDouble(speedTextBox.Text);
                if (Validator.ValidateSpeed(speed))
                {
                    // Start CW Jog
                    MovementResult result = rtController.StartRadioTelescopeJog(speed, RadioTelescopeDirectionEnum.CounterclockwiseOrPositive, RadioTelescopeAxisEnum.AZIMUTH);

                    if (result == MovementResult.Success)
                        logger.Info($"{Utilities.GetTimeStamp()}: Successfully started azimuth counterclockwise jog.");
                    else if (result == MovementResult.StoppingCurrentMove)
                        logger.Info($"{Utilities.GetTimeStamp()}: Stopping current movement. Please wait until that movement has finished ending and try to jog again.");
                    else if (result == MovementResult.AlreadyMoving)
                        logger.Info($"{Utilities.GetTimeStamp()}: Azimuth counterclockwise jog BLOCKED. Another manual script is already running.");
                    else
                    {
                        logger.Info($"{Utilities.GetTimeStamp()}: An error occurred trying to jog az counterclockwise: {result.ToString()}");
                        PushNotification.sendToAllAdmins("Jog Error", $"An error occurred trying to jog az counterclockwise: {result.ToString()}", rtController.PNEnabled);
                        EmailNotifications.sendToAllAdmins("Jog Error", $"An error occurred trying to jog az counterclockwise: {result.ToString()}", rtController.PNEnabled);
                    }
                }
                else
                {
                    MessageBox.Show("Speed is not in valid range, 0.0-2.0 RPMs. Please try again");
                }
            }
            else
            {
                MessageBox.Show("Invalid speed. Must be in RPMs between 0.0 and 2.0");
            }
        }

        private void ccwAzJogButton_Up( object sender , MouseEventArgs e ) {
            //Stop Move
            ExecuteCorrectStop();
        }

        private void cwAzJogButton_Down(object sender, MouseEventArgs e)
        {
            if (Validator.ValidateSpeedTextOnly(speedTextBox.Text))
            {
                double speed = Convert.ToDouble(speedTextBox.Text);
               
                if (Validator.ValidateSpeed(speed))
                {
                    // Start CW Jog
                    MovementResult result = rtController.StartRadioTelescopeJog(speed, RadioTelescopeDirectionEnum.ClockwiseOrNegative, RadioTelescopeAxisEnum.AZIMUTH);

                    if (result == MovementResult.Success)
                        logger.Info($"{Utilities.GetTimeStamp()}: Successfully started azimuth clockwise jog.");
                    else if (result == MovementResult.StoppingCurrentMove)
                        logger.Info($"{Utilities.GetTimeStamp()}: Stopping current movement. Please wait until that movement has finished ending and try to jog again.");
                    else if (result == MovementResult.AlreadyMoving)
                        logger.Info($"{Utilities.GetTimeStamp()}: Azimuth clockwise jog BLOCKED. Another manual script is already running.");
                    else
                    {
                        logger.Info($"{Utilities.GetTimeStamp()}: An error occurred trying to jog az clockwise: {result.ToString()}");
                        PushNotification.sendToAllAdmins("Jog Error", $"An error occurred trying to jog az clockwise: {result.ToString()}", rtController.PNEnabled);
                        EmailNotifications.sendToAllAdmins("Jog Error", $"An error occurred trying to jog az clockwise: {result.ToString()}", rtController.PNEnabled);
                    }
                }
                else
                {
                    MessageBox.Show("Speed is not in valid range, 0.0-2.0 RPMs. Please try again");
                }
            }
            else
            {
                MessageBox.Show("Invalid speed. Must be in RPMs between 0.0 and 2.0");
            }
        }

        private void cwAzJogButton_UP( object sender , MouseEventArgs e ) {
            //  Stop Move
            ExecuteCorrectStop();
        }

        private void ExecuteCorrectStop()
        {
            MovementResult result = MovementResult.None;

            if (ControlledButtonRadio.Checked)
            {
                result = rtController.ExecuteRadioTelescopeStopJog(MCUCommandType.ControlledStop);
                formData.immediateStopBool = false;
                formData.controlledStopBool = true;

                if (result == MovementResult.Success)
                    logger.Info($"{Utilities.GetTimeStamp()}: Successfully stopped jog with a controlled stop.");
                else
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Controlled stop error: {result}");
                }
            }
            else if (immediateRadioButton.Checked)
            {
                result = rtController.ExecuteRadioTelescopeStopJog(MCUCommandType.ImmediateStop);
                formData.immediateStopBool = true;
                formData.controlledStopBool = false;

                if (result == MovementResult.Success)
                    logger.Info($"{Utilities.GetTimeStamp()}: Successfully stopped jog with an immediate stop.");
                else
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Immediate stop error: {result}");
                }
            }
            else
            {
                logger.Info(Utilities.GetTimeStamp() + ": Invalid Stop Selected");
                throw new Exception();
            }

        }

        //This Button executes a system call that opens up the user interface documentation as a PDF
        private void helpButton_click(object sender, EventArgs e)
        {
            string filename = Directory.GetCurrentDirectory() + "\\" + "UIDoc.pdf";
            if (File.Exists(filename))
                System.Diagnostics.Process.Start(filename);
        }

        private void plusElaButton_Down(object sender, MouseEventArgs e ){
            if (Validator.ValidateSpeedTextOnly(speedTextBox.Text))
            {
                double speed = Convert.ToDouble(speedTextBox.Text);
                if (Validator.ValidateSpeed(speed))
                {
                    // Start CW Jog
                    MovementResult result = rtController.StartRadioTelescopeJog(speed, RadioTelescopeDirectionEnum.ClockwiseOrNegative, RadioTelescopeAxisEnum.ELEVATION);
                    
                    if(result == MovementResult.Success)
                        logger.Info($"{Utilities.GetTimeStamp()}: Successfully started elevation positive jog.");
                    else if(result == MovementResult.StoppingCurrentMove)
                        logger.Info($"{Utilities.GetTimeStamp()}: Stopping current movement. Please wait until that movement has finished ending and try to jog again.");
                    else if(result == MovementResult.AlreadyMoving)
                        logger.Info($"{Utilities.GetTimeStamp()}: Elevation positive jog BLOCKED. Another manual script is already running.");
                    else
                    {
                        logger.Info($"{Utilities.GetTimeStamp()}: An error occurred trying to positive jog el: {result.ToString()}");
                        PushNotification.sendToAllAdmins("Jog Error", $"An error occurred trying to positive jog el: {result.ToString()}", rtController.PNEnabled);
                        EmailNotifications.sendToAllAdmins("Jog Error", $"An error occurred trying to positive jog el: {result.ToString()}", rtController.PNEnabled);
                    }
                }
                else
                {
                    MessageBox.Show("Speed is not in valid range, 0.0-2.0 RPMs. Please try again");
                }
            }
            else
            {
                MessageBox.Show("Invalid speed. Must be in RPMs between 0.0 and 2.0");
            }
        }

        private void plusElaButton_Up( object sender , MouseEventArgs e ) {
            //  Stop Move
            ExecuteCorrectStop();
        }

        private void subElaButton_Down(object sender, MouseEventArgs e ){
            if (Validator.ValidateSpeedTextOnly(speedTextBox.Text))
            {
                double speed = Convert.ToDouble(speedTextBox.Text);
                if (Validator.ValidateSpeed(speed))
                {
                    // Start CW Jog
                    MovementResult result = rtController.StartRadioTelescopeJog(speed, RadioTelescopeDirectionEnum.CounterclockwiseOrPositive, RadioTelescopeAxisEnum.ELEVATION);

                    if (result == MovementResult.Success)
                        logger.Info($"{Utilities.GetTimeStamp()}: Successfully started elevation negative jog.");
                    else if (result == MovementResult.StoppingCurrentMove)
                        logger.Info($"{Utilities.GetTimeStamp()}: Stopping current movement. Please wait until that movement has finished ending and try to jog again.");
                    else if (result == MovementResult.AlreadyMoving)
                        logger.Info($"{Utilities.GetTimeStamp()}: Elevation negative jog BLOCKED. Another manual script is already running.");
                    else
                    {
                        logger.Info($"{Utilities.GetTimeStamp()}: An error occurred trying to negative jog el: {result.ToString()}");
                        PushNotification.sendToAllAdmins("Jog Error", $"An error occurred trying to negative jog el: {result.ToString()}", rtController.PNEnabled);
                        EmailNotifications.sendToAllAdmins("Jog Error", $"An error occurred trying to negative jog el: {result.ToString()}", rtController.PNEnabled);
                    }
                }
                else
                {
                    MessageBox.Show("Speed is not in valid range, 0.0-2.0 RPMs. Please try again");
                }
            }
            else
            {
                MessageBox.Show("Invalid speed. Must be in RPMs between 0.0 and 2.0");
            }
        }

        private void subElaButton_Up( object sender , MouseEventArgs e ) {
            //  Stop Move
            ExecuteCorrectStop();
        }

        private void finalizeSettings_Click(object sender, EventArgs e)
        {
                acceptSettings = !acceptSettings;
                if (acceptSettings)
                {
                    this.finalizeSettingsButton.Text = "Edit Settings";
                }
                else
                {
                    this.finalizeSettingsButton.Text = "Finalize Settings";
                }
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Finalize settings button has been clicked");


                if (spectraEditActive)
                {
                    scanTypeComboBox.BackColor = System.Drawing.Color.DarkGray;
                    scanTypeComboBox.Enabled = false;
                    integrationStepCombo.BackColor = System.Drawing.Color.DarkGray;
                    integrationStepCombo.Enabled = false;
                    offsetVoltage.BackColor = System.Drawing.Color.DarkGray;
                    offsetVoltage.Enabled = false;
                    frequency.BackColor = System.Drawing.Color.DarkGray;
                    frequency.Enabled = false;
                    DCGain.BackColor = System.Drawing.Color.DarkGray;
                    DCGain.Enabled = false;
                    IFGainVal.BackColor = System.Drawing.Color.DarkGray;
                    IFGainVal.Enabled = false;

                    startScanButton.BackColor = System.Drawing.Color.LimeGreen;
                    startScanButton.Enabled = true;

                    spectraEditActive = false;

                    int caseSwitch = DCGain.SelectedIndex;

                    switch (caseSwitch)
                    {
                        case 1:
                            DCGainInput = SpectraCyberDCGainEnum.X1;
                            break;
                        case 2:
                            DCGainInput = SpectraCyberDCGainEnum.X5;
                            break;
                        case 3:
                            DCGainInput = SpectraCyberDCGainEnum.X10;
                            break;
                        case 4:
                            DCGainInput = SpectraCyberDCGainEnum.X20;
                            break;
                        case 5:
                            DCGainInput = SpectraCyberDCGainEnum.X50;
                            break;
                        case 6:
                            DCGainInput = SpectraCyberDCGainEnum.X60;
                            break;
                    }

                    caseSwitch = integrationStepCombo.SelectedIndex;

                    switch (caseSwitch)
                    {
                        case 1:
                            IntTimeInput = SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN;
                            break;
                        case 2:
                            IntTimeInput = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
                            break;
                        case 3:
                            IntTimeInput = SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN;
                            break;
                    }

                }
                else
                {
                    scanTypeComboBox.BackColor = System.Drawing.Color.White;
                    scanTypeComboBox.Enabled = true;
                    integrationStepCombo.BackColor = System.Drawing.Color.White;
                    integrationStepCombo.Enabled = true;
                    offsetVoltage.BackColor = System.Drawing.Color.White;
                    offsetVoltage.Enabled = true;
                    frequency.BackColor = System.Drawing.Color.White;
                    frequency.Enabled = true;
                    DCGain.BackColor = System.Drawing.Color.White;
                    DCGain.Enabled = true;
                    IFGainVal.BackColor = System.Drawing.Color.White;
                    IFGainVal.Enabled = true;

                    startScanButton.BackColor = System.Drawing.Color.DarkGray;
                    startScanButton.Enabled = false;

                    spectraEditActive = true;
                }
        }

        private void startScan_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Start Scan button has been clicked");
            
                int caseSwitch = scanTypeComboBox.SelectedIndex;

                switch (caseSwitch)
                {
                    case 1:
                        rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
                        rtController.RadioTelescope.SpectraCyberController.SetFrequency(Convert.ToDouble(frequency.Text));
                        rtController.RadioTelescope.SpectraCyberController.SetContinuumIntegrationTime(IntTimeInput);
                        rtController.RadioTelescope.SpectraCyberController.SetContinuumOffsetVoltage(Convert.ToDouble(offsetVoltage.Text));
                        rtController.RadioTelescope.SpectraCyberController.SetContGain(DCGainInput);
                        rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberIFGain(Convert.ToDouble(IFGainVal.Text));
                        break;
                    case 2:
                        rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
                        rtController.RadioTelescope.SpectraCyberController.SetFrequency(Convert.ToDouble(frequency.Text));
                        rtController.RadioTelescope.SpectraCyberController.SetSpectralIntegrationTime(IntTimeInput);
                        rtController.RadioTelescope.SpectraCyberController.SetSpectralOffsetVoltage(Convert.ToDouble(offsetVoltage.Text));
                        rtController.RadioTelescope.SpectraCyberController.SetSpecGain(DCGainInput);
                        rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberIFGain(Convert.ToDouble(IFGainVal.Text));
                        break;
                }
            
                finalizeSettingsButton.Enabled = false;

                startScanButton.Enabled = false;
                startScanButton.BackColor = System.Drawing.Color.DarkGray;

                stopScanButton.Enabled = true;
                stopScanButton.BackColor = System.Drawing.Color.Red;

                rtController.RadioTelescope.SpectraCyberController.StartScan(CurrentAppointment);
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Scan has started");

        }

        private void stopScan_Click(object sender, EventArgs e)
        {
            if (rtController.RadioTelescope.SpectraCyberController.Schedule.Mode == SpectraCyberScanScheduleMode.OFF ||
                rtController.RadioTelescope.SpectraCyberController.Schedule.Mode == SpectraCyberScanScheduleMode.UNKNOWN)
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] There is no scan to stop");
            else
            {
                rtController.RadioTelescope.SpectraCyberController.StopScan();
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Scan has stopped");
            }

            finalizeSettingsButton.Enabled = true;

            startScanButton.Enabled = true;
            startScanButton.BackColor = System.Drawing.Color.LimeGreen;

            stopScanButton.Enabled = false;
            stopScanButton.BackColor = System.Drawing.Color.DarkGray;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (integrationStepCombo.SelectedIndex == 0)
            {
                finalizeSettingsButton.Enabled = false;
            }
            if (allScanInputsValid())
            {
                finalizeSettingsButton.Enabled = true;
            }
            formData.integrationStepIndex = integrationStepCombo.SelectedIndex;

        }

        private void scanTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scanTypeComboBox.SelectedIndex == 0)
            {
                finalizeSettingsButton.Enabled = false;
            }
            if (allScanInputsValid())
            {
                finalizeSettingsButton.Enabled = true;
            }
            formData.spectraCyberScanIndex = scanTypeComboBox.SelectedIndex;
        }


        private void DCGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DCGain.SelectedIndex == 0)
            {
                finalizeSettingsButton.Enabled = false;
            }
            if (allScanInputsValid())
            {
                finalizeSettingsButton.Enabled = true;
            }
            formData.DCGainIndex = DCGain.SelectedIndex;
            
        }

        private void frequency_TextChanged(object sender, EventArgs e)
        {
            frequencyValid = Validator.ValidateFrequency(frequency.Text);
            if (!frequencyValid)
            {
                finalizeSettingsButton.Enabled = false;
                frequency.BackColor = System.Drawing.Color.Yellow;
                Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
                {
                   this.frequencyToolTip.Show("Frequency must be a non-negative\n " +
                "value, in kilohertz (>= 0 kHz)", lblFrequency);
                });
                
            }
            else
            {
                frequency.BackColor = System.Drawing.Color.White;
                Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
                {
                    this.frequencyToolTip.Hide(lblFrequency);
                });
            }
            if(allScanInputsValid())
            {
                finalizeSettingsButton.Enabled = true;
            }
            formData.frequency = frequency.Text;
        }

        private void offsetVoltage_TextChanged(object sender, EventArgs e)
        {
            offsetVoltValid = Validator.ValidateOffsetVoltage(offsetVoltage.Text);
            if (!offsetVoltValid)
            {
                finalizeSettingsButton.Enabled = false;
                offsetVoltage.BackColor = System.Drawing.Color.Yellow;
                Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
                {
                    this.offsetVoltageToolTip.Show("Offset voltage must be\n " +
                        "between 0 - 4.095 Volts", label12);
                });
                
            }
            else
            {
                offsetVoltage.BackColor = System.Drawing.Color.White;
                Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
                {
                    this.offsetVoltageToolTip.Hide(label12);
                });
                
            }
            if (allScanInputsValid())
            {
                finalizeSettingsButton.Enabled = true;
            }
            formData.offsetVoltage = offsetVoltage.Text;
        }

        private void IFGainVal_TextChanged(object sender, EventArgs e)
        {
            IFGainValid = Validator.ValidateIFGain(IFGainVal.Text);
            if (!IFGainValid)
            {
                finalizeSettingsButton.Enabled = false;
                IFGainVal.BackColor = System.Drawing.Color.Yellow;
                Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
                {
                    this.IFGainToolTip.Show("IFGain must be between\n " +
                                "10.00 and 25.75 decibles.", lblIFGain);
                });
               
            }
            else
            {
                IFGainVal.BackColor = System.Drawing.Color.White;
                Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
                {
                     this.IFGainToolTip.Hide(lblIFGain);
                });
                
            }
            if (allScanInputsValid())
            {
                finalizeSettingsButton.Enabled = true;
            }
            formData.IFGain = IFGainVal.Text;
        }

        private void speedTrackBar_Scroll(object sender, EventArgs e)
        {
            double actualSpeed = (double)speedTrackBar.Value / 10;
            speedTextBox.Text = actualSpeed.ToString();
            Utilities.WriteToGUIFromThread<FreeControlForm>(this, () =>
            {
                formData.speed = Double.Parse(speedTextBox.Text);
            });
            
        }

        private bool allScanInputsValid()
        {
            return (scanTypeComboBox.SelectedIndex != 0 && integrationStepCombo.SelectedIndex != 0 &&
                DCGain.SelectedIndex != 0 && offsetVoltValid && IFGainValid && frequencyValid);
          
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (rtController.RadioTelescope.SpectraCyberController.Schedule.Mode == SpectraCyberScanScheduleMode.OFF ||
                rtController.RadioTelescope.SpectraCyberController.Schedule.Mode == SpectraCyberScanScheduleMode.UNKNOWN)
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] There is no scan to stop");
            else
            {
                rtController.RadioTelescope.SpectraCyberController.StopScan();
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Scan has stopped");
            }
        }

        private void speedTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Enable/disable the telescope's software-stops when the check box is checked or unchecked
        /// </summary>
        private void SoftwareStopsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SoftwareStopsCheckBox.Checked)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to disable software stops?", "Software-Stops Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    rtController.EnableSoftwareStops = false;
                }
                else
                {
                    SoftwareStopsCheckBox.Checked = true;
                    rtController.EnableSoftwareStops = true;
                }
            }
            else
            {
                rtController.EnableSoftwareStops = true;
            }
        }
    }
}