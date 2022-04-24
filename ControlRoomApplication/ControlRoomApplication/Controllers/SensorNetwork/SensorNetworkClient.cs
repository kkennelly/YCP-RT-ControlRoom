using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// This is the class used to send sensor initialization information to the Sensor Network.
    /// </summary>
    public class SensorNetworkClient
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor used to initialize a SensorNetworkClient, which will allow the SensorNetworkServer
        /// to use this to send data to the Sensor Network.
        /// </summary>
        /// <param name="clientIPAddress">IP address that the client will connect to.</param>
        /// <param name="clientPort">Port that the client will connect to.</param>
        /// <param name="telescopeId">The ID of the Radio Telescope so we know what config to grab.</param>
        public SensorNetworkClient(string clientIPAddress, int clientPort, int telescopeId)
        {
            IPAddress = clientIPAddress;
            Port = clientPort;
            SensorNetworkConfig = DatabaseOperations.RetrieveSensorNetworkConfigByTelescopeId(telescopeId);

            // if the config doesn't exist, create a new one
            if(SensorNetworkConfig == null)
            {
                SensorNetworkConfig = new SensorNetworkConfig(telescopeId);
                DatabaseOperations.AddSensorNetworkConfig(SensorNetworkConfig);

                SensorNetworkConfig.ElAccelConfig = new AccelerometerConfig(SensorNetworkConfig.Id, (int)SensorLocationEnum.EL_MOTOR);
                SensorNetworkConfig.AzAccelConfig = new AccelerometerConfig(SensorNetworkConfig.Id, (int)SensorLocationEnum.AZ_MOTOR);
                SensorNetworkConfig.CbAccelConfig = new AccelerometerConfig(SensorNetworkConfig.Id, (int)SensorLocationEnum.COUNTERBALANCE);

                DatabaseOperations.AddAccelerometerConfig(SensorNetworkConfig.ElAccelConfig);
                DatabaseOperations.AddAccelerometerConfig(SensorNetworkConfig.AzAccelConfig);
                DatabaseOperations.AddAccelerometerConfig(SensorNetworkConfig.CbAccelConfig);
            }
        }

        private TcpClient InitializationClient;
        private readonly string IPAddress;
        private readonly int Port;

        /// <summary>
        /// This is the configuration we are using to store any initialization data to send, as well
        /// as timeout information.
        /// </summary>
        public SensorNetworkConfig SensorNetworkConfig;

        /// <summary>
        /// Converts the initialization from the config file to bytes and then sends it to the Sensor Network.
        /// </summary>
        public bool SendSensorInitialization()
        {
            bool success = false;
            var init = SensorNetworkConfig.GetSensorInitAsBytes();

            try
            {
                // Set up TCP client
                InitializationClient = new TcpClient(IPAddress, Port);
                NetworkStream stream = InitializationClient.GetStream();

                // Send initialization
                stream.Write(init, 0, init.Length);

                stream.Flush();
                stream.Close();
                stream.Dispose();
                InitializationClient.Close();
                InitializationClient.Dispose();

                success = true; // Successfully sent the message without any errors
            }

            // Reaching this exception will set the SensorNetworkServer's status to InitializationSendingFailed
            catch (SocketException)
            {
                logger.Info(Utilities.GetTimeStamp() + $": There was an error sending data to the Sensor Network at {IPAddress}:{Port}; " +
                    $"the address:port may be busy or no server was found. Please verify the address:port is available and restart the " +
                    $"Control Room software.");
            }

            return success;
        }
    }
}
