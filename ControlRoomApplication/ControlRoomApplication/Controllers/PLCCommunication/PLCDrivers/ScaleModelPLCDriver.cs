using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Entities;
using Newtonsoft.Json;
using static ControlRoomApplication.Constants.MCUConstants;

namespace ControlRoomApplication.Controllers {


    public class PLCConnector {
        /// <summary>
        /// Constructor for the PLCConnector. This constructor should be used for 
        /// connecting to a TCP connection at the localhost 127.0.0.1 address and port 8080.
        /// </summary>
        public PLCConnector() {
            ConnectionEndpoint = new IPEndPoint( IPAddress.Parse( PLCConstants.LOCAL_HOST_IP ) , PLCConstants.PORT_8080 );
            TCPClient = new TcpClient();
        }

        /// <summary>
        /// Constructor for the PLCConnector. This constructor should be used for
        /// connecting to a serial port connection.
        /// </summary>
        /// <param name="portName"> The serial port name that should be connected to. </param>
        public PLCConnector( string portName ) {
            try {
                SPort = new SerialPort();
                SPort.PortName = portName;
                SPort.BaudRate = PLCConstants.SERIAL_PORT_BAUD_RATE;
                SPort.Open();

                logger.Info( $"Serial port ({portName}) opened." );
            } catch(Exception) {
                logger.Error( "Serial port was already opened." );
            }
        }

        /// <summary>
        /// Constructor for the PLCConecctor. This constructor should be used for
        /// connecting to a TCP connection at the specified IP address and port.
        /// </summary>
        /// <param name="ip"> The IP address, in string format, that should be connected to. </param>
        /// <param name="port"> The port that should be connected to. </param>
        public PLCConnector( string ip , int port ) {
            // Initialize Connector with information passed in
            ConnectionEndpoint = new IPEndPoint( IPAddress.Parse( ip ) , port );
            TCPClient = new TcpClient();
        }

        /// <summary>
        /// Connects the control room application to the PLC software through a TCPConnection
        /// that is established over ethernet.
        /// </summary>
        /// <returns> Returns a bool indicating whether or not the connection established successfully. </returns>
        private bool ConnectToPLC() {
            // This is one of 3 connect methods that must be used to connect the client 
            // instance with the endpoint (IP address and port number) listed.
            TCPClient.Connect( ConnectionEndpoint );

            // This gets the stream that the client is connected to above.
            // Stream is how we will write our data back and forth between
            // the PLC.
            Stream = TCPClient.GetStream();

            logger.Info( $"Established TCP connection at ({ConnectionEndpoint.Address}, {ConnectionEndpoint.Port})." );
            return TCPClient.Client.Connected;
        }

        /// <summary>
        /// Writes a message to the network stream that is connecting this application
        /// to the application running the PLC hardware.
        /// </summary>
        /// <param name="message"> A string that represents the state of the object. </param>
        public void WriteMessage( string message ) {
            // Convert the message passed in into a byte[] array.
            Data = System.Text.Encoding.ASCII.GetBytes( message );

            // If the connection to the PLC is successful, get the NetworkStream 
            // that is being used and write to it.
            if(ConnectToPLC()) {
                try {
                    Stream = TCPClient.GetStream();

                    Stream.Write( Data , 0 , Data.Length );

                    logger.Info( "Sent message to PLC over TCP." );
                } catch(SocketException e) {
                    Console.WriteLine( $"Encountered a socket exception." );
                    logger.Error( $"There was an issue with the socket: {e.Message}." );
                }
            }
        }

        /// <summary>
        /// Receives messages from a TCP connection from the IPEndpoint that TCPClient
        /// is connected to.
        /// </summary>
        /// <returns> A string that indicates the state of the operation. </returns>
        public string ReceiveMessage() {
            // Create a new byte[] array and initialize the string
            // we will send back
            Data = new byte[256];
            string responseData = string.Empty;

            if(ConnectToPLC()) {
                int i;
                try {
                    while((i = Stream.Read( Data , 0 , Data.Length )) != 0) {
                        Message = System.Text.Encoding.ASCII.GetString( Data , 0 , i );
                    }

                    logger.Info( "Message received from PLC." );
                } catch(SocketException e) {
                    Console.WriteLine( "Encountered a socket exception." );
                    logger.Error( $"There was an issue with the socket {e.Message}" );
                } finally {
                    DisconnectFromPLC();
                    logger.Info( "Disconnected from PLC." );
                }
            }

            return Message;
        }

        /// <summary>
        /// Disconnects the TCP connection that was established to the PLC.
        /// </summary>
        public void DisconnectFromPLC() {
            // Call the dispose() method to close the stream and connection.
            TCPClient.Dispose();
            logger.Info( "Disposed of the TCP Client." );
        }

        //*** These methods will be for the arduino scale model. ***//

        /// <summary>
        /// Closes the serial port that was opened in SPort.
        /// </summary>
        public void CloseSerialPort() {
            SPort.Close();
            logger.Info( "Serial port has been closed." );
        }

        /// <summary>
        /// Gets a message from the specified serial port in SPort.
        /// </summary>
        /// <returns> Returns a string that was read from the serial port. </returns>
        public string GetSerialPortMessage() {
            Message = string.Empty;

            // Read all existing bytes in the stream.
            Message = SPort.ReadExisting();

            logger.Info( "Message received from Arduino." );
            return Message;
        }

        public bool SendSerialPortMessage( string jsonOrientation ) {
            Data = System.Text.Encoding.ASCII.GetBytes( jsonOrientation );
            Thread.Sleep( 10 );
            SPort.Write( Data , 0 , Data.Length );
            Thread.Sleep( 10 );
            logger.Info( "Message sent to Arduino" );
            return true;
        }

        // Getters/Setters for TCP/IP connection
        public TcpClient TCPClient { get; set; }
        public IPEndPoint ConnectionEndpoint { get; set; }
        public NetworkStream Stream { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }

        // Getters/Setters for Serial Port connection (for arduino scale model)
        public SerialPort SPort { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
    }

    public class ScaleModelPLCDriver : AbstractPLCDriver {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        public PLCConnector PlcConnector { get; set; }


        public ScaleModelPLCDriver( string local_ip , string MCU_ip , int MCU_port , int PLC_port ) :base( local_ip , MCU_ip, MCU_port, PLC_port ) {
            Console.WriteLine( MCU_ip );
        }

        public override MovementPriority CurrentMovementPriority { get; set; }

        public override void setregvalue(ushort adr, ushort value)
        {
            throw new NotImplementedException();
        }



        public override void HandleClientManagementThread() {}


        public override bool StartAsyncAcceptingClients() {
            return true;
        }

        public override bool RequestStopAsyncAcceptingClientsAndJoin() {
            return true;
        }

        public override void Bring_down() {
        }

        public override bool Test_Connection() {
            return true;
        }

        public override Orientation GetMotorEncoderPosition() {
            return new Orientation();
        }

        public override bool Cancel_move() {
            return true;

        }

        public override bool Configure_MCU( double startSpeedAzimuth , double startSpeedElevation , int homeTimeoutAzimuth , int homeTimeoutElevation ) {
            return true;
        }

        public override bool ControlledStop() {
            return true;

        }

        public override bool ImmediateStop() {
            return true;

        }

        public MovementResult SendRelativeMove(int SpeedAZ, int SpeedEL, int positionTranslationAZ, int positionTranslationEL) {
            // Move the scale model's azimuth motor on com3 and its elevation on com4
            // make sure there is a delay in this thread for enough time to have the arduino
            // move the first motor (azimuth)
            string jsonOrientation = JsonConvert.SerializeObject( new {
                CMD = "relMove",
                az =positionTranslationAZ,
                el = positionTranslationEL
            } );
            PlcConnector = new PLCConnector( "COM12" );
            PlcConnector.SendSerialPortMessage( jsonOrientation );

            // Wait for the arduinos to send back a response 
            // in the arduino code, as of milestone 2, the response is a string: "finished"
            var state = string.Empty;
            //state = PlcConnector.GetSerialPortMessage();
            PlcConnector.CloseSerialPort();
            // Print the state of the move operation to the console.
            //Console.WriteLine(state);

            return MovementResult.Success;
        }

        public override MovementResult RelativeMove(int programmedPeakSpeedAZInt, int programmedPeakSpeedELInt,  int positionTranslationAZ, int positionTranslationEL, Orientation targetOrientation) {
            /*
                    if(Plc.OutgoingOrientation.Azimuth < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || Plc.OutgoingOrientation.Azimuth > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT) {
                        logger.Error( $"Azimuth ({Plc.OutgoingOrientation.Azimuth}) was out of range." );
                        throw new System.Exception();
                    } else if(Plc.OutgoingOrientation.Elevation < PLCConstants.DECLINATION_LOWER_LIMIT || Plc.OutgoingOrientation.Elevation > PLCConstants.DECLINATION_UPPER_LIMIT) {
                        logger.Error( $"Elevation ({Plc.OutgoingOrientation.Elevation} was out of range.)" );
                        throw new System.Exception();
                    }
                    */
            // Convert orientation object to a json string
            //string jsonOrientation = JsonConvert.SerializeObject( Plc.OutgoingOrientation );
            return SendRelativeMove(programmedPeakSpeedAZInt, programmedPeakSpeedELInt, positionTranslationAZ, positionTranslationEL);

        }



        public override MovementResult MoveToOrientation(Orientation target_orientation, Orientation current_orientation)
        {
            int positionTranslationAZ, positionTranslationEL;
            positionTranslationAZ = ConversionHelper.DegreesToSteps((target_orientation.Azimuth - current_orientation.Azimuth), MotorConstants.GEARING_RATIO_AZIMUTH);
            positionTranslationEL = ConversionHelper.DegreesToSteps((target_orientation.Elevation - current_orientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);

            int EL_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.2), MotorConstants.GEARING_RATIO_ELEVATION);
            int AZ_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.2), MotorConstants.GEARING_RATIO_AZIMUTH);

            //(ObjectivePositionStepsAZ - CurrentPositionStepsAZ), (ObjectivePositionStepsEL - CurrentPositionStepsEL)
            Console.WriteLine("degrees target az " + target_orientation.Azimuth + " el " + target_orientation.Elevation);
            Console.WriteLine("degrees curren az " + current_orientation.Azimuth + " el " + current_orientation.Elevation);


            //return sendmovecomand( EL_Speed * 20 , 50 , positionTranslationAZ , positionTranslationEL ).GetAwaiter().GetResult();
            return SendRelativeMove(AZ_Speed, EL_Speed, positionTranslationAZ, positionTranslationEL);

        }

        public override MovementResult StartBothAxesJog(double azSpeed, RadioTelescopeDirectionEnum azDirection, double elSpeed, RadioTelescopeDirectionEnum elDirection) {
            throw new NotImplementedException();

        }

        public override bool Get_interlock_status() {
            return true;

        }

        public override bool[] GET_MCU_Status( RadioTelescopeAxisEnum axis ) {//set 
            bool[] stuf = new bool[33];
            for(int i = 0; i < 32; i++) {
                stuf[i] = true;
            }
            return stuf;
        }

        public override bool TestIfComponentIsAlive() {
            return true;

        }

        public override MovementResult HomeTelescope() {
            throw new NotImplementedException();
        }

        public override ushort getregvalue(ushort adr)
        {
            throw new NotImplementedException();
        }

        public override void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// This has not been implemented on the scale model.
        /// </summary>
        public override void ResetMCUErrors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will check for any errors present in the MCU's registers.
        /// </summary>
        /// <returns>A list of errors present in the MCU's registers</returns>
        public override List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors()
        {
            throw new NotImplementedException();
        }

        public override bool InterruptMovementAndWaitUntilStopped(bool isCriticalMovementInterrupt = false, bool isSoftwareStopInterrupt = false)
        {
            throw new NotImplementedException();
        }

        public override bool MotorsCurrentlyMoving(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            throw new NotImplementedException();
        }

        public override void SetFinalOffset(Orientation finalPos)
        {
            throw new NotImplementedException();
        }

        public override RadioTelescopeDirectionEnum GetRadioTelescopeDirectionEnum(RadioTelescopeAxisEnum axis)
        {
            throw new NotImplementedException();
        }
    }
}
