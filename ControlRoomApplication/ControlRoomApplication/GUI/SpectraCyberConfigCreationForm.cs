using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlRoomApplication.GUI
{
    public partial class SpectraCyberConfigCreationForm : Form
    {
        public double _integrationTime { get; set; }
        public double _offsetVoltage { get; set; }
        public double _ifGain { get; set; }
        public int _dcGain { get; set; }
        public int _bandwidth { get; set; }

        public SpectraCyberConfigCreationForm()
        {
            InitializeComponent();

            LoadModes();

            LoadIntegrationTimes();

            LoadDCGains();

            LoadBandwidths();
        }

        private void LoadModes()
        {
            ModeInputList.Items.Add(SpectraCyberModeTypeEnum.CONTINUUM);
            ModeInputList.Items.Add(SpectraCyberModeTypeEnum.SPECTRAL);
            ModeInputList.Items.Add(SpectraCyberModeTypeEnum.NONE);
        }

        private void LoadIntegrationTimes()
        {
            IntegrationTimeInputList.Items.Add(SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN);
            IntegrationTimeInputList.Items.Add(SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN);
            IntegrationTimeInputList.Items.Add(SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN);
        }

        private void LoadDCGains()
        {
            DCGainInputList.Items.Add(SpectraCyberDCGainEnum.X1);
            DCGainInputList.Items.Add(SpectraCyberDCGainEnum.X5);
            DCGainInputList.Items.Add(SpectraCyberDCGainEnum.X10);
            DCGainInputList.Items.Add(SpectraCyberDCGainEnum.X20);
            DCGainInputList.Items.Add(SpectraCyberDCGainEnum.X50);
            DCGainInputList.Items.Add(SpectraCyberDCGainEnum.X60);
        }

        private void LoadBandwidths()
        {
            BandwidthInputList.Items.Add(SpectraCyberBandwidthEnum.SMALL_BANDWIDTH);
            BandwidthInputList.Items.Add(SpectraCyberBandwidthEnum.LARGE_BANDWIDTH);
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            bool invalidInput = false;

            if (ModeInputList.SelectedIndex == -1)
            {
                invalidInput = true;
            }

            try
            {
                _integrationTime = SpectraCyberIntegrationTimeEnumHelper.GetDoubleValue((SpectraCyberIntegrationTimeEnum) IntegrationTimeInputList.SelectedItem);
                _offsetVoltage = Convert.ToDouble(OffsetVoltageInput.Text);
                _ifGain = Convert.ToDouble(IFGainInput.Text);
                _dcGain = SpectraCyberDCGainEnumHelper.GetIntegerValue((SpectraCyberDCGainEnum) DCGainInputList.SelectedItem);
                _bandwidth = Convert.ToInt16((SpectraCyberBandwidthEnum) BandwidthInputList.SelectedItem);
            }
            catch (Exception ex)
            {
                invalidInput = true;
                
            }

            if (!invalidInput)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("One or more inputs are invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
