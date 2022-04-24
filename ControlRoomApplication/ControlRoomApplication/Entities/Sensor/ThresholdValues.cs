using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("thresholds")]
    public class ThresholdValues
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public SensorItemEnum _Name
        {
            get
            {
                return (SensorItemEnum)Enum.Parse(typeof(SensorItemEnum), sensor_name);
            }
            set
            {
                this.sensor_name = value.ToString();
            }
        }

        private string backingName { get; set; }

        [Required]
        [Column("sensor_name")]
        public string sensor_name
        {
            get
            {
                return this.backingName;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(SensorItemEnum), value))
                {
                    this.backingName = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        [Required]
        [Column("maximum")]
        public Single maxValue { get; set; }

        [Required]
        [Column("minimum")]
        public Single minValue { get; set; }
    }
}
