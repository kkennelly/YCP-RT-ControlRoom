using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.GUI
{
    public partial class CustomOrientationInputDialog : InputDialog
    {
        public double ElevationHighLimit { get; private set; }
        public double ElevationLowLimit { get; private set; }
        public double ElevationPos { get; private set; }
        public double AzimuthPos { get; private set; }
        public string[] Values;
        private Regex rx;
        double TempAz, TempElev;

        public CustomOrientationInputDialog(bool EnableSoftwareStops, string teleType, double maxElevationDegrees, double minElevationDegrees)
        {
            InitializeComponent();

            rx = new Regex(@"^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+),[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$");   // Regex statement to validate input (Format of #,#)

            okButton.Enabled = false;   // Disable the OK button by default     
            invalidInputLabel.Visible = false;      // Don't show the invalid input label at first to avoid confusing the user 

            textBox.TextChanged += new EventHandler(TextBox_TextChanged);

            if (EnableSoftwareStops)    // Set limits depending on whether software stops are enabled or not 
            {
                ElevationHighLimit = maxElevationDegrees;
                ElevationLowLimit = minElevationDegrees;
            }
            else
            {
                ElevationHighLimit = SimulationConstants.LIMIT_HIGH_EL_DEGREES;
                ElevationLowLimit = SimulationConstants.LIMIT_LOW_EL_DEGREES;
            }

            promptLabel.Text = "The Radio Telescope is currently set to be type " + teleType + "." +
                            " This script is best run with a telescope type of SLIP_RING.\n\n" +
                            "Please type an a custom orientation containing azimuth between 0 and 360 degrees," +
                                " and elevation between " + ElevationLowLimit + " and " + ElevationHighLimit +
                                " degrees. Format the entry as a comma-separated list in the format " +
                                "azimuth, elevation. Ex: 55,80";
        }
        
        public void SetPrompt(string text)
        {
            promptLabel.Text = text;
        }

        public void TextBox_TextChanged(Object sender, EventArgs e)
        {
            if (rx.IsMatch(textBox.Text))
            {
                Values = textBox.Text.Split(',');

                Double.TryParse(Values[0], out TempAz);
                Double.TryParse(Values[1], out TempElev);

                AzimuthPos = TempAz;
                ElevationPos = TempElev;

                // Enable the OK button and hide the invalid input label if the input is valid. Otherwise grey out the OK button and hide the label. 
                if ((AzimuthPos > 360 || AzimuthPos < 0) || (ElevationPos > ElevationHighLimit || ElevationPos < ElevationLowLimit))
                {
                    okButton.Enabled = false;
                    invalidInputLabel.Visible = true;
                }
                else
                {
                    okButton.Enabled = true;
                    invalidInputLabel.Visible = false;
                }
            }
            else
            {
                okButton.Enabled = false;
                invalidInputLabel.Visible = true;
            }
        }
    }
}
