using System;
using System.Collections.Generic;

namespace ControlRoomApplication.Entities.Encoder
{
    public class EncoderAverages
    {
        // Capacity is the number of readings we want to compare
        private const int _capacity = 500;
        // Max number of allowable degrees difference in readings
        private const double _maxDegrees = 1.5;
        // Max errors is the percent error that we would like based on the number of readings we save in the queue (capacity)
        public const int maxErrors = (int)(_capacity * 0.4);

        private double motorElAvg, motorAzAvg, absoluteElAvg, absoluteAzAvg = 0.0;

        public int NumErrors { get; set; }
        public Queue<Orientation> AbsoluteEncoder { get; private set; }
        public Queue<Orientation> MotorEncoder { get; private set; }

        public EncoderAverages()
        {
            AbsoluteEncoder = new Queue<Orientation>(_capacity);
            MotorEncoder = new Queue<Orientation>(_capacity);
            NumErrors = 0;
        }

        /// <summary>
        /// Adds a given orientation to the queue
        /// </summary>
        /// <param name="absolute">
        /// The absolute orientation to be added
        /// </param>
        /// <param name="motor">
        /// The motor orientation to be added
        /// </param>
        /// <returns>
        /// True if successfully adds orientation, false if orientation out of range
        /// </returns>
        public bool AddOrientation(Orientation absolute, Orientation motor)
        {
            if (CompareOrientation(absolute, motor))
            {
                while (AbsoluteEncoder.Count >= _capacity)
                {
                    Orientation prevAbsoluteOrientation = AbsoluteEncoder.Dequeue();

                    // Update averages by removing old values.
                    absoluteAzAvg -= ((prevAbsoluteOrientation.Azimuth + 180) % 360 - 180) / _capacity;
                    absoluteElAvg -= (Math.Abs(prevAbsoluteOrientation.Elevation) / _capacity);
                }
                while (MotorEncoder.Count >= _capacity)
                {
                    Orientation prevMotorOrientation = MotorEncoder.Dequeue();

                    // Update averages based on new values.
                    motorAzAvg -= ((prevMotorOrientation.Azimuth + 180) % 360 - 180) / _capacity;
                    motorElAvg -= (Math.Abs(prevMotorOrientation.Elevation) / _capacity);
                }

                // TODO: Test to make sure that this way of enqueuing new orientations produces non-empty values *thumbs up* LGTM
                AbsoluteEncoder.Enqueue(new Orientation { Azimuth = absolute.Azimuth, Elevation = absolute.Elevation, Id = absolute.Id });
                MotorEncoder.Enqueue(new Orientation { Azimuth = motor.Azimuth, Elevation = motor.Elevation, Id = motor.Id });

                // Add new values to running average.
                absoluteAzAvg += ((absolute.Azimuth + 180) % 360 - 180) / _capacity;
                absoluteElAvg += (Math.Abs(absolute.Elevation) / _capacity);
                motorAzAvg += ((motor.Azimuth + 180) % 360 - 180) / _capacity;
                motorElAvg += (Math.Abs(motor.Elevation) / _capacity);

                NumErrors = (NumErrors > 0) ? NumErrors - 1 : 0;

                Console.WriteLine("************BROKE ERROR CHAIN****************");

                return true;
            }

            if (NumErrors == 0)
                Console.WriteLine("*************STARTED ERROR CHAIN****************");
            NumErrors++;
            if (NumErrors == maxErrors / 2)
                Console.WriteLine("****************CRITICAL NUM ERRORS***************");

            return false;
        }

        // returns false if not a valid orientation, returns true if valid
        private bool CompareOrientation(Orientation absolute, Orientation motor)
        {
            // If not at capacity, return true. We want to fill the queue before acting on data. 
            if (AbsoluteEncoder.Count >= _capacity || MotorEncoder.Count >= _capacity)
            {
                if (
                    Math.Abs(absolute.Azimuth - absoluteAzAvg) > _maxDegrees*NumErrors ||
                    Math.Abs(absolute.Elevation - absoluteElAvg) > _maxDegrees*NumErrors ||
                    Math.Abs(((motor.Azimuth + 180) % 360 - 180) - motorAzAvg) > _maxDegrees*NumErrors ||
                    Math.Abs(Math.Abs(motor.Elevation) - motorElAvg) > _maxDegrees*NumErrors)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
