using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("spectracyber_config")]
    public class SpectraCyberConfig
    {
        public SpectraCyberConfig(SpectraCyberModeTypeEnum mode, SpectraCyberIntegrationTimeEnum integration_time, 
                                  double offset_voltage, double if_gain, SpectraCyberDCGainEnum dc_gain, SpectraCyberBandwidthEnum bandwidth)
        {
            _Mode = mode;
            IntegrationTime = integration_time;
            OffsetVoltage = offset_voltage;
            IFGain = if_gain;
            DCGain = dc_gain;
            Bandwidth = bandwidth;
        }

        public SpectraCyberConfig(SpectraCyberModeTypeEnum mode)
        {
            _Mode = mode;
            IntegrationTime = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
            OffsetVoltage = 0;
            IFGain = 10;
            DCGain = SpectraCyberDCGainEnum.X1;
            Bandwidth = SpectraCyberBandwidthEnum.SMALL_BANDWIDTH;
        }

        public SpectraCyberConfig() : this(SpectraCyberModeTypeEnum.SPECTRAL) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public SpectraCyberModeTypeEnum _Mode
        {
            get
            {
                return (SpectraCyberModeTypeEnum)Enum.Parse(typeof(SpectraCyberModeTypeEnum), mode);
            }
            set
            {
                this.mode = value.ToString();
            }
        }

        private string backingMode { get; set; }

        [Required]
        [Column("mode")]
        public string mode
        {
            get
            {
                return this.backingMode;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(SpectraCyberModeTypeEnum), value))
                {
                    this.backingMode = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        [NotMapped]
        public SpectraCyberIntegrationTimeEnum IntegrationTime { get; set; }

        [Required]
        [Column("integration_time")]
        public double time
        {
            get
            {
                return SpectraCyberIntegrationTimeEnumHelper.GetDoubleValue(IntegrationTime);
            }
            set
            {
                IntegrationTime = SpectraCyberIntegrationTimeEnumHelper.GetEnumFromValue(value);
            }
        }

        [Required]
        [Column("offset_voltage")]
        public double OffsetVoltage { get; set; }

        [Required]
        [Column("if_gain")]
        public double IFGain { get; set; }

        [NotMapped]
        public SpectraCyberDCGainEnum DCGain { get; set; }

        [Required]
        [Column("dc_gain")]
        public int dc_gain
        {
            get
            {
                return SpectraCyberDCGainEnumHelper.GetIntegerValue(DCGain);
            }
            set
            {
                DCGain = SpectraCyberDCGainEnumHelper.GetEnumFromValue(value);
            }
        }

        [NotMapped]
        public SpectraCyberBandwidthEnum Bandwidth { get; set; }

   //     [Required]
   //     [Column("bandwidth")]
        [NotMapped]
        public int bandwidth
        {
            get
            {
                return SpectraCyberBandwidthEnumHelper.GetIntegerValue(Bandwidth);
            }
            set
            {
                Bandwidth = SpectraCyberBandwidthEnumHelper.GetEnumFromValue(value);
            }
        }



        /// <summary>
        /// Checks if the current SpectraCyberConfig is Equal to another SpectraCyberConfig  
        /// and it checks if the other SpectraCyberConfig is null
        /// </summary>
        public override bool Equals(object obj)
        {
            SpectraCyberConfig other = obj as SpectraCyberConfig; //avoid double casting
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return  _Mode == other._Mode && 
                    IntegrationTime == other.IntegrationTime && 
                    OffsetVoltage == other.OffsetVoltage && 
                    IFGain == other.IFGain && 
                    DCGain == other.DCGain && 
                    Bandwidth == other.Bandwidth;
        }

        /// <summary>
        /// Returns the HashCode of the Orientation's Id
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id + "| IT: " + IntegrationTime + " OV: " + OffsetVoltage + " IFG: " + IFGain + " DCG: " + DCGain + "B: " + bandwidth; 
        }
    }
}
