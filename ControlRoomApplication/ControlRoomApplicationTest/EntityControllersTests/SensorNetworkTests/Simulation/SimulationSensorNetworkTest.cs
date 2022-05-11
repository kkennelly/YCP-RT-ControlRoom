using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Controllers.SensorNetwork.Simulation;
using ControlRoomApplication.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests.Simulation
{
    [TestClass]
    public class SimulationSensorNetworkTest
    {
        string ClientIP = "127.0.0.1";
        int ClientPort = 3000;

        IPAddress ServerIP = IPAddress.Parse("127.0.0.2");
        int ServerPort = 3001;

        int TelescopeId = 3000;

        string DataPath = "../../EntityControllersTests/SensorNetworkTests/Simulation/TestCSVData/";

        SimulationSensorNetwork SimSensorNetwork;
        PrivateObject PrivSim;

        SensorNetworkServer SensorNetworkServer;
        PrivateObject PrivServer;

        [TestInitialize]
        public void Initialize()
        {
            SimSensorNetwork = new SimulationSensorNetwork(ClientIP, ClientPort, ServerIP, ServerPort, DataPath);
            PrivSim = new PrivateObject(SimSensorNetwork);

            SensorNetworkServer = new SensorNetworkServer(IPAddress.Parse(ClientIP), ClientPort, ServerIP.ToString(), ServerPort, TelescopeId, false);
            PrivServer = new PrivateObject(SensorNetworkServer);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseOperations.DeleteSensorNetworkConfig(SensorNetworkServer.InitializationClient.SensorNetworkConfig);
        }

        [TestMethod]
        public void SimulationSensorNetwork_Constructor_AllFieldsCorrect()
        {
            // Gather data
            TcpListener resultListener = (TcpListener)PrivSim.GetFieldOrProperty("Server");
            string resultClientIP = (string)PrivSim.GetFieldOrProperty("ClientIP");
            int resultClientPort = (int)PrivSim.GetFieldOrProperty("ClientPort");
            string resultDataDirectory = (string)PrivSim.GetFieldOrProperty("DataDirectory");

            // Verify server
            Assert.IsNotNull(resultListener);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Address, ServerIP);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Port, ServerPort);

            // Verify client
            Assert.AreEqual(ClientIP, resultClientIP);
            Assert.AreEqual(ClientPort, resultClientPort);

            // Verify directory
            Assert.AreEqual(DataPath, resultDataDirectory);
        }

        [TestMethod]
        public void TestInitializeSensors_InitializeAllSensors_AllSensorsInitialized()
        {
            // Create an initialization that will enable all sensors.
            byte[] init = new byte[9];
            for(int i = 0; i < init.Length; i++) init[i] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableElTemp_OnlyElTempDisabled()
        {
            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.IsNull(resultElTemp);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAzTemp_OnlyAzTempDisabled()
        {
            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.IsNull(resultAzTemp);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableElEncoder_OnlyElEncoderDisabled()
        {
            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.IsNull(resultElEnc);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAzEncoder_OnlyAzEncoderDisabled()
        {
            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.IsNull(resultAzEnc);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAzAcc_OnlyAzAccDisabled()
        {
            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.IsNull(resultAzAcc);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableElAcc_OnlyElAccDisabled()
        {
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.IsNull(resultElAcc);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableCbAcc_OnlyCbAccDisabled()
        {
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.IsNull(resultCbAcc);
            Assert.AreEqual(expectedArrLength, resultAmbTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAmbHumidity.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAmbTempHumidity_OnlyAmbTempHumidityDisabled()
        {
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAllSensors_AllSensorsDisabled()
        {
            // Create an initialization
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            // Verify all arrays still have a length of 0 except the null one
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_ElTemp_ElTempsAreCorrect()
        {
            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            double[] expectedArray = new double[] { 1, 2 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultElTemp));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AzTemp_AzTempsAreCorrect()
        {
            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            double[] expectedArray = new double[] { 2, 3 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultAzTemp));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_ElEnc_ElEncPosAreCorrect()
        {
            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            double[] expectedArray = new double[] { 3, 4 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultElEnc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AzEnc_AzEncPosAreCorrect()
        {
            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            double[] expectedArray = new double[] { 4, 5 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultAzEnc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AzAcc_AzAccAreCorrect()
        {
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            RawAccelerometerData[] expectedAcc = new RawAccelerometerData[400];

            for (int i = 0; i < 400; i++)
            {
                if (i <= 199)
                {
                    expectedAcc[i].X = 6;
                    expectedAcc[i].Y = 13;
                    expectedAcc[i].Z = 11;
                }
                else
                {
                    expectedAcc[i].X = 5;
                    expectedAcc[i].Y = 12;
                    expectedAcc[i].Z = 10;
                }
            }

            // Verify the result array is as expected
            Assert.IsTrue(expectedAcc.SequenceEqual(resultAzAcc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_ElAcc_ElAccAreCorrect()
        {
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            RawAccelerometerData[] expectedAcc = new RawAccelerometerData[400];

            for (int i = 0; i < 400; i++)
            {
                if (i <= 199)
                {
                    expectedAcc[i].X = 6;
                    expectedAcc[i].Y = 13;
                    expectedAcc[i].Z = 11;
                }
                else
                {
                    expectedAcc[i].X = 5;
                    expectedAcc[i].Y = 12;
                    expectedAcc[i].Z = 10;
                }
            }

            // Verify the result array is as expected
            Assert.IsTrue(expectedAcc.SequenceEqual(resultElAcc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultCbAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_CbAcc_CbAccAreCorrect()
        {
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 0;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            RawAccelerometerData[] expectedAcc = new RawAccelerometerData[400];

            for (int i = 0; i < 400; i++)
            {
                if (i <= 199)
                {
                    expectedAcc[i].X = 6;
                    expectedAcc[i].Y = 13;
                    expectedAcc[i].Z = 11;
                }
                else
                {
                    expectedAcc[i].X = 5;
                    expectedAcc[i].Y = 12;
                    expectedAcc[i].Z = 10;
                }
            }

            // Verify the result array is as expected
            Assert.IsTrue(expectedAcc.SequenceEqual(resultCbAcc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultAmbTemp);
            Assert.IsNull(resultAmbHumidity);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AmbTempHumidity_AmbTempHumidityAreCorrect()
        {
            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.InitPacketSize];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;
            init[(int)SensorInitializationEnum.AmbientTempHumidity] = 1;

            PrivSim.Invoke("InitializeSensors", init);

            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            float[] resultAmbTemp = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] resultAmbHumidity = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");

            float[] expectedTempArray = new float[] { 5, 6 };
            float[] expectedHumidityArray = new float[] { 6, 7 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedTempArray.SequenceEqual(resultAmbTemp));
            Assert.IsTrue(expectedHumidityArray.SequenceEqual(resultAmbHumidity));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestWaitForAndConnectToServer_ConnectToServer_AsksForConfiguration()
        {
            // Must be currently running to do this
            PrivSim.SetFieldOrProperty("CurrentlyRunning", true);

            byte[] expected = Encoding.ASCII.GetBytes("Send Sensor Configuration");
            byte[] result = new byte[expected.Length];

            // First create server that expects the "Send Sensor Configuration" message
            Thread expectConfThread = new Thread(() =>
            {
                TcpListener confListen = new TcpListener(IPAddress.Parse(ClientIP), ClientPort);
                confListen.Start();

                NetworkStream confStream;
                TcpClient localClient = confListen.AcceptTcpClient();

                confStream = localClient.GetStream();
                confStream.Read(result, 0, expected.Length);

                confListen.Stop();
                confStream.Close();
                confStream.Dispose();
            });
            expectConfThread.Start();

            // This method has a blocking method, so we must run it in a separate thread
            Thread serverThread = new Thread(() => {
                // Set up client on the simulation
                PrivSim.Invoke("WaitForAndConnectToServer");
            });
            serverThread.Start();

            expectConfThread.Join();

            // Stop the server in the simulation
            ((TcpListener)PrivSim.GetFieldOrProperty("Server")).Stop();
            serverThread.Join();

            Assert.IsTrue(expected.SequenceEqual(result));
        }
        
        [TestMethod]
        public void TestRequestAndAcquireSensorInitialization_SendsConfiguration_ReceiveCorrectBytes()
        {
            // Must be currently running to do this
            PrivSim.SetFieldOrProperty("CurrentlyRunning", true);

            // First create server that expects the "Send Sensor Configuration" message
            // This only takes in the first
            Thread expectConfThread = new Thread(() =>
            {

                TcpListener confListen = new TcpListener(IPAddress.Parse(ClientIP), ClientPort);
                confListen.Start();

                NetworkStream confStream;
                TcpClient localClient = confListen.AcceptTcpClient();

                byte[] dontCareVal = new byte[1];

                confStream = localClient.GetStream();
                confStream.Read(dontCareVal, 0, dontCareVal.Length);

                confListen.Stop();
                confStream.Close();
                confStream.Dispose();
            });
            expectConfThread.Start();

            byte[] expected = new byte[SensorNetworkConstants.InitPacketSize];
            byte[] result = new byte[expected.Length];

            // This method has a blocking method, so we must run it in a separate thread
            Thread serverThread = new Thread(() => {

                // Set up client on the simulation
                PrivSim.Invoke("WaitForAndConnectToServer");

                result = (byte[])PrivSim.Invoke("RequestAndAcquireSensorInitialization");
            });
            serverThread.Start();

            expectConfThread.Join();

            // Create a client to send something to the server
            // We are using the server IP and port because these must be the same
            // between client/server in order to transmit data
            TcpClient client = new TcpClient(ServerIP.ToString(), ServerPort);
            NetworkStream stream = client.GetStream();
            stream.Write(expected, 0, expected.Length);

            //stream.Write(expected, 0, expected.Length);
            stream.Flush();

            // Dispose client and end thread
            serverThread.Join();
            stream.Close();
            stream.Dispose();
            client.Close();
            client.Dispose();

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestStartSimulationSensorNetwork_Starts_AllObjectsAsExpected()
        {
            SimSensorNetwork.StartSimulationSensorNetwork();

            bool resultCurrentlyRunning = (bool)PrivSim.GetFieldOrProperty("CurrentlyRunning");
            Thread resultMonitoringThread = (Thread)PrivSim.GetFieldOrProperty("SimulationSensorMonitoringThread");

            Assert.IsTrue(resultCurrentlyRunning);
            Assert.IsTrue(resultMonitoringThread.IsAlive);

            SimSensorNetwork.EndSimulationSensorNetwork();
        }

        [TestMethod]
        public void TestEndSimulationSensorNetwork_Ends_AllObjectsBroughtDown()
        {
            SensorNetworkServer.StartSensorMonitoringRoutine();
            SimSensorNetwork.StartSimulationSensorNetwork();

            // Give plenty of time for everything to connect
            Thread.Sleep(2000);

            SimSensorNetwork.EndSimulationSensorNetwork();
            SensorNetworkServer.EndSensorMonitoringRoutine();

            bool resultCurrentlyRunning = (bool)PrivSim.GetFieldOrProperty("CurrentlyRunning");
            Thread resultMonitoringThread = (Thread)PrivSim.GetFieldOrProperty("SimulationSensorMonitoringThread");
            TcpClient resultClient = (TcpClient)PrivSim.GetFieldOrProperty("Client");
            NetworkStream resultClientStream = (NetworkStream)PrivSim.GetFieldOrProperty("ClientStream");
            TcpListener resultServer = (TcpListener)PrivSim.GetFieldOrProperty("Server");
            NetworkStream resultServerStream = (NetworkStream)PrivSim.GetFieldOrProperty("ServerStream");

            Assert.IsFalse(resultCurrentlyRunning);
            Assert.IsFalse(resultMonitoringThread.IsAlive);
            Assert.IsFalse(resultClient.Connected);
            Assert.IsFalse(resultClientStream.CanWrite);
            Assert.IsFalse(resultServer.Server.IsBound);
            Assert.IsFalse(resultServerStream.CanRead);
        }

        [TestMethod]
        public void TestBuildSubArrays_AzimuthTemp_AzimuthTempIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("AzimuthTempData", new double[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            double[] initArray = (double[])PrivSim.GetFieldOrProperty("AzimuthTempData");
            double[] expectedArray = new double[initArray.Length + 1]; // + 1 is to test looping back to the beginning
            initArray.CopyTo(expectedArray, 0);
            expectedArray[expectedArray.Length - 1] = initArray[0];

            double[] resultArray = new double[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;
            
            for (int i = 0; i < expectedArray.Length; i++) {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref index,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt
                );

                resultArray[i] = subArrays.AzimuthTemps[subArrays.AzimuthTemps.Length - 1];
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_ElevationTemp_ElevationTempIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("ElevationTempData", new double[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            double[] initArray = (double[])PrivSim.GetFieldOrProperty("ElevationTempData");
            double[] expectedArray = new double[initArray.Length + 1]; // + 1 is to test looping back to the beginning
            initArray.CopyTo(expectedArray, 0);
            expectedArray[expectedArray.Length - 1] = initArray[0];

            double[] resultArray = new double[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i++)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref index,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt
                );

                resultArray[i] = subArrays.ElevationTemps[subArrays.ElevationTemps.Length - 1];
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_ElevationEncoder_ElevationEncoderIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("ElevationEncoderData", new double[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            double[] initArray = (double[])PrivSim.GetFieldOrProperty("ElevationEncoderData");
            double[] expectedArray = new double[initArray.Length + 1]; // + 1 is to test looping back to the beginning
            initArray.CopyTo(expectedArray, 0);
            expectedArray[expectedArray.Length - 1] = initArray[0];

            double[] resultArray = new double[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i++)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref index,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt
                );

                resultArray[i] = subArrays.ElevationEnc[subArrays.ElevationEnc.Length - 1];
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_AzimuthEncoder_AzimuthEncoderIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("AzimuthEncoderData", new double[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            double[] initArray = (double[])PrivSim.GetFieldOrProperty("AzimuthEncoderData");
            double[] expectedArray = new double[initArray.Length + 1]; // + 1 is to test looping back to the beginning
            initArray.CopyTo(expectedArray, 0);
            expectedArray[expectedArray.Length - 1] = initArray[0];

            double[] resultArray = new double[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i++)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref index,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt
                );

                resultArray[i] = subArrays.AzimuthEnc[subArrays.AzimuthEnc.Length - 1];
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_ElevationAccelerometer_ElevationAccelerometerIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("ElevationAccData", new RawAccelerometerData[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            RawAccelerometerData[] initArray = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] expectedArray = new RawAccelerometerData[initArray.Length + 200]; // + 200 is to test looping back to the beginning

            // Copy original array to expected array
            initArray.CopyTo(expectedArray, 0);

            // Get first half of the initial array for looping back
            Array.Copy(initArray, 0, expectedArray, 400, 200);

            RawAccelerometerData[] resultArray = new RawAccelerometerData[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i += 100)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref index,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt
                );

                subArrays.ElevationAccl.CopyTo(resultArray, i);
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_AzimuthAccelerometer_AzimuthAccelerometerIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("AzimuthAccData", new RawAccelerometerData[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            RawAccelerometerData[] initArray = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] expectedArray = new RawAccelerometerData[initArray.Length + 200]; // + 200 is to test looping back to the beginning

            // Copy original array to expected array
            initArray.CopyTo(expectedArray, 0);

            // Get first half of the initial array for looping back
            Array.Copy(initArray, 0, expectedArray, 400, 200);

            RawAccelerometerData[] resultArray = new RawAccelerometerData[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i += 100)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref index,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt
                );

                subArrays.AzimuthAccl.CopyTo(resultArray, i);
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_CounterbalanceAccelerometer_CounterbalanceAccelerometerIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("CounterbalanceAccData", new RawAccelerometerData[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            RawAccelerometerData[] initArray = (RawAccelerometerData[])PrivSim.GetFieldOrProperty("CounterbalanceAccData");
            RawAccelerometerData[] expectedArray = new RawAccelerometerData[initArray.Length + 200]; // + 200 is to test looping back to the beginning

            // Copy original array to expected array
            initArray.CopyTo(expectedArray, 0);

            // Get first half of the initial array for looping back
            Array.Copy(initArray, 0, expectedArray, 400, 200);

            RawAccelerometerData[] resultArray = new RawAccelerometerData[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i += 100)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref index,
                    ref nullInt,
                    ref nullInt
                );

                subArrays.CounterBAccl.CopyTo(resultArray, i);
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_AmbientTemperature_AmbientTemperatureIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("AmbientTempData", new float[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            float[] initArray = (float[])PrivSim.GetFieldOrProperty("AmbientTempData");
            float[] expectedArray = new float[initArray.Length + 1]; // + 1 is to test looping back to the beginning
            initArray.CopyTo(expectedArray, 0);
            expectedArray[expectedArray.Length - 1] = initArray[0];

            float[] resultArray = new float[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i++)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref index,
                    ref nullInt
                );

                resultArray[i] = subArrays.AmbientTemps[subArrays.AmbientTemps.Length - 1];
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestBuildSubArrays_AmbientHumidity_AmbientHumidityIterates()
        {
            // "Initialize" sensor
            PrivSim.SetFieldOrProperty("AmbientHumidityData", new float[0]);

            // Populate initialized sensor
            PrivSim.Invoke("ReadFakeDataFromCSV");

            // Retrieve populated sensor
            float[] initArray = (float[])PrivSim.GetFieldOrProperty("AmbientHumidityData");
            float[] expectedArray = new float[initArray.Length + 1]; // + 1 is to test looping back to the beginning
            initArray.CopyTo(expectedArray, 0);
            expectedArray[expectedArray.Length - 1] = initArray[0];

            float[] resultArray = new float[expectedArray.Length];

            int? index = 0;
            int? nullInt = null;
            SimulationSubArrayData subArrays;

            for (int i = 0; i < expectedArray.Length; i++)
            {
                subArrays = SimSensorNetwork.BuildSubArrays(
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref nullInt,
                    ref index
                );

                resultArray[i] = subArrays.AmbientHumidity[subArrays.AmbientHumidity.Length - 1];
            }

            Assert.IsTrue(expectedArray.SequenceEqual(resultArray));
        }

        [TestMethod]
        public void TestSimulationSensorMonitor_SimulationIsRunning_ConnectsToServerAndSendsData()
        {
            SensorNetworkServer.StartSensorMonitoringRoutine();
            SimSensorNetwork.StartSimulationSensorNetwork();

            // Give plenty of time for everything to connect
            Thread.Sleep(2000);

            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, SensorNetworkServer.Status);
            
            SimSensorNetwork.EndSimulationSensorNetwork();
            SensorNetworkServer.EndSensorMonitoringRoutine();
        }

        [TestMethod]
        public void TestSimulationSensorMonitor_ServerTriggersReboot_SimulationRebootsAndStartsSendingData()
        {
            SensorNetworkServer.StartSensorMonitoringRoutine();
            SimSensorNetwork.StartSimulationSensorNetwork();

            // Give plenty of time for everything to connect
            Thread.Sleep(2000);

            SensorNetworkServer.RebootSensorNetwork();

            // Give plenty of time for timeout and for initialization
            Thread.Sleep(2000 + SensorNetworkConstants.WatchDogTimeout);

            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, SensorNetworkServer.Status);

            SimSensorNetwork.EndSimulationSensorNetwork();
            SensorNetworkServer.EndSensorMonitoringRoutine();
        }
    }
}
