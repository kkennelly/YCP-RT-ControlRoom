using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications.Enumerations;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.Communications
{
    /// <summary>
    /// This is the object returned as a result of the ParseRLString() function inside of RemoteListener.
    /// A command comes in as a string inside of RemoteListener and gets passed into the ParseRLString.
    /// This object is the result of attempting to parse said command; a ParseTCPCommunicationResult enum is set indicating if the parse
    /// was successful, or what error occured. On error, the errMessage string is set to provide a descriptive error back to the mobile app or control room.
    /// 
    /// 
    /// </summary>
    public class ParseTCPCommandResult
    {
        public ParseTCPCommandResultEnum parseTCPCommandResultEnum { get; set; }
        public String errorMessage { get; set; }
        public String[] parsedString { get; set; }

        // Optional ErrMessage param that can be set if an error occurred, This will allow descriptive communication between CR and Mobile
        public ParseTCPCommandResult(ParseTCPCommandResultEnum parseResultEnum, String[] parsedStringArr, String errMessage = null)
        {
            parseTCPCommandResultEnum = parseResultEnum;
            parsedString = parsedStringArr;
            errorMessage = errMessage;

        }
    }
}
