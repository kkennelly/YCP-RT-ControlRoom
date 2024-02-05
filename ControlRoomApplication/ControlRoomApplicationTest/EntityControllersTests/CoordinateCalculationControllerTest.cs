using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class CoordinateCalculationControllerTest
    {
        [TestInitialize]
        public void Up()
        {
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;
            CoordinateCalculationController = new CoordinateCalculationController(location);
        }

        [TestMethod]
        public void TestCoordinateToOrientation()
        {
            // Test at 10:01:43 PM local time (Converted to UTC) 
            DateTime date_1 = new DateTime(2018, 6, 23, 2, 1, 43);
            Coordinate coord_1 = new Coordinate(1, 1);
            Orientation test_orientation_1 = new Orientation(40.81, -40.75);
            Orientation output_orientation_1 = CoordinateCalculationController.CoordinateToOrientation(coord_1, date_1);

            Assert.AreEqual(test_orientation_1.azimuth, output_orientation_1.azimuth, 0.5);
            Assert.AreEqual(test_orientation_1.elevation, output_orientation_1.elevation, 0.5);

            // Test at 6:00:00 AM local time (Converted to UTC)
            DateTime date_2 = new DateTime(2018, 12, 14, 11, 0, 0);
            Coordinate coord_2 = new Coordinate(12, 1);
            Orientation test_orientation_2 = new Orientation(166.09, 50.04);
            Orientation output_orientation_2 = CoordinateCalculationController.CoordinateToOrientation(coord_2, date_2);

            Assert.AreEqual(test_orientation_2.azimuth, output_orientation_2.azimuth, 0.5);
            Assert.AreEqual(test_orientation_2.elevation, output_orientation_2.elevation, 0.5);

            // Test at 5:55:02 PM local time (Converted to UTC)
            DateTime date_3 = new DateTime(2018, 2, 3, 22, 55, 2);
            Coordinate coord_3 = new Coordinate(23, 80);
            Orientation test_orientation_3 = new Orientation(348.36, 45.00);
            Orientation output_orientation_3 = CoordinateCalculationController.CoordinateToOrientation(coord_3, date_3);

            Assert.AreEqual(test_orientation_3.azimuth, output_orientation_3.azimuth, 0.5);
            Assert.AreEqual(test_orientation_3.elevation, output_orientation_3.elevation, 0.5);
        }

        [TestMethod]
        public void TestOrientationToCoordinate()
        {
            // Test at 10:01:43 PM local time (Converted to UTC) 
            DateTime date_1 = new DateTime(2018, 6, 23, 2, 1, 43);
            Coordinate coord_1 = new Coordinate(1, 1);
            Orientation test_orientation_1 = new Orientation(40.81, -40.75);
            Orientation output_orientation_1 = CoordinateCalculationController.CoordinateToOrientation(coord_1, date_1);
            Coordinate output_coordinate_1 = CoordinateCalculationController.OrientationToCoordinate(output_orientation_1, date_1);

            Assert.AreEqual(coord_1.right_ascension, output_coordinate_1.right_ascension, 0.5);
            Assert.AreEqual(coord_1.declination, output_coordinate_1.declination, 0.5);

            // Test at 6:00:00 AM local time (Converted to UTC)
            DateTime date_2 = new DateTime(2018, 12, 14, 11, 0, 0);
            Coordinate coord_2 = new Coordinate(12, 1);
            Orientation test_orientation_2 = new Orientation(166.09, 50.04);
            Orientation output_orientation_2 = CoordinateCalculationController.CoordinateToOrientation(coord_2, date_2);
            Coordinate output_coordinate_2 = CoordinateCalculationController.OrientationToCoordinate(output_orientation_2, date_2);

            Assert.AreEqual(coord_2.right_ascension, output_coordinate_2.right_ascension, 0.5);
            Assert.AreEqual(coord_2.declination, output_coordinate_2.declination, 0.5);

            // Test at 5:55:02 PM local time (Converted to UTC)
            DateTime date_3 = new DateTime(2018, 2, 3, 22, 55, 2);
            Coordinate coord_3 = new Coordinate(23, 80);
            Orientation test_orientation_3 = new Orientation(348.36, 45.00);
            Orientation output_orientation_3 = CoordinateCalculationController.CoordinateToOrientation(coord_3, date_3);
            Coordinate output_coordinate_3 = CoordinateCalculationController.OrientationToCoordinate(output_orientation_3, date_3);

            Assert.AreEqual(coord_3.right_ascension, output_coordinate_3.right_ascension, 0.5);
            Assert.AreEqual(coord_3.declination, output_coordinate_3.declination, 0.5);
        }

        [TestMethod]
        public void TestCalculateOrientation_PointAppointment()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);

            // Test point appointment
            Appointment point_appt = new Appointment();
            point_appt._Type = AppointmentTypeEnum.POINT;
            point_appt._Status = AppointmentStatusEnum.REQUESTED;
            point_appt.start_time = start;
            point_appt.end_time = end;
            point_appt.Coordinates.Add(new Coordinate(0, 0));
            var point_orientation = CoordinateCalculationController.CalculateOrientation(point_appt, start);

            Assert.IsTrue(point_orientation != null);
        }

        [TestMethod]
        public void TestCalculateOrientation_CelestialBodyAppointment()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);

            // Test celesital body appointment
            Appointment sun_appt = new Appointment();
            sun_appt._Type = AppointmentTypeEnum.CELESTIAL_BODY;
            sun_appt._Status = AppointmentStatusEnum.REQUESTED;
            sun_appt.start_time = start;
            sun_appt.end_time = end;
            sun_appt.CelestialBody = new CelestialBody(CelestialBodyConstants.SUN);
            var sun_orientation = CoordinateCalculationController.CalculateOrientation(sun_appt, start);

            Assert.IsTrue(sun_orientation != null);
        }

        [TestMethod]
        public void TestCalculateOrientation_RasterAppointment()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);

            // Test raster appointment
            Appointment raster_appt = new Appointment();
            raster_appt._Type = AppointmentTypeEnum.RASTER;
            raster_appt._Status = AppointmentStatusEnum.REQUESTED;
            raster_appt.start_time = start;
            raster_appt.end_time = end;
            raster_appt.Coordinates.Add(new Coordinate(0, 0));
            raster_appt.Coordinates.Add(new Coordinate(5, 5));
            var raster_orientation = CoordinateCalculationController.CalculateOrientation(raster_appt, start);

            Assert.IsTrue(raster_orientation != null);
        }

        [TestMethod]
        public void TestCalculateOrientation_DriftScanAppointment()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);

            // Test drift scan appointment
            Appointment drift_scan_appt = new Appointment();
            drift_scan_appt._Type = AppointmentTypeEnum.DRIFT_SCAN;
            drift_scan_appt._Status = AppointmentStatusEnum.REQUESTED;
            drift_scan_appt.start_time = start;
            drift_scan_appt.end_time = end;
            drift_scan_appt.Orientation = new Orientation(30, 30);
            var orientation_orientation = CoordinateCalculationController.CalculateOrientation(drift_scan_appt, start);

            Assert.IsTrue(orientation_orientation != null);
        }

        [TestMethod]
        public void TestCalculateOrientation_FreeControlAppointment()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);

            // Test free control appointment
            Appointment free_control_appt = new Appointment();
            free_control_appt._Type = AppointmentTypeEnum.FREE_CONTROL;
            free_control_appt._Status = AppointmentStatusEnum.REQUESTED;
            free_control_appt.start_time = start;
            free_control_appt.end_time = end;
            free_control_appt.Orientation = new Orientation(30, 30);
            free_control_appt._Priority = AppointmentPriorityEnum.MANUAL;
            var free_control_orientation_1 = CoordinateCalculationController.CalculateOrientation(free_control_appt, start);

            Assert.IsTrue(free_control_orientation_1 != null);
            Assert.IsTrue(free_control_appt.Orientation == null);

            free_control_appt.Coordinates.Add(new Coordinate(0, 0));
            var free_control_orientation_2 = CoordinateCalculationController.CalculateOrientation(free_control_appt, end);

            Assert.IsTrue(free_control_orientation_2 != null);
            Assert.IsTrue(free_control_appt.Coordinates.Count == 0);
        }

        [TestMethod]
        public void TestGetPointCoordinate()
        {
            DateTime time = new DateTime(2018, 10, 30, 12, 0, 0);
            var appt = new Appointment();
            appt.Coordinates = new List<Coordinate>();
            var coord_1 = new Coordinate(12, 12);
            appt.Coordinates.Add(coord_1);
            Coordinate output_coord_1 = CoordinateCalculationController.GetPointCoordinate(appt);

            Assert.AreEqual(coord_1.right_ascension, output_coord_1.right_ascension, 0.05);
            Assert.AreEqual(coord_1.declination, output_coord_1.declination, 0.05);
        }

        [TestMethod]
        public void TestGetCelestialBodyOrientation()
        {
            // Test at noon local time, highest elevation (Converted to UTC) 
            DateTime date = new DateTime(2018, 12, 14, 17, 0, 0);
            var appt = new Appointment();
            appt.CelestialBody = new CelestialBody(CelestialBodyConstants.SUN);
            Coordinate output_coord_1 = CoordinateCalculationController.GetCelestialBodyCoordinate(appt, date);
            Orientation output_orientation_1 = CoordinateCalculationController.CoordinateToOrientation(output_coord_1, date);
            Assert.AreEqual(26.74, output_orientation_1.elevation, 0.05);
            Assert.AreEqual(179.6, output_orientation_1.azimuth, 0.05);
        }

        [TestMethod]
        public void TestGetCelestialBodyCoordinate()
        {
            // Test at noon local time, highest elevation (Converted to UTC) 
            DateTime date = new DateTime(2018, 12, 14, 17, 0, 0);
            var appt = new Appointment();
            appt.CelestialBody = new CelestialBody(CelestialBodyConstants.SUN);
            Coordinate output_coord_1 = CoordinateCalculationController.GetCelestialBodyCoordinate(appt, date);

            Assert.AreEqual(17.4508, output_coord_1.right_ascension, 0.05);
            Assert.AreEqual(-23.2207, output_coord_1.declination, 0.05);
        }

        [TestMethod]
        public void TestGetSunCoordinate()
        {
            // Test at noon local time, highest elevation (Converted to UTC) 
            DateTime date = new DateTime(2018, 12, 14, 17, 0, 0);
            Coordinate output_coord_1 = CoordinateCalculationController.GetSunCoordinate(date);

            Assert.AreEqual(17.4508, output_coord_1.right_ascension, 0.05);
            Assert.AreEqual(-23.2207, output_coord_1.declination, 0.05);

            // Test at sunrise local time (Converted to UTC)  
            date = new DateTime(2018, 12, 14, 12, 0, 0);
            Coordinate output_coord_2 = CoordinateCalculationController.GetSunCoordinate(date);

            Assert.AreEqual(17.4354, output_coord_2.right_ascension, 0.05);
            Assert.AreEqual(-23.2085, output_coord_2.declination, 0.05);

            // Test at sunset local time (Converted to UTC)  
            date = new DateTime(2018, 12, 14, 22, 0, 0);
            Coordinate output_coord_3 = CoordinateCalculationController.GetSunCoordinate(date);

            Assert.AreEqual(17.4661, output_coord_3.right_ascension, 0.05);
            Assert.AreEqual(-23.2327, output_coord_3.declination, 0.05);
        }

        [TestMethod]
        public void TestGetMoonCoordinate()
        {
            // Test at 1:00pm local time (Converted to UTC)
            DateTime date = new DateTime(2018, 12, 14, 18, 0, 0);
            Coordinate output_coord_1 = CoordinateCalculationController.GetMoonCoordinate(date);

            Assert.AreEqual(23.1637847, output_coord_1.right_ascension, 1);
            Assert.AreEqual(-9.5685756655, output_coord_1.declination, 1);

            // Test at sunset local time (Converted to UTC)  
            date = new DateTime(2018, 12, 14, 22, 0, 0);
            Coordinate output_coord_2 = CoordinateCalculationController.GetMoonCoordinate(date);

            Assert.AreEqual(23.2914, output_coord_2.right_ascension, 1);
            Assert.AreEqual(-8.9042, output_coord_2.declination, 1);

            // Test at moonrise local time (Converted to UTC)
            date = new DateTime(2018, 12, 15, 0, 0, 0);
            Coordinate output_coord_3 = CoordinateCalculationController.GetMoonCoordinate(date);

            Assert.AreEqual(23.3550, output_coord_3.right_ascension, 1);
            Assert.AreEqual(-8.565285, output_coord_3.declination, 1);
        }

        [TestMethod]
        public void TestGetRasterOrientation()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);
            var appt = new Appointment();
            appt.start_time = start;
            appt.end_time = end;
            appt.Coordinates = new List<Coordinate>();
            var coord_1 = new Coordinate(12, 12);
            var coord_2 = new Coordinate(15, 15);
            appt.Coordinates.Add(coord_1);
            appt.Coordinates.Add(coord_2);
            Coordinate output_coord_1 = CoordinateCalculationController.GetRasterCoordinate(appt, start);
            Orientation output_orientation_1 = CoordinateCalculationController.CoordinateToOrientation(output_coord_1, start);
            Coordinate output_coord_2 = CoordinateCalculationController.GetRasterCoordinate(appt, end);
            Orientation output_orientation_2 = CoordinateCalculationController.CoordinateToOrientation(output_coord_2, end);

            Assert.AreEqual(46.41, output_orientation_1.elevation, 0.05);
            Assert.AreEqual(119.24, output_orientation_1.azimuth, 0.05);
            Assert.AreEqual(31.04, output_orientation_2.elevation, 0.05);
            Assert.AreEqual(97, output_orientation_2.azimuth, 0.05);
        }

        [TestMethod]
        public void TestGetRasterCoordinate()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);
            var appt = new Appointment();
            appt.start_time = start;
            appt.end_time = end;
            appt.Coordinates = new List<Coordinate>();
            var coord_1 = new Coordinate(12, 12);
            var coord_2 = new Coordinate(15, 15);
            appt.Coordinates.Add(coord_1);
            appt.Coordinates.Add(coord_2);
            Coordinate output_coord_1 = CoordinateCalculationController.GetRasterCoordinate(appt, start);
            Coordinate output_coord_2 = CoordinateCalculationController.GetRasterCoordinate(appt, end);

            Assert.AreEqual(coord_1.right_ascension, output_coord_1.right_ascension, 0.05);
            Assert.AreEqual(coord_1.declination, output_coord_1.declination, 0.05);

            Assert.IsTrue(coord_2.right_ascension > output_coord_2.right_ascension);
            Assert.IsTrue(coord_2.declination > output_coord_2.declination);
        }

        [TestMethod]
        public void TestGetDriftScanOrientation()
        {
            DateTime date = new DateTime(2018, 10, 30, 12, 0, 0);

            Appointment drift_scan_appt = new Appointment();
            drift_scan_appt._Type = AppointmentTypeEnum.DRIFT_SCAN;
            drift_scan_appt._Status = AppointmentStatusEnum.REQUESTED;
            Orientation test_orientation = new Orientation(30, 30);
            drift_scan_appt.Orientation = test_orientation;

            var orientation_orientation = CoordinateCalculationController.CalculateOrientation(drift_scan_appt, date);
            Assert.AreEqual(test_orientation.elevation, orientation_orientation.elevation, 0.05);
            Assert.AreEqual(test_orientation.azimuth, orientation_orientation.azimuth, 0.05);
        }

        [TestMethod]
        public void TestGetFreeControlOrientation()
        {
            DateTime date = new DateTime(2018, 10, 30, 12, 0, 0);

            // Test free control appointment
            Appointment free_control_appt = new Appointment();
            free_control_appt._Type = AppointmentTypeEnum.FREE_CONTROL;
            free_control_appt._Status = AppointmentStatusEnum.REQUESTED;
            free_control_appt._Priority = AppointmentPriorityEnum.MANUAL;
            Orientation test_orientation = new Orientation(30, 30);
            free_control_appt.Orientation = test_orientation;

            // Test free control calibration
            var free_control_orientation_1 = CoordinateCalculationController.CalculateOrientation(free_control_appt, date);
            Assert.IsTrue(free_control_orientation_1.elevation == test_orientation.elevation);
            Assert.IsTrue(free_control_orientation_1.azimuth == test_orientation.azimuth);
            Assert.IsTrue(free_control_appt.Orientation == null);

            // Test free control move
            free_control_appt.Coordinates.Add(new Coordinate(0, 0));
            var free_control_orientation_2 = CoordinateCalculationController.CalculateOrientation(free_control_appt, date);
            Assert.AreEqual(-37.14, free_control_orientation_2.elevation, 0.05);
            Assert.AreEqual(309.5, free_control_orientation_2.azimuth, 0.05);
            Assert.IsTrue(free_control_appt.Coordinates.Count == 0);

            // Test free control move without coords
            var free_control_orientation_3 = CoordinateCalculationController.CalculateOrientation(free_control_appt, date);
            Assert.IsTrue(free_control_orientation_3 == null);
            Assert.IsTrue(free_control_appt.Coordinates.Count == 0);
        }

        [TestMethod]
        public void TestGetFreeControlCoordinate()
        {
            // Test free control appointment
            Appointment free_control_appt = new Appointment();
            free_control_appt._Type = AppointmentTypeEnum.FREE_CONTROL;
            free_control_appt._Status = AppointmentStatusEnum.REQUESTED;
            free_control_appt._Priority = AppointmentPriorityEnum.MANUAL;
            free_control_appt.Coordinates.Add(new Coordinate(0, 0));

            // Test free control move
            var free_control_coordinate_1 = CoordinateCalculationController.GetFreeControlCoordinate(free_control_appt);
            Assert.AreEqual(0, free_control_coordinate_1.right_ascension, 0.05);
            Assert.AreEqual(0, free_control_coordinate_1.declination, 0.05);
            Assert.IsTrue(free_control_appt.Coordinates.Count == 0);

            // Test free control move without coords
            var free_control_coordinate_2 = CoordinateCalculationController.GetFreeControlCoordinate(free_control_appt);
            Assert.IsTrue(free_control_coordinate_2 == null);
            Assert.IsTrue(free_control_appt.Coordinates.Count == 0);
        }

        public CoordinateCalculationController CoordinateCalculationController { get; set; }
    }
}