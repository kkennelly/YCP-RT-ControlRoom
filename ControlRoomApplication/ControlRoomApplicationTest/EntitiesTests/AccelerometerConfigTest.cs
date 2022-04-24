using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AccelerometerConfigTest
    {
        [TestMethod]
        public void TestInitialization()
        {
            int sensorNetworkConfigId = -1;

            // Create new SensorNetworkConfig with a telescope ID of 5
            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);

            // SensorNetworkConfig ID and Location ID
            Assert.AreEqual(config.SensorNetworkConfigId, sensorNetworkConfigId);
            Assert.AreEqual(config.LocationId, -1);

            // Default initialization
            Assert.AreEqual(config.SamplingFrequency, 800);
            Assert.AreEqual(config.GRange, 16);
            Assert.AreEqual(config.FIFOSize, 32);
            Assert.AreEqual(config.XOffset, 0);
            Assert.AreEqual(config.YOffset, 0);
            Assert.AreEqual(config.ZOffset, 0);
            Assert.AreEqual(config.FullBitResolution, true);
        }

        [TestMethod]
        public void TestEmptyInitialization()
        {
            AccelerometerConfig config = new AccelerometerConfig();

            // All values should be an equivalent of 0
            Assert.AreEqual(config.SensorNetworkConfigId, 0);
            Assert.AreEqual(config.LocationId, 0);
            
            Assert.AreEqual(config.SamplingFrequency, 0);
            Assert.AreEqual(config.GRange, 0);
            Assert.AreEqual(config.FIFOSize, 0);
            Assert.AreEqual(config.XOffset, 0);
            Assert.AreEqual(config.YOffset, 0);
            Assert.AreEqual(config.ZOffset, 0);
            Assert.AreEqual(config.FullBitResolution, false);
        }

        [TestMethod]
        public void TestEquals_Identical_Equal()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);

            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            Assert.IsTrue(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_SamplingFrequencyDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.SamplingFrequency = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_GRangeDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.GRange = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_FIFOSizeDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.FIFOSize = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_XOffsetDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.XOffset = 1;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_YOffsetInitDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.YOffset = 1;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_ZOffsetDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.ZOffset = 1;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_FullBitResolutionDifferent_NotEqual()
        {
            int sensorNetworkConfigId = -1;

            AccelerometerConfig config = new AccelerometerConfig(sensorNetworkConfigId, -1);
            AccelerometerConfig other = new AccelerometerConfig(sensorNetworkConfigId, -1);

            other.FullBitResolution = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestGetAccelConfigAsBytes()
        {
            AccelerometerConfig config = new AccelerometerConfig(-1, -1);

            var bytes = config.GetAccelConfigAsBytes();

            Assert.AreEqual(0xD, bytes[0]);
            Assert.AreEqual(0x3, bytes[1]);
            Assert.AreEqual(31, bytes[2]);
            Assert.AreEqual(0, bytes[3]);
            Assert.AreEqual(0, bytes[4]);
            Assert.AreEqual(0, bytes[5]);
            Assert.IsTrue(Convert.ToBoolean(bytes[6]));
        }
    }
}
