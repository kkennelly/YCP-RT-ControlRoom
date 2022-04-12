using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class EmailNotificationTests
    {

        public static string testfileloc;

        [TestInitialize]
        public void TestInit()
        {
            testfileloc = $"test-out-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}";
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderName = "PushNotificationTestResults";
            string pathString = Path.Combine(currentPath, folderName);
            Directory.CreateDirectory(pathString);
        }

        [TestMethod]
        public void TestSendingEmailToAllAdmins()
        {
            string sender = "SystemTest@ycpradiotelescope.com";
            string subject = "Amazon SES Test";
            string body = "AmazonSES Test (.NET)\r\nThis email was sent through AmazonSES using the AWS SDK for .NET.";

            // Execute task
            Task<bool> task = EmailNotifications.sendToAllAdmins(subject, body, sender, true);

            // Wait for main task to finish before assertion
            task.Wait();

            Assert.IsTrue(task.Result);
        }

        [TestMethod]
        public void TestSendingEmailToUser()
        {
            string sender = "SystemTest@ycpradiotelescope.com";
            string subject = "Amazon SES Test";
            string body = "AmazonSES Test (.NET)\r\nThis email was sent through AmazonSES using the AWS SDK for .NET.";
            User fakeUser = new User("Test", "User", "testradiotelescopeuser@ycp.edu", NotificationTypeEnum.ALL);

            // Execute task
            Task<bool> task = EmailNotifications.sendToUser(fakeUser, subject, body, sender, null, true);

            // Wait for main task to finish before assertion
            task.Wait();

            Assert.IsTrue(task.Result);
        }

        [TestMethod]
        public void TestSendingEmailWithAttachment()
        {
            string testpath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\PushNotificationTestResults\test-out-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}")}";

            string sender = "SystemTest@ycpradiotelescope.com";
            string subject = "Amazon SES Test WITH ATTACHMENT";
            string body = "AmazonSES Test (.NET) with Attachment\r\nThis email and its attachment were sent through AmazonSES using the AWS SDK for .NET.";

            // If you want to actually get emails while testing, change the email address below to whatever one you want to receive at.
            // This was already done earlier.
            User fakeUser = new User("Test", "User", "testradiotelescopeuser@ycp.edu", NotificationTypeEnum.ALL);

            // Gather dummy data
            RFData junkdata = new RFData();
            junkdata.Id = 0;
            junkdata.appointment_id = 0;
            junkdata.TimeCaptured = System.DateTime.Now;
            junkdata.Intensity = 8675309;

            List<RFData> JunkRFData = new List<RFData>();
            JunkRFData.Add(junkdata);

            DataToCSV.ExportToCSV(JunkRFData, testpath);

            // List of "attachments"
            List<string> attachmentPath = new List<string>();
            attachmentPath.Add($"{testpath}.csv");

            // Execute task
            Task<bool> task = EmailNotifications.sendToUser(fakeUser, subject, body, sender, attachmentPath, true);

            // Wait for main task to finish before assertion
            task.Wait();
            
            Assert.IsTrue(task.Result);
        }

        [TestCleanup]
        public void Cleanup()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderName = "PushNotificationTestResults";
            string pathString = Path.Combine(currentPath, folderName);

            Directory.Delete(pathString, true);
        }
    }
}
