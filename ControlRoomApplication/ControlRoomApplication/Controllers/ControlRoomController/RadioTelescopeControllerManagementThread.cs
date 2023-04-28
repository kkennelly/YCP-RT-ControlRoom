using System;
using System.Threading;
using System.Collections.Generic;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using System.Net;
using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ControlRoomApplication.Util;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Constants;
using System.Linq;
using ControlRoomApplication.Controllers.SensorNetwork.Simulation;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeControllerManagementThread
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadioTelescopeController RTController { get; private set; }
        public Appointment AppointmentToDisplay { get; private set; }

        public Thread ManagementThread;
        private Mutex ManagementMutex;
        private volatile bool KeepThreadAlive;
        private volatile bool InterruptAppointmentFlag;
        private Orientation _NextObjectiveOrientation;

        public RemoteListener TCPListener { get; }

        public List<Override> ActiveOverrides;
        public List<Sensor> Sensors;

        private bool OverallSensorStatus;

        private bool endAppt = false;

        Appointment OldAppointment = new Appointment();
        Appointment NextAppointment = new Appointment();

        bool safeTel = false;

        private int timeoutCounter = 0;

        // List to store attachment paths for sending the email containing all RFData CSV's for an appointment and it's calibration sequence
        private List<string> attachmentPath = new List<string>();

        public Orientation NextObjectiveOrientation
        {
            get
            {
                return _NextObjectiveOrientation;
            }
            set
            {
                ManagementMutex.WaitOne();
                _NextObjectiveOrientation = value;
                ManagementMutex.ReleaseMutex();
            }
        }
        
        public int RadioTelescopeID
        {
            get
            {
                return RTController.RadioTelescope.Id;
            }
        }

        public bool Busy
        {
            get
            {
                if (!ManagementMutex.WaitOne(100))
                {
                    return true;
                }

                bool busy = _NextObjectiveOrientation != null;
                ManagementMutex.ReleaseMutex();

                return busy;
            }
        }

        public RadioTelescopeControllerManagementThread(RadioTelescopeController controller)
        {
            RTController = controller;

            ManagementThread = new Thread(new ParameterizedThreadStart(SpinRoutine))
            {
                Name = "RTControllerManagementThread (ID=" + RadioTelescopeID.ToString() + ")"
            };

            ManagementMutex = new Mutex();
            KeepThreadAlive = false;
            _NextObjectiveOrientation = null;
            InterruptAppointmentFlag = false;

            ActiveOverrides = new List<Override>();
            Sensors = new List<Sensor>();
            OverallSensorStatus = true;

            Sensors.Add(new Sensor(SensorItemEnum.WIND, SensorStatusEnum.NORMAL));

            // Add the eleveation aboslute encoder and the counterbalance accelerometer to the list of sensors to check for failures
            Sensors.Add(new Sensor(SensorItemEnum.ELEVATION_ABS_ENCODER, SensorStatusEnum.NORMAL));
            Sensors.Add(new Sensor(SensorItemEnum.COUNTER_BALANCE_VIBRATION, SensorStatusEnum.NORMAL));

            // Commented out because we will not be using this functionality in the future.
            // We will switch to connecting to a server on the cloud
            // Kate Kennelly 2/14/2020
            // TCPListener = new RemoteListener(8090, IPAddress.Parse("10.127.7.112"), controller);
        }

        public bool Start(RadioTelescopeController controller)
        {
            KeepThreadAlive = true;

            try
            {
                // Sensors.Add(new Sensor(SensorItemEnum.WIND_SPEED, SensorStatusEnum.NORMAL));

                ManagementThread.Start(controller);
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public void RequestToKill()
        {
            KeepThreadAlive = false;
        }

        public void KillWithHardInterrupt()
        {
            KeepThreadAlive = false;
            InterruptAppointmentFlag = true;
        }

        public bool WaitToJoin()
        {
            try
            {
                ManagementThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public void InterruptOnce()
        {
            InterruptAppointmentFlag = true;
        }

        private void SpinRoutine(Object controller)
        {
            RadioTelescopeController c = (RadioTelescopeController)controller;
            bool KeepAlive = KeepThreadAlive;

            // Let MCU connect
            Thread.Sleep(5000);
            RTController.RadioTelescope.PLCDriver.ResetMCUErrors();

            while (KeepAlive)
            {
                if(c.inclementWeather)
                {
                    Thread.Sleep(60000);
                    continue;
                }

                NextAppointment = WaitForNextAppointment();

                //Compares the ID of each appointment to see if they have changed
                if (NextAppointment != null && NextAppointment.Equals(OldAppointment))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Waiting for next Appointment");
                }

                if (NextAppointment != null)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Starting appointment...");
                    endAppt = false;

                    // Calibrate telescope before the appointment
                    if (NextAppointment._Type != AppointmentTypeEnum.FREE_CONTROL)
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Homing telescope... ");
                        RTController.HomeTelescope(MovementPriority.Appointment);

                        logger.Info(Utilities.GetTimeStamp() + ": Thermal Calibrating RadioTelescope Before Appointment");

                        DateTime startTreeCalTime, endTreeCalTime, startZenithCalTime, endZenithCalTime;

                        // Tree calibration 
                        RTController.ThermalCalibrateRadioTelescope(MovementPriority.Appointment);

                        startTreeCalTime = DateTime.Now;

                        StartReadingData(NextAppointment);
                        Thread.Sleep(MiscellaneousConstants.CALIBRATION_MS);
                        StopReadingRFData();

                        endTreeCalTime = DateTime.Now;

                        // Zenith calibration
                        RTController.MoveRadioTelescopeToOrientation(new Orientation(RTController.GetCurrentOrientation().azimuth, 90), MovementPriority.Appointment);

                        startZenithCalTime = DateTime.Now;
                        
                        StartReadingData(NextAppointment);
                        Thread.Sleep(MiscellaneousConstants.CALIBRATION_MS);
                        StopReadingRFData();

                        endZenithCalTime = DateTime.Now;

                        AppointmentCalibration apptCal = new AppointmentCalibration();
                        apptCal.appointment_id = NextAppointment.Id;
                        apptCal.calibration_type = AppointmentCalibrationTypeEnum.BEGINNING;
                        apptCal.tree_start_time = startTreeCalTime;
                        apptCal.tree_end_time = endTreeCalTime;
                        apptCal.zenith_start_time = startZenithCalTime;
                        apptCal.zenith_end_time = endZenithCalTime;
                        DatabaseOperations.AddAppointmentCalibrationData(apptCal);

                        // If the temperature is low and there's precipitation, dump the dish
                        if (RTController.RadioTelescope.WeatherStation.GetOutsideTemp() <= 40.00 && RTController.RadioTelescope.WeatherStation.GetTotalRain() > 0.00)
                        {
                            RTController.SnowDump(MovementPriority.Appointment);
                        }

                        // Add the beginning appointment calibration data to the file
                        string beginTreeAttachmentPath = "";
                        string beginZenithAttachmentPath = "";

                        string treeFname = startTreeCalTime.ToString("yyyyMMddHHmmss") + ("beginningTreeReading");
                        string zenithFname = startZenithCalTime.ToString("yyyyMMddHHmmss") + ("beginningZenithReading");
                        string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                        List<List<RFData>> data = DatabaseOperations.GetAppointmentCalibrationData(startTreeCalTime, endTreeCalTime, startZenithCalTime, endZenithCalTime);
                        try
                        {
                            beginTreeAttachmentPath = Path.Combine(currentPath, $"{treeFname}.csv");
                            DataToCSV.ExportToCSV(data[0], treeFname);

                            beginZenithAttachmentPath = Path.Combine(currentPath, $"{zenithFname}.csv");
                            DataToCSV.ExportToCSV(data[1], zenithFname);
                        }
                        catch (Exception e)
                        {
                            Console.Out.WriteLine($"Could not write data! Error: {e}");
                        }
                        // attach the ending calibration data to the email
                        attachmentPath.Add(beginTreeAttachmentPath);
                        attachmentPath.Add(beginZenithAttachmentPath);
                    }

                    // Create movement thread
                    Thread AppointmentMovementThread = new Thread(() => PerformRadioTelescopeMovement(NextAppointment))
                    {
                        Name = "RTControllerIntermediateThread (ID=" + RadioTelescopeID.ToString() + ")"
                    };

                    // Start movement thread
                    AppointmentMovementThread.Start();

                    if(NextAppointment._Type != AppointmentTypeEnum.FREE_CONTROL)
                    {
                        // End PLC thread & SpectraCyber 
                        AppointmentMovementThread.Join();
                        StopReadingRFData();
                        // Stow Telescope
                        EndAppointment();

                        // Calibrate at the end of the appointment
                        logger.Info(Utilities.GetTimeStamp() + ": Thermal Calibrating RadioTelescope After Appointment");

                        DateTime startTreeCalTime, endTreeCalTime, startZenithCalTime, endZenithCalTime;

                        // Zenith calibration
                        RTController.MoveRadioTelescopeToOrientation(new Orientation(RTController.GetCurrentOrientation().azimuth, 90), MovementPriority.Appointment);

                        startZenithCalTime = DateTime.Now;

                        StartReadingData(NextAppointment);
                        Thread.Sleep(MiscellaneousConstants.CALIBRATION_MS);
                        StopReadingRFData();

                        endZenithCalTime = DateTime.Now;

                        // Tree calibration 
                        RTController.ThermalCalibrateRadioTelescope(MovementPriority.Appointment);

                        startTreeCalTime = DateTime.Now;

                        StartReadingData(NextAppointment);
                        Thread.Sleep(MiscellaneousConstants.CALIBRATION_MS);
                        StopReadingRFData();

                        endTreeCalTime = DateTime.Now;

                        AppointmentCalibration endCal = new AppointmentCalibration();
                        endCal.appointment_id = NextAppointment.Id;
                        endCal.calibration_type = AppointmentCalibrationTypeEnum.END;
                        endCal.tree_start_time = startTreeCalTime;
                        endCal.tree_end_time = endTreeCalTime;
                        endCal.zenith_start_time = startZenithCalTime;
                        endCal.zenith_end_time = endZenithCalTime;
                        DatabaseOperations.AddAppointmentCalibrationData(endCal);

                        // If the temperature is low and there's precipitation, dump the dish
                        if (RTController.RadioTelescope.WeatherStation.GetOutsideTemp() <= 40.00 && RTController.RadioTelescope.WeatherStation.GetTotalRain() > 0.00)
                        {
                            RTController.SnowDump(MovementPriority.Appointment);
                        }

                        // Send the user a message containing the data from the RT end calibration

                        // Set email sender
                        string emailSender = "noreply@ycpradiotelescope.com";

                        // send message to appointment's user
                        SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_COMPLETION);

                        // Gather up email data
                        string subject = MessageTypeExtension.GetDescription(MessageTypeEnum.APPOINTMENT_COMPLETION);
                        string text = MessageTypeExtension.GetDescription(MessageTypeEnum.APPOINTMENT_COMPLETION);
                        string endTreeAttachmentPath = "";
                        string endZenithAttachmentPath = "";

                        string treeFname = startTreeCalTime.ToString("yyyyMMddHHmmss") + ("endTreeReading");
                        string zenithFname = startZenithCalTime.ToString("yyyyMMddHHmmss") + ("endZenithReading");
                        string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                        List<List<RFData>> data = DatabaseOperations.GetAppointmentCalibrationData(startTreeCalTime, endTreeCalTime, startZenithCalTime, endZenithCalTime);
                        try
                        {
                            endTreeAttachmentPath = Path.Combine(currentPath, $"{treeFname}.csv");
                            DataToCSV.ExportToCSV(data[0], treeFname);

                            endZenithAttachmentPath = Path.Combine(currentPath, $"{zenithFname}.csv");
                            DataToCSV.ExportToCSV(data[1], zenithFname);
                        }
                        catch (Exception e)
                        {
                            Console.Out.WriteLine($"Could not write data! Error: {e}");
                        }
                        // attach the ending calibration data to the email
                        attachmentPath.Add(endTreeAttachmentPath);
                        attachmentPath.Add(endZenithAttachmentPath);

                        // Send email with attachments
                        EmailNotifications.sendToUser(NextAppointment.User, subject, text, emailSender, attachmentPath, true);

                        // Clean up after yourself, otherwise you'll just fill up our storage space
                        for (int i = 0; i < attachmentPath.Count; i++)
                        {
                            DataToCSV.DeleteCSVFileWhenDone(attachmentPath[i]);
                        }
                    }
                    else
                    {
                        while (endAppt == false)
                        {
                            ;
                        }
                    }

                    logger.Info(Utilities.GetTimeStamp() + ": Appointment completed.");
                }
                else
                {
                    if (InterruptAppointmentFlag)
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Appointment interrupted in loading routine.");
                        ManagementMutex.WaitOne();
                        InterruptAppointmentFlag = false;
                        ManagementMutex.ReleaseMutex();
                    }

                    if (NextAppointment != null && NextAppointment.Equals(OldAppointment))
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Appointment does not have an orientation associated with it.");

                    }
                }

                KeepAlive = KeepThreadAlive;

                OldAppointment = NextAppointment;

                // Remove all attachment paths
                attachmentPath.Clear();

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Waits for the next chronological appointment's start time to be less than 10 minutes
        /// from the current time of day. Once we are 10 minutes from the appointment's start time
        /// we should begin operations such as calibration.
        /// </summary>
        /// <returns> An appointment object that is next in chronological order and is less than 10 minutes away from starting. </returns>
        private Appointment WaitForNextAppointment()
        {
            bool waiting = false;
            Appointment NextAppointment = DatabaseOperations.GetNextAppointment(RadioTelescopeID);
            TimeSpan diff;
            while (NextAppointment != null && (diff = NextAppointment.start_time - DateTime.UtcNow).TotalMinutes > 1)
            {
                NextAppointment = DatabaseOperations.GetNextAppointment(RadioTelescopeID);

                if (InterruptAppointmentFlag || (!KeepThreadAlive))
                {
                    return null;
                }

                // Delay between checking database for new appointments
                Thread.Sleep(100);

                if(!waiting) logger.Info(Utilities.GetTimeStamp() + ": Waiting for the next appointment to be within 1 minutes.");
                waiting = true;
            }

            if (NextAppointment != null && NextAppointment.Equals(OldAppointment))
            {
                logger.Info(Utilities.GetTimeStamp() + ": The next appointment is now within the correct timeframe.");
            }
            AppointmentToDisplay = NextAppointment;
            return NextAppointment;
        }

        /// <summary>
        /// Starts movement of the RT by updating the appointment status and
        /// then calling the RT controller to move the RT to the orientation
        /// it needs to go to.
        /// </summary>
        /// <param name="NextAppointment"> The appointment that is currently running. </param>
        private void PerformRadioTelescopeMovement(Appointment NextAppointment)
        {
            DateTime startTime = DateTime.UtcNow;

            NextAppointment._Status = AppointmentStatusEnum.IN_PROGRESS;
            DatabaseOperations.UpdateAppointment(NextAppointment);

            // send message to appointment's user
            SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_STARTED);

            // Loop through each second or minute of the appointment (depending on appt type)
            TimeSpan length = NextAppointment.end_time - startTime;
            double duration = NextAppointment._Type == AppointmentTypeEnum.FREE_CONTROL ? length.TotalSeconds : length.TotalMinutes;
            bool scanStarted = false;

            for (int i = 0; i <= (int) duration; i++)
            {
                // before we move, check to see if it is safe
                if (checkCurrentSensorAndOverrideStatus())
                {

                    // Get orientation for current datetime
                    DateTime datetime = NextAppointment._Type == AppointmentTypeEnum.FREE_CONTROL ? startTime.AddSeconds(i) : startTime.AddMinutes(i);
                    NextObjectiveOrientation = RTController.CoordinateController.CalculateOrientation(NextAppointment, datetime);

                    // Wait for datetime
                    while (DateTime.UtcNow < datetime)
                    {
                        if (InterruptAppointmentFlag)
                        {
                            logger.Info(Utilities.GetTimeStamp() + ": Interrupted appointment [" + NextAppointment.Id.ToString() + "] at " + DateTime.Now.ToString());
                            break;
                        }

                        //logger.Debug(datetime.ToString() + " vs. " + DateTime.UtcNow.ToString());
                        Thread.Sleep(1000);
                    }

                    if (InterruptAppointmentFlag)
                    {
                        break;
                    }

                    // Move to orientation
                    if (NextObjectiveOrientation != null)
                    {
                        // Kate - removed the check for azumith < 0 in the below if statement due to Todd's request
                        // Reason being, we should not have an azimuth below 0 be given to us. That check is in the
                        // method calling this!
                        if (NextObjectiveOrientation.azimuth < 0)
                        {
                            logger.Warn(Utilities.GetTimeStamp() + ": Invalid Appt: Az = " + NextObjectiveOrientation.azimuth + ", El = " + NextObjectiveOrientation.elevation);
                            InterruptAppointmentFlag = true;
                            break;
                        }

                        logger.Info(Utilities.GetTimeStamp() + ": Moving to Next Objective: Az = " + NextObjectiveOrientation.azimuth + ", El = " + NextObjectiveOrientation.elevation);
                        
                        MovementResult apptMovementResult = RTController.MoveRadioTelescopeToOrientation(NextObjectiveOrientation, MovementPriority.Appointment);

                        // Start SpectraCyber if the next appointment is NOT an appointment created by the control form
                        // This is to allow for greater control of the spectra cyber output from the control form
                        if (NextAppointment._Type != AppointmentTypeEnum.FREE_CONTROL && !scanStarted)
                        {
                            StartReadingData(NextAppointment);
                            scanStarted = true;
                        }

                        // If the movement result was anything other than success, it means the movement failed and something is wrong with
                        // the hardware.
                        // TODO: Talk to Todd about thresholds for this. (issue #388) Right now, it is cancelling the appointment if the movement
                        // returns back any single error. See the MovementResult enum for a list of the different errors. 
                        if (apptMovementResult == MovementResult.TimedOut)
                        {
                            timeoutCounter++;
                        }
                        else if(apptMovementResult == MovementResult.Success)
                        {
                            timeoutCounter = 0;
                        }

                        if (apptMovementResult != MovementResult.Success && timeoutCounter >= 5)
                        {
                            logger.Info($"{Utilities.GetTimeStamp()}: Appointment movement FAILED with the following error message: {apptMovementResult.ToString()}");
                            InterruptAppointmentFlag = true;
                        }

                        if (InterruptAppointmentFlag)
                        {
                            break;
                        }
                        
                         Thread.Sleep(100);

                        NextObjectiveOrientation = null;
                    }
                } 
                else
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Telescope stopped movement.");
                    i--;
                }
            }

            // Set email sender
            string emailSender = "noreply@ycpradiotelescope.com";

            if (InterruptAppointmentFlag)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Interrupted appointment [" + NextAppointment.Id.ToString() + "] at " + DateTime.Now.ToString());
                NextAppointment._Status = AppointmentStatusEnum.CANCELED;
                DatabaseOperations.UpdateAppointment(NextAppointment);
                NextObjectiveOrientation = null;
                InterruptAppointmentFlag = false;

                // send message to appointment's user
                SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_CANCELLED);

                string subject = MessageTypeExtension.GetDescription(MessageTypeEnum.APPOINTMENT_CANCELLED);
                string text = MessageTypeExtension.GetDescription(MessageTypeEnum.APPOINTMENT_CANCELLED);

                EmailNotifications.sendToUser(NextAppointment.User, subject, text, emailSender);
            }
            else
            {
                NextAppointment._Status = AppointmentStatusEnum.COMPLETED;
                DatabaseOperations.UpdateAppointment(NextAppointment);

                // send message to appointment's user that appointment was completed
                SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_COMPLETION);

                string appDataAttachmentPath = "";

                string fname = DateTime.Now.ToString("yyyyMMddHHmmss" + "appointmentData");
                string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                List<RFData> data = NextAppointment.RFDatas.Where(x => x.time_captured > startTime).ToList();
                
                try
                {
                    appDataAttachmentPath = Path.Combine(currentPath, $"{fname}.csv");
                    DataToCSV.ExportToCSV(data, fname);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine($"Could not write data! Error: {e}");
                }
                attachmentPath.Add(appDataAttachmentPath);
            }
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// </summary>
        public void EndAppointment()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Ending Appointment");
            endAppt = true;
            /*
            MovementResult result = RTController.StowRadioTelescope(MovementPriority.Appointment);
            if (result != MovementResult.Success)
            {
                logger.Error("Stowing telescope failed with message " + result);
            }
            */
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        public void StartReadingData(Appointment appt)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Starting Reading of RFData");
            RTController.RadioTelescope.SpectraCyberController.SetApptConfig(appt);
            RTController.RadioTelescope.SpectraCyberController.StartScan(appt);
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        private void StopReadingRFData()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Stoping Reading of RTData");
            RTController.RadioTelescope.SpectraCyberController.StopScan();
            RTController.RadioTelescope.SpectraCyberController.RemoveActiveAppointmentID();
            RTController.RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);
        }

        /// <summary>
        /// Checks to see if there are any sensors that are not overriden
        /// calls the stop telescope function if it is not safe
        /// Returns true if the telescope is safe to operate
        /// Returns false if the telescope is not safe to operate
        /// </summary>
        public bool checkCurrentSensorAndOverrideStatus()
        {
            // loop through all the current sensors

            CheckAndSwitchElevationDevice();
            
            foreach (Sensor curSensor in Sensors)
            {
                // if the sensor is in the ALARM state
                if (curSensor.Status == SensorStatusEnum.ALARM)
                {
                    // check to see if there is an override for that sensor
                    if (ActiveOverrides.Find(i => i.Item == curSensor.Item) == null)
                    {
                        // if not, return false
                        // we should not be operating the telescope
                        logger.Fatal(Utilities.GetTimeStamp() + ": Telescope in DANGER due to fatal sensors");
                        safeTel = false;
                        RTController.ExecuteRadioTelescopeImmediateStop(MovementPriority.GeneralStop);
                        OverallSensorStatus = false;
                        return false;
                    }                    
                }
            }

            if(safeTel == false)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Telescope in safe state.");
                safeTel = true;
            }
            OverallSensorStatus = true;
            return true;
        }

        /// <summary>
        /// Check the elevation absolute encoder and the counterbalance accelerometer statuses. If one fails, use the other. If both fail, then use the motor encoder. 
        /// </summary>
        public void CheckAndSwitchElevationDevice()
        {
            SensorStatuses sensorStatuses = RTController.RadioTelescope.SensorNetworkServer.SensorStatuses;

            // Check if a device has failed, then if so, change to the appropriate device
            if (sensorStatuses.ElevationAbsoluteEncoderStatus == Entities.DiagnosticData.SensorNetworkSensorStatus.Error ||
                CheckOutOfRangeAbsEncoder())
            {
                RTController.CanUseElevationAbsEncoder = false;
                RTController.UseElevationAbsEncoder = false;

                if (RTController.CanUseCounterbalance)
                {
                    RTController.UseCounterbalance = true;
                }
                else
                {
                    RTController.UseMotorEncoder = true;
                }
            }
            else
            {
                RTController.CanUseElevationAbsEncoder = true;
            }

            if (sensorStatuses.CounterbalanceAccelerometerStatus == Entities.DiagnosticData.SensorNetworkSensorStatus.Error)
            {
                RTController.CanUseCounterbalance = false;
                RTController.UseCounterbalance = false;

                if (RTController.CanUseElevationAbsEncoder)
                {
                    RTController.UseElevationAbsEncoder = true;
                }
                else
                {
                    RTController.UseMotorEncoder = true;
                }
            }
            else
            {
                RTController.CanUseCounterbalance = true;
            }
        }

        /// <summary>
        /// Check the current position of the elevation absolute encoder and return if it is out of range 
        /// </summary>
        /// <returns></returns>
        public bool CheckOutOfRangeAbsEncoder()
        {
            Double elevation = RTController.GetAbsoluteOrientation().elevation;
            return (elevation < 0 || elevation > 92);
        }
        /// <summary>
        /// Switch the device that is used to read elevation data in the event of one or both failing 
        /// </summary>
        /// <param name="sensor"></param>
        public void SwitchElevationDevice(Sensor sensor)
        {
            Sensor altSensor;
            if (sensor.Item == SensorItemEnum.ELEVATION_ABS_ENCODER)
            {
                RTController.UseElevationAbsEncoder = false;

                altSensor = Sensors.Find(Sensor => Sensor.Item == SensorItemEnum.COUNTER_BALANCE_VIBRATION);
                if (altSensor.Status != SensorStatusEnum.ALARM)
                {
                    RTController.UseCounterbalance = true;
                    RTController.UseMotorEncoder = false;
                }
                else if (altSensor.Status == SensorStatusEnum.ALARM)    // Both are failing, hence use the motor encoder 
                {
                    RTController.UseCounterbalance = false;
                    RTController.UseMotorEncoder = true;
                }
            }
            else    // Sensor is the Counterbalance Accelerometer 
            {
                RTController.UseCounterbalance = false;

                altSensor = Sensors.Find(Sensor => Sensor.Item == SensorItemEnum.ELEVATION_ABS_ENCODER);
                if (altSensor.Status != SensorStatusEnum.ALARM) {
                    RTController.UseElevationAbsEncoder = true;
                    RTController.UseMotorEncoder = false;
                }
                else if (altSensor.Status == SensorStatusEnum.ALARM)    // Both are failing, hence use the motor encoder 
                {
                    RTController.UseElevationAbsEncoder = false;
                    RTController.UseMotorEncoder = true;
                }
            }
        }
    }
}
