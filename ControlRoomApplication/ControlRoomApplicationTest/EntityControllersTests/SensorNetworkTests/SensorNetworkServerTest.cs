using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class SensorNetworkServerTest
    {

        // These are fields that will remain the same for all tests
        IPAddress ServerIpAddress = IPAddress.Parse("127.0.0.1");
        string ClientIpAddress = "127.0.0.2";
        int ClientPort = 3000;
        int ServerPort = 3001;
        int TelescopeId = 5;
        bool IsSimulation = false;
        SensorNetworkServer Server;
        string TestPacketDirectory = "../../EntityControllersTests/SensorNetworkTests/SensorNetworkTestPackets/";


        [TestInitialize]
        public void Initialize()
        {
            Server = new SensorNetworkServer(ServerIpAddress, ServerPort, ClientIpAddress, ClientPort, TelescopeId, IsSimulation);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseOperations.DeleteSensorNetworkConfig(Server.InitializationClient.SensorNetworkConfig);
        }

        [TestMethod]
        public void TestSensorNetworkServer_Initialization_Success()
        {
            PrivateObject privServer = new PrivateObject(Server);
            PrivateObject privClient = new PrivateObject(Server.InitializationClient);

            // Arrange/Act
            TcpListener resultListener = (TcpListener)privServer.GetFieldOrProperty("Server");
            Thread resultMonitorThread = (Thread)privServer.GetFieldOrProperty("SensorMonitoringThread");
            System.Timers.Timer resultTimer = (System.Timers.Timer)privServer.GetFieldOrProperty("Timeout");

            // Verify SensorNetworkServer is correct
            Assert.IsNotNull(resultListener);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Address, ServerIpAddress);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Port, ServerPort);

            // Verify SensorNetworkClient is correct
            Assert.IsNotNull(Server.InitializationClient);
            Assert.AreEqual((string)privClient.GetFieldOrProperty("IPAddress"), ClientIpAddress);
            Assert.AreEqual((int)privClient.GetFieldOrProperty("Port"), ClientPort);
            Assert.AreEqual(Server.InitializationClient.SensorNetworkConfig.TelescopeId, TelescopeId);

            // Verify temperatures are correct
            Assert.IsNotNull(Server.CurrentElevationMotorTemp);
            Assert.IsNotNull(Server.CurrentAzimuthMotorTemp);

            // Verify ambient temp and humidity are correct
            Assert.IsNotNull(Server.CurrentElevationAmbientTemp);
            Assert.IsNotNull(Server.CurrentElevationAmbientHumidity);

            // Verify orientation is correct
            Assert.IsNotNull(Server.CurrentAbsoluteOrientation);

            // Verify acceleration is correct
            Assert.IsNotNull(Server.CurrentAzimuthMotorAccl);
            Assert.IsNotNull(Server.CurrentElevationMotorAccl);
            Assert.IsNotNull(Server.CurrentCounterbalanceAccl);

            // Verify thread is correct
            Assert.IsNotNull(resultMonitorThread);
            Assert.AreEqual(resultMonitorThread.Name, "SensorMonitorThread");

            // Verify timer is correct
            Assert.IsNotNull(resultTimer);
            Assert.IsFalse(resultTimer.AutoReset);
        }

        [TestMethod]
        public void TestInterpretData_AllSensorsInitialized_InterpretsData()
        {
            // This is a test packet where all sensors are initialized, and 
            // all of their values are set to 50
            byte[] AllData50 = File.ReadAllBytes($"{TestPacketDirectory}AllData50.snp");

            PrivateObject privServer = new PrivateObject(Server);

            int expectedValue = 50;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", AllData50, AllData50.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, expectedValue, 0.1);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, expectedValue, 0.1);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, expectedValue, 0.1);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, expectedValue, 0.1);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, expectedValue);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, expectedValue);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, expectedValue);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, expectedValue);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, expectedValue);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, expectedValue);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, expectedValue);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, expectedValue);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, expectedValue);

            // 0.1 is the approximate margin of error with the conversions
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, expectedValue, 0.16);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, expectedValue, 0.16);
        }

        [TestMethod]
        public void TestInterpretData_OnlyElTemp_InterpretsOnlyElTemp()        {
            // Only the elevation temperature is initialized and should decode to the temp in Celsius
            byte[] OnlyElTemp50C = File.ReadAllBytes($"{TestPacketDirectory}OnlyElTemp50C.snp");

            PrivateObject privServer = new PrivateObject(Server);

            int expectedValue = 50;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyElTemp50C, OnlyElTemp50C.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, expectedValue, 0.1);

            // The rest of the values should be EMPTY (aka 0)
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);
            
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_OnlyAzTemp_InterpretsOnlyAzTemp()
        {
            // Only the azimuth temperature is initialized and should decode to the temp in Celsius
            byte[] OnlyAzTemp50C = File.ReadAllBytes($"{TestPacketDirectory}OnlyAzTemp50C.snp");

            PrivateObject privServer = new PrivateObject(Server);

            int expectedValue = 50;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyAzTemp50C, OnlyAzTemp50C.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, expectedValue, 0.1); // This should be the only populated value

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_OnlyAmbTempHumidity_InterpretsOnlyAmbTempHumidity()
        {
            // Only the ambient temperature and humidity are initialized and should decode to the temp in Fahrenheit and % humidity
            byte[] OnlyAmbTempHumidity50 = File.ReadAllBytes($"{TestPacketDirectory}OnlyAmbTempHumidity50.snp");

            PrivateObject privServer = new PrivateObject(Server);

            int expectedValue = 50;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyAmbTempHumidity50, OnlyAmbTempHumidity50.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            // These should be the only populated values
            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, expectedValue, 0.1);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, expectedValue, 0.1);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_OnlyAzEncoder_InterpretsOnlyAzEncoder()
        {
            // Only the azimuth encoder is initialized
            byte[] OnlyAzEnc = File.ReadAllBytes($"{TestPacketDirectory}OnlyAzEnc50.snp");

            PrivateObject privServer = new PrivateObject(Server);

            int expectedValue = 50;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyAzEnc, OnlyAzEnc.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 50, 0.17); // This should be the only populated value
        }

        [TestMethod]
        public void TestInterpretData_OnlyElEncoder_InterpretsOnlyElEncoder()
        {
            // Only the elevation encoder is initialized
            byte[] OnlyElEnc = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc50.snp");

            PrivateObject privServer = new PrivateObject(Server);

            int expectedValue = 50;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyElEnc, OnlyElEnc.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 50, 0.17); // This should be the only populated value
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_OnlyElAccl_InterpretsOnlyElAccl()
        {
            // Only the elevation accelerometer is initialized
            byte[] OnlyElAccl123 = File.ReadAllBytes($"{TestPacketDirectory}OnlyElAccl123.snp");

            PrivateObject privServer = new PrivateObject(Server);

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyElAccl123, OnlyElAccl123.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            // These should be the only populated values, based off of the pre-made
            // packet I made for them
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 1);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 2);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 3);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_OnlyAzAccl_InterpretsOnlyAzAccl()
        {
            // Only the azimuth accelerometer is initialized
            byte[] OnlyAzAccl123 = File.ReadAllBytes($"{TestPacketDirectory}OnlyAzAccl123.snp");

            PrivateObject privServer = new PrivateObject(Server);

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyAzAccl123, OnlyAzAccl123.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);
            
            // These should be the only populated values, based off of the pre-made
            // packet I made for them
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 1);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 2);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 3);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_OnlyCbAccl_InterpretsOnlyCbAccl()
        {
            // Only the counterbalance accelerometer is initialized
            byte[] OnlyCbAccl123 = File.ReadAllBytes($"{TestPacketDirectory}OnlyCbAccl123.snp");

            PrivateObject privServer = new PrivateObject(Server);

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", OnlyCbAccl123, OnlyCbAccl123.Length);

            Assert.IsTrue(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            // These should be the only populated values, based off of the pre-made
            // packet I made for them
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 1);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 2);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 3);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_RequestInitialization_SendsCorrectInitialization()
        {
            PrivateObject privServer = new PrivateObject(Server);

            byte[] result = new byte[0];

            // We must create a separate thread for the simulated server to run on because we are
            // expecting data from the client
            Thread snServer = new Thread(() => {

                // Start server using the client IP address because those values have to be the same
                // in order for us to receive anything
                TcpListener listener = new TcpListener(IPAddress.Parse(ClientIpAddress), ClientPort);
                listener.Start();

                // Create local client/stream to receive data
                TcpClient localClient = listener.AcceptTcpClient();
                NetworkStream stream = localClient.GetStream();

                // This will hold our sensor initialization
                byte[] bytes = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

                // Wait for initialization
                stream.Read(bytes, 0, bytes.Length);

                // If we reach here, we've received data
                result = bytes;

                localClient.Close();
                localClient.Dispose();
                stream.Close();
                stream.Dispose();
                listener.Stop();
            });
            snServer.Start();

            // Prepare the config request
            var packetToSend = Encoding.ASCII.GetBytes("Send Sensor Configuration");

            // Call the function, will cause the initialization to be sent to the server
            bool success = (bool)privServer.Invoke("InterpretData", packetToSend, packetToSend.Length);

            snServer.Join();

            Assert.IsTrue(success);

            var expected = Server.InitializationClient.SensorNetworkConfig.GetSensorInitAsBytes();

            // Verify that we received the sensor initialization that the client had
            Assert.IsTrue(expected.SequenceEqual(result));

            // Verify NO values get anything
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);
            
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_RequestInitializationFailed_StatusSetToInitFailed()
        {
            PrivateObject privServer = new PrivateObject(Server);

            // Prepare the config request
            var packetToSend = Encoding.ASCII.GetBytes("Send Sensor Configuration");

            // Call the function, will cause the initialization to be sent to the server
            bool success = (bool)privServer.Invoke("InterpretData", packetToSend, packetToSend.Length);

            Assert.IsFalse(success);

            // Check that the status is what we expect
            Assert.AreEqual(Server.Status, SensorNetworkStatusEnum.InitializationSendingFailed);

            // Verify NO values get anything
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_WrongTransitId_StatusTransitIdError()
        {
            byte[] AnyPacket = File.ReadAllBytes($"{TestPacketDirectory}OnlyCbAccl123.snp");

            PrivateObject privServer = new PrivateObject(Server);

            // Set the transmit ID to something invalid
            AnyPacket[0] = 5;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", AnyPacket, AnyPacket.Length);

            Assert.IsFalse(success);

            // Verify the status is set to unknown error
            Assert.AreEqual(Server.Status, SensorNetworkStatusEnum.TransitIdError);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);
            
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestInterpretData_PacketSizeNotCorrect_Fail()
        {
            byte[] AnyPacket = File.ReadAllBytes($"{TestPacketDirectory}OnlyCbAccl123.snp");

            PrivateObject privServer = new PrivateObject(Server);

            // Change the inner packet size so it doesn't match the length of the byte array
            AnyPacket[1] = 255;

            // Call the function, which decodes the data
            bool success = (bool)privServer.Invoke("InterpretData", AnyPacket, AnyPacket.Length);

            Assert.IsFalse(success);

            // Verify all of the values are as expected; converted values have a small margin of error
            Assert.AreEqual(Server.CurrentElevationMotorTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorTemp[0].temp, 0);

            Assert.AreEqual(Server.CurrentElevationAmbientTemp[0].temp, 0);
            Assert.AreEqual(Server.CurrentElevationAmbientHumidity[0].HumidityReading, 0);

            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentElevationMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentAzimuthMotorAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].x, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].y, 0);
            Assert.AreEqual(Server.CurrentCounterbalanceAccl[0].z, 0);

            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Elevation, 0);
            Assert.AreEqual(Server.CurrentAbsoluteOrientation.Azimuth, 0);
        }

        [TestMethod]
        public void TestTimedOut_StatusReceivingData_SetsToTimedOutReceivingData()
        {
            PrivateObject privServer = new PrivateObject(Server);
            
            Server.Status = SensorNetworkStatusEnum.ReceivingData;

            privServer.Invoke("TimedOut", new Object(), null);

            Assert.AreEqual(SensorNetworkStatusEnum.TimedOutDataRetrieval, Server.Status);
        }

        [TestMethod]
        public void TestTimedOut_StatusInitializing_SetsToTimedOutInitialization()
        {
            PrivateObject privServer = new PrivateObject(Server);

            Server.Status = SensorNetworkStatusEnum.Initializing;

            privServer.Invoke("TimedOut", new Object(), null);

            Assert.AreEqual(SensorNetworkStatusEnum.TimedOutInitialization, Server.Status);
        }

        [TestMethod]
        public void TestTimedOut_StatusNotInitializingOrReceivingData_SetsToServerError()
        {
            PrivateObject privServer = new PrivateObject(Server);

            Server.Status = SensorNetworkStatusEnum.ServerError;

            privServer.Invoke("TimedOut", new Object(), null);

            Assert.AreEqual(SensorNetworkStatusEnum.UnknownError, Server.Status);
        }

        [TestMethod]
        public void TestStartSensorMonitoringRoutine_RoutineStarts_AllValuesSet()
        {
            PrivateObject privServer = new PrivateObject(Server);

            Server.StartSensorMonitoringRoutine();

            // Retrieve any private fields
            TcpListener resultServer = (TcpListener)privServer.GetFieldOrProperty("Server");
            bool resultCurrentlyRunning = (bool)privServer.GetFieldOrProperty("CurrentlyRunning");
            System.Timers.Timer resultTimer = (System.Timers.Timer)privServer.GetFieldOrProperty("Timeout");
            Thread resultMonitorThread = (Thread)privServer.GetFieldOrProperty("SensorMonitoringThread");

            // Verify all fields are as we expect them to be.
            Assert.IsTrue(resultServer.Server.IsBound);

            Assert.IsTrue(resultCurrentlyRunning);

            Assert.AreEqual(SensorNetworkStatusEnum.Initializing, Server.Status);

            Assert.AreEqual(SensorNetworkConstants.DefaultInitializationTimeout, resultTimer.Interval);
            Assert.IsTrue(resultTimer.Enabled);

            Assert.IsTrue(resultMonitorThread.IsAlive);

            // Stop the monitoring routine. We aren't testing this, so no asserts are being checked here
            Server.EndSensorMonitoringRoutine();
        }

        [TestMethod]
        public void TestEndSensorMonitoringRoutine_RoutineEnds_AllValuesSet()
        {
            PrivateObject privServer = new PrivateObject(Server);

            // Now this is the one we will not have any asserts for, because we are not testing it in this function
            Server.StartSensorMonitoringRoutine();

            Server.EndSensorMonitoringRoutine();

            // Retrieve any private fields
            TcpListener resultServer = (TcpListener)privServer.GetFieldOrProperty("Server");
            bool resultCurrentlyRunning = (bool)privServer.GetFieldOrProperty("CurrentlyRunning");
            System.Timers.Timer resultTimer = (System.Timers.Timer)privServer.GetFieldOrProperty("Timeout");
            Thread resultMonitorThread = (Thread)privServer.GetFieldOrProperty("SensorMonitoringThread");

            // Verify everything is brought down correctly (fields are as we expect them to be)
            Assert.IsFalse(resultServer.Server.IsBound);

            Assert.IsFalse(resultCurrentlyRunning);

            Assert.IsFalse(resultMonitorThread.IsAlive);

            Assert.AreEqual(SensorNetworkStatusEnum.None, Server.Status);

            // Unfortunately, there is no good way to check if an object has been disposed, so we will just
            // have to assume that the Timeout object is being disposed properly.

            Assert.IsFalse(resultTimer.Enabled);
        }

        [TestMethod]
        public void TestEndSensorMonitoringRoutine_RebootingFlagTrue_AllValuesSetWithRebootingStatus()
        {
            PrivateObject privServer = new PrivateObject(Server);

            // Now this is the one we will not have any asserts for, because we are not testing it in this function
            Server.StartSensorMonitoringRoutine();

            Server.EndSensorMonitoringRoutine(true); // true represents it is "rebooting"

            // Retrieve any private fields
            TcpListener resultServer = (TcpListener)privServer.GetFieldOrProperty("Server");
            bool resultCurrentlyRunning = (bool)privServer.GetFieldOrProperty("CurrentlyRunning");
            System.Timers.Timer resultTimer = (System.Timers.Timer)privServer.GetFieldOrProperty("Timeout");
            Thread resultMonitorThread = (Thread)privServer.GetFieldOrProperty("SensorMonitoringThread");

            // Verify everything is brought down correctly (fields are as we expect them to be)
            Assert.IsFalse(resultServer.Server.IsBound);

            Assert.IsFalse(resultCurrentlyRunning);

            Assert.IsFalse(resultMonitorThread.IsAlive);

            Assert.AreEqual(SensorNetworkStatusEnum.Rebooting, Server.Status);

            // Unfortunately, there is no good way to check if an object has been disposed, so we will just
            // have to assume that the Timeout object is being disposed properly.

            Assert.IsFalse(resultTimer.Enabled);
        }

        [TestMethod]
        public void TestSensorMonitoringRoutine_InitializationTimeout_StatusInitializationTimeout()
        {
            Server.StartSensorMonitoringRoutine();
            
            Thread.Sleep(SensorNetworkConstants.DefaultInitializationTimeout + 250);

            Assert.AreEqual(SensorNetworkStatusEnum.TimedOutInitialization, Server.Status);

            Server.EndSensorMonitoringRoutine();
        }

        [TestMethod]
        public void TestSensorMonitoringRoutine_ReceivesFourPackets_StatusRetrievingData()
        {
            Server.StartSensorMonitoringRoutine();

            // Prepare packets and client to send. We want two packets so we can alternate
            // between them and verify data is being changed.
            byte[] OnlyElEnc50 = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc50.snp");
            byte[] OnlyElEnc25 = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc25.snp");
            TcpClient sendToServer = new TcpClient(ServerIpAddress.ToString(), ServerPort);
            NetworkStream stream = sendToServer.GetStream();

            // First packet
            stream.Write(OnlyElEnc50, 0, OnlyElEnc50.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(50, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            Thread.Sleep(500);

            // Second packet
            stream.Write(OnlyElEnc25, 0, OnlyElEnc25.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(25, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            Thread.Sleep(500);

            // Third packet
            stream.Write(OnlyElEnc50, 0, OnlyElEnc50.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(50, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            Thread.Sleep(500);

            // Last packet
            stream.Write(OnlyElEnc25, 0, OnlyElEnc25.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(25, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            // End everything
            Server.EndSensorMonitoringRoutine();
            stream.Close();
            stream.Dispose();
            sendToServer.Close();
            sendToServer.Dispose();
        }

        [TestMethod]
        public void TestSensorMonitoringRoutine_DataRetrievalTimeout_StatusDataRetrievalTimeout()
        {
            Server.StartSensorMonitoringRoutine();

            // Prepare a single packet and client to send
            byte[] OnlyElEnc = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc50.snp");
            TcpClient sendToServer = new TcpClient(ServerIpAddress.ToString(), ServerPort);
            NetworkStream stream = sendToServer.GetStream();

            // Send data to the Server
            stream.Write(OnlyElEnc, 0, OnlyElEnc.Length);
            stream.Flush();

            // Wait for the data timeout, because it is waiting for a second packet now
            Thread.Sleep(SensorNetworkConstants.DefaultDataRetrievalTimeout + 250);

            Assert.AreEqual(SensorNetworkStatusEnum.TimedOutDataRetrieval, Server.Status);

            // End everything
            Server.EndSensorMonitoringRoutine();
            stream.Close();
            stream.Dispose();
            sendToServer.Close();
            sendToServer.Dispose();
        }

        [TestMethod]
        public void TestSensorMonitoringRoutine_TimesOutThenReceivesPacket_StatusRetrievingData()
        {
            Server.StartSensorMonitoringRoutine();

            // Prepare a single packet and client to send
            byte[] OnlyElEnc = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc50.snp");
            byte[] OnlyElEnc25 = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc25.snp");
            TcpClient sendToServer = new TcpClient(ServerIpAddress.ToString(), ServerPort);
            NetworkStream stream = sendToServer.GetStream();

            // Send data to the Server
            stream.Write(OnlyElEnc, 0, OnlyElEnc.Length);
            stream.Flush();

            // Wait for the data timeout, because it is waiting for a second packet now
            Thread.Sleep(SensorNetworkConstants.DefaultDataRetrievalTimeout + 250);

            Assert.AreEqual(SensorNetworkStatusEnum.TimedOutDataRetrieval, Server.Status);

            // Now let's send another packet, which should change the status back to ReceivingData
            stream.Write(OnlyElEnc25, 0, OnlyElEnc25.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(25, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            // End everything
            Server.EndSensorMonitoringRoutine();
            stream.Close();
            stream.Dispose();
            sendToServer.Close();
            sendToServer.Dispose();
        }

        [TestMethod]
        public void TestRebootSensorNetwork_Reboots_AllFieldsSet()
        {// These fields should be the same as the ones set in StartSensorMonitoringRoutine()
            PrivateObject privServer = new PrivateObject(Server);
            Server.StartSensorMonitoringRoutine();

            Server.RebootSensorNetwork();

            // Retrieve any private fields
            TcpListener resultServer = (TcpListener)privServer.GetFieldOrProperty("Server");
            bool resultCurrentlyRunning = (bool)privServer.GetFieldOrProperty("CurrentlyRunning");
            System.Timers.Timer resultTimer = (System.Timers.Timer)privServer.GetFieldOrProperty("Timeout");
            Thread resultMonitorThread = (Thread)privServer.GetFieldOrProperty("SensorMonitoringThread");

            // Verify all fields are as we expect them to be.
            Assert.IsTrue(resultServer.Server.IsBound);

            Assert.IsTrue(resultCurrentlyRunning);

            Assert.AreEqual(SensorNetworkStatusEnum.Initializing, Server.Status);

            Assert.AreEqual(SensorNetworkConstants.DefaultInitializationTimeout, resultTimer.Interval);
            Assert.IsTrue(resultTimer.Enabled);

            Assert.IsTrue(resultMonitorThread.IsAlive);

            // Stop the monitoring routine. We aren't testing this, so no asserts are being checked here
            Server.EndSensorMonitoringRoutine();
        }

        [TestMethod]
        public void TestRebootSensorNetwork_Reboots_CanStillReceivePackets()
        {// These fields should be the same as the ones set in StartSensorMonitoringRoutine()
            PrivateObject privServer = new PrivateObject(Server);
            Server.StartSensorMonitoringRoutine();

            Server.RebootSensorNetwork();

            // Prepare packets and client to send. We want two packets so we can alternate
            // between them and verify data is being changed.
            byte[] OnlyElEnc50 = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc50.snp");
            byte[] OnlyElEnc25 = File.ReadAllBytes($"{TestPacketDirectory}OnlyElEnc25.snp");
            TcpClient sendToServer = new TcpClient(ServerIpAddress.ToString(), ServerPort);
            NetworkStream stream = sendToServer.GetStream();

            // First packet
            stream.Write(OnlyElEnc50, 0, OnlyElEnc50.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(50, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            Thread.Sleep(500);

            // Second packet
            stream.Write(OnlyElEnc25, 0, OnlyElEnc25.Length);
            stream.Flush();

            Thread.Sleep(25); // wait for data to be received

            Assert.AreEqual(25, Server.CurrentAbsoluteOrientation.Elevation, 0.17);
            Assert.AreEqual(SensorNetworkStatusEnum.ReceivingData, Server.Status);

            // Stop the monitoring routine. We aren't testing this, so no asserts are being checked here
            Server.EndSensorMonitoringRoutine();
        }
    }
}
