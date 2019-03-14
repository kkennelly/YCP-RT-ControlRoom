﻿using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Main
{
    public class ConfigurationManager
    {
        public ConfigurationManager()
        {

        }

        /// <summary>
        /// Configures the type of PLC to be used in the rest of the application based on
        /// the first program argument passed in.
        /// </summary>
        /// <param name="arg"> The first program argument passed in. </param>
        /// <returns> A concrete instance of a PLC. </returns>
        public AbstractPLC ConfigurePLC(string arg0)
        {
            switch(arg0.ToUpper())
            {
                case "/SP":
                    return new ScaleModelPLC();
               
                // Will be used later on and should be uncommented then
                //case "/PP":
                    //return new ProductionPLC();

                case "/TP":
                    return new TestPLC();

                case "/VP":
                    return new VRPLC();

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the PLC, assume we are using the simulated/testing one.
                    return new TestPLC();
            }
        }

        /// <summary>
        /// Configures the type of SpectraCyberController to be used in the rest of the 
        /// application based on the second program argument passed in.
        /// </summary>
        /// <param name="arg1"> The second program argument passed in. </param>
        /// <param name="dbContext"> The database context </param>
        /// <returns> A concrete instance of a SpectraCyberController. </returns>
        public AbstractSpectraCyberController ConfigureSpectraCyberController(string arg1)
        {
            switch(arg1.ToUpper())
            {
                case "/PS":
                    return new SpectraCyberController(new SpectraCyber());

                case "/SS":
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());

                case "/TS":
                    return new SpectraCyberTestController(new SpectraCyberSimulator());

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the spectraCyber, assume we are using the simulated/testing one.
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            }
        }

        /// <summary>
        /// Configures the type of radiotelescope that will be used in the rest
        /// of the application based on the third command line argument passed in.
        /// </summary>
        /// <param name="arg2"> The third program argument passed in. </param>
        /// <returns> A concrete instance of a radio telescope. </returns>
        public AbstractRadioTelescope ConfigureRadioTelescope(string arg2, AbstractSpectraCyberController spectraCyberController, AbstractPLC plc)
        {
            // Create PLCController
            PLCController plcController = new PLCController(plc);

            // Create Radio Telescope Location
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now

            switch (arg2.ToUpper())
            {
                case "/SR":
                    return new ScaleRadioTelescope(spectraCyberController, plcController, location);

                case "/PR":
                    return new ProductionRadioTelescope(spectraCyberController, plcController, location);

                case "/TR":
                    // Case for the test/simulated radiotelescope.
                    // Will need to be uncommented later.
                    return new TestRadioTelescope(spectraCyberController, plcController, location);
                default:
                    // Should be changed once we have a simulated
                    // radiotelescope class implemented
                    return new ScaleRadioTelescope(spectraCyberController, plcController, location);
            }
        }

        public void ConfigureLocalDatabase()
        {
            DatabaseOperations.InitializeLocalConnectionOnly();
            DatabaseOperations.PopulateLocalDatabase();
        }
    }
}
