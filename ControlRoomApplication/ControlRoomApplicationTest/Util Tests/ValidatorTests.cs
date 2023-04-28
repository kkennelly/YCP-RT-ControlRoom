using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Validation;
using System.Net.Sockets;
using System.Net;

namespace ControlRoomApplication.ValidationTests {

    [TestClass]
    public class ValidatorTests {


        // ports for testing: valid, too high, too low,  empty, wrong type
        private int validPort1 = 8080;
        private string validPort2 = "8080";

        private int invalidPort1 = 70000;
        private string invalidPort2 = "hello";
        private int invalidPort3 = 0;
        private int invalidPort4 = -10000;
        private string invalidPort5 = "";

        // IP address for testing: valid, empty, wrong type
        private string validIP1 = "127.0.0.1";
        private string validIP2 = "10.0.2.5";

        private string invalidIP1 = "i am an invalid IP address";
        private string invalidIP2 = "";

        // Speed values for testing
        private double validSpeed1 = 0.5;
        private double validSpeed2 = 1.5;
        private string validSpeed3 = "1.5";
        private string validSpeed4 = "0.5";

        private double invalidSpeed1 = 3.0;
        private double invalidSpeed2 = -1.5;
        private string invalidSpeed3 = "";
        private string invalidSpeed4 = "3.0";

        // azSpeed TEXT ONLY values for testing
        private string validTextSpeed1 = "2.0";
        private string validTextSpeed2 = "20.0";
        private string invalidTextSpeed1 = "hello";
        private string invalidTextSpeed2 = "saSDASDS";

        // IFGain values for testing
        private double validIFGain1 = 10.50;
        private double validIFGain2 = 20.50;
        private string validIFGain3 = "10.50";
        private string validIFGain4 = "20.50";

        private double invalidIFGain1 = 50.5;
        private double invalidIFGain2 = 5.00;
        private string invalidIFGain3 = "50.00";
        private string invalidIFGain4 = "";


        // Offset Volts values testing
        private double validVolts1 = 1.0;
        private double validVolts2 = 3.5;
        private string validVolts3 = "1.0";
        private string validVolts4 = "3.5";

        private double invalidVolts1 = -1.0;
        private double invalidVolts2 = 5.0;
        private string invalidVolts3 = "5.0";
        private string invalidVolts4 = "";

        // Frequency hertz value testing
        private double validFrequency1 = 0.1;
        private double validFrequency2 = 5.0;
        private string validFrequency3 = "0.1";
        private string validFrequency4 = "0.5";

        private double invalidFrequency1 = -1.0;
        private double invalidFrequency2 = -4.1;
        private string invalidFrequency3 = "-1.0";
        private string invalidFrequency4 = "-4.1";

        [TestMethod]
        public void TestPort()
        {
            Assert.IsTrue(Validator.ValidatePort(validPort1));
            Assert.IsTrue(Validator.ValidatePort(validPort2));
            Assert.IsFalse(Validator.ValidatePort(invalidPort1));
            Assert.IsFalse(Validator.ValidatePort(invalidPort2));
            Assert.IsFalse(Validator.ValidatePort(invalidPort3));
            Assert.IsFalse(Validator.ValidatePort(invalidPort4));
            Assert.IsFalse(Validator.ValidatePort(invalidPort5));

        }

        [TestMethod]
        public void TestIPAddress()
        {
            Assert.IsTrue(Validator.ValidateIPAddress(validIP1));
            Assert.IsTrue(Validator.ValidateIPAddress(validIP2));
            Assert.IsFalse(Validator.ValidateIPAddress(invalidIP1));
            Assert.IsFalse(Validator.ValidateIPAddress(invalidIP2));
        }

        [TestMethod]
        public void TestSpeed()
        {
            Assert.IsTrue(Validator.ValidateSpeed(validSpeed1));
            Assert.IsTrue(Validator.ValidateSpeed(validSpeed2));
            Assert.IsTrue(Validator.ValidateSpeed(validSpeed3));
            Assert.IsTrue(Validator.ValidateSpeed(validSpeed4));
            Assert.IsFalse(Validator.ValidateSpeed(invalidSpeed1));
            Assert.IsFalse(Validator.ValidateSpeed(invalidSpeed2));
            Assert.IsFalse(Validator.ValidateSpeed(invalidSpeed3));
            Assert.IsFalse(Validator.ValidateSpeed(invalidSpeed4));
        }

        [TestMethod]
        public void TestSpeedTextOnly()
        {
            Assert.IsTrue(Validator.ValidateSpeedTextOnly(validTextSpeed1));
            Assert.IsTrue(Validator.ValidateSpeedTextOnly(validTextSpeed2));
            Assert.IsFalse(Validator.ValidateSpeedTextOnly(invalidTextSpeed1));
            Assert.IsFalse(Validator.ValidateSpeedTextOnly(invalidTextSpeed1));


        }

        [TestMethod]
        public void TestIFGain()
        {
            Assert.IsTrue(Validator.ValidateIFGain(validIFGain1));
            Assert.IsTrue(Validator.ValidateIFGain(validIFGain2));
            Assert.IsTrue(Validator.ValidateIFGain(validIFGain3));
            Assert.IsTrue(Validator.ValidateIFGain(validIFGain4));
            Assert.IsFalse(Validator.ValidateIFGain(invalidIFGain1));
            Assert.IsFalse(Validator.ValidateIFGain(invalidIFGain2));
            Assert.IsFalse(Validator.ValidateIFGain(invalidIFGain3));
            Assert.IsFalse(Validator.ValidateIFGain(invalidIFGain4));


        }

        [TestMethod]
        public void TestFrequency()
        {
            Assert.IsTrue(Validator.ValidateFrequency(validFrequency1));
            Assert.IsTrue(Validator.ValidateFrequency(validFrequency2));
            Assert.IsTrue(Validator.ValidateFrequency(validFrequency3));
            Assert.IsTrue(Validator.ValidateFrequency(validFrequency4));
            Assert.IsFalse(Validator.ValidateFrequency(invalidFrequency1));
            Assert.IsFalse(Validator.ValidateFrequency(invalidFrequency2));
            Assert.IsFalse(Validator.ValidateFrequency(invalidFrequency3));
            Assert.IsFalse(Validator.ValidateFrequency(invalidFrequency4));
        }

        [TestMethod]
        public void TestOffsetVoltage()
        {
            Assert.IsTrue(Validator.ValidateOffsetVoltage(validVolts1));
            Assert.IsTrue(Validator.ValidateOffsetVoltage(validVolts2));
            Assert.IsTrue(Validator.ValidateOffsetVoltage(validVolts3));
            Assert.IsTrue(Validator.ValidateOffsetVoltage(validVolts4));
            Assert.IsFalse(Validator.ValidateOffsetVoltage(invalidVolts1));
            Assert.IsFalse(Validator.ValidateOffsetVoltage(invalidVolts2));
            Assert.IsFalse(Validator.ValidateOffsetVoltage(invalidVolts3));
            Assert.IsFalse(Validator.ValidateOffsetVoltage(invalidVolts4));



        }

        [TestMethod]
        public void TestIsDouble_IsDouble_Success()
        {
            Assert.IsTrue(Validator.IsDouble("1.5"));
            Assert.IsTrue(Validator.IsDouble("1"));
            Assert.IsTrue(Validator.IsDouble("0.0"));
            Assert.IsTrue(Validator.IsDouble("-1.2"));
        }

        [TestMethod]
        public void TestIsDouble_NotDouble_Fail()
        {
            Assert.IsFalse(Validator.IsDouble("hello"));
            Assert.IsFalse(Validator.IsDouble("."));
            Assert.IsFalse(Validator.IsDouble("32..."));
            Assert.IsFalse(Validator.IsDouble("25565;"));
        }

        [TestMethod]
        public void TestIsBetween_IsBetween_Success()
        {
            int upper = 5;
            int lower = -1;

            Assert.IsTrue(Validator.IsBetween(lower, lower, upper));
            Assert.IsTrue(Validator.IsBetween(upper, lower, upper));
            Assert.IsTrue(Validator.IsBetween(lower - 1, null, upper));
            Assert.IsTrue(Validator.IsBetween(upper + 1, lower, null));
        }

        [TestMethod]
        public void TestIsBetween_IsNotBetween_Fail()
        {
            int upper = 5;
            int lower = -1;

            Assert.IsFalse(Validator.IsBetween(lower - 1, lower, upper));
            Assert.IsFalse(Validator.IsBetween(upper + 1, lower, upper));
            Assert.IsFalse(Validator.IsBetween(lower - 1, lower, null));
            Assert.IsFalse(Validator.IsBetween(upper + 1, null, upper));
        }

        [TestMethod]
        public void TestServerRunningOnIp_ServerIsRunning_Success()
        {
            string ip = "127.0.0.1";
            int port = 3000;

            TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();

            Assert.IsTrue(Validator.ServerRunningOnIp(ip, port));

            server.Stop();
        }

        [TestMethod]
        public void TestServerRunningOnIp_ServerNotRunning_Fail()
        {
            string ip = "148.0.0.1";
            int port = 3000;

            Assert.IsFalse(Validator.ServerRunningOnIp(ip, port));
        }
    }
}

