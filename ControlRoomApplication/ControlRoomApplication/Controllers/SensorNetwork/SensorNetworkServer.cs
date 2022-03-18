using ControlRoomApplication.Entities;
using ControlRoomApplication.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ControlRoomApplication.Controllers.SensorNetwork.Simulation;
using ControlRoomApplication.Controllers.Communications;
using System.Collections;
using ControlRoomApplication.Entities.DiagnosticData;

namespace ControlRoomApplication.Controllers.SensorNetwork
{

    /// <summary>
    /// This is the central component of the Sensor Network on the Control Room end. This handles all the main communications
    /// with the main Sensor Network system, such as receiving data, setting statuses based on data it receives, and tells the
    /// client when to send initialization data.
    /// </summary>
    public class SensorNetworkServer : PacketDecodingTools
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor used to set the values needed to initialize the components of the class. It will not start waiting for data until
        /// StartSensorMonitoringRoutine() is called.
        /// 
        /// The reason the client IP address is a string and not an IPAddress is because none of the TCPClient class's
        /// constructors take an IPAddress. :(
        /// </summary>
        /// <param name="serverIPAddress">This is the IP address the SensorNetworkServer is "listening" to.</param>
        /// <param name="serverPort">This is the port the SensorNetworkServer is "listening" to.</param>
        /// <param name="clientIPAddress">This is the IP address of the SensorNetwork that we will be sending the sensor initialization to.</param>
        /// <param name="clientPort">This is the port of the SensorNetwork that we will be sending the sensor initialization to.</param>
        /// <param name="telescopeId">The Radio Telescope that the SensorNetworkConfig will apply to.</param>
        /// <param name="isSimulation">Tells the SensorNetworkServer if it should initialize the SimulationSensorNetwork,
        /// or if it is connecting to the production hardware (or maybe an outside simulation).</param>
        public SensorNetworkServer(IPAddress serverIPAddress, int serverPort, string clientIPAddress, int clientPort, int telescopeId, bool isSimulation)
        {
            // Initialize main parts of the Sensor Network
            Server = new TcpListener(serverIPAddress, serverPort);
            InitializationClient = new SensorNetworkClient(clientIPAddress, clientPort, telescopeId);

            // Sensor data initialization
            CurrentElevationMotorTemp = new Temperature[1];
            CurrentElevationMotorTemp[0] = new Temperature();

            CurrentAzimuthMotorTemp = new Temperature[1];
            CurrentAzimuthMotorTemp[0] = new Temperature();

            CurrentElevationAmbientTemp = new Temperature[1];
            CurrentElevationAmbientTemp[0] = new Temperature();

            CurrentElevationAmbientHumidity = new Humidity[1];
            CurrentElevationAmbientHumidity[0] = new Humidity();

            CurrentAbsoluteOrientation = new Orientation();

            CurrentElevationMotorAccl = new Acceleration[1];
            CurrentElevationMotorAccl[0] = new Acceleration();

            CurrentAzimuthMotorAccl = new Acceleration[1];
            CurrentAzimuthMotorAccl[0] = new Acceleration();

            CurrentCounterbalanceAccl = new Acceleration[1];
            CurrentCounterbalanceAccl[0] = new Acceleration();

            AbsoluteOrientationOffset = new Orientation();

            // Sensor error initialization
            SensorStatuses = new SensorStatuses();
            SensorStatuses.AzimuthAbsoluteEncoderStatus = SensorNetworkSensorStatus.Okay;
            SensorStatuses.ElevationAbsoluteEncoderStatus = SensorNetworkSensorStatus.Okay;
            SensorStatuses.AzimuthTemperature1Status = SensorNetworkSensorStatus.Okay;
            SensorStatuses.AzimuthTemperature2Status = SensorNetworkSensorStatus.Okay;
            SensorStatuses.ElevationTemperature1Status = SensorNetworkSensorStatus.Okay;
            SensorStatuses.ElevationTemperature2Status = SensorNetworkSensorStatus.Okay;
            SensorStatuses.ElevationAmbientStatus = SensorNetworkSensorStatus.Okay;
            SensorStatuses.AzimuthAccelerometerStatus = SensorNetworkSensorStatus.Okay;
            SensorStatuses.ElevationAccelerometerStatus = SensorNetworkSensorStatus.Okay;
            SensorStatuses.CounterbalanceAccelerometerStatus = SensorNetworkSensorStatus.Okay;

            // Initialize threads and additional processes, if applicable
            SensorMonitoringThread = new Thread(() => { SensorMonitoringRoutine(); });
            SensorMonitoringThread.Name = "SensorMonitorThread";

            // We only want to run the internal simulation if the user selected to run the Simulated Sensor Network
            if (isSimulation)
            {
                SimulationSensorNetwork = new SimulationSensorNetwork(serverIPAddress.ToString(), serverPort, IPAddress.Parse(clientIPAddress), clientPort);
            }
            else
            {
                SimulationSensorNetwork = null;
            }

            // Initialize the timeout timer but don't start it yet
            Timeout = new System.Timers.Timer();
            Timeout.Elapsed += TimedOut; // TimedOut is the function at the bottom that executes when this elapses
            Timeout.AutoReset = false;

            AzimuthAccBlob = new AzimuthAccelerationBlob();
            ElevationAccBlob = new ElevationAccelerationBlob();
            CounterbalanceAccBlob = new CounterbalanceAccelerationBlob();

        }

        /// <summary>
        /// The current elevation motor temperature received from the sensor network.
        /// </summary>
        public Temperature[] CurrentElevationMotorTemp { get; set; }

        /// <summary>
        /// The current azimuth motor temperature received from the sensor network. 
        /// </summary>
        public Temperature[] CurrentAzimuthMotorTemp { get; set; }

        /// <summary>
        /// The current elevation ambient temperature received from the sensor network. 
        /// </summary>
        public Temperature[] CurrentElevationAmbientTemp { get; set; }

        /// <summary>
        /// The current elevation ambient humidity received from the sensor network. 
        /// </summary>
        public Humidity[] CurrentElevationAmbientHumidity { get; set; }

        /// <summary>
        /// The current elevation ambient dew point calculated from the ambient humidity and 
        /// temperature recieved from the sensor network.
        /// </summary>
        public double CurrentElevationAmbientDewPoint { get; set; }

        /// <summary>
        /// The current orientation of the telescope based off of the absolute encoders. These
        /// are totally separate from the motor encoders' data that we get from the GetMotorEncoderPosition
        /// function in the PLC and MCU, and will provide more accurate data regarding the telescope's
        /// position.
        /// </summary>
        public Orientation CurrentAbsoluteOrientation { get; set; }

        /// <summary>
        /// The absolute encoders will be different from the motor encoders by default, so when we home the telescope,
        /// the offset will be calculated and stored in here. This should work no matter where the homing sensor
        /// is placed.
        /// </summary>
        public Orientation AbsoluteOrientationOffset { get; set; }

        /// <summary>
        /// This tells us the current vibration coming from the azimuth motor. It is always received as
        /// an array to give us higher data accuracy.
        /// </summary>
        public Acceleration[] CurrentElevationMotorAccl { get; set; }

        /// <summary>
        /// This tells us the current vibration coming from the azimuth motor. It is always received as
        /// an array to give us higher data accuracy.
        /// </summary>
        public Acceleration[] CurrentAzimuthMotorAccl { get; set; }

        /// <summary>
        /// This tells us the current vibration coming from the counterbalance. It is always received as
        /// an array to give us higher data accuracy. This can also technically be used to calculate the
        /// telescope's elevation position.
        /// </summary>
        public Acceleration[] CurrentCounterbalanceAccl { get; set; }

        /// <summary>
        /// This is the current orientation calculated from the counterbalance. It is updated with every
        /// packet recieved from the sensor network.
        /// </summary>
        public double CurrentCBAccelElevationPosition { get; set; }

        /// <summary>
        /// This is used for sending initialization data to the Sensor Network, and is also used to
        /// access the configuration.
        /// </summary>
        public SensorNetworkClient InitializationClient { get; }

        /// <summary>
        /// This is the simulation sensor network, which will run whenever we are not using the hardware.
        /// </summary>
        public SimulationSensorNetwork SimulationSensorNetwork { get; }

        /// <summary>
        /// This will be used to tell us what the SensorNetwork status using <seealso cref="SensorNetworkStatusEnum"/>.
        /// </summary>
        public SensorNetworkStatusEnum Status { get; set; }

        /// <summary>
        /// This contains data for the individual sensors' statuses.
        /// </summary>
        public SensorStatuses SensorStatuses { get; set; }

        /// <summary>
        /// These two objects are used together to receive data, and should be destroyed together as well.
        /// </summary>
        private TcpListener Server;
        private NetworkStream Stream;

        /// <summary>
        /// This should be true as long as the SensorMonitoringThread is running, and set to false if that thread
        /// is to be terminated.
        /// </summary>
        private bool CurrentlyRunning { get; set; }
        
        /// <summary>
        /// This thread should always be running and waiting or collecting some kind of data from the Sensor Network unless
        /// the CurrentlyRunning value (above) is set to false.
        /// </summary>
        private Thread SensorMonitoringThread { get; set; }

        /// <summary>
        /// This will help us detect if the SensorNetworkServer has stopped receiving data.
        /// </summary>
        private System.Timers.Timer Timeout { get; }

        /// <summary>
        /// This will be used to help send acceleration data to the database
        /// </summary>
        private AzimuthAccelerationBlob AzimuthAccBlob { get; set; }

        /// <summary>
        /// This will be used to help send acceleration data to the database
        /// </summary>
        private ElevationAccelerationBlob ElevationAccBlob { get; set; }

        /// <summary>
        /// This will be used to help send acceleration data to the database
        /// </summary>
        private CounterbalanceAccelerationBlob CounterbalanceAccBlob { get; set; }

        /// <summary>
        /// This stores the timestamp that the sensor network server got connected with a client
        /// </summary>
        private long ConnectionTimestamp { get; set; }

        /// <summary>
        /// This starts the SensorMonitoringRoutine. Calling this will immediately begin initialization.
        /// </summary>
        /// <returns>If started successfully, return true. Else, return false.</returns>
        public void StartSensorMonitoringRoutine(bool rebooting = false)
        {
            Server.Start();
            CurrentlyRunning = true;
            Status = SensorNetworkStatusEnum.Initializing;
            Timeout.Interval = InitializationClient.SensorNetworkConfig.TimeoutInitialization;
            Timeout.Start();
            SensorMonitoringThread.Start();

            if (SimulationSensorNetwork != null && !rebooting)
            {
                SimulationSensorNetwork.StartSimulationSensorNetwork();
            }
        }

        /// <summary>
        /// This ends the SensorMonitoringRoutine. This should only be executed when "Shutdown RT" is clicked.
        /// </summary>
        /// <param name="rebooting">This will tell the function if we are rebooting, or shutting down indefinitely.</param>
        /// <returns>If ended successfully, return true. Else, return false.</returns>
        public void EndSensorMonitoringRoutine(bool rebooting = false)
        {
            CurrentlyRunning = false;
            // The stream will only be null if the sensor monitoring thread has not been called
            if (Stream != null)
            {
                Stream.Close();
                Stream.Dispose();
            }

            Server.Stop();

            if (Timeout.Enabled) Timeout.Stop();

            SensorMonitoringThread.Join();
            
            if (!rebooting)
            { // We want to keep using the timer if we are rebooting, not destroy it.
                Status = SensorNetworkStatusEnum.None;
                Timeout.Dispose();

                if (SimulationSensorNetwork != null)
                {
                    SimulationSensorNetwork.EndSimulationSensorNetwork();
                }
            }
            else
            {
                Status = SensorNetworkStatusEnum.Rebooting;
                SensorMonitoringThread = new Thread(() => { SensorMonitoringRoutine(); });
            }
        }

        /// <summary>
        /// This will reboot the Sensor Network, which will update its sensor initialization with what the
        /// user has selected on the GUI.
        /// </summary>
        /// <returns></returns>
        public void RebootSensorNetwork()
        {
            logger.Info($"{Utilities.GetTimeStamp()}: Rebooting Sensor Network. This may take a few seconds...");

            EndSensorMonitoringRoutine(true); // Must pass through true because we are rebooting

            // This is to let the Sensor Network time out, triggering a reboot. We're adding 50ms on to this to make
            // sure it REALLY times out.
            Thread.Sleep(SensorNetworkConstants.WatchDogTimeout + 50);

            StartSensorMonitoringRoutine(true);
        }

        /// <summary>
        /// This is used internally by the SensorNetworkServer to tell what kind of data the Sensor Network
        /// is sending. It may send data, in which case a transit ID will be present, or it may ask for
        /// an initialization.
        /// </summary>
        /// <param name="data">The data we are interpreting.</param>
        /// <param name="receivedDataSize">This will be used to tell if our data is complete.</param>
        private bool InterpretData(byte[] data, int receivedDataSize)
        {
            bool success = false;

            if(Encoding.ASCII.GetString(data, 0, receivedDataSize).Equals("Send Sensor Configuration"))
            { // Reaching here means that we've received a request for sensor initialization
                Status = SensorNetworkStatusEnum.Initializing;
                
                // If the init sending fails, set status to reflect that
                if (!InitializationClient.SendSensorInitialization())
                {
                    Status = SensorNetworkStatusEnum.InitializationSendingFailed;
                    if(Timeout.Enabled) Timeout.Stop();

                    pushNotification.sendToAllAdmins("Sensor Network Error", $"Status: {Status}");
                }
                else
                {
                    ConnectionTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    logger.Info($"{Utilities.GetTimeStamp()}: Successfully sent sensor initialization to the Sensor Network.");
                    success = true;
                }
            }
            else
            {
                Status = SensorNetworkStatusEnum.ReceivingData;

                // Bytes 1-3 of the data contain the overall size of the packet
                UInt32 expectedDataSize = (UInt32)(data[1] << 24 | data[2] << 16 | data[3] << 8 | data[4]);

                // Check that the data we received is the same size as what we expect
                if (expectedDataSize == receivedDataSize)
                {
                    // Byte 0 contains the transmit ID
                    int receivedTransitId = data[0];

                    // Verify that the first byte contains the "success" code (transit ID)
                    if(receivedTransitId == SensorNetworkConstants.TransitIdSuccess)
                    {
                        // At this point, we may begin parsing the data

                        // Sensor statuses and error codes
                        BitArray sensorStatus = new BitArray(new byte[] { data[5], data[6] }); // sensor statuses 
                        UInt32 sensorErrors = (UInt32)(data[7] << 16 | data[8] << 8 | data[9]); // sensor self-tests | adxl error codes and azimuth encoder error code | temp sensor error codes

                        // Acquire the sample sizes for each sensor
                        UInt16 elAcclSize = (UInt16)(data[10] << 8 | data[11]);
                        UInt16 azAcclSize = (UInt16)(data[12] << 8 | data[13]);
                        UInt16 cbAcclSize = (UInt16)(data[14] << 8 | data[15]);
                        UInt16 elTempSensorSize = (UInt16)(data[16] << 8 | data[17]);
                        UInt16 azTempSensorSize = (UInt16)(data[18] << 8 | data[19]);
                        UInt16 elEncoderSize = (UInt16)(data[20] << 8 | data[21]);
                        UInt16 azEncoderSize = (UInt16)(data[22] << 8 | data[23]);
                        UInt16 ambientTempSize = (UInt16)(data[24] << 8 | data[25]);
                        UInt16 ambientHumiditySize = (UInt16)(data[26] << 8 | data[27]);

                        // TODO: Outside of right here, we aren't doing anything with the sensor statuses. These should
                        // be updated along with the sensor data on the diagnostics form. How this looks is up to you. (issue #353)
                        SensorStatuses = ParseSensorStatuses(sensorStatus, sensorErrors);

                        // This is the index we start reading sensor data
                        int k = 28;

                        // If no data comes through for a sensor (i.e. the size is 0), then it will not be updated,
                        // otherwise the UI value would temporarily be set to 0, which would be inaccurate

                        // Accelerometer 1 (elevation)
                        if (elAcclSize > 0)
                        {
                            //Create array of acceleration objects 
                            CurrentElevationMotorAccl = GetAccelerationFromBytes(ref k, data, elAcclSize, SensorLocationEnum.EL_MOTOR, ConnectionTimestamp);
                            ElevationAccBlob.BuildAccelerationBlob(CurrentElevationMotorAccl);
                        }

                        // Accelerometer 2 (azimuth)
                        if (azAcclSize > 0)
                        {
                            CurrentAzimuthMotorAccl = GetAccelerationFromBytes(ref k, data, azAcclSize, SensorLocationEnum.AZ_MOTOR, ConnectionTimestamp);
                            AzimuthAccBlob.BuildAccelerationBlob(CurrentAzimuthMotorAccl);
                        }

                        // Accelerometer 3 (counterbalance)
                        if (cbAcclSize > 0)
                        {
                            CurrentCounterbalanceAccl = GetAccelerationFromBytes(ref k, data, cbAcclSize, SensorLocationEnum.COUNTERBALANCE, ConnectionTimestamp);
                            CounterbalanceAccBlob.BuildAccelerationBlob(CurrentCounterbalanceAccl);

                            // If there is new counterbalance accelerometer data, update the elevation position
                            UpdateCBAccelElevationPosition();
                        }

                        // Elevation temperature
                        if (elTempSensorSize > 0)
                        {
                            CurrentElevationMotorTemp = GetMotorTemperatureFromBytes(ref k, data, elTempSensorSize, SensorLocationEnum.EL_MOTOR);
                            Database.DatabaseOperations.AddSensorData(CurrentElevationMotorTemp);
                        }

                        // Azimuth temperature
                        if (azTempSensorSize > 0)
                        {
                            CurrentAzimuthMotorTemp = GetMotorTemperatureFromBytes(ref k, data, azTempSensorSize, SensorLocationEnum.AZ_MOTOR);
                            Database.DatabaseOperations.AddSensorData(CurrentAzimuthMotorTemp);
                        }

                        // Elevation absolute encoder
                        if (elEncoderSize > 0)
                        {
                            CurrentAbsoluteOrientation.Elevation = GetElevationAxisPositionFromBytes(ref k, data, AbsoluteOrientationOffset.Elevation, CurrentAbsoluteOrientation.Elevation);
                        }

                        // Azimuth absolute encoder
                        if (azEncoderSize > 0)
                        {
                            CurrentAbsoluteOrientation.Azimuth = GetAzimuthAxisPositionFromBytes(ref k, data, AbsoluteOrientationOffset.Azimuth, CurrentAbsoluteOrientation.Azimuth);
                        }

                        // Elevation Ambient Temperature
                        if (ambientTempSize > 0)
                        {
                            CurrentElevationAmbientTemp = GetAmbientTemperatureFromBytes(ref k, data, ambientTempSize, SensorLocationEnum.EL_FRAME);
                            UpdateAmbientDewPoint();
                            Database.DatabaseOperations.AddSensorData(CurrentElevationAmbientTemp);
                        }

                        // Elevation Ambient Temperature
                        if (ambientHumiditySize > 0)
                        {
                            CurrentElevationAmbientHumidity = GetAmbientHumidityFromBytes(ref k, data, ambientHumiditySize, SensorLocationEnum.EL_FRAME);
                            UpdateAmbientDewPoint();
                            Database.DatabaseOperations.AddSensorData(CurrentElevationAmbientHumidity);
                        }

                        success = true;
                    }

                    // This may be replaced with different errors at some point (that have different transit IDs), 
                    // though there are currently no plans for this. Right now, it is treated as an error overall:
                    // We should NOT be receiving anything other than TransitIdSuccess.
                    else
                    {
                        if (Status != SensorNetworkStatusEnum.TransitIdError)
                        {
                            logger.Error($"{Utilities.GetTimeStamp()}: Transit ID error: Expected " +
                                $"ID {SensorNetworkConstants.TransitIdSuccess}, received ID {receivedTransitId})");

                            Status = SensorNetworkStatusEnum.TransitIdError;
                        }
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// This is the main routine that will constantly be expecting data from the Sensor Network. This will
        /// continuously run in a loop until EndSensorMonitoringRoutine() is called.
        /// </summary>
        private void SensorMonitoringRoutine()
        {
            TcpClient localClient;

            byte[] receivedData = new byte[SensorNetworkConstants.MaxPacketSize];

            while(CurrentlyRunning)
            {
                try
                {
                    localClient = Server.AcceptTcpClient();
                    Stream = localClient.GetStream();

                    logger.Info($"{Utilities.GetTimeStamp()}: Successfully connected to the Sensor Network!");

                    int receivedDataSize;

                    while ((receivedDataSize = Stream.Read(receivedData, 0, receivedData.Length)) != 0 && CurrentlyRunning)
                    {
                        // If the status is initializing, we want the timer to keep going. Else, we are currently receiving data, and
                        // want to stop the timer as soon as we get something.
                        if (Timeout.Enabled && Status == SensorNetworkStatusEnum.ReceivingData)
                        {
                            Timeout.Stop();
                        }

                        InterpretData(receivedData, receivedDataSize);

                        // We only want to start the timeout if we are currently receiving data. The reason is because, the timeout
                        // status will overwrite any preexisting status errors.
                        if (Status == SensorNetworkStatusEnum.ReceivingData)
                        {
                            Timeout.Interval = InitializationClient.SensorNetworkConfig.TimeoutDataRetrieval;
                            Timeout.Start();
                        }
                    }

                    localClient.Close();
                    localClient.Dispose();
                }
                catch (Exception e)
                {
                    if (CurrentlyRunning) // If we're not currently running, then it means we voluntarily shut down the server
                    {
                        Timeout.Stop();
                        Status = SensorNetworkStatusEnum.ServerError;
                        logger.Error($"{Utilities.GetTimeStamp()}: An error occurred while running the server; please check that the connection is available.");
                        logger.Info($"{Utilities.GetTimeStamp()}: Trying to reconnect to the Sensor Network...");

                        pushNotification.sendToAllAdmins("Sensor Network Error", $"Status: {Status}");
                    }
                }
            }
        }

        /// <summary>
        /// This is used to parse the sensor statuses into the SensorStatuses object.
        /// </summary>
        /// <param name="statuses">Regular statuses.</param>
        /// <param name="errors">Various error codes if there are errors.</param>
        /// <returns></returns>
        private SensorStatuses ParseSensorStatuses(BitArray statuses, UInt32 errors)
        {
            SensorStatuses s = new SensorStatuses
            {
                // Regular statuses
                AzimuthAbsoluteEncoderStatus = statuses[0] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                AzimuthTemperature1Status = statuses[2] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                AzimuthTemperature2Status = statuses[1] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                ElevationTemperature1Status = statuses[4] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                ElevationTemperature2Status = statuses[3] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                AzimuthAccelerometerStatus = statuses[6] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                ElevationAccelerometerStatus = statuses[7] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,
                CounterbalanceAccelerometerStatus = statuses[5] ? SensorNetworkSensorStatus.Okay : SensorNetworkSensorStatus.Error,

                // TODO: Parse errors here. You will need to add the errors to the SensorStatuses object (issue #353)
            };

            return s;
        }

        /// <summary>
        /// This is only reached when a timeout occurs from the Timer. One of the two TimedOut statuses will
        /// be set as a result of this call.
        /// </summary>
        /// <param name="source">unused</param>
        /// <param name="e">unused</param>
        private void TimedOut(Object source, ElapsedEventArgs e)
        {
            if(Status == SensorNetworkStatusEnum.ReceivingData)
            {
                Status = SensorNetworkStatusEnum.TimedOutDataRetrieval;
            }
            else if(Status == SensorNetworkStatusEnum.Initializing)
            {
                Status = SensorNetworkStatusEnum.TimedOutInitialization;
            }
            else
            {
                Status = SensorNetworkStatusEnum.UnknownError;
            }

            logger.Error($"{Utilities.GetTimeStamp()}: Connection to the Sensor Network timed out! Status: {Status}");

            pushNotification.sendToAllAdmins("Sensor Network Timeout", $"Status: {Status}");
        }

        /// <summary>
        /// Update the counterbalance accelerometer position with the data currently in CurrentCounterBalanceAccl
        /// </summary>
        private void UpdateCBAccelElevationPosition()
        {
            double y_sum = 0, z_sum = 0, x_sum = 0;

            for (int i = 0; i < CurrentCounterbalanceAccl.Length; i++)
            {
                // Gather a sum of all the accerlometer data
                y_sum += CurrentCounterbalanceAccl[i].y;
                z_sum += CurrentCounterbalanceAccl[i].z;
                x_sum += CurrentCounterbalanceAccl[i].x;
            }

            // Get an average of all accelerometer data for a more precise reading
            double y_avg = y_sum / CurrentCounterbalanceAccl.Length;
            double x_avg = x_sum / CurrentCounterbalanceAccl.Length;
            double z_avg = z_sum / CurrentCounterbalanceAccl.Length;
            //Console.WriteLine("X: " + x_avg + " Y: " + y_avg + " Z: " + z_avg);

            // Map the accerlerometer output values to their proper G-force range
            double X_out = x_avg / 256.0;
            double Y_out = y_avg / 256.0;
            double Z_out = z_avg / 256.0;
            //Console.WriteLine("X: " + X_out + " Y: " + Y_out + " Z: " + Z_out);

            //Console.WriteLine(Math.Atan2(Y_out, -Z_out) * 180.0 / Math.PI + SensorNetworkConstants.CBAccelPositionOffset);
            // Calculate roll orientation
            CurrentCBAccelElevationPosition = Math.Atan2(Y_out, -Z_out) * 180.0 / Math.PI + SensorNetworkConstants.CBAccelPositionOffset;
        }
        /// <summary>
        /// A method to update the current ambient dew point.
        /// Code was taken/modeled from https://www.best-microcontroller-projects.com/dht22.html
        /// reference (1) : http://wahiduddin.net/calc/density_algorithms.htm
        /// reference (2) : http://www.colorado.edu/geography/weather_station/Geog_site/about.htm
        /// </summary>
        private void UpdateAmbientDewPoint()
        {
            double currentTempC = (CurrentElevationAmbientTemp[CurrentElevationAmbientTemp.Length - 1].temp - 32) * 5.0 / 9.0;
            // (1) Saturation Vapor Pressure = ESGG(T)
            double ratio = 373.15 / (273.15 + currentTempC);
            double rhs = -7.90298 * (ratio - 1);
            rhs += 5.02808 * Math.Log10(ratio);
            rhs += -1.3816e-7 * (Math.Pow(10, (11.344 * (1 - 1 / ratio))) - 1);
            rhs += 8.1328e-3 * (Math.Pow(10, (-3.49149 * (ratio - 1))) - 1);
            rhs += Math.Log10(1013.246);

            // factor -3 is to adjust units -Vapor Pressure SVP * humidity
            double vp = Math.Pow(10, rhs - 3) * CurrentElevationAmbientHumidity[CurrentElevationAmbientHumidity.Length - 1].HumidityReading;

            // (2) DEWPOINT = F(Vapor Pressure)
            double t = Math.Log(vp / 0.61078);
            double dewPointC = (241.88 * t) / (17.558 - t);

            CurrentElevationAmbientDewPoint = 9.0 / 5.0 * dewPointC + 32;
        }
    }
}
