using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ControlRoomApplication.Util;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;

namespace ControlRoomApplication.Simulators.Hardware.PLC_MCU {
    class Simulation_control_pannel {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpListener MCU_TCPListener;
        private ModbusSlave MCU_Modbusserver;

        private TcpClient PLCTCPClient;
        public ModbusIpMaster PLCModbusMaster;

        private Thread MCU_emulator_thread;
        private Thread PLC_emulator_thread;

        private string PLC_ip;
        private int PLC_port;

        private bool runsimulator = true, mooving = false, jogging = false, isconfigured = false, isTest = false;

        private int acc, distAZ, distEL, currentAZ, currentEL, AZ_speed, EL_speed;

        private int EL0Lim_ON = -ConversionHelper.DegreesToSteps(15, MotorConstants.GEARING_RATIO_ELEVATION );
        private int EL90Lim_ON = ConversionHelper.DegreesToSteps(93, MotorConstants.GEARING_RATIO_ELEVATION );
        
        private int EL0Lim_OFF = -ConversionHelper.DegreesToSteps(4, MotorConstants.GEARING_RATIO_ELEVATION );
        private int EL90Lim_OFF = ConversionHelper.DegreesToSteps(89, MotorConstants.GEARING_RATIO_ELEVATION );

        bool AZ10LimStatus = false, AZ370LimStatus = false, EL0LimStatus = false, EL90LimStatus = false;

        public Simulation_control_pannel( string PLC_ip , string MCU_ip , int MCU_port , int PLC_port , bool istest ) {

            this.PLC_ip = PLC_ip;
            this.PLC_port = PLC_port;
            isTest = istest;
            try {

                MCU_TCPListener = new TcpListener( new IPEndPoint( IPAddress.Parse( MCU_ip ) , MCU_port ) );
                MCU_emulator_thread = new Thread( new ThreadStart( Run_MCU_server_thread ) ) { Name="MCU Simulator Thread"};
            } catch(Exception e) {
                if((e is ArgumentNullException) || (e is ArgumentOutOfRangeException)) {
                    logger.Error(e);
                    return;
                } else { throw e; }// Unexpected exception
            }
            try {
                MCU_TCPListener.Start( 1 );
            } catch(Exception e) {
                if((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException)) {
                    logger.Error(e);
                    return;
                }
            }
            runsimulator = true;
            MCU_emulator_thread.Start();
        }

        public void startPLC() {
            try {
                PLC_emulator_thread = new Thread( new ThreadStart( Run_PLC_emulator_thread ) ) { Name = "PLC simulator thread"};
                PLC_emulator_thread.Start();
            } catch(Exception e) {
                if((e is ArgumentNullException) || (e is ArgumentOutOfRangeException)) {
                    logger.Error(e);
                    return;
                }

            }
        }
        public void Bring_down()
        {
            runsimulator = false;
            if(MCU_Modbusserver != null) MCU_Modbusserver.Dispose();
            PLC_emulator_thread.Join();
            PLCTCPClient.Dispose();
            PLCModbusMaster.Dispose();
            if(MCU_emulator_thread.IsAlive) MCU_emulator_thread.Join();
            MCU_TCPListener.Stop();
        }

        private void Run_PLC_emulator_thread() {
            while(runsimulator) {
                try {
                    PLCTCPClient = new TcpClient( this.PLC_ip , this.PLC_port );
                    PLCModbusMaster = ModbusIpMaster.CreateIp( PLCTCPClient );
                } catch {//no server setup on control room yet 
                    logger.Info(Utilities.GetTimeStamp() + ": ________________PLC sim awaiting control room");
                }
                logger.Info(Utilities.GetTimeStamp() + ": ________________PLC sim running");
                PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK , new ushort[] { BoolToInt( true ) } );
                while(runsimulator) {
                    if(isTest) {
                        PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK , new ushort[] { BoolToInt( true ) } );
                        Thread.Sleep( 5 );
                        continue;
                    } else {
                        if(!EL0LimStatus) {
                            if(currentEL < EL0Lim_ON) {
                                EL0LimStatus = (currentEL < EL0Lim_ON);
                                PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT , new ushort[] { BoolToInt( !EL0LimStatus ) } );
                            }
                        } else if(currentEL > EL0Lim_OFF) {
                            EL0LimStatus = (currentEL > EL0Lim_OFF);
                            PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT , new ushort[] { BoolToInt( !EL0LimStatus ) } );
                        }

                        if(!EL90LimStatus) {
                            if(currentEL > EL90Lim_ON) {
                                EL90LimStatus = (currentEL > EL90Lim_ON);
                                PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT , new ushort[] { BoolToInt( !EL90LimStatus ) } );
                            }
                        } else if(currentEL < EL90Lim_OFF) {
                            EL90LimStatus = (currentEL < EL90Lim_OFF);
                            PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT , new ushort[] { BoolToInt( !EL90LimStatus ) } );
                        }
                    }
                    Thread.Sleep( 50 );
                }
            }
        }

        private void Server_Written_to_handler( object sender , DataStoreEventArgs e ) {
            MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
            MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] & 0xff7f);

            ushort[] data = new ushort[20];//e.Data.B.Count
            if (e.StartAddress >= 1023)
            {
                data = Copy_modbus_registers(1025, 20);
                handleTestCMD(data);
            }
        }

        private void Run_MCU_server_thread() {
            byte slaveId = 1;
            // create and start the TCP slave
            MCU_Modbusserver = ModbusTcpSlave.CreateTcp( slaveId , MCU_TCPListener );
            //coils, inputs, holdingRegisters, inputRegisters
            MCU_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore( 0 , 0 , 1054 , 0 );
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            //MCU_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            if(isTest) {
                MCU_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>( Server_Written_to_handler );
            }

            MCU_Modbusserver.Listen();

            // prevent the main thread from exiting
            ushort[] previos_out, current_out;
            previos_out = Copy_modbus_registers( 1025 , 20 );
            while(runsimulator) {
                Thread.Sleep( 1 );
                if(isTest) {
                    continue;
                }
                current_out = Copy_modbus_registers( 1025 , 20 );
                if(!current_out.SequenceEqual( previos_out )) {
                    handleCMD( current_out );
                    //Console.WriteLine("data changed");
                }
                if(mooving) {
                    if(distAZ != 0 || distEL != 0) {
                        int travAZ = (distAZ < -AZ_speed) ? -AZ_speed : (distAZ > AZ_speed) ? AZ_speed : distAZ;
                        int travEL = (distEL < -EL_speed) ? -EL_speed : (distEL > EL_speed) ? EL_speed : distEL;
                        move( travAZ , travEL );
                    } else {
                        mooving = false;
                        MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] | 0x0080);
                        MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] | 0x0080);
                    }
                }
                if(jogging) {
                    move( AZ_speed , EL_speed );
                }
                previos_out = current_out;
            }


        }

        private bool move( int travAZ , int travEL ) {
            distAZ -= travAZ;
            distEL -= travEL;
            currentAZ += travAZ;
            currentEL += travEL;

            MCU_Modbusserver.DataStore.HoldingRegisters[3] = (ushort)((currentAZ & 0xffff0000) >> 16);
            MCU_Modbusserver.DataStore.HoldingRegisters[4] = (ushort)(currentAZ & 0xffff);
            MCU_Modbusserver.DataStore.HoldingRegisters[13] = (ushort)((currentEL & 0xffff0000) >> 16);
            MCU_Modbusserver.DataStore.HoldingRegisters[14] = (ushort)(currentEL & 0xffff);

            MCU_Modbusserver.DataStore.HoldingRegisters[5] = (ushort)(((int)(currentAZ/2.5) & 0xffff0000) >> 16);
            MCU_Modbusserver.DataStore.HoldingRegisters[6] = (ushort)((int)(currentAZ / 2.5) & 0xffff);
            MCU_Modbusserver.DataStore.HoldingRegisters[15] = (ushort)(((int)(currentEL / 2.5) & 0xffff0000) >> 16);
            MCU_Modbusserver.DataStore.HoldingRegisters[16] = (ushort)((int)(currentEL / 2.5) & 0xffff);
            return true;
        }


        private bool handleCMD( ushort[] data ) {
            string outstr = " inreg";
            for(int v = 0; v < data.Length; v++) {
                outstr += Convert.ToString( data[v] , 16 ).PadLeft( 5 ) + ",";
            }
            Console.WriteLine(outstr);
            jogging = false;
            if((data[0] & 0xf000) == 0x8000) {//if not configured dont move

                isconfigured = true;
            } else if(!isconfigured) {
                return true;
            }

            if (data[1] == 0x0403) {//move cmd
                mooving = true;
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] & 0xff7f);
                AZ_speed = (data[2] << 16) + data[3];
                AZ_speed /= 250;
                EL_speed = AZ_speed;
                acc = data[4];
                distAZ = (data[6] << 16) + data[7];
                distEL = (data[12] << 16) + data[13];
                Console.WriteLine("moving to at ({0} , {1}) at ({2} , {3}) steps per second", distAZ, distEL, AZ_speed, EL_speed);
                return true;
            } else if (data[0] == 0x0080 || data[0] == 0x0100 || data[10] == 0x0080 || data[10] == 0x0100) {
                jogging = true;
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] & 0xff7f);
                if (data[0] == 0x0080) {
                    AZ_speed = ((data[4] << 16) + data[5]) / 1000;
                } else if (data[0] == 0x0100) {
                    AZ_speed = -((data[4] << 16) + data[5]) / 1000;
                } else {
                    AZ_speed = 0;
                }
                if (data[10] == 0x0080) {
                    EL_speed = ((data[14] << 16) + data[15]) / 1000;
                } else if (data[10] == 0x0100) {
                    EL_speed = -((data[14] << 16) + data[15]) / 1000;
                } else {
                    EL_speed = 0;
                }
                Console.WriteLine("jogging at {0}   {1}", AZ_speed, EL_speed);
                return true;
            } else if (data[0] == 0x0002 || data[10] == 0x0002) {//move cmd
                mooving = true;
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] & 0xff7f);
                AZ_speed = ((data[4] << 16) + data[5]) / 250;
                EL_speed = ((data[14] << 16) + data[15]) / 250;
                acc = data[6];
                distAZ = (data[2] << 16) + data[3];
                distEL = (data[12] << 16) + data[13];
                Console.WriteLine("also moving to at ({0} , {1}) at ({2} , {3}) steps per second", distAZ, distEL, AZ_speed, EL_speed);
                return true;
            }

            // Homing
            else if ((data[(int)MCURegPos.firstWordAzimuth] == (ushort)RadioTelescopeDirectionEnum.ClockwiseHoming || 
                        data[(int)MCURegPos.firstWordAzimuth] == (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming) && 
                        (data[(int)MCURegPos.firstWordElevation] == (ushort)RadioTelescopeDirectionEnum.ClockwiseHoming ||
                        data[(int)MCURegPos.firstWordElevation] == (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming))
            {
                // If we're homing, set the position to 0 and say we are At_Home
                currentAZ = 0;
                currentEL = 0;

                // Update position registers
                move(0, 0);

                // Update At_Home bit
                MCU_Modbusserver.DataStore.HoldingRegisters[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW + 1] |= 1 << (int)MCUConstants.MCUStatusBitsMSW.At_Home;
            }
            else {
                //logger.Info("SIMULATION CONTROL PANEL: Invalid telescope movement command");
            }
            return false;
        }


        private bool handleTestCMD( ushort[] data ) {
            string outstr = " inreg";
            for(int v = 0; v < data.Length; v++) {
                outstr += Convert.ToString( data[v] , 16 ).PadLeft( 5 ) + ",";
            }
            Console.WriteLine(outstr);
            if (data[1] == 0x0403)//move cmd
            {
                distAZ = (data[6] << 16) + data[7];
                distEL = (data[12] << 16) + data[13];
                Console.WriteLine( "AZ_22 {0,16} EL_22 {1,16}" , (MCU_Modbusserver.DataStore.HoldingRegisters[3] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[4] , (MCU_Modbusserver.DataStore.HoldingRegisters[13] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[14] );

                move( distAZ , distEL );
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] | 0x0080);
                MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] | 0x0080);

                Console.WriteLine( "AZ_finni1 {0,10} EL_finni1 {1,10}" , (MCU_Modbusserver.DataStore.HoldingRegisters[3] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[4] , (MCU_Modbusserver.DataStore.HoldingRegisters[13] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[14] );

                return true;
            } else if(data[0] == 0x0002 || data[0] == 0x0002) {//move cmd
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] & 0xff7f);
                distAZ = (data[2] << 16) + data[3];
                distEL = (data[12] << 16) + data[13];

                move( distAZ , distEL );
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] | 0x0080);
                MCU_Modbusserver.DataStore.HoldingRegisters[11] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[11] | 0x0080);

                return true;
            }

            // Homing
            else if ((data[(int)MCURegPos.firstWordAzimuth] == (ushort)RadioTelescopeDirectionEnum.ClockwiseHoming ||
                        data[(int)MCURegPos.firstWordAzimuth] == (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming) &&
                        (data[(int)MCURegPos.firstWordElevation] == (ushort)RadioTelescopeDirectionEnum.ClockwiseHoming ||
                        data[(int)MCURegPos.firstWordElevation] == (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming))
            {
                // If we're homing, set the position to 0 and say we are At_Home
                currentAZ = 0;
                currentEL = 0;

                // Update position registers
                move(0, 0);

                // Update At_Home bit
                MCU_Modbusserver.DataStore.HoldingRegisters[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW + 1] |= 1 << (int)MCUConstants.MCUStatusBitsMSW.At_Home;
            }
            return false;
        }

        private ushort[] Copy_modbus_registers( int start_index , int length ) {
            ushort[] data = new ushort[length];
            for(int i = 0; i < length; i++) {
                data[i] = MCU_Modbusserver.DataStore.HoldingRegisters[i + start_index];
            }
            return data;
        }
        
        private ushort BoolToInt(bool i ) {
            /*
            if(!i) {
                return 0;
            } else return 1;
            */
            return Convert.ToUInt16(i);
        }

    }
}

