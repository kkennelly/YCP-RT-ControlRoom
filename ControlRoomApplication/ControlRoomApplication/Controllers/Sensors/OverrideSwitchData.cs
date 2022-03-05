using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers.Sensors
{
    /// <summary>
    /// A public class to store the override switch data for
    /// use on DiagnosticsForm.cs and AbstractPLCDriver.cs
    /// This DOES NOT include weather station sensors.
    /// </summary>
    public class OverrideSwitchData
    {
        // Uses PLC
        public bool overrideGate { get; set; }
        public bool overrideElevatProx0 { get; set; }
        public bool overrideElevatProx90 { get; set; }

        // No longer used
        public bool overrideAzimuthProx0 { get; set; }
        public bool overrideAzimuthProx375 { get; set; }

        // Sensor Network
        public bool overrideAzimuthMotTemp { get; set; }
        public bool overrideElevatMotTemp { get; set; }
        public bool overrideAmbientTempHumidity { get; set; }

        public bool overrideAzimuthAbsEncoder { get; set; }
        public bool overrideElevationAbsEncoder { get; set; }
        public bool overrideAzimuthAccelerometer { get; set; }
        public bool overrideElevationAccelerometer { get; set; }
        public bool overrideCounterbalanceAccelerometer { get; set; }


        // Parent so we can set PLC values
        private RadioTelescope RadioTelescope { get; }

        public OverrideSwitchData(RadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;

            // PLC
            overrideGate = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);
            overrideElevatProx0 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.EL_PROXIMITY_0);
            overrideElevatProx90 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.EL_PROXIMITY_90);

            // Sensor Network
            overrideAzimuthMotTemp = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.AZIMUTH_MOTOR);
            overrideElevatMotTemp = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.ELEVATION_MOTOR);
            overrideAmbientTempHumidity = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.AMBIENT_TEMP_HUMIDITY);

            overrideAzimuthAbsEncoder = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.AZIMUTH_ABS_ENCODER);
            overrideElevationAbsEncoder = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.ELEVATION_ABS_ENCODER);
            overrideAzimuthAccelerometer = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.AZ_MOTOR_VIBRATION);
            overrideElevationAccelerometer = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.ELEV_MOTOR_VIBRATION);
            overrideCounterbalanceAccelerometer = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.COUNTER_BALANCE_VIBRATION);


            // Azimuth overrides are no longer needed because we are using the slip ring
            overrideAzimuthProx0 = true;
            overrideAzimuthProx375 = true;
        }

        // PLC
        public void setGatesOverride(bool doOverride)
        {
            overrideGate = doOverride;
            RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.GATE_OVERRIDE, Convert.ToUInt16(doOverride));
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.GATE, doOverride);
        }

        public void setElProx0Override(bool doOverride)
        {
            overrideElevatProx0 = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.EL_PROXIMITY_0, doOverride);
        }

        public void setElProx90Override(bool doOverride)
        {
            overrideElevatProx90 = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.EL_PROXIMITY_90, doOverride);
        }

        // Sensor Network
        public void setAzimuthMotTemp(bool doOverride)
        {
            overrideAzimuthMotTemp = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.AZIMUTH_MOTOR, doOverride);
        }

        public void setElevationMotTemp(bool doOverride)
        {
            overrideElevatMotTemp = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.ELEVATION_MOTOR, doOverride);
        }

        public void setAmbientTempHumidity(bool doOverride)
        {
            overrideAmbientTempHumidity = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.AMBIENT_TEMP_HUMIDITY, doOverride);
        }

        public void setAzimuthAbsEncoder(bool doOverride)
        {
            overrideAzimuthAbsEncoder = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.AZIMUTH_ABS_ENCODER, doOverride);
        }

        public void setElevationAbsEncoder(bool doOverride)
        {
            overrideElevationAbsEncoder = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.ELEVATION_ABS_ENCODER, doOverride);
        }

        public void setAzimuthAccelerometer(bool doOverride)
        {
            overrideAzimuthAccelerometer = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.AZ_MOTOR_VIBRATION, doOverride);
        }

        public void setElevationAccelerometer(bool doOverride)
        {
            overrideElevationAccelerometer = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.ELEV_MOTOR_VIBRATION, doOverride);
        }

        public void setCounterbalanceAccelerometer(bool doOverride)
        {
            overrideCounterbalanceAccelerometer = doOverride;
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.COUNTER_BALANCE_VIBRATION, doOverride);
        }
    }
}
