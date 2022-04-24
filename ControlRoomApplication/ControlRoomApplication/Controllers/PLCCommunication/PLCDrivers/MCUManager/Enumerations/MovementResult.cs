using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations
{
    /// <summary>
    /// This is returned after a movement is completed, denoting whether it was successful or a failure.
    /// </summary>
    public enum MovementResult
    {
        /// <summary>
        /// Result has not yet been assigned.
        /// </summary>
        None,

        /// <summary>
        /// The movement was successful and the position is correct.
        /// </summary>
        Success,

        /// <summary>
        /// A limit switch was hit during the movement, so it was cancelled.
        /// </summary>
        LimitSwitchHit,

        /// <summary>
        /// An emergency stop was hit during the movement, so it was cancelled.
        /// </summary>
        EstopHit,

        /// <summary>
        /// A limit switch or emergency stop was hit during the movement, so it was cancelled.
        /// </summary>
        /// <remarks>
        /// This exists because the MCU error bit denoting limit switch or estop activity are shared.
        /// </remarks>
        LimitSwitchOrEstopHit,

        /// <summary>
        /// The telescope elevation was out of range and was moving furthur out of range, so it was cancelled.
        /// </summary>
        SoftwareStopHit,

        /// <summary>
        /// The movement took too long to complete, so it timed out.
        /// </summary>
        TimedOut,

        /// <summary>
        /// The movement completed, but the position is not correct.
        /// </summary>
        IncorrectPosition,

        /// <summary>
        /// The movement was interrupted by either a higher-priority movement, or sensor data requested a stop.
        /// </summary>
        Interrupted,

        /// <summary>
        /// The movement was ended early because one or more MCU error bits were set.
        /// </summary>
        McuErrorBitSet,

        /// <summary>
        /// The movement will not be sent to the MCU because a part of the command was invalid.
        /// </summary>
        ValidationError,

        /// <summary>
        /// If you try to run a movement while one with equal or higher priority is running, this will be returned.
        /// </summary>
        AlreadyMoving,

        /// <summary>
        /// If the sensors are communicating unsafe data, blocks the incoming move.
        /// </summary>
        SensorsNotSafe,

        /// <summary>
        /// This is only used for Jog commands if they are running overtop of a movement that is lower priority.
        /// </summary>
        StoppingCurrentMove,

        /// <summary>
        /// Used if the RemoteListener class failed to parse a TCP command and movement wasn't started
        /// </summary>
        InvalidCommand,

        /// This is only used for the "moveradiotelescopebyxdegrees" function in RadioTelescopeController
        /// will be returned if a relative move would put the telescope past software limits
        /// </summary>
        InvalidRequestedPostion,

        /// <summary>
        /// This is only used for the "moveradiotelescopebyxdegrees" function in RadioTelescopeController
        /// will be returned if a requested azimuth move is too large
        /// </summary>
        RequestedAzimuthMoveTooLarge,

        /// This is set if the command could not be sent to the MCU for whatever reason. The most common reason this
        /// may happen is if the MCU goes offline, Ethernet becomes disconnected, etc.
        /// </summary>
        CouldNotSendCommand,

        /// <summary>
        /// Used if the motors are not homed, thus a we cannot perform a safe movement
        /// </summary>
        MotorsNotHomed
    }
}
