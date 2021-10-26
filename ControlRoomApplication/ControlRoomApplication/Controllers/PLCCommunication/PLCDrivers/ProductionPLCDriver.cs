using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ControlRoomApplication.Entities.Configuration;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Util;
using System.Collections.Generic;
using static ControlRoomApplication.Constants.MCUConstants;
using ControlRoomApplication.Database;
using System.Timers;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.Sensors;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpListener PLCTCPListener;
        private ModbusSlave PLC_Modbusserver;
        private long PLC_last_contact = 0;
        private bool keep_modbus_server_alive = true;
        private bool is_test = false;
        private MCUManager MCU;
        private RadioTelescopeTypeEnum telescopeType = RadioTelescopeTypeEnum.NONE;
        /// <summary>
        /// set this ONLY if using test driver, removes timouts and delays
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool set_is_test(bool val) { is_test = val; return is_test; }
        /// <summary>
        /// starts a modbus server to comunicate with the PLC on PLC_port and local_ip
        /// then sets up a modbus client to comunicate with the MCU located at MCU_ip, MCU_port (192.168.0.50 , 502) for actual hardware
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public ProductionPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port) : base(local_ip, MCU_ip, MCU_port, PLC_port)
        {
            limitSwitchData = new Simulators.Hardware.LimitSwitchData();
            homeSensorData = new Simulators.Hardware.HomeSensorData();
            pLCEvents = new PLCEvents();
            MCU = new MCUManager( MCU_ip , MCU_port  );

            try {
                PLCTCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse(local_ip), PLC_port));
                ClientManagmentThread = new Thread( new ThreadStart( HandleClientManagementThread ) ) { Name = "PLC server Thread"};
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString());
                    return;
                }
                else { throw e; }// Unexpected exception
            }
            try
            {
                PLCTCPListener.Start(1);
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [AbstractPLCDriver] ERROR: failure starting PLC TCP server: " + e.ToString());
                    return;
                }
            }
        }



        /// <summary>
        /// runs the modbus server to interface with the plc
        /// </summary>
        public override void HandleClientManagementThread() {
            byte slaveId = 1;
            // create and start the TCP slave
            PLC_Modbusserver = ModbusTcpSlave.CreateTcp(slaveId, PLCTCPListener);
            //coils, inputs, holdingRegisters, inputRegisters
            PLC_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 256, 0);
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            PLC_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            PLC_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Server_Written_to_handler);

            PLC_Modbusserver.Listen();

            //PLC_Modbusserver.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            Task.Delay(2000).Wait();

            PLCEvents.setDefaultLimitHandler( DefaultLimitSwitchHandle );

            while (keep_modbus_server_alive) {
                Thread.Sleep(1000);
                PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.CTRL_HEART_BEAT]++;
            }
        }

        private async void DefaultLimitSwitchHandle( object sender , limitEventArgs e ) {
            if(e.Value) {
                switch(e.ChangedValue) {
                    case PLC_modbus_server_register_mapping.AZ_0_LIMIT :
                        {
                            if (!Overrides.overrideAzimuthProx0)
                            {
                                CurrentMovementPriority = MovementPriority.Critical;
                                MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH, RadioTelescopeDirectionEnum.ClockwiseOrNegative, 0.25);
                            }
                            break;
                        }
                    case PLC_modbus_server_register_mapping.AZ_375_LIMIT:
                        {
                            if (!Overrides.overrideAzimuthProx375)
                            {
                                CurrentMovementPriority = MovementPriority.Critical;
                                MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH, RadioTelescopeDirectionEnum.CounterclockwiseOrPositive, 0.25);
                            }
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_10_LIMIT:
                        {
                            if (!Overrides.overrideElevatProx0)
                            {
                                CurrentMovementPriority = MovementPriority.Critical;
                                MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION, RadioTelescopeDirectionEnum.CounterclockwiseOrPositive, 0.25);
                            }
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_90_LIMIT:
                        {
                            if (!Overrides.overrideElevatProx90)
                            {
                                CurrentMovementPriority = MovementPriority.Critical;
                                MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION, RadioTelescopeDirectionEnum.ClockwiseOrNegative, 0.25);
                            }
                            break;
                        }
                }
                
            }
            if(!e.Value) {
                switch(e.ChangedValue) {
                    case PLC_modbus_server_register_mapping.AZ_0_LIMIT:
                        {
                            if (!Overrides.overrideAzimuthProx0)
                            {
                                MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH);
                                CurrentMovementPriority = MovementPriority.None;
                            }
                            break;
                        }
                    case PLC_modbus_server_register_mapping.AZ_375_LIMIT:
                        {
                            if (!Overrides.overrideAzimuthProx375)
                            {
                                MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH);
                                CurrentMovementPriority = MovementPriority.None;
                            }
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_10_LIMIT:
                        {
                            if (!Overrides.overrideElevatProx0)
                            {
                                MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION);
                                CurrentMovementPriority = MovementPriority.None;
                            }
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_90_LIMIT:
                        {
                            if (!Overrides.overrideElevatProx90)
                            {
                                MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION);
                                CurrentMovementPriority = MovementPriority.None;
                            }
                            break;
                        }
                }
            }
        }


        public override bool StartAsyncAcceptingClients() {
            MCU.StartAsyncAcceptingClients();
            keep_modbus_server_alive = true;
            try {
                ClientManagmentThread.Start();
            } catch (Exception e) {
                if ((e is ThreadStateException) || (e is OutOfMemoryException)) {
                    logger.Error(Utilities.GetTimeStamp() + ": failed to start prodi=uction plc and mcu threads err:____    {0}", e);
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        /// <summary>
        /// Waits for the PLC to contact the control room 
        /// </summary>
        /// <remarks>
        /// the PLC is constantly trying to send data to the controll room if the control room is not present it will retry again until it sucessfully connects
        /// this method will return a task that completes when that first contact occurs or when it times out
        /// </remarks>
        /// <param name="timeoutS"></param>
        /// <returns></returns>
        public async Task WaitForPLCConnection(int timeoutS) {
            var timout = new CancellationTokenSource( timeoutS*1000 );
            bool PLC_alive = false;
            while(!timout.Token.IsCancellationRequested && !PLC_alive) {
                PLC_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) < 3000;
                Task.Delay( 50 ).Wait();
                if(PLC_alive) {
                    timout.Dispose();
                    logger.Info(Utilities.GetTimeStamp() + ": sucsefully conected to the PLC");
                    return;
                }
            }
            timout.Dispose();
            return;
        }

        public override bool RequestStopAsyncAcceptingClientsAndJoin() {
            MCU.RequestStopAsyncAcceptingClientsAndJoin();
            keep_modbus_server_alive = false;
            try
            {
                PLC_Modbusserver.Dispose();
                PLCTCPListener.Stop();
                ClientManagmentThread.Join();
            } catch (Exception e) {
                if ((e is ThreadStateException) || (e is ThreadStartException)) {
                    logger.Error(e);
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        public override void Bring_down() {
            RequestStopAsyncAcceptingClientsAndJoin();
           
        }

        /// <summary>
        /// this can be used as a heart beat tracker as the plc will poll the ctrl room every ~100 ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_Read_handler(object sender, ModbusSlaveRequestEventArgs e) {
            if (is_test) {
                logger.Info(Utilities.GetTimeStamp() + ": PLC Red data from the the control room");
            }
            PLC_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return;
        }


        /// <summary>
        /// fires whenever the data on the modbus server changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_Written_to_handler(object sender, DataStoreEventArgs e) {
            //e.Data.B //array representing data   
            if (is_test) {
                logger.Info(Utilities.GetTimeStamp() + ": recived message from PLC");
            }
            PLC_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            switch (e.StartAddress) {
                case (ushort)PLC_modbus_server_register_mapping.AZ_0_LIMIT: {
                        bool previous = limitSwitchData.Azimuth_CCW_Limit;
                        limitSwitchData.Azimuth_CCW_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_0_LIMIT] );
                        if(previous != limitSwitchData.Azimuth_CCW_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.AZ_0_LIMIT , limitSwitchData.Azimuth_CCW_Limit );
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_0_HOME: {
                        bool previous = homeSensorData.Azimuth_Home_One;
                        homeSensorData.Azimuth_Home_One = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_0_HOME] );
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_0_SECONDARY: {
                        bool previous = homeSensorData.Azimuth_Home_Two;
                        homeSensorData.Azimuth_Home_Two = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_0_SECONDARY] );
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_375_LIMIT: {
                        bool previous = limitSwitchData.Azimuth_CW_Limit;
                        limitSwitchData.Azimuth_CW_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_375_LIMIT] );
                        if(previous != limitSwitchData.Azimuth_CW_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.AZ_375_LIMIT , limitSwitchData.Azimuth_CW_Limit );
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT: {
                        bool previous = limitSwitchData.Elevation_Lower_Limit;
                        limitSwitchData.Elevation_Lower_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_10_LIMIT]);
                        if(previous != limitSwitchData.Elevation_Lower_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.EL_10_LIMIT , limitSwitchData.Elevation_Lower_Limit );
                            if (limitSwitchData.Elevation_Lower_Limit)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Lower Limit Switch Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation lower limit switch hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation lower limit switch hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_0_HOME: {
                        bool previous = homeSensorData.Elevation_Home;
                        homeSensorData.Elevation_Home = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_0_HOME] );
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT: {
                        bool previous = limitSwitchData.Elevation_Upper_Limit;
                        limitSwitchData.Elevation_Upper_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_90_LIMIT] );
                        if(previous != limitSwitchData.Elevation_Upper_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.EL_90_LIMIT , limitSwitchData.Elevation_Upper_Limit );
                            if (limitSwitchData.Elevation_Upper_Limit)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Upper Limit Switch Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation upper limit switch hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation upper limit switch hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK: {
                        bool previous = plcInput.Gate_Sensor;
                        plcInput.Gate_Sensor = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK] );
                        if(previous != plcInput.Gate_Sensor) {
                            if (plcInput.Gate_Sensor)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": gate opened");

                                pushNotification.sendToAllAdmins("GATE ACTIVITY", "Gate has been opened.");
                                EmailNotifications.sendToAllAdmins("GATE ACTIVITY", "Gate has been opened.");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": gate closed");

                                pushNotification.sendToAllAdmins("GATE ACTIVITY", "Gate has been closed.");
                                EmailNotifications.sendToAllAdmins("GATE ACTIVITY", "Gate has been closed.");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.E_STOP: {
                        bool previous = plcInput.Estop;
                        plcInput.Estop = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.E_STOP] );
                        if(previous != plcInput.Estop) {
                            if (plcInput.Estop)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Estop Hit");
                                CurrentMovementPriority = MovementPriority.Critical;

                                pushNotification.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been hit.");
                                EmailNotifications.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been hit.");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Estop released");
                                CurrentMovementPriority = MovementPriority.None;

                                pushNotification.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been released.");
                                EmailNotifications.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been released.");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_SLIP_CAPTURE: {
                        bool previous = plcInput.EL_Slip_CAPTURE;
                        plcInput.EL_Slip_CAPTURE = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_SLIP_CAPTURE] );
                        break;
                    }
            }
        }

        private void set_Local_registers(ushort[] data, ushort starting_adress) {
            logger.Info(data.Length + " dsv " + starting_adress);
            for (int i = 1; i < (data.Length - 1); i++) {
                PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] = data[i];
                logger.Info( " " + PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] + ",");
            }
        }

        /// <summary>
        /// Sets a register value on the PLC that corresponds to the given address.
        /// </summary>
        /// <param name="adr">The address of the value you want to set.</param>
        /// <param name="value">The value you want to set the register to.</param>
        public override void setregvalue(ushort adr, ushort value) {
            PLC_Modbusserver.DataStore.HoldingRegisters[adr] = value;
        }

        /// <summary>
        /// Retrieves a register value from the PLC that corresponds to the given address.
        /// </summary>
        /// <remarks>
        /// See ControlRoomApplication.Entities.PLC_modbus_server_register_mapping for register mapping.
        /// </remarks>
        /// <param name="adr">Address of the register.</param>
        public override ushort getregvalue(ushort adr)
        {
            return PLC_Modbusserver.DataStore.HoldingRegisters[adr];
        }

        public override bool Get_interlock_status() {
            return plcInput.Gate_Sensor;
        }

        private bool Int_to_bool(int val) {
            if (val == 0) {
                return false;
            } else { return true; }
        }


        //above PLC modbus server ___below MCU comands


        public override bool Test_Connection() {
            return TestIfComponentIsAlive();
        }

        /// <summary>
        /// send basic configuration to MCU
        /// </summary>
        /// <param name="startSpeedRPMAzimuth"></param>
        /// <param name="startSpeedRPMElevation"></param>
        /// <param name="homeTimeoutSecondsAzimuth"></param>
        /// <param name="homeTimeoutSecondsElevation"></param>
        /// <returns></returns>
        public override bool Configure_MCU(double startSpeedRPMAzimuth, double startSpeedRPMElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation) {
            return MCU.Configure_MCU(
                    new MCUConfigurationAxys() { StartSpeed = startSpeedRPMAzimuth, HomeTimeoutSec = homeTimeoutSecondsAzimuth },
                    new MCUConfigurationAxys() { StartSpeed = startSpeedRPMElevation, HomeTimeoutSec = homeTimeoutSecondsElevation,UseCapture =true,CaptureActive_High =true }
                );
        }

        /// <summary>
        /// this gets the position stored in the MCU which is based of the number of steps the MCU has taken since it was last 0ed out
        /// </summary>
        /// <returns></returns>
        public override Orientation GetMotorEncoderPosition() {
            return MCU.GetMotorEncoderPosition();
        }

        /// <summary>
        /// get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation 
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// see <see cref="MCUConstants.MCUStatusBitsMSW"/> for description of each bit
        /// </summary>
        public override bool[] GET_MCU_Status( RadioTelescopeAxisEnum axis ) {
            ushort start = 0;
            if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                start = 10;
            }
            ushort[] data = MCU.ReadMCURegisters(start , 2);
            bool[] target = new bool[32];
            for(int i = 0; i < 16; i++) {
                target[i] = ((data[0] >> i) & 1) == 1;
                target[i + 16] = ((data[1] >> i) & 1) == 1;

            }
            return target;
        }

        /// <summary>
        /// clears the previos move comand from mthe PLC, only works for jog moves
        /// </summary>
        /// <returns></returns>
        public override bool Cancel_move() {
            return MCU.Cancel_move();
        }

        /// <summary>
        /// send a hold move command to the MCu
        /// </summary>
        /// <returns></returns>
        public override bool ControlledStop(  ) {
            return MCU.ControlledStop();
        }

        public override bool ImmediateStop() {
            return MCU.ImmediateStop();
        }

        /// <summary>
        /// move a set number of steps at the specified steps / second *intended for debuging
        /// </summary>
        /// <param name="programmedPeakSpeedAZInt"></param>
        /// <param name="positionTranslationAZ"></param>
        /// <param name="positionTranslationEL"></param>
        /// <param name="targetOrientation">The target orientation.</param>
        /// <returns></returns>
        public override MovementResult RelativeMove(int programmedPeakSpeedAZInt, int positionTranslationAZ, int positionTranslationEL, Orientation targetOrientation) {
            return MCU.MoveAndWaitForCompletion(programmedPeakSpeedAZInt, programmedPeakSpeedAZInt, positionTranslationAZ, positionTranslationEL, targetOrientation);
        }

        public override MovementResult MoveToOrientation(Orientation target_orientation, Orientation current_orientation)
        {
            int positionTranslationAZ, positionTranslationEL;

            // Calculates the hard stop calculation by default
            double azimuthOrientationMovement = target_orientation.Azimuth - current_orientation.Azimuth;

            // If the type is a slip ring, checks to see if it needs to calculate a new route
            if(telescopeType == RadioTelescopeTypeEnum.SLIP_RING)
            {
                if(azimuthOrientationMovement > 180)
                {
                    azimuthOrientationMovement = (target_orientation.Azimuth - 360) - current_orientation.Azimuth;
                }
                else if(azimuthOrientationMovement < -180)
                {
                    azimuthOrientationMovement = (360 - current_orientation.Azimuth) + target_orientation.Azimuth;
                }
            }
            else if(telescopeType == RadioTelescopeTypeEnum.NONE)
            {
                logger.Info(Utilities.GetTimeStamp() + ": ERROR: Invalid Telescope Type!");
                return MovementResult.ValidationError;
            }

            positionTranslationAZ = ConversionHelper.DegreesToSteps(azimuthOrientationMovement, MotorConstants.GEARING_RATIO_AZIMUTH);
            positionTranslationEL = ConversionHelper.DegreesToSteps((target_orientation.Elevation - current_orientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);

            int EL_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( 0.6 ), MotorConstants.GEARING_RATIO_ELEVATION);
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( 0.6 ), MotorConstants.GEARING_RATIO_AZIMUTH);

            //(ObjectivePositionStepsAZ - CurrentPositionStepsAZ), (ObjectivePositionStepsEL - CurrentPositionStepsEL)
            logger.Info(Utilities.GetTimeStamp() + ": degrees target az " + target_orientation.Azimuth + " el " + target_orientation.Elevation);
            logger.Info(Utilities.GetTimeStamp() + ": degrees curren az " + current_orientation.Azimuth + " el " + current_orientation.Elevation);

            return MCU.MoveAndWaitForCompletion(
                AZ_Speed, EL_Speed,
                positionTranslationAZ, 
                positionTranslationEL, 
                target_orientation
            );
        }

        /// <summary>
        /// speed in RPM
        /// </summary>
        /// <param name="azSpeed"></param>
        /// <param name="azDirection"></param>
        /// <param name="elSpeed"></param>
        /// <param name="elDirection"></param>
        /// <returns></returns>
        public override MovementResult StartBothAxesJog(double azSpeed, RadioTelescopeDirectionEnum azDirection, double elSpeed, RadioTelescopeDirectionEnum elDirection) {
            return MCU.SendBothAxesJog(Math.Abs(azSpeed), azDirection, Math.Abs(elSpeed), elDirection);
        }

        /// <summary>
        /// Moves both axes to where the homing sensors are. After this is run, the position offset needs applied to the motors, and then
        /// the absolute encoders.
        /// </summary>
        /// <returns>True if homing was successful, false if it failed</returns>
        public override MovementResult HomeTelescope() {
            return MCU.HomeBothAxes(0.25);
        }
        
        /// <summary>
        /// Tests if both the PLC and MCU are alive and working.
        /// </summary>
        /// <returns>True if both the MCU and PLC are alive and working.</returns>
        public override bool TestIfComponentIsAlive() {

            bool PLC_alive, MCU_alive;
            try {
                long MCU_last_contact = MCU.getLastContact();
                PLC_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) < 3000;
                MCU_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - MCU_last_contact) < 3000;
                if(is_test) {
                    return true;
                }
                return PLC_alive && MCU_alive;
            } catch { return false; }
        }

        public override void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            telescopeType = type;

            // Also set the MCU's telescope type
            MCU.setTelescopeType(type);
        }

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// </summary>
        public override void ResetMCUErrors()
        {
            MCU.ResetMCUErrors();
        }

        /// <summary>
        /// This will check for any errors present in the MCU's registers.
        /// </summary>
        /// <returns>A list of errors present in the MCU's registers</returns>
        public override List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors()
        {
            return MCU.CheckMCUErrors();
        }

        /// <summary>
        /// This will interrupt the current movement, wait until it has stopped, and then
        /// end when the movement has stopped.
        /// 
        /// If no motors are moving when this is called, then it will not wait, and just be
        /// able to pass through.
        /// </summary>
        /// <returns>True if it had to wait for a movement, false if it did not have to wait and there is no movement running.</returns>
        public override bool InterruptMovementAndWaitUntilStopped()
        {
            bool motorsMoving = MCU.MotorsCurrentlyMoving();

            if (motorsMoving && CurrentMovementPriority != MovementPriority.None)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Overriding current movement...");
                MCU.MovementInterruptFlag = true;

                // Wait until motors stop moving or the interrupt flag is set back to false,
                // meaning the MCU has acknowledged and acted on the interrupt.
                while(MotorsCurrentlyMoving() && MCU.MovementInterruptFlag == true) ;

                MCU.MovementInterruptFlag = false;

                // Ensure there is plenty of time between MCU commands
                Thread.Sleep(2000);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the motors are currently moving.
        /// </summary>
        /// <param name="axis">Azimuth, elevation, or both.</param>
        /// <returns>True if moving, false if not moving.</returns>
        public override bool MotorsCurrentlyMoving(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            return MCU.MotorsCurrentlyMoving(axis);
        }

        public override void SetFinalOffset(Orientation finalPos)
        {
            MCU.FinalPositionOffset = finalPos;
        }

        public override RadioTelescopeDirectionEnum GetRadioTelescopeDirectionEnum(RadioTelescopeAxisEnum axis)
        {
            //ushort[] directionData = MCU.ReadMCURegisters(10, 1);

            switch (axis)
            {
                //Axes must be checked independently because the MCU commands for motor moves have the same value for both Az and El
                /* case RadioTelescopeAxisEnum.AZIMUTH:
                  ushort[] directionData = MCU.ReadMCURegisters(10, 1);
                   //only checking AZ because of possible sim bug 
                   if ((directionData[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion & 0b1) == 1)
                   {
                       logger.Info("Azimuth Counter Clockwise");
                       return RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;
                   }

                   if ((directionData[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion & 0b1) == 1)
                   {
                       logger.Info("Azimuth clockwise");
                       return RadioTelescopeDirectionEnum.ClockwiseOrNegative;
                   }

                   //check for homing, have to read 18 bits from the MCU
                   directionData = MCU.ReadMCURegisters(0, 18);
                   if ((directionData[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_LSW] >> (int)MCUConstants.MCUStutusBitsLSW.Home_Input & 0b1) == 1)
                   {
                       logger.Info("Azimuth Clockwise Homing");
                       return RadioTelescopeDirectionEnum.ClockwiseHoming;
                   }

                   break;
                   */

                case RadioTelescopeAxisEnum.ELEVATION:
                    //in practice, only need to check the elevation
                    ushort[] directionData = MCU.ReadMCURegisters(10, 2);
                   

                    if (((directionData[0] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1)
                    {
                        logger.Info("Elevation Counter Clockwise (in simulation, UP) ");
                        return RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;
                    }

                    if (((directionData[0] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1)
                    {
                        logger.Info("Elevation Clockwise (in simulation, DOWN)");
                        return RadioTelescopeDirectionEnum.ClockwiseOrNegative;
                    }

                   // directionData = MCU.ReadMCURegisters(0, 18);
                    if ((directionData[1] >> (int)MCUConstants.MCUStutusBitsLSW.Home_Input & 0b1) == 1)
                    {
                        logger.Info("Elevation Clockwise HOME");
                        return RadioTelescopeDirectionEnum.ClockwiseHoming;
                    }

                    break;

                default:
                    //This function can only accept a single axis (either Az or El)
                    //see comment at top of function
                    throw new ArgumentException();
            }

            return RadioTelescopeDirectionEnum.None;

            


        }

    }
}
