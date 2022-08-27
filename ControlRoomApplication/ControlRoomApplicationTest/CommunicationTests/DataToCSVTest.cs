using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class DataToCSVTest
    {

        [TestInitialize]
        public void TestInit()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderName = "DeleteCSVTestResults";
            string pathString = Path.Combine(currentPath, folderName);
            Directory.CreateDirectory(pathString);
        }

        [TestMethod]
        public void TestConvertDataToCSV()
        {
            string testpath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\DeleteCSVTestResults\test-out-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}")}";
            RFData junk1 = new RFData();
            junk1.Id = 0;
            junk1.appointment_id = 0;
            junk1.time_captured = new DateTime();
            junk1.Intensity = 10000;

            RFData junk2 = new RFData();
            junk2.Id = 1;
            junk2.appointment_id = 0;
            junk2.time_captured = new DateTime();
            junk2.Intensity = 20000;

            List<RFData> JunkRFData = new List<RFData>();
            JunkRFData.Add(junk1);
            JunkRFData.Add(junk2);

            Assert.IsTrue(DataToCSV.ExportToCSV(JunkRFData, testpath));
        }

        [TestMethod]
        public void TestDeleteCSV()
        {
            string testpath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\DeleteCSVTestResults\test-out-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}")}";
            FileStream file = File.Create(testpath);
            file.Close();

            Assert.IsTrue(DataToCSV.DeleteCSVFileWhenDone(testpath));
        }

        [TestCleanup]
        public void Cleanup()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderName = "DeleteCSVTestResults";
            string pathString = Path.Combine(currentPath, folderName);
            Directory.Delete(pathString, true);
        }
    }
}
