using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom ControlRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread WeatherMonitoringThread;
        private Thread SensorStatusUpdateThread;
        private bool KeepWeatherMonitoringThreadAlive;
        private bool KeepSensorStatusUpdateThreadAlive;

        // Weather Station override
        //public bool weatherStationOverride = false;

        public ControlRoomController(ControlRoom controlRoom)
        {
            ControlRoom = controlRoom;
            SensorStatusUpdateThread = new Thread(new ThreadStart(SensorStatusUpdateRoutine)) { Name = "Sensor Status Update Routine" };
            KeepSensorStatusUpdateThreadAlive = false;
            WeatherMonitoringThread = new Thread(new ThreadStart(WeatherMonitoringRoutine)) { Name = "Weather Monitoring Routine" };
            KeepWeatherMonitoringThreadAlive = false;
        }

        public bool StartSensorStatusUpdateRoutine()
        {
            KeepSensorStatusUpdateThreadAlive = true;

            try
            {
                SensorStatusUpdateThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public bool RequestToKillSensorStatusUpdateRoutine()
        {
            KeepSensorStatusUpdateThreadAlive = false;

            try
            {
                SensorStatusUpdateThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public bool StartWeatherMonitoringRoutine()
        {
            KeepWeatherMonitoringThreadAlive = true;

            try
            {
                if (!ControlRoom.WeatherStation.Start())
                {
                    return false;
                }

                WeatherMonitoringThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public bool RequestToKillWeatherMonitoringRoutine()
        {
            KeepWeatherMonitoringThreadAlive = false;

            try
            {
                WeatherMonitoringThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public void SensorStatusUpdateRoutine()
        {
            while (KeepSensorStatusUpdateThreadAlive)
            {
                // Add checks for each sensor to update the status
                // Check Gate
                SensorStatusEnum gate = SensorStatusEnum.NORMAL;

                // Check azimuth temp 1
                SensorStatusEnum azTemp1 = SensorStatusEnum.NORMAL;

                // Check azimuth temp 2
                SensorStatusEnum azTemp2 = SensorStatusEnum.NORMAL;

                // Check elevation temp 1
                SensorStatusEnum elTemp1 = SensorStatusEnum.NORMAL;

                // Check elevation temp 2
                SensorStatusEnum elTemp2 = SensorStatusEnum.NORMAL;

                // Check weather
                SensorStatusEnum weather = SensorStatusEnum.NORMAL;

                // Check elevation absolute encoder
                SensorStatusEnum elAbsEncoder = SensorStatusEnum.NORMAL;

                // Check azimuth absolute encoder
                SensorStatusEnum azAbsEncoder = SensorStatusEnum.NORMAL;

                // Check proximity 0
                SensorStatusEnum prox0 = SensorStatusEnum.NORMAL;

                // Check proximity 90
                SensorStatusEnum prox90 = SensorStatusEnum.NORMAL;

                // Check azimuth acceleration
                SensorStatusEnum azAccel = SensorStatusEnum.NORMAL;

                // Check elevation acceleration
                SensorStatusEnum elAccel = SensorStatusEnum.NORMAL;

                // Check CB acceleration
                SensorStatusEnum cbAccel = SensorStatusEnum.NORMAL;

                // Check ambient temp humidity
                SensorStatusEnum ambientTempHumidity = SensorStatusEnum.NORMAL;

                // Take all updated statuses and add them to the DB
                DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(gate, azTemp1, azTemp2, elTemp1, elTemp2,
                    weather, elAbsEncoder, azAbsEncoder, prox0, prox90, azAccel, elAccel, cbAccel, ambientTempHumidity));
                Thread.Sleep(1000);
            }
        }
       
        // TODO: Modify this routine to be absorbed into the sensor status update routine
        public void WeatherMonitoringRoutine()
        {
            while (KeepWeatherMonitoringThreadAlive)
            {
                Sensor currentSensor = ControlRoom.RTControllerManagementThreads[0].Sensors.Find(i => i.Item == SensorItemEnum.WIND);
                int windSpeedStatus = ControlRoom.WeatherStation.CurrentWindSpeedStatus;

                // The Wind Speed has triggered an Alarm Status
                if (windSpeedStatus == 2)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds were too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);

                    // Overriding the status warning if override is true
                    if (!ControlRoom.weatherStationOverride)
                    {
                        currentSensor.Status = SensorStatusEnum.ALARM;

                        pushNotification.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                        EmailNotifications.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                    }
                    // TODO: Update this to use correct sensor status updates, that do not overwrite any other sensor status data in the DB except weather
                    //DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.WARNING, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM, currentSensor.Status));
                    DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, currentSensor.Status, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL));
                    
                    //ControlRoom.RTControllerManagementThreads[0].checkCurrentSensorAndOverrideStatus();

                    /*
                    //shut down telescopes
                    foreach (RadioTelescopeController telescopeController in ControlRoom.RadioTelescopeControllers)
                    {
                        // change the sensor status in our Radio Telescope Controller Management Thread
                        //int id = ControlRoom.RadioTelescopes.Find(i => i.WeatherStation == ControlRoom.WeatherStation).Id;
                        //ControlRoom.RTControllerManagementThreads[0].Sensors.Add(new Sensor(SensorItemEnum.WIND_SPEED, SensorStatus.ALARM));
                        //ControlRoom.RTControllerManagementThreads[0].checkCurrentSensorAndOverrideStatus();
                    }
                    */
                }
                // The Wind Speed has triggered a Warning Status
                else if(windSpeedStatus == 1)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds are in Warning Range: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);


                    // Overriding the status warning if override is true
                    if (!ControlRoom.weatherStationOverride)
                    {
                        currentSensor.Status = SensorStatusEnum.WARNING;

                        pushNotification.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are in Warning Range: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                        EmailNotifications.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are in Warning Range: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                    }
                    // TODO: Update this to use correct sensor status updates, that do not overwrite any other sensor status data in the DB except weather
                    //DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.WARNING, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM, currentSensor.Status));
                    DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, currentSensor.Status, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL));
                }
                else if (windSpeedStatus == 0)
                {
                    //logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds are in a Safe State: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                    currentSensor.Status = SensorStatusEnum.NORMAL;
                    // TODO: Update this to use correct sensor status updates, that do not overwrite any other sensor status data in the DB except weather
                    DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, currentSensor.Status, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL,
                        SensorStatusEnum.NORMAL));
                    //DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.WARNING, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM, currentSensor.Status));
                }

                /*
                else if(currentSensor != null && currentSensor.Status == SensorStatus.ALARM) {

                    //change the status
                    currentSensor.Status = SensorStatus.NORMAL;
                    logger.Info(Utilities.GetTimeStamp() + ": Wind speed sensor back in normal range.");
                }*/

                //logger.Info(Utilities.GetTimeStamp() + ": Current wind speed is: " + ControlRoom.WeatherStation.GetWindSpeed());

                Thread.Sleep(1000);                
            }
        }

        public bool AddRadioTelescopeController(RadioTelescopeController rtController)
        {
            if (ControlRoom.RadioTelescopes.Contains(rtController.RadioTelescope))
            {
                return false;
            }

            ControlRoom.RTControllerManagementThreads.Add(new RadioTelescopeControllerManagementThread(rtController));
            return true;
        }

        public bool AddRadioTelescopeControllerAndStart(RadioTelescopeController rtController)
        {
            if (AddRadioTelescopeController(rtController))
            {
                return ControlRoom.RTControllerManagementThreads[ControlRoom.RTControllerManagementThreads.Count - 1].Start();
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeControllerAt(int rtControllerIndex, bool waitForAnyTasks)
        {
            if ((rtControllerIndex < 0) || (rtControllerIndex >= ControlRoom.RTControllerManagementThreads.Count))
            {
                return false;
            }

            RadioTelescopeControllerManagementThread ToBeRemovedRTMT = ControlRoom.RTControllerManagementThreads[rtControllerIndex];

            if (!waitForAnyTasks)
            {
                ToBeRemovedRTMT.KillWithHardInterrupt();
            }
            else
            {
                ToBeRemovedRTMT.RequestToKill();
            }

            if (ToBeRemovedRTMT.WaitToJoin())
            {
                return ControlRoom.RTControllerManagementThreads.Remove(ToBeRemovedRTMT);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeController(RadioTelescopeController rtController, bool waitForAnyTasks)
        {
            return RemoveRadioTelescopeControllerAt(ControlRoom.RadioTelescopeControllers.IndexOf(rtController), waitForAnyTasks);
        }
    }
}