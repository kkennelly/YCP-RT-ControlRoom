using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("coordinate")]
    public class Coordinate
    {
        public Coordinate(double rightAscension, double declination)
        {
            right_ascension = rightAscension;
            this.declination = declination;
            appointment_id = -1;
        }

        public Coordinate() : this(0, 0) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("right_ascension")]
        public double right_ascension { get; set; }

        [Required]
        [Column("declination")]
        public double declination { get; set; }

        [Required]
        [Column("hours")]
        public int hours { get; set; }

        [Required]
        [Column("minutes")]
        public int minutes { get; set; }
        
        [Column("appointment_id")]
        public int appointment_id { get; set; }

        public override string ToString()
        {
            return Id + "| RA: " + right_ascension + " D: " + declination;
        }
    }
}