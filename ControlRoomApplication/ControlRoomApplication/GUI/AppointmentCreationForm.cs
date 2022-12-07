﻿using ControlRoomApplication.Entities;
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
        private List<User> _users;
        private Dictionary<int, CelestialBody> _cbDictionary;
        private Dictionary<int, Entities.Orientation> _orientationDictionary;
        private Dictionary<int, Coordinate> _coordinateDictionary;
        private int _id;

        private bool _isDrift, _isPoint, _isCelestialBody;

        public AppointmentCreationForm(int id)
        {
            this._id = id;

            _isDrift = _isPoint = _isCelestialBody = false;

            _appt = new Appointment();

            InitializeComponent();

            LoadUsers();

            LoadPriorities();

            LoadTypes();

            LoadSpectraCyberConfigs();

            LoadCoordinates();

            LoadOrientations();

            LoadCelestialBodies();

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
                /*
                User user  = users.Find(u => (u.first_name + " " + u.last_name).Equals(UsernameInputList.Text));
                    DateTime start_time = StartDateInput.Value + StartTimeInput.Value.TimeOfDay;
                    DateTime end_time = EndDateInput.Value + EndTimeInput.Value.TimeOfDay;
                    string status = AppointmentStatusEnum.SCHEDULED.ToString();
                    int telescope_id = _id;
                    int Public = Convert.ToInt16(PublicInput.Checked);
                    int spectracyber_config_id = int.Parse(((string)SpectraCyberConfigInputList.SelectedItem).Split('|')[0]);
                    string type = TypeInputList.Text;
                    string priority = PriorityInputList.Text;
                */

                _appt.User = _users.Find(u => (u.first_name + " " + u.last_name).Equals(UsernameInputList.Text));
                _appt.start_time = StartDateInput.Value.AddDays(-1) + StartTimeInput.Value.TimeOfDay;
                _appt.end_time = EndDateInput.Value.AddDays(-1) + EndTimeInput.Value.TimeOfDay;
                _appt.status = AppointmentStatusEnum.SCHEDULED.ToString();
                _appt.telescope_id = _id;
                _appt.Public = Convert.ToInt16(PublicInput.Checked);
                _appt.spectracyber_config_id = int.Parse(((string) SpectraCyberConfigInputList.SelectedItem).Split('|')[0]);
                _appt.type = TypeInputList.Text;
                _appt.priority = PriorityInputList.Text;

                if (_isDrift)
                {
                    int orientationId = int.Parse(OrientationInputList.Text.Split('|')[0]);
                    _appt.Orientation = _orientationDictionary.First(x => x.Key == orientationId).Value;
                    _appt.orientation_id = orientationId;
                }
                else
                {
                    _appt.Orientation = _orientationDictionary[1];
                    _appt.orientation_id = _appt.Orientation.Id;
                }

                if (_isCelestialBody)
                {
                    int cbId = int.Parse(CelestialBodiesInputList.Text.Split('|')[0]);
                    _appt.CelestialBody = _cbDictionary.First(x => x.Key == cbId).Value;
                    _appt.celestial_body_id = cbId;
                    _appt.CelestialBody.Coordinate = _coordinateDictionary.First(x => x.Key == _appt.CelestialBody.coordinate_id).Value;
                }
                else
                {
                    _appt.CelestialBody = _cbDictionary[1];
                    _appt.celestial_body_id = _appt.CelestialBody.Id;
                    _appt.CelestialBody.Coordinate = _coordinateDictionary.First(x => x.Key == _appt.CelestialBody.coordinate_id).Value;
                }

                if (!_isDrift && !_isCelestialBody)
                {
                    _appt.Coordinates.Add(_coordinateDictionary.First(x => x.Key == int.Parse(CoordinateInputList.Text.Split('|')[0])).Value);
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
            _users = Database.DatabaseOperations.GetAllUsers();

            foreach (User user in _users)
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
            _coordinateDictionary = new Dictionary<int, Coordinate>();

            CoordinateInputList.Items.Clear();

            CoordinateInputList.Items.Add("New coordinate");

            List<Coordinate> coordinates = Database.DatabaseOperations.GetAllCoordinates();
            
            foreach(Coordinate coord in coordinates)
            {
                CoordinateInputList.Items.Add(coord.ToString());
                _coordinateDictionary.Add(coord.Id, coord);
            }
        }

        private void LoadOrientations()
        {
            _orientationDictionary = new Dictionary<int, Entities.Orientation>();

            OrientationInputList.Items.Clear();

            OrientationInputList.Items.Add("New orientation");

            List<Entities.Orientation> orientations = Database.DatabaseOperations.GetAllOrientations();

            foreach (Entities.Orientation orientation in orientations)
            {
                OrientationInputList.Items.Add(orientation.ToString());
                _orientationDictionary.Add(orientation.Id, orientation);
            }
        }

        private void LoadCelestialBodies()
        {
            _cbDictionary = new Dictionary<int, CelestialBody>();

            CelestialBodiesInputList.Items.Clear();

            CelestialBodiesInputList.Items.Add("New celestial body");

            List<CelestialBody> cbs = Database.DatabaseOperations.GetAllCelestialBodies();

            foreach(CelestialBody cb in cbs)
            {
                CelestialBodiesInputList.Items.Add(cb.ToString());
                _cbDictionary.Add(cb.Id, cb);
            }
        }

        private void ResetAppointmentType()
        {
            CelestialBodyLabel.Hide();
            OrientationLabel.Hide();
            CoordinateLabel.Hide();

            CoordinateInputList.Hide();
            CelestialBodiesInputList.Hide();
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
                    CelestialBodiesInputList.Show();
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
                    _appt.SpectraCyberConfig = config;
                    MessageBox.Show("Successfully added SpectraCyber Configuration to DB and Appointment!");

                    LoadSpectraCyberConfigs();
                }
                else
                {
                    scForm.Dispose();
                }
            } else
            {
                List<SpectraCyberConfig> configs = Database.DatabaseOperations.GetAllSpectraCyberConfigs();
                _appt.SpectraCyberConfig = configs[SpectraCyberConfigInputList.SelectedIndex]; 
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
                    _appt.Coordinates.Add(coordinate);
                    MessageBox.Show("Successfully added coordinate to DB and Appointment!");

                    LoadCoordinates();
                }
                else
                {
                    coordinateForm.Dispose();
                }
            } else
            {
                List<Coordinate> coords = Database.DatabaseOperations.GetAllCoordinates();
                _appt.Coordinates.Add(coords[CoordinateInputList.SelectedIndex]);
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
                    _appt.Orientation = orientation;
                    MessageBox.Show("Successfully added orientation to DB and Appointment!");

                    LoadOrientations();
                }
                else
                {
                    orientationForm.Dispose();
                }
            } else
            {
                List<Entities.Orientation> orientations = Database.DatabaseOperations.GetAllOrientations();
                _appt.Orientation = orientations[OrientationInputList.SelectedIndex];
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