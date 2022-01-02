using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    [Table("counterbalance_acceleration_blob")]
    public class CounterbalanceAccelerationBlob : AccelerationBlob
    {
        public CounterbalanceAccelerationBlob()
        {
            //StringBuilder added to create LongString of accelerartion to be strored in the databse seperated by -
            NumberDataPoints = 0;
            Location = SensorLocationEnum.COUNTERBALANCE;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        [Column("acc_blob")]
        public override Byte[] BlobArray { get; set; }

        [NotMapped]
        public override List<Byte> BlobList { get; set; }

        [Required]
        [Column("first_time_captured")]
        public override long FirstTimeCaptured { get; set; }

        [NotMapped]
        public int NumberDataPoints { get; set; }

        [NotMapped]
        public override SensorLocationEnum Location { get; set; }
    }
}