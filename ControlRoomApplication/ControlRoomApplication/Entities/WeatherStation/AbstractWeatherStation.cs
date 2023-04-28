﻿using System;
using System.Collections.Generic;
using System.Threading;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Util;

namespace ControlRoomApplication.Entities
{
    public abstract class AbstractWeatherStation : HeartbeatInterface
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Mutex OperatingMutex;
        protected Thread OperatingThread;
        protected bool KeepOperatingThreadAlive;

        public Thread ReloadWeatherDataThread;
        public bool KeepReloadWeatherDataThreadAlive;

        public Thread ReloadWindDataThread;
        public bool KeepReloadWindDataThreadAlive;

        private double _CurrentWindSpeedMPH;
        public Queue<double> WindSpeedsMPH = new Queue<double>(0);

        public double CurrentWindSpeedMPH
        {
            get
            {
                OperatingMutex.WaitOne();
                double read = GetWindSpeed();
                OperatingMutex.ReleaseMutex();

                //we check this roughly 4 times a second, and it updates every 8 seconds,
                //cant just change that time, cause other sensors need to be checked that often
                //32 data points of the same reading, we get 5 data points and thats 160 long queue
                //this is bad code, 161 long queue is bad.
                WindSpeedsMPH.Enqueue(read);

                if(WindSpeedsMPH.Count > DatabaseOperations.FetchWeatherThreshold().QueueSize)
                {
                    WindSpeedsMPH.Dequeue();
                }

                return read;
            }
        }

        public struct Weather_Data
        {
            public float windSpeed;
            public String windDirection;
            public float windDirectionDegrees;
            public float dailyRain;
            public float rainRate;
            public float outsideTemp;
            public float insideTemp;
            public float baromPressure;
            public float dewPoint;
            public float windChill;
            public float outsideHumidity;
            public float totalRain;
            public float monthlyRain;
            public float heatIndex;

            public Weather_Data(float windSpeedIN, String windDirectionIN, float windDirectionDegreesIN, float dailyRainIN, float rainRateIN,
                                    float outsideTempIN, float insideTempIN, float baromPressureIN, float dewPointIN, float windChillIN,
                                    float outsideHumidityIN, float totalRainIN, float monthlyRainIN, float heatIndexIN)
            {
                windSpeed = windSpeedIN;
                windDirection = windDirectionIN;
                windDirectionDegrees = windDirectionDegreesIN;
                dailyRain = dailyRainIN;
                rainRate = rainRateIN;
                outsideTemp = outsideTempIN;
                insideTemp = insideTempIN;
                baromPressure = baromPressureIN;
                dewPoint = dewPointIN;
                windChill = windChillIN;
                outsideHumidity = outsideHumidityIN;
                totalRain = totalRainIN;
                monthlyRain = monthlyRainIN;
                heatIndex = heatIndexIN;
            }
        };

        public int CurrentWindSpeedScanDelayMS { get; }

        public bool CurrentWindSpeedIsAllowable
        {
            get
            {
                return CurrentWindSpeedMPH <= MiscellaneousHardwareConstants.WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH;
            }
        }

        public bool isBadWeather
        {
            get
            {

                int count = 0;
                foreach(double d in WindSpeedsMPH) 
                {
                    if (d >= DatabaseOperations.FetchWeatherMaxThreshold().WindSpeed)
                    {
                        count++;
                    }
                }
                return count >= ((WindSpeedsMPH.Count / 2) + 1);
            }
        }

        public bool isGoodWeather
        {
            get
            {
                foreach (double d in WindSpeedsMPH)
                {
                    if(d >= DatabaseOperations.FetchWeatherThreshold().WindSpeed)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        //change back to 20 and 30 in database when done testing
        public int CurrentWindSpeedStatus
        {
            get
            {
                if (isBadWeather)
                    return 2;
                else if (isGoodWeather)
                    return 0;
                else
                    return 1;

                /*if (CurrentWindSpeedMPH < DatabaseOperations.FetchWeatherThreshold().WindSpeed)
                    return 0; // Safe State of the Wind Speed
                else if (CurrentWindSpeedMPH >= DatabaseOperations.FetchWeatherThreshold().WindSpeed
                    && CurrentWindSpeedMPH < DatabaseOperations.FetchWeatherMaxThreshold().WindSpeed)
                    return 1; // Warning State of the Wind Speed
                else
                    return 2; // Alarm State of the Wind Speed*/
            }
        }



        public AbstractWeatherStation(int currentWindSpeedScanDelayMS)
        {
            CurrentWindSpeedScanDelayMS = currentWindSpeedScanDelayMS;
            _CurrentWindSpeedMPH = 0.0;
            OperatingMutex = new Mutex();
            OperatingThread = new Thread(new ThreadStart(OperationLoop));
            KeepOperatingThreadAlive = false;

            ReloadWeatherDataThread = null;
            KeepReloadWeatherDataThreadAlive = false;
    }

        public bool Start()
        {
            KeepOperatingThreadAlive = true;

            try
            {
                OperatingThread.Start();
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

        public bool RequestKillAndJoin()
        {
            try
            {
                OperatingMutex.WaitOne();
                KeepOperatingThreadAlive = false;
                OperatingMutex.ReleaseMutex();

                KeepReloadWeatherDataThreadAlive = false;
                ReloadWeatherDataThread.Join();
                OperatingThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ObjectDisposedException) || (e is AbandonedMutexException) || (e is InvalidOperationException)
                    || (e is ApplicationException) || (e is ThreadStateException) || (e is ThreadInterruptedException))
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

        public void OperationLoop()
        {
            OperatingMutex.WaitOne();
            bool KeepAlive = KeepOperatingThreadAlive;
            OperatingMutex.ReleaseMutex();

            while (KeepAlive)
            {
                OperatingMutex.WaitOne();
                _CurrentWindSpeedMPH = GetWindSpeed();
                KeepAlive = KeepOperatingThreadAlive;
                OperatingMutex.ReleaseMutex();

                Thread.Sleep(CurrentWindSpeedScanDelayMS);
            }
        }

        protected override bool KillHeartbeatComponent()
        {
            return RequestKillAndJoin();
        }
        
        public abstract float GetBarometricPressure();
        public abstract float GetOutsideTemp();
        public abstract float GetInsideTemp();
        public abstract float GetDewPoint();
        public abstract float GetWindChill();
        public abstract int GetHumidity();
        public abstract float GetTotalRain();
        public abstract float GetDailyRain();
        public abstract float GetMonthlyRain();
        public abstract float GetWindSpeed();
        public abstract String GetWindDirection();

        public abstract float GetWindDirectionDeg();
        public abstract float GetRainRate();
        public abstract int GetHeatIndex();

        // An abstract method that will get all of the information
        // to be able to put into the database cleanly
        //protected abstract short GetAllRecords();
    }
}
