namespace ControlRoomApplication.Constants
{
    public sealed class MiscellaneousHardwareConstants
    {
        public const double LIMIT_SWITCH_AZ_THRESHOLD_DEGREES = 2.0;
        public const double LIMIT_SWITCH_EL_THRESHOLD_DEGREES = 2.0;

        public const int WEATHER_STATION_DEFAULT_CHECKIN_FREQUENCY_MS = 1000;
        public const double WEATHER_STATION_WARNING_WIND_SPEED_MPH = 40.0;
        public const double WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH = 50.0;
        public const double SIMULATION_WEATHER_STATION_AVERAGE_WIND_SPEED_MPH = 20.0;
        public const double SIMULATION_WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH_STD_DEV = (WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH - SIMULATION_WEATHER_STATION_AVERAGE_WIND_SPEED_MPH) / 6.0;

        public const int MOVE_BY_X_DEGREES_AZ_MAX = 720;
      
        public const double SPECTRACYBER_VOLTS_PER_STEP = 0.0024414;
        public const double SPECTRACYBER_BANDWIDTH_STEP = 5; // Measured in Khz
    }
}