﻿using ControlRoomApplication.Entities;
using System;
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
        /// <param name="size">This is the size of the acceleration data that we expect to see in the byte array.</param>
        /// <param name="sensor">This is the sensor that the data is being created for.</param>
        /// <returns></returns>
        public static Acceleration[] GetAccelerationFromBytes(ref int currPointer, byte[] data, int size, SensorLocationEnum sensor)
        {

            Acceleration[] acceleration = new Acceleration[size];
            for (int j = 0; j < size; j++)
            {
                acceleration[j] = Acceleration.Generate(
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    data[currPointer++] << 8 | data[currPointer++],
                    data[currPointer++] << 8 | data[currPointer++],
                    data[currPointer++] << 8 | data[currPointer++],
                    sensor
                );
            }

            return acceleration;
        }

        /// <summary>
        /// This is a helper function to get the temperature data from the byte array we receive from the Sensor Network.
        /// </summary>
        /// <param name="currPointer">This is the current place we are in the byte array. We want to pass this by reference
        /// so that future functions know where to begin.</param>
        /// <param name="data">This is the byte array that we are converting in to acceleration.</param>
        /// <param name="size">This is the size of the acceleration data that we expect to see in the byte array.</param>
        /// <param name="sensor">This is the sensor that the data is being created for.</param>
        /// <returns></returns>
        public static Temperature[] GetTemperatureFromBytes(ref int currPointer, byte[] data, int size, SensorLocationEnum sensor)
        {

            Temperature[] temperature = new Temperature[size];
            for (int j = 0; j < size; j++)
            {
                temperature[j] = Temperature.Generate(
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    (data[currPointer++] << 8 | data[currPointer++]) / 16, // Converting to Celsius by default because we receive raw data
                    sensor
                );
            }

            return temperature;
        }
    }
}