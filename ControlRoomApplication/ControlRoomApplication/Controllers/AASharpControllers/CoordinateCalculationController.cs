using System;
using System.Collections.Generic;
using System.Linq;
using AASharp;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class CoordinateCalculationController
    {
        private Location Location { get; }

        public CoordinateCalculationController(Location location)
        {
            Location = location;
        }

        public Orientation CoordinateToOrientation(Coordinate coordinate, DateTime datetime)
        {
            if(coordinate == null)
            {
                throw new ArgumentException("Coordinate cannot be null");
            }

            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);

            double ApparentGreenwichSiderealTime = AASSidereal.ApparentGreenwichSiderealTime(date.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(Location.Longitude);
            double LocalHourAngle = ApparentGreenwichSiderealTime - LongtitudeAsHourAngle - coordinate.right_ascension;
            AAS2DCoordinate Horizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, coordinate.declination, Location.Latitude);

            // Since AASharp considers south zero, flip the orientation 180 degrees
            // TODO: Verify that this correctly points us north or south based on our true north offset (issue #391)
            Horizontal.X += 180;
            if (Horizontal.X > 360)
            {
                Horizontal.X -= 360;
            }

            return new Orientation(Horizontal.X, Horizontal.Y);
        }

        public Coordinate OrientationToCoordinate(Orientation currHorizontal, DateTime datetime)
        {
            // We don't want to modify the current orientation, so we must create a new instance
            Orientation horizontal = (Orientation)currHorizontal.Clone();

            if (horizontal == null)
            {
                throw new ArgumentException("Orientation cannot be null");
            }

            // Since AASharp considers south zero, flip the orientation 180 degrees
            horizontal.azimuth += 180;
            if (horizontal.azimuth > 360)
            {
                horizontal.azimuth -= 360;
            }
            
            AAS2DCoordinate equatorial = AASCoordinateTransformation.Horizontal2Equatorial(horizontal.azimuth, horizontal.elevation, Location.Latitude);

            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);
            double ApparentGreenwichSiderealTime = AASSidereal.ApparentGreenwichSiderealTime(date.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(Location.Longitude);
            double RightAscension = ApparentGreenwichSiderealTime - LongtitudeAsHourAngle - equatorial.X;
            if(RightAscension < 0)
            {
                RightAscension += 24;
            }
            return new Coordinate(RightAscension, equatorial.Y);
        }

        public Orientation CalculateOrientation(Appointment appt, DateTime datetime)
        {
            switch (appt._Type)
            {
                case (AppointmentTypeEnum.POINT):
                    return GetPointOrientation(appt, datetime);
                case (AppointmentTypeEnum.CELESTIAL_BODY):
                    return GetCelestialBodyOrientation(appt, datetime);
                case (AppointmentTypeEnum.RASTER):
                    return GetRasterOrientation(appt, datetime);
                case (AppointmentTypeEnum.DRIFT_SCAN):
                    return GetDriftScanOrienation(appt);
                case (AppointmentTypeEnum.FREE_CONTROL):
                    return GetFreeControlOrientation(appt, datetime);
                default:
                    throw new ArgumentException("Invalid Appt type");
            }
        }

        public Orientation GetPointOrientation(Appointment appt, DateTime datetime)
        {
            var point_coord = GetPointCoordinate(appt);
            return CoordinateToOrientation(point_coord, datetime);
        }

        public Coordinate GetPointCoordinate(Appointment appt)
        {
            var coords = appt.Coordinates.ToList();
            if (coords.Count > 1)
            {
                throw new ArgumentException("Too many Coordinates for a Point");
            }
            else if (coords.Count < 1)
            {
                throw new ArgumentException("No Point Coordinate");
            }
            return coords[0];
        }

        public Orientation GetCelestialBodyOrientation(Appointment appt, DateTime datetime)
        {
            var celestial_body_coord = GetCelestialBodyCoordinate(appt, datetime);
            return CoordinateToOrientation(celestial_body_coord, datetime);
        }

        public Coordinate GetCelestialBodyCoordinate(Appointment appt, DateTime datetime)
        {
            switch (appt.CelestialBody.Name)
            {
                case CelestialBodyConstants.SUN:
                    return GetSunCoordinate(datetime);
                case CelestialBodyConstants.MOON:
                    return GetMoonCoordinate(datetime);
                case null:
                    throw new ArgumentException("Invalid Celestial Body");
                default:
                    if (appt.CelestialBody.Coordinate != null)
                    {
                        return appt.CelestialBody.Coordinate;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Celestial Body Coordinate");
                    }
            }
        }

        public Coordinate GetSunCoordinate(DateTime datetime)
        {
            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            double SunLong = AASSun.ApparentEclipticLongitude(JD, false);
            double SunLat = AASSun.ApparentEclipticLatitude(JD, false);

            AAS2DCoordinate equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(SunLong, SunLat, AASNutation.TrueObliquityOfEcliptic(JD));
            double SunRad = AASEarth.RadiusVector(JD, false);

            // This line gives us RA & Declination.
            AAS2DCoordinate SunTopo = AASParallax.Equatorial2Topocentric(equatorial.X, equatorial.Y, SunRad, Location.Longitude, Location.Latitude, Location.Altitude, JD);

            return new Coordinate(SunTopo.X, SunTopo.Y);
        }

        public Coordinate GetMoonCoordinate(DateTime datetime)
        {
            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            double MoonLong = AASMoon.EclipticLongitude(JD);
            double MoonLat = AASMoon.EclipticLatitude(JD);

            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(MoonLong, MoonLat, AASNutation.TrueObliquityOfEcliptic(JD));
            double MoonRad = AASMoon.RadiusVector(JD);
            MoonRad /= 149597870.691; //Convert KM to AU

            AAS2DCoordinate MoonTopo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, MoonRad, Location.Longitude, Location.Latitude, Location.Altitude, JD);

            return new Coordinate(MoonTopo.X, MoonTopo.Y);
        }

        public Orientation GetRasterOrientation(Appointment appt, DateTime datetime)
        {
            var raster_coord = GetRasterCoordinate(appt, datetime);
            return CoordinateToOrientation(raster_coord, datetime);
        }

        public Coordinate GetRasterCoordinate(Appointment appt, DateTime datetime)
        {
            // Get start and end coordinates
            List<Coordinate> coords = appt.Coordinates.ToList();

            if (coords.Count > 2)
            {
                throw new ArgumentException("Too many Coordinates for a Raster Scan");
            }
            else if (coords.Count < 2)
            {
                throw new ArgumentException("Too few Coordinates for a Raster Scan");
            }
            Coordinate start_coord = coords[0];
            Coordinate end_coord = coords[1];

            // Make sure the coordinates do not overlap
            if( start_coord.right_ascension == end_coord.right_ascension ||
                start_coord.declination == end_coord.declination)
            {
                throw new ArgumentException("Coordinates cannot overlap");
            }

            // Find the width and the height of the square in points (minutes),
            // rounded down to an integer
            double num_points = (appt.end_time - appt.start_time).TotalMinutes;
            double point_width = Math.Floor(Math.Sqrt(num_points));
            double point_height = Math.Floor(Math.Sqrt(num_points));
            if (point_width == 0 || point_height == 0)
            {
                throw new ArgumentException("Appointment duration is too short");
            }

            // Find the width and the height of the square in coordinates
            double coord_width = end_coord.right_ascension - start_coord.right_ascension;
            double coord_height = end_coord.declination - start_coord.declination;

            // Find the coordinate increment per point 
            // (x = RightAscension, y = Declination)
            double x_increment = coord_width / point_width;
            double y_increment = coord_height / point_height;

            // Check if the point index is outside the bounds of the square
            // (this can occur because of the rounding down that occurs when
            // finding the point_width and point_height)
            // If it is, just stay at the last point of the square
            double max_point = point_width * point_height;
            double point = (datetime - appt.start_time).TotalMinutes;
            if (point >= max_point)
            {
                point = max_point - 1;
            }

            // Find the point's x, y location, rounded down
            // (x = RightAscension, y = Declination)
            double point_x = Math.Floor(point / point_width);
            double point_y = Math.Floor(point % point_height);

            // Find the coordinate change in x and y
            // (x = RightAscension, y = Declination)
            double dx = (point_x * x_increment) % coord_width;
            double dy = (point_y * y_increment) % coord_height;

            // Find the new coordinate x and y
            // (x = RightAscension, y = Declination)
            double x = start_coord.right_ascension + dx;
            double y = start_coord.declination + dy;

            //logger.Info(x + ", " + y); // (FOR TESTING)

            // Create the new coordinate
            // (x = RightAscension, y = Declination)
            return new Coordinate(x, y);
        }

        public Orientation GetDriftScanOrienation(Appointment appt)
        {
            return appt.Orientation;
        }

        public Orientation GetFreeControlOrientation(Appointment appt, DateTime datetime)
        {
            Orientation free_orientation = null;
            if (appt.Orientation == null)
            {
                var free_coord = GetFreeControlCoordinate(appt);
                if(free_coord != null)
                {
                    free_orientation = CoordinateToOrientation(free_coord, datetime);
                } 
            }
            else
            {
                free_orientation = appt.Orientation;
                appt.Orientation = null;
                DatabaseOperations.UpdateAppointment(appt);
            }
            return free_orientation;
        }

        public Coordinate GetFreeControlCoordinate(Appointment appt)
        {
            Coordinate free_coord = null;
            if(appt.Coordinates.Count > 0)
            {
                free_coord = appt.Coordinates.First();
                appt.Coordinates.Remove(free_coord);
                DatabaseOperations.UpdateAppointment(appt);
            }
            return free_coord;
        }
    }
}