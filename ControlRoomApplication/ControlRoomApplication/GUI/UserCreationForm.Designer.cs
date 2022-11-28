
namespace ControlRoomApplication.GUI
{
    partial class UserCreationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProfilePictureLabel = new System.Windows.Forms.Label();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.CompanyLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.PhoneLabel = new System.Windows.Forms.Label();
            this.ActiveLabel = new System.Windows.Forms.Label();
            this.ProfilePictureApprovedLabel = new System.Windows.Forms.Label();
            this.NotificationTypeLabel = new System.Windows.Forms.Label();
            this.FirstNameLabel = new System.Windows.Forms.Label();
            this.PictureApprovedInput = new System.Windows.Forms.TextBox();
            this.LastNameInput = new System.Windows.Forms.TextBox();
            this.CompanyInput = new System.Windows.Forms.TextBox();
            this.ProfilePictureInput = new System.Windows.Forms.TextBox();
            this.NotificationTypeInput = new System.Windows.Forms.TextBox();
            this.StatusInput = new System.Windows.Forms.TextBox();
            this.ActiveInput = new System.Windows.Forms.TextBox();
            this.PasswordInput = new System.Windows.Forms.TextBox();
            this.PhoneInput = new System.Windows.Forms.TextBox();
            this.EmailInput = new System.Windows.Forms.TextBox();
            this.FirstNameInput = new System.Windows.Forms.TextBox();
            this.FirebaseLabel = new System.Windows.Forms.Label();
            this.FirebaseInput = new System.Windows.Forms.TextBox();
            this.CreateUserBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProfilePictureLabel
            // 
            this.ProfilePictureLabel.AutoSize = true;
            this.ProfilePictureLabel.Location = new System.Drawing.Point(335, 218);
            this.ProfilePictureLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProfilePictureLabel.Name = "ProfilePictureLabel";
            this.ProfilePictureLabel.Size = new System.Drawing.Size(72, 13);
            this.ProfilePictureLabel.TabIndex = 44;
            this.ProfilePictureLabel.Text = "Profile Picture";
            this.ProfilePictureLabel.Click += new System.EventHandler(this.ProfilePictureLabel_Click);
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(146, 106);
            this.EmailLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(32, 13);
            this.EmailLabel.TabIndex = 43;
            this.EmailLabel.Text = "Email";
            this.EmailLabel.Click += new System.EventHandler(this.EmailLabel_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(371, 180);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.StatusLabel.TabIndex = 42;
            this.StatusLabel.Text = "Status";
            this.StatusLabel.Click += new System.EventHandler(this.StatusLabel_Click);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(356, 141);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.PasswordLabel.TabIndex = 41;
            this.PasswordLabel.Text = "Password";
            this.PasswordLabel.Click += new System.EventHandler(this.PasswordLabel_Click);
            // 
            // CompanyLabel
            // 
            this.CompanyLabel.AutoSize = true;
            this.CompanyLabel.Location = new System.Drawing.Point(357, 106);
            this.CompanyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CompanyLabel.Name = "CompanyLabel";
            this.CompanyLabel.Size = new System.Drawing.Size(51, 13);
            this.CompanyLabel.TabIndex = 40;
            this.CompanyLabel.Text = "Company";
            this.CompanyLabel.Click += new System.EventHandler(this.CompanyLabel_Click);
            // 
            // LastNameLabel
            // 
            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.Location = new System.Drawing.Point(351, 71);
            this.LastNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.LastNameLabel.TabIndex = 39;
            this.LastNameLabel.Text = "Last Name";
            this.LastNameLabel.Click += new System.EventHandler(this.LastNameLabel_Click);
            // 
            // PhoneLabel
            // 
            this.PhoneLabel.AutoSize = true;
            this.PhoneLabel.Location = new System.Drawing.Point(100, 141);
            this.PhoneLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PhoneLabel.Name = "PhoneLabel";
            this.PhoneLabel.Size = new System.Drawing.Size(78, 13);
            this.PhoneLabel.TabIndex = 38;
            this.PhoneLabel.Text = "Phone Number";
            this.PhoneLabel.Click += new System.EventHandler(this.PhoneLabel_Click);
            // 
            // ActiveLabel
            // 
            this.ActiveLabel.AutoSize = true;
            this.ActiveLabel.Location = new System.Drawing.Point(143, 180);
            this.ActiveLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ActiveLabel.Name = "ActiveLabel";
            this.ActiveLabel.Size = new System.Drawing.Size(37, 13);
            this.ActiveLabel.TabIndex = 37;
            this.ActiveLabel.Text = "Active";
            this.ActiveLabel.Click += new System.EventHandler(this.ActiveLabel_Click);
            // 
            // ProfilePictureApprovedLabel
            // 
            this.ProfilePictureApprovedLabel.AutoSize = true;
            this.ProfilePictureApprovedLabel.Location = new System.Drawing.Point(89, 218);
            this.ProfilePictureApprovedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProfilePictureApprovedLabel.Name = "ProfilePictureApprovedLabel";
            this.ProfilePictureApprovedLabel.Size = new System.Drawing.Size(89, 13);
            this.ProfilePictureApprovedLabel.TabIndex = 36;
            this.ProfilePictureApprovedLabel.Text = "Picture Approved";
            this.ProfilePictureApprovedLabel.Click += new System.EventHandler(this.ProfilePictureApprovedLabel_Click);
            // 
            // NotificationTypeLabel
            // 
            this.NotificationTypeLabel.AutoSize = true;
            this.NotificationTypeLabel.Location = new System.Drawing.Point(92, 253);
            this.NotificationTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NotificationTypeLabel.Name = "NotificationTypeLabel";
            this.NotificationTypeLabel.Size = new System.Drawing.Size(87, 13);
            this.NotificationTypeLabel.TabIndex = 35;
            this.NotificationTypeLabel.Text = "Notification Type";
            this.NotificationTypeLabel.Click += new System.EventHandler(this.NotificationTypeLabel_Click);
            // 
            // FirstNameLabel
            // 
            this.FirstNameLabel.AutoSize = true;
            this.FirstNameLabel.Location = new System.Drawing.Point(120, 71);
            this.FirstNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FirstNameLabel.Name = "FirstNameLabel";
            this.FirstNameLabel.Size = new System.Drawing.Size(57, 13);
            this.FirstNameLabel.TabIndex = 34;
            this.FirstNameLabel.Text = "First Name";
            this.FirstNameLabel.Click += new System.EventHandler(this.FirstNameLabel_Click);
            // 
            // PictureApprovedInput
            // 
            this.PictureApprovedInput.Location = new System.Drawing.Point(182, 215);
            this.PictureApprovedInput.Margin = new System.Windows.Forms.Padding(2);
            this.PictureApprovedInput.Name = "PictureApprovedInput";
            this.PictureApprovedInput.Size = new System.Drawing.Size(76, 20);
            this.PictureApprovedInput.TabIndex = 33;
            this.PictureApprovedInput.TextChanged += new System.EventHandler(this.PictureApprovedInput_TextChanged);
            // 
            // LastNameInput
            // 
            this.LastNameInput.Location = new System.Drawing.Point(412, 69);
            this.LastNameInput.Margin = new System.Windows.Forms.Padding(2);
            this.LastNameInput.Name = "LastNameInput";
            this.LastNameInput.Size = new System.Drawing.Size(76, 20);
            this.LastNameInput.TabIndex = 32;
            this.LastNameInput.TextChanged += new System.EventHandler(this.LastNameInput_TextChanged);
            // 
            // CompanyInput
            // 
            this.CompanyInput.Location = new System.Drawing.Point(412, 104);
            this.CompanyInput.Margin = new System.Windows.Forms.Padding(2);
            this.CompanyInput.Name = "CompanyInput";
            this.CompanyInput.Size = new System.Drawing.Size(76, 20);
            this.CompanyInput.TabIndex = 31;
            this.CompanyInput.TextChanged += new System.EventHandler(this.CompanyInput_TextChanged);
            // 
            // ProfilePictureInput
            // 
            this.ProfilePictureInput.Location = new System.Drawing.Point(412, 215);
            this.ProfilePictureInput.Margin = new System.Windows.Forms.Padding(2);
            this.ProfilePictureInput.Name = "ProfilePictureInput";
            this.ProfilePictureInput.Size = new System.Drawing.Size(76, 20);
            this.ProfilePictureInput.TabIndex = 30;
            this.ProfilePictureInput.TextChanged += new System.EventHandler(this.ProfilePictureInput_TextChanged);
            // 
            // NotificationTypeInput
            // 
            this.NotificationTypeInput.Location = new System.Drawing.Point(182, 251);
            this.NotificationTypeInput.Margin = new System.Windows.Forms.Padding(2);
            this.NotificationTypeInput.Name = "NotificationTypeInput";
            this.NotificationTypeInput.Size = new System.Drawing.Size(76, 20);
            this.NotificationTypeInput.TabIndex = 29;
            this.NotificationTypeInput.TextChanged += new System.EventHandler(this.NotificationTypeInput_TextChanged);
            // 
            // StatusInput
            // 
            this.StatusInput.Location = new System.Drawing.Point(412, 178);
            this.StatusInput.Margin = new System.Windows.Forms.Padding(2);
            this.StatusInput.Name = "StatusInput";
            this.StatusInput.Size = new System.Drawing.Size(76, 20);
            this.StatusInput.TabIndex = 28;
            this.StatusInput.TextChanged += new System.EventHandler(this.StatusInput_TextChanged);
            // 
            // ActiveInput
            // 
            this.ActiveInput.Location = new System.Drawing.Point(182, 178);
            this.ActiveInput.Margin = new System.Windows.Forms.Padding(2);
            this.ActiveInput.Name = "ActiveInput";
            this.ActiveInput.Size = new System.Drawing.Size(76, 20);
            this.ActiveInput.TabIndex = 27;
            this.ActiveInput.TextChanged += new System.EventHandler(this.ActiveInput_TextChanged);
            // 
            // PasswordInput
            // 
            this.PasswordInput.Location = new System.Drawing.Point(412, 139);
            this.PasswordInput.Margin = new System.Windows.Forms.Padding(2);
            this.PasswordInput.Name = "PasswordInput";
            this.PasswordInput.PasswordChar = '*';
            this.PasswordInput.Size = new System.Drawing.Size(76, 20);
            this.PasswordInput.TabIndex = 26;
            this.PasswordInput.TextChanged += new System.EventHandler(this.PasswordInput_TextChanged);
            // 
            // PhoneInput
            // 
            this.PhoneInput.Location = new System.Drawing.Point(182, 139);
            this.PhoneInput.Margin = new System.Windows.Forms.Padding(2);
            this.PhoneInput.Name = "PhoneInput";
            this.PhoneInput.Size = new System.Drawing.Size(76, 20);
            this.PhoneInput.TabIndex = 25;
            this.PhoneInput.TextChanged += new System.EventHandler(this.PhoneInput_TextChanged);
            // 
            // EmailInput
            // 
            this.EmailInput.Location = new System.Drawing.Point(182, 104);
            this.EmailInput.Margin = new System.Windows.Forms.Padding(2);
            this.EmailInput.Name = "EmailInput";
            this.EmailInput.Size = new System.Drawing.Size(76, 20);
            this.EmailInput.TabIndex = 24;
            this.EmailInput.TextChanged += new System.EventHandler(this.EmailInput_TextChanged);
            // 
            // FirstNameInput
            // 
            this.FirstNameInput.Location = new System.Drawing.Point(182, 69);
            this.FirstNameInput.Margin = new System.Windows.Forms.Padding(2);
            this.FirstNameInput.Name = "FirstNameInput";
            this.FirstNameInput.Size = new System.Drawing.Size(76, 20);
            this.FirstNameInput.TabIndex = 23;
            this.FirstNameInput.TextChanged += new System.EventHandler(this.FirstNameInput_TextChanged);
            // 
            // FirebaseLabel
            // 
            this.FirebaseLabel.AutoSize = true;
            this.FirebaseLabel.Location = new System.Drawing.Point(347, 253);
            this.FirebaseLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FirebaseLabel.Name = "FirebaseLabel";
            this.FirebaseLabel.Size = new System.Drawing.Size(61, 13);
            this.FirebaseLabel.TabIndex = 46;
            this.FirebaseLabel.Text = "Firebase ID";
            this.FirebaseLabel.Click += new System.EventHandler(this.FirebaseLabel_Click);
            // 
            // FirebaseInput
            // 
            this.FirebaseInput.Location = new System.Drawing.Point(412, 251);
            this.FirebaseInput.Margin = new System.Windows.Forms.Padding(2);
            this.FirebaseInput.Name = "FirebaseInput";
            this.FirebaseInput.Size = new System.Drawing.Size(76, 20);
            this.FirebaseInput.TabIndex = 45;
            this.FirebaseInput.TextChanged += new System.EventHandler(this.FirebaseInput_TextChanged);
            // 
            // CreateUserBtn
            // 
            this.CreateUserBtn.Location = new System.Drawing.Point(305, 299);
            this.CreateUserBtn.Margin = new System.Windows.Forms.Padding(2);
            this.CreateUserBtn.Name = "CreateUserBtn";
            this.CreateUserBtn.Size = new System.Drawing.Size(109, 21);
            this.CreateUserBtn.TabIndex = 47;
            this.CreateUserBtn.Text = "Create User";
            this.CreateUserBtn.UseVisualStyleBackColor = true;
            this.CreateUserBtn.Click += new System.EventHandler(this.CreateUserBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(191, 299);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(109, 21);
            this.CancelBtn.TabIndex = 48;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(264, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 49;
            this.label1.Text = "Add User";
            // 
            // UserCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 344);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.CreateUserBtn);
            this.Controls.Add(this.FirebaseLabel);
            this.Controls.Add(this.FirebaseInput);
            this.Controls.Add(this.ProfilePictureLabel);
            this.Controls.Add(this.EmailLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.CompanyLabel);
            this.Controls.Add(this.LastNameLabel);
            this.Controls.Add(this.PhoneLabel);
            this.Controls.Add(this.ActiveLabel);
            this.Controls.Add(this.ProfilePictureApprovedLabel);
            this.Controls.Add(this.NotificationTypeLabel);
            this.Controls.Add(this.FirstNameLabel);
            this.Controls.Add(this.PictureApprovedInput);
            this.Controls.Add(this.LastNameInput);
            this.Controls.Add(this.CompanyInput);
            this.Controls.Add(this.ProfilePictureInput);
            this.Controls.Add(this.NotificationTypeInput);
            this.Controls.Add(this.StatusInput);
            this.Controls.Add(this.ActiveInput);
            this.Controls.Add(this.PasswordInput);
            this.Controls.Add(this.PhoneInput);
            this.Controls.Add(this.EmailInput);
            this.Controls.Add(this.FirstNameInput);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserCreationForm";
            this.Text = "UserCreationForm";
            this.Load += new System.EventHandler(this.UserCreationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProfilePictureLabel;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label CompanyLabel;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.Label PhoneLabel;
        private System.Windows.Forms.Label ActiveLabel;
        private System.Windows.Forms.Label ProfilePictureApprovedLabel;
        private System.Windows.Forms.Label NotificationTypeLabel;
        private System.Windows.Forms.Label FirstNameLabel;
        private System.Windows.Forms.TextBox PictureApprovedInput;
        private System.Windows.Forms.TextBox LastNameInput;
        private System.Windows.Forms.TextBox CompanyInput;
        private System.Windows.Forms.TextBox ProfilePictureInput;
        private System.Windows.Forms.TextBox NotificationTypeInput;
        private System.Windows.Forms.TextBox StatusInput;
        private System.Windows.Forms.TextBox ActiveInput;
        private System.Windows.Forms.TextBox PasswordInput;
        private System.Windows.Forms.TextBox PhoneInput;
        private System.Windows.Forms.TextBox EmailInput;
        private System.Windows.Forms.TextBox FirstNameInput;
        private System.Windows.Forms.Label FirebaseLabel;
        private System.Windows.Forms.TextBox FirebaseInput;
        private System.Windows.Forms.Button CreateUserBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label label1;
    }
}