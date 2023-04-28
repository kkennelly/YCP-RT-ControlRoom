using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ControlRoomApplication.Constants;
using System.Net.Sockets;

namespace ControlRoomApplication.Validation {
    
    public class Validator
    {
     
        /// <summary>
        /// Used to determine if a port number is in the acceptable range. Helpful for user validation
        /// </summary>
        /// <param name="port"> port number, as an integer </param>
        /// <returns> true if the port is valid (within acceptable range), otherwise false</returns>
        public static bool ValidatePort(int port)
        {
            return (port <= MiscellaneousConstants.MAX_PORT_VALUE 
                && port >= MiscellaneousConstants.MIN_PORT_VALUE);

            
        }

        /// <summary>
        /// Alternate method to handle the case where the input is coming from a text box
        /// </summary>
        /// <param name="text"> text from the textbox to parse</param>
        /// <returns> true if the string is a valid port number, false otherwise </returns>
        public static bool ValidatePort(string text)
        {
            int port;
            int.TryParse(text, out port);
            // luckily, since port 0 is reserved, we don't need to check if
            // port would be 0 from the parse failing. It handles both cases!
            return ValidatePort(port);
        }


       /// <summary>
       /// Validating that a string input by user into IP field is a valid IP Address
       /// </summary>
       /// <param name="ip"> string to be converted </param>
       /// <returns> true if the IP is valid, false if the string throws an exception
       /// meaning it was invalid.</returns>
        public static bool ValidateIPAddress(string ip)
        {
            IPAddress address;
            try {
               address = IPAddress.Parse(ip);
            }catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
            
        }

      
        /// <summary>
        ///  Validating azSpeed from user input in RTControlForm
        /// </summary>
        /// <param name="speed"> azSpeed value, given by user in RPMs </param>
        /// <returns> true if the value is between the accepted range, false otherwise</returns>
        public static bool ValidateSpeed(double speed)
        {
            return (speed <= MiscellaneousConstants.MAX_SPEED_RPM 
                && speed >= MiscellaneousConstants.MIN_SPEED_RPM);
        }

        /// <summary>
        /// Helper method to validate via a text box
        /// </summary>
        /// <param name="text"> user text to validate </param>
        /// <returns> returns true, if the textbox azSpeed value is a valid 
        /// double AND is in the expected range. False otherwise</returns>
        public static bool ValidateSpeed(string text)
        {
            double speed;
            try
            {
                speed = Double.Parse(text);
            }
            catch(FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return ValidateSpeed(speed);
        }

        /// <summary>
        /// used to check ONLY if the text is a valid double
        /// Does NOT account for the azSpeed being in the expected range (use ValidateSpeed(string text) for that)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ValidateSpeedTextOnly(string text)
        {
            double speed;
            try
            {
                speed = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validating the voltage that the user entered is withing the acceptable range
        /// </summary>
        /// <param name="volts"> Valueinput by user, in Volts </param>
        /// <returns>true, if in the expected range, false otherwise</returns>
        public static bool ValidateOffsetVoltage(double volts)
        {
            return (volts >= 0 && volts <= 4.095);
        }

        /// <summary>
        /// Helper method to validate offset voltage via a string (Windows Form textbox will return as a string)
        /// Also used to validate the voltage that the user entered is withing the acceptable range
        /// </summary>
        /// <param name="text"> the textnox from the windows form, 
        /// or any string of text that contains a voltage value</param>
        /// <returns> true, if the voltage is a valid double AND is in the expected range. False otherwise. </returns>
        public static bool ValidateOffsetVoltage(string text)
        {
            double volts;
            try
            {
                volts = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return ValidateOffsetVoltage(volts);
        }

        /// <summary>
        /// Method to validate the IFGain user input in the RTControl Form. 
        /// Useful when input is a textbox, i.e. from a Windows Form.
        /// </summary>
        /// <param name="text">The textbox, or any string of text to be validated for IFGain </param>
        /// <returns>true if the value is a valid double AND the IFGain is in the expected range </returns>
        public static bool ValidateIFGain(string text)
        {
            double decibles;
            try
            {
                decibles = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return ValidateIFGain(decibles);
        }

        /// <summary>
        /// Method to validate decibles are in the expected range
        /// given a valid double. Mainly used for IFGain user input in the RTControl Form
        /// </summary>
        /// <param name="decibles"> Decibles entered by the user </param>
        /// <returns> True, if the decibles are in the expected range. False otherwise </returns>
        public static bool ValidateIFGain(double decibles)
        {
            return (decibles >= 10.00 && decibles <= 25.75);
        }

        /// <summary>
        /// Method to validate frequency from a string of text. Will attempt to parse
        /// the double out of the string, then validate the double is within the expected frequency range.
        /// </summary>
        /// <param name="text"> String of text entered by user, or any string of text to be
        /// validated for frequency </param>
        /// <returns>If unable to parse, or the double parsed is not in the expected
        /// range, it will return false. True otherwise</returns>
        public static bool ValidateFrequency(string text)
        {
            double hertz;
            try
            {
                hertz = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return ValidateFrequency(hertz);
        }

        /// <summary>
        /// Method to validate frequency entered by user, or any value of frequency
        /// </summary>
        /// <param name="hertz"> frequency, in hertz, as a double value </param>
        /// <returns> True, if the frequency is within the accepted range. False otherwise </returns>
        public static bool ValidateFrequency(double hertz)
        {
            return (hertz >= 0.0);
        }

        /// <summary>
        /// This is used to determine if a string value can be parsed into a double.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>True if it is a valid double, false otherwise.</returns>
        public static bool IsDouble(string text)
        {
            try
            {
                double.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This is used to validate if a value is in between two values. Null can be passed through
        /// the lower or upper bounds if a lower or upper bound is not necessary.
        /// </summary>
        /// <param name="value">The value being assessed.</param>
        /// <param name="lower">The valid lower bound of the value.</param>
        /// <param name="upper">The valid upper bound of the value.</param>
        /// <returns></returns>
        public static bool IsBetween(double value, double? lower, double? upper)
        {
            if (lower != null && value < lower) return false;

            if (upper != null && value > upper) return false;

            return true;
        }

        /// <summary>
        /// This verifies that a server is currently running on an IP address and port combo.
        /// </summary>
        /// <param name="ip">IP address that we are verifying.</param>
        /// <param name="port">Port that we are verifying.</param>
        /// <returns></returns>
        public static bool ServerRunningOnIp(string ip, int port)
        {
            bool success = false;
            TcpClient c = new TcpClient();
            try
            {
                c.Connect(ip, port);
                success = true;
            }
            catch {
                success = false;
            }

            c.Close();
            c.Dispose();

            return success;
        }
    }

}

