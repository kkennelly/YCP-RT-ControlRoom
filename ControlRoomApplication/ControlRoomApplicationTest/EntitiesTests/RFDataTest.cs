using ControlRoomApplication.Entities;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class RFDataTest
    {
        private RFData rfdata;

        private int id;
        private DateTime timeCaptured;
        private long intensity;
        private Appointment appt;
        private int apptId;

        [TestInitialize]
        public void BuildUp()
        {
            rfdata = new RFData();

            id = 0;
            timeCaptured = new DateTime();
            intensity = 0;
            appt = new Appointment();
            apptId = 1;
            appt.Id = apptId;
            DateTime start = DateTime.UtcNow;
            appt.start_time = start;
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            id = 512;
            timeCaptured = new DateTime(2018, 6, 10);
            intensity = 123456789;

            rfdata.Id = id;
            rfdata.time_captured = timeCaptured;
            rfdata.Intensity = intensity;
            rfdata.Appointment = appt;
            rfdata.appointment_id = apptId;

            Assert.AreEqual(id, rfdata.Id);
            Assert.AreEqual(timeCaptured, rfdata.time_captured);
            Assert.AreEqual(intensity, rfdata.Intensity);
            Assert.AreEqual(appt.start_time.Date, rfdata.Appointment.start_time.Date);
        }

        [TestMethod]
        public void TestGenerateFrom()
        {
            SpectraCyberResponse response = new SpectraCyberResponse();
            DateTime date = DateTime.UtcNow;
            response.DateTimeCaptured = date;
            response.DecimalData = 15;
            response.RequestSuccessful = true;
            response.SerialIdentifier = 'c';
            response.Valid = true;

            RFData data = RFData.GenerateFrom(response);

            Assert.IsTrue(data != null);
            Assert.AreEqual(response.DateTimeCaptured, data.time_captured);
            Assert.AreEqual(response.DecimalData, data.Intensity);
        }

        [TestMethod]
        public void TestGenerateListFrom()
        {
            SpectraCyberResponse response1 = new SpectraCyberResponse();
            SpectraCyberResponse response2 = new SpectraCyberResponse();
            SpectraCyberResponse response3 = new SpectraCyberResponse();
            DateTime date1 = DateTime.UtcNow;
            DateTime date2 = date1.AddHours(1);
            DateTime date3 = date2.AddMinutes(30);

            response1.DateTimeCaptured = date1;
            response1.DecimalData = 15;
            response1.RequestSuccessful = true;
            response1.SerialIdentifier = 'c';
            response1.Valid = true;

            response2.DateTimeCaptured = date2;
            response2.DecimalData = 10;
            response2.RequestSuccessful = true;
            response2.SerialIdentifier = 'c';
            response2.Valid = true;

            response3.DateTimeCaptured = date3;
            response3.DecimalData = 1500;
            response3.RequestSuccessful = true;
            response3.SerialIdentifier = 'c';
            response3.Valid = true;

            List<SpectraCyberResponse> responses = new List<SpectraCyberResponse>
            {
                response1,
                response2,
                response3
            };

            List<RFData> rfDatas = RFData.GenerateListFrom(responses);

            Assert.IsTrue(rfDatas != null);

            Assert.IsTrue(rfDatas[0] != null);
            Assert.AreEqual(date1.Date, rfDatas[0].time_captured.Date);
            Assert.AreEqual(response1.DecimalData, rfDatas[0].Intensity);

            Assert.IsTrue(rfDatas[1] != null);
            Assert.AreEqual(date2.Date, rfDatas[1].time_captured.Date);
            Assert.AreEqual(response2.DecimalData, rfDatas[1].Intensity);

            Assert.IsTrue(rfDatas[2] != null);
            Assert.AreEqual(date3.Date, rfDatas[2].time_captured.Date);
            Assert.AreEqual(response3.DecimalData, rfDatas[2].Intensity);
        }
    }
}