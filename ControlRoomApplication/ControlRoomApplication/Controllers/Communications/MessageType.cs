using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace ControlRoomApplication.Controllers
{ 
    public enum MessageTypeEnum
    {
        [Description("Your YCAS Radio Telescope appointment has completed.")]
        APPOINTMENT_COMPLETION,
        [Description("Your YCAS Radio Telescope appointment has been cancelled.")]
        APPOINTMENT_CANCELLED,
        [Description("Your YCAS Radio Telescope appointment has started.")]
        APPOINTMENT_STARTED,
        [Description("Starting Calibration has completed successfully.")]
        START_CALIBRATION_COMPLETION,
        [Description("Ending Calibration has completed successfully/")]
        END_CALIBRATION_COMPLETION,
        [Description("Appointment Calibration has failed.")]
        CALIBRATION_FAILURE
    }

    public static class MessageTypeExtension
    {
        public static string GetDescription(MessageTypeEnum e)
        {
            FieldInfo type = e.GetType().GetField(e.ToString());
            var attrs = type.GetCustomAttributes(typeof(DescriptionAttribute), true);

            string message = ((DescriptionAttribute)attrs[0]).Description;

            return message;
        }
    }
}
