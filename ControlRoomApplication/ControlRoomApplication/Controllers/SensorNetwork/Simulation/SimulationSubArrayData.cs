using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Simulation
{
    /// <summary>
    /// This references all the simulated sensors that the SimulatedSensorNetwork uses.
    /// It holds the data that it is sending to the SensorNetworkServer at a given time.
    /// </summary>
    public struct SimulationSubArrayData
    {
        /// <summary>
        /// Sub array of elevation temperatures.
        /// </summary>
        public double[] ElevationTemps { get; set; }
        
        /// <summary>
        /// Sub array of azimuth temperatures.
        /// </summary>
        public double[] AzimuthTemps { get; set; }

        /// <summary>
        /// Sub array of elevation accelerometer data.
        /// </summary>
        public RawAccelerometerData[] ElevationAccl { get; set; }

        /// <summary>
        /// Sub array of azimuth accelerometer data.
        /// </summary>
        public RawAccelerometerData[] AzimuthAccl { get; set; }

        /// <summary>
        /// Sub array of counterbalance accelerometer data.
        /// </summary>
        public RawAccelerometerData[] CounterBAccl { get; set; }

        /// <summary>
        /// Sub array of elevation encoder data.
        /// </summary>
        public double[] ElevationEnc { get; set; }

        /// <summary>
        /// Sub array of azimuth encoder data.
        /// </summary>
        public double[] AzimuthEnc { get; set; }

        /// <summary>
        /// Sub array of ambient temperatures.
        /// </summary>
        public float[] AmbientTemps { get; set; }

        /// <summary>
        /// Sub array of ambient temperatures.
        /// </summary>
        public float[] AmbientHumidity { get; set; }
    }
}
