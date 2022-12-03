using ControlRoomApplication.Entities;
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

            LoadTypes();

            LoadSpectraCyberConfigs();

            // Event listeners 
            TypeInputList.TextChanged += new EventHandler(TypeInputList_TextChanged);
        }

        private void AddApptBtn_Click(object sender, EventArgs e)
        {
            // Create appointment model
            try
            {
                _appt = new Appointment
                {
                    //user_id = users.Find(user => (user.first_name + " " + user.last_name).Equals(UsernameInputList.Text)).Id,
                    User = users.Find(user => (user.first_name + " " + user.last_name).Equals(UsernameInputList.Text)),
                    start_time = StartTimeInput.Value,
                    end_time = EndTimeInput.Value,
                    status = StatusInput.Text,
                    telescope_id = int.Parse(TelescopeIdInput.Text),
                    Public = Convert.ToInt16(PublicInput.Checked),
                    orientation_id = int.Parse(OrientationIdInput.Text),
                    spectracyber_config_id = (int) SpectraCyberConfigInputList.SelectedItem,
                    type = TypeInputList.Text,
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
            StatusInput.Text = AppointmentStatusEnum.SCHEDULED.ToString();
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

        private void LoadTypes()
        {
            TypeInputList.Items.Add(AppointmentTypeEnum.POINT.ToString());
            TypeInputList.Items.Add(AppointmentTypeEnum.RASTER.ToString());
            TypeInputList.Items.Add(AppointmentTypeEnum.CELESTIAL_BODY.ToString());
            TypeInputList.Items.Add(AppointmentTypeEnum.DRIFT_SCAN.ToString());
            TypeInputList.Items.Add(AppointmentTypeEnum.UNDEFINED.ToString());
        }

        private void LoadSpectraCyberConfigs()
        {
            List<SpectraCyberConfig> configs = Database.DatabaseOperations.GetAllSpectraCyberConfigs();
            
            foreach (SpectraCyberConfig config in configs)
            {
                SpectraCyberConfigInputList.Items.Add(config.Id);
            }

            SpectraCyberConfigInputList.Items.Add("New config");
        }

        private void TypeInputList_TextChanged(object sender, EventArgs e)
        {
            switch (TypeInputList.SelectedIndex)
            {
                case 0: // POINT
                    // display the coordinate input form 
                    Regex rx = new Regex(@"^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+),[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$"); // Regex statement to validate input (Format of #,#)
                    
                    break;
                case 2: // CELESTIAL_BODY
                    // display the celestial body input form 

                    break;
                default:
                    // hide both the coordinate and celetial body elements 

                    break;
            }   
        }

        private void SpectraCyberConfigInputList_TextChanged(object sender, EventArgs e)
        {
            var scForm = new SpectraCyberConfigCreationForm();

            if (scForm.ShowDialog() == DialogResult.OK)
            {
                SpectraCyberConfig config = new SpectraCyberConfig
                {
                    mode = scForm._mode,
                    IntegrationTime = scForm._integrationTime,
                    OffsetVoltage = scForm._offsetVoltage,
                    IFGain = scForm._ifGain,
                    DCGain = scForm._dcGain
                };
            }
            else
            {
                scForm.Dispose();
            }
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
