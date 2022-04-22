using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.PLC_MCU;
using static ControlRoomApplication.Constants.MCUConstants;

namespace ControlRoomApplication.Controllers
{
    public class SimulationPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Simulation_control_pannel SimMCU;
        public ProductionPLCDriver driver; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        /// <param name="startPLC"></param>
        /// <param name="is_Test">if this is true the sim telescope will teleport to its final position</param>
        public SimulationPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port, bool startPLC,bool is_Test ) : base(local_ip, MCU_ip, MCU_port, PLC_port )
        {
            SimMCU = new Simulation_control_pannel(local_ip, MCU_ip, MCU_port, PLC_port, is_Test);
            Thread.Sleep(1000);//wait for server in simMcu to come up
            driver = new ProductionPLCDriver(local_ip, MCU_ip, MCU_port, PLC_port);
            if(startPLC) {
                driver.StartAsyncAcceptingClients();
            }
            SimMCU.startPLC();
            driver.set_is_test( is_Test );
        }

        public override MovementPriority CurrentMovementPriority { get { return driver.CurrentMovementPriority; } set { driver.CurrentMovementPriority = value; } }

        public override bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            return driver.RequestStopAsyncAcceptingClientsAndJoin();
        }

        public override bool StartAsyncAcceptingClients()
        {
            return driver.StartAsyncAcceptingClients();
        }

        public override void HandleClientManagementThread()
        {
            driver.HandleClientManagementThread();
        }

        public override void Bring_down()
        {
            SimMCU.Bring_down();
            driver.Bring_down();
        }

        public override bool Test_Connection()
        {
            return driver.Test_Connection();
        }

        public override Orientation GetMotorEncoderPosition()
        {
            return driver.GetMotorEncoderPosition();
        }

        public override bool Cancel_move()
        {
            return driver.Cancel_move();
        }

        public override bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return driver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation);
        }

        public override bool ControlledStop()
        {
            return driver.ControlledStop();
        }

        public override bool ImmediateStop()
        {
            return driver.ImmediateStop();
        }

        public override MovementResult RelativeMove(int programmedPeakSpeedAZInt, int programmedPeakSpeedELInt,  int positionTranslationAZ, int positionTranslationEL, Orientation targetOrientation)
        {
            return driver.RelativeMove(programmedPeakSpeedAZInt, programmedPeakSpeedELInt, positionTranslationAZ, positionTranslationEL, targetOrientation);
        }

        public override MovementResult MoveToOrientation(Orientation target_orientation, Orientation current_orientation)
        {
            return driver.MoveToOrientation(target_orientation, current_orientation);
        }

        public override MovementResult StartBothAxesJog(double azSpeed, RadioTelescopeDirectionEnum azDirection, double elSpeed, RadioTelescopeDirectionEnum elDirection) {
            if (elDirection == RadioTelescopeDirectionEnum.ClockwiseOrNegative)
            {
                elDirection = RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;
            }
            else if (elDirection == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive)
            {
                elDirection = RadioTelescopeDirectionEnum.ClockwiseOrNegative;
            }

            return driver.StartBothAxesJog(azSpeed, azDirection, elSpeed, elDirection);
        }

        public override bool Get_interlock_status()
        {
            return driver.Get_interlock_status();
        }

        public override bool[] GET_MCU_Status( RadioTelescopeAxisEnum axis )
        {
            return driver.GET_MCU_Status( axis );
        }

        public override bool TestIfComponentIsAlive() {
            return driver.TestIfComponentIsAlive();
        }

        public override MovementResult HomeTelescope() {
            return driver.HomeTelescope();
        }

        public override void setregvalue(ushort adr, ushort value)
        {
            driver.setregvalue(adr, value);
        }

        public override ushort getregvalue(ushort adr)
        {
            return driver.getregvalue(adr);
        }

        public override void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            driver.setTelescopeType(type);
        }

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// </summary>
        public override void ResetMCUErrors()
        {
            driver.ResetMCUErrors();
        }

        /// <summary>
        /// This will check for any errors present in the MCU's registers.
        /// </summary>
        /// <returns>A list of errors present in the MCU's registers</returns>
        public override List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors()
        {
            return driver.CheckMCUErrors();
        }

        public override bool InterruptMovementAndWaitUntilStopped(bool isCriticalMovementInterrupt = false, bool isSoftwareStopInterrupt = false)
        {
            return driver.InterruptMovementAndWaitUntilStopped(isCriticalMovementInterrupt, isSoftwareStopInterrupt);
        }

        public override bool MotorsCurrentlyMoving(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            return driver.MotorsCurrentlyMoving(axis);
        }

        public override void SetFinalOffset(Orientation finalPos)
        {
            driver.SetFinalOffset(finalPos);
        }

        public override RadioTelescopeDirectionEnum GetRadioTelescopeDirectionEnum(RadioTelescopeAxisEnum axis)
        {
                return driver.GetRadioTelescopeDirectionEnum(axis);
        }
    }
}
