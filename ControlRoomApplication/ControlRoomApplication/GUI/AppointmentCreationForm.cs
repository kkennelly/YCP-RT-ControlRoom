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
        private int _id;

        private bool _isDrift, _isPoint, _isCelestialBody;

        public AppointmentCreationForm(int id)
        {
            this._id = id;

            _isDrift = _isPoint = _isCelestialBody = false;

            InitializeComponent();

            LoadUsers();

            LoadPriorities();

            LoadTypes();

            LoadSpectraCyberConfigs();

            LoadCoordinates();

            LoadOrientations();

            ResetAppointmentType();

            // Event listeners 
            TypeInputList.TextChanged += new EventHandler(TypeInputList_TextChanged);
            SpectraCyberConfigInputList.TextChanged += new EventHandler(SpectraCyberConfigInputList_TextChanged);
            CoordinateInputList.TextChanged += new EventHandler(CoordinateInputList_TextChanged);
            OrientationInputList.TextChanged += new EventHandler(OrientationInputList_TextChanged);
        }

        private void AddApptBtn_Click(object sender, EventArgs e)
        {
            // Create appointment model
            try
            {
                _appt = new Appointment
                {
                    User = users.Find(user => (user.first_name + " " + user.last_name).Equals(UsernameInputList.Text)),
                    start_time = StartDateInput.Value + StartTimeInput.Value.TimeOfDay,
                    end_time = EndDateInput.Value + EndTimeInput.Value.TimeOfDay,
                    status = AppointmentStatusEnum.SCHEDULED.ToString(),
                    telescope_id = _id,
                    Public = Convert.ToInt16(PublicInput.Checked),
                    spectracyber_config_id = (int) SpectraCyberConfigInputList.SelectedItem,
                    type = TypeInputList.Text,
                    priority = PriorityInputList.Text
                };

                if (!_isDrift)
                {
                    if (_isCelestialBody)
                    {
                        _appt.celestial_body_id = int.Parse(CelestialBodyIdInput.Text.Split('|')[0]);
                    }

                    // Add coordinate(s) 

                }
                else
                {
                    _appt.orientation_id = int.Parse(OrientationInputList.Text.Split('|')[0]);
                }


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
            SpectraCyberConfigInputList.Items.Clear();

            SpectraCyberConfigInputList.Items.Add("New config");

            List<SpectraCyberConfig> configs = Database.DatabaseOperations.GetAllSpectraCyberConfigs();

            foreach (SpectraCyberConfig config in configs)
            {
                SpectraCyberConfigInputList.Items.Add(config.ToString());
            }
        }

        private void LoadCoordinates()
        {
            CoordinateInputList.Items.Clear();

            CoordinateInputList.Items.Add("New coordinate");

            List<Coordinate> coordinates = Database.DatabaseOperations.GetAllCoordinates();
            
            foreach(Coordinate coord in coordinates)
            {
                CoordinateInputList.Items.Add(coord.ToString());
            }
        }

        private void LoadOrientations()
        {
            OrientationInputList.Items.Clear();

            OrientationInputList.Items.Add("New orientation");

            List<Entities.Orientation> orientations = Database.DatabaseOperations.GetAllOrientations();

            foreach (Entities.Orientation orientation in orientations)
            {
                OrientationInputList.Items.Add(orientation.ToString());
            }
        }

        private void ResetAppointmentType()
        {
            CelestialBodyLabel.Hide();
            OrientationLabel.Hide();
            CoordinateLabel.Hide();

            CoordinateInputList.Hide();
            CelestialBodyIdInput.Hide();
            OrientationInputList.Hide();

            _isDrift = _isCelestialBody = false;
        }

        private void TypeInputList_TextChanged(object sender, EventArgs e)
        {
            ResetAppointmentType();
            

            switch (TypeInputList.SelectedIndex)
            {
                case 3: // DRIFT
                    _isDrift = true;

                    OrientationLabel.Show();
                    OrientationInputList.Show();
                    break;

                case 2: // CELESTIAL_BODY
                    _isCelestialBody = true;

                    CelestialBodyLabel.Show();
                    CelestialBodyIdInput.Show();
                    break;

                default: // anything else 
                    CoordinateLabel.Show();
                    CoordinateInputList.Show();
                    break;
            }
        }

        private void SpectraCyberConfigInputList_TextChanged(object sender, EventArgs e)
        {
            if (SpectraCyberConfigInputList.SelectedIndex == 0) // New config
            { 
                var scForm = new SpectraCyberConfigCreationForm();

                if (scForm.ShowDialog() == DialogResult.OK)
                {
                    SpectraCyberConfig config = new SpectraCyberConfig
                    {
                        mode = scForm._mode.ToString(),
                        IntegrationTime = scForm._integrationTime,
                        offset_voltage = scForm._offsetVoltage,
                        if_gain = scForm._ifGain,
                        DCGain = scForm._dcGain
                    };

                    Database.DatabaseOperations.AddSpectraCyberConfig(config);
                    MessageBox.Show("Successfully added SpectraCyber Configuration!");

                    LoadSpectraCyberConfigs();
                }
                else
                {
                    scForm.Dispose();
                }
            }
        }

        private void CoordinateInputList_TextChanged(object sender, EventArgs e)
        {

            if (CoordinateInputList.SelectedIndex == 0)
            {
                var coordinateForm = new CoordinateCreationForm();

                if (coordinateForm.ShowDialog() == DialogResult.OK)
                {
                    Coordinate coordinate = new Coordinate
                    {
                        right_ascension = coordinateForm._rightAscension,
                        declination = coordinateForm._declination,
                    };

                    Database.DatabaseOperations.AddCoordinate(coordinate);
                    MessageBox.Show("Successfully added coordinate!");

                    LoadCoordinates();
                }
                else
                {
                    coordinateForm.Dispose();
                }
            }
        }

        private void OrientationInputList_TextChanged(object sender, EventArgs e)
        {
            if (OrientationInputList.SelectedIndex == 0)
            {
                var orientationForm = new OrientationCreationForm();

                if (orientationForm.ShowDialog() == DialogResult.OK)
                {
                    Entities.Orientation orientation = new Entities.Orientation
                    {
                        azimuth = orientationForm._azimuth,
                        elevation = orientationForm._elevation
                    };

                    Database.DatabaseOperations.AddOrientation(orientation);
                    MessageBox.Show("Successfully added orientation!");

                    LoadOrientations();
                }
                else
                {
                    orientationForm.Dispose();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

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
