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
    public partial class AppointmentControlForm : Form
    {
        private int id;

        public AppointmentControlForm(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void AddApptBtn_Click(object sender, EventArgs e)
        {
            var addApptForm = new AppointmentCreationForm(id);

            if (addApptForm.ShowDialog() == DialogResult.OK)
            {
                Database.DatabaseOperations.AddAppointment(addApptForm.GetAppointment());

                MessageBox.Show("Successfully added appointment!");
            }
            else
            {
                addApptForm.Dispose();
            }
        }

        private void AddUserBtn_Click(object sender, EventArgs e)
        {
            var addUserForm = new UserCreationForm();

            if (addUserForm.ShowDialog() == DialogResult.OK)
            {
                Database.DatabaseOperations.AddUser(addUserForm.GetUser(), addUserForm.GetUserRole());

                MessageBox.Show("Successfully added user!");
            } 
            else
            {
                addUserForm.Dispose();
            }
        }

        private void ViewApptButton_Click(object sender, EventArgs e)
        {
            var viewApptForm = new ViewAppointmentForm(id);

            viewApptForm.Show();
        }

        private void ViewUserBtn_Click(object sender, EventArgs e)
        {
            var viewUsersForm = new ViewUsersForm();

            viewUsersForm.Show();
        }
    }
}
