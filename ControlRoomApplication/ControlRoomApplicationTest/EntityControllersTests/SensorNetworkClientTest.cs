﻿using ControlRoomApplication.Controllers.SensorNetwork;
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

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class SensorNetworkClientTest
    {

        // These are fields that will remain the same for all tests
        string IpAddress = "127.0.0.1";
        int TelescopeId = 5;
        int Port = 3000;
        SensorNetworkClient client;

        [TestInitialize]
        public void Initialize()
        {
            client = new SensorNetworkClient(IpAddress, Port, TelescopeId);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseOperations.DeleteSensorNetworkConfig(client.config);
        }

        [TestMethod]
        public void TestSensorNetworkClient_SendSensorInitializationAllBytes1_CorrectDataReceived()
        {
            byte[] result = new byte[0];

            // We must create a separate thread for the simulated server to run on
            Thread snServer = new Thread(() => {

                // Start server
                TcpListener listener = new TcpListener(IPAddress.Parse(IpAddress), Port);
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

            // Start server, which will begin waiting for the init byte array
            snServer.Start();

            // Send the init byte array
            bool succeeded = client.SendSensorInitialization();
            
            snServer.Join();

            // This is what we expect to see on the other end
            var expected = client.config.GetSensorInitAsBytes();

            Assert.IsTrue(succeeded);
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestSensorNetworkClient_SendSensorInitializationAllBytes0_CorrectDataReceived()
        {
            byte[] result = new byte[0];

            // We must create a separate thread for the simulated server to run on
            Thread snServer = new Thread(() => {

                // Start server
                TcpListener listener = new TcpListener(IPAddress.Parse(IpAddress), Port);
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

            // Start server, which will begin waiting for the init byte array
            snServer.Start();

            // Flip sensor initialization bytes to 0
            client.config.ElevationTemp1Init = false;
            client.config.ElevationTemp2Init = false;
            client.config.AzimuthTemp1Init = false;
            client.config.AzimuthTemp2Init = false;
            client.config.AzimuthAccelerometerInit = false;
            client.config.ElevationAccelerometerInit = false;
            client.config.CounterbalanceAccelerometerInit = false;
            client.config.ElevationEncoderInit = false;
            client.config.AzimuthEncoderInit = false;

            // Send the init byte array
            bool succeeded = client.SendSensorInitialization();

            snServer.Join();

            // This is what we expect to see on the other end
            var expected = client.config.GetSensorInitAsBytes();

            Assert.IsTrue(succeeded);
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestSensorNetworkClient_NoServerToConnectTo_Fail()
        {
            // This will fail because there is no server to connect to
            bool succeeded = client.SendSensorInitialization();

            Assert.IsFalse(succeeded);
        }
    }
}
