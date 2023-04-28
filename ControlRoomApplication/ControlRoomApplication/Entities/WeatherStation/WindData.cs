using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("wind_data")]
    public class WindData
    {
        public WindData()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("wind_speed")]
        public Single ws { get; set; }

        [Required]
        [Column("wind_direction_deg")]
        public Single wd_deg { get; set; }

        [Required]
        [Column("wind_direction_str")]
        public String wd_str { get; set; }

        [Required]
        [Column("time_captured")]
        public long TimeCaptured { get; set; }

        public static WindData Generate(AbstractWeatherStation.Weather_Data data)
        {
            WindData dbData = new WindData();

            dbData.ws = data.windSpeed;
            dbData.wd_str = data.windDirection;
            dbData.wd_deg = data.windDirectionDegrees;
            dbData.TimeCaptured = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return dbData;
        }
    }
}
