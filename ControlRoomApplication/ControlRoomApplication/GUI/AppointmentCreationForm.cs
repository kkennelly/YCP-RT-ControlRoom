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
        private Appointment _appt;
        private List<User> users;
        private int id;

        public AppointmentCreationForm(int id)
        {
            this.id = id;

            InitializeComponent();

            LoadDefaultValues();

            LoadUsers();

            LoadPriorities();
        }

        private void AddApptBtn_Click(object sender, EventArgs e)
        {
            // Create appointment model
            try
            {
                _appt = new Appointment
                {
                    user_id = users.Find(user => (user.first_name + " " + user.last_name).Equals(UsernameInputList.Text)).Id,
                    start_time = StartTimeInput.Value,
                    end_time = EndTimeInput.Value,
                    status = StatusInput.Text,
                    telescope_id = int.Parse(TelescopeIdInput.Text),
                    Public = Convert.ToInt16(PublicInput.Checked),
                    orientation_id = int.Parse(OrientationIdInput.Text),
                    spectracyber_config_id = int.Parse(SpectraCyberConfigIdInput.Text),
                    type = TypeInput.Text,
                    celestial_body_id = int.Parse(CelestialBodyIdInput.Text),
                    priority = PriorityInputList.Text
                };

                DialogResult = DialogResult.OK;
            } 
            catch (Exception ex)
            {
                MessageBox.Show("One or more inputs are invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Appointment GetAppointment()
        {
            return _appt;
        }

        private void LoadDefaultValues()
        {
            TelescopeIdInput.Text = id.ToString();
            StatusInput.Text = AppointmentStatusEnum.REQUESTED.ToString();
        }

        private void LoadUsers()
        {
            users = Database.DatabaseOperations.GetAllUsers();

            foreach (User user in users)
            {
                UsernameInputList.Items.Add(user.first_name + " " + user.last_name);
            }
        }

        private void LoadPriorities()
        {
            PriorityInputList.Items.Add(AppointmentPriorityEnum.PRIMARY.ToString());
            PriorityInputList.Items.Add(AppointmentPriorityEnum.SECONDARY.ToString());
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void TypeInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
