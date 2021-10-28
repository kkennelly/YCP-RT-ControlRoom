using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Simulators.Hardware;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Controllers.PLCCommunication;
using System.Collections.Generic;
using static ControlRoomApplication.Constants.MCUConstants;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractPLCDriver : HeartbeatInterface {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected Thread ClientManagmentThread;

        public LimitSwitchData limitSwitchData;
        public HomeSensorData homeSensorData;
        public MiscPlcInput plcInput;
        protected PLCEvents pLCEvents;
        public OverrideSwitchData Overrides { get; set; }

        /// <summary>
        /// This is the priority of the currently-running move. This will be "None" if no move is currently running, otherwise it will
        /// reflect the priority.
        /// </summary>
        public MovementPriority CurrentMovementPriority { get; set; }

        /// <summary>
        /// the PLC will look for the server that we create in the control room, the control room will look for the remote server that the MCU has setup
        /// </summary>
        /// <param name="local_ip">IP adress to start the local modbus server on</param>
        /// <param name="MCU_ip">IP adress of the MCU</param>
        /// <param name="MCU_port">port the MCU is using</param>
        /// <param name="PLC_port">port to start the local modbus server on</param>
        /// <param name="autoStartPLCThread"> if true will automaticly start the modbus server</param>
        public AbstractPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port) { }


        protected override bool KillHeartbeatComponent() {
             Bring_down();
            return true;
        }

        public bool publicKillHeartbeatComponent() {
            return KillHeartbeatComponent();
        }

        /// <summary>
        /// modbuss server implamentation specific to each device
        /// </summary>
        public abstract void HandleClientManagementThread();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool StartAsyncAcceptingClients();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool RequestStopAsyncAcceptingClientsAndJoin();


        public abstract void Bring_down();

        public abstract bool Test_Connection();

        public abstract Orientation GetMotorEncoderPosition();

        public abstract bool Cancel_move();

        // All of the "scripts" are here all the way to....
        // Control Scripts

        /// <summary>
        /// Moves both axes to where the homing sensors are. After this is run, the position offset needs applied to the motors, and then
        /// the absolute encoders.
        /// </summary>
        /// <returns>True if homing was successful, false if it failed</returns>
        public abstract MovementResult HomeTelescope();

        public abstract bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation);

        public abstract bool ControlledStop();

        public abstract bool ImmediateStop();

        public abstract MovementResult RelativeMove(int programmedPeakSpeedAZInt, int positionTranslationAZ, int positionTranslationEL, Orientation targetOrientation);

        public abstract MovementResult MoveToOrientation(Orientation target_orientation, Orientation current_orientation);

        public abstract MovementResult StartBothAxesJog(double azSpeed, RadioTelescopeDirectionEnum azDirection, double elSpeed, RadioTelescopeDirectionEnum elDirection);

        public abstract bool Get_interlock_status();

        public abstract void setregvalue(ushort adr, ushort value);

        public abstract ushort getregvalue(ushort adr);

        /// <summary>
        /// get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation 
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// see <see cref="MCUConstants.MCUStatusBitsMSW"/> for description of each bit
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public abstract bool[] GET_MCU_Status( RadioTelescopeAxisEnum axis );

        public abstract void setTelescopeType(RadioTelescopeTypeEnum type);

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// </summary>
        public abstract void ResetMCUErrors();

        /// <summary>
        /// This will check for any errors present in the MCU's registers.
        /// </summary>
        /// <returns>A list of errors present in the MCU's registers</returns>
        public abstract List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors();

        /// <summary>
        /// This will interrupt the current movement, wait until it has stopped, and then
        /// end when the movement has stopped.
        /// 
        /// If no motors are moving when this is called, then it will not wait, and just be
        /// able to pass through.
        /// </summary>
        /// <param name="isCriticalMovement">Specify whether or not this is a critical movement</param>
        public abstract bool InterruptMovementAndWaitUntilStopped(bool isCriticalMovement = false);

        /// <summary>
        /// Checks to see if the motors are currently moving.
        /// </summary>
        /// <param name="axis">Azimuth, elevation, or both.</param>
        /// <returns>True if moving, false if not moving.</returns>
        public abstract bool MotorsCurrentlyMoving(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH);

        public abstract void SetFinalOffset(Orientation finalPos);

        public abstract RadioTelescopeDirectionEnum GetRadioTelescopeDirectionEnum(RadioTelescopeAxisEnum axis);
    }
}