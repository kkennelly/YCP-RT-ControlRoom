using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Main;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using ControlRoomApplication.Util;
using System.Threading;
using System.Text;

namespace ControlRoomApplication.Database
{
    public static class DatabaseOperations
    {
        private static readonly bool USING_REMOTE_DATABASE = false;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // used to tell if we need to create a control room user
        // if control room user is already in the database, we do not want to push any users
        // if it is not in the database, we want to create one then no more
        private static bool createUser = false;

        /// <summary>
        /// Updates the appointment status by saving the appt passed in.
        /// </summary>
        /// <param name="Context"> The Context that is being saved. </param>
        private static void SaveContext(RTDbContext Context)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    Context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Update the values of the entity that failed to save from the store
                    if (ex.Entries.ToList().Count > 0)
                    {
                        saveFailed = true;
                        ex.Entries.Single().Reload();
                    }
                }

            } while (saveFailed);
        }

        /// <summary>
        /// Return the appropriate database context
        /// </summary>
        private static RTDbContext InitializeDatabaseContext()
        {
            if (USING_REMOTE_DATABASE)
            {
                return new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);
            }
            else
            {
                RTDbContext LocalContext = new RTDbContext(AWSConstants.LOCAL_DATABASE_STRING);
                SaveContext(LocalContext);
                return LocalContext;
            }
        }

        /// <summary>
        /// Adds an appointment to the database
        /// </summary>
        public static void AddAppointment(Appointment appt)
        {
        
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                if (createUser == false)
                {
                    // we have this line so that we do not add another user
                    Context.Entry(appt.User).State = EntityState.Unchanged;
                }
                else
                {
                    Context.Users.Add(appt.User);
                    createUser = false;
                }
                    
                if (Context.CelestialBodies.Any(t => t.Id == appt.CelestialBody.Id) == false)
                {
                    if (Context.Coordinates.Any(t => t.Id == appt.CelestialBody.Coordinate.Id))
                        Context.Entry(appt.CelestialBody.Coordinate).State = EntityState.Unchanged;
                    
                    Context.CelestialBodies.Add(appt.CelestialBody);
                }
                if (Context.Orientations.Any(t => t.Id == appt.Orientation.Id) == false)
                    Context.Orientations.Add(appt.Orientation);
                if (Context.SpectraCyberConfigs.Any(t => t.Id == appt.SpectraCyberConfig.Id) == false)
                    Context.SpectraCyberConfigs.Add(appt.SpectraCyberConfig);

                Context.Entry(appt.Telescope).State = EntityState.Unchanged;
                
                Context.Appointments.Add(appt);
                SaveContext(Context);

                // Add coordinates
                List<Appointment> alist = Context.Appointments.ToList<Appointment>();
                foreach (Coordinate c in appt.Coordinates)
                {
                    c.apptId = alist[alist.Count - 1].Id;
                    Context.Coordinates.AddOrUpdate(c);
                }
                SaveContext(Context);
            }
        }

        //Update telescope to online 
        public static void UpdateTelescope(RadioTelescope radioTelescope)
        {
          
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                // Update radio telescope

                    radioTelescope.Location.Id = radioTelescope.location_id;
                    Context.Location.AddOrUpdate(radioTelescope.Location);

                    radioTelescope.CurrentOrientation.Id = radioTelescope.current_orientation_id;
                    Context.Orientations.AddOrUpdate(radioTelescope.CurrentOrientation);

                    radioTelescope.CalibrationOrientation.Id = radioTelescope.calibration_orientation_id;
                    Context.Orientations.AddOrUpdate(radioTelescope.CalibrationOrientation);

                    Context.RadioTelescope.AddOrUpdate(radioTelescope);
                    SaveContext(Context);

                }
            

        }


        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static List<Appointment> GetListOfAppointmentsForRadioTelescope(int radioTelescopeId)
        {
            List<Appointment> appts = new List<Appointment>();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                try
                {

                    // Use Include method to load related entities from the database
                    var appoints = Context.Appointments.Include(t => t.Telescope)
                                                        .Include(t => t.CelestialBody)
                                                        .Include(t => t.CelestialBody.Coordinate)
                                                        .Include(t => t.Orientation)
                                                        .Include(t => t.SpectraCyberConfig)
                                                        .Include(t => t.User)
                                                        .ToList<Appointment>();

                    appts = appoints.Where(x => x.telescope_id == radioTelescopeId).OrderBy(x => x.Id).ToList();

                    // Add coordinates to the appointment
                    var coordsForAppt = Context.Coordinates.ToList<Coordinate>();
                
                    foreach(Appointment a in appts)
                    {
                        foreach(Coordinate c in coordsForAppt)
                        {
                            if(c.apptId == a.Id) a.Coordinates.Add(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    ;
                }
                
            }
            return appts;
        }

        /// <summary>
        /// Returns the object of the control room user that we will use for all control room movements
        /// </summary>
        public static User GetControlRoomUser()
        {
            User controlRoomUser = null;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Use Include method to load related entities from the database
                List<User> users = Context.Users.SqlQuery("Select * from user WHERE first_name = 'control'").ToList<User>();

                // users = users.Where(x => x.first_name == "control").ToList<User>();
                
                if(users.Count() == 0)
                {
                    users.Add(new User("control", "room", "controlroom@gmail.com", NotificationTypeEnum.SMS));
                    createUser = true;
                }
                if(users.Count() > 1)
                {
                    throw new System.InvalidOperationException("Too many control room users");
                }

                controlRoomUser = users[0];
            }
            return controlRoomUser;
        }

        /// <summary>
        /// Returns a list of all Users
        /// </summary>
        public static List<User> GetAllUsers()
        {
            List<User> AllUsers = new List<User>();

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                AllUsers = Context.Users.SqlQuery("Select * from user").ToList<User>();

                if (AllUsers.Count() == 0)
                {
                    AllUsers.Add(new User("control", "room", "controlroom@gmail.com", NotificationTypeEnum.EMAIL));

                    Context.Database.ExecuteSqlCommand("INSERT INTO user (first_name, last_name, email_address, notification_type) VALUES ('control', 'room', 'controlroom@gmail.com', 'ALL')");
                    SaveContext(Context);
                }
            }
            return AllUsers;
        }
        
        /// <summary>
        /// Returns a list of all Admin Users
        /// </summary>
        public static List<User> GetAllAdminUsers(bool testflag = false)
        {
            List<User> AdminUsers = new List<User>();

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                AdminUsers = Context.Users.SqlQuery("SELECT * FROM user U INNER JOIN user_role UR ON U.id = UR.user_id WHERE UR.role = 'ADMIN'").ToList<User>();
            }
            if(AdminUsers.Count() == 0 && testflag)
            {
                User dummy = CreateDummyUser();
                AdminUsers.Add(dummy);
            }
            // We have to manually add the UserRole object to the admins pulled from the list; it doesn't travel with the SQL query
            foreach (User u in AdminUsers)
            {
                UserRole ur = new UserRole(u.Id, UserRoleEnum.ADMIN);
                u.UR = ur;
            }
            return AdminUsers;
        }

        /// <summary>
        /// FOR TEST PURPOSES ONLY. Creates a dummy Admin-level user.
        /// </summary>
        /// <returns>Returns a fake 'user' with Admin privileges.</returns>
        public static User CreateDummyUser()
        {
            User u = new User();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                AddNewDummyUser();
                u = GetDummyUser();
                
                Context.Database.ExecuteSqlCommand($"INSERT INTO user_role SET user_id = '{u.Id}', role = 'ADMIN'");
                SaveContext(Context);
            }
            return u;
        }

        /// <summary>
        /// FOR TEST PURPOSES ONLY. Adds the new dummy admin to the database.
        /// </summary>
        public static void AddNewDummyUser()
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                Context.Database.ExecuteSqlCommand("INSERT INTO user (first_name, last_name, email_address, notification_type) VALUES ('control2', 'room', 'controlroom2@gmail.com', 'ALL')");
                SaveContext(Context);
            }
        }

        /// <summary>
        /// FOR TEST PURPOSES ONLY. Retrieves the dummy user from the database, if it does not already exist, to elevate to admin rank.
        /// </summary>
        /// <returns>The dummy user</returns>
        public static User GetDummyUser()
        {
            User u = new User();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                List<User> testUserList = new List<User>();
                testUserList = Context.Users.SqlQuery("SELECT * FROM user WHERE (first_name = 'control2' AND last_name = 'room' AND email_address = 'controlroom2@gmail.com' AND notification_type = 'ALL')").ToList<User>();
                u = testUserList.First();
            }
            return u;
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static int GetTotalAppointmentCount()
        {
            int count = -1;

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                count = Context.Appointments.Count();
            }

            return count;
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static int GetTotalRTCount()
        {
            int count = -1;

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                count = Context.RadioTelescope.Count();
            }

            return count;
        }

        /// <summary>
        /// Returns the updated Appointment from the database.
        /// </summary>
        public static Appointment GetUpdatedAppointment(Appointment appt)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Because the celestial body is not required, we only want to 
                // perform these operations if one is present.
                if (appt.celestial_body_id != null)
                {
                    Context.Entry(appt.CelestialBody).State = EntityState.Unchanged;
                    if (appt.CelestialBody.Coordinate != null)
                        Context.Entry(appt.CelestialBody.Coordinate).State = EntityState.Unchanged;
                }
                if (appt.Orientation != null)
                    Context.Entry(appt.Orientation).State = EntityState.Unchanged;
                Context.Entry(appt.SpectraCyberConfig).State = EntityState.Unchanged;
                Context.Entry(appt.Telescope).State = EntityState.Unchanged;
                Context.Entry(appt.User).State = EntityState.Unchanged;

                Context.SaveChanges();

                Context.Appointments.Attach(appt);

                Context.Entry(appt).Reload();

            }
            return appt;
        }

        /// <summary>
        /// Creates and stores and RFData reading in the local database.
        /// </summary>
        /// <param name="data">The RFData reading to be created/stored.</param>
        public static void AddRFData(RFData data)
        {
            
            if (VerifyRFData(data))
            {

                using (RTDbContext Context = InitializeDatabaseContext())
                {

                    // add the rf data to the list in appointment
                    data.Appointment.RFDatas.Add(data);

                    // add the rf data to the database
                    Context.RFDatas.AddOrUpdate(data);

                    Context.Entry(data.Appointment.User).State = EntityState.Unchanged;
                    Context.Entry(data.Appointment).State = EntityState.Unchanged;
                    if (data.Appointment.Orientation != null)
                        Context.Entry(data.Appointment.Orientation).State = EntityState.Unchanged;
                    if (data.Appointment.CelestialBody != null)
                    {
                        if (data.Appointment.CelestialBody.Coordinate != null)
                            Context.Entry(data.Appointment.CelestialBody.Coordinate).State = EntityState.Unchanged;
                        Context.Entry(data.Appointment.CelestialBody).State = EntityState.Unchanged;
                    }
                    Context.Entry(data.Appointment.SpectraCyberConfig).State = EntityState.Unchanged;
                    Context.Entry(data.Appointment.Telescope).State = EntityState.Unchanged;

                    // Only perform the following operations on a real telescope
                    // (i.e. the fields will not be null)
                    if(data.Appointment.Telescope.Location != null)
                        Context.Entry(data.Appointment.Telescope.Location).State = EntityState.Unchanged;

                    if(data.Appointment.Telescope.CurrentOrientation != null)
                        Context.Entry(data.Appointment.Telescope.CurrentOrientation).State = EntityState.Unchanged;

                    if(data.Appointment.Telescope.CalibrationOrientation != null)
                        Context.Entry(data.Appointment.Telescope.CalibrationOrientation).State = EntityState.Unchanged;

                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Creates and stores the appointment calibration data (timestamps and when the calibration is performed in relation to the appointment)
        /// </summary>
        /// <param name="appointmentCalibration">Name of appointment calibration data to be stored in the db</param>
        public static void AddAppointmentCalibrationData(AppointmentCalibration appointmentCalibration)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                Context.AppointmentCalibrations.Add(appointmentCalibration);
                SaveContext(Context);
            }
        }

        public static int GetTotalRFDataCount()
        {
            int count = -1;

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                count = Context.RFDatas.Count();
            }

            return count;
        }

        /// <summary>
        /// Gets appointment calibration data stored in the database
        /// </summary>
        /// <param 
        /// name="treeStart, treeEnd, elStart, elEnd, type">Name of starting and stopping times of both calibrations 
        /// for any appointment, type is whether the appointment calibration data to be returned is for the beginning or end calibration
        /// </param>
        /// <returns>
        /// List of 2 RFData lists collected within the timeframe, tree calibration indexed at 0, elevation at 1
        /// </returns>
        
        public static List<List<RFData>> getAppointmentCalibrationData(DateTime treeStart, DateTime treeEnd, DateTime elStart, DateTime elEnd)
        {
            List<List<RFData>> fullCalibration = new List<List<RFData>>();

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Add the RFData from the calibration timeline, first add tree then add elevation
                fullCalibration.Add(Context.RFDatas.SqlQuery("SELECT * FROM rf_data WHERE 'time_captured' BETWEEN " + treeStart + " AND " + treeEnd).ToList<RFData>());
                fullCalibration.Add(Context.RFDatas.SqlQuery("SELECT * FROM rf_data WHERE 'time_captured' BETWEEN " + elStart + " AND " + elEnd).ToList<RFData>());
            }

            return fullCalibration;
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static List<RFData> GetListOfRFData()
        {
            List<RFData> appts = new List<RFData>();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Use Include method to load related entities from the database
                appts = Context.RFDatas.Include(t => t.Appointment).ToList<RFData>();

            }
            return appts;
        }

        /// <summary>
        /// Updates the appointment by saving the appt passed in.
        /// </summary>
        /// <param name="appt"> The appt that is being updated. </param>
        public static void UpdateAppointment(Appointment appt)
        {
            if (VerifyAppointmentStatus(appt))
            {
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    // Add appointment
                    Context.Appointments.AddOrUpdate(appt);

                    // Retrieve all coordinates for appointment
                    var coordsForAppt = Context.Coordinates.ToList<Coordinate>().Where(coord => coord.apptId == appt.Id);

                    // Delete coordinates
                    foreach(Coordinate c in coordsForAppt)
                    {
                        if (!appt.Coordinates.Any(co => co.Id == c.Id))
                        {
                            Context.Coordinates.Remove(c);
                            Context.SaveChangesAsync();
                        }
                    }

                    // Add coordinates
                    foreach (Coordinate c in appt.Coordinates)
                    {
                        c.apptId = appt.Id;
                        Context.Coordinates.AddOrUpdate(c);
                    }

                    Context.SaveChangesAsync();

                }
            }
        }

        /// <summary>
        /// Gets the next appointment from the local database context
        /// </summary>
        /// <returns></returns>
        public static Appointment GetNextAppointment(int radioTelescopeId)
        {
            Appointment appointment = null;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                //logger.Debug(Utilities.GetTimeStamp() + ": Retrieving list of appointments.");
                List<Appointment> appointments = GetListOfAppointmentsForRadioTelescope(radioTelescopeId);

                if (appointments.Count > 0)
                {
                    appointments.RemoveAll(x => x._Status == AppointmentStatusEnum.COMPLETED || x._Status == AppointmentStatusEnum.CANCELED || x._Status == AppointmentStatusEnum.REQUESTED);
                    appointments.Sort();
                    appointment = appointments.Count > 0 ? appointments[0] : null;
                }
                else
                {
                    //logger.Debug(Utilities.GetTimeStamp() + ": No appointments found");
                }
            }
            
            return appointment;
        }

        /// <summary>
        /// Verifies that the RFData being created/stored in the database has an
        /// intensity greater than 0 and that the time it was captured is not in
        /// the future for any reason. (1 minute into the future to allow leeway
        /// for processing time).
        /// </summary>
        /// <param name="data"> The RFData that is being created/stored. </param>
        /// <returns> A boolean indicating whether or not the RFData is valid. </returns>
        private static bool VerifyRFData(RFData data)
        {
            if (data.Intensity <= 0)
            {
                return false;
            }
            else if (data.TimeCaptured > DateTime.UtcNow.AddMinutes(1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifies that the appointment status set for the appt is actually
        /// a valid appointment status.
        /// </summary>
        /// <param name="appt"> The appointment that's status is being updated. </param>
        /// <returns> A boolean indicating whether or not the status is valid.</returns>
        private static bool VerifyAppointmentStatus(Appointment appt)
        {
            return  appt._Status.Equals(AppointmentStatusEnum.CANCELED) || 
                    appt._Status.Equals(AppointmentStatusEnum.COMPLETED) ||
                    appt._Status.Equals(AppointmentStatusEnum.IN_PROGRESS) ||
                    appt._Status.Equals(AppointmentStatusEnum.REQUESTED) ||
                    appt._Status.Equals(AppointmentStatusEnum.SCHEDULED);
        }

        /// <summary>
        /// add an array of sensor data to the apropriat table
        /// </summary>
        /// <param name="temp"></param>
        public static void AddSensorData(Temperature[] temp, bool testflag = false)
        {
            Thread t = new Thread(() =>
            {
                if (temp.Length <= 0) { return; }
                if (!USING_REMOTE_DATABASE)
                {
                    using (RTDbContext Context = InitializeDatabaseContext())
                    {
                        Context.Temperatures.AddRange(temp);
                        //foreach(Temperature tump in temp) {}
                        SaveContext(Context);
                    }
                }
            });

            t.Start();

            if (testflag) t.Join();
        }
        /// <summary>
        /// add an array of sensor data to the apropriat table
        /// </summary>
        /// <param name="acc"></param>
        public static void AddSensorData(Acceleration[] acc, bool testflag = false)
        {
            Thread t = new Thread(() =>
            {
                if (acc.Length <= 0) { return; }
                if (!USING_REMOTE_DATABASE)
                {
                    using (RTDbContext Context = InitializeDatabaseContext())
                    {
                        Context.Accelerations.AddRange(acc);
                        //foreach(Temperature tump in temp) {}
                        SaveContext(Context);
                    }
                }
            });

            t.Start();

            if (testflag) t.Join();
        }

        /// <summary>
        /// add an Blob of acceleration data to the apropriate table
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="location"></param>
        /// <param name="testflag"></param>
        public static void AddAccelerationBlobData(AccelerationBlob acc, SensorLocationEnum location, bool testflag = false)
        {
            Thread t = new Thread(() =>
            {

                if (!USING_REMOTE_DATABASE)
                {
                    using (RTDbContext Context = InitializeDatabaseContext())
                    {
                        //based on the specified location enum, add the blob to the corret table
                        switch (location)
                        {
                            case SensorLocationEnum.AZ_MOTOR:
                                Context.AzimuthAccelerationBlobs.Add((AzimuthAccelerationBlob)acc);
                                break;

                            case SensorLocationEnum.COUNTERBALANCE:
                                Context.CounterbalanceAccelerationBlobs.Add((CounterbalanceAccelerationBlob)acc);
                                break;

                            case SensorLocationEnum.EL_MOTOR:
                                Context.ElevationAccelerationBlobs.Add((ElevationAccelerationBlob)acc);
                                break;
                        }

                        SaveContext(Context);
                    }
                }
            });

            t.Start();

            if (testflag) t.Join();
        }

        /// <summary>
        /// get acc between starttime and now from sensor location loc
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endTime"> currently unused</param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static List<Acceleration> GetACCData(long starttime, long endTime, SensorLocationEnum loc)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {//&& x.TimeCaptured < endTime
                return Context.Accelerations.Where(x => x.TimeCaptured > starttime && x.location_ID == (int)loc).ToList();
            }
        }

        //returns the Azimuth Acc Blobs between the start and end times
        public static List<AzimuthAccelerationBlob> GetAzAccBlobData( long starttime , long endTime) {
            using(RTDbContext Context = InitializeDatabaseContext()) {//&& x.TimeCaptured < endTime
                return Context.AzimuthAccelerationBlobs.Where( x => x.FirstTimeCaptured >= starttime && x.FirstTimeCaptured <= endTime).ToList();
            }
        }

        //returns the Counterbalance Acc Blobs between the start and end times
        public static List<CounterbalanceAccelerationBlob> GetCbAccBlobData(long starttime, long endTime)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {//&& x.TimeCaptured < endTime
                return Context.CounterbalanceAccelerationBlobs.Where(x => x.FirstTimeCaptured >= starttime && x.FirstTimeCaptured <= endTime).ToList();
            }
        }

        //returns the Elevation Acc Blobs between the start and end times
        public static List<ElevationAccelerationBlob> GetElAccBlobData(long starttime, long endTime)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {//&& x.TimeCaptured < endTime
                return Context.ElevationAccelerationBlobs.Where(x => x.FirstTimeCaptured >= starttime && x.FirstTimeCaptured <= endTime).ToList();
            }
        }

        /// <summary>
        /// get temp between starttime and now from sensor location loc
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endTime"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static List<Temperature> GetTEMPData( long starttime , long endTime, SensorLocationEnum loc ) {
            using(RTDbContext Context = InitializeDatabaseContext()) {// && x.TimeCaptured < endTime) )   && x.TimeCaptured.Ticks < endTime.Ticks
                return Context.Temperatures.Where( x => x.TimeCapturedUTC > starttime && x.location_ID == (int)loc ).ToList();
            }
        }

        /// <summary>
        /// returns the most recent temerature for a given location
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static Temperature GetCurrentTemp( SensorLocationEnum loc ) {
            using(RTDbContext Context = InitializeDatabaseContext()) {// && x.TimeCaptured < endTime) )   && x.TimeCaptured.Ticks < endTime.Ticks
                try {
                    return Context.Temperatures.Where( x => x.location_ID == (int)loc ).OrderByDescending( x => x.TimeCapturedUTC ).First();
                } catch {
                    return new Temperature();
                }
            }
        }

        /// <summary>
        /// Adds the weather data
        /// </summary>
        public static void AddWeatherData(WeatherData weather)
        {
           using (RTDbContext Context = InitializeDatabaseContext())
           {
               Context.Weather.Add(weather);
               SaveContext(Context);
           }
        }

        /// <summary>
        /// Adds the sensor status data
        /// </summary>
        public static void AddSensorStatusData(SensorStatus sensors)
        {
           using (RTDbContext Context = InitializeDatabaseContext())
           {
               Context.SensorStatus.Add(sensors);
               SaveContext(Context);

               //logger.Info(Utilities.GetTimeStamp() + ": Added sensor status data to database");
           }
        }

        /// <summary>
        /// Grabs the current threshold value from the database for the specific sensor
        /// </summary>
        public static double GetThresholdForSensor(SensorItemEnum item)
        {
            double val;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                List<ThresholdValues> thresholds = Context.ThresholdValues.Where<ThresholdValues>(t => t.sensor_name == item.ToString()).ToList<ThresholdValues>();

                if (thresholds.Count() != 1)
                {
                    throw new InvalidOperationException();
                }

                val = Convert.ToDouble(thresholds[0].maxValue);
            }

            return val;
        }

        /// <summary>
        /// Updates the current override to whatever is provided
        /// </summary>
        public static void SetOverrideForSensor(SensorItemEnum item, bool doOverride)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var overrides = Context.Override.Where<Override>(t => t.sensor_name == item.ToString()).ToList<Override>();
                
                if(overrides.Count() != 1)
                {
                    throw new InvalidOperationException();
                }

                Override current = overrides[0];
                current.Overridden = Convert.ToSByte(doOverride);

                Context.Override.AddOrUpdate(current);

                Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets the current status of the override
        /// </summary>
        public static bool GetOverrideStatusForSensor(SensorItemEnum item)
        {
            Override current;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var overrides = Context.Override.Where<Override>(t => t.sensor_name == item.ToString()).ToList<Override>();

                if (overrides.Count() != 1)
                {
                    throw new InvalidOperationException();
                }

                current = overrides[0];
            }

            return current.Overridden == 1;
        }

        /// <summary>
        /// Adds the radio telescope
        /// </summary>
        public static void AddRadioTelescope(RadioTelescope telescope)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                Context.RadioTelescope.Add(telescope);
                SaveContext(Context);

                logger.Info(Utilities.GetTimeStamp() + ": Added radio telescope to database");
            }
        }

        /// <summary>
        /// Retrieves the lowest 'ID' radio telescope from the database
        /// </summary>
        public static RadioTelescope FetchFirstRadioTelescope()
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var telescopes = Context.RadioTelescope
                    .Include(t => t.Location)
                    .Include(t => t.CalibrationOrientation)
                    .Include(t => t.CurrentOrientation)
                    .ToList<RadioTelescope>();

                foreach (RadioTelescope rt in telescopes)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Retrieved Radio Telescope from the database");
                    return rt;
                }

                logger.Info(Utilities.GetTimeStamp() + ": No Radio Telescope found in the database");
                return null;
                
            }
        }

        /// <summary>
        /// Method used to retrieve the last RadioTelescope found in the database
        /// </summary>
        /// <returns>last RadioTelescope instance found in the database</returns>
        public static RadioTelescope FetchLastRadioTelescope()
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var telescopes = Context.RadioTelescope
                    .Include(t => t.Location)
                    .Include(t => t.CalibrationOrientation)
                    .Include(t => t.CurrentOrientation)
                    .ToList<RadioTelescope>();

                return telescopes[telescopes.Count - 1];
            }
        }
        

        /// <summary>
        /// Function to retrieve a RT Instance in the database by a given ID
        /// </summary>
        /// <param name="id">ID of RadioTelescope inside the DB</param>
        /// <returns> RadioTelescope found in the database with the associated ID
        /// Null, if the specified RT does not exist. </returns>
        public static RadioTelescope FetchRadioTelescopeByID(int id)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var telescope = Context.RadioTelescope.
                    Include(t => t.Location)
                    .Include(t => t.CalibrationOrientation)
                    .Include(t => t.CurrentOrientation)
                    .Where(t => t.Id == id).FirstOrDefault();

                if(telescope == null)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": The Radio Telescope with ID " + id + " could not be found.");
                    return null;
                }
                else
                {
                    return telescope;
                }
            }

            
        }

        /// <summary>
        /// Adds a new Sensor Network Configuration to the database
        /// </summary>
        public static void AddSensorNetworkConfig(SensorNetworkConfig config)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // First see if the config already exists (if Add was called by accident)
                var testConf = Context.SensorNetworkConfig
                    .Where(c => c.TelescopeId == config.TelescopeId).FirstOrDefault();

                // if it exists, forward the config to UpdateSensorNetworkConfig
                if (testConf != null)
                {
                    UpdateSensorNetworkConfig(config);
                }
                else
                {
                    Context.SensorNetworkConfig.Add(config);
                    SaveContext(Context);

                    logger.Info(Utilities.GetTimeStamp() + $": Created Sensor Network Configuration for Radio Telescope {config.TelescopeId}");
                }
            }
        }

        /// <summary>
        /// Returns a SensorNetworkConfig based on the RadioTelescope.Id provided.
        /// </summary>
        /// <param name="telescopeId">ID of the Radio Telescope the SensorNetworkConfig is related to</param>
        /// <returns>SensorNetworkConfig pertaining to a Radio Telescope; if none is found, null</returns>
        public static SensorNetworkConfig RetrieveSensorNetworkConfigByTelescopeId(int telescopeId)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var config = Context.SensorNetworkConfig
                    .Where(c => c.TelescopeId == telescopeId).FirstOrDefault();

                if (config == null)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": The Sensor Network Configuration for Telescope ID " + telescopeId + " could not be found.");
                    return null;
                }
                else
                {
                    return config;
                }
            }
        }

        /// <summary>
        /// This will update the sensor network configuration for a specific Telescope
        /// </summary>
        /// <param name="config">The SensorNetworkConfig to be updated</param>
        public static void UpdateSensorNetworkConfig(SensorNetworkConfig config)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var outdated = Context.SensorNetworkConfig
                    .Where(c => c.TelescopeId == config.TelescopeId).FirstOrDefault();

                if (outdated == null)
                {
                    throw new InvalidOperationException($"Cannot update config; no config found with a telescope of ID {config.TelescopeId}");
                }
                else
                {
                    config.Id = outdated.Id;
                    Context.SensorNetworkConfig.AddOrUpdate(config);
                    SaveContext(Context);

                    logger.Info(Utilities.GetTimeStamp() + ": Updated Sensor Network Configuration for Telescope ID " + config.TelescopeId);
                }
            }
        }

        /// <summary>
        /// Method used to delete a SensorNetworkConfig. This is not currently being
        /// used in any production code, only for unit tests.
        /// </summary>
        /// <param name="config">The SensorNetworkConfig to be deleted.</param>
        public static void DeleteSensorNetworkConfig(SensorNetworkConfig config)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var toDelete = Context.SensorNetworkConfig
                    .Where(c => c.TelescopeId == config.TelescopeId).FirstOrDefault();

                if (toDelete == null)
                {
                    throw new InvalidOperationException($"Cannot delete config; no config found with a telescope of ID {config.TelescopeId}");
                }
                else
                {
                    Context.SensorNetworkConfig.Attach(toDelete);
                    Context.SensorNetworkConfig.Remove(toDelete);
                    SaveContext(Context);

                    logger.Info(Utilities.GetTimeStamp() + ": Deleted Sensor Network Configuration for Telescope ID " + config.TelescopeId);
                }
            }
        }
        /// <summary>
        /// Adds a selected weather Threshold to the database
        /// </summary>
        /// <param name="weatherThreshold"></param>
        public static void AddWeatherThreshold(WeatherThreshold weatherThreshold)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                Context.WeatherThreshold.Add(weatherThreshold);
                SaveContext(Context);

                logger.Info(Utilities.GetTimeStamp() + ": Added WeatherThreshold to database");
            }
        }

        /// <summary>
        /// Routine to retrieve the time interval for dumping snow off of the dish (in minutes) from the database
        /// </summary>
        public static WeatherThreshold FetchWeatherThreshold()
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var threshold = Context.WeatherThreshold.FirstOrDefault();


                if (threshold == null)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": The WeatherThreshold data could not be found. Creating a new one with default values...");
                    // default values of 0 windSpeed and 2 hours for snow dump time. If the table is empty, add it
                    threshold = new WeatherThreshold(0, 120);
                    AddWeatherThreshold(threshold);
                }
                return threshold;
               
            }
            
        }


    }
}