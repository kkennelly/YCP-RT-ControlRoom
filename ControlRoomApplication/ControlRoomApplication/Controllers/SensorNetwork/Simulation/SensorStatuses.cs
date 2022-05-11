using ControlRoomApplication.Entities.DiagnosticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Simulation
{
    /// <summary>
    /// This contains all of the sensor statuses for the Sensor Network.
    /// </summary>
    public class SensorStatuses // TODO: Add errors here (issue #353)
    {
        /// <summary>
        /// The error state of the azimuth absolute encoder.
        /// </summary>
        public SensorNetworkSensorStatus AzimuthAbsoluteEncoderStatus { get; set; }

        /// <summary>
        /// The error state of the elevation absolute encoder.
        /// </summary>
        public SensorNetworkSensorStatus ElevationAbsoluteEncoderStatus { get; set; }

        /// <summary>
        /// The error state of the azimuth temperature sensor.
        /// </summary>
        public SensorNetworkSensorStatus AzimuthTemperature1Status { get; set; }

        /// <summary>
        /// The error state of the redundant azimuth temperature sensor.
        /// </summary>
        public SensorNetworkSensorStatus AzimuthTemperature2Status { get; set; }

        /// <summary>
        /// The error state of the elevation temperature sensor.
        /// </summary>
        public SensorNetworkSensorStatus ElevationTemperature1Status { get; set; }

        /// <summary>
        /// The error state of the redundant elevation temperature sensor.
        /// </summary>
        public SensorNetworkSensorStatus ElevationTemperature2Status { get; set; }

        /// <summary>
        /// The error state of the elevation ambient temperature and humidity sensor.
        /// </summary>
        public SensorNetworkSensorStatus ElevationAmbientStatus { get; set; }

        /// <summary>
        /// The error state of the azimuth accelerometer status.
        /// </summary>
        public SensorNetworkSensorStatus AzimuthAccelerometerStatus { get; set; }

        /// <summary>
        /// The error state of the elevation accelerometer status.
        /// </summary>
        public SensorNetworkSensorStatus ElevationAccelerometerStatus { get; set; }

        /// <summary>
        /// The error state of the counterbalance accelerometer status.
        /// </summary>
        public SensorNetworkSensorStatus CounterbalanceAccelerometerStatus { get; set; }
    }
}
