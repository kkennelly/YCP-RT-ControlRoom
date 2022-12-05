using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities.Encoder;
using ControlRoomApplication.Entities;
using System.Linq;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class EncoderAveragesTest
    {
        // Fields
        private EncoderAverages balls;
        private const int capacity = 500;
        private Orientation absolute;
        private Orientation cabsolute;
        private Orientation fabsolute;

        [TestInitialize]
        public void BuildUp()
        {
            // Set up
            balls = new EncoderAverages();
            absolute = new Orientation { Azimuth = 50, Elevation = 50 };
            cabsolute = new Orientation { Azimuth = 50.001, Elevation = 50.001 };
            fabsolute = new Orientation { Azimuth = 72, Elevation = 72 };
        }

        [TestMethod]
        public void WillAddOrientationsWhileEmpty()
        {
            for (int i = 0; i < capacity; i++)
            {
                Assert.IsTrue(balls.AddOrientation(absolute, absolute));
            }
        }

        [TestMethod]
        public void WillUpdateQueuesWhenFull()
        {
            Assert.IsTrue(balls.AddOrientation(cabsolute, cabsolute));

            Assert.IsNotNull(balls.AbsoluteEncoder.First(x => x.Azimuth == cabsolute.Azimuth));
        }

        [TestMethod]
        public void TestCompareOrientationValidOrientation()
        {
            balls = new EncoderAverages();
            for (int i = 0; i < capacity; i++)
            {
                balls.AddOrientation(absolute, absolute);
            }

            Assert.IsTrue(balls.AddOrientation(absolute, absolute));
        }

        [TestMethod]
        public void TestCompareOrientationInvalidOrientation()
        {
            balls = new EncoderAverages();
            for (int i = 0; i < capacity; i++)
            {
                balls.AddOrientation(absolute, absolute);
            }

            Assert.IsFalse(balls.AddOrientation(fabsolute, fabsolute));
            Assert.IsFalse(balls.AddOrientation(fabsolute, absolute));
            Assert.IsFalse(balls.AddOrientation(absolute, fabsolute));
        }
    }
}
