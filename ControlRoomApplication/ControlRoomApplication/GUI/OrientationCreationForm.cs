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
    public partial class OrientationCreationForm : Form
    {
        public double _azimuth { get; set; }
        public double _elevation { get; set; }

        public OrientationCreationForm()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _azimuth = Double.Parse(AzimuthInput.Text);
                _elevation = Double.Parse(ElevationInput.Text);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("One or more inputs are invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
