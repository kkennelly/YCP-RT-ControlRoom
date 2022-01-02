using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System.Net.Sockets;
using System.Net;
using ControlRoomApplication.Controllers.SensorNetwork;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class ControlRoomControllerTest
    {
        public static ControlRoomController CRController;
        public static ControlRoom ControlRoom;
        public static Orientation CalibrationOrientation;
        public static string IP = PLCConstants.LOCAL_HOST_IP;
        public static int Port1 = 15001;
        public static int Port2 = 15003;
        public static RadioTelescopeController RTController0;
        public static RadioTelescopeController RTController1;
        public static RadioTelescopeController RTController2;


        public static IPAddress SnServerIp = IPAddress.Parse("127.0.0.1");
        public static int SnServerPort = 3000;
        public static string SnClientIp = "127.0.0.1";
        public static int SnClientPort = 3001;
        public static int SnTelescopeId = 3000;

        [ClassInitialize]
        public static void BringUp(TestContext context)
        {
            ControlRoom = new ControlRoom(new SimulationWeatherStation(100), 81);

            // End the CR's listener's server. We have to do this until we stop hard-coding that dang value.
            // TODO: Remove this logic when the value is no longer hard-coded (issue #350)
           // PrivateObject listener = new PrivateObject(ControlRoom.mobileControlServer);
            //((TcpListener)listener.GetFieldOrProperty("server")).Stop();

            CRController = new ControlRoomController(ControlRoom);
            CalibrationOrientation = new Orientation(0, 90);
        }



        [TestInitialize]
        public void ReinitializeRTs()
        {

            SensorNetworkServer server = new SensorNetworkServer(SnServerIp, SnServerPort, SnClientIp, SnClientPort, SnTelescopeId, true);


            RTController0 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                     new TestPLCDriver(IP, IP, Port1, Port2, true),
                    MiscellaneousConstants.JOHN_RUDY_PARK,
                    CalibrationOrientation,1,server
                )
            );

            RTController1 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new TestPLCDriver(IP, IP, Port1 + 3, Port2 + 3,true),
                    MiscellaneousConstants.JOHN_RUDY_PARK,
                    CalibrationOrientation,2,server
                )
            );

            RTController2 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new TestPLCDriver(IP, IP, Port1 + 6, Port2 + 6,true),
                    MiscellaneousConstants.JOHN_RUDY_PARK,
                    CalibrationOrientation,3,server
                )
            );

            

        }

        [TestCleanup]
        public void testClean() {
            try {
                RTController0.RadioTelescope.PLCDriver.Bring_down();
                RTController1.RadioTelescope.PLCDriver.Bring_down();
                RTController2.RadioTelescope.PLCDriver.Bring_down();
            } catch { }
        }

        [TestMethod]
        public void TestConstructorAndProperties()
        {
            Assert.IsTrue(ControlRoom == CRController.ControlRoom);
        }

        [TestMethod]
        public void TestAddRTControllerAndStartAndHardRemoveInstance()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController2));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController1, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController2, false));
        }

        [TestMethod]
        public void TestAddRTControllerAndSoftRemoveInstance()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController2));

            foreach (RadioTelescopeControllerManagementThread ManagementThread in CRController.ControlRoom.RTControllerManagementThreads)
            {
                ManagementThread.Start();
            }

            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController1, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController2, true));
        }

        [TestMethod]
        public void TestAddRTControllerAndHardRemoveIndex()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController2));

            foreach (RadioTelescopeControllerManagementThread ManagementThread in CRController.ControlRoom.RTControllerManagementThreads)
            {
                ManagementThread.Start();
            }


            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, false));
        }

        [TestMethod]
        public void TestAddRTControllerAndStartAndSoftRemoveIndex()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController2));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(2, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(1, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, true));
        }

        [TestMethod]
        public void TestPreventAddMultipleTimes()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(false, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
        }

        [TestMethod]
        public void TestPreventRemoveMultipleTimes()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
            Assert.AreEqual(false, CRController.RemoveRadioTelescopeController(RTController0, true));
        }

        [TestMethod]
        public void TestWeatherStationOverride()
        {
            CRController.ControlRoom.weatherStationOverride = false;
            Assert.IsFalse(CRController.ControlRoom.weatherStationOverride);
            CRController.ControlRoom.weatherStationOverride = true;
            Assert.IsTrue(CRController.ControlRoom.weatherStationOverride);
        }

        [ClassCleanup]
        public static void Bringdown( ) {
            RTController0.RadioTelescope.PLCDriver.Bring_down();
            RTController1.RadioTelescope.PLCDriver.Bring_down();
            RTController2.RadioTelescope.PLCDriver.Bring_down();
            CRController.ControlRoom.mobileControlServer.RequestToKillTCPMonitoringRoutine();

        }
    }
}
