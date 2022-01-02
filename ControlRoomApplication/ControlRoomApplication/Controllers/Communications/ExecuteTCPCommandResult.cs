using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.Communications.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications
{
    /// <summary>
    /// The object returned as the result of executing a command received inside of RemoteListener.
    /// The command will first be parsed (via ParseRLString() ), and then, if safe to do so, will execute.
    /// The object contains a MovementResult which specifies the resulting action of any given movement sent to the PLC.
    /// The error message string is set on error (e.g., MvmtResult!=success) and will contain a string from TCPCommunicationConstants
    /// indicating what went wrong and where.
    /// </summary>
    public class ExecuteTCPCommandResult
    {
        public MovementResult movementResult { get; set; }
        public String errorMessage { get; set; }

        // Optional ErrMessage param that can be set if an error occurred, This will allow descriptive communication between CR and Mobile
        public ExecuteTCPCommandResult(MovementResult mvmtResult, String errMessage = null)
        {
            movementResult = mvmtResult;
            errorMessage = errMessage;

        }
    }
}
