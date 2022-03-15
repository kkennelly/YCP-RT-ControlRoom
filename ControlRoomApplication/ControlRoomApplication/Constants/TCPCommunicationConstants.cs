using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Constants
{
    /// <summary>
    /// This class contains all of the error message strings we will be sending back to the mobile app
    /// or the front-end team while communicating with the control room via RemoteListener.
    /// </summary>
    public sealed class TCPCommunicationConstants
    {
        public static String VERSION_CONVERSION_ERR = "Error converting version from string to double: ";

        public static String VERSION_NOT_FOUND = "The specified version was not found: ";

        public static String AZ_EL_CONVERSION_ERR = "Error converting azimuth or elevation from string to double: ";

        public static String MISSING_AZ_EL_ARGS = "Arguments for AZ or EL not supplied.";

        public static String ORIENTATION_MOVE_ERR = "Command ORIENTATION_MOVE failed with result: ";

        public static String RELATIVE_MOVE_ERR = "Command RELATIVE_MOVE failed with result: ";

        public static String MISSING_OVERRIDE_ARG = "No sensor name supplied for SET_OVERRIDE.";

        public static String MISSING_SET_OVERRIDE_ARG = "No true/false value supplied for SET_OVERRIDE.";

        public static String INVALID_SENSOR_OVERRIDE = "The following sensor was not found to override: ";

        public static String SCRIPT_ERR = "The selected script failed with error: ";

        public static String ALL_STOP_ERR = "The All Stop command failed with error: TimedOut ";

        public static String MISSING_SENSOR_INIT_ARGS = "SENSOR_INIT command is missing arguments; supply all 10 of them. 1" +
            "means initialize this sensor, 0 means do not initialize.";
        public static String COMMAND_NOT_FOUND = "The specified command was not found or is invalid: ";

        public static String SCRIPT_NOT_FOUND = "The specified script was not found or is invalid: ";

        public static String MISSING_SCRIPT_TYPE_ARG = "Missing a script name after 'SCRIPT'";

        public static String MISSING_COMMAND_ARGS = "The total number of arguments is invalid for this type of command: ";

        public static String INVALID_REQUEST_TYPE = "The specified REQUEST type was not found or is invalid: ";

        public static String[] SCRIPT_NAME_ARRAY = new String[] { "DUMP", "FULL_EV", "THERMAL_CALIBRATE", "STOW", "FULL_CLOCK", "FULL_COUNTER", "HOME", "HARDWARE_MVMT_SCRIPT" };

        public static String[] SENSORS_ARRAY = new String[] { "WEATHER_STATION", "MAIN_GATE", "ELEVATION_LIMIT_0", "ELEVATION_LIMIT_90", "AZ_ABS_ENC", "EL_ABS_ENC", "AZ_ACC", "EL_ACC", "CB_ACC",
        "AZIMUTH_MOT_TEMP", "ELEVATION_MOT_TEMP" };

        public static String[] ALL_VERSIONS_LIST = new String[] { "1.0" };


        // Integers for indexing array of command pieces
        // General numbers
        public static int VERSION_NUM = 0;
        public static int COMMAND_TYPE = 1;
        public static int MIN_NUM_PARAMS = 3; // case of STOP_RT, will only have Version, stopRT, time --> <V> | STOP_RT | <time>

        // Total params expected
        public static int NUM_SENSOR_OVERRIDE_PARAMS = 5;
        public static int NUM_SCRIPT_PARAMS = 4;
        public static int NUM_ORIENTATION_MOVE_PARAMS = 5;
        public static int NUM_RELATIVE_MOVE_PARAMS = 5;
        public static int NUM_ALL_STOP_PARAMS = 3;
        public static int NUM_SENSOR_INIT_PARAMS = 4;
        public static int NUM_REQUEST_PARAMS = 4;
        public static int NUM_MCU_RESET_PARAMS = 3;

        // SET_OVERRIDE specific numbers
        public static int SENSOR_TO_OVERRIDE = 2;
        public static int DO_OVERRIDE = 3;
        public static int SENSOR_OVERRIDE_TIME = 4;

        // SCRIPT specific numbers
        public static int SCRIPT_NAME = 2;
        public static int SCRIPT_TIME = 3;

        // ORIENTATION_MOVE specific numbers
        public static int ORIENTATION_MOVE_AZ = 2;
        public static int ORIENTATION_MOVE_EL = 3;
        public static int ORIENTATION_MOVE_TIME = 4;

        // RELATIVE_MOVE specific numbers
        public static int RELATIVE_MOVE_AZ = 2;
        public static int RELATIVE_MOVE_EL = 3;
        public static int RELATIVE_MOVE_TIME = 4;

        // REQUEST specific numbers
        public static int REQUEST_TYPE = 2;
        public static int REQUEST_TIME = 3;

        // SENSOR_INIT numbers
        public static int SENSOR_INIT_VALUES = 2;
    }
}
