using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class RadioTelescopeControllerManagementThreadTest
    {
        private string IP;
        private static int Port0;
        private int Port1;

        private Location JohnRudyPark;
        private Orientation CalibrationOrientation;

        private SimulationPLCDriver PLCCCH0;
        private SimulationPLCDriver PLCCCH1;

        public static RadioTelescopeController RTC0;
        public static RadioTelescopeController RTC1;

        private static RadioTelescopeControllerManagementThread RTCMT0;
        private static RadioTelescopeControllerManagementThread RTCMT1;

        [TestInitialize]
        public void BringUp() {
            IP = PLCConstants.LOCAL_HOST_IP;
            Port0 = 8112;
            Port1 = 8115;

            JohnRudyPark = MiscellaneousConstants.JOHN_RUDY_PARK;
            CalibrationOrientation = new Orientation( 0 , 90 );

            PLCCCH0 = new SimulationPLCDriver( IP , IP , Port0 , Port0 , true , false );
            RTC0 = new RadioTelescopeController( new RadioTelescope( new SpectraCyberSimulatorController( new SpectraCyberSimulator() ) , PLCCCH0 , JohnRudyPark , CalibrationOrientation , 1 ) );
            RTCMT0 = new RadioTelescopeControllerManagementThread( RTC0 );

            PLCCCH1 = new SimulationPLCDriver( IP , IP , Port1 , Port1 , true , false );
            RTC1 = new RadioTelescopeController( new RadioTelescope( new SpectraCyberSimulatorController( new SpectraCyberSimulator() ) , PLCCCH1 , JohnRudyPark , CalibrationOrientation , 2 ) );
            RTCMT1 = new RadioTelescopeControllerManagementThread( RTC1 );
        }


        [TestMethod]
        public void TestConstructorsAndProperties()
        {

            Assert.AreEqual(RTC0, RTCMT0.RTController);
            Assert.AreEqual(RTC1, RTCMT1.RTController);

            Assert.AreEqual(1, RTCMT0.RadioTelescopeID);
            Assert.AreEqual(2, RTCMT1.RadioTelescopeID);

            Assert.AreEqual(null, RTCMT0.NextObjectiveOrientation);
            Assert.AreEqual(null, RTCMT1.NextObjectiveOrientation);

            Assert.AreEqual(false, RTCMT0.Busy);
            Assert.AreEqual(false, RTCMT1.Busy);
        }

        [TestMethod]
        public void TestLifecycle()
        {
            Assert.AreEqual(true, RTCMT0.Start(RTCMT0.RTController));
            Assert.AreEqual(true, RTCMT1.Start(RTCMT1.RTController));

            Assert.AreEqual(false, RTCMT0.Busy);
            Assert.AreEqual(false, RTCMT1.Busy);

            RTCMT0.KillWithHardInterrupt();
            Assert.AreEqual(true, RTCMT0.WaitToJoin());

            RTCMT1.KillWithHardInterrupt();
            Assert.AreEqual(true, RTCMT1.WaitToJoin());

            Assert.AreEqual(false, RTCMT0.Busy);
            Assert.AreEqual(false, RTCMT1.Busy);
        }

        [TestCleanup]
        public void Bringdown() {
            RTC0.RadioTelescope.PLCDriver.Bring_down();
            RTC1.RadioTelescope.PLCDriver.Bring_down();
            RTCMT0.RequestToKill();
            RTCMT1.RequestToKill();
            PLCCCH0.Bring_down();
            PLCCCH1.Bring_down();
        }
    }
}
