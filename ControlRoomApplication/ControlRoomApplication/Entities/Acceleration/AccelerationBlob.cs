using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public abstract class AccelerationBlob
    {
        public abstract int Id { get; set; }

        public abstract List<Byte> BlobList { get; set; }

        public abstract Byte[] BlobArray { get; set; }


        public abstract long FirstTimeCaptured { get; set; }

        private int NumberDataPoints { get; set; }

        public virtual SensorLocationEnum Location { get; set; }

        //takes an acceleration array along with other config data in order to blob together the data into a byte array
        public virtual void BuildAccelerationBlob(Acceleration[] accArray, byte version = 1, byte FIFO_Size = 32, short SampleFrequency = 800, byte GRange = 16, Boolean FullResolution = true,  bool testFlag = false)
        {

            //the switch statement is used for versioning of the blob, currently only one version exists, however; it is easy to add a new version
            //by adding to a new case
            switch (version)
            {
                case 1:
                    if (NumberDataPoints == 0)
                    {
                        //set the First Time Captured
                        FirstTimeCaptured = accArray[0].TimeCaptured;

                        //define the new List of Bytes
                        BlobList = new List<Byte>();

                        //add the version to the start
                        BlobList.Add(version);

                        //add the fifo size to the start
                        BlobList.Add(FIFO_Size);

                        //add the sample frequency to the start
                        Byte[] frequencyBytes = BitConverter.GetBytes(SampleFrequency);
                        for(int i=0; i<frequencyBytes.Length; i++)
                        {
                            BlobList.Add(frequencyBytes[i]);
                        }

                        //add the G Range to the start
                        BlobList.Add(GRange);

                        //add the Full Resolution to the start
                        BlobList.Add(BitConverter.GetBytes(FullResolution)[0]);
                    }

                    foreach(Acceleration acc in accArray)
                    {
                        //if it is the first datapoint from the FIFO dump, add a t Time value as well as the acc data
                        if (NumberDataPoints % FIFO_Size == 0)
                        {
                            //label this part of the blob with t for Time
                            char label = 't';
                            BlobList.Add((byte)label);

                            //add 8 byte timestamp to blob
                            Byte[] time = BitConverter.GetBytes(acc.TimeCaptured);
                            for(int i=0; i<time.Length; i++)
                            {
                                BlobList.Add(time[i]);
                            }

                            //add 2 byte x acceleration
                            Byte[] accX = BitConverter.GetBytes(acc.x);
                            for (int i = 0; i < 2; i++)
                            {
                                BlobList.Add(accX[i]);
                            }

                            //add 2 byte y acceleration
                            Byte[] accY = BitConverter.GetBytes(acc.y);
                            for (int i = 0; i < 2; i++)
                            {
                                BlobList.Add(accY[i]);
                            }

                            //add 2 byte z acceleration
                            Byte[] accZ = BitConverter.GetBytes(acc.z);
                            for (int i = 0; i < 2; i++)
                            {
                                BlobList.Add(accZ[i]);
                            }

                            NumberDataPoints++;

                        }
                        else //otherwise only add acc data
                        {
                            //label this part of the blob with a for just an Acceleration data point
                            char label = 'a';
                            BlobList.Add((byte)label);

                            //add 2 byte x acceleration
                            Byte[] accX = BitConverter.GetBytes(acc.x);
                            for (int i = 0; i < 2; i++)
                            {
                                BlobList.Add(accX[i]);
                            }

                            //add 2 byte y acceleration
                            Byte[] accY = BitConverter.GetBytes(acc.y);
                            for (int i = 0; i < 2; i++)
                            {
                                BlobList.Add(accY[i]);
                            }

                            //add 2 byte z acceleration
                            Byte[] accZ = BitConverter.GetBytes(acc.z);
                            for (int i = 0; i < 2; i++)
                            {
                                BlobList.Add(accZ[i]);
                            }

                            NumberDataPoints++;
                        }

                    }

                    //if the number of datapoints meets or exceeds the prespecified blob size (currently 4000), send the blob to the DB
                    if (NumberDataPoints >= MiscellaneousConstants.BLOB_SIZE)
                    {
                        BlobArray = BlobList.ToArray();
                        Database.DatabaseOperations.AddAccelerationBlobData(this, Location, true);
                        NumberDataPoints = 0;
                    }else if (testFlag) // if it is a test, only send it to an array so as to not mess with the DB
                    {
                        BlobArray = BlobList.ToArray();
                        NumberDataPoints = 0;
                    }

                    break;

                case 2:
                    // if there ever is a second version of the blob, blobify it here
                    //currently no v2 so do nothing
                    break;
            }
        }

        //takes an array of bytes in a given order to be able to parse into acceleration data
        public virtual Acceleration[] BlobParser(Byte[] BlobToParse)
        {
            List<Acceleration> accArrayList = new List<Acceleration>();
            //The blob has been designed in a modular way so that
            //different versions can be blobbed and parsed.  The first 
            //byte is dedicated to the version #
            byte version = BlobToParse[0];

            switch (version)
            {
                case 1:

                    //store teh FIFO size, sample frequency, GRange, and Full resolution for later use
                    byte FIFO_Size = BlobToParse[1];
                    short SampleFrequency = BitConverter.ToInt16(BlobToParse, 2);
                    byte GRange = BlobToParse[4];
                    Boolean FullResolution = BitConverter.ToBoolean(BlobToParse, 5);


                    //loop over the rest of the blob, the first byte in this section will be a label to tell us what 
                    //to parse out of the blob next
                    for (int i = 6; i < BlobToParse.Length;)
                    {
                        char label = (char)BlobToParse[i];
                        i++;
                        Acceleration tempAcc = new Acceleration();

                        switch (label)
                        {
                            case 't':
                                //when the label is t, there is one time value and 3 acceleration values
                                tempAcc.TimeCaptured = BitConverter.ToInt64(BlobToParse, i) * 100;
                                i += 8;
                                tempAcc.x = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.y = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.z = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);
                                tempAcc.location_ID = (int)Location;
                                accArrayList.Add(tempAcc);
                                break;

                            case 'a':
                                //when the label is a, there are 3 acceleration values
                                tempAcc.TimeCaptured = accArrayList[accArrayList.Count - 1].TimeCaptured + ((1000 * 100) / SampleFrequency); /* 1000 / frequency = time offset, then * 100 to save precision */
                                tempAcc.x = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.y = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.z = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);
                                tempAcc.location_ID = (int)Location;
                                accArrayList.Add(tempAcc);
                                break;
                        }

                    }

                    break;

                case 2:
                    //Parse the blob a different way if there is a 2nd version
                    break;

            }

            return accArrayList.ToArray();
        
        }


        //print the blob in binary form, used for visualization in presentations
        public String blobToString(Byte[] blob)
        {

            string blobString = "";
            foreach(byte b in blob)
            {
                string singleByte = Convert.ToString(b, 2).PadLeft(8, '0');
                blobString += singleByte + " ";
            }

            return blobString;
        }


        
    }
}