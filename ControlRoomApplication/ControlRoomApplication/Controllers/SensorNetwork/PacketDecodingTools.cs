using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.DiagnosticData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// Various functions that we will be using to decode the packets.
    /// </summary>
    public class PacketDecodingTools
    {

        /// <summary>
        /// This is a helper function to get the acceleration array from the byte data.
        /// </summary>
        /// <param name="currPointer">This is the current place we are in the byte array. We want to pass this by reference
        /// so that future functions know where to begin.</param>
        /// <param name="data">This is the byte array that we are converting in to acceleration.</param>
        /// <param name="size">This is the size of the acceleration dumps that we expect to see in the byte array.</param>
        /// <param name="sensor">This is the sensor that the data is being created for.</param>
        /// <param name="timeOffset">This is an offset applied to the incoming sensor data timestamps to get more accurate readings.</param>
        /// <returns></returns>
        public static Acceleration[] GetAccelerationFromBytes(ref int currPointer, byte[] data, int size, SensorLocationEnum sensor, long timeOffset = 0)
        {
            // Default sampling frequency for accelerometers is 800 Hz
            double samplingFrequency = 800;

            // Determine which frequency to use in time calculations based on sensor location
            switch (sensor)
            {
                case SensorLocationEnum.AZ_MOTOR:
                    samplingFrequency = SensorNetworkConstants.AzAccelSamplingFrequency;
                    break;
                case SensorLocationEnum.EL_MOTOR:
                    samplingFrequency = SensorNetworkConstants.ElAccelSamplingFrequency;
                    break;
                case SensorLocationEnum.COUNTERBALANCE:
                    samplingFrequency = SensorNetworkConstants.CbAccelSamplingFrequency;
                    break;
            }
            
            // Parse acceleration data from packet
            List<Acceleration> acceleration = new List<Acceleration>();
            for (int j = 0; j < size; j++)
            {
                // Parse out timestamp from first 8 bytes
                long timeStamp = (data[currPointer++] << 56 | data[currPointer++] << 48 | data[currPointer++] << 40 | data[currPointer++] << 32
                    | data[currPointer++] << 24 | data[currPointer++] << 16 | data[currPointer++] << 8 | data[currPointer++]);

                // Apply timestamp offset
                timeStamp += timeOffset;

                // Get the acceleration dump size from next 2 bytes
                short dumpSize = (short)(data[currPointer++] << 8 | data[currPointer++]);

                // Parse acceleration samples
                for (int k = 0; k < dumpSize; k++)
                {
                    acceleration.Add(Acceleration.Generate(
                        // Timestamp is for the last acceleration value taken, so compute the difference for each acceleration value up to the timestamp
                        timeStamp - (long)((dumpSize - (k + 1)) / samplingFrequency * 1000),
                        (short)(data[currPointer++] << 8 | data[currPointer++]),
                        (short)(data[currPointer++] << 8 | data[currPointer++]),
                        (short)(data[currPointer++] << 8 | data[currPointer++]),
                        sensor
                    ));
                }
            }

            return acceleration.ToArray();
        }

        /// <summary>
        /// This is a helper function to get the motor temperature data from the byte array we receive from the Sensor Network.
        /// </summary>
        /// <param name="currPointer">This is the current place we are in the byte array. We want to pass this by reference
        /// so that future functions know where to begin.</param>
        /// <param name="data">This is the byte array that we are converting into temperature.</param>
        /// <param name="size">This is the size of the temperature data that we expect to see in the byte array.</param>
        /// <param name="sensor">This is the sensor that the data is being created for.</param>
        /// <returns>The parsed temperature values in a temperature array.</returns>
        public static Temperature[] GetMotorTemperatureFromBytes(ref int currPointer, byte[] data, int size, SensorLocationEnum sensor)
        {

            Temperature[] temperature = new Temperature[size];
            for (int j = 0; j < size; j++)
            {
                temperature[j] = Temperature.Generate(
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    (double)((short)((data[currPointer++] << 8 | data[currPointer++]))) / 16, // Converting to Celsius by default because we receive raw data
                    sensor
                );
            }

            return temperature;
        }

        /// <summary>
        /// This is a helper function to get the ambient temperature data from the byte array we receive from the Sensor Network.
        /// </summary>
        /// <param name="currPointer">This is the current place we are in the byte array. We want to pass this by reference
        /// so that future functions know where to begin.</param>
        /// <param name="data">This is the byte array that we are converting into temperature.</param>
        /// <param name="size">This is the size of the temperature data that we expect to see in the byte array.</param>
        /// <param name="sensor">This is the sensor that the data is being created for.</param>
        /// <returns>The parsed temperature values in a temperature array.</returns>
        public static Temperature[] GetAmbientTemperatureFromBytes(ref int currPointer, byte[] data, int size, SensorLocationEnum sensor)
        {
            Temperature[] temperature = new Temperature[size];
            for (int j = 0; j < size; j++)
            {
                temperature[j] = Temperature.Generate(
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    (float)((data[currPointer++] << 8 | data[currPointer++])),  // Comes in as fahrenheit float
                    sensor
                );
            }

            return temperature;
        }

        /// <summary>
        /// This is a helper function to get the ambient humidity data from the byte array we receive from the Sensor Network.
        /// </summary>
        /// <param name="currPointer">This is the current place we are in the byte array. We want to pass this by reference
        /// so that future functions know where to begin.</param>
        /// <param name="data">This is the byte array that we are converting into humidity.</param>
        /// <param name="size">This is the size of the humidity data that we expect to see in the byte array.</param>
        /// <param name="sensor">This is the sensor that the data is being created for.</param>
        /// <returns>The parsed humidity values in a temperature array.</returns>
        public static Humidity[] GetAmbientHumidityFromBytes(ref int currPointer, byte[] data, int size, SensorLocationEnum sensor)
        {
            Humidity[] humidity = new Humidity[size];
            for (int j = 0; j < size; j++)
            {
                humidity[j] = Humidity.Generate(
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    (float)((data[currPointer++] << 8 | data[currPointer++])),  // Comes in as percent humidity float
                    sensor
                );
            }

            return humidity;
        }

        /// <summary>
        /// Converts two bytes to the azimuth position, then adds an offset and normalizes the orientation.
        /// </summary>
        /// <param name="currPointer">Current index in the byte array.</param>
        /// <param name="data">Data we are converting to a double.</param>
        /// <param name="offset">The offset to apply after double conversion.</param>
        /// <param name="currPos">The current position in case the one we get in is invalid.</param>
        /// <returns>Current azimuth position.</returns>
        public static double GetAzimuthAxisPositionFromBytes(ref int currPointer, byte[] data, double offset, double currPos)
        {
            double azPos = 360 / SensorNetworkConstants.AzimuthEncoderScaling * (short)(data[currPointer++] << 8 | data[currPointer++]);

            if (Math.Abs(azPos) > 360) return currPos;

            double azPosOffs = (azPos + offset) * -1;

            // Because the offset could cause the axis position to be negative, we want to normalize that to a positive value.
            while (azPosOffs < 0) azPosOffs += 360;

            return azPosOffs;
        }

        /// <summary>
        /// Converts two bytes to the elevation position, then adds an offset to the position.
        /// </summary>
        /// <param name="currPointer">Current index in the byte array.</param>
        /// <param name="data">Data we are converting to a double.</param>
        /// <param name="offset">The offset to apply after double conversion.</param>
        /// <param name="currPos">The current position in case the one we get in is invalid.</param>
        /// <returns>Current elevation position.</returns>
        public static double GetElevationAxisPositionFromBytes(ref int currPointer, byte[] data, double offset, double currPos)
        {
            double elPos = -0.25 * (short)(data[currPointer++] << 8 | data[currPointer++]) + 104.375;

            double elPosOffs = (elPos - offset);

            return elPosOffs;
        }
    }
}
