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
    public partial class AppointmentCreationForm : Form
    {
        public AppointmentCreationForm()
        {
            InitializeComponent();
        }

        private void AddApptBtn_Click(object sender, EventArgs e)
        {
            // Create appointment model
            var appt = new Appointment
            {
                user_id = int.Parse(UserIdInput.Text),
                start_time = DateTime.Parse(StartTimeInput.Text),
                end_time = DateTime.Parse(EndTimeInput.Text),
                status = StatusInput.Text,
                telescope_id = int.Parse(TelescopeIdInput.Text),
                Public = int.Parse(PublicInput.Text),
                orientation_id = int.Parse(OrientationIdInput.Text).,
                spectracyber_config_id = int.Parse(SpectraCyberConfigIdInput.Text),
                type = TypeInput.Text,
                celestial_body_id = int.Parse(CelestialBodyIdInput.Text),
                priority = PriorityInput.Text
            };

            // Work with backend to insert this appointment
        }
    }
}
