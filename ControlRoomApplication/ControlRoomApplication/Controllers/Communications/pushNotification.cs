using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using System.Threading;
using System.Collections.Generic;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using ControlRoomApplication.Util;

namespace ControlRoomApplication.Controllers.Communications
{
    public class PushNotification
    {
        private static readonly log4net.ILog logger =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool firebaseAppCreated = false;

        public static bool sendToAllAdmins(String titleText, String bodyText, bool testflag = false)
        {
            if (!firebaseAppCreated)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/ControlRoomApplication/Constants/rtmobile-v2-528c5-firebase-adminsdk-n5z8d-ff80743be4.json")
                });
                firebaseAppCreated = true;
            }
            
            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "specialMessages", "1234" },
                },
                //Token = registrationToken,
                Topic = "admin",
                Notification = new Notification()
                {
                    Title = titleText,
                    Body = bodyText
                }
            };

            // Send a message to the device corresponding to the provided
           // string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
           /*
            if (!testflag)
            {
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);
                logger.Debug(Utilities.GetTimeStamp() + ": Notification sent: " + bodyText);
            }
           */
            return true;
        }
    }
}
