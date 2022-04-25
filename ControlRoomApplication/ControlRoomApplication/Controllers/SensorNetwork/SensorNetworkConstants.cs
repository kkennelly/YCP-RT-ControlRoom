using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// This will contain all of the various constants that the SensorNetwork components need
    /// to use. These values may contain data conversions, simulation intervals, and so on.
    /// </summary>
    public sealed class SensorNetworkConstants
    {

        /// <summary>
        /// This is a constant needed for converting the raw azimuth data received from the
        /// absolute azimuth encoder to degrees, which the user will see.
        /// This is based on the resolution of the digital data that the encoder gives. 
        /// The angle is related from 0-360 in digital counts from 0-2047.
        /// </summary>
        public const double AzimuthEncoderScaling = 2047;
        
        /// <summary>
        /// This is the default approximate interval at which the SensorNetworkServer expects to receive
        /// data from the Sensor Network. Because the transfer isn't perfect, there will be an additional
        /// ~50ms delay. For example, a 250ms interval will yield about a 300ms receive interval.
        /// This is also used in the SensorNetworkSimulation.
        /// </summary>
        public const int DefaultDataSendingInterval = 250; // in milliseconds
        
        /// <summary>
        /// The default interval at which temperature data will be sampled on the Sensor Network.
        /// </summary>
        public const int DefaultTemperatureReadingInterval = 1000; // in milliseconds

        /// <summary>
        /// The default interval at which the absolute encoders will be sampled and sent from the Sensor Network.
        /// </summary>
        public const int DefaultEncoderSendingInterval = 20; // in milliseconds

        /// <summary>
        /// The default interval at which the timer will interrupt and increment the Sensor Network counters.
        /// </summary>
        public const int DefaultTimerInterruptInterval = 1; // in milliseconds

        /// <summary>
        /// This is the default data retrieval timeout when the user has run the telescope for the first time,
        /// and a new SensorNetworkConfig is created.
        /// </summary>
        public const int DefaultDataRetrievalTimeout = 1000; // in milliseconds

        /// <summary>
        /// This is the default initialization timeout when the user has run the telescope for the first time,
        /// and a new SensorNetworkConfig is created.
        /// </summary>
        public const int DefaultInitializationTimeout = 10000; // in milliseconds

        /// <summary>
        /// This is the total number of bytes used for the initialization packet. 
        /// </summary>
        public const int InitPacketSize = 45;

        /// <summary>
        /// If we receive this ID from the sensor network, it means that everything is going well, and we are about
        /// to get a nice load of sensor data. The number doesn't come from anything, it's just the number we chose.
        /// </summary>
        public const int TransitIdSuccess = 129;

        /// <summary>
        /// This is the max packet size the Sensor Network is able to send. We aren't really sure why it cuts off
        /// at 2048, but for this reason, this is the size that we will create our main "monitor" byte array.
        /// </summary>
        public const int MaxPacketSize = 2048; // in bytes

        /// <summary>
        /// This is how long it takes for the sensor network to time out and reboot. Basically, if it loses connection
        /// to the SensorNetworkServer for that amount of ms, it will trigger a reboot. This is useful to us for when we
        /// want to tell it to restart.
        /// </summary>
        public const int WatchDogTimeout = 1500; // in milliseconds

        /// <summary>
        /// This is where our simulation CSV files are located. These files can be swapped out with each other.
        /// </summary>
        public const string SimCSVDirectory = "../../Controllers/SensorNetwork/Simulation/SimulationCSVData/";

        /// <summary>
        /// The default sampling frequency to be used by our accelerometers.
        /// </summary>
        public const short DefaultAccelSamplingFrequency = 800; // in Hz

        /// <summary>
        /// The default FIFO size to be used by our accelerometers.
        /// </summary>
        public const byte DefaultAccelFIFOSize = 32;

        /// <summary>
        /// The default G-range to be used by our accelerometers.
        /// </summary>
        public const byte DefaultAccelGRange = 16; // in G's

        /// <summary>
        /// The default x-axis offset to apply to our accelerometers.
        /// </summary>
        public const byte DefaultAccelXOffset = 0;

        /// <summary>
        /// The default y-axis offset to apply to our accelerometers.
        /// </summary>
        public const byte DefaultAccelYOffset = 0;

        /// <summary>
        /// The default z-axis offset to apply to our accelerometers.
        /// </summary>
        public const byte DefaultAccelZOffset = 0;

        /// <summary>
        /// The default Full Bit Resolution setting to be used by our accelerometers.
        /// </summary>
        public const bool DefaultAccelFullBitResolution = true;

        /// <summary>
        /// The maximum offset that can be applied to the accelerometers. The accelerometer offsets are 8-bit signed registers
        /// </summary>
        public const int MaxAccelOffset = 127;

        /// <summary>
        /// The minimum offset that can be applied to the accelerometers. The accelerometer offsets are 8-bit signed registers
        /// </summary>
        public const int MinAccelOffset = -128;

        /// <summary>
        /// This is the FIFO size used by the azimuth motor accelerometer
        /// </summary>
        public const int AzAccelFIFOSize = 32;

        /// <summary>
        /// This is the FIFO size used by the elevation motor accelerometer
        /// </summary>
        public const int ElAccelFIFOSize = 32;

        /// <summary>
        /// This is the FIFO size used by the counterbalance accelerometer
        /// </summary>
        public const int CbAccelFIFOSize = 32;

        /// <summary>
        /// This is the degrees offset manually applied to the counterbalance accelerometer position used for greater precision.
        /// </summary>
        public const double CBAccelPositionOffset = 2;
    }
}
