using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities.DiagnosticData
{
    /// <summary>
    /// This class is for any humidity readings that we aquire and store. It's primary use is for the DHT22 sensor
    /// humidity readings.
    /// </summary>
    [Table("Humidity")]
    public class Humidity : IEquatable<Humidity>
    {
        public Humidity()
        {

        }

        /// <summary>
        /// Database generated ID.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// UTC time captured ms.
        /// </summary>
        [Required]
        [Column("time_captured")]
        public long TimeCapturedUTC { get; set; }

        /// <summary>
        /// Percent humidity read by the DHT22.
        /// </summary>
        [Required]
        [Column("humidity")]
        public double HumidityReading { get; set; }

        /// <summary>
        /// The location ID coresponding to the location the data was sampled in the SensorLocationEnum.
        /// </summary>
        [Required]
        [Column("location")]
        public int LocationID { get; set; }

        /// <summary>
        /// Generate a humidity sample from the specified input data.
        /// </summary>
        /// <param name="UTCtics">The UTC ms that the data was sampled at.</param>
        /// <param name="humidity">The percent humidity of the data sample.</param>
        /// <param name="loc">The location that the sampling took place.</param>
        /// <returns>A humidity object generated from the input data.</returns>
        public static Humidity Generate(long UTCtics, double humidity, SensorLocationEnum loc)
        {
            Humidity temp = new Humidity();
            temp.TimeCapturedUTC = UTCtics;
            temp.HumidityReading = humidity;
            temp.LocationID = (int)loc;
            return temp;
        }

        /// <summary>
        /// A helper method to compare two humidity samples for equality.
        /// </summary>
        /// <param name="other">The other humidity sample to compare to.</param>
        /// <returns>True if the other humidity sample is equal, false if it is not.</returns>
        public bool Equals(Humidity other)
        {
            return HumidityReading == other.HumidityReading && LocationID == other.LocationID;
        }
    }
}
