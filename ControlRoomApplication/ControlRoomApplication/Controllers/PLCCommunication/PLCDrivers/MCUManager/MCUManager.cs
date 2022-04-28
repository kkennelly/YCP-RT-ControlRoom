using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Configuration;
using ControlRoomApplication.Simulators.Hardware;
using ControlRoomApplication.Util;
using Modbus.Device;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ControlRoomApplication.Constants.MCUConstants;

namespace ControlRoomApplication.Controllers {
    public class MCUManager {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        public bool MovementInterruptFlag = false;
        public bool CriticalMovementInterruptFlag = false;
        public bool SoftwareStopInterruptFlag = false;

        private long MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Thread HeartbeatMonitorThread;
        private bool HeartbeatMonitorRunning;
        private int AZStartSpeed = 0;
        private int ELStartSpeed = 0;
        public ModbusIpMaster MCUModbusMaster;
        private TcpClient MCUTCPClient;
        private MCUConfigurationAxys Current_AZConfiguration;
        private MCUConfigurationAxys Current_ELConfiguration;
        /// <summary>
        /// this value should not be changed from outside the MCU class
        /// </summary>
        private MCUCommand RunningCommand= new MCUCommand(new ushort[20], MCUCommandType.EmptyData) { completed = true };
        private MCUCommand PreviousCommand = new MCUCommand(new ushort[20], MCUCommandType.EmptyData) { completed = true };
        public Orientation FinalPositionOffset { get; set; }

        private int McuPort;
        private string McuIp;
        private RadioTelescopeTypeEnum telescopeType;

        public MCUManager(string ip, int port) {
            McuPort = port;
            McuIp = ip;

            logger.Info(Utilities.GetTimeStamp() + ": Attempting to connect to the MCU...");
            ConnectToModbusServer();

            HeartbeatMonitorThread = new Thread(new ThreadStart(HeartbeatMonitor)) { Name = "MCU Heartbeat Monitor Thread" };
            FinalPositionOffset = new Orientation(0, 0);
        }

        /// <summary>
        /// Attempts to connect to the MCU either for the first time, or after the connection has been lost.
        /// </summary>
        /// <returns>Result of whether or not the connection was successful.</returns>
        private bool ConnectToModbusServer() {

            // Attempt to connect to the MCU's TCP server
            try
            {
                MCUTCPClient = new TcpClient(McuIp, McuPort);
            }
            catch
            {
                return false;
            }

            try
            {
                MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
            }
            catch
            {
                return false;
            }

            logger.Info(Utilities.GetTimeStamp() + ": Successfully connected to the MCU and the Modbus Master!");

            return true;
        }

        /// <summary>
        /// Reads registers from the MCU without writing. This is public so the PLC can also read registers if needed.
        /// </summary>
        /// <param name="address">The starting address that we are reading.</param>
        /// <param name="length">The number of addresses to read.</param>
        /// <returns>Registers that were requested.</returns>
        public ushort[] ReadMCURegisters(ushort address, ushort length)
        {
            ushort[] value = new ushort[length];
            try
            {
                value = MCUModbusMaster.ReadHoldingRegistersAsync(address, length).GetAwaiter().GetResult();
            }

            // This may happen if we lose connection to the MCU
            catch (InvalidOperationException)
            {
                value = new ushort[50];
            }

            return value;
        }
        
        /// <summary>
        /// Writes data to the MCU registers based on what motor axis we want to send information to.
        /// </summary>
        /// <remarks>
        /// If elevation or azimuth the selected axis type, the ushort size should only be 10.
        /// If "both" are selected, then the ushort size should be 20. This is the default selection.
        /// If elevation is selected, then an additional offset is required to skip the azimuth registers, which
        /// is automatically handled in this function.
        /// </remarks>
        /// <param name="data">The command data we want to send.</param>
        /// <param name="axis">The motor axis registers we want to modify.</param>
        /// <returns>Whether or not the writing was successful.</returns>
        private bool WriteMCURegisters(ushort[] data, RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            // Offset used to skip azimuth registers if we're only writing to elevation
            ushort offset = 0;
            if (axis == RadioTelescopeAxisEnum.ELEVATION) offset = 10;

            try
            {
                MCUModbusMaster.WriteMultipleRegistersAsync((ushort)(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + offset), data).GetAwaiter().GetResult();
            }
            catch (InvalidOperationException) {
                
                logger.Error(Utilities.GetTimeStamp() + ": The MCU failed to receive the command, and is either offline or the connection has been terminated.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// This thread will read the heartbeat bit in the MCU status to determine if the MCU is still alive.
        /// This loops around every 250 ms.
        /// </summary>
        private void HeartbeatMonitor() {

            int lastHeartBeat = 0;
            DateTime lastConnectAttempt = DateTime.Now;
            bool inError = false;

            while(HeartbeatMonitorRunning) {

                ushort[] networkStatus = ReadMCURegisters((ushort)MCUConstants.MCUOutputRegs.NetworkConnectivity, 1);

                // If the network status length is 50, it means the network has disconnected, and we must attempt to reconnect
                if (networkStatus.Length == 50)
                {
                    // Only try to connect if it's been more than 5 seconds since the last attempt
                    if ((DateTime.Now - lastConnectAttempt) > TimeSpan.FromSeconds(5))
                    {
                        if (!inError)
                        {
                            logger.Error(Utilities.GetTimeStamp() + ": The MCU failed to retrieve the register data, and is either offline or the connection has been terminated.");
                            inError = true;
                            logger.Info(Utilities.GetTimeStamp() + ": Attempting to connect to the MCU...");
                        }
                        ConnectToModbusServer();
                        lastConnectAttempt = DateTime.Now;
                    }
                }
                else
                {
                    // This heartbeat bit flips between 0 and 1 every 500ms to ensure that the MCU is still alive
                    // and doing MCU stuff
                    int currentHeartBeat = (networkStatus[0] >> (ushort)MCUNetworkStatus.MCUHeartBeat) & 1;
                    if (currentHeartBeat != lastHeartBeat)
                    {
                        MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        lastHeartBeat = currentHeartBeat;
                    }

                    // This bit is used to tell when the control room has reconnected to the MCU after being
                    // disconnected
                    if (((networkStatus[0] >> (ushort)MCUNetworkStatus.MCUNetworkDisconnected) & 1) == 1)
                    {
                        inError = false;
                    }
                }

                Task.Delay(250).Wait();
            }
        }

        /// <summary>
        /// Starts the MCU heartbeat monitor thread.
        /// </summary>
        /// <returns>Boolean denoting whether the thread was started successfully or not.</returns>
        public bool StartAsyncAcceptingClients() {
            HeartbeatMonitorRunning = true;

            try
            {
                HeartbeatMonitorThread.Start();
            }
            catch (Exception e)
            {
                if((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    logger.Error(Utilities.GetTimeStamp() + ": Failed to start the MCU heartbeat monitor thread. Please restart the Control Room software.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// kills the MCU monitor thread
        /// </summary>
        /// <returns></returns>
        public bool RequestStopAsyncAcceptingClientsAndJoin() {
            HeartbeatMonitorRunning = false;
            try {
                HeartbeatMonitorThread.Join();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is ThreadStartException)) {
                    logger.Error(Utilities.GetTimeStamp() + ": " + e);
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        /// <summary>
        /// Reads the position from the motor encoders.
        /// </summary>
        /// <returns></returns>
        public Orientation GetMotorEncoderPosition() {

            ushort[] data = ReadMCURegisters(0, 16);

            int azMotorEncoderTicks = (int)Math.Round(((data[(ushort)MCUOutputRegs.AZ_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUOutputRegs.AZ_MTR_Encoder_Pos_LSW]) / MCUConstants.AZIMUTH_DISCREPANCY_SCALING_FACTOR);
            int elMotorEncoderTicks = -((data[(ushort)MCUOutputRegs.EL_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUOutputRegs.EL_MTR_Encoder_Pos_LSW]);

            // If the telescope type is SLIP_RING, we want to normalize the azimuth orientation
            if (telescopeType == RadioTelescopeTypeEnum.SLIP_RING)
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees_Encoder_Normalized(azMotorEncoderTicks, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees_Encoder(elMotorEncoderTicks, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
            else
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees_Encoder(azMotorEncoderTicks, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees_Encoder(elMotorEncoderTicks, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
        }

        /// <summary>
        /// gets the position from MCU step count, thsi should be compaired with the value from <see cref="GetMotorEncoderPosition"/>
        /// </summary>
        /// <remarks>
        /// the MCU traks 2 values for position one that comes from the encoder mounted on the motors shaft, 
        /// the other comes from the MCU keeping track of howmany steps it has told the motor to take
        /// if these numbers get out of sync that means the motors are missing steps and are experiencing a high load,
        /// there are 8_000 encoder counts per revolution of the motor and 20_000 steeps from the MCU so the exact numbers wont match up 
        /// also there will always be some play in the position that the encoder reports +-10 counts should be within acceptabele limts
        /// </remarks>
        /// <returns></returns>
        public Orientation GetMotorStepsPosition() {
            ushort[] data = ReadMCURegisters(0, 16);

            int azSteps = (data[(ushort)MCUOutputRegs.AZ_Current_Position_MSW] << 16) + data[(ushort)MCUOutputRegs.AZ_Current_Position_LSW];
            int elSteps = -((data[(ushort)MCUOutputRegs.EL_Current_Position_MSW] << 16) + data[(ushort)MCUOutputRegs.EL_Current_Position_LSW]);

            // If the telescope type is SLIP_RING, we want to normalize the azimuth orientation
            if (telescopeType == RadioTelescopeTypeEnum.SLIP_RING)
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees_Normalized(azSteps, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees(elSteps, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
            else
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees(azSteps, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees(elSteps, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
        }

        /// <summary>
        /// clears the previos move comand from mthe PLC, only works for jog moves
        /// </summary>
        /// <returns></returns>
        public MovementResult Cancel_move() {
            MovementResult success = MovementResult.None;

            // Build command to clear both axes registers
            var cmd = new MCUCommand(MCUMessages.ClearBothAxesMove, MCUCommandType.EmptyData) { completed = false };

            // Verify that the command could be sent
            if(!SendGenericCommand( cmd )) return MovementResult.CouldNotSendCommand;

            // Determine whether stopping was a success or not
            success = WaitUntilStopped();
            cmd.completed = true;

            return success;
        }

        /// <summary>
        /// Attempts to bring the Telescope to a controlled stop, ramping down speed.
        /// Certian moves, such as homing, are unaffected by this.
        /// </summary>
        /// <returns></returns>
        public MovementResult ControlledStop() {
            MovementResult success = MovementResult.None;

            // Jog movements must be stopped differently than other movements
            if(RunningCommand.CommandType == MCUCommandType.Jog) {
                success = Cancel_move();
            } else {
                var cmd = new MCUCommand(MCUMessages.HoldMove , MCUCommandType.HoldMove) { completed = false };
                if (!SendGenericCommand(cmd)) return MovementResult.CouldNotSendCommand;
                success = WaitUntilStopped();
                cmd.completed = true;
            }
            return success;
        }

        /// <summary>
        /// Immediately stops the telescope movement with no ramp down in speed.
        /// Homing is unaffected by this command.
        /// </summary>
        /// <returns></returns>
        public MovementResult ImmediateStop() {
            //if (!SendGenericCommand(new MCUCommand(MCUMessages.ImmediateStop, MCUCommandType.ImmediateStop) { completed = true }))
            //    return MovementResult.CouldNotSendCommand;

            //return MovementResult.Success;

            // Use a controlled stop because the immediate stop command stops too immediately and could damage the gearbox
            return ControlledStop();
        }

        // This only resets command errors
        private void CheckForAndResetCommandErrors() {

            // Registers 0 and 11 are the most significant words fo the azimuth and elevation statuses
            var data = ReadMCURegisters(0, 11);

            bool azCmdErr = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;
            bool elCmdErr = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;

            if(elCmdErr || azCmdErr) {
                ResetMCUErrors();
            }
        }

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// </summary>
        public void ResetMCUErrors()
        {
            SendGenericCommand(new MCUCommand(MCUMessages.ResetErrors, MCUCommandType.ClearErrors) { completed = true });
        }

        /// <summary>
        /// Checks the MCU registers for any errors
        /// </summary>
        /// <returns>Returns all errors that were found.</returns>
        public List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors()
        {
            var data = ReadMCURegisters(0, 12);

            // We will be storing the values in pairs so we can see which status bit has which error
            List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> errors = new List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>>();

            // Loop through every MCU output register and check for errors
            for(int i = 0; i <= 11; i++)
            {
                // If the register contains an error, add it to the output list
                // We are only interested in output registers 0, 1, 10 and 11, which contain the
                // azimuth and elevation motor errors

                // Home invalid errors
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Home_Invalid_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Home_Invalid_Error));

                // Profile invalid errors
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Profile_Invalid) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Profile_Invalid));

                // Input error
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Input_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Input_Error));

                // Command errors
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Command_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Command_Error));

                // Configuration error
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Configuration_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Configuration_Error));
            }

            return errors;
        }

        /// <summary>
        /// This function assumes that you have already told both axes to stop moving, otherwise it will time out.
        /// </summary>
        /// <returns>False if the telescope is still moving at the end of the timeout.</returns>
        private MovementResult WaitUntilStopped()
        {
            int mS_To_Decelerate = Math.Abs(estimateStopTime(PreviousCommand));
            var timout = new CancellationTokenSource( mS_To_Decelerate ).Token;
            while(!timout.IsCancellationRequested) {
                Task.Delay( 33 ).Wait();
                if(!MotorsCurrentlyMoving()) {
                    return MovementResult.Success;
                }
            }
            return MovementResult.TimedOut;

        }

        private int estimateStopTime( MCUCommand CMD ) {
            int mS_To_DecelerateAZ = (int)1.25 * (CMD.AzimuthSpeed - AZStartSpeed) / CMD.AZ_ACC;
            int mS_To_DecelerateEL = (int)1.25 * (CMD.ElevationSpeed - AZStartSpeed) / CMD.EL_ACC;
            int mS_To_Decelerate;
            if(mS_To_DecelerateAZ > mS_To_DecelerateEL) {
                mS_To_Decelerate = mS_To_DecelerateAZ;
            } else {
                mS_To_Decelerate = mS_To_DecelerateEL;
            }
            return mS_To_Decelerate;
        }

        private Orientation estimateDistanceToStop( MCUCommand CMD ) {
            int StepsAZ = 0, StepsEL = 0;
            if(CMD.AzimuthSpeed > 0) {
                int mS_To_DecelerateAZ = (int)((1.25 * (CMD.AzimuthSpeed - AZStartSpeed) / (double)CMD.AZ_ACC) / 1000.0);
                StepsAZ = (int)(mS_To_DecelerateAZ * ((CMD.AzimuthSpeed + ConversionHelper.RPMToSPS( Current_AZConfiguration.StartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH )) / 2.0));
                StepsAZ += (int)(CMD.AzimuthSpeed * 0.25);//add 100 ms worth of steps
                if(CMD.AzimuthDirection == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive) {
                    StepsAZ = -StepsAZ;
                }
            }
            if(CMD.ElevationSpeed > 0) {
                int mS_To_DecelerateEL = (int)((1.25 * (CMD.ElevationSpeed - AZStartSpeed) / (double)CMD.EL_ACC) / 1000.0);
                StepsEL = (int)(mS_To_DecelerateEL * ((CMD.ElevationSpeed + ConversionHelper.RPMToSPS( Current_ELConfiguration.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION )) / 2.0));
                StepsEL += (int)(CMD.ElevationSpeed * 0.25);//add 100 ms worth of steps
                if(CMD.ElevationDirection == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive) {
                    StepsEL = -StepsEL;
                }
            }
            return new Orientation( ConversionHelper.StepsToDegrees( StepsAZ , MotorConstants.GEARING_RATIO_AZIMUTH ) , ConversionHelper.StepsToDegrees( StepsEL , MotorConstants.GEARING_RATIO_ELEVATION ) );
        }

        /// <summary>
        /// This function assumes that you have already told both Axes to stop moving, otherwise it will time out.
        /// </summary>
        /// <returns>False if the telescope was still running at the end of the timeout</returns>
        private bool WaitUntilStoppedPerAxis(RadioTelescopeAxisEnum axis) {

            int msToDecelerate = 0;

            // Calculate timeout based on the axis
            if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                msToDecelerate = (int)1.25 * (PreviousCommand.AzimuthSpeed - AZStartSpeed) / PreviousCommand.AZ_ACC;
            } else {
                msToDecelerate = (int)1.25 * (PreviousCommand.ElevationSpeed - ELStartSpeed) / PreviousCommand.EL_ACC;
            }

            CancellationToken timeout = new CancellationTokenSource(msToDecelerate).Token;

            // Wait for the axis to stop. If it takes longer than the estimated time to decelerate, then
            // the movement failed and we return false.
            try
            {
                while (!timeout.IsCancellationRequested)
                {
                    if (!MotorsCurrentlyMoving(axis))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;

        }

        /// <summary>
        /// Tells us if the motors are currently moving. This will automatically return if both motors are moving
        /// if no parameter is passed through.
        /// </summary>
        /// <remarks>
        /// This code could, technically, be shortened so we read all the registers in the beginning and decide
        /// what to do with them in the switch, but my idea here was to only read the registers we need to ensure
        /// the fastest possible execution time.
        /// </remarks>
        /// <param name="axis">The axis we want to check is moving.</param>
        /// <returns>True if they are moving clockwise or counter-clockwise, false if they are still.</returns>
        public bool MotorsCurrentlyMoving(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH) {

            bool isMoving = false;
            ushort[] data;

            switch(axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    // Only read the registers we need
                    data = ReadMCURegisters(0, 1);

                    // Check if the azimuth motor is spinning clockwise or counter-clockwise
                    isMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);
                    break;

                case RadioTelescopeAxisEnum.ELEVATION:
                    // Only read the registers we need
                    data = ReadMCURegisters(0, 11);
                    isMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);
                    break;

                case RadioTelescopeAxisEnum.BOTH:
                    // Now we need to capture some more registers to get both az and el
                    data = ReadMCURegisters(0, 11);
                    
                    bool azMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);

                    bool elMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);

                    // Check if azimuth or elevation are moving
                    isMoving = azMoving || elMoving;

                    break;
            }

            return isMoving;
        }

        /// <summary>
        /// Will read the MCU bits to tell whether a movement has completed or not.
        /// </summary>
        /// <param name="axis">What axis you want to know the completion status of.</param>
        /// <returns>Whether an axis' movement has completed or not.</returns>
        public bool MovementCompleted(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            bool isFinished = false;
            ushort[] data;

            switch (axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    // Only read the registers we need
                    data = ReadMCURegisters(0, 1);
                    isFinished = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                    break;

                case RadioTelescopeAxisEnum.ELEVATION:
                    // Only read the registers we need
                    data = ReadMCURegisters(10, 1);
                    isFinished = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                    break;

                case RadioTelescopeAxisEnum.BOTH:
                    data = ReadMCURegisters(0, 11);
                    bool azFinished = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                    bool elFinished = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;

                    // Check if azimuth or elevation are finished with their movements
                    isFinished = azFinished && elFinished;
                    break;
            }

            return isFinished;
        }

        public bool Configure_MCU(MCUConfigurationAxys AZconfig , MCUConfigurationAxys ELconfig) {
            Current_AZConfiguration = AZconfig;
            Current_ELConfiguration = ELconfig;
            int gearedSpeedAZ = ConversionHelper.RPMToSPS( AZconfig.StartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int gearedSpeedEL = ConversionHelper.RPMToSPS( ELconfig.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION );
            AZStartSpeed = gearedSpeedAZ;
            ELStartSpeed = gearedSpeedEL;
            TestDefaultParams( AZconfig.StartSpeed , ELconfig.StartSpeed , AZconfig.HomeTimeoutSec , ELconfig.HomeTimeoutSec );
            ushort[] data = {   MakeMcuConfMSW(AZconfig), MakeMcuConfLSW(AZconfig) , (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0,
                                MakeMcuConfMSW(ELconfig), MakeMcuConfLSW(ELconfig), (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0 };
            ImmediateStop();
            SendGenericCommand( new MCUCommand( data , MCUCommandType.Configure) { completed = true} );

            // Allow the MCU to configure
            Thread.Sleep(250);

            // If there are any errors present, return false
            if (CheckMCUErrors().Count > 0) return false;
            return true;
        }

        private ushort MakeMcuConfMSW(MCUConfigurationAxys AxysConf) {
            ushort conf = 0x8400;//first byte should be 84 for current hardware setup
            switch (AxysConf.CWinput) {
                case CW_CCW_input_use.LimitSwitch:
                    conf = (ushort)(conf | 0b000_1000);
                    break;
                case CW_CCW_input_use.EStop:
                    conf = (ushort)(conf | 0b001_0000);
                    break;
            }
            switch (AxysConf.CCWinput) {
                case CW_CCW_input_use.LimitSwitch:
                    conf = (ushort)(conf | 0b0010_0000);
                    break;
                case CW_CCW_input_use.EStop:
                    conf = (ushort)(conf | 0b0100_0000);
                    break;
            }
            switch(AxysConf.EncoderType) {
                case EncoderTyprEnum.Quadrature_Encoder:
                    conf = (ushort)(conf | 0b1_0000_0000);
                    break;
                case EncoderTyprEnum.Diagnostic_Feedback:
                    conf = (ushort)(conf | 0b10_0000_0000);
                    break;
            }
            if (AxysConf.UseHomesensors) {
                conf = (ushort)(conf | 0b0100);
            }
            if(AxysConf.UseCapture) {
                conf = (ushort)(conf | 0b0001);
            }
            return conf;
        }

        private ushort MakeMcuConfLSW(MCUConfigurationAxys AxysConf) {
            ushort conf = 0x0000;
            if(AxysConf.CaptureActive_High) {
                conf = (ushort)(conf | 0b0001);
            }
            if (AxysConf.HomeActive_High) {
                conf = (ushort)(conf | 0b0_0100);
            }
            if (AxysConf.CWactive_High) {
                conf = (ushort)(conf | 0b0_1000);
            }
            if (AxysConf.CCWactive_High) {
                conf = (ushort)(conf | 0b1_0000);
            }
            return conf;
        }

        private void TestDefaultParams(double startSpeedDPSAzimuth, double startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation) {
            int gearedSpeedAZ = ConversionHelper.DPSToSPS(startSpeedDPSAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int gearedSpeedEL = ConversionHelper.DPSToSPS(startSpeedDPSElevation, MotorConstants.GEARING_RATIO_ELEVATION);
            Console.WriteLine(gearedSpeedAZ.ToString() + " :AZ           EL:" + gearedSpeedEL.ToString());
            if ((gearedSpeedEL < 1) || (gearedSpeedEL > MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY)) {
                throw new ArgumentOutOfRangeException("startSpeedDPSElevation", startSpeedDPSElevation,
                    String.Format("startSpeedDPSElevation should be between {0} and {1}",
                    ConversionHelper.SPSToDPS(1, MotorConstants.GEARING_RATIO_ELEVATION),
                    ConversionHelper.SPSToDPS(MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY, MotorConstants.GEARING_RATIO_ELEVATION)));
            }
            if ((gearedSpeedAZ < 1) || (gearedSpeedAZ > MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY)) {
                throw new ArgumentOutOfRangeException("startSpeedDPSAzimuth", startSpeedDPSAzimuth,
                    String.Format("startSpeedDPSAzimuth should be between {0} and {1}",
                    ConversionHelper.SPSToDPS(1, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.SPSToDPS(MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY, MotorConstants.GEARING_RATIO_AZIMUTH)));
            }
            if ((homeTimeoutSecondsElevation < 0) || (homeTimeoutSecondsElevation > 300)) {
                throw new ArgumentOutOfRangeException("homeTimeoutSecondsElevation", homeTimeoutSecondsElevation,
                    String.Format("homeTimeoutSecondsElevation should be between {0} and {1}", 0, 300));
            }
            if ((homeTimeoutSecondsAzimuth < 0) || (homeTimeoutSecondsAzimuth > 300)) {
                throw new ArgumentOutOfRangeException("homeTimeoutSecondsAzimuth", homeTimeoutSecondsAzimuth,
                    String.Format("homeTimeoutSecondsAzimuth should be between {0} and {1}", 0, 300));
            }
        }

        /// <summary>
        /// Sends a home command and waits for the MCU to finish homing.
        /// </summary>
        /// <param name="RPM"></param>
        /// <returns></returns>
        public MovementResult HomeBothAxes(double RPM) {

            int EL_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_ELEVATION );
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_AZIMUTH );

            //set config word to 0x0040 to have the RT home at the minimumum speed// this requires the MCU to be configured properly
            ushort[] data = {
                // azimuth data
                (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming,
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,
                (ushort)((AZ_Speed & 0xFFFF0000)>>16),
                (ushort)(AZ_Speed & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                GetDeceleration(AZ_Speed, MotorConstants.GEARING_RATIO_AZIMUTH),
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,

                // elevation data
                (ushort)RadioTelescopeDirectionEnum.ClockwiseHoming,
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,
                (ushort)((EL_Speed & 0xFFFF0000)>>16),
                (ushort)(EL_Speed & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                GetDeceleration(EL_Speed, MotorConstants.GEARING_RATIO_ELEVATION),
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData
            };

            // Calculate which axis has the longest timeout, then set the timeout as that axis
            int timeout;
            if(Current_AZConfiguration.HomeTimeoutSec > Current_ELConfiguration.HomeTimeoutSec) {
                timeout = Current_AZConfiguration.HomeTimeoutSec;
            } else {
                timeout = Current_ELConfiguration.HomeTimeoutSec;
            }

            // Builds the MCU command and then sends it
            MCUCommand command = new MCUCommand(data, MCUCommandType.Home)
            {
                AzimuthSpeed = AZ_Speed,
                ElevationSpeed = EL_Speed,
                EL_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                AZ_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                timeout = new CancellationTokenSource((int)(timeout * 1200)) //* 1000 for seconds to ms //* 1.2 for a 20% margin 
            };

            // If sending the command fails, return because the movement cannot complete.
            if(!SendGenericCommand(command)) return MovementResult.CouldNotSendCommand;
            
            // The new orientation is 0,0 because homing should result in the motor encoders being zeroed out
            return MovementMonitor(command, new Orientation(0,0), true);
        }

        private bool BuildAndSendRelativeMove(MCUCommand command, int positionTranslationAz, int positionTranslationEl) {
            bool success = false;

            if (command.AzimuthSpeed < AZStartSpeed) {
                throw new ArgumentOutOfRangeException("SpeedAZ", command.AzimuthSpeed,
                    String.Format("Azimuth speed should be greater than {0}, which is the starting speed set when configuring the MCU", AZStartSpeed));
            }
            if (command.ElevationSpeed < ELStartSpeed) {
                throw new ArgumentOutOfRangeException("SpeedEL", command.ElevationSpeed,
                    String.Format("Elevation speed should be greater than {0}, which is the starting speed set when configuring the MCU", ELStartSpeed));
            }

            // This needs flipped so that the elevation axis moves the correct direction
            positionTranslationEl = -positionTranslationEl;
            positionTranslationAz = (int)Math.Round(-positionTranslationAz * MCUConstants.AZIMUTH_DISCREPANCY_SCALING_FACTOR);

            command.commandData = new ushort[] {
                // Azimuth data
                (ushort)MCUCommandType.RelativeMove,
                0x0003,
                (ushort)((positionTranslationAz & 0xFFFF0000)>>16),
                (ushort)(positionTranslationAz & 0xFFFF),
                (ushort)((command.AzimuthSpeed & 0xFFFF0000)>>16),
                (ushort)(command.AzimuthSpeed & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                GetDeceleration(command.AzimuthSpeed, MotorConstants.GEARING_RATIO_AZIMUTH),
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,

                // Elevation data
                (ushort)MCUCommandType.RelativeMove,
                0x0003,
                (ushort)((positionTranslationEl & 0xFFFF0000)>>16),
                (ushort)(positionTranslationEl & 0xFFFF),
                (ushort)((command.ElevationSpeed & 0xFFFF0000)>>16),
                (ushort)(command.ElevationSpeed & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                GetDeceleration(command.ElevationSpeed, MotorConstants.GEARING_RATIO_ELEVATION),
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData
            };

            RunningCommand = command;
            success = WriteMCURegisters(command.commandData);

            // Give the MCU time to process the data
            Thread.Sleep(100);

            return success;
        }

        /// <summary>
        /// perform the specified move and wait for it to be completted by the telescope
        /// </summary>
        /// <param name="SpeedAZ"></param>
        /// <param name="SpeedEL"></param>
        /// <param name="positionTranslationAZ"></param>
        /// <param name="positionTranslationEL"></param>
        /// <param name="targetOrientation"></param>
        /// <returns></returns>
        public MovementResult MoveAndWaitForCompletion(int SpeedAZ, int SpeedEL, int positionTranslationAZ, int positionTranslationEL, 
                Orientation targetOrientation) 
        {
            // Clear any leftover register data
            Cancel_move();

            // Estimate the time to move, which will be the axis that takes the longest
            int AZTime = EstimateMovementTime(SpeedAZ, positionTranslationAZ);
            int ELTime = EstimateMovementTime(SpeedEL, positionTranslationEL);
            int TimeToMove;
            if(AZTime > ELTime) {
                TimeToMove = AZTime;
            } else { TimeToMove = ELTime; }

            // Add on some extra time so the timeout doesn't end early
            TimeToMove = ( int)(TimeToMove * 1.2);
            TimeToMove += 100;

            if (targetOrientation.Azimuth == 0.0 || targetOrientation.Azimuth==360.0) TimeToMove += 100;
   

            // Calculate azimuth movement direction
            RadioTelescopeDirectionEnum azDirection;
            if (positionTranslationAZ > 0) azDirection = RadioTelescopeDirectionEnum.ClockwiseOrNegative;
            else azDirection = RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;

            // Calculate elevation movement direction
            RadioTelescopeDirectionEnum elDirection;
            if (positionTranslationEL > 0) elDirection = RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;
            else elDirection = RadioTelescopeDirectionEnum.ClockwiseOrNegative;

            // Build and send the MCU Command (the data is the only part sent to the MCU; the rest is for inner tracking)
            MCUCommand ThisMove =
                new MCUCommand(
                    new ushort[0],
                    MCUCommandType.RelativeMove,
                    azDirection,
                    elDirection,
                    SpeedAZ,
                    SpeedEL
                )
                {
                    EL_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                    AZ_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                    timeout = new CancellationTokenSource(TimeToMove)
                };

            // Build command data
            // If the command could not be sent, return
            if(!BuildAndSendRelativeMove(
                ThisMove,
                positionTranslationAZ,
                positionTranslationEL
            ))
            {
                return MovementResult.CouldNotSendCommand;
            }

            return MovementMonitor(ThisMove, targetOrientation);
        }

        /// <summary>
        /// This will loop through and monitor MCU movement as long as the following conditions are true:
        /// Cannot be timed out
        /// Interrupt flag must be false
        /// No MCU errors
        /// The movement result cannot have been set yet. As soon as it is set, the routine ends.
        /// </summary>
        /// <remarks>
        /// I don't like having "homing or not homing" in a bool, because the homing routine should technically have
        /// its own monitoring routine (as well as every other movement), but it's legacy at this point, and unless
        /// we plan on adding more movements down the road, I would say it's not worth changing.
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="targetOrientation"></param>
        /// <param name="homing">Tells us whether we are monitoring homing or a relative move</param>
        /// <returns></returns>
        private MovementResult MovementMonitor(MCUCommand command, Orientation targetOrientation, bool homing = false)
        {
            MovementResult result = MovementResult.None;
            bool completed = false;
            
            while (!command.timeout.IsCancellationRequested && !MovementInterruptFlag && CheckMCUErrors().Count == 0 && result == MovementResult.None)
            {
                // We must wait for the MCU registers to be set before trying to read them
                Thread.Sleep(100);

                // Anything but homing...
                if (!homing) completed = MovementCompleted();

                // How we tell if homing completed...
                else
                {
                    // Check the At_Home bit and see if it's true
                    ushort[] data = ReadMCURegisters(0, 1);
                    completed = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.At_Home) & 0b1) == 1;
                }

                if (completed && !MotorsCurrentlyMoving() && !MovementInterruptFlag)
                {
                    Thread.Sleep(200);
                    // If this is reached, the motors have stopped and we are now checking that the orientation is correct
                    Orientation encoderOrientation = GetMotorEncoderPosition();
                    Orientation offsetOrientation = new Orientation(encoderOrientation.Azimuth + FinalPositionOffset.Azimuth, encoderOrientation.Elevation + FinalPositionOffset.Elevation);

                    while (offsetOrientation.Azimuth > 360) offsetOrientation.Azimuth -= 360;
                    while (offsetOrientation.Azimuth < 0) offsetOrientation.Azimuth += 360;

                    // Check if target or offset orientations are close to the 360 degree line. If the orientations stradle 360 degrees (ex 359.99 and 0.01),
                    // add the offset to them to shift their positions and subtract 360 from the one that is greater so that the orientation check is
                    // accurate (ex to 0.09 and 0.19)
                    if (targetOrientation.Azimuth + 0.1 >= 360 && offsetOrientation.Azimuth - 0.1 <= 0)
                    {
                        targetOrientation.Azimuth += 0.1 - 360;
                        offsetOrientation.Azimuth += 0.1;
                    }
                    else if (offsetOrientation.Azimuth + 0.1 >= 360 && targetOrientation.Azimuth - 0.1 <= 0)
                    {
                        targetOrientation.Azimuth += 0.1;
                        offsetOrientation.Azimuth += 0.1 - 360;
                    }

                    if ((targetOrientation.Azimuth == 0.0 || targetOrientation.Azimuth==360.0) && (offsetOrientation.Azimuth > 359.9 || offsetOrientation.Azimuth < 0.1))
                    {
                        result = MovementResult.Success;
                    }

                   else if (Math.Abs(offsetOrientation.Azimuth - targetOrientation.Azimuth) <= 0.1 && Math.Abs(offsetOrientation.Elevation - targetOrientation.Elevation) <= 0.1)
                   {
                        result = MovementResult.Success;
                   }
                    else
                    {
                        result = MovementResult.IncorrectPosition;
                    }
                }
            }

            List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> errors = CheckMCUErrors();

            bool hitLimitSwitch =
                errors.Contains(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>(MCUOutputRegs.AZ_Status_Bist_MSW, MCUStatusBitsMSW.Input_Error)) ||
                errors.Contains(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>(MCUOutputRegs.EL_Status_Bist_MSW, MCUStatusBitsMSW.Input_Error));

            // If a limit switch gets hit, this is reached. We do NOT want to stop
            // the motors if this happens. All other errors entail stopping the motors.
            if (hitLimitSwitch)
            {
                result = MovementResult.LimitSwitchOrEstopHit;
                ResetMCUErrors();
            }
            else
            {
                // If the movement times out, this is reached
                if (command.timeout.IsCancellationRequested)
                {
                    result = MovementResult.TimedOut;
                }

                // If there are errors on the MCU registers, this is reached
                else if (errors.Count > 0)
                {
                    result = MovementResult.McuErrorBitSet;
                }

                // If the movement was voluntarily interrupted, this is reached
                else if (MovementInterruptFlag)
                {
                    MovementInterruptFlag = false;

                    // Return software-stops hit if they caused the interrupt
                    if (SoftwareStopInterruptFlag)
                    {
                        SoftwareStopInterruptFlag = false;
                        result = MovementResult.SoftwareStopHit;
                    }
                    else
                    {
                        result = MovementResult.Interrupted;
                    }
                }

                // A critical movement interrupt signals an immediate stop
                if (CriticalMovementInterruptFlag)
                {
                    CriticalMovementInterruptFlag = false;
                    ImmediateStop();
                }
                else
                {
                    ControlledStop();
                }
            }

            command.completed = true;

            command.timeout.Cancel();
            command.Dispose();

            return result;
        }

        /// <summary>
        /// Estimate time to complete a single motor's movement based on input values.
        /// </summary>
        /// <param name="maxVelocity">The maximum number of steps per second a motor will run.</param>
        /// <param name="acceleration"></param>
        /// <param name="distance">The total number of motor steps that a movement will take.</param>
        /// <returns>Estimated time that a movement command will take in milliseconds.</returns>
        public static int EstimateMovementTime(int maxVelocity, int distance) {
            //acc steps/millisecond/second
            distance = Math.Abs(distance);
            maxVelocity = Math.Abs(maxVelocity);

            int t1 = maxVelocity / ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING;//ms

            double t1s = t1 / 1000.0;

            int distT1 = (int)(ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING * 1000 / 2 * (t1s * t1s) * 2);

            if (distT1 < distance) {

                int t2 = (distance - distT1) / maxVelocity;

                return t2*1000 + (2 * t1);

            } else {
                return 2 * t1;
            }
        }

        /// <summary>
        /// Jogs one axis, or both axes at the same time.
        /// </summary>
        /// <param name="AZspeed"></param>
        /// <param name="azDirection"></param>
        /// <param name="ELspeed"></param>
        /// <param name="elDirection"></param>
        /// <returns></returns>
        public MovementResult SendBothAxesJog(double AZspeed, RadioTelescopeDirectionEnum azDirection, double ELspeed, RadioTelescopeDirectionEnum elDirection) {

            int AZstepSpeed = ConversionHelper.RPMToSPS( AZspeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int ELstepSpeed = ConversionHelper.RPMToSPS( ELspeed , MotorConstants.GEARING_RATIO_ELEVATION );
            ushort[] data = new ushort[10] { (ushort)azDirection , 0x0003 , 0x0 , 0x0 , (ushort)(AZstepSpeed >> 16) , (ushort)(AZstepSpeed & 0xffff) , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , GetDeceleration(AZstepSpeed, MotorConstants.GEARING_RATIO_AZIMUTH), 0x0 , 0x0 , };
            ushort[] data2 = new ushort[20];

            if(AZstepSpeed > AZStartSpeed) {
                for(int j = 0; j < data.Length; j++) {
                    data2[j] = data[j];
                }
            } else {
                AZstepSpeed = 0;
            }

            if(ELstepSpeed > ELStartSpeed) {
                for(int j = 0; j < data.Length; j++) {
                    data2[j + 10] = data[j];
                }

                data2[10] = (ushort)(elDirection);
                data2[14] = (ushort)(ELstepSpeed >> 16);
                data2[15] = (ushort)(ELstepSpeed & 0xffff);
            } else {
                ELstepSpeed = 0;
            }
            bool booth = false;
            if(RunningCommand.CommandType == MCUCommandType.Jog) {
                //if telescope is already joging changing direction requires stopping first
                ushort[] data3 = new ushort[20];
                data2.CopyTo( data3 , 0 );
                booth = (RunningCommand.AzimuthDirection != azDirection) && (RunningCommand.ElevationDirection != elDirection);
                if(booth) {//if both axis need to change direction
                    Cancel_move();
                    WaitUntilStopped();
                } else if(RunningCommand.ElevationDirection != elDirection) {//if only elevation needs to change direction
                    for(int j = 0; j <= data3.Length - 11; j++) {
                        // TODO: Replace ClearBothAxesMove with simplified code and ClearSingleAxisMove (issue #390)
                        data3[j + 10] = MCUMessages.ClearBothAxesMove[j + 10];//replace elevation portion of move with controled stop
                    }
                    if (!SendGenericCommand(new MCUCommand(data3, MCUCommandType.Jog, azDirection, elDirection, AZstepSpeed, ELstepSpeed)
                    {
                        EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                    })) return MovementResult.CouldNotSendCommand;
                    WaitUntilStoppedPerAxis(RadioTelescopeAxisEnum.AZIMUTH);
                } else if(RunningCommand.AzimuthDirection != azDirection) {//only Azimuth needs to change direction
                    for(int j = 0; j <= data3.Length - 1; j++)
                    {
                        // TODO: Replace ClearBothAxesMove with simplified code and ClearSingleAxisMove (issue #390)
                        data3[j] = MCUMessages.ClearBothAxesMove[j];//replace Azimuth portion of move with controled stop
                    }
                    if (!SendGenericCommand(new MCUCommand(data3, MCUCommandType.Jog, azDirection, elDirection, AZstepSpeed, ELstepSpeed)
                    {
                        AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                    })) return MovementResult.CouldNotSendCommand;
                    WaitUntilStoppedPerAxis(RadioTelescopeAxisEnum.ELEVATION);
                }
            }

            if (!SendGenericCommand(new MCUCommand(data2, MCUCommandType.Jog, azDirection, elDirection, AZstepSpeed, ELstepSpeed)
            {//send the portion of the jog move that was previously replaced with a controlled stop
                EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
            })) return MovementResult.CouldNotSendCommand;

            return MovementResult.Success;
        }

        private bool SendGenericCommand(MCUCommand incoming) {
            bool success = false;
            RunningCommand = incoming;
            success = WriteMCURegisters(incoming.commandData);
            return success;
        }

        /// <summary>
        /// This should only be used to back off of limit switches.
        /// For any other jog moves, use the <see cref="SendBothAxesJog"/>
        /// </summary>
        /// <param name="axis">What axis (elevation or azimuth) is spinning.</param>
        /// <param name="direction">Denotes what direction a motor will be spinning.</param>
        /// <param name="speed">What speed the motor will be spinning at.</param>
        /// <returns></returns>
        public bool SendSingleAxisJog(RadioTelescopeAxisEnum axis, RadioTelescopeDirectionEnum direction, double speed) {
            int stepSpeed;
            int gearingRatio;
            bool success = false;

            if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                stepSpeed = ConversionHelper.RPMToSPS(speed, MotorConstants.GEARING_RATIO_AZIMUTH);
                gearingRatio = MotorConstants.GEARING_RATIO_AZIMUTH;
            } else {
                stepSpeed = ConversionHelper.RPMToSPS(speed, MotorConstants.GEARING_RATIO_ELEVATION);
                gearingRatio = MotorConstants.GEARING_RATIO_ELEVATION;
            }

            ushort[] data = new ushort[10] {
                (ushort)direction,
                0x0003,
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData,
                (ushort)(stepSpeed >> 16),
                (ushort)(stepSpeed & 0xffff),
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                GetDeceleration(stepSpeed, gearingRatio),
                (ushort)MCUCommandType.EmptyData,
                (ushort)MCUCommandType.EmptyData
            };

            success = WriteMCURegisters(data, axis);

            RunningCommand.completed = false;

            return success;
        }

        /// <summary>
        /// This should only be used to stop the jog command from backing off limit switches.
        /// </summary>
        /// <param name="axis">The motor axis we are stopping.</param>
        /// <returns></returns>
        public bool StopSingleAxisJog(RadioTelescopeAxisEnum axis) {
            WriteMCURegisters(MCUMessages.ClearOneAxisMove, axis);
            RunningCommand.completed = true;
            return true;
        }

        public long getLastContact() {
            return MCU_last_contact;
        }

        /// <summary>
        /// Sets the telescope type so we know if we should be normalizing the azimuth orientation or not.
        /// </summary>
        /// <param name="type">The telescope type we are setting the MCU manager telescope type to.</param>
        public void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            telescopeType = type;
        }

        /// <summary>
        /// Compute the deceleration to be used for MCU movements. Takes in an input speed and computes the deceleration needed to stop
        /// within the target stop distance constant.
        /// </summary>
        /// <param name="inputSpeed">The speed in ticks/s the motor will be running at.</param>
        /// <param name="gearingratio">The gearing ratio used to convert the distance constant.</param>
        /// <returns>The deceleration value ticks/s/ms to slow the motor down within the stopping distance constant.</returns>
        private ushort GetDeceleration(double inputSpeed, int gearingratio)
        {
            // Time it takes to stop is t = distance/velocity
            // Acceleration is change in velocity over time: (v1-v0)/t => (v1-0)/t => v1/distance/v1 => v1*v1/distance
            ushort deceleration = (ushort)Math.Ceiling(inputSpeed * inputSpeed / ConversionHelper.DegreesToSteps(MCUConstants.TARGET_STOP_DISTANCE, gearingratio) / 1000);

            // If deceleration is less than the default, use the default because telesope will stop within target distance and won't take as long
            if (deceleration < MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING)
            {
                deceleration = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING;
            }
            // If the deceleration is greater than the allowed amount, set it to the allowed amount
            else if (deceleration > MCUConstants.ACTUAL_MCU_PEAK_ACCELERATION)
            {
                deceleration = MCUConstants.ACTUAL_MCU_PEAK_ACCELERATION;
            }

            return deceleration;
        }
    }
}
