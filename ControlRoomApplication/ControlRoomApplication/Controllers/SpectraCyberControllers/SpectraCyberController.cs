using System;
using System.IO.Ports;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers
{
    public class SpectraCyberController : AbstractSpectraCyberController
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool SerialCommsFailed;

        public SpectraCyberController(SpectraCyber spectraCyber) : base(spectraCyber) { }

        public override bool BringUp()
        {
            try
            {
                ((SpectraCyber)SpectraCyber).SerialPort = new SerialPort(
                ((SpectraCyber)SpectraCyber).CommPort,
                AbstractSpectraCyberConstants.BAUD_RATE,
                AbstractSpectraCyberConstants.PARITY_BITS,
                AbstractSpectraCyberConstants.DATA_BITS,
                AbstractSpectraCyberConstants.STOP_BITS
                )
                {
                    ReadTimeout = AbstractSpectraCyberConstants.TIMEOUT_MS,
                    WriteTimeout = AbstractSpectraCyberConstants.TIMEOUT_MS
                };

                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Created serial port connection");
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Failed creating serial port connection.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            try
            {
                ((SpectraCyber)SpectraCyber).SerialPort.Open();

                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Serial port has been opened");
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException
                    || e is InvalidOperationException
                    || e is ArgumentOutOfRangeException
                    || e is ArgumentException
                    || e is UnauthorizedAccessException)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Failed opening serial communication.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            try
            {
                // Initialize thread and start it
                CommunicationThread = new Thread(() => RunCommunicationThread());
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] The communication thread has been assigned");

                CommunicationThread.Start();
                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] The communication thread has started");
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Failed creating communication thread.");
                    return false;
                }
                else if (e is ThreadStartException || e is OutOfMemoryException)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Failed starting communication thread.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Successfully started SpectraCyber communication and communication thread.");
            return true;
        }

        public override bool BringDown()
        {
            try
            {
                if (((SpectraCyber)SpectraCyber).SerialPort != null)
                {
                    ((SpectraCyber)SpectraCyber).SerialPort.Close();
                }

                logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] The serial port has been closed");
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException)
                {
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            KillCommunicationThreadAndWait();

            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Successfully killed SpectraCyber communication and communication thread.");
            return true;
        }

        // Submit a command and return a response
        protected override void SendCommand(SpectraCyberRequest request, ref SpectraCyberResponse response)
        {
            // If the request is empty, don't process
            if (request.IsEmpty())
            {
                return;
            }

            try
            {
                // Attempt to write the command to the serial port
                ((SpectraCyber)SpectraCyber).SerialPort.Write(request.CommandString);
            }
            catch (Exception)
            {
                // Something went wrong, return the response
                SerialCommsFailed = true;
                return;
            }

            // Command was successfully sent through serial communication
            response.RequestSuccessful = true;

            // Give the SpectraCyber some time to process the command
            Thread.Sleep(AbstractSpectraCyberConstants.WAIT_TIME_MS);

            // Check for any significant cases
            switch (request.CommandType)
            {
                // Termination, safely end communication
                case SpectraCyberCommandTypeEnum.TERMINATE:
                    BringDown();
                    break;

                // This command type is unused
                case SpectraCyberCommandTypeEnum.SCAN_STOP:
                    break;

                // Purge the serial buffer
                case SpectraCyberCommandTypeEnum.DATA_DISCARD:
                    ((SpectraCyber)SpectraCyber).SerialPort.DiscardInBuffer();
                    break;
                
                //
                // Do nothing by default
                //
            }

            // If the request expects a reply back, capture the data and attach it to the response
            if (request.WaitForReply)
            {
                // Reponse's data is valid (assuming no exceptions are thrown)
                response.Valid = true;

                try
                {
                    // Create a character array in which to store the buffered characters
                    string hexString;

                    // Read a number of characters in the buffer
                    char[] charInBuffer = new char[AbstractSpectraCyberConstants.BUFFER_SIZE];

                    int length = -1;
                    try
                    {
                        length = ((SpectraCyber)SpectraCyber).SerialPort.Read(charInBuffer, 0, request.CharsToRead);
                    }
                    catch (Exception)
                    {
                        // Something went wrong, return the response
                        SerialCommsFailed = true;
                        response.Valid = false;
                        return;
                    }

                    // Set the time captured to be as close to the read as possible, in case it's valid
                    response.DateTimeCaptured = DateTime.UtcNow;

                    // Clip the string to the exact number of bytes read
                    if (AbstractSpectraCyberConstants.CLIP_BUFFER_RESPONSE && (length != AbstractSpectraCyberConstants.BUFFER_SIZE))
                    {
                        char[] actual = new char[length];

                        for (int i = 0; i < length; i++)
                        {
                            actual[i] = charInBuffer[i];
                        }

                        hexString = new string(actual);
                    }

                    // Leave the string how it is, with the possibility of trailing chararacters being "0"
                    else
                    {
                        hexString = new string(charInBuffer);
                    }

                    // Set the SerialIdentifier, as heard (but not necessarily expected)
                    response.SerialIdentifier = hexString[0];

                    // Check to see that replyString's first character is what was expected
                    if (response.SerialIdentifier != request.ResponseIdentifier)
                    {
                        throw new Exception();
                    }

                    // Convert the hex string into an int
                    response.DecimalData = HexStringToInt(hexString.Substring(1));
                }
                catch (Exception e)
                {
                    // Something went wrong, the response isn't valid
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Failed to receive a response: " + e.ToString());
                    response.Valid = false;
                }
            }

            // Clear the input buffer
            ((SpectraCyber)SpectraCyber).SerialPort.DiscardInBuffer();
        }

        // Test if the physical SpectraCyber is alive, while making sure to not interrupt the schedule
        public override bool TestIfComponentIsAlive()
        {
            // If the SpectraCyber has already told us it failed, then it's clearly not alive
            if (SerialCommsFailed)
            {
                return false;
            }

            switch (Schedule.GetMode())
            {
                case SpectraCyberScanScheduleMode.UNKNOWN:
                case SpectraCyberScanScheduleMode.OFF:
                    return DoSpectraCyberScan().Valid;

                case SpectraCyberScanScheduleMode.SINGLE_SCAN:
                    return true;

                case SpectraCyberScanScheduleMode.CONTINUOUS_SCAN:
                    return true;

                case SpectraCyberScanScheduleMode.SCHEDULED_SCAN:
                    {
                        CommunicationMutex.WaitOne();
                        DateTime LastConsumeTick = Schedule.GetLastConsumeTick();
                        int TimeRemainingMS = Schedule.TimeUntilReadyMS();
                        int ScanIntervalMS = Schedule.GetScanIntervalMS();
                        CommunicationMutex.ReleaseMutex();

                        if ((DateTime.UtcNow - LastConsumeTick).TotalMilliseconds < HeartbeatConstants.INTERFACE_CHECK_IN_RATE_MS)
                        {
                            // It was operable relatively recently, and the scheduled aspects of it means the SerialCommsFailed flag will go
                            // high if it does fail, so assume it's ok for now
                            return true;
                        }
                        else if (TimeRemainingMS > AbstractSpectraCyberConstants.WAIT_TIME_MS)
                        {
                            // The amount of time the schedule is waiting to do another scan is theoretically enough time for the SpectraCyber
                            // to do another scan and be ready for the scheduled scan, so do another scan to check its status
                            return DoSpectraCyberScan().Valid;
                        }
                        else if (ScanIntervalMS < (2 * AbstractSpectraCyberConstants.WAIT_TIME_MS))
                        {
                            // There's no time to scan and let the schedule resume normally - again, assume the SerialPortCommsFailed flag will
                            // catch failures
                            return true;
                        }
                        else
                        {
                            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberController] Unpredicted scheduling combination...");
                            return true;
                        }
                    }

                default:
                    throw new ArgumentException("Illegal type of SpectraCyber scan schedule mode: " + Schedule.GetMode().ToString());
            }
        }
    }
}