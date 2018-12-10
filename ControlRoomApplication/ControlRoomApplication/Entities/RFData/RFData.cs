﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("RFData")]
    public class RFData
    {
        public RFData()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("time_captured")]
        public DateTime TimeCaptured { get; set; }

        [Required]
        [Column("intensity")]
        public long Intensity { get; set; }

        public virtual Orientation AcquisitionOrientation { get; set; }

        public static RFData GenerateFrom(SpectraCyberResponse response)
        {
            RFData rfData = new RFData();
            rfData.TimeCaptured = response.DateTimeCaptured;
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
