using System;

namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// the cctrlroom has a modbus server running on it that is maped to input and output regs on the plc 
    /// 0-20 arr comand bytes that go through the plc to the mcu
    /// warnings, limits and other boolean values are sent as ints with 0 for false and 1 for true
    /// </summary>
    /// <remarks>
    /// the registers in the PLC start at 0 however the registers in the controll room start at one 
    /// so an offset between the register number in the PLC and the number in the controll room is required
    /// </remarks>
    public enum PLC_modbus_server_register_mapping : ushort
    {
        /// <summary>
        /// <item><description><para><em>OUT</em></para></description></item>
        /// the PLC will not honor any requests to override the limits,
        /// this is only a formality and testing
        /// </summary>
        LIMIT_OVERRIDE = 0+1,
        /// <summary>
        /// <item><description><para><em>OUT</em></para></description></item>
        /// when this is set the elescope will still move even with the gate open
        /// </summary>
        GATE_OVERRIDE = 1+1,
        /// <summary>
        /// <item><description><para><em>OUT</em></para></description></item>
        /// the PLC will never honor this request under any circumstances,
        /// this mainly serves as a place holder
        /// </summary>
        E_STOP_OVERRIDE = 2+1,
        /// <summary>
        /// <item><description><para><em>OUT</em></para></description></item>
        /// this word (ushort) is incramented every 1 second by the PLCDriver and is used by the PLC to determine if the control room is active
        /// </summary>
        CTRL_HEART_BEAT = 3+1,
        /// <summary>
        ///  <item><description><para><em>OUT</em></para></description></item>
        ///  when this word is changed the PLC will shutDown the MCU
        /// </summary>
        MCU_RESET = 4+1,


        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit at -10
        /// </summary>
        AZ_0_LIMIT = 8 + 1, /// Azimuth proximity sensor 1
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit at 375 degrees
        /// </summary>
        AZ_375_LIMIT = 9 + 1, /// Azimuth proximity sensor 2
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// home sensor at 0 degrees,
        /// will be active between -15 and 0
        /// </summary>
        AZ_0_HOME = 10 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// SECONDARY home sensor at 0 degrees,
        /// will be active between -1 and 15
        /// </summary>
        AZ_0_SECONDARY = 11 + 1,


        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit switch at -10
        /// </summary>
        /// <remarks>
        /// im not 100% certian that the telesope will actually have this switch
        /// </remarks>
        EL_10_LIMIT = 12 + 1, /// Elevation proximity sensor 1?
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// active between 0 and -15 degrees
        /// </summary>
        EL_0_HOME = 13 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit at 90 degrees
        /// </summary>
        EL_90_LIMIT = 14 + 1, /// Elevation proximity sensor 2?



        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// emergency stop the plc will handle disabling the motors and MCU, the controll should re-configure (and posibly re-home) the MCU after this is ended
        /// </summary>
        E_STOP = 15 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when gate is open
        /// </summary>
        Gate_Safety_INTERLOCK = 16 + 1,

        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the MCU has a fault on the AZ axsis
        /// </summary>
        AZ_MCU_FAULT = 17+1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the MCU has a fault on the AZ axsis
        /// </summary>
        EL_MCU_FAULT = 18+1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the azimuth motor has a fault
        /// </summary>
        AZ_MTR_DRIVER_FAULT = 19 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the ELEVATION motor has a fault
        /// </summary>
        EL_MTR_DRIVER_FAULT = 20 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// the elevation will have a second sensor connected directly to the Elevation Frame
        /// </summary>
        EL_SLIP_CAPTURE = 21+1,


        /// <summary>
        /// not currerntly used by the PLC
        /// </summary>
        AZ_MOTOR_CURRENT = 40 + 1,
        /// <summary>
        /// not currerntly used by the PLC
        /// </summary>
        EL_MOTOR_CURRENT = 42 + 1
    }


}
