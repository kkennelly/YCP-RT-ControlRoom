﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// This is the structure of the appointment calibration,
    /// this will be useful for saving appointment calibration
    /// data and linking said data to an appointment already in the DB
    /// </summary>
    [Table("appointment_calibration")]
    public class AppointmentCalibration
    {
        public AppointmentCalibration()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int appointment_id { get; set; }
        [ForeignKey("appointment_id")]
        public virtual Appointment Appointment { get; set; }

        [Required]
        [Column("calibration_type")]
        public AppointmentCalibrationTypeEnum CalibrationType { get; set; }

        [Required]
        [Column("zenith_start_time")]
        public DateTime zenith_start_time { get; set; }

        [Required]
        [Column("zenith_end_time")]
        public DateTime zenith_end_time { get; set; }

        [Required]
        [Column("tree_start_time")]
        public DateTime tree_start_time { get; set; }

        [Required]
        [Column("tree_end_time")]
        public DateTime tree_end_time { get; set; }

        public static AppointmentCalibration Generate(AppointmentCalibrationTypeEnum calibrationType, DateTime zenithStart, DateTime zenithEnd, DateTime treeStart, DateTime treeEnd)
        {
            AppointmentCalibration apptCal = new AppointmentCalibration();

            //apptCal.appointment_id = appointmentId;
            apptCal.CalibrationType = calibrationType;
            apptCal.zenith_start_time = zenithStart;
            apptCal.zenith_end_time = zenithEnd;
            apptCal.tree_start_time = treeStart;
            apptCal.tree_end_time = treeEnd;

            return apptCal;
        }
    }
}