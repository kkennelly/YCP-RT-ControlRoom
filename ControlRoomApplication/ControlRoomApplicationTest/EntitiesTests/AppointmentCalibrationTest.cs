using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AppointmentCalibrationTest
    {
        Timer timer;
        bool exit = false;

        [TestMethod]
        public void TimerTest()
        {
            timer = new Timer(5000);
            timer.Elapsed += new ElapsedEventHandler(TimerEvent);

            timer.Start();

            while (!exit)
            {
                // Do stuff 
            }

            Assert.IsTrue(exit);
        }

        public void TimerEvent(Object o, ElapsedEventArgs e)
        {
            exit = true;
        }
    }
}
