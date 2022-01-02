using System.Collections.Generic;
using ControlRoomApplication.Controllers;
using System.Net;
using ControlRoomApplication.Database;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public List<RadioTelescopeControllerManagementThread> RTControllerManagementThreads { get; }
        public AbstractWeatherStation WeatherStation { get; }
        public RemoteListener mobileControlServer { get; }
        public int RemoteListenerPort { get; set; }
        public bool weatherStationOverride { get; set; }

        public List<RadioTelescopeController> RadioTelescopeControllers
        {
            get
            {
                List<RadioTelescopeController> rtControllers = new List<RadioTelescopeController>();

                foreach (RadioTelescopeControllerManagementThread rtmt in RTControllerManagementThreads)
                {
                    rtControllers.Add(rtmt.RTController);
                }

                return rtControllers;
            }
        }

        public List<RadioTelescope> RadioTelescopes
        {
            get
            {
                List<RadioTelescope> RTList = new List<RadioTelescope>();

                foreach (RadioTelescopeControllerManagementThread rtmt in RTControllerManagementThreads)
                {
                    RTList.Add(rtmt.RTController.RadioTelescope);
                }

                return RTList;
            }
        }

        public ControlRoom(AbstractWeatherStation weatherStation, int RemoteListenerPort)
        {
            this.RemoteListenerPort = RemoteListenerPort;
            RTControllerManagementThreads = new List<RadioTelescopeControllerManagementThread>();
            WeatherStation = weatherStation;
            mobileControlServer = new RemoteListener(RemoteListenerPort, this);
            weatherStationOverride = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.WEATHER_STATION);
        }
    }
}