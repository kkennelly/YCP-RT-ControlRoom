using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    [Table("celestial_body")]
    public class CelestialBody
    {
        public CelestialBody(string name)
        {
            Name = name;
        }

        public CelestialBody(string name, Coordinate coord)
        {
            Name = name;
            Coordinate = coord;
        }

        public CelestialBody() : this(CelestialBodyConstants.NONE) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // not nullable
        [Column("name")]
        public string Name { get; set; }

        // not nullable
        public int coordinate_id { get; set; }
        [ForeignKey("coordinate_id")]
        public virtual Coordinate Coordinate { get; set; }


        public override string ToString()
        {
            return Id + "| " + Name;
        }

    }
}
