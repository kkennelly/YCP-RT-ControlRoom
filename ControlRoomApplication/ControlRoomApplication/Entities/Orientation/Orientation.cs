using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    [Table("orientation")]
    [Serializable]
    public class Orientation : ICloneable
    {
        public Orientation(double azimuth, double elevation)
        {
            this.azimuth = azimuth;
            this.elevation = elevation;
        }

        public Orientation() : this(0, 0) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("azimuth")]
        public double azimuth { get; set; }

        [Required]
        [Column("elevation")]
        public double elevation { get; set; }

        /// <summary>
        /// Checks if the current Orientation is Equal to another Orientation  
        /// and it checks if the other Orientation is null
        /// </summary>
        public override bool Equals(object obj)
        {
            Orientation other = obj as Orientation; //avoid double casting
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // These are based off of 12 and 10 bit encoder precisions, respectively
            bool az_equal = Math.Abs(azimuth - other.azimuth) < (360.0 / 4096);
            bool el_equal = Math.Abs(elevation - other.elevation) < (360.0 / 1024);
            return az_equal && el_equal;
        }

        /// <summary>
        /// Checks if the current orientation is valid, based off of the max/min limit switch degrees
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool orientationValid()
        {
            if (
                elevation > SimulationConstants.LIMIT_HIGH_EL_DEGREES ||
                elevation < SimulationConstants.LIMIT_LOW_EL_DEGREES
                )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the HashCode of the Orientation's Id
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// An extension function to duplicate (clone) an orientation.
        /// </summary>
        /// <returns>Returns a new orientation identical to the object in which it was called.</returns>
        public object Clone()
        {
            return new Orientation(azimuth, elevation);
        }

        public override string ToString()
        {
            return Id + "| A: " + azimuth + " E: " + elevation;
        }
    }
}