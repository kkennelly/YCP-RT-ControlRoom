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
        public AppointmentControlForm()
        {
            InitializeComponent();
        }

        private void AddApptBtn_Click(object sender, EventArgs e)
        {
            var addApptForm = new AppointmentCreationForm();

            addApptForm.Show();
        }

        private void AddUserBtn_Click(object sender, EventArgs e)
        {
            var addUserForm = new UserCreationForm();

            addUserForm.Show();
        }
    }
}
