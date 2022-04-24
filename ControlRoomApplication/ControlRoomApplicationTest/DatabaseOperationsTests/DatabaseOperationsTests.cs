using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using System.Threading;
using System.Text;
using ControlRoomApplication.Entities.DiagnosticData;

namespace ControlRoomApplicationTest.DatabaseOperationsTests
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        // Test RFData objects
        private RFData data1;
        private RFData data2;
        private RFData data3;
        private RFData data4;

        // Test Appointment objects
        private Appointment appt;
        private int NumRTInstances = 1;
        private int NumAppointments = 0;
        private int NumRFData = -1;

        [TestInitialize]
        public void BuildUp()
        {
            NumAppointments = DatabaseOperations.GetTotalAppointmentCount();
            NumRTInstances = DatabaseOperations.GetTotalRTCount();
            NumRFData = DatabaseOperations.GetTotalRFDataCount();

            appt = new Appointment();
            appt.start_time = DateTime.UtcNow;
            appt.end_time = DateTime.UtcNow.AddMinutes(1);
            appt._Status = AppointmentStatusEnum.IN_PROGRESS;
            appt._Priority = AppointmentPriorityEnum.MANUAL;
            appt._Type = AppointmentTypeEnum.FREE_CONTROL;
            appt.Coordinates.Add(new Coordinate(0, 0));
            appt.CelestialBody = new CelestialBody();
            appt.CelestialBody.Coordinate = new Coordinate();
            appt.Orientation = new Orientation();
            appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            appt.Telescope = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            appt.Telescope._TeleType = RadioTelescopeTypeEnum.SLIP_RING;
            appt.User = DatabaseOperations.GetControlRoomUser();

            DatabaseOperations.AddRadioTelescope(appt.Telescope);
            DatabaseOperations.AddAppointment(appt);

            // RFData initialization
            data1 = new RFData();
            data2 = new RFData();
            data3 = new RFData();
            data4 = new RFData();
            DateTime now = DateTime.UtcNow;
            DateTime date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            Appointment RFappt = appt;
            data1.Intensity = 9234875;
            data1.TimeCaptured = date;
            data1.Appointment = RFappt;

            data2.Intensity = 8739425;
            data2.TimeCaptured = date.AddSeconds(3);
            data2.Appointment = RFappt;

            data3.Intensity = 12987;
            data3.TimeCaptured = date.AddSeconds(4);
            data3.Appointment = RFappt;

            data4.Intensity = 12987;
            data4.TimeCaptured = date.AddSeconds(5);
            data4.Appointment = RFappt;

            NumAppointments++;
            NumRTInstances++;

        }

        [TestMethod]
        public void TestGetListOfAppointmentsForRadioTelescope()
        {
            var appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id);
            Assert.AreEqual(1, appts.Count);
        }

        [TestMethod]
        public void TestAddAppointment()
        {
            var new_appt = new Appointment();
            new_appt.start_time = DateTime.UtcNow;
            new_appt.end_time = DateTime.UtcNow.AddMinutes(1);
            new_appt._Status = AppointmentStatusEnum.REQUESTED;
            new_appt._Priority = AppointmentPriorityEnum.MANUAL;
            new_appt._Type = AppointmentTypeEnum.POINT;
            new_appt.Coordinates.Add(new Coordinate(15, 15));
            new_appt.CelestialBody = new CelestialBody();
            new_appt.CelestialBody.Coordinate = new Coordinate();
            new_appt.Orientation = new Orientation();
            new_appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            new_appt.Telescope = appt.Telescope;
            new_appt.User = DatabaseOperations.GetControlRoomUser();

            //new_appt.Telescope.type = appt.Telescope.type;

            //DatabaseOperations.AddRadioTelescope(new_appt.Telescope);
            DatabaseOperations.AddAppointment(new_appt);

            var output_appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(new_appt.Telescope.Id);

            Assert.IsTrue(2 == output_appts.Count()); // Should be two appointments retrieved

            //Assert.AreEqual(new_appt.start_time.ToString(), output_appts[0].start_time.ToString());
            //Assert.AreEqual(new_appt.end_time.ToString(), output_appts[0].end_time.ToString());
            Assert.AreEqual(new_appt._Status, output_appts[1]._Status);
            Assert.AreEqual(new_appt._Priority, output_appts[1]._Priority);
            Assert.AreEqual(new_appt._Type, output_appts[1]._Type);

            // Coordinates
            Assert.AreEqual(new_appt.Id, output_appts[1].Coordinates.First().apptId);
            Assert.AreEqual(new_appt.Coordinates.First().hours, output_appts[1].Coordinates.First().hours);
            Assert.AreEqual(new_appt.Coordinates.First().minutes, output_appts[1].Coordinates.First().minutes);
            Assert.AreEqual(new_appt.Coordinates.First().Declination, output_appts[1].Coordinates.First().Declination);
            Assert.AreEqual(new_appt.Coordinates.First().RightAscension, output_appts[1].Coordinates.First().RightAscension);

            // Other entities that Appointment uses
            Assert.AreEqual(new_appt.celestial_body_id, output_appts[1].celestial_body_id);
            Assert.AreEqual(new_appt.orientation_id, output_appts[1].orientation_id);
            Assert.AreEqual(new_appt.spectracyber_config_id, output_appts[1].spectracyber_config_id);
            Assert.AreEqual(new_appt.telescope_id, output_appts[1].telescope_id);
            Assert.AreEqual(new_appt.user_id, output_appts[1].user_id);
        }

        [TestMethod]
        public void TestGetTotalAppointmentCount()
        {
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(NumAppointments, appt_count);
        }

        [TestMethod]
        public void TestCreateRFData()
        {
            DatabaseOperations.AddRFData(data1);
            DatabaseOperations.AddRFData(data2);

            Assert.AreEqual(DatabaseOperations.GetTotalRFDataCount(), NumRFData + 2);

            List<RFData> datas = DatabaseOperations.GetListOfRFData();
            RFData dbData1 = datas.Find(x => x.Id == data1.Id);
            RFData dbData2 = datas.Find(x => x.Id == data2.Id);

            Assert.IsTrue(dbData1 != null);
            Assert.IsTrue(dbData2 != null);

            Assert.AreEqual(dbData1.Intensity, data1.Intensity);
            Assert.AreEqual(dbData2.Intensity, data2.Intensity);

            Assert.IsTrue(dbData1.TimeCaptured == data1.TimeCaptured);
            Assert.IsTrue(dbData2.TimeCaptured == data2.TimeCaptured);

        }

        [TestMethod]
        public void TestCreateRFData_InvalidDate()
        {
            data3.TimeCaptured = DateTime.UtcNow.AddDays(1);

            DatabaseOperations.AddRFData(data3);

            List<RFData> datas = DatabaseOperations.GetListOfRFData();

            RFData testData = appt.RFDatas.ToList().Find(x => x.Id == data3.Id);

            Assert.AreEqual(null, datas.Find(t => t.Id == data3.Id));
        }

        [TestMethod]
        public void TestCreateRFData_InvalidIntensity()
        {
            data4.Intensity = -239458;

            DatabaseOperations.AddRFData(data4);

            List<RFData> datas = DatabaseOperations.GetListOfRFData();

            RFData testData = appt.RFDatas.ToList().Find(x => x.Id == data4.Id);

            Assert.AreEqual(null, datas.Find(t => t.Id == data4.Id));
        }

        [TestMethod]
        public void TestUpdateAppointmentStatus()
        {
            appt._Status = AppointmentStatusEnum.IN_PROGRESS;

            DatabaseOperations.UpdateAppointment(appt);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);
            var testStatus = appt._Status;

            Assert.AreEqual(AppointmentStatusEnum.IN_PROGRESS, testStatus);
        }

        [TestMethod]
        public void TestUpdateAppointmentPriority()
        {
            var expectedPriority = AppointmentPriorityEnum.MANUAL;
            appt._Priority = expectedPriority;

            DatabaseOperations.UpdateAppointment(appt);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);

            var resultPriority = appt._Priority;

            Assert.AreEqual(expectedPriority, resultPriority);
        }

        [TestMethod]
        public void TestUpdateAppointmentCoordinates()
        {
            // Create expected coordinates and add to the appointment
            var coords = new Coordinate(5, 5);
            appt.Coordinates.Add(coords);

            // Add appointment to the database
            DatabaseOperations.UpdateAppointment(appt);

            // Retrieve appointment from the database
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);
            List<Coordinate> resultCoordinates = appt.Coordinates.ToList<Coordinate>();

            // Verify only two results are returned
            Assert.IsTrue(resultCoordinates.Count == 2);

            // Create expected coordinate list
            List<Coordinate> expectedCoordinates = new List<Coordinate>();
            expectedCoordinates.Add(new Coordinate(0, 0));
            expectedCoordinates.Add(new Coordinate(5, 5));

            // Verify coordinates are correct
            Assert.AreEqual(expectedCoordinates[0].Declination, resultCoordinates[0].Declination);
            Assert.AreEqual(expectedCoordinates[0].RightAscension, resultCoordinates[0].RightAscension);

            Assert.AreEqual(expectedCoordinates[1].Declination, resultCoordinates[1].Declination);
            Assert.AreEqual(expectedCoordinates[1].RightAscension, resultCoordinates[1].RightAscension);

        }

        [TestMethod]
        public void TestAddThenDeleteCoordinates()
        {
            // Create expected coordinates and add to the appointment
            var coords = new Coordinate(5, 5);
            appt.Coordinates.Add(coords);

            // Add appointment to the database
            DatabaseOperations.UpdateAppointment(appt);

            Thread.Sleep(100);

            // Delete coordinates and then update the DB entry
            appt.Coordinates.Remove(coords);
            DatabaseOperations.UpdateAppointment(appt);

            // Retrieve appointment from the database
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);
            List<Coordinate> resultCoordinates = appt.Coordinates.ToList<Coordinate>();

            // Verify only one result is returned
            Assert.IsTrue(resultCoordinates.Count == 1);

            // Create expected coordinate
            Coordinate expectedCoordinate = new Coordinate(0, 0);

            // Verify coordinates are correct
            Assert.AreEqual(expectedCoordinate.Declination, resultCoordinates[0].Declination);
            Assert.AreEqual(expectedCoordinate.RightAscension, resultCoordinates[0].RightAscension);
        }

        [TestMethod]
        public void TestGetNextAppointment()
        {
            var nextAppt = DatabaseOperations.GetNextAppointment(appt.Telescope.Id);
            Assert.IsTrue(nextAppt != null);
            Assert.IsTrue(nextAppt._Status != AppointmentStatusEnum.COMPLETED);
        }

        [TestMethod]
        public void TestSetOverrideForSensor()
        {
            bool before = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);

            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.GATE, !before);

            bool after = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);

            Assert.IsTrue(before != after);
        }

        [TestMethod]
        public void TestGetThresholdForSensor()
        {
            double wind = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.WIND);
            Assert.IsTrue(wind > 0);

            double az_motor_temp = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_TEMP);
            Assert.IsTrue(az_motor_temp > 0);

            double elev_motor_temp = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_TEMP);
            Assert.IsTrue(elev_motor_temp > 0);

            double az_motor_vibe = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_VIBRATION);
            Assert.IsTrue(az_motor_vibe > 0);

            double elev_motor_vibe = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_VIBRATION);
            Assert.IsTrue(elev_motor_vibe > 0);

            double az_motor_current = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_CURRENT);
            Assert.IsTrue(az_motor_current > 0);

            double elev_motor_current = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_CURRENT);
            Assert.IsTrue(elev_motor_current > 0);

            double counter_vibe = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.COUNTER_BALANCE_VIBRATION);
            Assert.IsTrue(counter_vibe > 0);

            double amb_temp_high = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP);
            Assert.IsTrue(amb_temp_high > 0);

            double amb_temp_low = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP, false);
            Assert.IsTrue(amb_temp_low > 0);

            double amb_humid_high = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_HUMIDITY);
            Assert.IsTrue(amb_humid_high > 0);

            double amb_humid_low = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_HUMIDITY, false);
            Assert.IsTrue(amb_humid_low > 0);
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            List<User> users = new List<User>();
            users = DatabaseOperations.GetAllUsers();
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void TestGetAllAdminUsers()
        {
            List<User> AdminUsers = new List<User>();

            AdminUsers = DatabaseOperations.GetAllAdminUsers(true);
            Assert.IsTrue(AdminUsers.Count > 0);
            Assert.IsTrue(AdminUsers.All(user => user.UR._User_Role == UserRoleEnum.ADMIN));
        }

        [TestMethod]
        public void TestAddAndRetrieveRadioTelescope()
        {
            RadioTelescope telescope = new RadioTelescope();

            // not saved in the database
            telescope.SpectraCyberController = new SpectraCyberController(new SpectraCyber());
            telescope.PLCDriver = new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false);

            // saved in the database
            telescope.online = 1;
            telescope.CurrentOrientation = new Orientation(25, 25);
            telescope.CalibrationOrientation = new Orientation(30, 30);
            telescope.Location = new Location(1, 2, 3, "test");
            telescope._TeleType = RadioTelescopeTypeEnum.SLIP_RING;
            telescope.maxElevationDegrees = MiscellaneousConstants.MAX_SOFTWARE_STOP_EL_DEGREES;
            telescope.minElevationDegrees = MiscellaneousConstants.MIN_SOFTWARE_STOP_EL_DEGREES;

            DatabaseOperations.AddRadioTelescope(telescope);

            RadioTelescope retrievedTele = DatabaseOperations.FetchLastRadioTelescope();
            RadioTelescope teleByID = DatabaseOperations.FetchRadioTelescopeByID(retrievedTele.Id);


            // online
            Assert.IsTrue(telescope.online == retrievedTele.online);

            // type
            Assert.IsTrue(telescope.teleType == retrievedTele.teleType);

            // location
            Assert.IsTrue(telescope.Location.Latitude == retrievedTele.Location.Latitude);
            Assert.IsTrue(telescope.Location.Longitude == retrievedTele.Location.Longitude);
            Assert.IsTrue(telescope.Location.Altitude == retrievedTele.Location.Altitude);
            Assert.IsTrue(telescope.Location.Name == retrievedTele.Location.Name);

            // current orientation (not yet implemented)
            Assert.IsTrue(telescope.CurrentOrientation.Azimuth == retrievedTele.CurrentOrientation.Azimuth);
            Assert.IsTrue(telescope.CurrentOrientation.Elevation == retrievedTele.CurrentOrientation.Elevation);

            // calibration orientation (not yet implemented)
            Assert.IsTrue(telescope.CalibrationOrientation.Azimuth == retrievedTele.CalibrationOrientation.Azimuth);
            Assert.IsTrue(telescope.CalibrationOrientation.Elevation == retrievedTele.CalibrationOrientation.Elevation);

            // elevation thresholds
            Assert.IsTrue(telescope.maxElevationDegrees == retrievedTele.maxElevationDegrees);
            Assert.IsTrue(telescope.minElevationDegrees == retrievedTele.minElevationDegrees);

            // test FetchByID
            // we will never have this many telescopes, just ensuring null operation performed correctly
            Assert.IsTrue(DatabaseOperations.FetchRadioTelescopeByID(32323232) == null);

            Assert.IsFalse(teleByID == null);
            Assert.IsFalse(teleByID.Location == null);
            Assert.IsFalse(teleByID.CalibrationOrientation == null);
            Assert.IsFalse(teleByID.CurrentOrientation == null);
            Assert.IsTrue(teleByID.Id == retrievedTele.Id);
            Assert.IsTrue(teleByID.Location.Latitude == retrievedTele.Location.Latitude);
            Assert.IsTrue(teleByID.Location.Longitude == retrievedTele.Location.Longitude);
            Assert.IsTrue(teleByID.Location.Altitude == retrievedTele.Location.Altitude);
            Assert.IsTrue(teleByID.Location.Name == retrievedTele.Location.Name);
            Assert.IsTrue(teleByID.CurrentOrientation.Azimuth == retrievedTele.CurrentOrientation.Azimuth);
            Assert.IsTrue(teleByID.CurrentOrientation.Elevation == retrievedTele.CurrentOrientation.Elevation);
            Assert.IsTrue(teleByID.CalibrationOrientation.Azimuth == retrievedTele.CalibrationOrientation.Azimuth);
            Assert.IsTrue(teleByID.CalibrationOrientation.Elevation == retrievedTele.CalibrationOrientation.Elevation);
            Assert.IsTrue(teleByID.maxElevationDegrees == retrievedTele.maxElevationDegrees);
            Assert.IsTrue(teleByID.minElevationDegrees == retrievedTele.minElevationDegrees);
        }

        [TestMethod]
        public void TestAddAndRetrieveTemperature()
        {
            Temperature[] temp = new Temperature[1];
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Temperature
            Temperature t1 = Temperature.Generate(dateTime, 0.0, loc1);

            temp[0] = (t1);

            DatabaseOperations.AddSensorData(temp, true);
            List<Temperature> tempReturn = DatabaseOperations.GetTEMPData(dateTime - 1, dateTime + 1, loc1);

            Assert.AreEqual(tempReturn.Count, 1);

            //Test only temp
            Assert.AreEqual(temp[tempReturn.Count - 1].location_ID, tempReturn[tempReturn.Count - 1].location_ID);
            Assert.AreEqual(temp[tempReturn.Count - 1].temp, tempReturn[tempReturn.Count - 1].temp);
            Assert.AreEqual(temp[tempReturn.Count - 1].TimeCapturedUTC, tempReturn[tempReturn.Count - 1].TimeCapturedUTC);

        }

        [TestMethod]
        public void TestAddAndRetrieveTemperatures()
        {
            Temperature[] temp = new Temperature[2];
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Make 2 new temperatures
            Temperature t1 = Temperature.Generate(dateTime, 500.0, loc1);
            Temperature t2 = Temperature.Generate(dateTime, 999.0, loc1);

            temp[0] = (t1);
            temp[1] = (t2);

            DatabaseOperations.AddSensorData(temp, true);
            List<Temperature> tempReturn = DatabaseOperations.GetTEMPData(dateTime - 1, dateTime + 1, loc1);

            Assert.AreEqual(tempReturn.Count, 2);

            //Test first temp
            Assert.AreEqual(temp[tempReturn.Count - 1].location_ID, tempReturn[tempReturn.Count - 1].location_ID);
            Assert.AreEqual(temp[tempReturn.Count - 1].temp, tempReturn[tempReturn.Count - 1].temp);
            Assert.AreEqual(temp[tempReturn.Count - 1].TimeCapturedUTC, tempReturn[tempReturn.Count - 1].TimeCapturedUTC);

            //Test second temp
            Assert.AreEqual(temp[tempReturn.Count - 2].location_ID, tempReturn[tempReturn.Count - 2].location_ID);
            Assert.AreEqual(temp[tempReturn.Count - 2].temp, tempReturn[tempReturn.Count - 2].temp);
            Assert.AreEqual(temp[tempReturn.Count - 2].TimeCapturedUTC, tempReturn[tempReturn.Count - 2].TimeCapturedUTC);
        }

        [TestMethod]
        public void TestAddAndRetrieveHumidity()
        {
            Humidity[] humidity = new Humidity[1];
            SensorLocationEnum loc1 = SensorLocationEnum.EL_FRAME;

            //Generate current time
            long dateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Humidity
            Humidity h1 = Humidity.Generate(dateTime, 0.0, loc1);

            humidity[0] = (h1);

            DatabaseOperations.AddSensorData(humidity, true);
            List<Humidity> humidityReturn = DatabaseOperations.GetHumidityData(dateTime - 1, dateTime + 1, loc1);

            Assert.AreEqual(humidityReturn.Count, 1);

            //Test only humidity
            Assert.AreEqual(humidity[humidityReturn.Count - 1].LocationID, humidityReturn[humidityReturn.Count - 1].LocationID);
            Assert.AreEqual(humidity[humidityReturn.Count - 1].HumidityReading, humidityReturn[humidityReturn.Count - 1].HumidityReading);
            Assert.AreEqual(humidity[humidityReturn.Count - 1].TimeCapturedUTC, humidityReturn[humidityReturn.Count - 1].TimeCapturedUTC);
        }

        [TestMethod]
        public void TestAddAndRetrieveMultipleHumidity()
        {
            Humidity[] humidity = new Humidity[2];
            SensorLocationEnum loc1 = SensorLocationEnum.EL_FRAME;

            //Generate current time
            long dateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Make 2 new humidity
            Humidity h1 = Humidity.Generate(dateTime, 500.0, loc1);
            Humidity h2 = Humidity.Generate(dateTime, 999.0, loc1);

            humidity[0] = (h1);
            humidity[1] = (h2);

            DatabaseOperations.AddSensorData(humidity, true);
            List<Humidity> humidityReturn = DatabaseOperations.GetHumidityData(dateTime - 1, dateTime + 1, loc1);

            Assert.AreEqual(humidityReturn.Count, 2);

            //Test first temp
            Assert.AreEqual(humidity[humidityReturn.Count - 1].LocationID, humidityReturn[humidityReturn.Count - 1].LocationID);
            Assert.AreEqual(humidity[humidityReturn.Count - 1].HumidityReading, humidityReturn[humidityReturn.Count - 1].HumidityReading);
            Assert.AreEqual(humidity[humidityReturn.Count - 1].TimeCapturedUTC, humidityReturn[humidityReturn.Count - 1].TimeCapturedUTC);

            //Test second temp
            Assert.AreEqual(humidity[humidityReturn.Count - 2].LocationID, humidityReturn[humidityReturn.Count - 2].LocationID);
            Assert.AreEqual(humidity[humidityReturn.Count - 2].HumidityReading, humidityReturn[humidityReturn.Count - 2].HumidityReading);
            Assert.AreEqual(humidity[humidityReturn.Count - 2].TimeCapturedUTC, humidityReturn[humidityReturn.Count - 2].TimeCapturedUTC);

        }

        //Acceleration
        [TestMethod]
        public void TestAddAndRetrieveAcceleration()
        {
            Acceleration[] acc = new Acceleration[1];
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Acceleration
            Acceleration a1 = Acceleration.Generate(dateTime1, 1, 1, 1, loc1);

            acc[0] = a1;

            DatabaseOperations.AddSensorData(acc, true);
            List<Acceleration> accReturn = DatabaseOperations.GetACCData(dateTime1 - 1, dateTime1 + 1, loc1);

            Assert.AreEqual(accReturn.Count, 1);

            //Test only acc
            Assert.AreEqual(acc[accReturn.Count - 1].location_ID, accReturn[accReturn.Count - 1].location_ID);
            Assert.AreEqual(acc[accReturn.Count - 1].x, accReturn[accReturn.Count - 1].x);
            Assert.AreEqual(acc[accReturn.Count - 1].y, accReturn[accReturn.Count - 1].y);
            Assert.AreEqual(acc[accReturn.Count - 1].z, accReturn[accReturn.Count - 1].z);
            Assert.AreEqual(acc[accReturn.Count - 1].TimeCaptured, accReturn[accReturn.Count - 1].TimeCaptured);

        }

        [TestMethod]
        public void TestAddAndRetrieveAccelerations()
        {
            Acceleration[] acc = new Acceleration[2];
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Make 2 new Accelerations
            Acceleration a1 = Acceleration.Generate(dateTime1, 1, 1, 1, loc1);
            Acceleration a2 = Acceleration.Generate(dateTime1, 2, 2, 2, loc1);

            acc[0] = (a1);
            acc[1] = (a2);


            DatabaseOperations.AddSensorData(acc, true);
            List<Acceleration> accReturn = DatabaseOperations.GetACCData(dateTime1 - 1, dateTime1 + 1, loc1);

            Assert.AreEqual(accReturn.Count, 2);

            //Test first acc
            Assert.AreEqual(acc[accReturn.Count - 1].location_ID, accReturn[accReturn.Count - 1].location_ID);
            Assert.AreEqual(acc[accReturn.Count - 1].x, accReturn[accReturn.Count - 1].x);
            Assert.AreEqual(acc[accReturn.Count - 1].y, accReturn[accReturn.Count - 1].y);
            Assert.AreEqual(acc[accReturn.Count - 1].z, accReturn[accReturn.Count - 1].z);
            Assert.AreEqual(acc[accReturn.Count - 1].TimeCaptured, accReturn[accReturn.Count - 1].TimeCaptured);

            //Test second acc
            Assert.AreEqual(acc[accReturn.Count - 2].location_ID, accReturn[accReturn.Count - 2].location_ID);
            Assert.AreEqual(acc[accReturn.Count - 2].x, accReturn[accReturn.Count - 2].x);
            Assert.AreEqual(acc[accReturn.Count - 2].y, accReturn[accReturn.Count - 2].y);
            Assert.AreEqual(acc[accReturn.Count - 2].z, accReturn[accReturn.Count - 2].z);
            Assert.AreEqual(acc[accReturn.Count - 2].TimeCaptured, accReturn[accReturn.Count - 2].TimeCaptured);

        }

        [TestMethod]
        public void TestUpdateTelescope()
        {
            RadioTelescope telescope = new RadioTelescope();

            // saved in the database
            telescope.online = 0;
            telescope.CurrentOrientation = new Orientation(0, 0);
            telescope.CalibrationOrientation = new Orientation(0, 0);
            telescope.Location = new Location(0, 0, 0, "");
            telescope._TeleType = RadioTelescopeTypeEnum.NONE;
            telescope.maxElevationDegrees = MiscellaneousConstants.MAX_SOFTWARE_STOP_EL_DEGREES;
            telescope.minElevationDegrees = MiscellaneousConstants.MIN_SOFTWARE_STOP_EL_DEGREES;
            DatabaseOperations.AddRadioTelescope(telescope);
            RadioTelescope retrievedTele = DatabaseOperations.FetchLastRadioTelescope();


            // not saved in the database
            telescope.SpectraCyberController = new SpectraCyberController(new SpectraCyber());
            telescope.PLCDriver = new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false);

            // saved in the database
            telescope.Id = retrievedTele.Id;
            telescope.online = 1;
            telescope.CurrentOrientation = new Orientation(25, 25);
            telescope.CalibrationOrientation = new Orientation(30, 30);
            telescope.Location = new Location(1, 2, 3, "test");
            telescope._TeleType = RadioTelescopeTypeEnum.SLIP_RING;
            telescope.maxElevationDegrees = 90;
            telescope.minElevationDegrees = 0;

            DatabaseOperations.UpdateTelescope(telescope);

            retrievedTele = DatabaseOperations.FetchLastRadioTelescope();

            // online
            Assert.IsTrue(telescope.online == retrievedTele.online);

            // type
            Assert.IsTrue(telescope.teleType == retrievedTele.teleType);

            // location
            Assert.IsTrue(telescope.Location.Latitude == retrievedTele.Location.Latitude);
            Assert.IsTrue(telescope.Location.Longitude == retrievedTele.Location.Longitude);
            Assert.IsTrue(telescope.Location.Altitude == retrievedTele.Location.Altitude);
            Assert.IsTrue(telescope.Location.Name == retrievedTele.Location.Name);

            // current orientation (not yet implemented)
            Assert.IsTrue(telescope.CurrentOrientation.Azimuth == retrievedTele.CurrentOrientation.Azimuth);
            Assert.IsTrue(telescope.CurrentOrientation.Elevation == retrievedTele.CurrentOrientation.Elevation);

            // calibration orientation (not yet implemented)
            Assert.IsTrue(telescope.CalibrationOrientation.Azimuth == retrievedTele.CalibrationOrientation.Azimuth);
            Assert.IsTrue(telescope.CalibrationOrientation.Elevation == retrievedTele.CalibrationOrientation.Elevation);

            // elevation thresholds
            Assert.IsTrue(telescope.maxElevationDegrees == retrievedTele.maxElevationDegrees);
            Assert.IsTrue(telescope.minElevationDegrees == retrievedTele.minElevationDegrees);
        }

        [TestMethod]
        public void TestAddAndRetrieveSensorNetworkConfig_Valid_CreatesConfig()
        {
            int telescopeId = 5;

            // Create new SensorNetworkConfig with a telescope ID of 5
            SensorNetworkConfig original = new SensorNetworkConfig(telescopeId);

            original.TimeoutDataRetrieval = 5;
            original.TimeoutInitialization = 5;

            DatabaseOperations.AddSensorNetworkConfig(original);

            var retrieved = DatabaseOperations.RetrieveSensorNetworkConfigByTelescopeId(telescopeId);

            Assert.IsTrue(original.Equals(retrieved));

            // Delete config
            DatabaseOperations.DeleteSensorNetworkConfig(original);
        }

        [TestMethod]
        public void TestUpdateSensorNetworkConfig_ChangeAllFields_UpdatesConfig()
        {
            int telescopeId = 5;
            SensorNetworkConfig original = new SensorNetworkConfig(telescopeId);

            // Save original config
            DatabaseOperations.AddSensorNetworkConfig(original);

            // Change values so the updated one is different
            original.ElevationTemp1Init = false;
            original.AzimuthTemp1Init = false;
            original.ElevationAccelerometerInit = false;
            original.AzimuthAccelerometerInit = false;
            original.CounterbalanceAccelerometerInit = false;
            original.ElevationEncoderInit = false;
            original.AzimuthEncoderInit = false;
            original.TimeoutDataRetrieval = 5;
            original.TimeoutInitialization = 5;

            // Update config
            DatabaseOperations.UpdateSensorNetworkConfig(original);

            var retrieved = DatabaseOperations.RetrieveSensorNetworkConfigByTelescopeId(telescopeId);

            Assert.IsTrue(original.Equals(retrieved));

            // Delete config
            DatabaseOperations.DeleteSensorNetworkConfig(original);
        }

        [TestMethod]
        public void TestUpdateSensorNetworkConfig_TelescopeDoesntExist_ShouldThrowInvalidOperationException()
        {
            SensorNetworkConfig invalidConfig = new SensorNetworkConfig(9000);

            Assert.ThrowsException<InvalidOperationException>(() =>
                DatabaseOperations.UpdateSensorNetworkConfig(invalidConfig)
            );
        }

        [TestMethod]
        public void TestDeleteSensorNetworkConfig_ConfigExists_DeletesConfig()
        {
            int telescopeId = 10;
            SensorNetworkConfig config = new SensorNetworkConfig(telescopeId);

            // Save config
            DatabaseOperations.AddSensorNetworkConfig(config);

            // Delete config
            DatabaseOperations.DeleteSensorNetworkConfig(config);

            // Attempt to find config
            SensorNetworkConfig result = DatabaseOperations.RetrieveSensorNetworkConfigByTelescopeId(telescopeId);

            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void TestDeleteSensorNetworkConfig_TelescopeDoesntExist_ShouldThrowInvalidOperationException()
        {
            SensorNetworkConfig invalidConfig = new SensorNetworkConfig(9000);

            Assert.ThrowsException<InvalidOperationException>(() =>
                DatabaseOperations.DeleteSensorNetworkConfig(invalidConfig)
            );
        }

        [TestMethod]
        public void testAddAndFetchWeatherStation()
        {
            WeatherThreshold testThreshold = new WeatherThreshold(10, 120);
            DatabaseOperations.AddWeatherThreshold(testThreshold);
            int time = DatabaseOperations.FetchWeatherThreshold().SnowDumpTime;
            Assert.AreEqual(120, time);

        }

        [TestMethod]
        public void testGetSensorStatus()
        {
            DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM,
                SensorStatusEnum.ALARM, SensorStatusEnum.ALARM, SensorStatusEnum.ALARM, SensorStatusEnum.ALARM, SensorStatusEnum.ALARM,
                SensorStatusEnum.ALARM, SensorStatusEnum.ALARM, SensorStatusEnum.ALARM, SensorStatusEnum.ALARM, SensorStatusEnum.ALARM,
                SensorStatusEnum.ALARM, SensorStatusEnum.ALARM));
            SensorStatus status = null;
            status = DatabaseOperations.GetSensorStatusData();
            Assert.AreEqual(status.gate, (SByte)SensorStatusEnum.NORMAL);
        }

        [TestMethod]
        public void TestUpdateSensorThreshold_ChangeAllFields()
        {
            ThresholdValues original = new ThresholdValues();

            // Save original threshold
            double upper = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP);
            double lower = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP, false);

            original.minValue = (float)lower;
            original.maxValue = (float)upper;
            original.sensor_name = SensorItemEnum.AMBIENT_TEMP.ToString();

            ThresholdValues changed = new ThresholdValues();
            changed.minValue = 20;
            changed.maxValue = 50;
            changed.sensor_name = SensorItemEnum.AMBIENT_TEMP.ToString();

            // Update threshold
            DatabaseOperations.UpdateSensorThreshold(changed);

            Assert.AreEqual(DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP), changed.maxValue);
            Assert.AreEqual(DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP, false), changed.minValue);

            // Revert threshold
            DatabaseOperations.UpdateSensorThreshold(original);
        }

        [TestMethod]
        public void TestUpdateSensorThreshold_IdDoesntExist()
        {
            ThresholdValues invalidThreshold = new ThresholdValues();
            
            // Give the threshold a sensor name that will never exist
            invalidThreshold.sensor_name = SensorItemEnum.GATE.ToString();

            Assert.ThrowsException<InvalidOperationException>(() =>
                DatabaseOperations.UpdateSensorThreshold(invalidThreshold)
            );
        }
    }
}
