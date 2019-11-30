﻿using ControlRoomApplication.Constants;
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

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpListener PLCTCPListener;
        private TcpClient MCUTCPClient;
        private ModbusSlave PLC_Modbusserver;
        private ModbusIpMaster MCUModbusMaster;
        private SemaphoreSlim comand_acknoledged = new SemaphoreSlim(0, 1);
        private long PLC_last_contact;
        private long MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Thread MCU_Monitor_Thread;

        private static readonly ushort[] MESSAGE_CONTENTS_IMMEDIATE_STOP = new ushort[] {
            0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_HOLD_MOVE = new ushort[] {
            0x0004, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0004, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_CLEAR_MOVE = new ushort[] {  
            0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_RESET_ERRORS = new ushort[] {
            0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private bool keep_modbus_server_alive=true;
        private bool is_test= false;
        /// <summary>
        /// set this ONLY if using test driver, removes timouts and delays
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool set_is_test(bool val) { is_test = val;return is_test; }
        /// <summary>
        /// starts a modbus server to comunicate with the PLC on PLC_port and local_ip
        /// then sets up a modbus client to comunicate with the MCU located at MCU_ip, MCU_port (192.168.0.50 , 502) for actual hardware
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public ProductionPLCDriver(string local_ip,  string MCU_ip, int MCU_port, int PLC_port, bool startPLC) : base(local_ip,  MCU_ip, MCU_port, PLC_port, startPLC )
        {
            MCUTCPClient = new TcpClient(MCU_ip, MCU_port);
            MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
            try
            {
                PLCTCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse(local_ip), PLC_port));
                ClientManagmentThread = new Thread(new ThreadStart(HandleClientManagementThread));
                MCU_Monitor_Thread = new Thread( new ThreadStart( MonitorMCU ) );
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    logger.Info("[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString());
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
                    logger.Info("[AbstractPLCDriver] ERROR: failure starting PLC TCP server: " + e.ToString());
                    return;
                }
            }

        }

       

        /// <summary>
        /// runs the modbus server to interface with the plc
        /// </summary>
        protected override void HandleClientManagementThread() {
            byte slaveId = 1;
            // create and start the TCP slave
            PLC_Modbusserver = ModbusTcpSlave.CreateTcp( slaveId , PLCTCPListener );
            //coils, inputs, holdingRegisters, inputRegisters
            PLC_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore( 0 , 0 , 256 , 0 );
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            PLC_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>( Server_Read_handler );
            PLC_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>( Server_Written_to_handler );

            PLC_Modbusserver.Listen();

            //PLC_Modbusserver.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            while(keep_modbus_server_alive) {
                Thread.Sleep( 100 );
            }
        }

        public override bool StartAsyncAcceptingClients() {
            keep_modbus_server_alive = true;
            try {
                MCU_Monitor_Thread.Start();
                ClientManagmentThread.Start();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is OutOfMemoryException)) {
                    Console.WriteLine( "failed to start prodi=uction plc and mcu threads err:____    {0}" ,e);
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }


        public override bool RequestStopAsyncAcceptingClientsAndJoin() {
            keep_modbus_server_alive = false;
            try {
                PLCTCPListener.Stop();
                PLC_Modbusserver.Dispose();
                ClientManagmentThread.Join();
                MCU_Monitor_Thread.Join();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is ThreadStartException)) {
                    Console.WriteLine( e );
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
        private void Server_Read_handler( object sender , ModbusSlaveRequestEventArgs e ) {
            if(is_test) {
                Console.WriteLine( "PLC Red data from the the control room" );
            }
            PLC_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // Console.WriteLine(e.Message);
            return;
            /*
            Regex rx = new Regex(@"\b(?:Read )([0-9]+)(?:.+)(?:address )([0-9]+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(e.Message.ToString());
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine("'{0}' repeated at positions {1} and {2}", groups["word"].Value, groups[0].Index, groups[1].Index);
            }
            //*/
        }


        /// <summary>
        /// fires whenever the data on the modbus server changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_Written_to_handler( object sender , DataStoreEventArgs e ) {
            //e.Data.B //array representing data   
            if(is_test) {
                Console.WriteLine( "recived message from PLC" );
            }
            PLC_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            switch(e.StartAddress) {
                case (ushort)PLC_modbus_server_register_mapping.CMD_ACK: {
                        // Console.WriteLine(" data {0} written to 22",PLC_Modbusserver.DataStore.HoldingRegisters[e.StartAddress]);
                        try {
                            //comand_acknoledged.Release();
                        } catch(Exception err) {
                            Console.WriteLine( err );
                        }
                        break;

                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_LEFT_LIMIT: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_LEFT_WARNING: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_WARNING: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_LIMIT: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_LIMIT: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_WARNING: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_TOP_WARNING: {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_TOP_LIMIT: {

                        break;
                    }
            }
        }

        private void set_Local_registers( ushort[] data , ushort starting_adress ) {
            Console.WriteLine( "{0}   dsv  {1} " , data.Length , starting_adress );
            for(int i = 1; i < (data.Length - 1); i++) {
                PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] = data[i];
                Console.Write( " {0}," , PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        /// <param name="value"></param>
        public void setregvalue( ushort adr , ushort value ) {
            PLC_Modbusserver.DataStore.HoldingRegisters[adr] = value;
        }

        /// <summary>
        /// see   ControlRoomApplication.Entities.PLC_modbus_server_register_mapping
        /// for register maping
        /// </summary>
        /// <param name="adr"></param>
        /// <returns></returns>
        public ushort readregval( ushort adr ) {
            return PLC_Modbusserver.DataStore.HoldingRegisters[adr];
        }



        public override bool Get_interlock_status() {
            return Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.Safety_INTERLOCK] );
        }


        public override bool[] Get_Limit_switches() {
            return new bool[] {
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_LEFT_LIMIT]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_LEFT_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_LIMIT]),

                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_LIMIT]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_TOP_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_TOP_LIMIT])
            };
        }


        private bool Int_to_bool( int val ) {
            Console.WriteLine( val );
            if(val == 0) {
                return false;
            } else { return true; }
        }


        //above PLC modbus server ___below MCU comands






        public override bool Test_Conection() {
            return TestIfComponentIsAlive();
        }


        private void MonitorMCU() {
            int lastMCUHeartbeatBit = 0;
            while(keep_modbus_server_alive) {
                ushort network_status = MCUModbusMaster.ReadHoldingRegisters( 9 , 1 )[0];
                int CurrentHeartBeat = (network_status >> 14)&1;//this bit changes every 500ms
                if (CurrentHeartBeat != lastMCUHeartbeatBit) {
                    MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
                lastMCUHeartbeatBit = CurrentHeartBeat;
                if(((network_status >> 13) & 1) == 1) {
                    logger.Warn( "MCU network disconected, reseting errors" );
                    MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_RESET_ERRORS );

                }
                Thread.Sleep( 250 );
            }
        }

        /// <summary>
        /// This is a script that is called when we want to check the current thermal calibration of the telescope
        /// Moves to point to the tree, reads in data, gets data from weather station, and compares
        /// Postcondition: return true if the telescope data IS within 0.001 degrees Farenheit
        ///                return false if the telescope data IS NOT within 0.001 degrees Farenheit
        /// </summary>
        public override bool Thermal_Calibrate() {
            Orientation current = read_Position();
            Move_to_orientation(MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION, current);

            // start a timer so we can have a time variable
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // read data
            SpectraCyberResponse response = Parent.SpectraCyberController.DoSpectraCyberScan();

            // end the timer
            stopWatch.Stop();
            double time = stopWatch.Elapsed.TotalSeconds;

            RFData rfResponse = RFData.GenerateFrom(response);

            // move back to previous location
            Move_to_orientation(current, MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION);

            // analyze data
            // temperature (Kelvin) = (intensity * time * wein's displacement constant) / (Planck's constant * speed of light)
            double weinConstant = 2.8977729;
            double planckConstant = 6.62607004 * Math.Pow(10, -34);
            double speedConstant = 299792458;
            double temperature = (rfResponse.Intensity * time * weinConstant) / (planckConstant * speedConstant);

            // convert to farenheit
            temperature = temperature * (9 / 5) - 459.67;

            // check against weather station reading
            double weatherStationTemp = Parent.WeatherStation.GetOutsideTemp();

            // return true if working correctly, false if not
            return Math.Abs(weatherStationTemp - temperature) < 0.001;
        }

        /// <summary>
        /// This is a script that is called when we want to dump snow out of the dish
        /// </summary>
        public override bool SnowDump()
        {
            // default is azimuth of 0 and elevation of 0
            Orientation dump = new Orientation();
            Orientation current = read_Position();

            // move to dump snow
            if(Move_to_orientation(dump, current))
            {
                // move back to initial orientation
                return Move_to_orientation(current, read_Position());
            }
            return false;
        }

        /// <summary>
        /// Moves the telescope to the stowed position
        /// </summary>
        public override bool Stow()
        {
            Orientation stow = new Orientation(0, 90);

            return Move_to_orientation(stow, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the left azimuth switch
        /// </summary>
        public override bool HitAzimuthLeftLimitSwitch()
        {
            Orientation AZLeftLimit = new Orientation(-9, 0);

            return Move_to_orientation(AZLeftLimit, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the right azimuth switch
        /// </summary>
        public override bool HitAzimuthRightLimitSwitch()
        {
            Orientation AZRightLimit = new Orientation(369, 0);

            return Move_to_orientation(AZRightLimit, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the lower elevation switch
        /// </summary>
        public override bool HitElevationLowerLimitSwitch()
        {
            Orientation ELLowerLimit = new Orientation(0, -14);

            return Move_to_orientation(ELLowerLimit, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the upper elevation switch
        /// </summary>
        public override bool HitElevationUpperLimitSwitch()
        {
            Orientation ELUpperLimit = new Orientation(0, 92);

            return Move_to_orientation(ELUpperLimit, read_Position());
        }

        /// <summary>
        /// Recovers the telescope when a limit switch is hit
        /// </summary>
        public override bool RecoverFromLimitSwitch()
        {
            Orientation currentPos = read_Position();

            Orientation safe;

            bool safeAz = false;
            bool safeEl = false;

            // Loops through just in case the move fails or if it as hit two limit switches
            while (true)
            {
                // Checks to see if the left az switch has been hit
                /// TODO: Update to also use limit switch sensors
                if (currentPos.Azimuth <= -8 && !safeAz)
                {
                    safe = new Orientation(0, currentPos.Elevation);

                    safeAz = Move_to_orientation(safe, currentPos);
                }
                // Checks to see if the right az switch has been hit
                /// TODO: Update to also use limit switch sensors
                else if (currentPos.Azimuth >= 368 && !safeAz)
                {
                    safe = new Orientation(360, currentPos.Elevation);

                    safeAz = Move_to_orientation(safe, currentPos);
                }
                else
                    safeAz = true;

                // Checks to see if the lower el switch has been hit
                /// TODO: Update to also use limit switch sensors
                if (currentPos.Elevation <= -13 && !safeEl)
                {
                    safe = new Orientation(currentPos.Azimuth, 0);

                    safeEl = Move_to_orientation(safe, currentPos);
                }
                // Checks to see if the upper el switch has been hit
                /// TODO: Update to also use limit switch sensors
                else if (currentPos.Elevation >= 91 && !safeEl)
                {
                    safe = new Orientation(currentPos.Azimuth, 85);

                    safeEl = Move_to_orientation(safe, currentPos);
                }
                else
                    safeEl = true;

                // Check to see if the telescope is in a safe state
                if (safeAz && safeEl)
                    return true;
            }
        }

        /// <summary>
        /// Moves the telescope from its current position to a start position at
        /// 0 degrees elevation, then moves to 90 degrees, then returns to its
        /// initial position
        /// </summary>
        public override bool FullElevationMove()
        {
            Orientation currentPos = read_Position();

            bool elStartFlag = false;
            bool elFinishFlag = false;

            Orientation elStart = new Orientation(currentPos.Azimuth, 0); ;
            Orientation elFinish = new Orientation(currentPos.Azimuth, 90);

            // Moves elevation to start position if not already there
            if (currentPos.Elevation == 0)
                elStartFlag = true;
            else
                elStartFlag = Move_to_orientation(elStart, currentPos);

            // Moves elevation to the finish position
            if (elStartFlag)
                elFinishFlag = Move_to_orientation(elFinish, elStart);

            // Moves elevation to the original position
            if (elFinishFlag)
                return Move_to_orientation(currentPos, elFinish);
            else
                return elFinishFlag;
        }
                
        /// This is a script that is called when we want to move the telescope in a full 360 degree azimuth rotation
        /// The counter clockwise direction
        /// </summary>
        public override bool Full_360_CCW_Rotation()
        {
            Orientation current = read_Position();
            Orientation start = new Orientation(360, 0);
            Orientation finish = new Orientation(0, 0);

            if(Move_to_orientation(start, current) && Move_to_orientation(finish, start))
            {
                return Move_to_orientation(current, finish);
            }
            return false;

        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope in a full 360 degree azimuth rotation
        /// The clockwise direction
        /// </summary>
        public override bool Full_360_CW_Rotation()
        {
            Orientation current = read_Position();
            Orientation start = new Orientation(0, 0);
            Orientation finish = new Orientation(360, 0);

            if (Move_to_orientation(start, current) && Move_to_orientation(finish, start))
            {
                return Move_to_orientation(current, finish);
            }
            return false;
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope to the CW hardware stop
        /// </summary>
        public override bool Hit_CW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation hardstop = new Orientation(370, current.Elevation);

            return Move_to_orientation(hardstop, current);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope to the CCW hardware stop
        /// </summary>
        public override bool Hit_CCW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation hardstop = new Orientation(-10, current.Elevation);

            return Move_to_orientation(hardstop, current);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope from the current position
        /// to a safe position away from the hardstop
        /// Precondition: The telescope just hit the clockwise hardstop
        /// Postcondition: The telescope will be placed at 360 degrees azimuth (safe spot away from hard stop)
        /// </summary>
        public override bool Recover_CW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation recover = new Orientation(360, current.Elevation);

            return Move_to_orientation(recover, current);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope from the current position
        /// to a safe position away from the hardstop
        /// Precondition: The telescope just hit the counter clockwise hardstop
        /// Postcondition: The telescope will be placed at 0 degrees azimuth (safe spot away from hard stop)
        /// </summary>
        public override bool Recover_CCW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation recover = new Orientation(0, current.Elevation);

            return Move_to_orientation(recover, current);
        }

        public override bool Configure_MCU( double startSpeedDPSAzimuth , double startSpeedDPSElevation , int homeTimeoutSecondsAzimuth , int homeTimeoutSecondsElevation ) {
            int gearedSpeedAZ = ConversionHelper.DPSToSPS( startSpeedDPSAzimuth , MotorConstants.GEARING_RATIO_AZIMUTH );
            int gearedSpeedEL = ConversionHelper.DPSToSPS( startSpeedDPSElevation , MotorConstants.GEARING_RATIO_ELEVATION );
            //gearedSpeedAZ = startSpeedDPSAzimuth;
            //gearedSpeedEL = startSpeedDPSElevation;
            if((gearedSpeedAZ < 1) || (gearedSpeedEL < 1) || (homeTimeoutSecondsAzimuth < 0) || (homeTimeoutSecondsElevation < 0)
             || (gearedSpeedAZ > 1000000) || (gearedSpeedEL > 1000000) || (homeTimeoutSecondsAzimuth > 300) || (homeTimeoutSecondsElevation > 300)) {
                return false;
            }
            ushort[] data = {   0x8400, 0x0000, (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), (ushort)homeTimeoutSecondsAzimuth,   0x0,    0x0,    0x0,                                 0x0,                            0x0,
                                0x8400, 0x0000, (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), (ushort)homeTimeoutSecondsElevation, 0x0,    0x0,    0x0,                                0x0,                             0x0
                                };
            //set_multiple_registers( data,  1);
            MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , data );
            Thread.Sleep( 100 );
            MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_CLEAR_MOVE );
            return true;
        }
        /// <summary>
        /// this gets the position stored in the MCU which is based of the number of steps the MCU has taken since it was last 0ed out
        /// </summary>
        /// <returns></returns>
        public override Orientation read_Position(){
            ushort[] data = MCUModbusMaster.ReadHoldingRegisters(2, 12);
           // Console.WriteLine("AZ_finni2 {0,10} EL_finni2 {1,10}", (65536 * data[0]) + data[1], (65536 * data[10]) + data[11]);
            Orientation current_orientation = new Orientation(
                ConversionHelper.StepsToDegrees((data[0]<<16) + data[1], MotorConstants.GEARING_RATIO_AZIMUTH), 
                ConversionHelper.StepsToDegrees((data[10]<<16) + data[11], MotorConstants.GEARING_RATIO_ELEVATION)
            );
            return current_orientation;
        }
        /// <summary>
        /// get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation 
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// </summary>
        public override async Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis ) {
            ushort start = 0;
            if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                start = 10;
            }
            ushort[] data = MCUModbusMaster.ReadHoldingRegisters( start , 2 );
            bool[] target = new bool[32];
            for(int i = 0; i < 16; i++) {
                target[i] = ((data[0] >> i) & 1) == 1;
                target[i + 16] = ((data[1] >> i) & 1) == 1;

            }
            return target;
        }

        /// <summary>
        /// clears the previos move comand from mthe PLC
        /// </summary>
        /// <returns></returns>
        public override bool Cancel_move() {
            MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_CLEAR_MOVE );
            return true;
        }


        public override bool Controled_stop( RadioTelescopeAxisEnum axis , bool both ) {
            ushort[] data = new ushort[] {
                0x0004 , 0x0003 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0,
                0x0004 , 0x0003 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0
            };
            //MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , data );
            //return true;
            return Cancel_move();
            /*
            ushort[] data = new ushort[] { 0x4 , 0x3 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 , 0x0 };
            if(both) {
                MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_HOLD_MOVE );
                return true;
            } else if (axis== RadioTelescopeAxisEnum.AZIMUTH) {
                MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , data );
                return true;
            } else if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS+10 , data );
                return true;
            }
            return false;
            //*/
        }

        public override bool Immediade_stop() {
            MCUModbusMaster.WriteMultipleRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_IMMEDIATE_STOP );
            return true;
        }

        // Is called when the PLC and/or MCU is shutdown, stows the telescope
        public override bool Shutdown_PLC_MCU()
        {
            return Stow();
        }

        public override bool relative_move( int programmedPeakSpeedAZInt , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            return send_relative_move_sync( programmedPeakSpeedAZInt , programmedPeakSpeedAZInt , ACCELERATION , positionTranslationAZ , positionTranslationEL );
            //return sendmovecomand( programmedPeakSpeedAZInt , ACCELERATION , positionTranslationAZ , positionTranslationEL ).GetAwaiter().GetResult();   //.ContinueWith(antecedent => { return antecedent.; });
        }


        public override bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation)
        {
            int positionTranslationAZ, positionTranslationEL;
            positionTranslationAZ = ConversionHelper.DegreesToSteps((target_orientation.Azimuth - current_orientation.Azimuth), MotorConstants.GEARING_RATIO_AZIMUTH);
            positionTranslationEL = ConversionHelper.DegreesToSteps((target_orientation.Elevation - current_orientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);

            int EL_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.2), MotorConstants.GEARING_RATIO_ELEVATION);
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( 0.2 ) , MotorConstants.GEARING_RATIO_AZIMUTH );

            //(ObjectivePositionStepsAZ - CurrentPositionStepsAZ), (ObjectivePositionStepsEL - CurrentPositionStepsEL)
            Console.WriteLine("degrees target az "+target_orientation.Azimuth + " el " + target_orientation.Elevation);
            Console.WriteLine("degrees curren az " + current_orientation.Azimuth + " el " + current_orientation.Elevation);


            //return sendmovecomand( EL_Speed * 20 , 50 , positionTranslationAZ , positionTranslationEL ).GetAwaiter().GetResult();
            return send_relative_move( AZ_Speed , EL_Speed ,50, positionTranslationAZ , positionTranslationEL ).GetAwaiter().GetResult();
        }

        public override bool Start_jog( RadioTelescopeAxisEnum axis , int speed , bool clockwise ) {
            ushort adress = MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, dir;
            if(clockwise) {
                dir = 0x0080;
            } else dir = 0x0100;
            //                                         reserved       msb speed                 lsb speed                acc                                                       dcc                                                    reserved
            ushort[] data = new ushort[] { dir , 0x0003 , 0x0 , 0x0 , (ushort)(speed >> 16) , (ushort)(speed & 0xffff) , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , 0x0 , 0x0 };
            // this is a jog comand for a single axis
            // RadioTelescopeAxisEnum jogging_axies = Is_jogging();
            switch(axis) {
                case RadioTelescopeAxisEnum.AZIMUTH: {
                        break;
                    }
                case RadioTelescopeAxisEnum.ELEVATION: {
                        adress = MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + 10;
                        break;
                    }
                case RadioTelescopeAxisEnum.BOTH: {
                        ushort[] data2 = new ushort[data.Length * 2];
                        data.CopyTo( data2 , 0 );
                        data.CopyTo( data2 , data.Length );
                        data = data2;
                        break;
                    }
                default: {
                        throw new ArgumentException( "Invalid RadioTelescopeAxisEnum value can be AZIMUTH, ELEVATION or BOTH got: " + axis );
                    }
            }
            MCUModbusMaster.WriteMultipleRegisters( adress , data );
            return true;
            //throw new NotImplementedException();
        }

        public async Task<bool> send_relative_move( int SpeedAZ , int SpeedEL , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            bool Sucess = true;
            await MCUModbusMaster.WriteMultipleRegistersAsync( 1024 , MESSAGE_CONTENTS_CLEAR_MOVE );//write a no-op to the mcu
            if(!is_test) {
                Task task = Task.Delay( 100 );//wait to ensure it is porcessed
                await task;
            }
            ushort[] data = {
                0x0002 , 0x0003, (ushort)((positionTranslationAZ & 0xFFFF0000)>>16),(ushort)(positionTranslationAZ & 0xFFFF),(ushort)((SpeedAZ & 0xFFFF0000)>>16),(ushort)(SpeedAZ & 0xFFFF), ACCELERATION,ACCELERATION ,0,0,
                0x0002 , 0x0003, (ushort)((positionTranslationEL & 0xFFFF0000)>>16),(ushort)(positionTranslationEL & 0xFFFF),(ushort)((SpeedEL & 0xFFFF0000)>>16),(ushort)(SpeedEL & 0xFFFF), ACCELERATION,ACCELERATION ,0,0
            };
            await MCUModbusMaster.WriteMultipleRegistersAsync( 1024 , data );
            return Sucess;
        }

        public  bool send_relative_move_sync( int SpeedAZ , int SpeedEL , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            bool Sucess = true;
            MCUModbusMaster.WriteMultipleRegisters( 1024 , MESSAGE_CONTENTS_CLEAR_MOVE );//write a no-op to the mcu
            if(!is_test) {
                Task task = Task.Delay( 100 );//wait to ensure it is porcessed
                task.Wait();
            }
            ushort[] data = {
                0x0002 , 0x0003, (ushort)((positionTranslationAZ & 0xFFFF0000)>>16),(ushort)(positionTranslationAZ & 0xFFFF),(ushort)((SpeedAZ & 0xFFFF0000)>>16),(ushort)(SpeedAZ & 0xFFFF), ACCELERATION,ACCELERATION ,0,0,
                0x0002 , 0x0003, (ushort)((positionTranslationEL & 0xFFFF0000)>>16),(ushort)(positionTranslationEL & 0xFFFF),(ushort)((SpeedEL & 0xFFFF0000)>>16),(ushort)(SpeedEL & 0xFFFF), ACCELERATION,ACCELERATION ,0,0
            };
            MCUModbusMaster.WriteMultipleRegisters( 1024 , data );
            return Sucess;
        }

        public async Task<bool> sendmovecomand( int programmedPeakSpeedAZInt , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            bool Sucess = true;
            //the mcu registers need to be reset befor a new comand can be set in case the same comand is sent multiple times in a row
            MCUModbusMaster.WriteMultipleRegisters( 1024 , MESSAGE_CONTENTS_CLEAR_MOVE );//write a no-op to the mcu
            if(!is_test) {
                Task task = Task.Delay( 100 );//wait to ensure it is porcessed
                await task;
            }
            ushort[] data = {0, 0x0403,
                            (ushort)(programmedPeakSpeedAZInt >> 0x10), (ushort)(programmedPeakSpeedAZInt & 0xFFFF), ACCELERATION, ACCELERATION,
                            (ushort)((positionTranslationAZ & 0xFFFF0000)>>16), (ushort)(positionTranslationAZ & 0xFFFF),
                            0, 0, 0, 0,
                            (ushort)((positionTranslationEL & 0xFFFF0000)>>16) , (ushort)(positionTranslationEL & 0xFFFF),
                            0, 0, 0, 0, 0, 0
            };
            MCUModbusMaster.WriteMultipleRegisters( 1024 , data );
            return Sucess;
        }

        public RadioTelescopeAxisEnum Is_jogging()//////////
        {
            ushort[] data = MCUModbusMaster.ReadHoldingRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, 20);
            //word 0 and 10 indicate a jog comand for a specific axis however if the position registers(2,3  && 12,13) have non zero value then its a REGISTRATION MOVE
            if ((data[10] == 0x0100|| data[10] == 0x0080) && (data[0] == 0x0100 || data[0] == 0x0080) && data[2] == 0x0 && data[3] == 0x0 && data[12] == 0x0 && data[13] == 0x0)//both jogging
            {
                return RadioTelescopeAxisEnum.BOTH;
            }
            else if ((data[10] == 0x0100 || data[10] == 0x0080) && data[12] == 0x0 && data[13] == 0x0)//el is jogging
            {
                return RadioTelescopeAxisEnum.ELEVATION;
            }
            else if ((data[0] == 0x0100 || data[0] == 0x0080) && data[2] == 0x0 && data[3] == 0x0)//az is jogging
            {
                return RadioTelescopeAxisEnum.AZIMUTH;
            }
            else return RadioTelescopeAxisEnum.UNKNOWN;
        }

        protected override bool TestIfComponentIsAlive() {

            bool PLC_alive, MCU_alive;
            PLC_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) < 3000;
            MCU_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - MCU_last_contact) < 3000;
            if(is_test) {
                //return true;
                Console.WriteLine( "{0}   {1} ",(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) , (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - MCU_last_contact) );
            }
            return PLC_alive && MCU_alive;
        }
        /// <summary>
        /// public version of TestIfComponentIsAlive
        /// </summary>
        /// <returns></returns>
        public bool workaroundAlive() {
            return TestIfComponentIsAlive();
        }
    }
}
