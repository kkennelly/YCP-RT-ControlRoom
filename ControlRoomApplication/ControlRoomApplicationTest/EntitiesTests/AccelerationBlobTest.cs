using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AccelerationBlobTest
    {
        // Class being tested
        Acceleration azAcc, cbAcc, elAcc;
        Acceleration[] azAccArr, cbAccArr, elAccArr;
        AzimuthAccelerationBlob azBlob;
        CounterbalanceAccelerationBlob cbBlob;
        ElevationAccelerationBlob elBlob;
        List<Byte> blobData;

        [TestInitialize]
        public void BuildUp()
        {

            //byte array to store 32 acceleration data points
            blobData = new List<byte>();
            azBlob = new AzimuthAccelerationBlob();
            cbBlob = new CounterbalanceAccelerationBlob();
            elBlob = new ElevationAccelerationBlob();
            azAcc = Acceleration.Generate(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 1, 2, 3, SensorLocationEnum.AZ_MOTOR);
            cbAcc = Acceleration.Generate(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);
            elAcc = Acceleration.Generate(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 1, 2, 3, SensorLocationEnum.EL_MOTOR);

            //version
            blobData.Add(1);

            //FIFO Size
            blobData.Add(32);

            //SampleFrequency
            byte[] frequency = BitConverter.GetBytes(800);
            blobData.Add(frequency[0]);
            blobData.Add(frequency[1]);

            //GRange
            blobData.Add(1);

            //full resolution
            blobData.Add(BitConverter.GetBytes(true)[0]);

            //label
            char label = 't';
            blobData.Add(BitConverter.GetBytes(label)[0]);


            //time
            byte[] time = BitConverter.GetBytes(azAcc.TimeCaptured);
           
            for (int i = 0; i < 8; i++)
            {
                blobData.Add(time[i]);
            }

            //acc x
            byte[] accX = BitConverter.GetBytes(azAcc.x);

            for (int i = 0; i < 2; i++)
            {
                blobData.Add(accX[i]);
            }
            //acc x
            byte[] accY = BitConverter.GetBytes(azAcc.y);

            for (int i = 0; i < 2; i++)
            {
                blobData.Add(accY[i]);
            }

            //acc z
            byte[] accZ = BitConverter.GetBytes(azAcc.z);
            for(int i=0; i<2; i++)
            {
                blobData.Add(accZ[i]);
            }
            

            // write 31 data points without time
            for(int size=0; size < 31; size++)
            {
                //label
                label = 'a';
                blobData.Add(BitConverter.GetBytes(label)[0]);

                //acc x
                for (int i = 0; i < 2; i++)
                {
                    blobData.Add(accX[i]);
                }
                //acc y
                for (int i = 0; i < 2; i++)
                {
                    blobData.Add(accY[i]);
                }

                //acc z
                for (int i = 0; i < 2; i++)
                {
                    blobData.Add(accZ[i]);
                }
            }

            //save the three blobs

            azBlob.BlobList = blobData;
            cbBlob.BlobList = blobData;
            elBlob.BlobList = blobData;


            // create the three comparison acceleration arrays
            azAccArr = new Acceleration[32];
            cbAccArr = new Acceleration[32];
            elAccArr = new Acceleration[32];

            for (int i=0; i<32; i++)
            {
                azAccArr[i] = azAcc;
                cbAccArr[i] = cbAcc;
                elAccArr[i] = elAcc;
            }

        }

        [TestMethod]
        public void TestParsing()
        {
            //Check the first acceleration datapoint
            Assert.IsTrue(azAcc.Equals(azBlob.BlobParser(azBlob.BlobList.ToArray())[0]));
            Assert.IsTrue(cbAcc.Equals(cbBlob.BlobParser(cbBlob.BlobList.ToArray())[0]));
            Assert.IsTrue(elAcc.Equals(elBlob.BlobParser(elBlob.BlobList.ToArray())[0]));

            //check the acceleration array
            Assert.IsTrue(Acceleration.SequenceEquals(azAccArr, azBlob.BlobParser(azBlob.BlobList.ToArray())));
            Assert.IsTrue(Acceleration.SequenceEquals(cbAccArr, cbBlob.BlobParser(cbBlob.BlobList.ToArray())));
            Assert.IsTrue(Acceleration.SequenceEquals(elAccArr, elBlob.BlobParser(elBlob.BlobList.ToArray())));

            //print out the blob
            Console.WriteLine(azBlob.blobToString(azBlob.BlobList.ToArray()));

        }

        [TestMethod]
        public void TestBlobbing()
        {
            //run the dependent test
            TestParsing();

            //blobs and then parses and checks that the parsed values are correct
            //if the parse is incorrect, then the blobber is incorrect, since the Parsing function test passes, as it is a dependent test
            
            azBlob.BuildAccelerationBlob(azAccArr, 1,32, 800, 16,true, true);
            Assert.IsTrue(Acceleration.SequenceEquals(azBlob.BlobParser(azBlob.BlobArray), azAccArr));

            cbBlob.BuildAccelerationBlob(cbAccArr, 1, 32, 800, 16, true, true);
            Assert.IsTrue(Acceleration.SequenceEquals(cbBlob.BlobParser(cbBlob.BlobArray), cbAccArr));

            elBlob.BuildAccelerationBlob(elAccArr, 1, 32, 800, 16, true, true);
            Assert.IsTrue(Acceleration.SequenceEquals(elBlob.BlobParser(elBlob.BlobArray), elAccArr));


        }
    }
}
    