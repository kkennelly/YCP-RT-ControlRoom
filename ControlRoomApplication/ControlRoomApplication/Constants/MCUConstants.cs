namespace ControlRoomApplication.Constants {
    public sealed class MCUConstants {
        public const double SIMULATION_MCU_PEAK_VELOCITY = 22.5; // steps/s
        public const double SIMULATION_MCU_PEAK_ACCELERATION = 32.0; // steps/s^2

        public const int ACTUAL_MCU_DEFAULT_PEAK_VELOCITY = 500_000; // steps/s
        public const int ACTUAL_MCU_DEFAULT_ACCELERATION = 1000; // steps/ms/s
        public const double ACTUAL_MCU_STEPS_PER_DEGREE = 166 + (2.0 / 3);

        public const string ACTUAL_MCU_IP_ADDRESS = "192.168.0.2";
        public const int ACTUAL_MCU_MODBUS_TCP_PORT = 502;
        public const ushort ACTUAL_MCU_READ_INPUT_REGISTER_START_ADDRESS = 0;
        public const ushort ACTUAL_MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS = 9;
        public const ushort ACTUAL_MCU_READ_INPUT_REGISTER_CURRENT_POSITION_ADDRESS = 2;
        public const ushort ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS = 1024;
        public const int ACTUAL_MCU_AZIMUTH_ENCODER_BIT_RESOLUTION = 12;
        public const int ACTUAL_MCU_MOVE_PEAK_VELOCITY_WITH_GEARING = 100_000;
        public const ushort ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING = 50;

        /// <summary>
        /// The target number of degrees to stop within
        /// </summary>
        public const double TARGET_STOP_DISTANCE = 5;   // Degrees


        /// <summary>
        /// this enum represents the data that comes out of the MCU starting in position <see cref="ACTUAL_MCU_READ_INPUT_REGISTER_START_ADDRESS"/> = 0  of its registers
        /// </summary>
        public enum MCUOutputRegs : ushort {
            /// <summary>
            /// most signifigant word (16 bits) of the az axsys status <see cref="MCUStatusBitsMSW"/> for description of eacs bit
            /// </summary>
            AZ_Status_Bist_MSW = 0,
            /// <summary>
            /// least signifigant word (16 bits) of the az axsys status <see cref="MCUStutusBitsLSW"/> for description of eacs bit
            /// </summary>
            AZ_Status_Bist_LSW = 1,
            /// <summary>
            /// this is the position of the axsys in terms of motor step count (most signifigant word)
            /// </summary>
            AZ_Current_Position_MSW = 2,
            /// <summary>
            /// this is the position of the axsys in terms of motor step count (least signifigant word)
            /// </summary>
            AZ_Current_Position_LSW = 3,
            /// <summary>
            /// if we were using encoders on the motors this is where the data from those encoders would be
            /// </summary>
            AZ_MTR_Encoder_Pos_MSW=4,
            /// <summary>
            /// if we were using encoders on the motors this is where the data from those encoders would be
            /// </summary>
            AZ_MTR_Encoder_Pos_LSW = 5,
            /// <summary>
            /// if the MCU is told to capture the current position this is where that data will be stored
            /// </summary>
            AZ_Capture_Data_MSW=6,
            /// <summary>
            /// if the MCU is told to capture the current position this is where that data will be stored
            /// </summary>
            AZ_Capture_Data_LSW = 7,
            RESERVED1 =8,
            /// <summary>
            /// used to track network conectivity bit 14 of this register will flip every .5 seconds,
            /// bit 13 is set when the MCU looses or has previously lost ethernet conectivity
            /// </summary>
            NetworkConnectivity = 9,
            /// <summary>
            /// most signifigant word (16 bits) of the EL axsys status <see cref="MCUStatusBitsMSW"/> for description of eacs bit
            /// </summary>
            EL_Status_Bist_MSW = 10,
            /// <summary>
            /// least signifigant word (16 bits) of the EL axsys status <see cref="MCUStutusBitsLSW"/> for description of eacs bit
            /// </summary>
            EL_Status_Bist_LSW = 11,
            /// <summary>
            /// this is the position of the axsys in terms of motor step count (most signifigant word)
            /// </summary>
            EL_Current_Position_MSW = 12,
            /// <summary>
            /// this is the position of the axsys in terms of motor step count (least signifigant word)
            /// </summary>
            EL_Current_Position_LSW = 13,
            /// <summary>
            /// if we were using encoders on the motors this is where the data from those encoders would be
            /// </summary>
            EL_MTR_Encoder_Pos_MSW = 14,
            /// <summary>
            /// if we were using encoders on the motors this is where the data from those encoders would be
            /// </summary>
            EL_MTR_Encoder_Pos_LSW = 15,
            /// <summary>
            /// if the MCU is told to capture the current position this is where that data will be stored
            /// </summary>
            EL_Capture_Data_MSW = 16,
            /// <summary>
            /// if the MCU is told to capture the current position this is where that data will be stored
            /// </summary>
            EL_Capture_Data_LSW = 17,
            RESERVED2 = 18,
            RESERVED3 = 19

        }



        /// <summary>
        /// desciptions taken from anf1-anf2-motion-controller-user-manual.pdf  page 76 - 78
        /// </summary>
        public enum MCUStatusBitsMSW : int {
            /// <summary>
            /// Set when the ANF1/2 axis is outputting pulses for clockwise motion
            /// </summary>
            CW_Motion = 0,
            /// <summary>
            /// Set when the ANF1/2 axis is outputting pulses for counter-clockwise motion
            /// </summary>
            CCW_Motion = 1,
            /// <summary>
            /// Set when the ANF1/2 axis has stopped motion as a result of a Hold Move Command
            /// </summary>
            Hold_State = 2,
            /// <summary>
            /// Set when the ANF1/2 axis is not in motion for any reason
            /// </summary>
            Axis_Stopped = 3,
            /// <summary>
            /// This bit is only set after the successful completion of a homing command
            /// </summary>
            At_Home = 4,
            /// <summary>
            /// Set when the ANF1/2 axis is accelerating during any move
            /// </summary>
            Move_Accelerating = 5,
            /// <summary>
            /// Set when the ANF1/2 axis is decelerating during any move
            /// </summary>
            Move_Decelerating = 6,
            /// <summary>
            /// Set when the ANF1/2 axis has successfully completed an Absolute, Relative,
            /// Blend, or Interpolated Move
            /// </summary>
            Move_Complete = 7,
            /// <summary>
            /// Set when the ANF1/2 could not home the axis because of an error durring homeing see MCU documaentation for list of potential eroorrs
            /// </summary>
            Home_Invalid_Error = 8,
            /// <summary>
            /// Set when there was an error in the last Program Blend Profile data block //we don't use blend move so this shouldnt come up
            /// </summary>
            Profile_Invalid = 9,
            /// <summary>
            /// this bit is set when the position stored in the MCU could be incorrect.
            /// set under the fowling conditions, Axis switched to Command Mode | An Immediate Stop command was issued | An Emergency Stop input was activated | CW or CCW Limit reached
            /// </summary>
            Position_Invalid = 10,
            /// <summary>
            /// see MCU documaentation for list of potential eroorrs
            /// </summary>
            Input_Error = 11,
            /// <summary>
            /// Set when the last command issued to the ANF1/2 axis forced an error
            /// </summary>
            Command_Error = 12,
            /// <summary>
            /// set when the axis has a configuration error
            /// </summary>
            Configuration_Error = 13,
            /// <summary>
            /// Set when the axis is enabled. Axis 2 of an ANF2 is disabled by default. An
            /// axis is automatically enabled when valid configuration data is written to it
            /// </summary>
            Axis_Enabled = 14,
            /// <summary>
            /// Set to “1” when the axis is in Configuration Mode. Reset to “0” when the axis is in Command Mode
            /// </summary>
            Axis_Configuration_Mode = 15,
        }

        /// <summary>
        /// desciptions taken from anf1-anf2-motion-controller-user-manual.pdf  page 76 - 78
        /// </summary>
        public enum MCUStutusBitsLSW : int {
            /// <summary>
            /// Set when the Capture Input is active
            /// </summary>
            Capture_Input_Active = 0,
            /// <summary>
            /// Set when the External Input is active
            /// </summary>
            External_Input = 1,
            /// <summary>
            /// Set when the Home Input is active
            /// </summary>
            Home_Input = 2,
            /// <summary>
            /// Set when the CW Input/E-Stop Input is active
            /// </summary>
            CW_Limit = 3,
            /// <summary>
            /// Set when the CCW Input/E-Stop Input is active
            /// </summary>
            CCW_Limit = 4,
            /// <summary>
            /// see MCU documaentation
            /// </summary>
            No_Fault_Output_State = 5,
            /// <summary>
            /// see MCU documaentation (unused)
            /// </summary>
            On_Line_Output_State = 6,
            /// <summary>
            /// see MCU documaentation (unused)
            /// </summary>
            Move_Complete_Output_State = 7,
            /// <summary>
            /// Set when the axis is presently running a Blend Move Profile (unused)
            /// </summary>
            Running_Blend_Move = 8,
            /// <summary>
            /// Set when ANF1/2 has accepted a Blend Move Profile programming block (unused)
            /// </summary>
            Blend_Move_Acknowledge = 9,
            /// <summary>
            /// Set when the Minimum Registration Move Distance
            /// parameter is programmed to a non-zero value (unused)
            /// </summary>
            NonZero_Registration_Distance = 10,
            /// <summary>
            ///  Set when the ANF1/2 is running an Interpolated Move. only valid for axis one
            /// </summary>
            Running_Interpolated_Move = 11,
            /// <summary>
            /// This bit always equals the state of the Backplane Home
            /// Proximity bit, which is bit 6 in the Command Bits LSW for the axis
            /// </summary>
            Backplane_Home_Proximity = 12,
            /// <summary>
            /// Set when the axis is in Encoder Follower Mode
            /// </summary>
            Encoder_Follower_Mode = 13,
            /// <summary>
            /// These bits will always equal zero
            /// </summary>
            Reserved1 = 14,
            /// <summary>
            /// These bits will always equal zero
            /// </summary>
            reserved2 = 15,
        }
    }
}

