using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("weather_data")]
    public class WeatherData
    {
        public WeatherData()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column("time_captured")]
        public long TimeCaptured { get; set; }

        [Required]
        [Column("outside_temperature_degF")]
        public Single ot { get; set; }

        [Required]
        [Column("inside_temperature_degF")]
        public Single it { get; set; }

        [Required]
        [Column("rain_rate")]
        public Single rr { get; set; }

        [Required]
        [Column("rain_total")]
        public Single rt { get; set; }

        [Required]
        [Column("rain_day")]
        public Single rd { get; set; }

        [Required]
        [Column("rain_month")]
        public Single rm { get; set; }

        [Required]
        [Column("barometric_pressure")]
        public Single bp { get; set; }

        [Required]
        [Column("dew_point")]
        public Single dp { get; set; }

        [Required]
        [Column("wind_chill")]
        public Single wc { get; set; }

        [Required]
        [Column("humidity")]
        public Single hum { get; set; }

        [Required]
        [Column("heat_index")]
        public Single heat { get; set; }



        public static WeatherData Generate(AbstractWeatherStation.Weather_Data data)
        {
            WeatherData dbData = new WeatherData();

            dbData.TimeCaptured = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            dbData.rd = data.dailyRain;
            dbData.rr = data.rainRate;
            dbData.ot = data.outsideTemp;
            dbData.it = data.insideTemp;
            dbData.bp = data.baromPressure;
            dbData.dp = data.dewPoint;
            dbData.wc = data.windChill;
            dbData.hum = data.outsideHumidity;
            dbData.rt = data.totalRain;
            dbData.rm = data.monthlyRain;
            dbData.heat = data.heatIndex;

            return dbData;
        }
    }
}

