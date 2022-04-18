using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("rf_data")]
    public class RFData
    {
        public RFData()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int appointment_id { get; set; }
        [ForeignKey("appointment_id")]
        public virtual Appointment Appointment { get; set; }

        [Required]
        [Column("time_captured")]
        public DateTime time_captured { get; set; }

        [Required]
        [Column("intensity")]
        public double Intensity { get; set; }

        //public Orientation AcquisitionOrientation { get; set; }

        public static RFData GenerateFrom(SpectraCyberResponse response)
        {
            RFData rfData = new RFData();
            rfData.time_captured = response.DateTimeCaptured;
            rfData.Intensity = response.DecimalData;
            return rfData;
        }

        public static List<RFData> GenerateListFrom(List<SpectraCyberResponse> responses)
        {
            List<RFData> rfDataList = new List<RFData>();
            foreach (SpectraCyberResponse response in responses)
            {
                rfDataList.Add(GenerateFrom(response));
            }

            return rfDataList;
        }
    }
}
