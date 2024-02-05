using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class SpectraCyberConfigTest
    {
        // Class being tested
        private SpectraCyberConfig spectraCyberConfig_1;
        private SpectraCyberConfig spectraCyberConfig_2;
        private SpectraCyberConfig spectraCyberConfig_3;
        private SpectraCyberModeTypeEnum mode = SpectraCyberModeTypeEnum.CONTINUUM;
        private SpectraCyberIntegrationTimeEnum integration_time = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
        private double offset_voltage_1 = 0.4;
        private double offset_voltage_2 = 0;
        private double if_gain = 1;
        private SpectraCyberDCGainEnum dc_gain = SpectraCyberDCGainEnum.X5;
        private SpectraCyberBandwidthEnum bandwidth = SpectraCyberBandwidthEnum.SMALL_BANDWIDTH;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            spectraCyberConfig_1 = new SpectraCyberConfig(mode, integration_time, offset_voltage_1, if_gain, dc_gain, bandwidth);
            spectraCyberConfig_2 = new SpectraCyberConfig(mode, integration_time, offset_voltage_1, if_gain, dc_gain, bandwidth);
            spectraCyberConfig_3 = new SpectraCyberConfig(mode, integration_time, offset_voltage_2, if_gain, dc_gain, bandwidth);
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(mode, spectraCyberConfig_1._Mode);
            Assert.AreEqual(integration_time, spectraCyberConfig_1.IntegrationTime);
            Assert.AreEqual(offset_voltage_1, spectraCyberConfig_1.offset_voltage);
            Assert.AreEqual(if_gain, spectraCyberConfig_1.if_gain);
            Assert.AreEqual(dc_gain, spectraCyberConfig_1.DCGain);
            Assert.AreEqual(bandwidth, spectraCyberConfig_1.Bandwidth);
        }

        [TestMethod]
        public void TestEquals()
        {
            Assert.IsTrue(spectraCyberConfig_1.Equals(spectraCyberConfig_2));
        }

        [TestMethod]
        public void TestNotEquals()
        {
            Assert.IsFalse(spectraCyberConfig_1.Equals(spectraCyberConfig_3));
        }
    }
}
