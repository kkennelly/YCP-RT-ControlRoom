using ControlRoomApplication.Controllers.SensorNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Entities;
using EmbeddedSystemsTest.SensorNetworkSimulation;
using ControlRoomApplication.Entities.DiagnosticData;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests
{
    [TestClass]
    public class PacketDecodingToolsTest
    {
        [TestMethod]
        public void TestGetAccelerationFromBytes_BytesToAcceleration_ReturnsAcceleration()
        {
            // The byte size for one acceleration is 6 bytes, because each axis takes up 2 bytes, 
            // and there are 3 axes.
            byte[] oneAcceleration = new byte[16];

            // Create acceleration with timestamp of UTC 1
            oneAcceleration[0] = 0;
            oneAcceleration[1] = 0;
            oneAcceleration[2] = 0;
            oneAcceleration[3] = 0;
            oneAcceleration[4] = 0;
            oneAcceleration[5] = 0;
            oneAcceleration[6] = 0;
            oneAcceleration[7] = 1;

            // Create acceleration with a size 1 FIFO dump
            oneAcceleration[8] = 0;
            oneAcceleration[9] = 1;

            // This will create an acceleration with x, y and z axes as 1, 2 and 3 respectively
            oneAcceleration[10] = 0;
            oneAcceleration[11] = 1;
            oneAcceleration[12] = 0;
            oneAcceleration[13] = 2;
            oneAcceleration[14] = 0;
            oneAcceleration[15] = 3;

            Acceleration[] expected = new Acceleration[1];
            expected[0] = Acceleration.Generate(1, 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAccelerationFromBytes(ref i, oneAcceleration, 1, SensorLocationEnum.COUNTERBALANCE, 800);

            Assert.AreEqual(1, result.Length); // Only expecting one result
            Assert.AreEqual(expected[0].TimeCaptured, result[0].TimeCaptured);
            Assert.AreEqual(expected[0].x, result[0].x);
            Assert.AreEqual(expected[0].y, result[0].y);
            Assert.AreEqual(expected[0].z, result[0].z);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);
        }

        [TestMethod]
        public void TestGetAccelerationFromBytes_BytesToMultipleAcceleration_ReturnsMultipleAcceleration()
        {
            // The byte size for one acceleration is 6 bytes, because each axis takes up 2 bytes, 
            // and there are 3 axes.
            byte[] twoAcceleration = new byte[22];

            // Create acceleration with timestamp of UTC 1
            twoAcceleration[0] = 0;
            twoAcceleration[1] = 0;
            twoAcceleration[2] = 0;
            twoAcceleration[3] = 0;
            twoAcceleration[4] = 0;
            twoAcceleration[5] = 0;
            twoAcceleration[6] = 0;
            twoAcceleration[7] = 1;

            // Create acceleration with a size 2 FIFO dump
            twoAcceleration[8] = 0;
            twoAcceleration[9] = 2;

            // This will create two acceleration results with x, y and z axes as 1, 2 and 3 respectively
            twoAcceleration[10] = 0;
            twoAcceleration[11] = 1;
            twoAcceleration[12] = 0;
            twoAcceleration[13] = 2;
            twoAcceleration[14] = 0;
            twoAcceleration[15] = 3;
            twoAcceleration[16] = 0;
            twoAcceleration[17] = 1;
            twoAcceleration[18] = 0;
            twoAcceleration[19] = 2;
            twoAcceleration[20] = 0;
            twoAcceleration[21] = 3;

            Acceleration[] expected = new Acceleration[2];
            expected[0] = Acceleration.Generate(1 - (long)(1 / 800.0 * 1000), 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);
            expected[1] = Acceleration.Generate(1, 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAccelerationFromBytes(ref i, twoAcceleration, 1, SensorLocationEnum.COUNTERBALANCE, 800);

            Assert.AreEqual(2, result.Length); // Expecting two results

            Assert.AreEqual(expected[0].TimeCaptured, result[0].TimeCaptured);

            Assert.AreEqual(expected[0].x, result[0].x);
            Assert.AreEqual(expected[0].y, result[0].y);
            Assert.AreEqual(expected[0].z, result[0].z);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);

            Assert.AreEqual(expected[1].TimeCaptured, result[1].TimeCaptured);

            Assert.AreEqual(expected[1].x, result[1].x);
            Assert.AreEqual(expected[1].y, result[1].y);
            Assert.AreEqual(expected[1].z, result[1].z);
            Assert.AreEqual(expected[1].location_ID, result[1].location_ID);
        }

        [TestMethod]
        public void TestGetMotorTemperatureFromBytes_BytesToTemperature_ReturnsTemperature()
        {
            // The byte size for one temperature is 2 bytes
            byte[] oneTemperature = new byte[2];

            // This will create temperature value of 1, because the temperature is divided by 16
            oneTemperature[0] = 0;
            oneTemperature[1] = 16;

            // Skipping the timestamp because we aren't concerned with that in this test
            Temperature[] expected = new Temperature[1];
            expected[0] = Temperature.Generate(0, 1, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetMotorTemperatureFromBytes(ref i, oneTemperature, 1, SensorLocationEnum.COUNTERBALANCE);

            Assert.AreEqual(1, result.Length); // Only expecting one result

            Assert.AreEqual(expected[0].temp, result[0].temp);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);
        }

        [TestMethod]
        public void TestGetMotorTemperatureFromBytes_BytesToMultipleTemperatures_ReturnsMultipleTemperatures()
        {
            // The byte size for one temperature is 2 bytes
            byte[] twoTemperature = new byte[4];

            // This will create temperature value of 1, because the temperature is divided by 16
            twoTemperature[0] = 0;
            twoTemperature[1] = 16;
            twoTemperature[2] = 0;
            twoTemperature[3] = 16;

            // Skipping the timestamp because we aren't concerned with that in this test
            Temperature[] expected = new Temperature[2];
            expected[0] = Temperature.Generate(0, 1, SensorLocationEnum.COUNTERBALANCE);
            expected[1] = Temperature.Generate(0, 1, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetMotorTemperatureFromBytes(ref i, twoTemperature, 2, SensorLocationEnum.COUNTERBALANCE);

            Assert.AreEqual(2, result.Length); // Expecting two results

            Assert.AreEqual(expected[0].temp, result[0].temp);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);

            Assert.AreEqual(expected[1].temp, result[1].temp);
            Assert.AreEqual(expected[1].location_ID, result[1].location_ID);
        }

        [TestMethod]
        public void TestGetAmbientTemperatureFromBytes_BytesToTemperature_ReturnsTemperature()
        {
            // This will create temperature value of 1
            byte[] oneTemperature = BitConverter.GetBytes(1f);

            // Skipping the timestamp because we aren't concerned with that in this test
            Temperature[] expected = new Temperature[1];
            expected[0] = Temperature.Generate(0, 1, SensorLocationEnum.EL_FRAME);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAmbientTemperatureFromBytes(ref i, oneTemperature, 1, SensorLocationEnum.EL_FRAME);

            Assert.AreEqual(1, result.Length); // Only expecting one result

            Assert.AreEqual(expected[0].temp, result[0].temp);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);
        }

        [TestMethod]
        public void TestGetAmbientTemperatureFromBytes_BytesToMultipleTemperatures_ReturnsMultipleTemperatures()
        {
            // The byte size for one temperature is 8 bytes
            byte[] twoTemperature = new byte[8];

            // This will create temperature value of 1, because the temperature is divided by 16
            byte[] oneTemperature = BitConverter.GetBytes(1f);

            // Add 1 to the array twice
            for (int j = 0; j < 8; j++)
            {
                twoTemperature[j] = oneTemperature[j % 4];
            }

            // Skipping the timestamp because we aren't concerned with that in this test
            Temperature[] expected = new Temperature[2];
            expected[0] = Temperature.Generate(0, 1, SensorLocationEnum.EL_FRAME);
            expected[1] = Temperature.Generate(0, 1, SensorLocationEnum.EL_FRAME);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAmbientTemperatureFromBytes(ref i, twoTemperature, 2, SensorLocationEnum.EL_FRAME);

            Assert.AreEqual(2, result.Length); // Expecting two results

            Assert.AreEqual(expected[0].temp, result[0].temp);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);

            Assert.AreEqual(expected[1].temp, result[1].temp);
            Assert.AreEqual(expected[1].location_ID, result[1].location_ID);
        }

        [TestMethod]
        public void TestGetAmbientHumidityFromBytes_BytesToHumidity_ReturnsHumidity()
        {
            // This will create humidity value of 1
            byte[] oneHumidity = BitConverter.GetBytes(1f);

            // Skipping the timestamp because we aren't concerned with that in this test
            Humidity[] expected = new Humidity[1];
            expected[0] = Humidity.Generate(0, 1, SensorLocationEnum.EL_FRAME);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAmbientHumidityFromBytes(ref i, oneHumidity, 1, SensorLocationEnum.EL_FRAME);

            Assert.AreEqual(1, result.Length); // Only expecting one result

            Assert.AreEqual(expected[0].HumidityReading, result[0].HumidityReading);
            Assert.AreEqual(expected[0].LocationID, result[0].LocationID);
        }

        [TestMethod]
        public void TestGetAmbientHumidityFromBytes_BytesToMultipleHumidity_ReturnsMultipleHumidity()
        {
            // The byte size for two humidity is 8 bytes
            byte[] twoHumidity = new byte[8];

            // This will create temperature value of 1, because the temperature is divided by 16
            byte[] oneHumidity = BitConverter.GetBytes(1f);

            // Add 1 to the array twice
            for (int j = 0; j < 8; j++)
            {
                twoHumidity[j] = oneHumidity[j % 4];
            }

            // Skipping the timestamp because we aren't concerned with that in this test
            Humidity[] expected = new Humidity[2];
            expected[0] = Humidity.Generate(0, 1, SensorLocationEnum.EL_FRAME);
            expected[1] = Humidity.Generate(0, 1, SensorLocationEnum.EL_FRAME);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAmbientHumidityFromBytes(ref i, twoHumidity, 2, SensorLocationEnum.EL_FRAME);

            Assert.AreEqual(2, result.Length); // Expecting two results

            Assert.AreEqual(expected[0].HumidityReading, result[0].HumidityReading);
            Assert.AreEqual(expected[0].LocationID, result[0].LocationID);

            Assert.AreEqual(expected[1].HumidityReading, result[1].HumidityReading);
            Assert.AreEqual(expected[1].LocationID, result[1].LocationID);
        }

        [TestMethod]
        public void TestGetAzimuthAxisPositionFromBytes_BytesToPosition_ReturnsPosition()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;

            // 310 is 50 degrees away from 0, on the opposite end of 50
            double expected = 50;
            short encoded = PacketEncodingTools.ConvertDegreesToRawAzData(expected);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            int offset = 0;

            double result = PacketDecodingTools.GetAzimuthAxisPositionFromBytes(ref i, pos, offset, 0);

            Assert.AreEqual(expected, result, 0.16);
        }

        [TestMethod]
        public void TestGetAzimuthAxisPositionFromBytes_BytesToPositionWithOffset_ReturnsNormalizedPosition()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;
            double initialValue = 50;
            short encoded = PacketEncodingTools.ConvertDegreesToRawAzData(initialValue);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            // With an offset of 60, that would make the origination originally -10, but with normalization, it should be 350
            int offset = 60;

            double expected = 350;

            double result = PacketDecodingTools.GetAzimuthAxisPositionFromBytes(ref i, pos, offset, 0);

            Assert.AreEqual(expected, result, 0.16);
        }

        [TestMethod]
        public void TestGetAzimuthAxisPositionFromBytes_CalculatedGreaterThan360_ReturnsOriginalValue()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;
            double initialValue = 361;
            short encoded = PacketEncodingTools.ConvertDegreesToRawAzData(initialValue);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            int offset = 0;

            int expected = 10;

            double result = PacketDecodingTools.GetAzimuthAxisPositionFromBytes(ref i, pos, offset, 10);

            Assert.AreEqual(expected, result, 0.16);
        }

        [TestMethod]
        public void TestGetElevationAxisPositionFromBytes_BytesToPosition_ReturnsPosition()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;
            double expected = 50;
            short encoded = PacketEncodingTools.ConvertDegreesToRawElData(expected);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            int offset = 0;

            double result = PacketDecodingTools.GetElevationAxisPositionFromBytes(ref i, pos, offset, 0);

            Assert.AreEqual(expected, result, 0.16);
        }

        [TestMethod]
        public void TestGetElevationAxisPositionFromBytes_BytesToPositionWithOffset_ReturnsPosition()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;
            double expected = 50;
            short encoded = PacketEncodingTools.ConvertDegreesToRawElData(expected);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            int offset = 10;

            double result = PacketDecodingTools.GetElevationAxisPositionFromBytes(ref i, pos, offset, 0);

            Assert.AreEqual(expected - offset, result, 0.16);
        }
    }
}
