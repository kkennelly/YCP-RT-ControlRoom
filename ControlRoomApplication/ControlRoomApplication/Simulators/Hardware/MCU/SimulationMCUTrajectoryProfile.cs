using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;

namespace ControlRoomApplication.Simulators.Hardware.MCU
{
    public class SimulationMCUTrajectoryProfile
    {
        // This describes the type of profile that this trajectory is abiding by
        public SimulationMCUTrajectoryProfileTypeEnum ProfileType { get; }

        // This is the initial velocity of the move
        // This value is in steps/seconds
        public double InitialVelocity { get; }

        // This is the maximum velocity, the azSpeed that the profile will have during its constant velocity sequence
        // This value is in steps/second, where "steps" is in a domain like encoder tick positions
        // This is the magnitude, do not make this negative!
        public double TrajectoryPeakVelocity { get; }

        // This is the maximum acceleration and deceleration that the profile will abide by
        // This value is in steps/second^2, where "steps" is in a domain like encoder tick positions
        // This is the magnitude, do not make this negative!
        public double TrajectoryPeakAcceleration { get; }

        // This is the jerk throughout the move that the profile will abide by
        // This value is in steps/second^3, where "steps" is in a domain like encoder tick positions
        public double TrajectoryJerk { get; }

        // This is the calculated amount of time that it will take for the entire acceleration phase of a profile
        // This value is in seconds
        public double TimeForAcceleration { get; }

        // This is the calculated amount of time that the move should take, according to the kinematic characteristics of the profile
        // This value is in seconds
        public double TotalTime { get; }

        // This is the initial absolute step position in this axis
        public int InitialStep { get; }

        // This is the objective absolute step position in this axis
        public int ObjectiveStep { get; }

        // Private constructor, so the factory function has to be used
        private SimulationMCUTrajectoryProfile(
            SimulationMCUTrajectoryProfileTypeEnum type,
            double initialVelocity,
            double peakVelocity,
            double peakAcceleration,
            double accelerationTime,
            double totalTime,
            int initialStep,
            int objectiveStep)
        {
            ProfileType = type;
            InitialVelocity = initialVelocity;
            TrajectoryPeakVelocity = peakVelocity;
            TrajectoryPeakAcceleration = peakAcceleration;
            TrajectoryJerk = TrajectoryPeakAcceleration / accelerationTime;
            TimeForAcceleration = accelerationTime;
            TotalTime = totalTime;
            InitialStep = initialStep;
            ObjectiveStep = objectiveStep;
        }

        public double InterpretDegreesAt(SimulationAbsoluteEncoder motorEncoder, DateTime startTime, DateTime evaluationTime)
        {
            return motorEncoder.GetEquivalentDegreesFromEncoderTicks(InterpretEncoderTicksAt(startTime, evaluationTime));
        }

        public int InterpretEncoderTicksAt(DateTime startTime, DateTime evaluationTime)
        {
            double timeElapsed = (evaluationTime - startTime).TotalSeconds;

            if (timeElapsed < 0)
            {
                throw new ArgumentException("Evaluation time was before the start time.");
            }

            bool isNegative = ObjectiveStep < InitialStep;

            switch (ProfileType)
            {
                case SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE:
                    {
                        return ObjectiveStep;
                    }
                case SimulationMCUTrajectoryProfileTypeEnum.LINEAR_FULL:
                    {
                        double cumulativePosition = InitialStep;

                        if (timeElapsed < TimeForAcceleration)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, TrajectoryPeakAcceleration, 0, timeElapsed, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(0, TrajectoryPeakAcceleration, 0, TimeForAcceleration, isNegative);
                        double cumulativeVelocity = GetTickVelocityChange(0, TrajectoryPeakAcceleration, TimeForAcceleration);

                        if (timeElapsed < (TotalTime - TimeForAcceleration))
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, 0, cumulativeVelocity, timeElapsed - TimeForAcceleration, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(0, 0, cumulativeVelocity, TotalTime - (2 * TimeForAcceleration), isNegative);
                        // cumulativeVelocity += GetTickVelocityChange(0, 0, TimeForAcceleration); --> This value is obviously 0, but I'm leaving it here to clarify I didn't forget it!

                        if (timeElapsed < TotalTime)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - TotalTime + TimeForAcceleration, isNegative));
                        }

                        return ObjectiveStep;
                    }

                case SimulationMCUTrajectoryProfileTypeEnum.LINEAR_PARTIAL:
                    {
                        double cumulativePosition = InitialStep;

                        if (timeElapsed < TimeForAcceleration)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, TrajectoryPeakAcceleration, 0, timeElapsed, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(0, TrajectoryPeakAcceleration, 0, TimeForAcceleration, isNegative);
                        double cumulativeVelocity = GetTickVelocityChange(0, TrajectoryPeakAcceleration, TimeForAcceleration);

                        if (timeElapsed < TotalTime)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - TimeForAcceleration, isNegative));
                        }

                        return ObjectiveStep;
                    }

                case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL:
                    {
                        double cumulativePosition = InitialStep;

                        double t1 = TrajectoryPeakAcceleration / TrajectoryJerk;
                        if (timeElapsed < t1)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(TrajectoryJerk, 0, 0, timeElapsed, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(TrajectoryJerk, 0, 0, t1, isNegative);
                        double cumulativeVelocity = GetTickVelocityChange(TrajectoryJerk, 0, t1);
                        double t2 = ((TrajectoryPeakVelocity - (2 * cumulativeVelocity)) / TrajectoryPeakAcceleration) + t1;
                        if (timeElapsed < t2)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t1, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(0, TrajectoryPeakAcceleration, cumulativeVelocity, t2 - t1, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(0, TrajectoryPeakAcceleration, t2 - t1);
                        double t3 = t2 + t1;
                        if (timeElapsed < t3)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t1, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t2, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, TrajectoryPeakAcceleration, timeElapsed - t2);
                        double t4 = TotalTime - (2 * t3);
                        if (timeElapsed < t4)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, 0, cumulativeVelocity, timeElapsed - t3, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(0, 0, cumulativeVelocity, t4 - t3, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(0, 0, t4 - t3);
                        double t5 = t4 + t1;
                        if (timeElapsed < t5)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, timeElapsed - t4, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, t5 - t4, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, 0, t5 - t4);
                        double t6 = t4 + t2;
                        if (timeElapsed < t6)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(0, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t5, isNegative));
                        }

                        cumulativePosition += GetTickTranslation(0, -TrajectoryPeakAcceleration, cumulativeVelocity, t6 - t5, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(0, -TrajectoryPeakAcceleration, t6 - t5);
                        if (timeElapsed < TotalTime)
                        {
                            return (int)(cumulativePosition + GetTickTranslation(TrajectoryJerk, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t6, isNegative));
                        }

                        return ObjectiveStep;
                    }

                case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_FULL:
                case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_PARTIAL:
                    {
                        double t1 = TotalTime / 4;
                        if (timeElapsed < t1)
                        {
                            return (int)(GetTickTranslation(TrajectoryJerk, 0, 0, timeElapsed, isNegative));
                        }

                        double cumulativeDisplacement = GetTickTranslation(TrajectoryJerk, 0, 0, t1, isNegative);
                        double cumulativeVelocity = GetTickVelocityChange(TrajectoryJerk, 0, t1);
                        double t2 = t1;

                        double t3 = 2 * t1;
                        if (timeElapsed < t3)
                        {
                            return (int)(cumulativeDisplacement + GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t1, isNegative));
                        }

                        cumulativeDisplacement += GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t2, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, TrajectoryPeakAcceleration, timeElapsed - t2);
                        double t4 = t3;

                        double t5 = 3 * t1;
                        if (timeElapsed < t5)
                        {
                            return (int)(cumulativeDisplacement + GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, timeElapsed - t4, isNegative));
                        }

                        cumulativeDisplacement += GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, t5 - t4, isNegative);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, 0, t5 - t4);
                        double t6 = t5;

                        if (timeElapsed < TotalTime)
                        {
                            return (int)(cumulativeDisplacement + GetTickTranslation(TrajectoryJerk, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t6, isNegative));
                        }

                        return ObjectiveStep;
                    }

                default:
                    {
                        throw new InvalidOperationException("This profile is of an unrecognized type: " + ProfileType.ToString());
                    }
            }
        }

        private static double GetTickTranslation(double j, double a, double v, double dt, bool n)
        {
            double d = ((((j * dt / 6) + (a / 2)) * dt) + v) * dt;
            return n ? -d : d;
        }

        private static double GetTickVelocityChange(double j, double a, double dt)
        {
            return ((j * dt / 2) + a) * dt;
        }

        // This factory function utilizes the equations on page 20 and 21 of the 2-axis MCU documentation to form a trajectory profile
        // with a linear acceleration to reach the peak programmed azSpeed
        public static SimulationMCUTrajectoryProfile ForceLinearInstance(
            SimulationAbsoluteEncoder motorEncoder,
            double initialDegrees,
            double initialVelocity,
            double peakVelocity,
            double acceleration,
            double objectiveDegrees)
        {
            int initialSteps = motorEncoder.GetEquivalentEncoderTicksFromDegrees(initialDegrees);
            int objectiveSteps = motorEncoder.GetEquivalentEncoderTicksFromDegrees(objectiveDegrees);
            int changeInSteps = objectiveSteps - initialSteps;
            int absoluteChangeInSteps = Math.Abs(changeInSteps);

            int minimumTicksNeededToReachPeakSpeed = (int)(((peakVelocity * peakVelocity) - (initialVelocity * initialVelocity)) / (2 * acceleration));
            int minimumTicksNeededForAcceleration = 2 * minimumTicksNeededToReachPeakSpeed;

            // See if this move is even worth the effort of TRYING to move
            if (Math.Abs(motorEncoder.GetEquivalentDegreesFromEncoderTicks(changeInSteps)) < MiscellaneousConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES)
            {
                return new SimulationMCUTrajectoryProfile(SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE, initialVelocity, peakVelocity, acceleration, 0.0, 0.0, initialSteps, objectiveSteps);
            }
            else if (absoluteChangeInSteps >= minimumTicksNeededForAcceleration)
            {
                double timeRequiredForAcceleration = (peakVelocity - initialVelocity) / acceleration;
                double totalTime = (2 * timeRequiredForAcceleration) + ((absoluteChangeInSteps - minimumTicksNeededForAcceleration) / peakVelocity);

                return new SimulationMCUTrajectoryProfile(
                    SimulationMCUTrajectoryProfileTypeEnum.LINEAR_FULL,
                    initialVelocity,
                    peakVelocity,
                    acceleration,
                    timeRequiredForAcceleration,
                    totalTime,
                    initialSteps,
                    objectiveSteps
                );
            }
            else
            {
                double actualPeakVelocity = Math.Sqrt((initialVelocity * initialVelocity) + (acceleration * absoluteChangeInSteps));
                double actualAccelerationTime = (actualPeakVelocity - initialVelocity) / acceleration;

                return new SimulationMCUTrajectoryProfile(
                    SimulationMCUTrajectoryProfileTypeEnum.LINEAR_PARTIAL,
                    initialVelocity,
                    actualPeakVelocity,
                    acceleration,
                    actualAccelerationTime,
                    2 * actualAccelerationTime,
                    initialSteps,
                    objectiveSteps
                );
            }
        }

        // This factory function utilizes the equations on page 20 and 21 of the 2-axis MCU documentation to calculate the type of
        // trajectory profile associated with the possible move type, as well as the characteristics of said profile
        public static SimulationMCUTrajectoryProfile CalculateInstance(
            SimulationAbsoluteEncoder motorEncoder,
            double initialDegrees,
            double initialVelocity,
            double peakVelocity,
            double peakAcceleration,
            double objectiveDegrees)
        {
            //    int initialSteps = motorEncoder.GetEquivalentEncoderTicksFromDegrees(initialDegrees);
            //    int objectiveSteps = motorEncoder.GetEquivalentEncoderTicksFromDegrees(objectiveDegrees);
            //    int changeInSteps = objectiveSteps - initialSteps;

            //    SimulationMCUTrajectoryProfile resultingProfile;

            //    // See if this move is even worth the effort of TRYING to move
            //    if (Math.Abs(motorEncoder.GetEquivalentDegreesFromEncoderTicks(changeInSteps)) < HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES)
            //    {
            //        resultingProfile = new SimulationMCUTrajectoryProfile(SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE, initialVelocity, peakVelocity, peakAcceleration, 0.0, 0.0, initialSteps, objectiveSteps);
            //    }
            //    else
            //    {
            //        // The move is substantial enough for us to want to move...
            //        // Try to make the most optimal types of trajectories work
            //        double timeRequiredForAcceleration = 4 * (peakVelocity - initialVelocity) / (3 * peakAcceleration);
            //        double distanceRequiredForAcceleration = timeRequiredForAcceleration * (peakVelocity + initialVelocity) / 2;
            //        int actualDistanceRequired = (int)(2 * distanceRequiredForAcceleration);

            //        // See if it can be a trapezoidal s-curve
            //        if (actualDistanceRequired < changeInSteps)
            //        {
            //            double remainingDistanceForConstantVelocity = changeInSteps - (2 * distanceRequiredForAcceleration);
            //            double remainingTimeForConstantVelocity = peakVelocity / remainingDistanceForConstantVelocity;

            //            resultingProfile = new SimulationMCUTrajectoryProfile(
            //                SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL,
            //                initialVelocity,
            //                peakVelocity,
            //                peakAcceleration,
            //                timeRequiredForAcceleration,
            //                (2 * timeRequiredForAcceleration) + remainingTimeForConstantVelocity,
            //                initialSteps,
            //                objectiveSteps
            //            );
            //        }

            //        // Did not fit the full trapezoidal s-curve requirements, try a triangular s-curve
            //        else if (changeInSteps == actualDistanceRequired)
            //        {
            //            resultingProfile = new SimulationMCUTrajectoryProfile(
            //                SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_FULL,
            //                initialVelocity,
            //                peakVelocity,
            //                peakAcceleration,
            //                timeRequiredForAcceleration,
            //                2 * timeRequiredForAcceleration,
            //                initialSteps,
            //                objectiveSteps
            //            );
            //        }

            //        // Did not fit the full trapezoidal or triangular s-curve requirements, it's a partial triangular s-curve
            //        else
            //        {
            //            double fixedPeakVelocity = Math.Sqrt((0.75 * distanceRequiredForAcceleration * peakAcceleration) + (initialVelocity * initialVelocity));
            //            double fixedTimeRequiredForAcceleration = 4 * (fixedPeakVelocity - initialVelocity) / (3 * peakAcceleration);

            //            resultingProfile = new SimulationMCUTrajectoryProfile(
            //                SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_PARTIAL,
            //                initialVelocity,
            //                fixedPeakVelocity,
            //                peakAcceleration,
            //                fixedTimeRequiredForAcceleration,
            //                2 * fixedTimeRequiredForAcceleration,
            //                initialSteps,
            //                objectiveSteps
            //            );
            //        }
            //    }

            //    return resultingProfile;
            throw new NotImplementedException("Logic is here, but tests are still failing, so more work needs to be done - use linear instance for now.");
        }
    }
}
