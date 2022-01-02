using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications.Enumerations
{
    /// <summary>
    /// This enumeration is used to descriptively describe any and all errors the control room
    /// might encounter when parsing remote listener strings. To be used ONLY IN REMOTELISTENER for now.
    /// </summary>
    public enum ParseTCPCommandResultEnum
    {
        /// <summary>
        /// The command was parsed correctly
        /// </summary>
        Success,

        /// <summary>
        /// The version number was not found or is invalid
        /// </summary>
        InvalidVersion,

        /// <summary>
        /// The command type (Coordinate move, script, relative move, all stop, etc.) was not found or invalid
        /// </summary>
        InvalidCommandType,

        /// <summary>
        /// The command type was valid, but the arguments passed were not.
        /// E.g. AZ: 30 EL: 120 --> EL is out of range
        /// E.g. SENSOR_OVERRIDE: GATE_THAT_DOESNT_EXIST
        /// </summary>
        InvalidCommandArgs,

        /// <summary>
        /// The command type was valid, but did not receive required arguments
        /// E.g. AZ: 30 (no EL) --> EL is not given
        /// E.g. SENSOR_OVERRIDE: (nothing)
        /// </summary>
        MissingCommandArgs,

        /// <summary>
        /// The script name requested was not found or is invalid
        /// </summary>
        InvalidScript,

        /// <summary>
        /// The request type was not found
        /// </summary>
        InvalidRequestType

    }
}
