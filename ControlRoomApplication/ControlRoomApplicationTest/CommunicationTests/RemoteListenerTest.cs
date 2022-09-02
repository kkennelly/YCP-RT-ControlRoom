using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Controllers.Communications.Enumerations;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using ControlRoomApplication.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class RemoteListenerTest
    {
        // This will contain the RemoteListener object we are using for the various tests
        PrivateObject PrivListener;

        ControlRoom ControlRoom;

        RadioTelescopeController RtController;
        RadioTelescope RadioTelescope;

        // PLC driver
        readonly string PlcIp = "127.0.0.1";
        readonly int PlcPort = 4000;
        readonly string McuIp = "127.0.0.1";
        readonly int McuPort = 4001;

        // Sensor Network
        readonly IPAddress SnServerIp = IPAddress.Parse("127.0.0.1");
        readonly int SnServerPort = 3000;
        readonly string SnClientIp = "127.0.0.1";
        readonly int SnClientPort = 3001;
        readonly int SnTelescopeId = 3000;

        [TestInitialize]
        public void Initialize()
        {
            RadioTelescope = new RadioTelescope();

            AbstractPLCDriver PLC = new SimulationPLCDriver(PlcIp, McuIp, McuPort, PlcPort, true, false);
            RadioTelescope.PLCDriver = PLC;

            SensorNetworkServer SN = new SensorNetworkServer(SnServerIp, SnServerPort, SnClientIp, SnClientPort, SnTelescopeId, true);
            RadioTelescope.SensorNetworkServer = SN;

            RtController = new RadioTelescopeController(RadioTelescope);

            AbstractWeatherStation WS = new SimulationWeatherStation(1000);

            RadioTelescope.WeatherStation = WS;

            ControlRoom = new ControlRoom(WS, 80);

            ControlRoom.mobileControlServer.rtController = RtController;

            PrivListener = new PrivateObject(ControlRoom.mobileControlServer);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Until a proper bring-down method is created for the Remote Listener, we must stop the server like this after
            // every test.
            ((TcpListener)PrivListener.GetFieldOrProperty("server")).Stop();

            DatabaseOperations.DeleteSensorNetworkConfig(RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig);
        }

        [TestMethod]
        public void TestProcessMessage_SensorInit_RebootsSensorNetworkWithNewInit()
        {
            // Create and start SensorNetwork
            RadioTelescope.SensorNetworkServer.StartSensorMonitoringRoutine();

            // Build command to disable all sensors and set the timeouts to specific values
            byte expectedDisabled = 0;
            int expectedDataTimeout = 6;
            int expectedInitTimeout = 5;
            string command = "1.0 | SENSOR_INIT | " +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDataTimeout}," +
                $"{expectedInitTimeout}" + "| 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            byte[] resultInitBytes = RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig.GetSensorInitAsBytes();
            int resultDataTimeout = RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig.TimeoutDataRetrieval / 1000; // ms to seconds
            int resultInitTimeout = RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig.TimeoutInitialization / 1000; // ms to seconds

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);


            // Verify the init values are as expected
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationTemp]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.AzimuthTemp]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationEncoder]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.AzimuthEncoder]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.AzimuthAccelerometer]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationEncoder]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.CounterbalanceAccelerometer]);

            // Verify init values are as expected
            Assert.AreEqual(expectedDataTimeout, resultDataTimeout);
            Assert.AreEqual(expectedInitTimeout, resultInitTimeout);

            // Bring down the server and delete config
            RadioTelescope.SensorNetworkServer.EndSensorMonitoringRoutine();
        }

        [TestMethod]
        public void TestProcessMessage_SensorInitInvalidMessage_Fail()
        {
            string invalidCommand = "1.0 | SENSOR_INIT | 0,1,2,3,4 | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", invalidCommand);

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.MissingCommandArgs);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetCounterbalanceAcc_SetsCounterbalanceOverride()
        {
            string command = "1.0 | SET_OVERRIDE | CB_ACC | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideCounterbalanceAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemCounterbalanceAcc_RemovesCounterbalanceOverride()
        {
            string command = "1.0 | SET_OVERRIDE | CB_ACC | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideCounterbalanceAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElevationAcc_SetsElevationAccOverride()
        {
            string command = "1.0 | SET_OVERRIDE | EL_ACC | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });


            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideElevationAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElevationAcc_RemovesElevationAccOverride()
        {
            string command = "1.0 | SET_OVERRIDE | EL_ACC | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });


            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideElevationAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetAzimuthAcc_SetsAzimuthAccOverride()
        {
            string command = "1.0 | SET_OVERRIDE | AZ_ACC | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });


            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideAzimuthAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemAzimuthAcc_RemovesAzimuthAccOverride()
        {
            string command = "1.0 | SET_OVERRIDE | AZ_ACC | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });


            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideAzimuthAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElevationEnc_SetsElevationEncOverride()
        {
            string command = "1.0 | SET_OVERRIDE | EL_ABS_ENC | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });


            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideElevationAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElevationEnc_RemovesElevationEncOverride()
        {
            string command = "1.0 | SET_OVERRIDE | EL_ABS_ENC | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });


            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideElevationAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetAzimuthEnc_SetsAzimuthEncOverride()
        {
            string command = "1.0 | SET_OVERRIDE | AZ_ABS_ENC | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideAzimuthAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemAzimuthEnc_RemovesAzimuthEncOverride()
        {
            string command = "1.0 | SET_OVERRIDE | AZ_ABS_ENC | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideAzimuthAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElProx90_SetsElProx90Override()
        {
            string command = "1.0 | SET_OVERRIDE | ELEVATION_LIMIT_90 | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideElevatProx90);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElProx90_RemovesElProx90Override()
        {
            string command = "1.0 | SET_OVERRIDE | ELEVATION_LIMIT_90 | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideElevatProx90);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElProx0_SetsElProx0Override()
        {
            string command = "1.0 | SET_OVERRIDE | ELEVATION_LIMIT_0 | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideElevatProx0);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetAzMotTemp_SetsAzMotTempOverride()
        {
            string command = "1.0 | SET_OVERRIDE | AZIMUTH_MOT_TEMP | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideAzimuthMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemAzMotTemp_RemovesAzMotTempOverride()
        {
            string command = "1.0 | SET_OVERRIDE | AZIMUTH_MOT_TEMP | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideAzimuthMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElMotTemp_SetsElMotTempOverride()
        {
            string command = "1.0 | SET_OVERRIDE | ELEVATION_MOT_TEMP | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideElevatMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElMotTemp_RemovesElMotTempOverride()
        {
            string command = "1.0 | SET_OVERRIDE | ELEVATION_MOT_TEMP | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideElevatMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetGate_SetsGateOverride()
        {
            string command = "1.0 | SET_OVERRIDE | MAIN_GATE | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(RtController.overrides.overrideGate);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemGate_RemovesGateOverride()
        {
            string command = "1.0 | SET_OVERRIDE | MAIN_GATE | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(RtController.overrides.overrideGate);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetWeather_SetsWeatherOverride()
        {
            string command = "1.0 | SET_OVERRIDE | WEATHER_STATION | TRUE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsTrue(ControlRoom.weatherStationOverride);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemWeather_RemovesWeatherOverride()
        {
            string command = "1.0 | SET_OVERRIDE | WEATHER_STATION | FALSE | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.Success);
            Assert.IsFalse(ControlRoom.weatherStationOverride);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideInvalidOverride_Fail()
        {
            string command = "SET_OVERRIDE CACTUS";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.MissingCommandArgs);
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_SensorOverrideCommand_MissingTimestamp()
        {
            string command = "1.0 | SET_OVERRIDE | MAIN_GATE | TRUE  ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_Sensor_Override_InvalidSensor()
        {
            string command = "1.0 | SET_OVERRIDE | I_AINT_NO_SENSOR | TRUE | 12:00:00 ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.INVALID_SENSOR_OVERRIDE));
        }

        [TestMethod]
        public void TestProcessMessage_Sensor_Override_InvalidOverride()
        {
            string command = "1.0 | SET_OVERRIDE | I_AINT_NO_SENSOR | NOT_T_OR_F| 12:00:00 ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_SET_OVERRIDE_ARG));
        }

        [TestMethod]
        public void TestProcessMessage_Sensor_Override_MissingOverride()
        {
            string command = "1.0 | SET_OVERRIDE | I_AINT_NO_SENSOR | | 12:00:00 ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_SET_OVERRIDE_ARG));
        }

        [TestMethod]
        public void TestProcessMessage_Sensor_Override_MissingSensor()
        {
            string command = "1.0 | SET_OVERRIDE |  | TRUE | 12:00:00 ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.INVALID_SENSOR_OVERRIDE));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidVersion_DoesNotExist()
        {
            string command = "12.0 | STOP_RT | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidVersion, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.VERSION_NOT_FOUND));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidVersion_InvalidConversion()
        {
            string command = "yoooooo | STOP_RT | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidVersion, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.VERSION_CONVERSION_ERR));
        }


        [TestMethod]
        public void TestProcessMessage_TestMissingAzElArgs()
        {
            string command = "1.0 | ORIENTATION_MOVE | AZ | EL | 12:00:00";
            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.MissingCommandArgs);
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidAzElArgs_ElOutOfRange()
        {
            string command = "1.0 | ORIENTATION_MOVE | AZ 50 | EL 500 | 12:00:00";
            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.Success);
            Assert.AreEqual(mvmtResult.movementResult, MovementResult.ValidationError);
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidAzElArgs_NotNumbers()
        {
            string command = "1.0 | ORIENTATION_MOVE | AZ hi | EL sup | 12:00:00";
            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);

            Assert.AreEqual(result.parseTCPCommandResultEnum, ParseTCPCommandResultEnum.InvalidCommandArgs);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.AZ_EL_CONVERSION_ERR));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_StopCommand_MissingTimestamp()
        {
            string command = "1.0 | STOP_RT ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_ScriptCommand_MissingTimestamp()
        {
            string command = "1.0 | SCRIPT | DUMP ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_AbsMove_MissingTimestamp()
        {
            string command = "1.0 | ORIENTATION_MOVE | AZ 20 | EL 40";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_RelMove_MissingTimestamp()
        {
            string command = "1.0 | RELATIVE_MOVE | AZ 20 | EL 40";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_SensorInit_MissingTimestamp()
        {
            string command = "1.0 | SENSOR_INIT | 1,0,1,1,1,1,1,1,1 ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_Request_MissingTimestamp()
        {
            string command = "1.0 | REQUEST | MVMT_DATA ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.MISSING_COMMAND_ARGS));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_Request_InvalidRequest()
        {
            string command = "1.0 | REQUEST | NOT_A_REAL_REQUEST | 12:00:00 ";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidRequestType, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.INVALID_REQUEST_TYPE));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_MissingVersion()
        {
            string command = "REQUEST | MVMT_DATA | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidVersion, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.VERSION_CONVERSION_ERR));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidNumberArgs_MissingCommand()
        {
            string command = "1.0 | MVMT_DATA | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidCommandType, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.COMMAND_NOT_FOUND));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidCommandType()
        {
            string command = "1.0 | I_AINT_NO_COMMAND | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidCommandType, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.COMMAND_NOT_FOUND));
        }

        [TestMethod]
        public void TestProcessMessage_TestInvalidScriptType()
        {
            string command = "1.0 | SCRIPT | I_AINT_NO_SCRIPT | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidScript, result.parseTCPCommandResultEnum);
            Assert.IsTrue(result.errorMessage.Contains(TCPCommunicationConstants.SCRIPT_NOT_FOUND));
        }

        // Test happy paths
        [TestMethod]
        public void TestProcessMessage_TestValidOrientationMove()
        {
            string command = "1.0 | ORIENTATION_MOVE | AZ 10 | EL 30 | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestValidRelativeMove()
        {
            string command = "1.0 | RELATIVE_MOVE | AZ 1 | EL 1 | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestValidScript()
        {
            string command = "1.0 | SCRIPT | DUMP | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestValidRequest()
        {
            string command = "1.0 | REQUEST | MVMT_DATA | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestValidStopRT()
        {
            string command = "1.0 | STOP_RT | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
            Assert.AreEqual(MovementResult.Success, mvmtResult.movementResult);
        }
        
        [TestMethod]
        public void TestProcessMessage_TestEncryptedMessage()
        {
            string receivedCommand = "1.1|AA9uv3O7ov+eZ2xM9478QpgOxSBhBbyYMf21krHQMZLAdnaAqGwJ2GkZcT8hxE7T";

            Tuple<string, bool> dataPair = Utilities.CheckEncrypted(receivedCommand);

            string command = dataPair.Item1;

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestEncryptionUnsupportedVersion()
        {
            string receivedCommand = "1.0 | STOP_RT | 12:00:00";

            Tuple<string, bool> dataPair = Utilities.CheckEncrypted(receivedCommand);

            Assert.IsTrue(receivedCommand.Equals(dataPair.Item1));
            Assert.IsFalse(dataPair.Item2);
        }

        [TestMethod]
        public void TestProcessMessage_TestValidResetMCUErrorBit()
        {
            string command = "1.1 | RESET_MCU_BIT | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult resetResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
            Assert.AreEqual(MCUResetResult.Success, resetResult.resetResult);
        }
        
        [TestMethod]
        public void TestProcessMessage_TestResetMCUErrorBit_MissingTimestamp()
        {
            string command = "1.1 | RESET_MCU_BIT";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult resetResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
        }
        
        [TestMethod]
        public void TestProcessMessage_TestResetMCUErrorBit_UnsupportedVersion()
        {
            string command = "1.0 | RESET_MCU_BIT | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult resetResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidVersion, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestRequestMVMT_DATA()
        {
            string command = "1.1 | REQUEST | MVMT_DATA | 12:00:00";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult resetResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_TestNewTCPVersion()
        {
            string command = "1.1 | STOP_RT | 12:00:00";    // Run a command from an older version of TCP with a new version 

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", command);
            ExecuteTCPCommandResult mvmtResult = (ExecuteTCPCommandResult)PrivListener.Invoke("ExecuteRLCommand", new object[] { result.parsedString });
            Assert.AreEqual(ParseTCPCommandResultEnum.Success, result.parseTCPCommandResultEnum);
            Assert.AreEqual(MovementResult.Success, mvmtResult.movementResult);
        }

        [TestMethod]
        public void TestProcessMessage_NonCommandSent()
        {
            string definitelyACommandAndNotAMaliciousAttemptToBreachDataOrSomethingSoDontThinkThat = "lmao give me ip";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", definitelyACommandAndNotAMaliciousAttemptToBreachDataOrSomethingSoDontThinkThat);
            Assert.AreEqual(ParseTCPCommandResultEnum.MissingCommandArgs, result.parseTCPCommandResultEnum);
        }

        [TestMethod]
        public void TestProcessMessage_NonCommandSentWithPipes()
        {
            string definitelyACommandAndNotAMaliciousAttemptToBreachDataOrSomethingSoDontThinkThat = "lmao|give|me|ip";

            ParseTCPCommandResult result = (ParseTCPCommandResult)PrivListener.Invoke("ParseRLString", definitelyACommandAndNotAMaliciousAttemptToBreachDataOrSomethingSoDontThinkThat);
            Assert.AreEqual(ParseTCPCommandResultEnum.InvalidVersion, result.parseTCPCommandResultEnum);
        }
    }
}
