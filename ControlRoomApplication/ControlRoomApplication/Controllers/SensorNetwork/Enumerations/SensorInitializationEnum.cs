using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// Contains sensor initialization that is used to determine what sensor is at what index
    /// in the byte array
    /// </summary>
    public enum SensorInitializationEnum
    {
        /// <summary>
        /// Elevation temperature initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        ElevationTemp,

        /// <summary>
        /// Azimuth temperature initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        AzimuthTemp,

        /// <summary>
        /// Elevation encoder initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        ElevationEncoder,

        /// <summary>
        /// Azimuth encoder initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        AzimuthEncoder,
        
        /// <summary>
        /// Azimuth accelerometer initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        AzimuthAccelerometer,

        /// <summary>
        /// Elevation accelerometer initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        ElevationAccelerometer,

        /// <summary>
        /// Counterbalance accelerometer initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        CounterbalanceAccelerometer,

        /// <summary>
        /// Ambient temperature and humidity initialization. This corresponds to the index of this sensor in the
        /// sensor initialization byte array.
        /// </summary>
        AmbientTempHumidity
    }
}
