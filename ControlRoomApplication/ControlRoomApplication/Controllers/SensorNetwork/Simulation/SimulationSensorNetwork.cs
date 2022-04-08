using ControlRoomApplication.Util;
using EmbeddedSystemsTest.SensorNetworkSimulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Simulation
{
    /// <summary>
    /// The simulation sensor network, which will respond exactly the same was as the physical SensorNetwork hardware will. The SensorNetworkServer
    /// does not know the difference between this and the physical hardware.
    /// </summary>
    public class SimulationSensorNetwork : PacketEncodingTools
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor to initialize a new instance of the SimulationSensorNetwork. This will NOT start its simulated sensor monitoring.
        /// </summary>
        /// <param name="teensyClientIP">The Teensy's Client IP; this must be set equal to the SensorNetworkServer's Server IP.</param>
        /// <param name="teensyClientPort">The Teensy's Client Port; this must be set equal to the SensorNetworkServer's Server Port.</param>
        /// <param name="teensyServerIP">The Teensy's Server IP; this must be set equal to the SensorNetworkServer's Client IP.</param>
        /// <param name="teensyServerPort">The Teensy's Server Port; this must be set equal to the SensorNetworkServer's Client Port.</param>
        /// <param name="dataPath">The path to CSV data. By default, it looks for it in the SensorNetworkConstants.SimCSVDirectory location.</param>
        public SimulationSensorNetwork(string teensyClientIP, int teensyClientPort, IPAddress teensyServerIP, int teensyServerPort, string dataPath = SensorNetworkConstants.SimCSVDirectory)
        {
            Server = new TcpListener(teensyServerIP, teensyServerPort);

            ClientIP = teensyClientIP;
            ClientPort = teensyClientPort;

            DataDirectory = dataPath;
        }

        private TcpListener Server { get; set; }
        private NetworkStream ServerStream { get; set; }

        private TcpClient Client { get; set; }
        private string ClientIP { get; set; }
        private int ClientPort { get; set; }
        private NetworkStream ClientStream { get; set; }

        private string DataDirectory;

        private Thread SimulationSensorMonitoringThread { get; set; }

        private bool CurrentlyRunning { get; set; }

        private double[] AzimuthTempData { get; set; }

        private double[] ElevationTempData { get; set; }

        private float[] AmbientTempData { get; set; }

        private float[] AmbientHumidityData { get; set; }

        private RawAccelerometerData[] AzimuthAccData { get; set; }

        private RawAccelerometerData[] ElevationAccData { get; set; }

        private RawAccelerometerData[] CounterbalanceAccData { get; set; }

        private double[] AzimuthEncoderData { get; set; }

        private double[] ElevationEncoderData { get; set; }

        private long ConnectionTimeStamp { get; set; }

        private bool FanOn { get; set; }

        /// <summary>
        /// This is used to start the simulation Sensor Network. Calling this is equivalent to powering on the Teensy.
        /// </summary>
        public void StartSimulationSensorNetwork()
        {
            CurrentlyRunning = true;
            SimulationSensorMonitoringThread = new Thread(() => { SimulationSensorMonitor(); });
            SimulationSensorMonitoringThread.Start();
        }

        /// <summary>
        /// This is used to start the simulation Sensor Network. Calling this is equivalent to powering off the Teensy.
        /// </summary>
        public void EndSimulationSensorNetwork()
        {
            if (CurrentlyRunning)
            {
                CurrentlyRunning = false;
                if (Client != null) Client.Dispose();
                if (ClientStream != null)
                {
                    ClientStream.Close();
                    ClientStream.Dispose();
                }
                Server.Stop();
                if (ServerStream != null)
                {
                    ServerStream.Close();
                    ServerStream.Dispose();
                }
                SimulationSensorMonitoringThread.Join();
            }
        }

        private void SimulationSensorMonitor()
        {
            // First, we want to connect to the SensorNetworkServer
            if(CurrentlyRunning) WaitForAndConnectToServer();

            // Next, we want to request initialization and receive it
            byte[] receivedInit = new byte[0];
            if(CurrentlyRunning) receivedInit = RequestAndAcquireSensorInitialization();

            ConnectionTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // At this point, we have the initialization and can initialize the sensors
            if (CurrentlyRunning) InitializeSensors(receivedInit);

            // Now we can grab the CSV data for ONLY the initialized sensors...
            if(CurrentlyRunning) ReadFakeDataFromCSV();

            // Keep track of the indexes for each data array, because we are only extracting a small subsection of each one.
            // We want to know what subsection we just got so we can get the next subsection in the next iteration
            int? elTempIdx = 0;
            int? azTempIdx = 0;
            int? elEncIdx = 0;
            int? azEncIdx = 0;
            int? elAccIdx = 0;
            int? azAccIdx = 0;
            int? cbAccIdx = 0;
            int? ambTempIdx = 0;
            int? ambHumidityIdx = 0;

            // This will tell us if we are rebooting or not. We will only reboot if the connection is randomly terminated.
            bool reboot = false;

            // Now we enter the "super loop"
            while (CurrentlyRunning)
            {
                // Convert subarrays to bytes
                SimulationSubArrayData subArrays = BuildSubArrays(ref elTempIdx, ref azTempIdx, ref elEncIdx, ref azEncIdx, ref elAccIdx, ref azAccIdx, ref cbAccIdx, ref ambTempIdx, ref ambHumidityIdx);

                SensorStatuses statuses = new SensorStatuses
                {
                    // TODO: Write the values of each sensor status in here so it can get be encoded (issue #376)
                };

                byte[] dataToSend = ConvertDataArraysToBytes(
                    subArrays.ElevationAccl, 
                    subArrays.AzimuthAccl, 
                    subArrays.CounterBAccl, 
                    subArrays.ElevationTemps, 
                    subArrays.AzimuthTemps, 
                    subArrays.ElevationEnc, 
                    subArrays.AzimuthEnc,
                    subArrays.AmbientTemps,
                    subArrays.AmbientHumidity,
                    statuses,
                    ConnectionTimeStamp,
                    FanOn
                );

                // We have to check for CurrentlyRunning down here because we don't know when the connection is going to be terminated, and
                // it could very well be in the middle of the loop.
                if (CurrentlyRunning)
                {
                    try
                    {
                        // Send arrays
                        ClientStream.Write(dataToSend, 0, dataToSend.Length);

                        // Read fan byte
                        FanOn = (ClientStream.ReadByte() != 0);

                        Thread.Sleep(SensorNetworkConstants.DefaultDataSendingInterval);
                    }
                    // This will be reached if the connection is unexpectedly terminated (like it is during sensor reinitialization)
                    catch
                    {
                        CurrentlyRunning = false;
                        reboot = true;
                    }
                }
            }

            // If the server disconnects, that triggers a reboot
            if(reboot)
            {
                CurrentlyRunning = true;
                Client.Dispose();
                ClientStream.Dispose();
                SimulationSensorMonitor();
            }
        }

        /// <summary>
        /// This will read into each data array and pull out a subarray based on the index value, and then encode those subarrays.
        /// </summary>
        /// <param name="azTempIdx">Azimuth temperature data array index that we are pulling from.</param>
        /// <param name="elTempIdx">Elevation temperature data array index that we are pulling from.</param>
        /// <param name="elEncIdx">Elevation encoder data array index that we are pulling from.</param>
        /// <param name="azEncIdx">Azimuth encoder data array index that we are pulling from.</param>
        /// <param name="elAccIdx">Elevation accelerometer data array index that we are pulling from.</param>
        /// <param name="azAccIdx">Azimuth accelerometer data array index that we are pulling from.</param>
        /// <param name="cbAccIdx">Counterbalance accelerometer data array index that we are pulling from.</param>
        /// <returns></returns>
        public SimulationSubArrayData BuildSubArrays(ref int? elTempIdx, ref int? azTempIdx, ref int? elEncIdx, ref int? azEncIdx, ref int? elAccIdx, ref int? azAccIdx, ref int? cbAccIdx, ref int? ambTempIdx, ref int? ambHumidityIdx)
        {
            SimulationSubArrayData subArrays = new SimulationSubArrayData();

            // If the sensors are initialized, give them their subarrays, while also updating the index so that
            // this  knows what subarrays to go to next

            if (ElevationTempData != null && elTempIdx != null)
            {
                subArrays.ElevationTemps = new double[1];
                Array.Copy(ElevationTempData, elTempIdx ?? 0, subArrays.ElevationTemps, 0, 1);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (elTempIdx + 1 > ElevationTempData.Length - 1) elTempIdx = 0;
                else elTempIdx++;
            }
            else subArrays.ElevationTemps = new double[0];

            if (AzimuthTempData != null && azTempIdx != null)
            {
                subArrays.AzimuthTemps = new double[1];
                Array.Copy(AzimuthTempData, azTempIdx ?? 0, subArrays.AzimuthTemps, 0, 1);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (azTempIdx + 1 > AzimuthTempData.Length - 1) azTempIdx = 0;
                else azTempIdx++;
            }
            else subArrays.AzimuthTemps = new double[0];

            if (ElevationEncoderData != null && elEncIdx != null)
            {
                subArrays.ElevationEnc = new double[1];
                Array.Copy(ElevationEncoderData, elEncIdx ?? 0, subArrays.ElevationEnc, 0, 1);
                
                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (elEncIdx + 1 > ElevationEncoderData.Length - 1) elEncIdx = 0;
                else elEncIdx++;
            }
            else subArrays.ElevationEnc = new double[0];

            if (AzimuthEncoderData != null && azEncIdx != null)
            {
                subArrays.AzimuthEnc = new double[1];
                Array.Copy(AzimuthEncoderData, azEncIdx ?? 0, subArrays.AzimuthEnc, 0, 1);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (azEncIdx + 1 > AzimuthEncoderData.Length - 1) azEncIdx = 0;
                else azEncIdx++;
            }
            else subArrays.AzimuthEnc = new double[0];

            if (AzimuthAccData != null && azAccIdx != null)
            {
                subArrays.AzimuthAccl = new RawAccelerometerData[100];
                Array.Copy(AzimuthAccData, azAccIdx ?? 0, subArrays.AzimuthAccl, 0, 100);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (azAccIdx + 199 > AzimuthAccData.Length - 1) azAccIdx = 0;
                else azAccIdx += 100;
            }
            else subArrays.AzimuthAccl = new RawAccelerometerData[0];

            if (ElevationAccData != null && elAccIdx != null)
            {
                subArrays.ElevationAccl = new RawAccelerometerData[100];
                Array.Copy(ElevationAccData, elAccIdx ?? 0, subArrays.ElevationAccl, 0, 100);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (elAccIdx + 199 > ElevationAccData.Length - 1) elAccIdx = 0;
                else elAccIdx += 100;
            }
            else subArrays.ElevationAccl = new RawAccelerometerData[0];

            if (CounterbalanceAccData != null && cbAccIdx != null)
            {
                subArrays.CounterBAccl = new RawAccelerometerData[100];
                Array.Copy(CounterbalanceAccData, cbAccIdx ?? 0, subArrays.CounterBAccl, 0, 100);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (cbAccIdx + 199 > CounterbalanceAccData.Length - 1) cbAccIdx = 0;
                else cbAccIdx += 100;
            }
            else subArrays.CounterBAccl = new RawAccelerometerData[0];

            if (AmbientTempData != null && ambTempIdx != null)
            {
                subArrays.AmbientTemps = new float[1];
                Array.Copy(AmbientTempData, ambTempIdx ?? 0, subArrays.AmbientTemps, 0, 1);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (ambTempIdx + 1 > AmbientTempData.Length - 1) ambTempIdx = 0;
                else ambTempIdx++;
            }
            else subArrays.AmbientTemps = new float[0];

            if (AmbientHumidityData != null && ambHumidityIdx != null)
            {
                subArrays.AmbientHumidity = new float[1];
                Array.Copy(AmbientHumidityData, ambHumidityIdx ?? 0, subArrays.AmbientHumidity, 0, 1);

                // Increment to next index, or back to 0 if we've reached the end of the main array
                if (ambHumidityIdx + 1 > AmbientHumidityData.Length - 1) ambHumidityIdx = 0;
                else ambHumidityIdx++;
            }
            else subArrays.AmbientHumidity = new float[0];

            return subArrays;
        }

        /// <summary>
        /// This will wait for the SensorNetworkServer, and when it finds it, it will connect!
        /// This code was directly lifted from how this functionality works in the Teensy's source code.
        /// </summary>
        private void WaitForAndConnectToServer()
        {
            bool connected = false;

            // Wait for the SensorNetworkServer to be up
            while (!connected && CurrentlyRunning)
            {
                try
                {
                    Client = new TcpClient(ClientIP, ClientPort);
                    ClientStream = Client.GetStream();
                    connected = true;
                    
                    // Ask the SensorNetworkServer for its initialization
                    byte[] askForInit = Encoding.ASCII.GetBytes("Send Sensor Configuration");
                    ClientStream.Write(askForInit, 0, askForInit.Length);
                    ClientStream.Flush();
                }
                catch
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: SimulationSensorNetwork is waiting for the SensorNetworkServer.");
                    if (Client != null) Client.Dispose();
                    if (ClientStream != null) ClientStream.Dispose();
                }
            }
        }

        /// <summary>
        /// This will send a request for sensor initialization to the SensorNetworkServer, where the
        /// SensorNetworkServer will then respond with sensor initialization.
        /// </summary>
        /// <returns>The sensor initialization byte array.</returns>
        private byte[] RequestAndAcquireSensorInitialization()
        {
            // Start the server that will expect the Sensor Configuration
            Server.Start();

            // Wait for the SensorNetworkClient to send the initialization
            TcpClient localClient;
            byte[] receivedInit = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            // Once this line is passed, we have connected and received the initialization
            localClient = Server.AcceptTcpClient();
            logger.Info($"{Utilities.GetTimeStamp()}: Simulation Sensor Network received Sensor Initialization");
            ServerStream = localClient.GetStream();
            ServerStream.Read(receivedInit, 0, receivedInit.Length);

            // We will now dispose of the Server/Stream components because that is the only time we are using it
            ServerStream.Close();
            ServerStream.Dispose();
            Server.Stop();

            return receivedInit;
        }

        /// <summary>
        /// This is used to set the initialization of the sensors. Any non-initialized sensors will not be encoded.
        /// </summary>
        /// <param name="init">Sensor initialization we receive from the SensorNetworkServer's InitializationClient</param>
        private void InitializeSensors(byte[] init)
        {
            if (init[(int)SensorInitializationEnum.ElevationTemp] == 0) ElevationTempData = null;
            else ElevationTempData = new double[0];

            if (init[(int)SensorInitializationEnum.AzimuthTemp] == 0) AzimuthTempData = null;
            else AzimuthTempData = new double[0];

            if (init[(int)SensorInitializationEnum.ElevationEncoder] == 0) ElevationEncoderData = null;
            else ElevationEncoderData = new double[0];

            if (init[(int)SensorInitializationEnum.AzimuthEncoder] == 0) AzimuthEncoderData = null;
            else AzimuthEncoderData = new double[0];

            if (init[(int)SensorInitializationEnum.AzimuthAccelerometer] == 0) AzimuthAccData = null;
            else AzimuthAccData = new RawAccelerometerData[0];

            if (init[(int)SensorInitializationEnum.ElevationAccelerometer] == 0) ElevationAccData = null;
            else ElevationAccData = new RawAccelerometerData[0];

            if (init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] == 0) CounterbalanceAccData = null;
            else CounterbalanceAccData = new RawAccelerometerData[0];

            if (init[(int)SensorInitializationEnum.AmbientTempHumidity] == 0)
            {
                AmbientTempData = null;
                AmbientHumidityData = null;
            }
            else
            {
                AmbientTempData = new float[0];
                AmbientHumidityData = new float[0];
            }
        }

        /// <summary>
        /// This will read fake data from the CSV files based off of the initialization. If the sensor is "initialized" (not null),
        /// then it will read in CSV data for it. Otherwise, it will stay null.
        /// </summary>
        private void ReadFakeDataFromCSV()
        {
            if (ElevationTempData != null)
            {
                double dbl;

                var values = File.ReadAllLines(DataDirectory + "TestElTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                ElevationTempData = values.ToArray();
            }
            
            if (AzimuthTempData != null)
            {
                double dbl;

                var values = File.ReadAllLines(DataDirectory + "TestAzTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                AzimuthTempData = values.ToArray();
            }
            
            if (AzimuthAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(DataDirectory + "TestAzAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(DataDirectory + "TestAzAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(DataDirectory + "TestAzAccZ.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                // Find which axis has the lowest number, which we will use as the size for the overall array
                int lowest = xData.Length;
                if (lowest > yData.Length) lowest = yData.Length;
                if (lowest > zData.Length) lowest = zData.Length;
                
                AzimuthAccData = new RawAccelerometerData[lowest];

                // Populate raw accelerometer data with individual axes
                for(int i = 0; i < lowest; i++)
                {
                    AzimuthAccData[i].X = xData[i];
                    AzimuthAccData[i].Y = yData[i];
                    AzimuthAccData[i].Z = zData[i];
                }
            }
            
            if (ElevationAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(DataDirectory + "TestElAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(DataDirectory + "TestElAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(DataDirectory + "TestElAccZ.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                // Find which axis has the lowest number, which we will use as the size for the overall array
                int lowest = xData.Length;
                if (lowest > yData.Length) lowest = yData.Length;
                if (lowest > zData.Length) lowest = zData.Length;

                ElevationAccData = new RawAccelerometerData[lowest];

                // Populate raw accelerometer data with individual axes
                for (int i = 0; i < lowest; i++)
                {
                    ElevationAccData[i].X = xData[i];
                    ElevationAccData[i].Y = yData[i];
                    ElevationAccData[i].Z = zData[i];
                }
            }
            
            if (CounterbalanceAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(DataDirectory + "TestCbAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(DataDirectory + "TestCbAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(DataDirectory + "TestCbAccZ.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                // Find which axis has the lowest number, which we will use as the size for the overall array
                int lowest = xData.Length;
                if (lowest > yData.Length) lowest = yData.Length;
                if (lowest > zData.Length) lowest = zData.Length;

                CounterbalanceAccData = new RawAccelerometerData[lowest];

                // Populate raw accelerometer data with individual axes
                for (int i = 0; i < lowest; i++)
                {
                    CounterbalanceAccData[i].X = xData[i];
                    CounterbalanceAccData[i].Y = yData[i];
                    CounterbalanceAccData[i].Z = zData[i];
                }
            }
            
            if (ElevationEncoderData != null)
            {
                double dbl;

                var values = File.ReadAllLines(DataDirectory + "TestElEnc.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                ElevationEncoderData = values.ToArray();
            }
            
            if (AzimuthEncoderData != null)
            {
                double dbl;

                var values = File.ReadAllLines(DataDirectory + "TestAzEnc.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                AzimuthEncoderData = values.ToArray();
            }

            if (AmbientTempData != null)
            {
                float flt;

                var values = File.ReadAllLines(DataDirectory + "TestAmbTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => float.TryParse(str, out flt) ? flt : 0));

                AmbientTempData = values.ToArray();
            }

            if (AmbientHumidityData != null)
            {
                float flt;

                var values = File.ReadAllLines(DataDirectory + "TestAmbHumidity.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => float.TryParse(str, out flt) ? flt : 0));

                AmbientHumidityData = values.ToArray();
            }
        }
    }
}
