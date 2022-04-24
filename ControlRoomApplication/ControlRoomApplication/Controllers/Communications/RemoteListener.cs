using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using ControlRoomApplication.Database;
using ControlRoomApplication.Util;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Controllers.Communications.Enumerations;


namespace ControlRoomApplication.Controllers
{
    public class RemoteListener
    {

        private static readonly log4net.ILog logger =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TcpListener server = null;
        private Thread TCPMonitoringThread;
        private bool KeepTCPMonitoringThreadAlive;
        public RadioTelescopeController rtController;
        private ControlRoom controlRoom;

        private bool waitingForConn = true;

        public RemoteListener(int port, ControlRoom control)
        {
            logger.Debug(Utilities.GetTimeStamp() + ": Setting up remote listener");
            server = new TcpListener(port);

            controlRoom = control;

            // Start listening for client requests.
            server.Start();

            KeepTCPMonitoringThreadAlive = true;
            // start the listening thread
            TCPMonitoringThread = new Thread(new ThreadStart(TCPMonitoringRoutine));

            TCPMonitoringThread.Start();
        }

        public void TCPMonitoringRoutine()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[512];
            String data = null;

            // Enter the listening loop.
            while (KeepTCPMonitoringThreadAlive)
            {
                if (waitingForConn)
                {
                  logger.Debug(Utilities.GetTimeStamp() + ": Waiting for a connection... ");
                   waitingForConn = false;
                }                

                // Place each command in its own asynchronous thread so that we can run commands in parallel
                if (server.Pending())
                {
                    waitingForConn = true;
                    new Thread(() =>
                    {
                        using (TcpClient client = server.AcceptTcpClient())
                        {
                            logger.Debug(Utilities.GetTimeStamp() + ": TCP Client connected!");

                            data = null;
                            // Get a stream object for reading and writing
                            using (NetworkStream stream = client.GetStream())
                            {
                                int i;

                                // Loop to receive all the data sent by the client.
                                if ((i = readFromStream(stream, bytes)) != 0)
                                {
                                    // Reset the encrypted bool because we don't know if every command sent will be encrypted  (we will have to check each time) 
                                    bool encrypted = false;

                                    // Translate data bytes to ASCII string.
                                    data = Encoding.ASCII.GetString(bytes, 0, i);

                                    logger.Debug(Utilities.GetTimeStamp() + ": Received: " + data);

                                    // If the command is encrypted, decrypt the command and proceed as normal 
                                    Tuple<string, bool> dataPair = Utilities.CheckEncrypted(data);
                                    data = dataPair.Item1;
                                    encrypted = dataPair.Item2;

                                    string myWriteBuffer = null;

                                    // Inform mobile command received 
                                    writeBackToClient(data, stream, encrypted);

                                    // Process the data sent by the client.
                                    data = data.ToUpper();

                                    // if processing the data fails, report an error message
                                    ParseTCPCommandResult parsedTCPCommandResult = ParseRLString(data);
                                    if (parsedTCPCommandResult.parseTCPCommandResultEnum != ParseTCPCommandResultEnum.Success)
                                    {
                                        // send back a failure response
                                        logger.Info("Parsing command failed with ERROR: " + parsedTCPCommandResult.errorMessage);
                                        myWriteBuffer = "Parsing command failed with error: " + parsedTCPCommandResult.errorMessage;
                                        writeBackToClient(myWriteBuffer, stream, encrypted);
                                    }
                                    // else the parsing was successful, attempt to run the command
                                    else
                                    {
                                        // if script inform which script is running, else just command type
                                        if (parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE] == "SCRIPT")
                                        {
                                            logger.Debug(Utilities.GetTimeStamp() + ": Successfully parsed command " + data + ". Beginning requested movement " +
                                                parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE] + " " +
                                                parsedTCPCommandResult.parsedString[TCPCommunicationConstants.SCRIPT_NAME] + "...");
                                            string startedCommandMsg = "Successfully parsed command " + data + ". Beginning requested movement " +
                                                parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE] + " " +
                                                parsedTCPCommandResult.parsedString[TCPCommunicationConstants.SCRIPT_NAME] + "...";
                                            writeBackToClient(startedCommandMsg, stream, encrypted);
                                            // writeback eta to client
                                            int estMoveTime = ScriptETA(parsedTCPCommandResult.parsedString[TCPCommunicationConstants.SCRIPT_NAME]);
                                            logger.Info("Script " + parsedTCPCommandResult.parsedString[TCPCommunicationConstants.SCRIPT_NAME] +
                                                " has an estimated time of " + estMoveTime + " ms");
                                            writeBackToClient(("Script " + parsedTCPCommandResult.parsedString[TCPCommunicationConstants.SCRIPT_NAME] +
                                                " has an estimated time of " + estMoveTime + " ms"), stream, encrypted);

                                        }
                                        else
                                        {
                                            logger.Debug(Utilities.GetTimeStamp() + ": Successfully parsed command " + data + ". Beginning requested movement " +
                                                parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE] + "...");
                                            string startedCommandMsg = "Successfully parsed command " + data + ". Beginning requested movement " +
                                                parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE] + "...";
                                            writeBackToClient(startedCommandMsg, stream, encrypted);

                                            switch (parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE])
                                            {
                                                case "ORIENTATION_MOVE":
                                                    try
                                                    {
                                                        // Attempt to parse double values
                                                        double azAbs = Double.Parse(parsedTCPCommandResult.parsedString[TCPCommunicationConstants.ORIENTATION_MOVE_AZ]);
                                                        double elAbs = Double.Parse(parsedTCPCommandResult.parsedString[TCPCommunicationConstants.ORIENTATION_MOVE_EL]);

                                                        int mvmtTimeAbs = AbsoluteMovementETA(new Orientation(azAbs, elAbs));
                                                        writeBackToClient("ORIENTATION_MOVE TO AZ " + azAbs + " and EL " + elAbs + " has an estimated time of " + mvmtTimeAbs + " ms", stream, encrypted);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        writeBackToClient("An exception occurred attempting to parse AZ and/or EL values: " + e.Message, stream, encrypted);
                                                    }
                                                    break;

                                                case "RELATIVE_MOVE":
                                                    try
                                                    {
                                                        // Attempt to parse double values
                                                        double azRelative = Double.Parse(parsedTCPCommandResult.parsedString[TCPCommunicationConstants.RELATIVE_MOVE_AZ]);
                                                        double elRelative = Double.Parse(parsedTCPCommandResult.parsedString[TCPCommunicationConstants.RELATIVE_MOVE_EL]);
                                                        int mvmtTimeRelative = RelativeMovementETA(new Orientation(azRelative, elRelative));

                                                        writeBackToClient("RELATIVE_MOVE BY AZ " + azRelative + " and EL " + elRelative + " has an estimated time of " +
                                                            mvmtTimeRelative + " ms", stream, encrypted);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        writeBackToClient("An exception occurred attempting to parse AZ and/or EL values: " + e.Message, stream, encrypted);
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }

                                        // Now that we have finished parsing our command, execute it since it has been determined to be valid
                                        ExecuteTCPCommandResult executeTCPCommandResult = ExecuteRLCommand(parsedTCPCommandResult.parsedString);

                                        // inform user of the result of command
                                        if (executeTCPCommandResult.movementResult != MovementResult.Success)
                                        {
                                            logger.Debug(Utilities.GetTimeStamp() + ": Command " + data + " failed with error: " + executeTCPCommandResult.errorMessage);
                                            myWriteBuffer = "Command " + data + " failed with error: " + executeTCPCommandResult.errorMessage;
                                            writeBackToClient(myWriteBuffer, stream, encrypted);
                                        }
                                        else
                                        {
                                            switch (parsedTCPCommandResult.parsedString[TCPCommunicationConstants.COMMAND_TYPE])
                                            {
                                                // we write back different in the case of a request command. Otherwise, the default is just successfully completing a command
                                                case "REQUEST":
                                                    switch (parsedTCPCommandResult.parsedString[TCPCommunicationConstants.REQUEST_TYPE])
                                                    {
                                                        case "MVMT_DATA":
                                                            writeBackToClient(executeTCPCommandResult.errorMessage, stream, encrypted);
                                                            logger.Debug(Utilities.GetTimeStamp() + ": " + executeTCPCommandResult.errorMessage);
                                                            break;
                                                    }
                                                    break;

                                                default:
                                                    logger.Debug(Utilities.GetTimeStamp() + ": SUCCESSFULLY COMPLETED COMMAND: " + data);
                                                    // send back a success response -- finished command
                                                    myWriteBuffer = "SUCCESSFULLY COMPLETED COMMAND: " + data;
                                                    writeBackToClient(myWriteBuffer, stream, encrypted);
                                                    break;
                                            }
                                        }
                                    }

                                    // Shutdown and end connection
                                    client.Close();
                                    client.Dispose();
                                    stream.Close();
                                    stream.Dispose();
                                }
                            }
                        }
                    }).Start(); // begin our worker thread to execute our TCP command
                }
            }
        }

        public bool RequestToKillTCPMonitoringRoutine()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Killing TCP Monitoring Routine");

            KeepTCPMonitoringThreadAlive = false;

            try
            {
                server.Stop();
                TCPMonitoringThread.Join();
               
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

        public ExecuteTCPCommandResult ExecuteRLCommand(String[] splitCommandString)
        {
            // Convert version from string to double. This is the first value in our string before the "|" character.
            // From here we will direct to the appropriate parsing for said version
            double version = 0.0;
            try
            {
                version = Double.Parse(splitCommandString[TCPCommunicationConstants.VERSION_NUM]);
            }
            catch (Exception e)
            {
                String errMessage = TCPCommunicationConstants.VERSION_CONVERSION_ERR + e.Message;
                return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, errMessage);
 
            }

            // Use appropriate parsing for given version
            if (version >= 1.1)
            {
                string command = splitCommandString[TCPCommunicationConstants.COMMAND_TYPE];

                if (command == "RESET_MCU_BIT")
                {
                    if (rtController.ResetMCUErrors())
                    {
                        return new ExecuteTCPCommandResult(MCUResetResult.Success, null);
                    }
                    else
                    {
                        return new ExecuteTCPCommandResult(MCUResetResult.Failed, "Failed to reset MCU error bit.");
                    }
                }
                else if (command == "REQUEST")
                {
                    switch (splitCommandString[TCPCommunicationConstants.REQUEST_TYPE])
                    {
                        case "MVMT_DATA":
                            return new ExecuteTCPCommandResult(MovementResult.Success, GetMovementData(version));

                        default:
                            return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, TCPCommunicationConstants.INVALID_REQUEST_TYPE + splitCommandString[TCPCommunicationConstants.REQUEST_TYPE]);
                    }
                }
            }
            if (version >= 1.0)
            {
                // command is placed after pike and before colon; get it here
                // <VERSION> | <COMMANDTYPE> | <NAME<VALUES>> | TIME
                string command = splitCommandString[TCPCommunicationConstants.COMMAND_TYPE];
                
                if (command=="ORIENTATION_MOVE")
                {
                    // we have a move command coming in
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    double azimuth = 0.0;
                    double elevation = 0.0;
                    try
                    {
                        azimuth = Convert.ToDouble(splitCommandString[TCPCommunicationConstants.ORIENTATION_MOVE_AZ]);
                        elevation = Convert.ToDouble(splitCommandString[TCPCommunicationConstants.ORIENTATION_MOVE_EL]);
                    }
                    catch (Exception e)
                    {
                        String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                        return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, errMessage);
                    }
                   
                    logger.Debug(Utilities.GetTimeStamp() + ": Azimuth " + azimuth);
                    logger.Debug(Utilities.GetTimeStamp() + ": Elevation " + elevation);

                    Orientation movingTo = new Orientation(azimuth, elevation);

                    // check result of movement, if it fails we return the result type along with an error message.
                    // The command parse was successful however, so we indicate that
                    MovementResult result = rtController.MoveRadioTelescopeToOrientation(movingTo, MovementPriority.Manual);
                    if (result != MovementResult.Success)
                    {
                        return new ExecuteTCPCommandResult(result, TCPCommunicationConstants.ORIENTATION_MOVE_ERR + result.ToString());
                    }
                    else
                    // everything was successful
                    {
                        return new ExecuteTCPCommandResult(result);
                    }

                }
                else if (command=="RELATIVE_MOVE")
                {
                    
                    // we have a relative movement command
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    double azimuth = 0.0;
                    double elevation = 0.0;

                    try
                    {
                        azimuth = Convert.ToDouble(splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_AZ]);
                        elevation = Convert.ToDouble(splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_EL]);
                    }
                    catch (Exception e)
                    {
                        String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                        return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, errMessage);
                    }

                    logger.Debug(Utilities.GetTimeStamp() + ": Azimuth " + azimuth);
                    logger.Debug(Utilities.GetTimeStamp() + ": Elevation " + elevation);

                    Orientation movingBy = new Orientation(azimuth, elevation);

                    MovementResult result =  rtController.MoveRadioTelescopeByXDegrees(movingBy, MovementPriority.Manual);
                    if (result != MovementResult.Success)
                    {
                        // the actual movement failed
                        return new ExecuteTCPCommandResult(result, TCPCommunicationConstants.RELATIVE_MOVE_ERR + result.ToString());
                    }
                    else
                    {
                        // everything was successful
                        return new ExecuteTCPCommandResult(result);
                    }
                }
                else if (command=="SET_OVERRIDE")
                {
                    string sensorToOverride = splitCommandString[TCPCommunicationConstants.SENSOR_TO_OVERRIDE];
                    bool doOverride = splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] == "TRUE" ? true : false;
                    switch (sensorToOverride)
                    {
                        // Non-PLC Overrides
                        case "WEATHER_STATION":
                            controlRoom.weatherStationOverride = doOverride;
                            rtController.setOverride("weather station", doOverride);
                            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.WEATHER_STATION, doOverride);
                            break;

                        // PLC Overrides
                        case "MAIN_GATE":
                            rtController.setOverride("main gate", doOverride);
                            break;

                        // Proximity overrides
                        case "ELEVATION_LIMIT_0":
                            rtController.setOverride("elevation proximity (1)", doOverride);
                            break;

                        case "ELEVATION_LIMIT_90":
                            rtController.setOverride("elevation proximity (2)", doOverride);
                            break;

                        // Sensor network overrides
                        case "AZ_ABS_ENC":
                            rtController.setOverride("azimuth absolute encoder", doOverride);
                            break;

                        case "EL_ABS_ENC":
                            rtController.setOverride("elevation absolute encoder", doOverride);
                            break;

                        case "AZ_ACC":
                            rtController.setOverride("azimuth motor accelerometer", doOverride);
                            break;

                        case "EL_ACC":
                            rtController.setOverride("elevation motor accelerometer", doOverride);
                            break;

                        case "CB_ACC":
                            rtController.setOverride("counterbalance accelerometer", doOverride);
                            break;

                        case "AZIMUTH_MOT_TEMP":
                            rtController.setOverride("azimuth motor temperature", doOverride);
                            break;

                        case "ELEVATION_MOT_TEMP":
                            rtController.setOverride("elevation motor temperature", doOverride);
                            break;
                        
                        // If no case is reached, the sensor is not valid. Return appropriately
                        default: 
                            return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, TCPCommunicationConstants.INVALID_SENSOR_OVERRIDE + sensorToOverride);
                    }

                    return new ExecuteTCPCommandResult(MovementResult.Success);

                }
                else if (command=="SCRIPT")
                {
                    // we have a move command coming in
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // Retrieve script name used for switch case
                    string script = splitCommandString[TCPCommunicationConstants.SCRIPT_NAME];
                    MovementResult result = MovementResult.None;

                    logger.Debug(Utilities.GetTimeStamp() + ": Script " + script);

                    switch (script) {

                        case "DUMP":
                            result = rtController.SnowDump(MovementPriority.Manual);
                            break;

                        case "FULL_EV":
                            result = rtController.FullElevationMove(MovementPriority.Manual);
                            break;

                        case "THERMAL_CALIBRATE":
                            result = rtController.ThermalCalibrateRadioTelescope(MovementPriority.Manual);
                            break;

                        case "STOW":
                            result = rtController.MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow, MovementPriority.Manual);
                            break;

                        case "FULL_CLOCK":
                            result = rtController.MoveRadioTelescopeByXDegrees(new Orientation(360, 0), MovementPriority.Manual);
                            break;

                        case "FULL_COUNTER":
                            result = rtController.MoveRadioTelescopeByXDegrees(new Orientation(-360, 0), MovementPriority.Manual);
                            break;

                        case "HOME":
                            result = rtController.HomeTelescope(MovementPriority.Manual);
                            break;

                        case "HARDWARE_MVMT_SCRIPT":
                            result = rtController.ExecuteHardwareMovementScript(MovementPriority.Manual);
                            break;

                        default:
                            // If no command is found, result = invalid
                            result = MovementResult.InvalidCommand;
                            break;
                    }
                    
                    // Return based off of movement result
                    if (result != MovementResult.Success)
                    {
                        return new ExecuteTCPCommandResult(result, TCPCommunicationConstants.SCRIPT_ERR + result.ToString());
                    }
                    else
                    {
                        // everything was successful
                        return new ExecuteTCPCommandResult(result);
                    }
                }
                else if (command=="STOP_RT")
                {
                    MovementResult result = rtController.InterruptRadioTelescope();

                    return new ExecuteTCPCommandResult(result, TCPCommunicationConstants.ALL_STOP_ERR);

                }
                else if (command=="SENSOR_INIT")
                {

                    // Retrieve sensor init values from the comma separated portion of the string
                    string[] splitData = splitCommandString[TCPCommunicationConstants.SENSOR_INIT_VALUES].Split(',');

                    var config = rtController.RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig;

                    // Set all the sensors to their new initialization
                    config.AzimuthTemp1Init = splitData[(int)SensorInitializationEnum.AzimuthTemp].Equals("1");
                    config.ElevationTemp1Init = splitData[(int)SensorInitializationEnum.ElevationTemp].Equals("1");
                    config.AzimuthAccelerometerInit = splitData[(int)SensorInitializationEnum.AzimuthAccelerometer].Equals("1");
                    config.ElevationAccelerometerInit = splitData[(int)SensorInitializationEnum.ElevationAccelerometer].Equals("1");
                    config.CounterbalanceAccelerometerInit = splitData[(int)SensorInitializationEnum.CounterbalanceAccelerometer].Equals("1");
                    config.AzimuthEncoderInit = splitData[(int)SensorInitializationEnum.AzimuthEncoder].Equals("1");
                    config.ElevationEncoderInit = splitData[(int)SensorInitializationEnum.ElevationEncoder].Equals("1");

                    // Set the timeout values
                    config.TimeoutDataRetrieval = (int)(double.Parse(splitData[7]) * 1000);
                    config.TimeoutInitialization = (int)(double.Parse(splitData[8]) * 1000);

                    // Reboot
                    rtController.RadioTelescope.SensorNetworkServer.RebootSensorNetwork();

                    return new ExecuteTCPCommandResult(MovementResult.Success);
                }
                else if (command=="REQUEST")
                {
                    switch (splitCommandString[TCPCommunicationConstants.REQUEST_TYPE])
                    {
                        case "MVMT_DATA":
                            return new ExecuteTCPCommandResult(MovementResult.Success, GetMovementData(version));
                          
                        default:
                            return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, TCPCommunicationConstants.INVALID_REQUEST_TYPE + splitCommandString[TCPCommunicationConstants.REQUEST_TYPE]);
                    }
                }
                else
                {
                    // can't find a keyword then we return Invalid Command sent
                    return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, TCPCommunicationConstants.COMMAND_NOT_FOUND + command);
                }
            }
            
            // Version is not found; add new versions here
            else
            {
                return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, TCPCommunicationConstants.VERSION_NOT_FOUND + version);
            }
        }

        public ParseTCPCommandResult ParseRLString(string data)
        {
            // Break our string into different parts to retrieve respective pieces of command
            // Based on the command, we will choose what path to follow
            String[] splitCommandString = data.Trim().Split('|');

            // first check to make sure we have the mininum number of parameters before beginning parsing
            if (splitCommandString.Length < TCPCommunicationConstants.MIN_NUM_PARAMS)
            {
                return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS);
            }

            // proceed if valid
            for (int i = 0; i < splitCommandString.Length; i++)
            {
                splitCommandString[i] = splitCommandString[i].Trim();
            }
            logger.Info(Utilities.GetTimeStamp() + ": " + String.Join(" ", splitCommandString));

            // Convert version from string to double. This is the first value in our string before the "|" character.
            // From here we will direct to the appropriate parsing for said version
            double version = 0.0;
            bool foundVersion = false;
            try
            {
                version = Double.Parse(splitCommandString[TCPCommunicationConstants.VERSION_NUM]);
            }
            catch (Exception e)
            {
                String errMessage = TCPCommunicationConstants.VERSION_CONVERSION_ERR + e.Message;
                return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidVersion, splitCommandString, errMessage);

            }
            // ensure version exists
            foreach (string versionNum in TCPCommunicationConstants.ALL_VERSIONS_LIST)
            {
                if (versionNum == splitCommandString[TCPCommunicationConstants.VERSION_NUM].Trim())
                {
                    foundVersion = true;
                    break;
                }
            }
            // if version not found, return false
            if (!foundVersion)
            {
                return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidVersion, splitCommandString, TCPCommunicationConstants.VERSION_NOT_FOUND + version);
            }
  
            String command = splitCommandString[TCPCommunicationConstants.COMMAND_TYPE];

            switch (command)
            {
                case "ORIENTATION_MOVE":
                    // check to see if we have a valid number of parameters before attempting to parse
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_ORIENTATION_MOVE_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS + command);
                    }
                    // get azimuth and orientation
                    double absAzimuth = 0.0;
                    double absElevation = 0.0;
                    // Get the number from the AZ and EL portions: e.g AZ 30 --> we want the "30"
                    String[] absAzArr = splitCommandString[TCPCommunicationConstants.ORIENTATION_MOVE_AZ].Split(' ');
                    String[] absElArr = splitCommandString[TCPCommunicationConstants.ORIENTATION_MOVE_EL].Split(' ');

                    // If these numbers arent included return error
                    if (absAzArr.Length != 2 || absElArr.Length != 2)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_AZ_EL_ARGS);
                    }
                    else
                    {
                        try
                        {
                            absAzimuth = Convert.ToDouble(absAzArr[1]);
                            absElevation = Convert.ToDouble(absElArr[1]);
                        }
                        catch (Exception e)
                        {
                            String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                            return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidCommandArgs, splitCommandString, errMessage);
                        }
                        splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_AZ] = absAzArr[1];
                        splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_EL] = absElArr[1];
                    }
                    return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success,splitCommandString);
    
                case "RELATIVE_MOVE":
                    // check to see if we have a valid number of parameters before attempting to parse
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_RELATIVE_MOVE_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS + command);
                    }
                    // get azimuth and orientation
                    double relativeAzimuth = 0.0;
                    double relativeElevation = 0.0;
                    // Get the number from the AZ and EL portions: e.g AZ 30 --> we want the "30"
                    String[] relativeAzArr = splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_AZ].Split(' ');
                    String[] relativeElArr = splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_EL].Split(' ');

                    // If these numbers arent included return error
                    if (relativeAzArr.Length != 2 || relativeElArr.Length != 2)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_AZ_EL_ARGS);
                    }
                    else
                    {
                        try
                        {
                            relativeAzimuth = Convert.ToDouble(relativeAzArr[1]);
                            relativeElevation = Convert.ToDouble(relativeElArr[1]);
                        }
                        catch (Exception e)
                        {
                            String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                            return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidCommandArgs, splitCommandString, errMessage);
                        }

                        splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_AZ] = relativeAzArr[1];
                        splitCommandString[TCPCommunicationConstants.RELATIVE_MOVE_EL] = relativeElArr[1];
                    }
                    return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);

                case "SCRIPT":
                    // Check if we have the correct number of params in our string. If not, no need to begin parsing.
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_SCRIPT_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS + command);
                    }

                    string script = splitCommandString[TCPCommunicationConstants.SCRIPT_NAME];

                    foreach (string scriptInArr in TCPCommunicationConstants.SCRIPT_NAME_ARRAY)
                    {
                        // if we match a script, we know this script is valid. Return success
                        if (script == scriptInArr) return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);
                    }
                    // if none found, return
                    return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidScript, splitCommandString, TCPCommunicationConstants.SCRIPT_NOT_FOUND + script);
                    
                case "SET_OVERRIDE":
                    // Always check to see if we have our correct number of arguments for command type. Return false if not
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_SENSOR_OVERRIDE_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS + command);
                    }
                    // If the true false value is not given, we don't know what to do. Return error
                    else if (splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] != "TRUE" &&
                        splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] != "FALSE")
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_SET_OVERRIDE_ARG + splitCommandString[TCPCommunicationConstants.DO_OVERRIDE]);
                    }
                    else
                    {
                        // see if the sensor requested for override is a valid sensor
                        foreach (string sensor in TCPCommunicationConstants.SENSORS_ARRAY)
                        {
                            if(sensor == splitCommandString[TCPCommunicationConstants.SENSOR_TO_OVERRIDE]) return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);
                        }
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidCommandArgs, splitCommandString, TCPCommunicationConstants.INVALID_SENSOR_OVERRIDE + splitCommandString[TCPCommunicationConstants.SENSOR_TO_OVERRIDE]);
                    }
                        
                case "STOP_RT":
                    break;
                case "SENSOR_INIT":
                    // Check for valid number of parameters before continuing parsing
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_SENSOR_INIT_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS + command);
                    }

                    // Retrieve sensor init values from the comma separated portion of the string
                    string[] splitData = splitCommandString[TCPCommunicationConstants.SENSOR_INIT_VALUES].Split(',');
                    // there should be 9, if not, no bueno
                    if (splitData.Length != 9) return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString,TCPCommunicationConstants.MISSING_SENSOR_INIT_ARGS);
                    else { return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString); }

                   
                case "REQUEST":
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_REQUEST_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS + command);
                    }

                    switch (splitCommandString[TCPCommunicationConstants.REQUEST_TYPE])
                    {
                        case "MVMT_DATA":
                            return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);

                        default:
                            return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidRequestType, splitCommandString, TCPCommunicationConstants.INVALID_REQUEST_TYPE + splitCommandString[TCPCommunicationConstants.REQUEST_TYPE]);
                    }
                    break;

                case "RESET_MCU_BIT":
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_MCU_RESET_PARAMS)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS);
                    }
                    else if (version < 1.1)
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidVersion, splitCommandString, TCPCommunicationConstants.INVALID_VERSION);
                    }
                    else
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);
                    }
                    break;
                // if we get here, command type not found
                default:
                    return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidCommandType, splitCommandString, TCPCommunicationConstants.COMMAND_NOT_FOUND);
              
            }

            // else all parsing was successful, return and inform client
            return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);
        }

        /// <summary>
        /// Util method to handle error checking with strem.read/writes
        /// </summary>
        /// <param name="text">What you are sending back to the client</param>
        /// <param name="stream">Data stream to send the data back to the client</param>
        /// <param name="encrypted">Boolean value that represents whether or not the data should be encrypted before sending</param>
        public void writeBackToClient(string text, NetworkStream stream, bool encrypted)
        {
            byte[] textToBytes;

            if (encrypted)
            {
                textToBytes = Encoding.ASCII.GetBytes(AES.Encrypt(text, AESConstants.KEY, AESConstants.IV));
            }
            else
            {
                textToBytes = Encoding.ASCII.GetBytes(text);
            }

            try
            {
                stream.Write(textToBytes, 0, textToBytes.Length);
            }
            catch(Exception e)
            {
                logger.Error("An error occured when attempting to write back to the client: " + e.Message);
            }
        }

        public int readFromStream(NetworkStream stream, Byte[] buffer)
        {
            try
            {
                return stream.Read(buffer, 0, buffer.Length);
            }
            catch(Exception e)
            {
                logger.Error("An error occured reaading data from the client: " + e.Message);
             
            }
            return 0;
            
        }

        
        /// <summary>
        /// These ETA functions are used to calculate the estimated time a movement should take, and in turn sent back to the mobile app in order to display a progress bar.
        /// </summary>
        /// <param name="targetOrientation"></param>
        /// <returns></returns>
        public int AbsoluteMovementETA(Orientation targetOrientation)
        {
            // distance is degrees to steps for az/el
            // time is in milliseconds
            int EL_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_ELEVATION);
            int AZ_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_AZIMUTH);

            Orientation currentOrientation = rtController.GetCurrentOrientation();
            int positionTranslationAZ = ConversionHelper.DegreesToSteps(targetOrientation.Azimuth - currentOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int positionTranslationEL = ConversionHelper.DegreesToSteps((targetOrientation.Elevation - currentOrientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);

            int timeToMoveEl = MCUManager.EstimateMovementTime(EL_Speed, positionTranslationEL);
            int timeToMoveAz = MCUManager.EstimateMovementTime(AZ_Speed, positionTranslationAZ);

            // return the greater of the two times (we have to wait for longest mvmt)
            int timeToMove = timeToMoveEl > timeToMoveAz ? timeToMoveEl : timeToMoveAz;
            return timeToMove;

        }

        public int RelativeMovementETA(Orientation movingBy)
        {
            // distance is degrees to steps for az/el
            // time is in milliseconds
            int EL_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_ELEVATION);
            int AZ_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_AZIMUTH);

            int positionTranslationAZ = ConversionHelper.DegreesToSteps(movingBy.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int positionTranslationEL = ConversionHelper.DegreesToSteps(movingBy.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            int timeToMoveEl = MCUManager.EstimateMovementTime(EL_Speed, positionTranslationEL);
            int timeToMoveAz = MCUManager.EstimateMovementTime(AZ_Speed, positionTranslationAZ);

            // return the greater of the two times (we have to wait for longest mvmt)
            int timeToMove = timeToMoveEl > timeToMoveAz ? timeToMoveEl : timeToMoveAz;
            return timeToMove;
        }

        public int ScriptETA(string scriptType)
        {
            switch (scriptType.Trim())
            {
                case "DUMP":
                    return 0;  

                case "FULL_EV":
                    return AbsoluteMovementETA(new Orientation(0, 90)) + RelativeMovementETA(new Orientation(0, -90));

                case "THERMAL_CALIBRATE":
                    return 0;

                case "STOW":
                    return AbsoluteMovementETA(new Orientation(0, 90));

                case "FULL_CLOCK":
                    return AbsoluteMovementETA(new Orientation(360, 0));

                case "FULL_COUNTER":
                    return AbsoluteMovementETA(new Orientation(-360,0));

                case "HOME":
                    return AbsoluteMovementETA(new Orientation(0, 0));

                case "HARDWARE_MVMT_SCRIPT":
                    return 0;

                // If no command is found, invalid script. Return -1
                default:
                    return -1;
            }
            return -1;
        }
        /// <summary>
        /// Used for when the mobile app sends a command for REQUEST | MVMT_DATA. This will specify the current location of the telescope, and whether or not
        /// it is currently moving.
        /// </summary>
        /// <returns></returns>
        public string GetMovementData(double versionNum)
        {
            Orientation currentPos = rtController.GetCurrentOrientation();
            string currentlyMoving = rtController.RadioTelescope.PLCDriver.MotorsCurrentlyMoving().ToString().ToUpper();
            string sendBack = "MOVING: " + currentlyMoving + " | " + "AZ: " + currentPos.Azimuth + " | " + "EL: " + currentPos.Elevation;

            // Send back MCU data if TCP version is 1.1 or greater 
            if (versionNum >= 1.1)
            {
                sendBack += " | BIT_FLIPPED: " + Convert.ToString(rtController.RadioTelescope.PLCDriver.CheckMCUErrors().Count > 0).ToUpper();
            }

            return sendBack;
        }
    }
}
