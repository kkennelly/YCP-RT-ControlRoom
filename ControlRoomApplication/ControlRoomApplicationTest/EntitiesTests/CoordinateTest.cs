using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class CoordinateTest
    {
        private double latitude;
        private double longitude;

        [TestInitialize]
        public void BuildUp()
        {
            latitude = 87.7;
            longitude = 70.5;
        }

        [TestMethod]
        public void TestCoordinate()
        {
            Coordinate coordinate = new Coordinate(87.7, 70.5);
            Assert.AreEqual(longitude, coordinate.declination);
            Assert.AreEqual(latitude, coordinate.right_ascension);
            //Set setters outside of constructor
            coordinate.right_ascension = 70.5;
            coordinate.declination = 87.7;
            Assert.AreEqual(latitude, coordinate.declination);
            Assert.AreEqual(longitude, coordinate.right_ascension);
        }
    }
}
