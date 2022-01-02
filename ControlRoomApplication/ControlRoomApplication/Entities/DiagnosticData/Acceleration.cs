using ControlRoomApplication.Entities.DiagnosticData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities {
    [Table( "Acceleration" )]
    public class Acceleration : IEquatable<Acceleration>
    {
        public Acceleration() {

        }

        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }


        [Required]
        [Column( "time_captured" )]
        public long TimeCaptured { get; set; }

        [Required]
        [Column( "acceleration_magnitude" )]
        public double acc { get; set; }

        [Required]
        [Column( "acceleration_x" )]
        public int x { get; set; }

        [Required]
        [Column("acceleration_y")]
        public int y { get; set; }

        [Required]
        [Column("acceleration_z")]
        public int z { get; set; }

        [Required]
        [Column( "location" )]
        public int location_ID { get; set; }

        public static Acceleration Generate( long UTCtics , int x , int y , int z , SensorLocationEnum loc ) {
            Acceleration acx = new Acceleration();
            acx.TimeCaptured = UTCtics;// Constants.TIME.UnixEpoch.AddMilliseconds( UTCtics );
            acx.x = x;
            acx.y = y;
            acx.z = z;
            acx.location_ID = (int)loc;
            acx.acc = Math.Sqrt(x * x + y * y + z * z);
            return acx;
        }

        public bool Equals(Acceleration other)
        {
            if (x != other.x || y != other.y || z != other.z || location_ID != other.location_ID)
            {
                return false;
            }
            return true;
        }

        public static bool SequenceEquals(Acceleration[] start , Acceleration[] other)
        {
            if(start.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < start.Length; i++)
            {
                if (start[i].x != other[i].x || start[i].y != other[i].y || start[i].z != other[i].z || start[i].location_ID != other[i].location_ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

