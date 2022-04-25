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
    public class SensorNetworkConfigTest
    {
        [TestMethod]
        public void TestInitialization()
        {
            int telescopeId = 5;

            // Create new SensorNetworkConfig with a telescope ID of 5
            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);

            // telescope ID 
            Assert.AreEqual(config.TelescopeId, telescopeId);

            // default constants
            Assert.AreEqual(config.TimeoutDataRetrieval, SensorNetworkConstants.DefaultDataRetrievalTimeout);
            Assert.AreEqual(config.TimeoutInitialization, SensorNetworkConstants.DefaultInitializationTimeout);

            // default initialization (all must default to true)
            Assert.AreEqual(config.ElevationTemp1Init, true);
            Assert.AreEqual(config.AzimuthTemp1Init, true);
            Assert.AreEqual(config.ElevationAccelerometerInit, true);
            Assert.AreEqual(config.AzimuthAccelerometerInit, true);
            Assert.AreEqual(config.CounterbalanceAccelerometerInit, true);
            Assert.AreEqual(config.ElevationEncoderInit, true);
            Assert.AreEqual(config.AzimuthEncoderInit, true);
            Assert.AreEqual(config.ElevationAmbientInit, true);
        }

        [TestMethod]
        public void TestEmptyInitialization()
        {
            SensorNetworkConfig config = new SensorNetworkConfig();

            // All values should be an equivalent of 0

            Assert.AreEqual(config.TelescopeId, 0);
            
            Assert.AreEqual(config.TimeoutDataRetrieval, 0);
            Assert.AreEqual(config.TimeoutInitialization, 0);
            
            Assert.AreEqual(config.ElevationTemp1Init, false);
            Assert.AreEqual(config.AzimuthTemp1Init, false);
            Assert.AreEqual(config.ElevationAccelerometerInit, false);
            Assert.AreEqual(config.AzimuthAccelerometerInit, false);
            Assert.AreEqual(config.CounterbalanceAccelerometerInit, false);
            Assert.AreEqual(config.ElevationEncoderInit, false);
            Assert.AreEqual(config.AzimuthEncoderInit, false);
            Assert.AreEqual(config.ElevationAmbientInit, false);
        }

        [TestMethod]
        public void TestEquals_Identical_Equal()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);

            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            Assert.IsTrue(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_TelescopeIdDifferent_NotEqual()
        {
            SensorNetworkConfig config = new SensorNetworkConfig(5);
            SensorNetworkConfig other = new SensorNetworkConfig(6);

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_ElevationTemp1InitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.ElevationTemp1Init = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_AzimuthTemp1InitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.AzimuthTemp1Init = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_AzimuthAccelerometerInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.AzimuthAccelerometerInit = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_ElevationAccelerometerInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.ElevationAccelerometerInit = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_CounterbalanceAccelerometerInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.CounterbalanceAccelerometerInit = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_AzimuthEncoderInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.AzimuthEncoderInit = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_ElevationEncoderInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.ElevationEncoderInit = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_AmbTempHumidityInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.ElevationAmbientInit = false;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_TimerPeriodInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.TimerPeriod = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_EthernetPeriodInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.EthernetPeriod = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_ElAccelConfigInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.ElAccelConfig = new AccelerometerConfig(other.Id, (int)SensorLocationEnum.EL_MOTOR);

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_AzAccelConfigInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.AzAccelConfig = new AccelerometerConfig(other.Id, (int)SensorLocationEnum.AZ_MOTOR);

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_CbAccelConfigInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.CbAccelConfig = new AccelerometerConfig(other.Id, (int)SensorLocationEnum.COUNTERBALANCE);

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_TemperaturePeriodInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.TemperaturePeriod = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_EncoderPeriodInitDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.EncoderPeriod = 0;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_TimeoutDataRetrievalDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.TimeoutDataRetrieval = 5;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestEquals_TimeoutInitializationDifferent_NotEqual()
        {
            int telescopeId = 5;

            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);
            SensorNetworkConfig other = new SensorNetworkConfig(telescopeId);

            other.TimeoutInitialization = 5;

            Assert.IsFalse(config.Equals(other));
        }

        [TestMethod]
        public void TestGetSensorInitAsBytes_AllTrue_AllBytesOne()
        {
            SensorNetworkConfig config = new SensorNetworkConfig(5);

            config.TimerPeriod = 1;
            config.EthernetPeriod = 1;
            config.TemperaturePeriod = 1;
            config.EncoderPeriod = 1;

            var bytes = config.GetSensorInitAsBytes();

            // All sensor init bytes in the array should be 1
            for (int i = 0; i < 8; i++)
            {
                Assert.IsTrue(bytes[i] == 1);
            }

            int parsedTimerPeriod = BitConverter.ToInt32(bytes, 8);
            Assert.AreEqual(1, parsedTimerPeriod);

            int parsedEthernetPeriod = BitConverter.ToInt32(bytes, 12);
            Assert.AreEqual(1, parsedEthernetPeriod);

            int parsedTemperaturePeriod = BitConverter.ToInt32(bytes, 16);
            Assert.AreEqual(1, parsedTemperaturePeriod);

            int parsedEncoderPeriod = BitConverter.ToInt32(bytes, 20);
            Assert.AreEqual(1, parsedEncoderPeriod);
        }

        [TestMethod]
        public void TestGetSensorInitAsBytes_AllFalse_AllBytesZero()
        {
            SensorNetworkConfig config = new SensorNetworkConfig(5);

            config.ElevationTemp1Init = false;
            config.AzimuthTemp1Init = false;
            config.ElevationAccelerometerInit = false;
            config.AzimuthAccelerometerInit = false;
            config.CounterbalanceAccelerometerInit = false;
            config.ElevationEncoderInit = false;
            config.AzimuthEncoderInit = false;
            config.ElevationAmbientInit = false;

            config.TimerPeriod = 0;
            config.EthernetPeriod = 0;
            config.TemperaturePeriod = 0;
            config.EncoderPeriod = 0;

            var bytes = config.GetSensorInitAsBytes();

            // All sensor init bytes in the array should be 0
            for (int i = 0; i < 8; i++)
            {
                Assert.IsTrue(bytes[i] == 0);
            }

            int parsedTimerPeriod = BitConverter.ToInt32(bytes, 8);
            Assert.AreEqual(0, parsedTimerPeriod);

            int parsedEthernetPeriod = BitConverter.ToInt32(bytes, 12);
            Assert.AreEqual(0, parsedEthernetPeriod);

            int parsedTemperaturePeriod = BitConverter.ToInt32(bytes, 16);
            Assert.AreEqual(0, parsedTemperaturePeriod);

            int parsedEncoderPeriod = BitConverter.ToInt32(bytes, 20);
            Assert.AreEqual(0, parsedEncoderPeriod);
        }
    }
}
