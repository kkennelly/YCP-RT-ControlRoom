using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers.SensorNetwork;

namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// This is the configuration settings that the sensor network accelerometers use.
    /// These are apart of the sensor network configuration settings and are used to denote
    /// how we want the different accelerometers customized.
    /// </summary>
    [Table("accelerometer_config")]
    public class AccelerometerConfig : IEquatable<AccelerometerConfig>
    {
        /// <summary>
        /// Constructor to initialize a new AccelerometerConfig for a SensorNetworkConfig with default values.
        /// </summary>
        /// <param name="snConfigId">The SensorNetworkConfig.Id that this configuration corresponds to.</param>
        /// <param name="locationId">The SensorLocationEnum location that this configuration is made for.</param>
        public AccelerometerConfig(int snConfigId, int locationId)
        {
            SensorNetworkConfigId = snConfigId;
            LocationId = locationId;

            // Set to default accelerometer settings
            SamplingFrequency = SensorNetworkConstants.DefaultAccelSamplingFrequency;
            GRange = SensorNetworkConstants.DefaultAccelGRange;
            FIFOSize = SensorNetworkConstants.DefaultAccelFIFOSize;
            XOffset = SensorNetworkConstants.DefaultAccelXOffset;
            YOffset = SensorNetworkConstants.DefaultAccelYOffset;
            ZOffset = SensorNetworkConstants.DefaultAccelZOffset;
            FullBitResolution = SensorNetworkConstants.DefaultAccelFullBitResolution;
        }

        /// <summary>
        /// This constructor takes no parameters and sets all values to equivalents of 0.
        /// It is only used for the Entity-MySql communication.
        /// </summary>
        public AccelerometerConfig()
        {
            SensorNetworkConfigId = 0;
            LocationId = 0;
            SamplingFrequency = 0;
            GRange = 0;
            FIFOSize = 0;
            XOffset = 0;
            YOffset = 0;
            ZOffset = 0;
            FullBitResolution = false;
        }

        /// <summary>
        /// Database-generated id value. This has a uniqueness constraint (meaning that the Id in one AccelerometerConfig
        /// cannot be equal to the Id of another).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The Id of the associated SensorNetworkConfig that this AccelerometerConfig references.
        /// </summary>
        [Required]
        [Column("sensor_network_config_id")]
        public int SensorNetworkConfigId { get; set; }

        /// <summary>
        /// The Id from the SensorLocationEnum that denotes which accelerometer this config is for.
        /// </summary>
        [Required]
        [Column("location")]
        public int LocationId { get; set; }

        /// <summary>
        /// The sampling frequency that accelerometer is set to. This decides how often the accelerometer reads a sample.
        /// </summary>
        [Required]
        [Column("sampling_frequency")]
        public double SamplingFrequency { get; set; }

        /// <summary>
        /// The G-range that the accelerometer data is mapped upon. Depending on the bit resolution settings,
        /// this how many G's the max and min raw acceleration values map to.
        /// </summary>
        [Required]
        [Column("g_range")]
        public int GRange { get; set; }

        /// <summary>
        /// The size of the FIFO queue on the accelerometer. The accelerometer's FIFO queue is where all samples are stored until
        /// it is full, and tells the ESS to unload it.
        /// </summary>
        [Required]
        [Column("fifo_size")]
        public int FIFOSize { get; set; }

        /// <summary>
        /// The x-axis offset applied to the accelerometer for calibration. For example if the x-axis is supposed to read 0, but is reading
        /// 4, set the offset to -4 to bring the reading to what it is supposed to be.
        /// </summary>
        [Required]
        [Column("x_offset")]
        public int XOffset { get; set; }

        /// <summary>
        /// The y-axis offset applied to the accelerometer for calibration. For example if the y-axis is supposed to read 0, but is reading
        /// 4, set the offset to -4 to bring the reading to what it is supposed to be.
        /// </summary>
        [Required]
        [Column("y_offset")]
        public int YOffset { get; set; }

        /// <summary>
        /// The z-axis offset applied to the accelerometer for calibration. For example if the z-axis is supposed to read 0, but is reading
        /// 4, set the offset to -4 to bring the reading to what it is supposed to be.
        /// </summary>
        [Required]
        [Column("z_offset")]
        public int ZOffset { get; set; }

        /// <summary>
        /// Whether or not the accelerometer is using full bit resolution. Full bit resolution indicates that regardless of G-range, 1 G will
        /// always map to 256, which means that 2G is 10 bits, 4G 11 bits, 8G 12 bits, and 16G 13 bits.
        /// Setting this to false puts the accelerometer in 10-bit resolution mode, meaning every G-range will have 10 bits of resolution.
        /// </summary>
        [Required]
        [Column("full_bit_resolution")]
        public bool FullBitResolution { get; set; }

        /// <summary>
        /// This will check if two AccelerometerConfigs are identical or not. This only compares the accelerometer settings, not database Ids.
        /// </summary>
        /// <param name="other">The other AccelerometerConfig to compare to.</param>
        /// <returns>True if the AccelerometerConfigs are equal, false if they are not.</returns>
        public bool Equals(AccelerometerConfig other)
        {
            return other != null &&
                SamplingFrequency == other.SamplingFrequency &&
                GRange == other.GRange &&
                FIFOSize == other.FIFOSize &&
                XOffset == other.XOffset &&
                YOffset == other.YOffset &&
                ZOffset == other.ZOffset &&
                FullBitResolution == other.FullBitResolution;
        }

        /// <summary>
        /// This will get this accelerometer config's settings as a byte[], so that it can be sent with the sensor initialization packet.
        /// </summary>
        /// <returns>The accelerometer config in bytes</returns>
        public byte[] GetAccelConfigAsBytes()
        {
            // Get sampling frequency bytes that correspond with the datasheet
            byte samplingFrequency;
            switch (SamplingFrequency)
            {
                case 800:
                    samplingFrequency = 0xD;
                    break;

                case 400:
                    samplingFrequency = 0xC;
                    break;

                case 200:
                    samplingFrequency = 0xB;
                    break;

                case 100:
                    samplingFrequency = 0xA;
                    break;

                case 50:
                    samplingFrequency = 0x9;
                    break;

                case 25:
                    samplingFrequency = 0x8;
                    break;

                default:
                    samplingFrequency = 0xD;    // Should never reach here, if so, make default 800Hz
                    break;
            }

            // Get the gRange bytes that correspond with the datasheet
            byte gRange;
            switch (GRange)
            {
                case 16:
                    gRange = 0x3;
                    break;

                case 8:
                    gRange = 0x2;
                    break;

                case 4:
                    gRange = 0x1;
                    break;

                case 2:
                    gRange = 0x0;
                    break;

                default:
                    gRange = 0x3;   // Should never reach here, if so, make default 16g
                    break;
            }

            return new byte[]
            {
                samplingFrequency,
                gRange,
                (byte)FIFOSize,
                (byte)XOffset,
                (byte)YOffset,
                (byte)ZOffset,
                BitConverter.GetBytes(FullBitResolution)[0]
            };
        }
    }
}
