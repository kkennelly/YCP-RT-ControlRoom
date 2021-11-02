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

        private double MaxElTempThreshold;
        private double MaxAzTempThreshold;

        // Previous snow dump azimuth -- we need to keep track of this in order to add 45 degrees each time we dump
        private double previousSnowDumpAzimuth;

        // Snow dump timer
        private static System.Timers.Timer snowDumpTimer;

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field.
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(RadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
            CoordinateController = new CoordinateCalculationController(radioTelescope.Location);

            overrides = new OverrideSwitchData(radioTelescope);
            radioTelescope.PLCDriver.Overrides = overrides;

            SensorMonitoringThread = new Thread(SensorMonitor);
            SensorMonitoringThread.Start();
            MonitoringSensors = true;
            AllSensorsSafe = true;
            EnableSoftwareStops = true;

            MaxAzTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_TEMP);
            MaxElTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_TEMP);

            previousSnowDumpAzimuth = 0;

            snowDumpTimer = new System.Timers.Timer(DatabaseOperations.FetchWeatherThreshold().SnowDumpTime * 1000 * 60);
            snowDumpTimer.Elapsed += AutomaticSnowDumpInterval;
            snowDumpTimer.AutoReset = true;
            snowDumpTimer.Enabled = true;

            FinalCalibrationOffset = new Orientation(0, 0);
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

            finalOffsetOrientation.Elevation = RadioTelescope.PLCDriver.GetMotorEncoderPosition().Elevation + FinalCalibrationOffset.Elevation;
            finalOffsetOrientation.Azimuth = RadioTelescope.PLCDriver.GetMotorEncoderPosition().Azimuth + FinalCalibrationOffset.Azimuth;

            // Normalize azimuth orientation
            while (finalOffsetOrientation.Azimuth > 360) finalOffsetOrientation.Azimuth -= 360;
            while (finalOffsetOrientation.Azimuth < 0) finalOffsetOrientation.Azimuth += 360;

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

            finalOffsetOrientation.Elevation = RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Elevation + FinalCalibrationOffset.Elevation;
            finalOffsetOrientation.Azimuth = RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Azimuth + FinalCalibrationOffset.Azimuth;

            // Normalize azimuth orientation
            while (finalOffsetOrientation.Azimuth > 360) finalOffsetOrientation.Azimuth -= 360;
            while (finalOffsetOrientation.Azimuth < 0) finalOffsetOrientation.Azimuth += 360;

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
        public bool CancelCurrentMoveCommand(MovementPriority priority)
        {
            bool result = false;


            if(Monitor.TryEnter(MovementLock) && priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
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
            //MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow);
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

                moveResult = RadioTelescope.PLCDriver.MoveToOrientation(MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION, current);

                if (moveResult != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    Monitor.Exit(MovementLock);
                    return moveResult;
                }

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
                moveResult = RadioTelescope.PLCDriver.MoveToOrientation(current, MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION);
                if (moveResult != MovementResult.Success)
                {
                    if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                    RadioTelescope.SpectraCyberController.StopScan();
                    Monitor.Exit(MovementLock);
                    return moveResult;
                }

                // analyze data
                // temperature (Kelvin) = (intensity * time * wein's displacement constant) / (Planck's constant * speed of light)
                double weinConstant = 2.8977729;
                double planckConstant = 6.62607004 * Math.Pow(10, -34);
                double speedConstant = 299792458;
                double temperature = (rfResponse.Intensity * time * weinConstant) / (planckConstant * speedConstant);

                // convert to fahrenheit
                temperature = temperature * (9 / 5) - 459.67;

                // check against weather station reading
                double weatherStationTemp = RadioTelescope.WeatherStation.GetOutsideTemp();

                // Set SpectraCyber mode back to UNKNOWN
                RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);

                // return true if working correctly, false if not
                if (Math.Abs(weatherStationTemp - temperature) < MiscellaneousConstants.THERMAL_CALIBRATION_OFFSET)
                {
                    moveResult = RadioTelescope.PLCDriver.MoveToOrientation(MiscellaneousConstants.Stow, current);
                }

                if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovementPriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;

                RadioTelescope.SpectraCyberController.StopScan();
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

        public bool PastSoftwareStopLimit()
        {
            double position = 0;
            if (RadioTelescope.SensorNetworkServer.SimulationSensorNetwork != null)
            {
                 position = RadioTelescope.PLCDriver.GetMotorEncoderPosition().Elevation;
            }
            else
            {
                position= RadioTelescope.SensorNetworkServer.CurrentCBAccelElevationPosition;
            }

            return (position > RadioTelescope.maxElevationDegrees || position < RadioTelescope.minElevationDegrees);
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public MovementResult MoveRadioTelescopeToOrientation(Orientation orientation, MovementPriority priority)
        {
            MovementResult result = MovementResult.None;

            if (PastSoftwareStopLimit() && (orientation.Elevation > RadioTelescope.maxElevationDegrees || orientation.Elevation < RadioTelescope.minElevationDegrees)) return MovementResult.SoftwareStopHit;

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

                result = RadioTelescope.PLCDriver.MoveToOrientation(orientation, GetCurrentOrientation());
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
            //TODO:verify second argument is correct for conversion to orientation
            Orientation orientation = CoordinateController.CoordinateToOrientation(coordinate, DateTime.Now);

            if (PastSoftwareStopLimit() && (orientation.Elevation > RadioTelescope.maxElevationDegrees || orientation.Elevation < RadioTelescope.minElevationDegrees)) return MovementResult.SoftwareStopHit;

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
            // TODO: Implement (issue #379)
            return MovementResult.None;
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

                // Perform a home telescope movement
                result = RadioTelescope.PLCDriver.HomeTelescope();

                // Zero out absolute encoders
                RadioTelescope.SensorNetworkServer.AbsoluteOrientationOffset = (Orientation)RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Clone();

                // Allow the absolute encoders' positions to even out
                Thread.Sleep(100);

                // Verify the absolute encoders have successfully zeroed out. There is a bit of fluctuation with their values, so homing could have occurred
                // with an outlier value. This check (with half-degree of precision) verifies that did not happen.
                Orientation absOrientation = RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation;
                if ((Math.Abs(absOrientation.Elevation) > 0.5 && !overrides.overrideElevationAbsEncoder) || 
                        (Math.Abs(absOrientation.Azimuth) > 0.5 && !overrides.overrideAzimuthAbsEncoder))
                {
                    result = MovementResult.IncorrectPosition;
                }

                // Apply final calibration offset
                FinalCalibrationOffset = RadioTelescope.CalibrationOrientation;
                RadioTelescope.PLCDriver.SetFinalOffset(FinalCalibrationOffset);

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

                Orientation move1 = new Orientation(origOrientation.Azimuth, 0);
                Orientation move2 = new Orientation(origOrientation.Azimuth, 90);

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

            double position = 0;
            if (RadioTelescope.SensorNetworkServer.SimulationSensorNetwork != null)
            {
                position = RadioTelescope.PLCDriver.GetMotorEncoderPosition().Elevation;
            }
            else
            {
                position = RadioTelescope.SensorNetworkServer.CurrentCBAccelElevationPosition;
            }

            if (axis == RadioTelescopeAxisEnum.ELEVATION && PastSoftwareStopLimit())
            {
                if(direction == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive && position> 80)
                {
                    return MovementResult.SoftwareStopHit;
                }

                else if(direction == RadioTelescopeDirectionEnum.ClockwiseOrNegative && position < RadioTelescope.minElevationDegrees)
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
                    if (RadioTelescope.PLCDriver.Cancel_move()) result = MovementResult.Success;
                }
                else if (stopType == MCUCommandType.ImmediateStop)
                {
                    if (RadioTelescope.PLCDriver.ImmediateStop()) result = MovementResult.Success;
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
        public bool ExecuteRadioTelescopeControlledStop(MovementPriority priority)
        {
            bool success = false;

            if (Monitor.TryEnter(MovementLock))
            {
                if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
                {
                    success = RadioTelescope.PLCDriver.ControlledStop();
                    RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                }
                Monitor.Exit(MovementLock);
            }

            return success;
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to an immediate stop.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRadioTelescopeImmediateStop(MovementPriority priority)
        {
            bool success = false;

            if (Monitor.TryEnter(MovementLock))
            {
                if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
                {
                    success = RadioTelescope.PLCDriver.ImmediateStop();
                    RadioTelescope.PLCDriver.CurrentMovementPriority = MovementPriority.None;
                }
                Monitor.Exit(MovementLock);
            }

            return success;
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

            if(Monitor.TryEnter(MovementLock))
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
        public bool finished_exicuting_move( RadioTelescopeAxisEnum axis )//[7]
        {
             
            var Taz = RadioTelescope.PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH );
            var Tel = RadioTelescope.PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.ELEVATION );
            
            bool azFin = Taz[(int)MCUConstants.MCUStatusBitsMSW.Move_Complete];
            bool elFin = Tel[(int)MCUConstants.MCUStatusBitsMSW.Move_Complete];
            if(axis == RadioTelescopeAxisEnum.BOTH) {
                return elFin && azFin;
            } else if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                return azFin;
            } else if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                return elFin;
            }
            return false;
        }

        // Checks the motor temperatures against acceptable ranges every second
        private void SensorMonitor()
        {
            // Getting initial current temperatures
            Temperature currAzTemp = RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp[RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp.Length - 1];
            Temperature currElTemp = RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp[RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp.Length - 1];
            bool elTempSafe = checkTemp(currElTemp, true);
            bool azTempSafe = checkTemp(currAzTemp, true);

            // Sensor overrides must be taken into account
            bool currentAZOveride = overrides.overrideAzimuthMotTemp;
            bool currentELOveride = overrides.overrideElevatMotTemp;

            // Loop through every one second to get new sensor data
            while (MonitoringSensors)
            {
                Temperature azTemp = RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp[RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp.Length - 1];
                Temperature elTemp = RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp[RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp.Length - 1];

                azTempSafe = checkTemp(azTemp, azTempSafe);
                elTempSafe = checkTemp(elTemp, elTempSafe);

                // Determines if the telescope is in a safe state
                if (azTempSafe && elTempSafe) AllSensorsSafe = true;
                else
                {
                    AllSensorsSafe = false;

                    // If the motors are moving, interrupt the current movement.
                    if (RadioTelescope.PLCDriver.MotorsCurrentlyMoving())
                    {
                        RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped();
                    }
                }

                // Run the software-stop routine
                CheckAndRunSoftwareStops();
                
                Thread.Sleep(1000);
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

                    pushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature BELOW stable temperature by " + Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.");
                    EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature BELOW stable temperature by " + Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.");
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

                    pushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.");
                    EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.");
                }

                // Only overrides if switch is true
                if (!isOverridden) return false;
                else return true;
            }
            else if (t.temp <= SimulationConstants.MAX_MOTOR_TEMP && t.temp >= SimulationConstants.MIN_MOTOR_TEMP && !lastIsSafe) {
                logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature stable.");

                pushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature stable.");
                EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature stable.");
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

                pushNotification.sendToAllAdmins("SENSOR OVERRIDES", "Overriding " + sensor + " sensor.");
                EmailNotifications.sendToAllAdmins("SENSOR OVERRIDES", "Overriding " + sensor + " sensor.");
            }
            else
            {
                logger.Info(Utilities.GetTimeStamp() + ": Enabled " + sensor + " sensor.");

                pushNotification.sendToAllAdmins("SENSOR OVERRIDES", "Enabled " + sensor + " sensor.");
                EmailNotifications.sendToAllAdmins("SENSOR OVERRIDES", "Enabled " + sensor + " sensor.");
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

                Orientation dump = new Orientation(previousSnowDumpAzimuth, -5);
                Orientation current = GetCurrentOrientation();

                Orientation dumpAzimuth = new Orientation(dump.Azimuth, current.Elevation);
                Orientation dumpElevation = new Orientation(dump.Azimuth, dump.Elevation);

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
                if(Math.Abs(currentOrientation.Azimuth - MiscellaneousConstants.Stow.Azimuth) <= DELTA && Math.Abs(currentOrientation.Elevation - MiscellaneousConstants.Stow.Elevation) <= DELTA)
                {
                    Console.WriteLine("Time threshold reached. Running snow dump...");

                    MovementResult result = SnowDump(MovementPriority.Appointment);

                    if(result != MovementResult.Success)
                    {
                        logger.Info($"{Utilities.GetTimeStamp()}: Automatic snow dump FAILED with error message: {result.ToString()}");
                        pushNotification.sendToAllAdmins("Snow Dump Failed", $"Automatic snow dump FAILED with error message: {result.ToString()}");
                        EmailNotifications.sendToAllAdmins("Snow Dump Failed", $"Automatic snow dump FAILED with error message: {result.ToString()}");
                    }
                    else
                    {
                        Console.WriteLine("Snow dump completed");
                    }
                }
                
            }
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

                bool pastSoftwareStopLimit = PastSoftwareStopLimit();

                // Perform a critical movement interrupt if the telescope is moving past either elevation threshold
                if ((pastSoftwareStopLimit && direction == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive) ||
                    (pastSoftwareStopLimit && direction == RadioTelescopeDirectionEnum.ClockwiseOrNegative))
                {
                    RadioTelescope.PLCDriver.InterruptMovementAndWaitUntilStopped(true, true);
                    logger.Info("Software-stop hit!");
                }
            }
        }
    }
}