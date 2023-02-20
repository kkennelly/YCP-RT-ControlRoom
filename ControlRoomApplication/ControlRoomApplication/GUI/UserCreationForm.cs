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
        private User _user;

        public UserCreationForm()
        {
            InitializeComponent();

            LoadRoles();

            LoadNotificationTypes();
        }

        private void CreateUserBtn_Click(object sender, EventArgs e)
        {
            try
            {
                _user = new User
                {
                    first_name = FirstNameInput.Text,
                    last_name = LastNameInput.Text,
                    email_address = EmailInput.Text,
                    phone_number = PhoneInput.Text,
                    //UR = new UserRole(?, (UserRoleEnum) RoleInput.SelectedItem),
                    notification_type = NotificationTypeInput.Text
                    // No password? = PasswordInput.Text,
                    // No Active? = ActiveInput.Text,
                    // No Status? = StatusInput.Text,
                    // No Picture Approved? = PictureApprovedInput.Text,
                    // No Picture Input? = ProfilePictureInput.Text,
                };

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("One or more inputs are invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public User GetUser()
        {
            return _user;
        }
        public string GetUserRole()
        {
            return RoleInput.Text;
        }

        private void LoadRoles()
        {
            RoleInput.Items.Add(UserRoleEnum.ADMIN.ToString());
            RoleInput.Items.Add(UserRoleEnum.STUDENT.ToString());
            RoleInput.Items.Add(UserRoleEnum.GUEST.ToString());
            RoleInput.Items.Add(UserRoleEnum.MEMBER.ToString());
            RoleInput.Items.Add(UserRoleEnum.RESEARCHER.ToString());
            RoleInput.SelectedIndex = 2; // Added to prevent no-role errors- will now default to GUEST
        }

        private void LoadNotificationTypes()
        {
            NotificationTypeInput.Items.Add(NotificationTypeEnum.EMAIL.ToString());
            NotificationTypeInput.Items.Add(NotificationTypeEnum.SMS.ToString());
            NotificationTypeInput.Items.Add(NotificationTypeEnum.ALL.ToString());
        }

        private void ProfilePictureInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void UserCreationForm_Load(object sender, EventArgs e)
        {

        }

        private void FirstNameInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void EmailLabel_Click(object sender, EventArgs e)
        {

        }

        private void EmailInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void PhoneLabel_Click(object sender, EventArgs e)
        {

        }

        private void PhoneInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void ActiveLabel_Click(object sender, EventArgs e)
        {

        }

        private void ActiveInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProfilePictureApprovedLabel_Click(object sender, EventArgs e)
        {

        }

        private void PictureApprovedInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void NotificationTypeLabel_Click(object sender, EventArgs e)
        {

        }

        private void NotificationTypeInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void LastNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void FirstNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void LastNameInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void CompanyLabel_Click(object sender, EventArgs e)
        {

        }

        private void CompanyInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void PasswordLabel_Click(object sender, EventArgs e)
        {

        }

        private void PasswordInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void StatusLabel_Click(object sender, EventArgs e)
        {

        }

        private void StatusInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProfilePictureLabel_Click(object sender, EventArgs e)
        {

        }

        private void FirebaseLabel_Click(object sender, EventArgs e)
        {

        }

        private void FirebaseInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
