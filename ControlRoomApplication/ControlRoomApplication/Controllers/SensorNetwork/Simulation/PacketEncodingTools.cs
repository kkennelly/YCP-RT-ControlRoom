using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Controllers.SensorNetwork.Simulation;
using ControlRoomApplication.Entities.DiagnosticData;
using System;
using System.Linq;

namespace EmbeddedSystemsTest.SensorNetworkSimulation
{
    /// <summary>
    /// This contains helper functions to allow us to encode data in the same way the Sensor Network would before sending it to us.
    /// The data it encodes will be data from CSVs, but it will "send" raw data to the SensorNetworkServer.
    /// </summary>
    public class PacketEncodingTools
    {
        /// <summary>
        /// This is important for the simulation. This will convert the data arrays we get from CSV files
        /// to bytes that we can send to the SensorNetworkServer.
        /// </summary>
        /// <param name="elAccl">Array of RAW elevation accelerometer samples.</param>
        /// <param name="azAccl">Array of RAW azimuth accelerometer samples.</param>
        /// <param name="cbAccl">Array of RAW counterbalance accelerometer samples.</param>
        /// <param name="elTemps">Array of elevation temperature samples.</param>
        /// <param name="azTemps">Array of azimuth temperature samples.</param>
        /// <param name="elEnc">Array of elevation encoder samples.</param>
        /// <param name="azEnc">Array of azimuth encoder samples.</param>
        /// <param name="ambTemp">Array of ambient temperature samples.</param>
        /// <param name="ambHumidity">Array of ambient humidity samples.</param>
        /// <param name="statuses">All the sensor statuses and errors that come from the sensor network.</param>
        /// <param name="connectionTimeStamp">The time the sensor network connected to the control room. Used to generate simulated acceleration time captures</param>
        /// <returns></returns>
        public static byte[] ConvertDataArraysToBytes(RawAccelerometerData[] elAccl, RawAccelerometerData[] azAccl, RawAccelerometerData[] cbAccl, double[] elTemps, double[] azTemps, double[] elEnc, double[] azEnc, float[] ambTemp, float[] ambHumidity, SensorStatuses statuses, long connectionTimeStamp)
        {
            uint dataSize = CalcDataSize(elAccl.Length, azAccl.Length, cbAccl.Length, elTemps.Length, azTemps.Length, elEnc.Length, azEnc.Length, ambTemp.Length, ambHumidity.Length);

            // If you want to input raw data instead, just comment out the next few loops.
            // They exist so that we can input data into our CSV files that make sense to us, since
            // raw data values are not very readable

            // Convert elevation temperature to raw data
            short[] rawElTemps = new short[elTemps.Length];
            for (int i = 0; i < elTemps.Length; i++)
            {
                rawElTemps[i] = ConvertTempCToRawData(elTemps[i]);
            }

            // Convert azimuth temperature to raw data
            short[] rawAzTemps = new short[azTemps.Length];
            for (int i = 0; i < azTemps.Length; i++)
            {
                rawAzTemps[i] = ConvertTempCToRawData(azTemps[i]);
            }

            // Convert elevation position to raw data
            short[] rawElEnc = new short[elEnc.Length];
            for(int i = 0; i < elEnc.Length; i++)
            {
                rawElEnc[i] = ConvertDegreesToRawElData(elEnc[i]);
            }

            // Convert azimuth position to raw data
            short[] rawAzEnc = new short[azEnc.Length];
            for (int i = 0; i < azEnc.Length; i++)
            {
                rawAzEnc[i] = ConvertDegreesToRawAzData(azEnc[i]);
            }

            bool[] sensorStatusBoolArray = new bool[] {
                statuses.ElevationAccelerometerStatus == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthAccelerometerStatus == SensorNetworkSensorStatus.Okay,
                statuses.CounterbalanceAccelerometerStatus == SensorNetworkSensorStatus.Okay,
                statuses.ElevationTemperature1Status == SensorNetworkSensorStatus.Okay,
                statuses.ElevationTemperature2Status == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthTemperature1Status == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthTemperature2Status == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthAbsoluteEncoderStatus == SensorNetworkSensorStatus.Okay,
                statuses.ElevationAmbientStatus == SensorNetworkSensorStatus.Okay
            };

            int errors = 0; // TODO: implement conversion (issue #376)

            return EncodeRawData(dataSize, elAccl, azAccl, cbAccl, rawElTemps, rawAzTemps, rawElEnc, rawAzEnc, ambTemp, ambHumidity, sensorStatusBoolArray, errors, connectionTimeStamp);
        }

        /// <summary>
        /// This will take each RAW data array and add it to its proper location in the byte array
        /// </summary>
        /// <param name="dataSize">The total size of the byte array we are adding data to.</param>
        /// <param name="elAcclData">Array of RAW elevation accelerometer samples.</param>
        /// <param name="azAcclData">Array of RAW azimuth accelerometer samples.</param>
        /// <param name="cbAcclData">Array of RAW counterbalance accelerometer samples.</param>
        /// <param name="elTemp">Array of RAW elevation temperature samples.</param>
        /// <param name="azTemp">Array of RAW azimuth temperature samples.</param>
        /// <param name="elEnc">Array of RAW elevation encoder samples. (Should only ever be a size of 1)</param>
        /// <param name="azEnc">Array of RAW azimuth encoder samples. (Should only ever be a size of 1)</param>
        /// <param name="ambTemp">Array of ambient temperature samples./param>
        /// <param name="ambHumidity">Array of ambient humidity samples.</param>
        /// <param name="connectionTimeStamp">The time the sensor network connected to the control room. Used to generate simulated acceleration time captures</param>
        /// <returns></returns>
        public static byte[] EncodeRawData(uint dataSize, RawAccelerometerData[] elAcclData, RawAccelerometerData[] azAcclData, RawAccelerometerData[] cbAcclData, short[] elTemp, short[] azTemp, short[] elEnc, short[] azEnc, float[] ambTemp, float[] ambHumidity, bool[] statuses, int errors, long connectionTimeStamp)
        {
            byte[] data = new byte[dataSize];

            int i = 0;
            data[i++] = SensorNetworkConstants.TransitIdSuccess;

            // Store the total data size in 4 bytes
            Add32BitValueToByteArray(ref data, ref i, dataSize);

            // Store the sensor statuses
            // To avoid changing the code too much, store the first 8 statuses, and then the remaining status
            data[i++] = ConvertBoolArrayToByte(statuses.Take(8).ToArray());
            data[i++] = ConvertBoolArrayToByte(statuses.Skip(8).ToArray());

            // Store the sensor errors in 3 bytes
            Add24BitValueToByteArray(ref data, ref i, errors);

            // Store the number of elevation accelerometer fifo dumps in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)Math.Ceiling(elAcclData.Length * 1.0 / SensorNetworkConstants.ElAccelFIFOSize));

            // Store the number of azimuth accelerometer fifo dumps in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)Math.Ceiling(azAcclData.Length * 1.0 / SensorNetworkConstants.AzAccelFIFOSize));

            // Store the number of counterbalance accelerometer fifo dumps in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)Math.Ceiling(cbAcclData.Length * 1.0 / SensorNetworkConstants.CbAccelFIFOSize));

            // Store elevation temperature size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)elTemp.Length);

            // Store azimuth temperature size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)azTemp.Length);

            // Store elevation encoder size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)elEnc.Length);

            // Store azimuth encoder size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)azEnc.Length);

            // Store ambeint temp size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)ambTemp.Length);

            // Store ambient humidity size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)ambHumidity.Length);

            // Store elevation accelerometer data in a variable number of bytes
            AddAcclDataToByteArray(ref data, ref i, ref elAcclData, SensorNetworkConstants.ElAccelFIFOSize, connectionTimeStamp);

            // Store azimuth accelerometer data in a variable number of bytes
            AddAcclDataToByteArray(ref data, ref i, ref azAcclData, SensorNetworkConstants.AzAccelFIFOSize, connectionTimeStamp);

            // Store counterbalance accelerometer data in a variable number of bytes
            AddAcclDataToByteArray(ref data, ref i, ref cbAcclData, SensorNetworkConstants.CbAccelFIFOSize, connectionTimeStamp);

            // Store elevation temperature data in a variable number of bytes
            // Each temperature occupies 2 bytes
            for (uint j = 0; j < elTemp.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)elTemp[j]);
            }

            // Store azimuth temperature data in a variable number of bytes
            // Each temperature occupies 2 bytes
            for (uint j = 0; j < azTemp.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)azTemp[j]);
            }

            // Store elevation encoder data in a variable number of bytes
            // Each position occupies 2 bytes
            for (uint j = 0; j < elEnc.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)elEnc[j]);
            }

            // Store azimuth encoder data in a variable number of bytes
            // Each position occupies 2 bytes
            for (uint j = 0; j < azEnc.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)azEnc[j]);
            }

            // Store ambient temp data in a variable number of bytes
            // Each sample occupies 4 bytes
            for (uint j = 0; j < ambTemp.Length; j++)
            {
                byte[] bytes = BitConverter.GetBytes(ambTemp[j]);
                for (int k = 0; k < bytes.Length; k++)
                {
                    data[i++] = bytes[k];
                }
            }

            // Store ambient humidity data in a variable number of bytes
            // Each sample occupies 4 bytes
            for (uint j = 0; j < ambHumidity.Length; j++)
            {
                byte[] bytes = BitConverter.GetBytes(ambHumidity[j]);
                for (int k = 0; k < bytes.Length; k++)
                {
                    data[i++] = bytes[k];
                }
            }

            return data;
        }

        /// <summary>
        /// Calculates the size of the packet that will be sent to the sensor network.
        /// This value will be used to create the byte array.
        /// </summary>
        /// <param name="elAccSize">The number of elevation accelerometer samples.</param>
        /// <param name="azAccSize">The number of azimuth accelerometer samples.</param>
        /// <param name="cbAccSize">The number of counterbalance accelerometer samples.</param>
        /// <param name="elTempSize">The number of elevation temperature samples.</param>
        /// <param name="azTempSize">The number of azimuth temperature samples.</param>
        /// <param name="elEncSize">The number of elevation encoder samples.</param>
        /// <param name="azEncSize">The number of azimuth encoder samples.</param>
        /// <param name="ambTempSize">The number of ambient temperature samples.</param>
        /// <param name="ambHumiditySize">The number of ambient humidity samples.</param>
        /// <returns></returns>
        public static uint CalcDataSize(int elAccSize, int azAccSize, int cbAccSize, int elTempSize, int azTempSize, int elEncSize, int azEncSize, int ambTempSize, int ambHumiditySize)
        {
            // 1 for the transmit ID
            // 4 for the total data size
            // 4 for the sensor statuses and errors
            // 14 for each sensor's data size (each sensor size is 2 bytes, with 7 sensors total)
            uint length = 1 + 4 + 5 + 18;

            // Each accelerometer axis is 2 bytes each. With three axes, that's 6 bytes per accelerometer
            length += (uint)elAccSize * 6;
            length += (uint)azAccSize * 6;
            length += (uint)cbAccSize * 6;

            // Each accelerometer dump has a timestamp associated with it of 8 bytes
            length += (uint)Math.Ceiling(elAccSize * 1.0 / SensorNetworkConstants.ElAccelFIFOSize) * 8;
            length += (uint)Math.Ceiling(azAccSize * 1.0 / SensorNetworkConstants.AzAccelFIFOSize) * 8;
            length += (uint)Math.Ceiling(cbAccSize * 1.0 / SensorNetworkConstants.CbAccelFIFOSize) * 8;

            // Each acceleromter dump has a dump size associated with it that takes up 2 bytes
            length += (uint)Math.Ceiling(elAccSize * 1.0 / SensorNetworkConstants.ElAccelFIFOSize) * 2;
            length += (uint)Math.Ceiling(azAccSize * 1.0 / SensorNetworkConstants.AzAccelFIFOSize) * 2;
            length += (uint)Math.Ceiling(cbAccSize * 1.0 / SensorNetworkConstants.CbAccelFIFOSize) * 2;

            // Each temp and encoder value is 2 bytes
            length += (uint)elTempSize * 2;
            length += (uint)azTempSize * 2;

            // Encoder arrays should always be of size 1, so they should only ever be two bytes each
            length += (uint)elEncSize * 2;
            length += (uint)azEncSize * 2;

            // Each ambient temp and humidity value is 4 bytes
            length += (uint)ambTempSize * 4;
            length += (uint)ambHumiditySize * 4;

            return length;
        }

        /// <summary>
        /// This is so we can give the simulation "real" data, where it will be converted to raw
        /// before being encoded. This is approximate, and may not be exact.
        /// </summary>
        /// <param name="dataToConvert">The data we are converting to a raw elevation position.</param>
        /// <returns>The raw elevation position.</returns>
        public static short ConvertDegreesToRawElData(double dataToConvert)
        {
            return (short)Math.Round((dataToConvert - 104.375) / -0.25);
        }

        /// <summary>
        /// This is so we can give the simulation "real" data, where it will be converted to raw
        /// before being encoded. This is approximate, and may not be exact.
        /// </summary>
        /// <param name="dataToConvert">The data we are converting to a raw azimuth position.</param>
        /// <returns>The raw elevation position</returns>
        public static short ConvertDegreesToRawAzData(double dataToConvert)
        {
            return (short)((SensorNetworkConstants.AzimuthEncoderScaling * dataToConvert / 360) * -1);
        }

        /// <summary>
        /// This converts the degrees from celsius into raw data. This is approximate.
        /// </summary>
        /// <param name="dataToConvert">The data we are converting to a raw value (in Celsius)</param>
        /// <returns>Raw temperature data.</returns>
        public static short ConvertTempCToRawData(double dataToConvert)
        {
            return (short)(dataToConvert * 16);
        }

        /// <summary>
        /// A helper function to add 16-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add16BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, short dataBeingAdded)
        {
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0xFF00) >> 8);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded & 0x00FF)));
        }

        /// <summary>
        /// A helper function to add 24-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add24BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, int dataBeingAdded)
        {
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0xFF0000) >> 16);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0x00FF00) >> 8);
            dataToAddTo[counter++] = (byte)((short)dataBeingAdded & 0x0000FF);
        }

        /// <summary>
        /// A helper function to add 32-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add32BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, uint dataBeingAdded)
        {
            dataToAddTo[counter++] = (byte)((dataBeingAdded & 0xFF000000) >> 24);
            dataToAddTo[counter++] = (byte)((dataBeingAdded & 0x00FF0000) >> 16);
            dataToAddTo[counter++] = (byte)((dataBeingAdded & 0x0000FF00) >> 8);
            dataToAddTo[counter++] = (byte)(dataBeingAdded & 0x000000FF);
        }

        /// <summary>
        /// A helper function to add 64-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add64BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, ulong dataBeingAdded)
        {
            Add32BitValueToByteArray(ref dataToAddTo, ref counter, (uint)((dataBeingAdded & 0xFFFFFFFF00000000) >> 32));
            Add32BitValueToByteArray(ref dataToAddTo, ref counter, (uint)(dataBeingAdded & 0x00000000FFFFFFFF));
        }


        private static byte ConvertBoolArrayToByte(bool[] source)
        {
            if (source.Length > 8) throw new ArgumentOutOfRangeException("There can only be 8 bits in a byte array.");

            byte result = 0;

            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }

        /// <summary>
        /// A helper function to add acceleration data to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="accl">The raw acceleration data we are adding.</param>
        /// <param name="fifoSize">The size of the FIFO for the accelerometer.</param>
        /// <param name="connectionTimeStamp">The UTC ms time that the sensor network connected to the control room.</param>
        private static void AddAcclDataToByteArray(ref byte[] dataToAddTo, ref int counter, ref RawAccelerometerData[] accl, short fifoSize, long connectionTimeStamp)
        {
            double totalNumDumps = Math.Ceiling(accl.Length * 1.0 / fifoSize);

            // Process and encode each fifo dump into the array
            for (int dumpNum = 0; dumpNum < totalNumDumps; dumpNum++)
            {
                // Add a generated timestamp, apply a offset to simulate time passing, and account for the connection time
                Add64BitValueToByteArray(ref dataToAddTo, ref counter, (ulong)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + (50 * dumpNum) - connectionTimeStamp));

                // Set default dump size
                short dumpSize = fifoSize;

                // Calculate the base index to take from the accl array
                int baseIndex = dumpNum * fifoSize;

                // Change the dump size to be the remaining data size left if there is not a full dump available
                if (baseIndex + fifoSize > accl.Length)
                {
                    dumpSize = (short)(accl.Length - (dumpNum * fifoSize));
                }

                // Add dump size
                Add16BitValueToByteArray(ref dataToAddTo, ref counter, dumpSize);

                // Each axis occupies 2 bytes, making a total of 6 bytes for each accelerometer data
                for (int j = baseIndex; j < baseIndex + dumpSize; j++)
                {
                    Add16BitValueToByteArray(ref dataToAddTo, ref counter, (short)accl[j].X);
                    Add16BitValueToByteArray(ref dataToAddTo, ref counter, (short)accl[j].Y);
                    Add16BitValueToByteArray(ref dataToAddTo, ref counter, (short)accl[j].Z);
                }
            }
        }
    }
}
