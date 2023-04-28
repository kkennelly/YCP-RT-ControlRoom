using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using ControlRoomApplication.Database;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Util;
using System.Data.Entity.Core;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;

namespace ControlRoomApplication.Entities.WeatherStation
{
    public class WeatherStation : AbstractWeatherStation
    {
        private static readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int count = 0;

        public struct WeatherUnits
        {
            public char tempUnit;
            public char RainUnit;
            public char BaromUnit;
            public char WindUnit;
            public char elevUnit;
        };

        private String[] windDirections = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S",
                                                "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"};

        // declarations of all methods we will use in the dll

        //getters, get data from dll
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetWindSpeed_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetBarometer_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetOutsideTemp_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetInsideTemp_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetDewPt_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetWindChill_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short GetOutsideHumidity_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetTotalRain_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetDailyRain_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetMonthlyRain_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short GetWindDir_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetRainRate_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern int GetHeatIndex_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short GetModelNo_V();

        //setup
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short OpenCommPort_V( short comPort , int baudRate );
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short InitStation_V();
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short OpenUSBPort_V();
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short SetUnits_V( WeatherUnits units );
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short SetVantageTimeoutVal_V(short TimeOutType);
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short SetCommTimeoutVal_V( short ReadTimeout , short WriteTimeout );

        //loaders, call these to get from weather station into dll
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short LoadCurrentVantageData_V();
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern int LoadVantageHiLows_V();
        //public static extern short GetArchiveRecord_V(WeatherRecordStruct newRecord, short i);

        private const int COM_ERROR = -32701;
        private const int MEMORY_ERROR = -102;
        private const int COM_OPEN_ERROR = -32701;
        private const int NOT_LOADED_ERROR = -104;
        private const int BAD_DATA = -32704;

        Weather_Data data;

        public WeatherStation(int currentWindSpeedScanDelayMS, int commPort = 4)
            : base(currentWindSpeedScanDelayMS)
        {

            data = new Weather_Data(0, " ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            InitializeStation(commPort);

            ReloadWeatherDataThread = new Thread(() => { ReloadWeatherDataRoutine(); });
            KeepReloadWeatherDataThreadAlive = true;
            ReloadWeatherDataThread.Start();
            
            ReloadWindDataThread = new Thread(() => { ReloadWindDataRoutine(); });
            KeepReloadWindDataThreadAlive = true;
            ReloadWindDataThread.Start();
            
        }

        // From the HeartbeatInterface
        public override bool TestIfComponentIsAlive()
        {
            // see if we can connect to the weather station
            return GetModelNo_V() != COM_ERROR;
        }

        /// <summary>
        /// Sets up connection with the production weather station. 
        /// If loading the data fails, its no big deal. We are going to attempt to load the data every time
        /// a getter method is called.
        /// </summary>
        /// <returns> void </returns>
        protected void InitializeStation(int commPort)
        {
            if (OpenCommPort_V((short)commPort, 19200) != 0)
            {
                logger.Error(Utilities.GetTimeStamp() + ": OpenCommPort unsuccessful!");
                throw new ExternalException();
            }

            if (InitStation_V() == COM_ERROR)
            {
                logger.Error(Utilities.GetTimeStamp() + ": Initialize Station unsuccessful!");
                throw new ExternalException();
            }

          //  if(LoadCurrentVantageData_V() == COM_ERROR || LoadVantageHiLows_V() == COM_ERROR) {
          //      logger.Info( "failed to collect data from weather station. will attempt again." );
          //  }

            logger.Info(Utilities.GetTimeStamp() + ": Initialize Concrete Weather Station successful!");
        }

        private void ReloadWeatherDataRoutine()
        {
            bool retrySave = false;

            while (KeepReloadWeatherDataThreadAlive) {
                if (LoadCurrentVantageData_V() != COM_ERROR)
                {
                    data.dailyRain = GetDailyRain_V();
                    data.rainRate = GetRainRate_V();
                    data.outsideTemp = GetOutsideTemp_V();
                    data.insideTemp = GetInsideTemp_V();
                    data.baromPressure = GetBarometer_V();
                    data.dewPoint = GetDewPt_V();
                    data.windChill = GetWindChill_V();
                    data.outsideHumidity = GetOutsideHumidity_V();
                    data.totalRain = GetTotalRain_V();
                    data.monthlyRain = GetMonthlyRain_V();
                    data.heatIndex = GetHeatIndex_V();

                    // This is here for a workaround to issue #360. We will occasionally get a "Connection failed on open" Entity exception
                    // We have this used to simply retry the database update if the exception occurs. It always (while I was testing, at least) succeeds after the retry.
                    // We may need to remove this in production if we find a different solution for actually eliminating the bug insetad of this workaround
                    do
                    {
                        try
                        {
                            DatabaseOperations.AddWeatherData(WeatherData.Generate(data));
                            retrySave = false;
                            count = 0;
                        }
                        catch (EntityException e)
                        {
                            count++;
                            retrySave = true;

                            // We want to retry 4 times, after which we will abandon the update.
                            if (count == 4)
                            {
                                retrySave = false;
                                count = 0;
                                logger.Info(Utilities.GetTimeStamp() + " : Failed to update weather station data after 4 tries. Abandoning update...");
                            }   
                        }
                    }
                    while (retrySave);
                }


                Thread.Sleep(60000);
            }
        }

        private void ReloadWindDataRoutine()
        {
            bool retrySave = false;
            int safeWeatherCount = 0;

            while (KeepReloadWindDataThreadAlive)
            {
                if (LoadCurrentVantageData_V() != COM_ERROR)
                {
                    data.windSpeed = GetWindSpeed_V();
                    data.windDirectionDegrees = GetWindDir_V();
                    data.windDirection = ConvertWindDirDegreesToStr(data.windDirectionDegrees);

                    /*if(data.windSpeed >= radioTelescopeController.windThreshold)
                    {
                        if(!radioTelescopeController.inclementWeather)
                        {
                            //call stow telescope and stop all other movements until otherwise told, and reset safe count if
                            MovementResult result = radioTelescopeController.ExecuteRadioTelescopeImmediateStop(MovementPriority.Critical);
                            result = radioTelescopeController.StowRadioTelescope(MovementPriority.Critical);
                            radioTelescopeController.inclementWeather = true;
                        }

                        else
                        {
                            safeWeatherCount = 0;
                        }
                    }

                    else if(radioTelescopeController.inclementWeather && data.windSpeed <= (radioTelescopeController.windThreshold - 5))
                    {
                        safeWeatherCount++;
                        if(safeWeatherCount == 60) //checks every 5 seconds, if safe for 5 minutes, inclement weather has passed
                        {
                            radioTelescopeController.inclementWeather = false;
                            MovementResult result = radioTelescopeController.HomeTelescope(MovementPriority.Critical);
                        }
                    }*/

                    // This is here for a workaround to issue #360. We will occasionally get a "Connection failed on open" Entity exception
                    // We have this used to simply retry the database update if the exception occurs. It always (while I was testing, at least) succeeds after the retry.
                    // We may need to remove this in production if we find a different solution for actually eliminating the bug insetad of this workaround

                    //potentially get max and avg over course of a minute, and put in database so it does not blow up.
                    do
                    {
                        try
                        {
                            DatabaseOperations.AddWindData(WindData.Generate(data));
                            retrySave = false;
                            count = 0;
                        }
                        catch (EntityException e)
                        {
                            count++;
                            retrySave = true;

                            // We want to retry 4 times, after which we will abandon the update.
                            if (count == 4)
                            {
                                retrySave = false;
                                count = 0;
                                logger.Info(Utilities.GetTimeStamp() + " : Failed to update weather station data after 4 tries. Abandoning update...");
                            }
                        }
                    }
                    while (retrySave);
                }


                Thread.Sleep(10000);
            }
        }

            public bool RequestToKillReloadWeatherDataRoutine()
        {
            KeepReloadWeatherDataThreadAlive = false;

            try
            {
                ReloadWeatherDataThread.Join();
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

        public override float GetWindSpeed()
        {
            return data.windSpeed;
        }

        public override float GetBarometricPressure()
        {
            return data.baromPressure;
        }

        public override float GetOutsideTemp()
        {
            return data.outsideTemp;
        }

        public override float GetInsideTemp()
        {
            return data.insideTemp;
        }

        public override float GetDewPoint()
        {
            return data.dewPoint;
        }

        public override float GetWindChill()
        {
            return data.windChill;
        }

        public override int GetHumidity()
        {
            return (int)data.outsideHumidity;
        }

        public override float GetTotalRain()
        {
            return data.totalRain;
        }

        public override float GetDailyRain()
        {
            return data.dailyRain;
        }

        public override float GetMonthlyRain()
        {
            return data.monthlyRain;
        }

        public override String GetWindDirection()
        { 
            return data.windDirection;
        }

        public override float GetRainRate()
        {
            return data.rainRate;
        }

        public override int GetHeatIndex()
        {
            return (int)data.heatIndex;
        }

        public override float GetWindDirectionDeg()
        {
            return (float)data.windDirectionDegrees;
        }

        private string ConvertWindDirDegreesToStr(float degrees)
        {
            double leftBoundary = 348.75;
            double rightBoundary = 11.25;

            for(int i = 0; i < windDirections.Length ; i++)
            {
                if (degrees > (leftBoundary+(22.5*i)) % 360 && degrees < rightBoundary+(22.5 * i))
                {
                    return windDirections[i - 1];
                }
            }

            return "ERR";
        }
    }
}
