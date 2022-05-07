using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Enumerations
{
    /// <summary>
    /// Denotes what specific errors the DHT22 sensor may be having.
    /// </summary>
    public enum AmbientTempHumidityErrorCodes
    {
        /// <summary>
        /// The sensor is working properly.
        /// </summary>
        NoError,

        /// <summary>
        /// The sensor is not receiving data.
        /// </summary>
        NoData,

        /// <summary>
        /// The data recieved from the sensor is out of its readable range (data corruption most likely)
        /// </summary>
        OutOfRange
    }
}
