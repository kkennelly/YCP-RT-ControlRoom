using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Util;

namespace ControlRoomApplication.UtilitiesTests
{

    [TestClass]
    public class UtilitiesTests { 
    
        
        [TestMethod]
        public void TestGetTimeStamp()
        {
            string timeStamp = Utilities.GetTimeStamp();
            bool timeZone = false;

            // Allow for timestamps containing both EST or EDT, rather than just EST
            if(timeStamp.Contains("EST") || timeStamp.Contains("EDT"))
            {
                timeZone = true;
            }
            
            Assert.IsTrue(timeZone);

        }

        [TestMethod]
        public void TestCheckEncrypted_NotEncrypted()
        {
            string receivedCommand = "1.0|efjoaifjwaovn894q9q2j8qv48qv4g4q3gq38hg93";

            Tuple<string, bool> dataPair = Utilities.CheckEncrypted(receivedCommand);

            Assert.IsTrue(receivedCommand.Equals(dataPair.Item1));
            Assert.IsFalse(dataPair.Item2);
        }
    }
}

