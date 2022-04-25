namespace ControlRoomApplication.Constants
{
    public sealed class SimulationConstants
    {
        public const double STABLE_MOTOR_TEMP = 50; // Measured in fahrenheit
        public const double OVERHEAT_MOTOR_TEMP = 150;
        public const double MIN_MOTOR_TEMP = 0;
        public const double MAX_MOTOR_TEMP = 200;

        public const double AZIMUTH_UPDATE_RATE = 1.0; // Measured as degrees per second
        public const double ELEVATION_UPDATE_RATE = 0.5;
        public const double MAX_AZIMUTH_ANGLE = 369; //Measured in degrees
        public const double MIN_AZIMUTH_ANGLE = -9;
        public const double MAX_ELEVATION_ANGLE = 94;
        public const double MIN_ELEVATION_ANGLE = -15;

        public const double LIMIT_CW_AZ_DEGREES = -5;
        public const double LIMIT_CCW_AZ_DEGREES = 365;
        public const double LIMIT_LOW_EL_DEGREES = -7;  // Changed from -15
        public const double LIMIT_HIGH_EL_DEGREES = 93; // Changed from 93

        public const double PROX_CW_AZ_DEGREES = 10;
        public const double PROX_CCW_AZ_DEGREES = 345;
        public const double PROX_LOW_EL_DEGREES = -5;
        public const double PROX_HIGH_EL_DEGREES = 85;


    }
}
