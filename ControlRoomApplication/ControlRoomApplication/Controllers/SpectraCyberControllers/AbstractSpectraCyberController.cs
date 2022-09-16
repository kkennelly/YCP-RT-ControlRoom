using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Util;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractSpectraCyberController : HeartbeatInterface
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected RadioTelescope Parent { get; set; }
        protected AbstractSpectraCyber SpectraCyber { get; set; }
        public SpectraCyberScanSchedule Schedule { get; set; }
        public SpectraCyberConfigValues configVals;

        protected Thread CommunicationThread { get; set; }
        protected bool KillCommunicationThreadFlag { get; set; }
        protected Mutex CommunicationMutex;

        public AbstractSpectraCyberController(AbstractSpectraCyber spectraCyber) : base()
        {
            SpectraCyber = spectraCyber;
            Schedule = new SpectraCyberScanSchedule(SpectraCyberScanScheduleMode.OFF);
            KillCommunicationThreadFlag = false;
            CommunicationMutex = new Mutex();
            SpectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.CONTINUUM;
            configVals = new SpectraCyberConfigValues(SpectraCyberModeTypeEnum.CONTINUUM, 0, 0.3,
                                                        SpectraCyberDCGainEnum.X1, SpectraCyberDCGainEnum.X1, 10, SpectraCyberBandwidthEnum.SMALL_BANDWIDTH, 1200, 0, 0, -600);
        }

        public struct SpectraCyberConfigValues
        {
            public SpectraCyberModeTypeEnum spectraCyberMode;
            public double offsetVoltage;
            public double integrationStep;
            public double IFGain;
            public SpectraCyberDCGainEnum specGain;
            public SpectraCyberDCGainEnum contGain;
            public SpectraCyberBandwidthEnum bandwidth;
            public double frequency;
            public double rfData;
            public double scanTime;
            public double bandscan;

            public SpectraCyberConfigValues(SpectraCyberModeTypeEnum spectraCyberModeIN, double offsetVoltageIN, double integrationStepIN,
                                            SpectraCyberDCGainEnum specGainIN, SpectraCyberDCGainEnum contGainIN,
                                            double IFGainIN, SpectraCyberBandwidthEnum bandwidthIN, double frequencyIN, double rfDataIN, double scanTimeIN, double bandscanIn)
            {
                spectraCyberMode = spectraCyberModeIN;
                offsetVoltage = offsetVoltageIN;
                integrationStep = integrationStepIN;
                IFGain = IFGainIN;
                specGain = specGainIN;
                contGain = contGainIN;
                bandwidth = bandwidthIN;
                frequency = frequencyIN;
                rfData = rfDataIN;
                scanTime = scanTimeIN;
                bandscan = bandscanIn;
            }
        };

        public RadioTelescope GetParent()
        {
            return Parent;
        }

        public void SetParent(RadioTelescope rt)
        {
            Parent = rt;
        }

        public bool SetApptConfig(Appointment appt)
        {
            bool success = false;
            SetActiveAppointment(appt);
            SpectraCyberConfig config = appt.SpectraCyberConfig;
            SetSpectraCyberModeType(config._Mode);
            if(config._Mode == SpectraCyberModeTypeEnum.CONTINUUM)
            {
                success = SetContinuumIntegrationTime(config.IntegrationTime) && SetContinuumOffsetVoltage(config.OffsetVoltage);
            }
            else if(config._Mode == SpectraCyberModeTypeEnum.SPECTRAL)
            {
                success = SetSpectralOffsetVoltage(config.OffsetVoltage) && SetSpectralIntegrationTime(config.IntegrationTime);
            }
            else
            {
                // Unknown current mode type
                logger.Info(Utilities.GetTimeStamp() + ": [AbstractSpectraCyberController] ERROR: invalid SpectraCyber mode type: " + SpectraCyber.CurrentModeType.ToString());
            }
            return success;

        }

        public bool SetSpectraCyberIFGain(double ifGain)
        {
            if(ifGain < 10.0 || ifGain > 25.75)
            {
                logger.Info(Utilities.GetTimeStamp() + ": [AbstractSpectraCyberController] ERROR: invalid IF Gain value: " + ifGain);
                return false;
            }

            double adjustedGain = (ifGain - 10.0) / 0.25;

            string Command = "!A" + IntToHexString(Convert.ToInt32(adjustedGain));

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            configVals.IFGain = ifGain;

            return Response.RequestSuccessful;
        }

        public bool SetSpectraCyberDCGain(int dcgain, string identifier)
        {
            string Command = "!" + identifier + IntToHexString(dcgain);

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            return Response.RequestSuccessful;
        }

        public bool SetSpecGain(SpectraCyberDCGainEnum specGain)
        {
            // Spec Gain is K00X where x is the enum
            int gain = 0;

            if (specGain == SpectraCyberDCGainEnum.X1)
                gain = 0;
            else if (specGain == SpectraCyberDCGainEnum.X5)
                gain = 1;
            else if (specGain == SpectraCyberDCGainEnum.X10)
                gain = 2;
            else if (specGain == SpectraCyberDCGainEnum.X20)
                gain = 3;
            else if (specGain == SpectraCyberDCGainEnum.X50)
                gain = 4;
            else if (specGain == SpectraCyberDCGainEnum.X60)
                gain = 5;

            configVals.specGain = specGain;

            return SetSpectraCyberDCGain(gain, "K");
        }

        public bool SetContGain(SpectraCyberDCGainEnum contGain)
        {
            // Spec Gain is K00X where x is the enum
            int gain = 0;

            if (contGain == SpectraCyberDCGainEnum.X1)
                gain = 0;
            else if (contGain == SpectraCyberDCGainEnum.X5)
                gain = 1;
            else if (contGain == SpectraCyberDCGainEnum.X10)
                gain = 2;
            else if (contGain == SpectraCyberDCGainEnum.X20)
                gain = 3;
            else if (contGain == SpectraCyberDCGainEnum.X50)
                gain = 4;
            else if (contGain == SpectraCyberDCGainEnum.X60)
                gain = 5;

            configVals.specGain = contGain;

            return SetSpectraCyberDCGain(gain, "G");
        }

        public void SetSpectraCyberModeType(SpectraCyberModeTypeEnum type)
        {
            configVals.spectraCyberMode = type;
            SpectraCyber.CurrentModeType = type;
        }

        public void SetActiveAppointment(Appointment appt)
        {
            CommunicationMutex.WaitOne();
            SpectraCyber.ActiveAppointment = appt;
            CommunicationMutex.ReleaseMutex();
        }

        public void RemoveActiveAppointmentID()
        {
            SetActiveAppointment(null);
        }

        public void TestCommunication()
        {
            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.RESET,
                "!R000",
                true,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            string ResponseData = Response.SerialIdentifier + Response.DecimalData.ToString("X3");
            logger.Info(Utilities.GetTimeStamp() + ": [AbstractSpectraCyberController] Attempted RESET with command \"!R000\", heard back: " + ResponseData);
        }

        private bool SetSomeOffsetVoltage(double offset, char identifier)
        {
            if ((offset < 0.0) || (offset > 4.095))
            {
                logger.Info(Utilities.GetTimeStamp() + ": [AbstractSpectraCyberController] ERROR: input voltage outside of range [0, 4.095]");
                return false;
            }

            configVals.offsetVoltage = offset;

            int Magnitude = (int)(offset * 1000);
            string Command = "!" + identifier + IntToHexString(Magnitude);

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            return Response.RequestSuccessful;
        }

        public bool SetContinuumOffsetVoltage(double offset)
        {
            return SetSomeOffsetVoltage(offset, 'O');
        }

        public bool SetSpectralOffsetVoltage(double offset)
        {
            return SetSomeOffsetVoltage(offset, 'J');
        }

        private bool SetSomeIntegrationTime(SpectraCyberIntegrationTimeEnum time, char identifier)
        {
            string Command = "!" + identifier + "00" + SpectraCyberIntegrationTimeEnumHelper.GetValue(time);

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            return Response.RequestSuccessful;
        }

        public bool SetContinuumIntegrationTime(SpectraCyberIntegrationTimeEnum time)
        {
            if (time == SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN)
            {
                configVals.integrationStep = 0.3;
                Schedule.ScanDelayMS = 300;
            }
            else if (time == SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN)
            {
                configVals.integrationStep = 1.0;
                Schedule.ScanDelayMS = 1000;
            }
            else if (time == SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN)
            {
                configVals.integrationStep = 10.0;
                Schedule.ScanDelayMS = 10000;
            }
            return SetSomeIntegrationTime(time, 'I');
        }

        public bool SetSpectralIntegrationTime(SpectraCyberIntegrationTimeEnum time)
        {
            if (time == SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN)
            {
                configVals.integrationStep = 0.3;
                Schedule.ScanDelayMS = 300;
            }
            else if (time == SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN)
            {
                configVals.integrationStep = 0.5;
                Schedule.ScanDelayMS = 500;
            }
            else if (time == SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN)
            {
                configVals.integrationStep = 1.0;
                Schedule.ScanDelayMS = 1000;
            }
            return SetSomeIntegrationTime(time, 'L');
        }

        public bool SetFrequency(double frequency)
        {
            string Command = "!F" + IntToHexString(Convert.ToInt32(frequency));

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            configVals.frequency = frequency;
            configVals.bandscan = -1 * (configVals.frequency / 2);

            return Response.RequestSuccessful;
        }

        public bool SetBandwidth(SpectraCyberBandwidthEnum bandwidth)
        {
            // Our spectra cyber does not use this command
            /*
            string Command = "";

            if (bandwidth.GetValue().Equals("15Khz"))
            {
                Command = "!B000";

            }
            else if (bandwidth.GetValue().Equals("30Khz"))
            {
                Command = "!B001";
            }

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            configVals.bandwidth = bandwidth;

            return Response.RequestSuccessful;
            */
            return true;
        }

        // Perform a single scan, based on current mode
        public SpectraCyberResponse DoSpectraCyberScan()
        {
            SpectraCyberResponse Response = new SpectraCyberResponse();

            CommunicationMutex.WaitOne();
            SendCommand(GenerateCurrentDataRequest(), ref Response);
            CommunicationMutex.ReleaseMutex();

            return Response;
        }

        // Start scanning, keep doing so until requested to stop
        public void SingleScan()
        {
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetSingleMode();
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Start scanning, keep doing so until requested to stop
        public void StartScan(Appointment appt)
        {

            // set the spectra cyber active appointment so that rf data has an appointment to refer to
            SpectraCyber.ActiveAppointment = appt;

            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberAbstractController] Scan has been started");
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetContinuousMode();
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Stop scanning and return scan results
        public void StopScan()
        {
            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberAbstractController] Scan has been stopped");

            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetModeOff();
                configVals.scanTime = 0;
                configVals.bandscan = -1 * (configVals.frequency / 2);
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Start a scheduled scan interval
        public void ScheduleScan(int intervalMS, int delayMS, bool startAfterDelay)
        {
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetScheduledMode(intervalMS, delayMS, startAfterDelay);
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Generate a SpectraCyberRequest based on currentMode
        protected SpectraCyberRequest GenerateCurrentDataRequest(bool waitForReply = true, int numChars = 0)
        {
            if (numChars <= 0)
            {
                numChars = AbstractSpectraCyberConstants.BUFFER_SIZE;
            }

            // Based on the current mode, create the proper command string
            string commandString;

            if (SpectraCyber.CurrentModeType == SpectraCyberModeTypeEnum.CONTINUUM)
            {
                commandString = "!D000";
            }
            else if (SpectraCyber.CurrentModeType == SpectraCyberModeTypeEnum.SPECTRAL)
            {
                commandString = "!D001";
            }
            else
            {
                // Unknown current mode type
                throw new ArgumentException("Invalid SpectraCyber mode type: " + SpectraCyber.CurrentModeType.ToString());
            }

            return new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.GENERAL_COMMUNICATION,
                commandString,
                waitForReply,
                numChars
            );
        }

        public void RunCommunicationThread()
        {
            bool KeepRunningCommsThread = true;

            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] The scan schedule type is " + Schedule.GetMode());

            // Loop until the thread is attempting to be shutdown (don't directly reference SpectraCyber.KillCommunicationThreadFlag
            // because it can't be kept in the mutex's scope)
            while (KeepRunningCommsThread)
            {
                // Wait for the mutex to say it's safe to proceed
                CommunicationMutex.WaitOne();

                if (Schedule.PollReadiness())
                {
                    AddToRFDataDatabase(DoSpectraCyberScan(), SpectraCyber.ActiveAppointment);
                    Schedule.Consume();
                }

                // Tell the loop to break on its next pass (so the mutex is still released if the flag is high)
                KeepRunningCommsThread = !KillCommunicationThreadFlag;

                // Release the mutex
                CommunicationMutex.ReleaseMutex();
            }
        }

        // Implicitly kills the processing thread and waits for it to join before returning
        public void KillCommunicationThreadAndWait()
        {
            KillCommunicationThreadFlag = true;
            if(CommunicationThread.IsAlive) CommunicationThread.Join();
        }

        protected override bool KillHeartbeatComponent()
        {
            return BringDown();
        }

        private RFData AddToRFDataDatabase(SpectraCyberResponse spectraCyberResponse, Appointment appt)
        {
            RFData rfData = RFData.GenerateFrom(spectraCyberResponse);
            appt = DatabaseOperations.GetUpdatedAppointment(appt);
            rfData.Appointment = appt;
            rfData.Intensity = rfData.Intensity * MiscellaneousHardwareConstants.SPECTRACYBER_VOLTS_PER_STEP;

            // Add to database
            DatabaseOperations.AddRFData(rfData);

            configVals.rfData = rfData.Intensity;

            if (configVals.spectraCyberMode == SpectraCyberModeTypeEnum.SPECTRAL)
                if (configVals.bandscan > configVals.frequency / 2)
                    configVals.bandscan = -1 * (configVals.frequency / 2);
                else
                    configVals.bandscan = configVals.bandscan + MiscellaneousHardwareConstants.SPECTRACYBER_BANDWIDTH_STEP;
            else if (configVals.spectraCyberMode == SpectraCyberModeTypeEnum.CONTINUUM)
                configVals.scanTime = configVals.scanTime + configVals.integrationStep;

            return rfData;
        }

        public abstract bool BringUp();
        public abstract bool BringDown();
        protected abstract void SendCommand(SpectraCyberRequest request, ref SpectraCyberResponse response);

        protected static int HexCharToInt(char ch)
        {
            int baseVal = Convert.ToByte(ch);

            // Between [0-9]
            if (baseVal >= 48 && baseVal <= 57)
            {
                return baseVal - 48;
            }

            // Between [A-F]
            if (baseVal >= 65 && baseVal <= 70)
            {
                return baseVal - 55;
            }

            // Between [a-f]
            if (baseVal >= 97 && baseVal <= 102)
            {
                return baseVal - 87;
            }

            // Unknown
            return 0;
        }

        protected static int HexStringToInt(string hex, int length)
        {
            if (length == 0)
            {
                return 0;
            }

            if (length == 1)
            {
                return HexCharToInt(hex[0]);
            }

            return (int)(HexCharToInt(hex[0]) * Math.Pow(16, length - 1)) + HexStringToInt(hex.Substring(1), length - 1);
        }

        protected static int HexStringToInt(string hex)
        {
            return HexStringToInt(hex, hex.Length);
        }

        protected static string IntToHexString(int value, int minimumLength = 3)
        {
            string strHex = "0123456789ABCDEF";
            string strOutput = "";

            // First, encode the integer into hex
            while (value > 0)
            {
                strOutput = strHex[value % 16] + strOutput;
                value /= 16;
            }

            // Now, pad the string with zeros to make it the correct length
            for (int i = strOutput.Length; i < minimumLength; i++)
            {
                strOutput = "0" + strOutput;
            }

            // Return it
            return strOutput;
        }
    }
}