﻿using ControlRoomApplication.Constants;
using System.IO.Ports;

namespace ControlRoomApplication.Entities
{
    public class SpectraCyber : AbstractSpectraCyber
    {
        public string CommPort { get; }
        public SerialPort SerialPort { get; set; }

        public SpectraCyber(string commPort) : base()
        {
            CommPort = commPort;
        }

        public SpectraCyber() : this(AbstractSpectraCyberConstants.SPECTRA_CYBER_DEFAULT_COMM_PORT) { }
    }
}