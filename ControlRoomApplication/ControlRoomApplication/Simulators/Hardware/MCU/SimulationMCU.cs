using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;

namespace ControlRoomApplication.Simulators.Hardware.MCU
{
    /// <summary>
    ///
    /// </summary>
    /// 
    public class SimulationMCU
    {
        private SimulationAbsoluteEncoder AzEncoder;
        private SimulationAbsoluteEncoder ElEncoder;

        private DateTime ActiveObjectiveOrientationMoveStart;
        private Orientation ActiveObjectiveOrientation;
        private SimulationMCUTrajectoryProfile ActiveObjectiveAzimuthProfile;
        private SimulationMCUTrajectoryProfile ActiveObjectiveElevationProfile;

        private SimulationStopTypeEnum RequestedStopType;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public SimulationMCU(SimulationAbsoluteEncoder azEncoder, SimulationAbsoluteEncoder elEncoder)
        {
            AzEncoder = azEncoder;
            ElEncoder = elEncoder;

            ActiveObjectiveOrientationMoveStart = DateTime.MinValue;
            ActiveObjectiveOrientation = null;
            ActiveObjectiveAzimuthProfile = null;
            ActiveObjectiveElevationProfile = null;

            RequestedStopType = SimulationStopTypeEnum.NONE;
        }

        public SimulationMCU(int azEncoderPrecision, int elEncoderPrecision)
            : this(new SimulationAbsoluteEncoder(azEncoderPrecision), new SimulationAbsoluteEncoder(elEncoderPrecision)) {}

        public Orientation GetActiveObjectiveOrientation()
        {
            return ActiveObjectiveOrientation;
        }

        public bool HasActiveMove()
        {
            return ActiveObjectiveOrientation != null;
        }

        public Orientation GetCurrentOrientationInDegrees()
        {
            if (HasActiveMove())
            {
                Orientation updated = UpdatePositionsToNow();

                if (updated.Equals(ActiveObjectiveOrientation))
                {
                    ActiveObjectiveOrientation = null;
                }

                return updated;
            }
            else
            {
                return new Orientation(AzEncoder.CurrentPositionDegrees, ElEncoder.CurrentPositionDegrees);
            }
        }

        private void TryStop(SimulationStopTypeEnum stopType)
        {
            RequestedStopType = HasActiveMove() ? stopType : SimulationStopTypeEnum.NONE;
        }

        public void ExecuteControlledStop()
        {
            TryStop(SimulationStopTypeEnum.CONTROLLED);
        }

        public void ExecuteImmediateStop()
        {
            TryStop(SimulationStopTypeEnum.IMMEDIATE);
        }

        public void SetActiveObjectiveOrientationAndStartMove(Orientation orientationDegrees, bool forceLinear)
        {
            RequestedStopType = SimulationStopTypeEnum.NONE;
            ActiveObjectiveOrientation = orientationDegrees;

            if (forceLinear)
            {
                ActiveObjectiveAzimuthProfile = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                    AzEncoder,
                    AzEncoder.CurrentPositionDegrees,
                    0.0,
                    MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                    MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                    orientationDegrees.azimuth
                );

                ActiveObjectiveElevationProfile = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                    ElEncoder,
                    ElEncoder.CurrentPositionDegrees,
                    0.0,
                    MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                    MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                    orientationDegrees.elevation
                );
            }
            else
            {
                ActiveObjectiveAzimuthProfile = SimulationMCUTrajectoryProfile.CalculateInstance(
                    AzEncoder,
                    AzEncoder.CurrentPositionDegrees,
                    0.0,
                    MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                    MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                    orientationDegrees.azimuth
                );

                ActiveObjectiveElevationProfile = SimulationMCUTrajectoryProfile.CalculateInstance(
                    ElEncoder,
                    ElEncoder.CurrentPositionDegrees,
                    0.0,
                    MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                    MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                    orientationDegrees.elevation
                );
            }

            ActiveObjectiveOrientationMoveStart = DateTime.UtcNow;
        }

        private Orientation UpdatePositionsToNow()
        {
            DateTime CoordinatedEvaluationTime = DateTime.UtcNow;

            Orientation NewPosition = new Orientation(
                ActiveObjectiveAzimuthProfile.InterpretDegreesAt(AzEncoder, ActiveObjectiveOrientationMoveStart, CoordinatedEvaluationTime),
                ActiveObjectiveElevationProfile.InterpretDegreesAt(ElEncoder, ActiveObjectiveOrientationMoveStart, CoordinatedEvaluationTime)
            );

            AzEncoder.SetPositionFromDegrees(NewPosition.azimuth);
            ElEncoder.SetPositionFromDegrees(NewPosition.elevation);

            return NewPosition;
        }
    }
}