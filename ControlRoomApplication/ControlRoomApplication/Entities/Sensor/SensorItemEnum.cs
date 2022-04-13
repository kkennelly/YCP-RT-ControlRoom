using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities
{
    public enum SensorItemEnum
    {
        // general
        GATE,
        PROXIMITY,
        AZIMUTH_MOTOR,
        ELEVATION_MOTOR,
        WEATHER_STATION,
        EL_PROXIMITY_0,
        EL_PROXIMITY_90,

        // specific
        AZ_MOTOR_TEMP,
        ELEV_MOTOR_TEMP,
        AMBIENT_TEMP_HUMIDITY,
        AZ_MOTOR_VIBRATION,
        ELEV_MOTOR_VIBRATION,
        AZ_MOTOR_CURRENT,
        ELEV_MOTOR_CURRENT,
        COUNTER_BALANCE_VIBRATION,
        WIND,
        RAIN_AMOUNT,
        ELEVATION_ABS_ENCODER,
        AZIMUTH_ABS_ENCODER
    }
}
