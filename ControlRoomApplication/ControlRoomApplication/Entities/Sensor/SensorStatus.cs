using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("sensor_status")]
    public class SensorStatus
    {
        public SensorStatus()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column("gate")]
        public SByte gate { get; set; }

        [Required]
        [Column("az_motor_temp_1")]
        public SByte az_motor_temp_1 { get; set; }

        [Required]
        [Column("az_motor_temp_2")]
        public SByte az_motor_temp_2 { get; set; }

        [Required]
        [Column("el_motor_temp_1")]
        public SByte el_motor_temp_1 { get; set; }

        [Required]
        [Column("el_motor_temp_2")]
        public SByte el_motor_temp_2 { get; set; }

        [Required]
        [Column("weather_station")]
        public SByte weather_station { get; set; }

        [Required]
        [Column("elevation_abs_encoder")]
        public SByte elevation_abs_encoder { get; set; }

        [Required]
        [Column("azimuth_abs_encoder")]
        public SByte azimuth_abs_encoder { get; set; }

        [Required]
        [Column("el_proximity_0")]
        public SByte el_proximity_0 { get; set; }

        [Required]
        [Column("el_proximity_90")]
        public SByte el_proximity_90 { get; set; }

        [Required]
        [Column("az_accel")]
        public SByte az_accel { get; set; }

        [Required]
        [Column("el_accel")]
        public SByte el_accel { get; set; }

        [Required]
        [Column("counter_balance_accel")]
        public SByte counter_balance_accel { get; set; }

        [Required]
        [Column("ambient_temp_humidity")]
        public SByte ambient_temp_humidity { get; set; }


        public static SensorStatus Generate(SensorStatusEnum gateEnum, SensorStatusEnum azTemp1Enum, 
            SensorStatusEnum azTemp2Enum, SensorStatusEnum elTemp1Enum, SensorStatusEnum elTemp2Enum, SensorStatusEnum weatherEnum,
            SensorStatusEnum elAbsEncoderEnum, SensorStatusEnum azAbsEncoderEnum, SensorStatusEnum elProximity0Enum,
            SensorStatusEnum elProximity90Enum, SensorStatusEnum azAccelEnum, SensorStatusEnum elAccelEnum, SensorStatusEnum cbAccelEnum,
            SensorStatusEnum ambientTempHumidityEnum)
        {
            SensorStatus status = new SensorStatus();

            status.gate = Convert.ToSByte(gateEnum);
            status.az_motor_temp_1 = Convert.ToSByte(azTemp1Enum);
            status.az_motor_temp_2 = Convert.ToSByte(azTemp2Enum);
            status.el_motor_temp_1 = Convert.ToSByte(elTemp1Enum);
            status.el_motor_temp_2 = Convert.ToSByte(elTemp2Enum);
            status.weather_station = Convert.ToSByte(weatherEnum);
            status.elevation_abs_encoder = Convert.ToSByte(elAbsEncoderEnum);
            status.azimuth_abs_encoder = Convert.ToSByte(azAbsEncoderEnum);
            status.el_proximity_0 = Convert.ToSByte(elProximity0Enum);
            status.el_proximity_90 = Convert.ToSByte(elProximity90Enum);
            status.az_accel = Convert.ToSByte(azAccelEnum);
            status.el_accel = Convert.ToSByte(elAccelEnum);
            status.counter_balance_accel = Convert.ToSByte(cbAccelEnum);
            status.ambient_temp_humidity = Convert.ToSByte(ambientTempHumidityEnum);

            return status;
        }
    }
}
