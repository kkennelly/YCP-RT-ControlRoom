using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class CelestialBodyTest
    {
        // Class being tested
        private CelestialBody celestial_body_1;
        private CelestialBody celestial_body_2;
        private CelestialBody celestial_body_3;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            celestial_body_1 = new CelestialBody(CelestialBodyConstants.SUN);
            celestial_body_2 = new CelestialBody(CelestialBodyConstants.MOON);
            celestial_body_3 = new CelestialBody(CelestialBodyConstants.NONE, new Coordinate(0,0));
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(CelestialBodyConstants.SUN, celestial_body_1.Name);
            Assert.AreEqual(CelestialBodyConstants.MOON, celestial_body_2.Name);
            Assert.AreEqual(CelestialBodyConstants.NONE, celestial_body_3.Name);
            Assert.AreEqual(0, celestial_body_3.Coordinate.right_ascension);
            Assert.AreEqual(0, celestial_body_3.Coordinate.declination);
        }
    }
}
