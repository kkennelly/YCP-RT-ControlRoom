using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Util;
using System.Windows.Forms;
using ControlRoomApplication.Controllers.Communications;

namespace ControlRoomApplication.Util
{
    
    public class Utilities
    {
        public static object timeZoneName;

        public static String GetTimeStamp()
        {
            string timeZone = string.Empty;
            string timeZoneString = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now)
                                      ? TimeZone.CurrentTimeZone.StandardName
                                      : TimeZone.CurrentTimeZone.DaylightName;
            
            string[] timeZoneWords = timeZoneString.Split(' ');
            foreach (string timeZoneWord in timeZoneWords)
            {
                if (timeZoneWord[0] != '(')
                {
                    timeZone += timeZoneWord[0];
                }
                else
                {
                    timeZone += timeZoneWord;
                }
            }

            timeZone = timeZone.ToUpper().Trim();
            string dateTime = (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + timeZone);

            return dateTime;
        }

        /// <summary>
        /// This allows us to write to the user interface from a thread other than the thread that created
        /// the user interface.
        /// </summary>
        /// <typeparam name="T">The form class name.</typeparam>
        /// <param name="writeTo">What form class object.</param>
        /// <param name="codeBlock">The code that changes the UI component.</param>
        public static void WriteToGUIFromThread<T>(T writeTo, Action codeBlock) where T:Form
        {
            if (writeTo.InvokeRequired)
            {
                IAsyncResult result = writeTo.BeginInvoke(new MethodInvoker(delegate ()
                {
                    codeBlock();
                }));
            }
            else if (writeTo.IsHandleCreated)
            {
                codeBlock();
            }
        }

        /// <summary>
        /// Checks if the command sent is using a version of TCP that uses encryption. If so, it decrypts and return the command to be parsed like normal 
        /// If the version of TCP uses encryption, then we must also encrypt the data sent back to the mobile app, hence we change the encrypted flag to true 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A tuple that contains the either decrypted or unmodified string and a boolean that says whether or not the string was encrypted.</returns>
        public static Tuple<string, bool> CheckEncrypted(string data)
        {
            bool encrypted = false;
            double versionNum;

            if (Double.TryParse(data.Substring(0, data.IndexOf('|')), out versionNum) && versionNum >= 1.1)
            {
                // Set the instances encrypted bool to true
                encrypted = true;

                // Decrypt the command
                data = AES.Decrypt(data.Substring(data.IndexOf('|') + 1), AESConstants.KEY, AESConstants.IV);
            }

            return new Tuple<string, bool>(data, encrypted);
        }
    }
}

