using System;
using System.Collections.Generic;

namespace ControlRoomApplication.Entities.Encoder
{
    class EncoderAverages
    {
        // Capacity is the number of readings we want to compare
        private const int _capacity = 10;
        // Max number of allowable degrees difference in readings
        private const double _maxDegrees = 1.5;
        // Max errors is the percent error that we would like based on the number of readings we save in the queue (capacity)
        public const int maxErrors = (int) (_capacity * 0.4);

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
                    AbsoluteEncoder.Dequeue();
                }
                while (MotorEncoder.Count >= _capacity)
                {
                    MotorEncoder.Dequeue();
                }

                // TODO: MAKE THIS FUCKING ENQUEUE THESE STUPID ASS FUCKING VALUES, IT JUST ENQUES EMPTY BALLS ASS VALUES WHAT THE FUCK
                AbsoluteEncoder.Enqueue(new Orientation { Azimuth = absolute.Azimuth, Elevation = absolute.Elevation, Id = absolute.Id });
                MotorEncoder.Enqueue(new Orientation { Azimuth = motor.Azimuth, Elevation = motor.Elevation, Id = motor.Id });

                return true;
            }

            return false;
        }

        // returns false if not a valid orientation, returns true if valid
        private bool CompareOrientation(Orientation absolute, Orientation motor)
        {
            if (!(MotorEncoder.Count < _capacity) || !(AbsoluteEncoder.Count < _capacity))
            {
                var absoluteAverage = new Orientation();
                var motorAverage = new Orientation();

                foreach (var orientation in AbsoluteEncoder)
                {
                    absoluteAverage.Azimuth += Math.Abs((orientation.Azimuth + 180.0) % 360 - 180);
                    absoluteAverage.Elevation += orientation.Elevation;
                }
                absoluteAverage.Azimuth /= AbsoluteEncoder.Count;
                absoluteAverage.Elevation /= AbsoluteEncoder.Count;

                foreach (var orientation in MotorEncoder)
                {
                    motorAverage.Azimuth += Math.Abs((orientation.Azimuth + 180.0) % 360 - 180);
                    motorAverage.Elevation += orientation.Elevation;
                }
                motorAverage.Azimuth /= MotorEncoder.Count;
                motorAverage.Elevation /= MotorEncoder.Count;

                if (
                    //Math.Abs(absolute.Azimuth - absoluteAverage.Azimuth) > _maxDegrees || 
                    //Math.Abs(absolute.Elevation - absoluteAverage.Elevation) > _maxDegrees ||
                    Math.Abs(motor.Azimuth - motorAverage.Azimuth) > _maxDegrees ||
                    Math.Abs(motor.Elevation - motorAverage.Elevation) > _maxDegrees)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
