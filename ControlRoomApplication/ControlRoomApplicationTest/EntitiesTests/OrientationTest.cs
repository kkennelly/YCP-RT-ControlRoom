using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class OrientationTest
    {
        private Orientation orientation_1;
        private Orientation orientation_2;

        private double azimuth_1;
        private double elevation_1;
        private double azimuth_2;
        private double elevation_2;

        [TestInitialize]
        public void BuildUp()
        {
            orientation_1 = new Orientation();
            orientation_2 = new Orientation();

            azimuth_1 = 0;
            elevation_1 = 0;
            azimuth_2 = 0;
            elevation_2 = 0;
        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual(azimuth_1, orientation_1.Azimuth);
            Assert.AreEqual(elevation_1, orientation_1.Elevation);
            Assert.AreEqual(azimuth_2, orientation_2.Azimuth);
            Assert.AreEqual(elevation_2, orientation_2.Elevation);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            azimuth_1 = 10;
            elevation_1 = 25;

            orientation_1.Azimuth = azimuth_1;
            orientation_1.Elevation = elevation_1;

            Assert.AreEqual(azimuth_1, orientation_1.Azimuth);
            Assert.AreEqual(elevation_1, orientation_1.Elevation);
        }

        [TestMethod]
        public void TestEquals()
        {
            azimuth_1 = 10.001;
            elevation_1 = 25.15752547;
            azimuth_2 = 10;
            elevation_2 = 25.15987524;

            orientation_1.Azimuth = azimuth_1;
            orientation_1.Elevation = elevation_1;
            orientation_2.Azimuth = azimuth_2;
            orientation_2.Elevation = elevation_2;

            Assert.IsTrue(orientation_1.Equals(orientation_2));
        }

        [TestMethod]
        public void TestNotEquals()
        {
            azimuth_1 = 10.1;
            elevation_1 = 25.15752547;
            azimuth_2 = 10;
            elevation_2 = 25.15987524;

            orientation_1.Azimuth = azimuth_1;
            orientation_1.Elevation = elevation_1;
            orientation_2.Azimuth = azimuth_2;
            orientation_2.Elevation = elevation_2;

            Assert.IsFalse(orientation_1.Equals(orientation_2));
        }

        [TestMethod]
        public void TestOrientationValid()
        {
            const int SAFE_VAL = 50;

            // Valid (low edge azimuth)
            Orientation orientation;

            // Valid (low edge elevation)
            orientation = new Orientation(SAFE_VAL, SimulationConstants.LIMIT_LOW_EL_DEGREES);
            Assert.IsTrue(orientation.orientationValid());

            // Valid (high edge elevation)
            orientation = new Orientation(SAFE_VAL, SimulationConstants.LIMIT_HIGH_EL_DEGREES);
            Assert.IsTrue(orientation.orientationValid());

            // Invalid (low elevation)
            orientation = new Orientation(SAFE_VAL, SimulationConstants.LIMIT_LOW_EL_DEGREES - 1);
            Assert.IsFalse(orientation.orientationValid());

            // Invalid (high elevation)
            orientation = new Orientation(SAFE_VAL, SimulationConstants.LIMIT_HIGH_EL_DEGREES + 1);
            Assert.IsFalse(orientation.orientationValid());
        }

        [TestMethod]
        public void TestCloneOrientation()
        {
            // Create initial orientation
            Orientation initial = new Orientation(123, 456);

            // Clone initial orientation
            Orientation cloned = (Orientation)initial.Clone();

            // Verify cloned orientation's azimuth and elevation are the same
            Assert.AreEqual(initial.Azimuth, cloned.Azimuth);
            Assert.AreEqual(initial.Elevation, cloned.Elevation);
        }
    }
}