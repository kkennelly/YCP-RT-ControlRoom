
namespace ControlRoomApplication.GUI
{
    partial class AppointmentControlForm
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
            this.AddUserBtn = new System.Windows.Forms.Button();
            this.AddApptBtn = new System.Windows.Forms.Button();
            this.ViewApptButton = new System.Windows.Forms.Button();
            this.ViewUserBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddUserBtn
            // 
            this.AddUserBtn.Location = new System.Drawing.Point(432, 94);
            this.AddUserBtn.Name = "AddUserBtn";
            this.AddUserBtn.Size = new System.Drawing.Size(304, 108);
            this.AddUserBtn.TabIndex = 0;
            this.AddUserBtn.Text = "Add User";
            this.AddUserBtn.UseVisualStyleBackColor = true;
            this.AddUserBtn.Click += new System.EventHandler(this.AddUserBtn_Click);
            // 
            // AddApptBtn
            // 
            this.AddApptBtn.Location = new System.Drawing.Point(69, 94);
            this.AddApptBtn.Name = "AddApptBtn";
            this.AddApptBtn.Size = new System.Drawing.Size(304, 108);
            this.AddApptBtn.TabIndex = 1;
            this.AddApptBtn.Text = "Add Appointment";
            this.AddApptBtn.UseVisualStyleBackColor = true;
            this.AddApptBtn.Click += new System.EventHandler(this.AddApptBtn_Click);
            // 
            // ViewApptButton
            // 
            this.ViewApptButton.Location = new System.Drawing.Point(69, 261);
            this.ViewApptButton.Name = "ViewApptButton";
            this.ViewApptButton.Size = new System.Drawing.Size(304, 108);
            this.ViewApptButton.TabIndex = 2;
            this.ViewApptButton.Text = "View Appointments";
            this.ViewApptButton.UseVisualStyleBackColor = true;
            // 
            // ViewUserBtn
            // 
            this.ViewUserBtn.Location = new System.Drawing.Point(432, 261);
            this.ViewUserBtn.Name = "ViewUserBtn";
            this.ViewUserBtn.Size = new System.Drawing.Size(304, 108);
            this.ViewUserBtn.TabIndex = 3;
            this.ViewUserBtn.Text = "View Users";
            this.ViewUserBtn.UseVisualStyleBackColor = true;
            // 
            // AppointmentControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ViewUserBtn);
            this.Controls.Add(this.ViewApptButton);
            this.Controls.Add(this.AddApptBtn);
            this.Controls.Add(this.AddUserBtn);
            this.Name = "AppointmentControlForm";
            this.Text = "AppointmentControlForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddUserBtn;
        private System.Windows.Forms.Button AddApptBtn;
        private System.Windows.Forms.Button ViewApptButton;
        private System.Windows.Forms.Button ViewUserBtn;
    }
}