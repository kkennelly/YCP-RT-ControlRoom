using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using System.Threading;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Database;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Util;
using System.Timers;
using System.Diagnostics;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Entities.DiagnosticData;
using ControlRoomApplication.Entities.Encoder;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeController
    {
        public RadioTelescope RadioTelescope { get; set; }
        public CoordinateCalculationController CoordinateController { get; set; }
        public OverrideSwitchData overrides;

        /// <summary>
        /// This is the final offset for when the telescope is set in production. It will be the offset to make sure
        /// orientation 0,0 corresponds to what it should be when the telescope is set up.
        /// </summary>
        private Orientation FinalCalibrationOffset;

        private object MovementLock = new object();

        // Thread that monitors database current temperature
        private Thread SensorMonitoringThread;
        private bool MonitoringSensors;
        private bool AllSensorsSafe;
        public bool EnableSoftwareStops;

        public bool PNEnabled;

        private double MaxElTempThreshold;
        private double MaxAzTempThreshold;

        public double MinAmbientTempThreshold { get; set; }
        public double MaxAmbientTempThreshold { get; set; }
        public double MinAmbientHumidityThreshold { get; set; }
        public double MaxAmbientHumidityThreshold { get; set; }

        // Previous snow dump azimuth -- we need to keep track of this in order to add 45 degrees each time we dump
        private double previousSnowDumpAzimuth;

        // Snow dump timer
        private static System.Timers.Timer snowDumpTimer;

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EncoderAverages EncoderAverages = new EncoderAverages();

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field.
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(RadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
            CoordinateController = new CoordinateCalculationController(radioTelescope.Location);

            FinalCalibrationOffset = new Orientation(0, 0);

            overrides = new OverrideSwitchData(radioTelescope);
            radioTelescope.PLCDriver.Overrides = overrides;

            SensorMonitoringThread = new Thread(SensorMonitor);
            SensorMonitoringThread.Start();
            MonitoringSensors = true;
            AllSensorsSafe = true;
            EnableSoftwareStops = true;

            MaxAzTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_TEMP);
            MaxElTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_TEMP);

            MinAmbientTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP, false);
            MaxAmbientTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_TEMP);
            MinAmbientHumidityThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_HUMIDITY, false);
            MaxAmbientHumidityThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AMBIENT_HUMIDITY);

            previousSnowDumpAzimuth = 0;

            snowDumpTimer = new System.Timers.Timer(DatabaseOperations.FetchWeatherThreshold().SnowDumpTime * 1000 * 60);
            snowDumpTimer.Elapsed += AutomaticSnowDumpInterval;
            snowDumpTimer.AutoReset = true;
            snowDumpTimer.Enabled = true;
        }

        /// <summary>
        /// Gets the status of whether this RT is responding.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> Whether or not the RT responded. </returns>
        public bool TestCommunication()
        {
            return RadioTelescope.PLCDriver.Test_Connection();
        }

        /// <summary>
        /// Gets the current orientation of the radiotelescope in azimuth and elevation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentOrientation()
        {
            // Apply final offset
            Orientation finalOffsetOrientation = new Orientation();

            finalOffsetOrientation.elevation = RadioTelescope.PLCDriver.GetMotorEncoderPosition().elevation + FinalCalibrationOffset.elevation;
            finalOffsetOrientation.azimuth = RadioTelescope.PLCDriver.GetMotorEncoderPosition().azimuth + FinalCalibrationOffset.azimuth;

            // Normalize azimuth orientation
            while (finalOffsetOrientation.azimuth > 360) finalOffsetOrientation.azimuth -= 360;
            while (finalOffsetOrientation.azimuth < 0) finalOffsetOrientation.azimuth += 360;

            return finalOffsetOrientation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Orientation GetAbsoluteOrientation()
        {
            // Apply final offset
            Orientation finalOffsetOrientation = new Orientation();

            finalOffsetOrientation.elevation = RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.elevation + FinalCalibrationOffset.elevation;
            finalOffsetOrientation.azimuth = RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.azimuth + FinalCalibrationOffset.azimuth;

            // Normalize azimuth orientation
            while (finalOffsetOrientation.azimuth > 360) finalOffsetOrientation.azimuth -= 360;
            while (finalOffsetOrientation.azimuth < 0) finalOffsetOrientation.azimuth += 360;

            return finalOffsetOrientation;
        }

        /// <summary>
        /// Gets the status of the interlock system associated with this Radio Telescope.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> Returns true if the safety interlock system is still secured, false otherwise. </returns>
        public bool GetCurrentSafetyInterlockStatus()
        {
            return RadioTelescope.PLCDriver.Get_interlock_status();
        }

        /// <summary>
        /// Method used to cancel this Radio Telescope's current attempt to change orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult CancelCurrentMoveCommand(MovementPriority priority)
        {
            MovementResult result = MovementResult.None;


            if (Monitor.TryEnter(MovementLock) && priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                result = RadioTelescope.PLCDriver.Cancel_move();
                RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                Monitor.Exit(MovementLock);
            }

            return result;
        }

        /// <summary>
        /// Method used to shutdown the Radio Telescope in the case of inclement
        /// weather, maintenance, etcetera.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ShutdownRadioTelescope()
        {
            //StowRadioTelescope(MovementPriority.GeneralStop);
            snowDumpTimer.Stop();
            snowDumpTimer.Dispose();

            return RadioTelescope.PLCDriver.RequestStopAsyncAcceptingClientsAndJoin();
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult ThermalCalibrateRadioTelescope(MovementPriority priority)
        {
            MovementResult moveResult = MovementResult.None;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock))
            {
                Orientation current = GetCurrentOrientation();

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                moveResult = RadioTelescope.PLCDriver.MoveToOrientation(MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION, current);

                if (moveResult != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    Monitor.Exit(MovementLock);
                    return moveResult;
                }

                // temporarily set spectracyber mode to continuum
                RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);

                // start a timer so we can have a time variable
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                // temporarily set spectracyber mode to continuum
                RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);

                // read data
                SpectraCyberResponse response = RadioTelescope.SpectraCyberController.DoSpectraCyberScan();

                // end the timer
                stopWatch.Stop();
                double time = stopWatch.Elapsed.TotalSeconds;

                RFData rfResponse = RFData.GenerateFrom(response);

                // move back to previous location
                /*
                moveResult = RadioTelescope.PLCDriver.MoveToOrientation(current, MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION);
                if (moveResult != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    RadioTelescope.SpectraCyberController.StopScan();
                    Monitor.Exit(MovementLock);
                    return moveResult;
                }
                */

                /*
                // analyze data
                // temperature (Kelvin) = (intensity * time * wein's displacement constant) / (Planck's constant * speed of light)
                double weinConstant = 2.8977729;
                double planckConstant = 6.62607004 * Math.Pow(10, -34);
                double speedConstant = 299792458;
                double temperature = (rfResponse.Intensity * spectraCyberTimer.Interval * weinConstant) / (planckConstant * speedConstant);

                // convert to fahrenheit
                temperature = temperature * (9 / 5) - 459.67;

                // check against weather station reading
                double weatherStationTemp = RadioTelescope.WeatherStation.GetOutsideTemp();

                // Set SpectraCyber mode back to UNKNOWN
                RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);

                // return true if working correctly, false if not
                if (Math.Abs(weatherStationTemp - temperature) < MiscellaneousConstants.THERMAL_CALIBRATION_OFFSET)
                {
                    moveResult = StowRadioTelescope(priority);
                }
                */

                if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Critical)
                {
                    RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                }

                //RadioTelescope.SpectraCyberController.StopScan();
                Monitor.Exit(MovementLock);
            }
            else
            {
                moveResult = MovementResult.AlreadyMoving;
            }

            return moveResult;
        }

        /// <summary>
        /// Method used to request to set configuration of elements of the RT.
        /// takes the starting speed of the motor in RPM (speed of tellescope after gearing)
        /// </summary>
        /// <param name="startSpeedAzimuth">RPM</param>
        /// <param name="startSpeedElevation">RPM</param>
        /// <param name="homeTimeoutAzimuth">SEC</param>
        /// <param name="homeTimeoutElevation">SEC</param>
        /// <returns></returns>
        public bool ConfigureRadioTelescope(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return RadioTelescope.PLCDriver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation); // NO MOVE
        }

        /// <summary>
        /// Gets the elevation readings used by the software stops. When the simulation sensor network is in use,
        /// the motor positions are used, otherwise the sensor network's absolute orientation reading is used.
        /// </summary>
        /// /// <returns></returns>
        private double GetSoftwareStopElevation()
        {
            if (RadioTelescope.SensorNetworkServer.SimulationSensorNetwork != null)
            {
                return GetCurrentOrientation().elevation;
            }
            else
            {
                return GetAbsoluteOrientation().elevation;
            }
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult MoveRadioTelescopeToOrientation(Orientation orientation, MovementPriority priority, bool useAbsoluteOrientation = false)
        {
            MovementResult result = MovementResult.None;

            if (EnableSoftwareStops && ((GetSoftwareStopElevation() > RadioTelescope.maxElevationDegrees && orientation.elevation > RadioTelescope.maxElevationDegrees) ||
                (GetSoftwareStopElevation() < RadioTelescope.minElevationDegrees && orientation.elevation < RadioTelescope.minElevationDegrees))) return MovementResult.SoftwareStopHit;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock))
            {
                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                // Use absolute orientation if specified
                Orientation currentOrientation = GetCurrentOrientation();
                if (useAbsoluteOrientation)
                {
                    currentOrientation = GetAbsoluteOrientation();
                }

                result = RadioTelescope.PLCDriver.MoveToOrientation(orientation, currentOrientation);
                if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult MoveRadioTelescopeToCoordinate(Coordinate coordinate, MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            Orientation orientation = CoordinateController.CoordinateToOrientation(coordinate, DateTime.UtcNow);

            if (EnableSoftwareStops && (GetSoftwareStopElevation() > RadioTelescope.maxElevationDegrees && orientation.elevation > RadioTelescope.maxElevationDegrees) ||
                (GetSoftwareStopElevation() < RadioTelescope.minElevationDegrees && orientation.elevation < RadioTelescope.minElevationDegrees)) return MovementResult.SoftwareStopHit;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock))
            {
                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                result = RadioTelescope.PLCDriver.MoveToOrientation(CoordinateController.CoordinateToOrientation(coordinate, DateTime.UtcNow), GetCurrentOrientation());
                if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;
        }

        /// <summary>
        /// This is a method used to move the radio telescope by X degrees.
        /// Entering 0 for an axis will not move that motor.
        /// </summary>
        /// <param name="degreesToMoveBy">The number of degrees to move by.</param>
        /// <param name="priority">The movement's priority.</param>
        /// <returns></returns>
        public MovementResult MoveRadioTelescopeByXDegrees(Orientation degreesToMoveBy, MovementPriority priority)
        {
            MovementResult result = MovementResult.None;


            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            if (Math.Abs(degreesToMoveBy.azimuth) > MiscellaneousHardwareConstants.MOVE_BY_X_DEGREES_AZ_MAX) return MovementResult.RequestedAzimuthMoveTooLarge;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            Orientation origOrientation = GetCurrentOrientation();
            double normalizedAzimuth = (degreesToMoveBy.azimuth + origOrientation.azimuth) % 360;
            if (normalizedAzimuth < 0)
            {
                normalizedAzimuth += 360;
            }

            Orientation expectedOrientation = new Orientation(normalizedAzimuth, degreesToMoveBy.elevation + origOrientation.elevation);

            if (expectedOrientation.elevation < RadioTelescope.minElevationDegrees || expectedOrientation.elevation > RadioTelescope.maxElevationDegrees) return MovementResult.InvalidRequestedPostion;

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock))
            {
                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;


                int absoluteElMove, absoluteAzMove;
                absoluteElMove = ConversionHelper.DegreesToSteps(degreesToMoveBy.elevation, MotorConstants.GEARING_RATIO_ELEVATION);
                absoluteAzMove = ConversionHelper.DegreesToSteps(degreesToMoveBy.azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);

                //Peak speed calculations (using 0.6 RPM to match other move functions)
                int EL_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_ELEVATION);
                int AZ_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_AZIMUTH);

                result = RadioTelescope.PLCDriver.RelativeMove(AZ_Speed, EL_Speed, absoluteAzMove, absoluteElMove, expectedOrientation);

                if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;

        }

        /// <summary>
        /// This is used to home the telescope. Immediately after homing, the telescope will move to "Stow" position.
        /// This will also zero out the absolute encoders and account for the true north offset.
        /// </summary>
        /// <param name="priority">The priority of this movement.</param>
        /// <returns>True if homing was successful; false if homing failed.</returns>
        public MovementResult HomeTelescope(MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock)) {
                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                // Remove all offsets first so we can accurately zero out the positions
                RadioTelescope.SensorNetworkServer.AbsoluteOrientationOffset = new Orientation(0, 0);
                FinalCalibrationOffset = new Orientation(0, 0);
                RadioTelescope.PLCDriver.SetFinalOffset(FinalCalibrationOffset);

                EnableSoftwareStops = false;

                // Perform a home telescope movement
                result = RadioTelescope.PLCDriver.HomeTelescope();

                EnableSoftwareStops = true;

                // Zero out absolute encoders
                RadioTelescope.SensorNetworkServer.AbsoluteOrientationOffset = (Orientation)RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Clone();

                // Allow the absolute encoders' positions to even out
                Thread.Sleep(100);

                // Verify the absolute encoders have successfully zeroed out. There is a bit of fluctuation with their values, so homing could have occurred
                // with an outlier value. This check (with half-degree of precision) verifies that did not happen.
                Orientation absOrientation = RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation;
                if (RadioTelescope.SensorNetworkServer.SimulationSensorNetwork == null && ((Math.Abs(absOrientation.elevation) > 0.5 && !overrides.overrideElevationAbsEncoder) || 
                        (Math.Abs(absOrientation.azimuth) > 0.5 && !overrides.overrideAzimuthAbsEncoder)))
                {
                    result = MovementResult.IncorrectPosition;
                }

                // Apply final calibration offset
                FinalCalibrationOffset = RadioTelescope.CalibrationOrientation;
                RadioTelescope.PLCDriver.SetFinalOffset(FinalCalibrationOffset);

                // Reset the Encoder Average queues
                EncoderAverages.AbsoluteEncoder.Clear();
                EncoderAverages.MotorEncoder.Clear();

                if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;
        }

        /// <summary>
        /// A demonstration script that moves the elevation motor to its maximum and minimum.
        /// </summary>
        /// <param name="priority">Movement priority.</param>
        /// <returns></returns>
        public MovementResult FullElevationMove(MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock)) {
                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                Orientation origOrientation = GetCurrentOrientation();

                Orientation move1 = new Orientation(origOrientation.azimuth, 0);
                Orientation move2 = new Orientation(origOrientation.azimuth, 90);

                // Move to a low elevation point
                result = RadioTelescope.PLCDriver.MoveToOrientation(move1, origOrientation);
                if (result != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    Monitor.Exit(MovementLock);
                    return result;
                }

                // Move to a high elevation point
                result = RadioTelescope.PLCDriver.MoveToOrientation(move2, move1);
                if (result != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    Monitor.Exit(MovementLock);
                    return result;
                }

                // Move back to the original orientation
                result = RadioTelescope.PLCDriver.MoveToOrientation(origOrientation, move2);

                if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;
        }

        /// <summary>
        /// Method used to request to start jogging the Radio Telescope's elevation
        /// at a speed (in RPM), in either the clockwise or counter-clockwise direction.
        /// </summary>
        public MovementResult StartRadioTelescopeJog(double speed, RadioTelescopeDirectionEnum direction, RadioTelescopeAxisEnum axis)
        {
            MovementResult result = MovementResult.None;

            //may want to check for jogs using the RadioTelescopeAxisEnum.BOTH if a jog on both axes is needed in the future 
            if (EnableSoftwareStops && axis == RadioTelescopeAxisEnum.ELEVATION)
            {
                if (direction == RadioTelescopeDirectionEnum.ClockwiseOrNegative && GetSoftwareStopElevation() > RadioTelescope.maxElevationDegrees)
                {
                    return MovementResult.SoftwareStopHit;
                }

                else if (direction == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive && GetSoftwareStopElevation() < RadioTelescope.minElevationDegrees)
                {
                    return MovementResult.SoftwareStopHit;
                }
            }

            // Return if incoming priority is equal to or less than current movement
            if ((MovementPriority.Jog - 1) <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            if (RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped())
            {
                return MovementResult.StoppingCurrentMove;
            }

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock)) {
                RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.Jog;

                double azSpeed = 0;
                double elSpeed = 0;

                if (axis == RadioTelescopeAxisEnum.AZIMUTH) azSpeed = speed;
                else elSpeed = speed;

                result = RadioTelescope.PLCDriver.StartBothAxesJog(azSpeed, direction, elSpeed, direction);

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;
        }


        /// <summary>
        /// send a clear move to the MCU to stop a jog
        /// </summary>
        public MovementResult ExecuteRadioTelescopeStopJog(MCUCommandType stopType)
        {
            MovementResult result = MovementResult.None;

            if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Jog) return result;

            if (Monitor.TryEnter(MovementLock))
            {
                if (stopType == MCUCommandType.ControlledStop)
                {
                    result = RadioTelescope.PLCDriver.Cancel_move();
                }
                else if (stopType == MCUCommandType.ImmediateStop)
                {
                    result = RadioTelescope.PLCDriver.ImmediateStop();
                }
                else throw new ArgumentException("Jogs can only be stopped with a controlled stop or immediate stop.");

                RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }

            return result;
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to a controlled stop. this will not work for jog moves use 
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult ExecuteRadioTelescopeControlledStop(MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            if (Monitor.TryEnter(MovementLock))
            {
                if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
                {
                    result = RadioTelescope.PLCDriver.ControlledStop();
                    RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                }
                Monitor.Exit(MovementLock);
            }

            return result;
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to an immediate stop.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult ExecuteRadioTelescopeImmediateStop(MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            if (Monitor.TryEnter(MovementLock))
            {
                if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
                {
                    result = RadioTelescope.PLCDriver.ImmediateStop();
                    RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                }
                Monitor.Exit(MovementLock);
            }

            return result;
        }

        /// <summary>
        /// This allows us to safely reset the motor controller errors if an error bit happens to be set.
        /// This way, we don't have to restart the hardware to get things to work.
        /// </summary>
        /// <remarks>
        /// This will overwrite a movement if one is currently running, so it cannot be run unless there are
        /// no movements running.
        /// </remarks>
        /// <returns>True if successful, False if another movement is currently running</returns>
        public bool ResetMCUErrors()
        {
            bool success = false;

            if (Monitor.TryEnter(MovementLock))
            {
                RadioTelescope.PLCDriver.ResetMCUErrors();
                success = true;
                Monitor.Exit(MovementLock);
            }

            return success;
        }

        /// <summary>
        /// return true if the RT has finished the previous move comand
        /// </summary>
        public bool finished_exicuting_move(RadioTelescopeAxisEnum axis)//[7]
        {

            var Taz = RadioTelescope.PLCDriver.GET_MCU_Status(RadioTelescopeAxisEnum.AZIMUTH);
            var Tel = RadioTelescope.PLCDriver.GET_MCU_Status(RadioTelescopeAxisEnum.ELEVATION);

            bool azFin = Taz[(int)MCUConstants.MCUStatusBitsMSW.Move_Complete];
            bool elFin = Tel[(int)MCUConstants.MCUStatusBitsMSW.Move_Complete];
            if (axis == RadioTelescopeAxisEnum.BOTH) {
                return elFin && azFin;
            } else if (axis == RadioTelescopeAxisEnum.AZIMUTH) {
                return azFin;
            } else if (axis == RadioTelescopeAxisEnum.ELEVATION) {
                return elFin;
            }
            return false;
        }

        // Checks the motor temperatures and positions against acceptable ranges every second
        private void SensorMonitor()
        {
            // Getting initial current temperatures
            Temperature currAzTemp = RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp[RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp.Length - 1];
            Temperature currElTemp = RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp[RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp.Length - 1];
            bool elTempSafe = checkTemp(currElTemp, true);
            bool azTempSafe = checkTemp(currAzTemp, true);

            // Current Elevation Position, used to compare to see if the elevation changes when motors move
            double prevElevation = GetCurrentOrientation().elevation;

            // Set the timeout count to 0. threshold is a constant, these will be in MS
            int elevationTimeoutCount = 0;

            // Sensor overrides must be taken into account
            bool currentAZOveride = overrides.overrideAzimuthMotTemp;
            bool currentELOveride = overrides.overrideElevatMotTemp;

            SensorStatus sensors = new SensorStatus();

            // Loop through every one second to get new sensor data
            while (MonitoringSensors)
            {
                // Get initial motor and absolute encoder values
                Orientation currentABSPosition = GetAbsoluteOrientation();
                Orientation currentMotorPosition = GetCurrentOrientation();
                bool orientationSafe;

                Temperature azTemp = RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp[RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp.Length - 1];
                Temperature elTemp = RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp[RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp.Length - 1];

                azTempSafe = checkTemp(azTemp, azTempSafe);
                elTempSafe = checkTemp(elTemp, elTempSafe);

                // Sensor status routine, checks for each sensor to update the status in the DB
                // Check Gate
                sensors.gate = (SByte)SensorStatusEnum.NORMAL;
                if (!GetCurrentSafetyInterlockStatus())
                {
                    sensors.gate = (SByte)SensorStatusEnum.ALARM;
                }

                // Check azimuth temp 1, 1 - current ess sensor status will flip the bit
                sensors.az_motor_temp_1 = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.AzimuthTemperature1Status);

                // Check azimuth temp 2
                sensors.az_motor_temp_2 = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.AzimuthTemperature2Status);

                // Check elevation temp 1
                sensors.el_motor_temp_1 = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationTemperature1Status);

                // Check elevation temp 2
                sensors.el_motor_temp_2 = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationTemperature2Status);

                // Check weather
                int windSpeedStatus = RadioTelescope.WeatherStation.CurrentWindSpeedStatus;
                sensors.weather_station = (SByte)SensorStatusEnum.NORMAL;

                // Tragic wind speed
                if (windSpeedStatus == 2)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds were too high: " + RadioTelescope.WeatherStation.CurrentWindSpeedMPH);
                    // Might want to consider weather station overrides
                    sensors.weather_station = (SByte)SensorStatusEnum.ALARM;

                    PushNotification.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are too high: " + RadioTelescope.WeatherStation.CurrentWindSpeedMPH, PNEnabled);
                    EmailNotifications.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are too high: " + RadioTelescope.WeatherStation.CurrentWindSpeedMPH, PNEnabled);
                }
                // Slightly potentially tragic wind speed
                else if (windSpeedStatus == 1)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds are in Warning Range: " + RadioTelescope.WeatherStation.CurrentWindSpeedMPH);
                    // Might want to consider weather station overrides
                    sensors.weather_station = (SByte)SensorStatusEnum.WARNING;

                    PushNotification.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are in Warning Range: " + RadioTelescope.WeatherStation.CurrentWindSpeedMPH, PNEnabled);
                    EmailNotifications.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are in Warning Range: " + RadioTelescope.WeatherStation.CurrentWindSpeedMPH, PNEnabled);
                }
                
                // Check elevation absolute encoder, set to ALERT if timed out
                sensors.elevation_abs_encoder = (SByte)SensorStatusEnum.NORMAL;
                RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationAbsoluteEncoderStatus = SensorNetworkSensorStatus.Okay;

                if (RadioTelescope.PLCDriver.MotorsCurrentlyMoving(RadioTelescopeAxisEnum.ELEVATION))
                {
                    if (prevElevation == GetCurrentOrientation().elevation)
                    {
                        elevationTimeoutCount++;
                        if (elevationTimeoutCount >= SensorNetwork.SensorNetworkConstants.ElevationTimeoutThreshold)
                        {
                            sensors.elevation_abs_encoder = (SByte)SensorStatusEnum.ALARM;
                            RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationAbsoluteEncoderStatus = SensorNetworkSensorStatus.Error;
                        }
                    }
                    else
                    {
                        elevationTimeoutCount = 0;
                    }
                }
                prevElevation = GetCurrentOrientation().elevation;

                //sensors.elevation_abs_encoder = (SByte)RadioTelescope.SensorNetworkServer.SensorStatuses.elevationAbsoluteEncoderStatus;

                // Check azimuth absolute encoder
                sensors.azimuth_abs_encoder = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.AzimuthAbsoluteEncoderStatus);

                // Check proximity 0
                sensors.el_proximity_0 = (SByte)SensorStatusEnum.NORMAL;

                // Check proximity 90
                sensors.el_proximity_90 = (SByte)SensorStatusEnum.NORMAL;

                // Check azimuth acceleration
                sensors.az_accel = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.AzimuthAccelerometerStatus);

                // Check elevation acceleration
                sensors.el_accel = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationAccelerometerStatus);

                // Check CB acceleration
                sensors.counter_balance_accel = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.CounterbalanceAccelerometerStatus);

                // Check ambient temp humidity
                sensors.ambient_temp_humidity = (SByte)(1 - RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationAmbientStatus);

                // Take all updated statuses and add them to the DB
                DatabaseOperations.AddSensorStatusData(sensors);

                
                // If using not using sensor network (running a simulation) then just set sensor orientations to true since we won't need to monitor simulation values
                if (RadioTelescope.SensorNetworkServer.SimulationSensorNetwork != null)
                    orientationSafe = true;
                else
                    orientationSafe = CompareMotorAndAbsoluteEncoders(currentMotorPosition, currentABSPosition);

                // Determines if the telescope is in a safe state
                if (azTempSafe && elTempSafe) AllSensorsSafe = true;
                else
                {
                    AllSensorsSafe = false;

                    // If the motors are moving, interrupt the current movement.
                    if (RadioTelescope.PLCDriver.MotorsCurrentlyMoving())
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Sensors not safe! Interrupting current movement");
                        RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();
                    }
                }

                if (!orientationSafe && RadioTelescope.PLCDriver.MotorsCurrentlyMoving())
                {
                    if (RadioTelescope.PLCDriver.GetMotorsHomed())
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Encoders too far apart! Interrupting current movement");
                        RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();
                    }
                }

                // Run the software-stop routine
                CheckAndRunSoftwareStops();

                // If ambient temperature and humidity are overriden, simply leave the fan state as is
                if (!overrides.overrideAmbientTempHumidity)
                {
                    RadioTelescope.SensorNetworkServer.SetFanOnOrOff = DetermineFanState();
                }
                
                Thread.Sleep(100);
            }
        }

        /// <summary>
        ///  Checks that the motor temperatures are within acceptable ranges. If the temperature exceeds
        ///  the corresponding value in SimulationConstants.cs, it will return false, otherwise
        ///  it will return true if everything is good.
        ///  Tl;dr:
        ///  False - bad
        ///  True - good
        /// </summary>
        /// <returns>override bool</returns>
        public bool checkTemp(Temperature t, bool lastIsSafe)
        {
            // get maximum temperature threshold
            double max;

            // Determine whether azimuth or elevation
            String s;
            bool isOverridden;
            if (t.location_ID == (int)SensorLocationEnum.AZ_MOTOR)
            {
                s = "Azimuth";
                isOverridden = overrides.overrideAzimuthMotTemp;
                max = MaxAzTempThreshold;
            }
            else
            {
                s = "Elevation";
                isOverridden = overrides.overrideElevatMotTemp;
                max = MaxElTempThreshold;
            }

            // Check temperatures
            if (t.temp < SimulationConstants.MIN_MOTOR_TEMP)
            {
                if (lastIsSafe)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature BELOW stable temperature by " + Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.");

                    PushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature BELOW stable temperature by " + 
                        Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.", PNEnabled);
                    EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature BELOW stable temperature by " + 
                        Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.", PNEnabled);
                }

                // Only overrides if switch is true
                if (!isOverridden) return false;
                else return true;
            }
            else if (t.temp > SimulationConstants.OVERHEAT_MOTOR_TEMP)
            {
                if (lastIsSafe)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.");

                    PushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.", PNEnabled);
                    EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.", PNEnabled);
                }

                // Only overrides if switch is true
                if (!isOverridden) return false;
                else return true;
            }
            else if (t.temp <= SimulationConstants.MAX_MOTOR_TEMP && t.temp >= SimulationConstants.MIN_MOTOR_TEMP && !lastIsSafe) {
                logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature stable.");

                PushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature stable.", PNEnabled);
                EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature stable.", PNEnabled);
            }

            return true;
        }

        /// <summary>
        /// This will set the overrides based on input. Takes in the sensor that it will be changing,
        /// and then the status, true or false.
        /// true = overriding
        /// false = enabled
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="set"></param>
        public void setOverride(String sensor, bool set)
        {
            if      (sensor.Equals("azimuth motor temperature"))    overrides.setAzimuthMotTemp(set);
            else if (sensor.Equals("elevation motor temperature"))  overrides.setElevationMotTemp(set);
            else if (sensor.Equals("ambient temperature and humidity")) overrides.setAmbientTempHumidity(set);
            else if (sensor.Equals("main gate"))                    overrides.setGatesOverride(set);
            else if (sensor.Equals("elevation proximity (1)"))      overrides.setElProx0Override(set);
            else if (sensor.Equals("elevation proximity (2)"))      overrides.setElProx90Override(set);
            else if (sensor.Equals("azimuth absolute encoder")) overrides.setAzimuthAbsEncoder(set);
            else if (sensor.Equals("elevation absolute encoder")) overrides.setElevationAbsEncoder(set);
            else if (sensor.Equals("azimuth motor accelerometer")) overrides.setAzimuthAccelerometer(set);
            else if (sensor.Equals("elevation motor accelerometer")) overrides.setElevationAccelerometer(set);
            else if (sensor.Equals("counterbalance accelerometer")) overrides.setCounterbalanceAccelerometer(set);


            if (set)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Overriding " + sensor + " sensor.");

                PushNotification.sendToAllAdmins("SENSOR OVERRIDES", "Overriding " + sensor + " sensor.", PNEnabled);
                EmailNotifications.sendToAllAdmins("SENSOR OVERRIDES", "Overriding " + sensor + " sensor.", PNEnabled);
            }
            else
            {
                logger.Info(Utilities.GetTimeStamp() + ": Enabled " + sensor + " sensor.");

                PushNotification.sendToAllAdmins("SENSOR OVERRIDES", "Enabled " + sensor + " sensor.", PNEnabled);
                EmailNotifications.sendToAllAdmins("SENSOR OVERRIDES", "Enabled " + sensor + " sensor.", PNEnabled);
            }
        }

        /// <summary>
        /// This is a script that is called when we want to dump snow out of the dish
        /// </summary>
        public MovementResult SnowDump(MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock)) {

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;


                // insert snow dump movements here
                // default is azimuth of 0 and elevation of 0
                previousSnowDumpAzimuth += 45;
                if (previousSnowDumpAzimuth >= 360) previousSnowDumpAzimuth -= 360;

                Orientation dump = new Orientation(previousSnowDumpAzimuth, -4);
                Orientation current = GetCurrentOrientation();

                Orientation dumpAzimuth = new Orientation(dump.azimuth, current.elevation);
                Orientation dumpElevation = new Orientation(dump.azimuth, dump.elevation);

                // move to dump snow
                result = RadioTelescope.PLCDriver.MoveToOrientation(dumpAzimuth, current);
                if (result != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    Monitor.Exit(MovementLock);
                    return result;
                }

                result = RadioTelescope.PLCDriver.MoveToOrientation(dumpElevation, dumpAzimuth);
                if (result != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    Monitor.Exit(MovementLock);
                    return result;
                }

                // move back to initial orientation
                result = RadioTelescope.PLCDriver.MoveToOrientation(current, dumpElevation);

                if (RadioTelescope.PLCDriver.CurrentMovementPriority == priority) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                Monitor.Exit(MovementLock);
            }
            else
            {
                result = MovementResult.AlreadyMoving;
            }

            return result;
        }

        private void AutomaticSnowDumpInterval(Object source, ElapsedEventArgs e)
        {
            double DELTA = 0.01;
            Orientation currentOrientation = GetCurrentOrientation();

            // Check if we need to dump the snow off of the telescope
            if (RadioTelescope.WeatherStation.GetOutsideTemp() <= 30.00 && RadioTelescope.WeatherStation.GetTotalRain() > 0.00)
            {
                // We want to check stow position precision with a 0.01 degree margin of error
                if (Math.Abs(currentOrientation.azimuth - MiscellaneousConstants.Stow.azimuth) <= DELTA && Math.Abs(currentOrientation.elevation - MiscellaneousConstants.Stow.elevation) <= DELTA)
                {
                    Console.WriteLine("Time threshold reached. Running snow dump...");

                    MovementResult result = SnowDump(MovementPriority.Appointment);

                    if (result != MovementResult.Success)
                    {
                        logger.Info($"{Utilities.GetTimeStamp()}: Automatic snow dump FAILED with error message: {result.ToString()}");
                        PushNotification.sendToAllAdmins("Snow Dump Failed", $"Automatic snow dump FAILED with error message: {result.ToString()}", PNEnabled);
                        EmailNotifications.sendToAllAdmins("Snow Dump Failed", $"Automatic snow dump FAILED with error message: {result.ToString()}", PNEnabled);
                    }
                    else
                    {
                        Console.WriteLine("Snow dump completed");
                    }
                }

            }
        }

        /// <summary>
        /// This method runs the hardware movement script, used to verify the telescopes full ROM (az and el) and confirm that we can
        /// safely back off from both limit switches.
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public MovementResult ExecuteHardwareMovementScript(MovementPriority priority)
        {
            MovementResult movementResult = MovementResult.None;

            // Return if incoming priority is equal to or less than current movement
            if (priority <= RadioTelescope.PLCDriver.CurrentMovementPriority) return MovementResult.AlreadyMoving;

            // We only want to do this if it is safe to do so. Return false if not
            if (!AllSensorsSafe) return MovementResult.SensorsNotSafe;

            // If a lower-priority movement was running, safely interrupt it.
            RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();

            // If the thread is locked (two moves coming in at the same time), return
            if (Monitor.TryEnter(MovementLock))
            {
                bool isSim = RadioTelescope.SensorNetworkServer.SimulationSensorNetwork != null;

                // First, home telescope to get correct positioning
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning first movement: Home Telescope...");
                movementResult = HomeTelescope(MovementPriority.Manual);
                if (movementResult != MovementResult.Success)
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished first movement: Home Telescope, waiting 1 second before beginning next movement...");
                Thread.Sleep(1000);


                // TEST 1: Move to Azimuth 180 degrees
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning second movement: Move Azimuth by 180 degrees...");
                Entities.Orientation currOrientation = GetCurrentOrientation();
                movementResult = MoveRadioTelescopeToOrientation(new Entities.Orientation(180, currOrientation.elevation), MovementPriority.Manual);
                if (movementResult != MovementResult.Success)
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished second movement: Move Azimuth by 180 degrees, waiting 1 second before beginning next movement...");
                Thread.Sleep(1000);

                //TEST 2: Move in opposite direction 180 degrees using orientation from 180 degrees in opposite direction
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning third movement: Move Azimuth by -180 degrees...");
                movementResult = MoveRadioTelescopeToOrientation(currOrientation, MovementPriority.Manual);
                if (movementResult != MovementResult.Success)
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished third movement: Move Azimuth by -180 degrees, waiting 1 second before beginning next movement...");
                Thread.Sleep(1000);

                // TEST 3: Move to 90 degrees elevation
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning fourth movement: Move Elevation to 90 degrees");
                movementResult = MoveRadioTelescopeToOrientation(new Entities.Orientation(currOrientation.azimuth, 90), MovementPriority.Manual);
                if (movementResult != MovementResult.Success)
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished fourth movement: Move Elevation to 90 degrees, waiting 1 second before beginning next movement...");
                Thread.Sleep(1000);

                //TEST 4: Move to 0 degrees elevation
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning fifth movement: Move Elevation to 0 degrees");
                movementResult = MoveRadioTelescopeToOrientation(new Entities.Orientation(currOrientation.azimuth, 0), MovementPriority.Manual);
                if (movementResult != MovementResult.Success)
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished fifth movement: Move Elevation to 0 degrees, waiting 1 second before beginning next movement...");
                Thread.Sleep(1000);

                // TEST 5: Move to lower elevation limit switch - movement should fail
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning sixth movement: Move Elevation to -8 degrees (lower limit switch)");
                movementResult = MoveRadioTelescopeToOrientation(new Entities.Orientation(currOrientation.azimuth, -8), MovementPriority.Manual);
                if ((movementResult != MovementResult.LimitSwitchOrEstopHit && movementResult != MovementResult.SoftwareStopHit) || (isSim && movementResult == MovementResult.TimedOut))
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished sixth movement: Move Elevation to -8 degrees, waiting 5 seconds before beginning next movement...");
                Thread.Sleep(5000);

                // TEST 6: Move to upper elevation limit switch - movement should fail
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning seventh movement: Move Elevation to 95 degrees (upper limit switch)");
                movementResult = MoveRadioTelescopeToOrientation(new Entities.Orientation(currOrientation.azimuth, 95), MovementPriority.Manual);
                if ((movementResult != MovementResult.LimitSwitchOrEstopHit && movementResult != MovementResult.SoftwareStopHit) || (isSim && movementResult == MovementResult.TimedOut))
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished seventh movement: Move Elevation to 95 degrees, waiting 1 second before beginning next movement...");
                Thread.Sleep(5000);

                //TEST 7: Return to home
                logger.Info($"{Utilities.GetTimeStamp()}: Beginning eighth movement: Move to Home");
                movementResult = HomeTelescope(MovementPriority.Manual);
                if (movementResult != MovementResult.Success)
                {
                    Monitor.Exit(MovementLock);
                    return movementResult;
                }
                logger.Info($"{Utilities.GetTimeStamp()}: Finished eighth movement: Move to home");
                Thread.Sleep(1000);

                Monitor.Exit(MovementLock);
            }
            else
            {
                movementResult = MovementResult.AlreadyMoving;
            }

            return movementResult;
        }

        /// <summary>
        /// This is the method that handles and executes the software-stop logic
        /// </summary>
        private void CheckAndRunSoftwareStops()
        {
            // Run checks for software-stops only if they are enabled
            if (EnableSoftwareStops)
            {
                // Get the elevation direction
                RadioTelescopeDirectionEnum direction = RadioTelescope.PLCDriver.GetRadioTelescopeDirectionEnum(RadioTelescopeAxisEnum.ELEVATION);

                // Perform a critical movement interrupt if the telescope is moving past either elevation threshold
                if ((GetSoftwareStopElevation() > RadioTelescope.maxElevationDegrees && direction == RadioTelescopeDirectionEnum.ClockwiseOrNegative) ||
                    (GetSoftwareStopElevation() < RadioTelescope.minElevationDegrees && direction == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive))
                {
                    RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped(true, true);
                    logger.Info(Utilities.GetTimeStamp() + ": Software-stop hit!");
                }

                // Interrupts telescope from moving up if Upper LS is disabled & from moving down if Lower LS is disabled. 
                // NOTE: The Positive/Negative enum values were swapped previously. Hence, upward movement is currently considered
                //          negative and vice versa. 
                if(direction == RadioTelescopeDirectionEnum.ClockwiseOrNegative && overrides.overrideElevatProx90)
                {
                    RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped(true, true);
                    logger.Info(Utilities.GetTimeStamp() + ": Software-stop hit! Upper LS is disabled. No movements upwards are allowed.");
                }
                if (direction == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive && overrides.overrideElevatProx0)
                {
                    RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped(true, true);
                    logger.Info(Utilities.GetTimeStamp() + ": Software-stop hit! Lower LS is disabled. No movements downwards are allowed.");
                }
            }
        }

        /// <summary>
        /// A method to handle stowing the radio telescope.
        /// </summary>
        /// <param name="priority">The priority of the stow movement</param>
        /// <returns>The result of the movement</returns>
        public MovementResult StowRadioTelescope(MovementPriority priority)
        {
            // If motors are homed
            if (RadioTelescope.PLCDriver.GetMotorsHomed())
            {
                logger.Info(Utilities.GetTimeStamp() + ": Stowing telescope");
                return MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow, priority);
            }
            // Motors are not homed, try to use absolute encoders if we are not using the simulation sensor network 
            else if (RadioTelescope.SensorNetworkServer.SimulationSensorNetwork == null && 
                RadioTelescope.SensorNetworkServer.SensorStatuses.ElevationAbsoluteEncoderStatus != SensorNetworkSensorStatus.Error &&
                RadioTelescope.SensorNetworkServer.SensorStatuses.AzimuthAbsoluteEncoderStatus != SensorNetworkSensorStatus.Error)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Stowing telescope with absolute encoders");
                MovementResult result = MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow, priority, true);

                // Since the motors are not homed, the movement result will likely return an incorrect position,
                // so check the absolute encoder orientations instead
                if (result == MovementResult.IncorrectPosition && 
                    // Using delta of 0.5 to account for potential abs en inaccuracies and because we do not need this to be a precise movement
                    Math.Abs(GetAbsoluteOrientation().azimuth - MiscellaneousConstants.Stow.azimuth) <= 0.5 &&
                    Math.Abs(GetAbsoluteOrientation().elevation - MiscellaneousConstants.Stow.elevation) <= 0.5)
                {
                    result = MovementResult.Success;
                }
                return result;
            }
            // Don't perform the movement if the motors aren't homed and the absolute encoders are not connected
            else
            {
                logger.Info(Utilities.GetTimeStamp() + ": Canceled stow. Motors not homed and absolute encoders offline!");
                return MovementResult.MotorsNotHomed;
            }
        }

        /// <summary>
        /// Interrupts the telescope regardless of the movement movement type and stops it immediately.
        /// </summary>
        /// <returns>The result of the stop command</returns>
        public MovementResult InterruptRadioTelescope()
        {
            // Execute special stop if a jog movement. (Jog movements are not monitored and cannot be interrupted)
            if (RadioTelescope.PLCDriver.CurrentMovementPriority == MovementPriority.Jog)
            {
                return ExecuteRadioTelescopeImmediateStop(MovementPriority.Critical);
            }
            else
            {
                RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped(true);
                return MovementResult.Success;
            }
        }

        /// <summary>
        /// This is the method that handles determining whether the ESS fan should be on or off.
        /// </summary>
        /// <returns>True to turn the fan on, false to turn the fan off.</returns>
        private bool DetermineFanState()
        {
            SensorNetwork.SensorNetworkServer sn = RadioTelescope.SensorNetworkServer;

            // If the fan is on, check to see if it needs to be turned off
            if (sn.FanIsOn)
            {
                // Temp is below the lower threshold and either the humidity reached below its threshold or the outside is too
                // dew point is higher than the inside temp, which means the telescope is warming up and humidity will lower.
                // Bringing in hot air that has a dew point higher than the inside temp will cause condensation
                if (sn.CurrentElevationAmbientTemp[0].temp < MinAmbientTempThreshold &&
                    (sn.CurrentElevationAmbientHumidity[0].HumidityReading < MinAmbientHumidityThreshold ||
                    sn.CurrentElevationAmbientTemp[0].temp <= RadioTelescope.WeatherStation.GetDewPoint()))
                {
                    return false;
                }
            }
            // The fan is off, so check if it needs to be turned on
            else
            {
                // Temp is passed the upper threshold, or the humiditity is passed the upper threshold and the outside air is cooler,
                // which means the telescope is cooling down and outside air needs to be brought in to avoid condinsation
                if (sn.CurrentElevationAmbientTemp[0].temp >= MaxAmbientTempThreshold ||
                    sn.CurrentElevationAmbientHumidity[0].HumidityReading >= MaxAmbientHumidityThreshold &&
                    sn.CurrentElevationAmbientTemp[0].temp >= RadioTelescope.WeatherStation.GetOutsideTemp())
                {
                    return true;
                }
            }

            // Reaching this point means that the fan state doesn't need to be changed
            return sn.FanIsOn;
        }

        /// <summary>
        /// This is the method that checks for acceptable discrepancy in the absolute and motor encoders
        /// </summary>
        /// <returns> True if the discrepancy is below the desired threshold for both Elevation and Azimuth values, false otherwise </returns>
        /// <param name="motor"> The current orientation of the motor encoders </param>
        /// <param name="absolute"> The current orientation of the absolute encoders </param>
        public bool CompareMotorAndAbsoluteEncoders(Orientation motor, Orientation absolute)
        {
            if (!EncoderAverages.AddOrientation(absolute, motor))
            {
                if (EncoderAverages.NumErrors >= EncoderAverages.maxErrors)
                {
                    EncoderAverages.MotorEncoder.Clear();
                    EncoderAverages.AbsoluteEncoder.Clear();
                    return false;
                }

                return true;
            }

            return true;
            //// This calculates edge cases for when the azimuth goes from 360 degrees to 0
            //double diff = Math.Abs(motor.Azimuth - absolute.Azimuth);
            //diff = Math.Abs((diff + 180) % 360 - 180);

            //// Compare discrepancy of current orientations and keep below constant
            //if (Math.Abs(motor.Elevation - absolute.Elevation) <= MiscellaneousConstants.MOTOR_ABSOLUTE_ENCODER_DISCREPANCY && 
            //    diff <= MiscellaneousConstants.MOTOR_ABSOLUTE_ENCODER_DISCREPANCY)
            //    return true;
            //else 
            //    return false;
        }
    }
}