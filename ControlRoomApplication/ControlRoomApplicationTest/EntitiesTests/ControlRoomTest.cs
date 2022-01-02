using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;
using System.Net.Sockets;
using System.Net;
using ControlRoomApplication.Controllers.SensorNetwork;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class ControlRoomTest
    {
        private static ControlRoom controlRoom;
        private AbstractWeatherStation weatherStation;
        private List<RadioTelescopeControllerManagementThread> rtManagementThreads;

        // PLC driver
        readonly string PlcIp = "127.0.0.1";
        readonly int PlcPort = 4000;
        readonly string McuIp = "127.0.0.1";
        readonly int McuPort = 4010;

        // Sensor Network
        readonly IPAddress SnServerIp = IPAddress.Parse("127.0.0.1");
        readonly int SnServerPort = 3000;
        readonly string SnClientIp = "127.0.0.1";
        readonly int SnClientPort = 3001;
        readonly int SnTelescopeId = 3000;

        [TestInitialize]
        public void BuildUp() {

            SensorNetworkServer SN = new SensorNetworkServer(SnServerIp, SnServerPort, SnClientIp, SnClientPort, SnTelescopeId, true);

            rtManagementThreads = new List<RadioTelescopeControllerManagementThread>()
            {
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberSimulatorController(new SpectraCyberSimulator()),  new  SimulationPLCDriver(PlcIp, McuIp, McuPort, PlcPort, true, false), new Location(), new Orientation() , 1, SN))),
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberSimulatorController(new SpectraCyberSimulator()),  new  SimulationPLCDriver(PlcIp, McuIp, McuPort+1, PlcPort+1, true, false), new Location(), new Orientation(), 2, SN))),
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberSimulatorController(new SpectraCyberSimulator()), new  SimulationPLCDriver(PlcIp, McuIp, McuPort+2, PlcPort+2, true, false), new Location(), new Orientation() , 3, SN))),
            };

            controlRoom = new ControlRoom( weatherStation, 87 );

            // End the CR's listener's server. We have to do this until we stop hard-coding that dang value.
            // TODO: Remove this logic when the value is no longer hard-coded (issue #350)
            //PrivateObject listener = new PrivateObject(controlRoom.mobileControlServer);
            //((TcpListener)listener.GetFieldOrProperty("server")).Stop();

            controlRoom.RTControllerManagementThreads.Add( rtManagementThreads[0] );
            controlRoom.RTControllerManagementThreads.Add( rtManagementThreads[1] );
            controlRoom.RTControllerManagementThreads.Add( rtManagementThreads[2] );
        }

        [ClassCleanup]
        public static void cleanUp()
        {
            foreach (RadioTelescopeControllerManagementThread RTCMT in controlRoom.RTControllerManagementThreads)
            {
                RTCMT.RequestToKill();
            }
            controlRoom.mobileControlServer.RequestToKillTCPMonitoringRoutine();
        }

        
        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(weatherStation, controlRoom.WeatherStation);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads != null);

            Assert.IsTrue(controlRoom.RTControllerManagementThreads != null);
            Assert.AreEqual(3, controlRoom.RTControllerManagementThreads.Count);

            Assert.IsTrue(controlRoom.RTControllerManagementThreads[0].RTController != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[1].RTController != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[2].RTController != null);

            Assert.IsTrue(controlRoom.RTControllerManagementThreads[0].RTController.RadioTelescope != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[1].RTController.RadioTelescope != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[2].RTController.RadioTelescope != null);
        }
    }
}
