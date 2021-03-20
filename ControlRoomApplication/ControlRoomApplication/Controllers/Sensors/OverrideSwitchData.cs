﻿using System;
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
            RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT, Convert.ToUInt16(doOverride));
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.EL_PROXIMITY_0, doOverride);
        }

        public void setElProx90Override(bool doOverride)
        {
            overrideElevatProx90 = doOverride;
            RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT, Convert.ToUInt16(doOverride));
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
    }
}
