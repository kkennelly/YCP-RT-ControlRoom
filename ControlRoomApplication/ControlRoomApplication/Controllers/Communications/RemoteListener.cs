﻿using System;
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
                logger.Debug(Utilities.GetTimeStamp() + ": Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                logger.Debug(Utilities.GetTimeStamp() + ": TCP Client connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                // Add try catch to reading
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    logger.Debug(Utilities.GetTimeStamp() + ": Received: " + data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] myWriteBuffer = null;

                    // Inform mobile command received 
                    // TODO: Surround any .Write in a try-catch
                    myWriteBuffer = Encoding.ASCII.GetBytes("Received command: " + data);
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

                    // if processing the data fails, report an error message
                    ParseTCPCommandResult parsedTCPCommandResult = ParseRLString(data);
                    if (parsedTCPCommandResult.parseTCPCommandResultEnum != ParseTCPCommandResultEnum.Success)
                    {
                        //logger.Error(Utilities.GetTimeStamp() + ": Processing data from tcp connection failed!");

                        // send back a failure response
                        logger.Info("Parsing command failed with ERROR: " + parsedTCPCommandResult.errorMessage);
                        myWriteBuffer = Encoding.ASCII.GetBytes("Parsing command failed with error: "+ parsedTCPCommandResult.errorMessage);
                    }
                    // else the parsing was successful, attempt to run the command
                    else
                    {
                        logger.Debug(Utilities.GetTimeStamp() + ": Successfully parsed command " +data+ ". beginning requested movement " +parsedTCPCommandResult.parsedString[1]+"...");
                        ExecuteTCPCommandResult executeTCPCommandResult = ExecuteRLCommand(parsedTCPCommandResult.parsedString);
                        // inform user of the result of command
                        if (executeTCPCommandResult.movementResult != MovementResult.Success)
                        {
                            logger.Debug(Utilities.GetTimeStamp() + ": Command " + data + " failed with error: " + executeTCPCommandResult.errorMessage);
                            myWriteBuffer = Encoding.ASCII.GetBytes("Command " + data + " failed with error: " + executeTCPCommandResult.errorMessage);
                        }
                        else
                        {
                            logger.Debug(Utilities.GetTimeStamp() + ": SUCCESSFULLY COMPLETED COMMAND: " + data);
                            // send back a success response -- finished command
                            myWriteBuffer = Encoding.ASCII.GetBytes("SUCCESSFULLY COMPLETED COMMAND: " + data);
                        }
                    }

                    // Send message back -- send final state
                    // Surround stream writes in a try catch
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                }

                // Shutdown and end connection
                client.Close();
            }
        }

        public bool RequestToKillTCPMonitoringRoutine()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Killing TCP Monitoring Routine");

            KeepTCPMonitoringThreadAlive = false;

            try
            {
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

        private ExecuteTCPCommandResult ExecuteRLCommand(String[] splitCommandString)
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
            if (version == 1.0)
            {
                // command is placed after pike and before colon; get it here
                // <VERSION> | <COMMANDTYPE> | <NAME<VALUES>> | TIME
                // TODO: use command instead of "contains" and "indexOf"
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
                        return new ExecuteTCPCommandResult(result, TCPCommunicationConstants.SCRIPT_ERR + script);
                    }
                    else
                    {
                        // everything was successful
                        return new ExecuteTCPCommandResult(result);
                    }
                }
                else if (command=="STOP_RT")
                {
                    bool success = rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    if (success) return new ExecuteTCPCommandResult(MovementResult.Success);
                    else return new ExecuteTCPCommandResult(MovementResult.TimedOut, TCPCommunicationConstants.ALL_STOP_ERR); 
                    // Uses a semaphore to acquire lock so a false means it has timed out or cannot gain access to movement thread

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
                else if(command=="REQUEST")
                {
                    // TODO: implement request commands
                }

                // can't find a keyword then we return Invalid Command sent
                return new ExecuteTCPCommandResult(MovementResult.InvalidCommand, TCPCommunicationConstants.COMMAND_NOT_FOUND + command);
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
                return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString,TCPCommunicationConstants.MISSING_COMMAND_ARGS);
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
            try
            {
                version = Double.Parse(splitCommandString[TCPCommunicationConstants.VERSION_NUM]);
            }
            catch (Exception e)
            {
                String errMessage = TCPCommunicationConstants.VERSION_CONVERSION_ERR + e.Message;
                return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidVersion, splitCommandString, errMessage);

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
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_ORIENTATION_MOVE_PARAMS)
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
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_COMMAND_ARGS);
                    }
                    // If the true false value is not given, we don't know what to do. Return error
                    else if (splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] != "TRUE" &&
                        splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] != "FALSE")
                    {
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_SET_OVERRIDE_ARG);
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
                        return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString, TCPCommunicationConstants.MISSING_SENSOR_INIT_ARGS);
                    }

                    // Retrieve sensor init values from the comma separated portion of the string
                    string[] splitData = splitCommandString[TCPCommunicationConstants.SENSOR_INIT_VALUES].Split(',');
                    // there should be 10, if not, no bueno
                    if (splitData.Length != 10) return new ParseTCPCommandResult(ParseTCPCommandResultEnum.MissingCommandArgs, splitCommandString,TCPCommunicationConstants.MISSING_SENSOR_INIT_ARGS);
                    else { }

                    break;
                case "REQUEST":
                    // TODO: implement request commands
                    break;

                // if we get here, command type not found
                default:
                    return new ParseTCPCommandResult(ParseTCPCommandResultEnum.InvalidCommandType, splitCommandString, TCPCommunicationConstants.COMMAND_NOT_FOUND);
              
            }


            // else all parsing was successful, return and inform client
            return new ParseTCPCommandResult(ParseTCPCommandResultEnum.Success, splitCommandString);
        }
    }

   


}
