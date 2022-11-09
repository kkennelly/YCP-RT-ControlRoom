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
    public partial class UserCreationForm : Form
    {
        public UserCreationForm()
        {
            InitializeComponent();
        }

        private void CreateUserBtn_Click(object sender, EventArgs e)
        {
            var userModel = new User
            {
                first_name = FirstNameInput.Text,
                last_name = LastNameInput.Text,
                email_address = EmailInput.Text,
                // No Company? = CompanyInput.Text,
                phone_number = PhoneInput.Text,
                // No password? = PasswordInput.Text,
                // No Active? = ActiveInput.Text,
                // No Status? = StatusInput.Text,
                // No Picture Approved? = PictureApprovedInput.Text,
                // No Picture Input? = ProfilePictureInput.Text,
                notification_type = NotificationTypeInput.Text,
                firebase_id = FirebaseInput.Text
            };

            // Work with the backend team to insert a new user into the database

        }
    }
}
