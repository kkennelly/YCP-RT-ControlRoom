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
    public partial class CoordinateCreationForm : Form
    {
        public double _rightAscension { get; set; }
        public double _declination { get; set; }

        public CoordinateCreationForm()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _rightAscension = Double.Parse(RightAscensionInput.Text);
                _declination = Double.Parse(DeclinationInput.Text);

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
