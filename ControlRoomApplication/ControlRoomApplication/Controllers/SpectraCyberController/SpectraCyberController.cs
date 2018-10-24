﻿using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using System;
using System.IO.Ports;
using System.Threading;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public class SpectraCyberController : AbstractSpectraCyberController
    {
        public SpectraCyberController(SpectraCyber spectraCyber) : base(spectraCyber) { }

        public override bool BringUp()
        {
            try
            {
                ((SpectraCyber)SpectraCyber).SerialPort = new SerialPort(
                ((SpectraCyber)SpectraCyber).CommPort,
                AbstractSpectraCyberConstants.SPECTRA_CYBER_BAUD_RATE,
                AbstractSpectraCyberConstants.SPECTRA_CYBER_PARITY_BITS,
                AbstractSpectraCyberConstants.SPECTRA_CYBER_DATA_BITS,
                AbstractSpectraCyberConstants.SPECTRA_CYBER_STOP_BITS
                )
                {
                    ReadTimeout = AbstractSpectraCyberConstants.SPECTRA_CYBER_TIMEOUT_MS,
                    WriteTimeout = AbstractSpectraCyberConstants.SPECTRA_CYBER_TIMEOUT_MS
                };
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException)
                {
                    Console.WriteLine("Failed creating serial port connection.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            try
            {
                ((SpectraCyber)SpectraCyber).SerialPort.Open();
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException
                    || e is InvalidOperationException
                    || e is ArgumentOutOfRangeException
                    || e is ArgumentException
                    || e is UnauthorizedAccessException)
                {
                    Console.WriteLine("Failed opening serial communication.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            try
            {
                // Initialize thread and start it
                SpectraCyber.CommunicationThread = new Thread(new ThreadStart(RunCommunicationThread));
                SpectraCyber.CommunicationThread.Start();
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                {
                    Console.WriteLine("Failed creating communication thread.");
                    return false;
                }
                else if (e is ThreadStartException || e is OutOfMemoryException)
                {
                    Console.WriteLine("Failed starting communication thread.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            Console.WriteLine("Successfully started SpectraCyber communication and communication thread.");
            return true;
        }

        public override bool BringDown()
        {
            try
            {
                if (((SpectraCyber)SpectraCyber).SerialPort != null)
                {
                    ((SpectraCyber)SpectraCyber).SerialPort.Close();
                }
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException)
                {
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            KillCommunicationThreadAndWait();

            Console.WriteLine("Successfully killed SpectraCyber communication and communication thread.");
            return true;
        }

        // Submit a command and return a response
        protected override SpectraCyberResponse SendCommand(SpectraCyberRequest request)
        {
            SpectraCyberResponse response = new SpectraCyberResponse();

            // If the request is empty, don't process
            if (request.IsEmpty())
            {
                return response;
            }

            try
            {
                // Attempt to write the command to the serial port
                ((SpectraCyber)SpectraCyber).SerialPort.Write(request.CommandString);
            }
            catch (Exception)
            {
                // Something went wrong, return the response
                return response;
            }

            // Command was successfully sent through serial communication
            response.RequestSuccessful = true;

            // Give the SpectraCyber some time to process the command
            Thread.Sleep(AbstractSpectraCyberConstants.SPECTRA_CYBER_WAIT_TIME_MS);

            // Check for any significant cases
            switch (request.CommandType)
            {
                // Termination, safely end communication
                case SpectraCyberCommandTypeEnum.TERMINATE:
                    BringDown();
                    break;

                // TODO: implement this case further probably
                case SpectraCyberCommandTypeEnum.SCAN_STOP:
                    break;

                // Purge the serial buffer
                case SpectraCyberCommandTypeEnum.DATA_DISCARD:
                    ((SpectraCyber)SpectraCyber).SerialPort.DiscardInBuffer();
                    break;

                    //
                    // Do nothing by default
                    //
            }

            // If the request expects a reply back, capture the data and attach it to the response
            if (request.WaitForReply)
            {
                // Reponse's data is valid (assuming no exceptions are thrown)
                response.Valid = true;

                try
                {
                    // Create a character array in which to store the buffered characters
                    string hexString;

                    // Read a number of characters in the buffer
                    char[] charInBuffer = new char[AbstractSpectraCyberConstants.SPECTRA_CYBER_BUFFER_SIZE];
                    int length = ((SpectraCyber)SpectraCyber).SerialPort.Read(charInBuffer, 0, request.CharsToRead);

                    // Clip the string to the exact number of bytes read
                    if (AbstractSpectraCyberConstants.SPECTRA_CYBER_CLIP_BUFFER_RESPONSE && (length != AbstractSpectraCyberConstants.SPECTRA_CYBER_BUFFER_SIZE))
                    {
                        char[] actual = new char[length];

                        for (int i = 0; i < length; i++)
                        {
                            actual[i] = charInBuffer[i];
                        }

                        hexString = new string(actual);
                    }

                    // Leave the string how it is, with the possibility of trailing chararacters being "0"
                    else
                    {
                        hexString = new string(charInBuffer);
                    }

                    // Check to see that replyString's first character is what was expected
                    if (hexString[0] != request.ResponseIdentifier)
                    {
                        throw new Exception();
                    }

                    // Convert the hex string into an int
                    response.DecimalData = HexStringToInt(hexString.Substring(1));
                }
                catch (Exception e)
                {
                    // Something went wrong, the response isn't valid
                    Console.WriteLine("Failed to receive a response: " + e.ToString());
                    response.Valid = false;
                }
            }

            // Clear the input buffer
            ((SpectraCyber)SpectraCyber).SerialPort.DiscardInBuffer();

            // Return the response, no matter what happened
            return response;
        }
    }
}