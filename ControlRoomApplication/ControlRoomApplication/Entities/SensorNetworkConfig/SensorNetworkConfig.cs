using ControlRoomApplication.Controllers.SensorNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities
{

    /// <summary>
    /// This is the conviguration that the SensorNetworkServer and Client use.
    /// This contains values that we want to be retained when the control
    /// room is closed and then reopened, such as initialization information.
    /// </summary>
    [Table("sensor_network_config")]
    public class SensorNetworkConfig : IEquatable<SensorNetworkConfig>
    {
        /// <summary>
        /// Constructor to initialize a new SensorNetworkConfig for a RadioTelescope with default values.
        /// </summary>
        /// <param name="telescopeId">The RadioTelescope.Id that this configuration corresponds to.</param>
        public SensorNetworkConfig(int telescopeId)
        {
            TelescopeId = telescopeId;
            TimeoutDataRetrieval = SensorNetworkConstants.DefaultDataRetrievalTimeout;
            TimeoutInitialization = SensorNetworkConstants.DefaultInitializationTimeout;

            // Initialize all sensors to enabled by default
            ElevationTemp1Init = true;
            AzimuthTemp1Init = true;
            ElevationAmbientInit = true;
            AzimuthAccelerometerInit = true;
            ElevationAccelerometerInit = true;
            CounterbalanceAccelerometerInit = true;
            AzimuthEncoderInit = true;
            ElevationEncoderInit = true;

            TimerPeriod = SensorNetworkConstants.DefaultTimerInterruptInterval;
            EthernetPeriod = SensorNetworkConstants.DefaultDataSendingInterval;
            TemperaturePeriod = SensorNetworkConstants.DefaultTemperaturereadingInterval;
            EncoderPeriod = SensorNetworkConstants.DefaultEncoderSendingInterval;

            ElAccelConfig = new AccelerometerConfig(Id, (int)SensorLocationEnum.EL_MOTOR);
            AzAccelConfig = new AccelerometerConfig(Id, (int)SensorLocationEnum.AZ_MOTOR);
            CbAccelConfig = new AccelerometerConfig(Id, (int)SensorLocationEnum.COUNTERBALANCE);
        }

        /// <summary>
        /// This constructor takes no parameters and sets all values to equivalents of 0.
        /// It is only used for the Entity-MySql communication.
        /// </summary>
        public SensorNetworkConfig()
        {
            TelescopeId = 0;
            TimeoutDataRetrieval = 0;
            TimeoutInitialization = 0;

            // Initialize all sensors to be disabled
            ElevationTemp1Init = false;
            AzimuthTemp1Init = false;
            ElevationAmbientInit = false;
            AzimuthAccelerometerInit = false;
            ElevationAccelerometerInit = false;
            CounterbalanceAccelerometerInit = false;
            AzimuthEncoderInit = false;
            ElevationEncoderInit = false;

            TimerPeriod = 0;
            EthernetPeriod = 0;
            TemperaturePeriod = 0;
            EncoderPeriod = 0;

            ElAccelConfig = new AccelerometerConfig();
            AzAccelConfig = new AccelerometerConfig();
            CbAccelConfig = new AccelerometerConfig();
        }

        /// <summary>
        /// Database-generated id value. This has a uniqueness constraint (meaning that the Id in one SensorNetworkConfig
        /// cannot be equal to the Id of another).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The Radio Telescope ID that this configuration is for. This should not be
        /// a foreign key, because a SensorNetworkConfig does not have a whole RadioTelescope object.
        /// </summary>
        [Column("telescope_id")]
        public int TelescopeId { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the primary elevation motor temp sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_temp_1_init")]
        public bool ElevationTemp1Init { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the primary azimuth motor temp sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_temp_1_init")]
        public bool AzimuthTemp1Init { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the DHT22 ambient temp and humidity sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_ambient_init")]
        public bool ElevationAmbientInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the azimuth accelerometer.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_accelerometer_init")]
        public bool AzimuthAccelerometerInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the elevation accelerometer.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_accelerometer_init")]
        public bool ElevationAccelerometerInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the counterbalance accelerometer.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("counterbalance_accelerometer_init")]
        public bool CounterbalanceAccelerometerInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the azimuth encoder.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_encoder_init")]
        public bool AzimuthEncoderInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the elevation encoder.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_encoder_init")]
        public bool ElevationEncoderInit { get; set; }

        /// <summary>
        /// The timeout interval, in seconds, that the telescope will go into a state of <seealso cref="SensorNetworkStatusEnum.TimedOutDataRetrieval"/>.
        /// </summary>
        [Column("timeout_data_retrieval")]
        public int TimeoutDataRetrieval { get; set; }

        /// <summary>
        /// The timeout interval, in seconds, that the telescope will go into a state of <seealso cref="SensorNetworkStatusEnum.TimedOutInitialization"/>.
        /// </summary>
        [Column("timeout_initialization")]
        public int TimeoutInitialization { get; set; }

        /// <summary>
        /// How often the ms timer will go off on the ESS.
        /// </summary>
        public int TimerPeriod { get; set; }

        /// <summary>
        /// How often the ESS will send the main packet to us.
        /// </summary>
        public int EthernetPeriod { get; set; }

        /// <summary>
        /// How often temperature data will be collected.
        /// </summary>
        public int TemperaturePeriod { get; set; }

        /// <summary>
        /// How often the absolute encoders will be read and data sent to us.
        /// </summary>
        public int EncoderPeriod { get; set; }

        [NotMapped]
        public AccelerometerConfig ElAccelConfig { get; set; }

        [NotMapped]
        public AccelerometerConfig AzAccelConfig { get; set; }

        [NotMapped]
        public AccelerometerConfig CbAccelConfig { get; set; }

        /// <summary>
        /// This will check if two SensorNetworkConfigs are identical or not
        /// </summary>
        /// <param name="other">The SensorNetworkConfig to compare against</param>
        /// <returns></returns>
        public bool Equals(SensorNetworkConfig other)
        {
            // First do null checking
            if (this == null && other == null) return true;
            else if (this == null) return false;
            else if (other == null) return false;

            else if (
                this.TelescopeId == other.TelescopeId &&
                this.ElevationTemp1Init == other.ElevationTemp1Init &&
                this.AzimuthTemp1Init == other.AzimuthTemp1Init &&
                this.ElevationAmbientInit == other.ElevationAmbientInit &&
                this.AzimuthAccelerometerInit == other.AzimuthAccelerometerInit &&
                this.ElevationAccelerometerInit == other.ElevationAccelerometerInit &&
                this.CounterbalanceAccelerometerInit == other.CounterbalanceAccelerometerInit &&
                this.AzimuthEncoderInit == other.AzimuthEncoderInit &&
                this.ElevationEncoderInit == other.ElevationEncoderInit &&
                this.TimeoutDataRetrieval == other.TimeoutDataRetrieval &&
                this.TimeoutInitialization == other.TimeoutInitialization &&
                ElAccelConfig.Equals(other.ElAccelConfig) &&
                AzAccelConfig.Equals(other.AzAccelConfig) &&
                CbAccelConfig.Equals(other.CbAccelConfig)
                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// This will be used to convert the current sensor initialization to a byte array that we
        /// can send to the Sensor Network. It will convert each init to its own byte that will 
        /// be either 1 or 0.
        /// 0 = not initialized;
        /// 1 = initialized
        /// </summary>
        public byte[] GetSensorInitAsBytes()
        {
            byte[] init = new byte[] {
                ElevationTemp1Init ?                (byte)1 : (byte)0,
                AzimuthTemp1Init ?                  (byte)1 : (byte)0,
                ElevationEncoderInit ?              (byte)1 : (byte)0,
                AzimuthEncoderInit ?                (byte)1 : (byte)0,
                AzimuthAccelerometerInit ?          (byte)1 : (byte)0,
                ElevationAccelerometerInit ?        (byte)1 : (byte)0,
                CounterbalanceAccelerometerInit ?   (byte)1 : (byte)0,
                ElevationAmbientInit ?              (byte)1 : (byte)0
            };

            return init;
        }
    }
    

}
