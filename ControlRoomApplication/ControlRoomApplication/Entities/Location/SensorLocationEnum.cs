using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities {
    public enum SensorLocationEnum : int {
        UNDEFINED,
        AZ_MOTOR,
        EL_MOTOR,
        COUNTERBALANCE,
        MICRO_CTRL,
        MICRO_CTRL_BOX,
        EL_FRAME
    }

    public class SensorLocationEnumTypeConversionHelper {
        /// <summary>
        /// if input is not in SensorLocationEnum returns undefined
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static SensorLocationEnum FromInt( int input ) {
            if(!Enum.IsDefined( typeof( SensorLocationEnum ) , input )) {
                return SensorLocationEnum.UNDEFINED;
            } else {
                return (SensorLocationEnum)input;
            }
        }
    }
}
