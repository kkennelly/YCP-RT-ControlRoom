using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.DiagnosticData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class HumidityTest
    {
        // Class being tested
        private Humidity Humidity;

        [TestInitialize]
        public void BuildUp()
        {

        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            // Initialize appointment entity
            SensorLocationEnum loc1 = SensorLocationEnum.EL_FRAME;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long dateTime2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Temperature
            Humidity h1 = Humidity.Generate(dateTime1, 0.0, loc1);

            h1.TimeCapturedUTC = dateTime2;
            Assert.AreEqual(h1.TimeCapturedUTC, dateTime2);
            h1.HumidityReading = 100.0;
            Assert.AreEqual(h1.HumidityReading, 100.0);
            h1.LocationID =1;
            Assert.AreEqual(h1.LocationID, 1);

        }

        [TestMethod]
        public void TestEquals()
        {
            // Initialize appointment entity
            SensorLocationEnum loc1 = SensorLocationEnum.EL_FRAME;

            //Generate current time
            long dateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Temperature
            Humidity h1 = Humidity.Generate(dateTime, 0.0, loc1);
            Humidity h2 = Humidity.Generate(dateTime, 2.0, loc1);


            Assert.AreEqual(h1, h1);
            Assert.AreNotEqual(h1, h2);
            Assert.AreNotEqual(h2, h1);
        }
    }
}
    