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

namespace ControlRoomApplication.Controllers.Communications
{
    public class pushNotification
    {
        public static bool send(String titleText, String bodyText) { return true; }
        public static bool sendEmail(bool b) { return true; }

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
            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);

            return false;
        }
    }
}
