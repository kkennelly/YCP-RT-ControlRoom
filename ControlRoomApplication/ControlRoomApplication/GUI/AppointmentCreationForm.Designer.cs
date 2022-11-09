
namespace ControlRoomApplication.GUI
{
    partial class AppointmentCreationForm
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
            this.UserIdInput = new System.Windows.Forms.TextBox();
            this.StartTimeInput = new System.Windows.Forms.TextBox();
            this.EndTimeInput = new System.Windows.Forms.TextBox();
            this.TypeInput = new System.Windows.Forms.TextBox();
            this.StatusInput = new System.Windows.Forms.TextBox();
            this.CelestialBodyIdInput = new System.Windows.Forms.TextBox();
            this.PublicInput = new System.Windows.Forms.TextBox();
            this.PriorityInput = new System.Windows.Forms.TextBox();
            this.SpectraCyberConfigIdInput = new System.Windows.Forms.TextBox();
            this.OrientationIdInput = new System.Windows.Forms.TextBox();
            this.TelescopeIdInput = new System.Windows.Forms.TextBox();
            this.AddApptBtn = new System.Windows.Forms.Button();
            this.UserLabel = new System.Windows.Forms.Label();
            this.PublicLabel = new System.Windows.Forms.Label();
            this.TelescopeIDLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.EndTimeLabel = new System.Windows.Forms.Label();
            this.OrientationLabel = new System.Windows.Forms.Label();
            this.SpectraCyberConfigLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.CelestialBodyLabel = new System.Windows.Forms.Label();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.PriorityLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UserIdInput
            // 
            this.UserIdInput.Location = new System.Drawing.Point(160, 37);
            this.UserIdInput.Name = "UserIdInput";
            this.UserIdInput.Size = new System.Drawing.Size(100, 22);
            this.UserIdInput.TabIndex = 0;
            // 
            // StartTimeInput
            // 
            this.StartTimeInput.Location = new System.Drawing.Point(160, 80);
            this.StartTimeInput.Name = "StartTimeInput";
            this.StartTimeInput.Size = new System.Drawing.Size(100, 22);
            this.StartTimeInput.TabIndex = 1;
            // 
            // EndTimeInput
            // 
            this.EndTimeInput.Location = new System.Drawing.Point(160, 123);
            this.EndTimeInput.Name = "EndTimeInput";
            this.EndTimeInput.Size = new System.Drawing.Size(100, 22);
            this.EndTimeInput.TabIndex = 2;
            // 
            // TypeInput
            // 
            this.TypeInput.Location = new System.Drawing.Point(509, 123);
            this.TypeInput.Name = "TypeInput";
            this.TypeInput.Size = new System.Drawing.Size(100, 22);
            this.TypeInput.TabIndex = 3;
            // 
            // StatusInput
            // 
            this.StatusInput.Location = new System.Drawing.Point(160, 171);
            this.StatusInput.Name = "StatusInput";
            this.StatusInput.Size = new System.Drawing.Size(100, 22);
            this.StatusInput.TabIndex = 4;
            // 
            // CelestialBodyIdInput
            // 
            this.CelestialBodyIdInput.Location = new System.Drawing.Point(509, 171);
            this.CelestialBodyIdInput.Name = "CelestialBodyIdInput";
            this.CelestialBodyIdInput.Size = new System.Drawing.Size(100, 22);
            this.CelestialBodyIdInput.TabIndex = 5;
            // 
            // PublicInput
            // 
            this.PublicInput.Location = new System.Drawing.Point(160, 261);
            this.PublicInput.Name = "PublicInput";
            this.PublicInput.Size = new System.Drawing.Size(100, 22);
            this.PublicInput.TabIndex = 6;
            // 
            // PriorityInput
            // 
            this.PriorityInput.Location = new System.Drawing.Point(509, 217);
            this.PriorityInput.Name = "PriorityInput";
            this.PriorityInput.Size = new System.Drawing.Size(100, 22);
            this.PriorityInput.TabIndex = 7;
            // 
            // SpectraCyberConfigIdInput
            // 
            this.SpectraCyberConfigIdInput.Location = new System.Drawing.Point(509, 80);
            this.SpectraCyberConfigIdInput.Name = "SpectraCyberConfigIdInput";
            this.SpectraCyberConfigIdInput.Size = new System.Drawing.Size(100, 22);
            this.SpectraCyberConfigIdInput.TabIndex = 8;
            // 
            // OrientationIdInput
            // 
            this.OrientationIdInput.Location = new System.Drawing.Point(509, 37);
            this.OrientationIdInput.Name = "OrientationIdInput";
            this.OrientationIdInput.Size = new System.Drawing.Size(100, 22);
            this.OrientationIdInput.TabIndex = 9;
            // 
            // TelescopeIdInput
            // 
            this.TelescopeIdInput.Location = new System.Drawing.Point(160, 217);
            this.TelescopeIdInput.Name = "TelescopeIdInput";
            this.TelescopeIdInput.Size = new System.Drawing.Size(100, 22);
            this.TelescopeIdInput.TabIndex = 10;
            // 
            // AddApptBtn
            // 
            this.AddApptBtn.Location = new System.Drawing.Point(285, 344);
            this.AddApptBtn.Name = "AddApptBtn";
            this.AddApptBtn.Size = new System.Drawing.Size(191, 61);
            this.AddApptBtn.TabIndex = 11;
            this.AddApptBtn.Text = "Add Appointment";
            this.AddApptBtn.UseVisualStyleBackColor = true;
            this.AddApptBtn.Click += new System.EventHandler(this.AddApptBtn_Click);
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Location = new System.Drawing.Point(99, 40);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(55, 17);
            this.UserLabel.TabIndex = 12;
            this.UserLabel.Text = "User ID";
            // 
            // PublicLabel
            // 
            this.PublicLabel.AutoSize = true;
            this.PublicLabel.Location = new System.Drawing.Point(106, 264);
            this.PublicLabel.Name = "PublicLabel";
            this.PublicLabel.Size = new System.Drawing.Size(46, 17);
            this.PublicLabel.TabIndex = 13;
            this.PublicLabel.Text = "Public";
            // 
            // TelescopeIDLabel
            // 
            this.TelescopeIDLabel.AutoSize = true;
            this.TelescopeIDLabel.Location = new System.Drawing.Point(63, 220);
            this.TelescopeIDLabel.Name = "TelescopeIDLabel";
            this.TelescopeIDLabel.Size = new System.Drawing.Size(91, 17);
            this.TelescopeIDLabel.TabIndex = 14;
            this.TelescopeIDLabel.Text = "Telescope ID";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(106, 174);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(48, 17);
            this.StatusLabel.TabIndex = 15;
            this.StatusLabel.Text = "Status";
            // 
            // EndTimeLabel
            // 
            this.EndTimeLabel.AutoSize = true;
            this.EndTimeLabel.Location = new System.Drawing.Point(86, 126);
            this.EndTimeLabel.Name = "EndTimeLabel";
            this.EndTimeLabel.Size = new System.Drawing.Size(68, 17);
            this.EndTimeLabel.TabIndex = 16;
            this.EndTimeLabel.Text = "End Time";
            // 
            // OrientationLabel
            // 
            this.OrientationLabel.AutoSize = true;
            this.OrientationLabel.Location = new System.Drawing.Point(408, 40);
            this.OrientationLabel.Name = "OrientationLabel";
            this.OrientationLabel.Size = new System.Drawing.Size(95, 17);
            this.OrientationLabel.TabIndex = 17;
            this.OrientationLabel.Text = "Orientation ID";
            // 
            // SpectraCyberConfigLabel
            // 
            this.SpectraCyberConfigLabel.AutoSize = true;
            this.SpectraCyberConfigLabel.Location = new System.Drawing.Point(348, 83);
            this.SpectraCyberConfigLabel.Name = "SpectraCyberConfigLabel";
            this.SpectraCyberConfigLabel.Size = new System.Drawing.Size(155, 17);
            this.SpectraCyberConfigLabel.TabIndex = 18;
            this.SpectraCyberConfigLabel.Text = "SpectraCyber Config ID";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(463, 126);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(40, 17);
            this.TypeLabel.TabIndex = 19;
            this.TypeLabel.Text = "Type";
            // 
            // CelestialBodyLabel
            // 
            this.CelestialBodyLabel.AutoSize = true;
            this.CelestialBodyLabel.Location = new System.Drawing.Point(389, 174);
            this.CelestialBodyLabel.Name = "CelestialBodyLabel";
            this.CelestialBodyLabel.Size = new System.Drawing.Size(114, 17);
            this.CelestialBodyLabel.TabIndex = 20;
            this.CelestialBodyLabel.Text = "Celestial Body ID";
            // 
            // StartTimeLabel
            // 
            this.StartTimeLabel.AutoSize = true;
            this.StartTimeLabel.Location = new System.Drawing.Point(81, 83);
            this.StartTimeLabel.Name = "StartTimeLabel";
            this.StartTimeLabel.Size = new System.Drawing.Size(73, 17);
            this.StartTimeLabel.TabIndex = 21;
            this.StartTimeLabel.Text = "Start Time";
            // 
            // PriorityLabel
            // 
            this.PriorityLabel.AutoSize = true;
            this.PriorityLabel.Location = new System.Drawing.Point(451, 220);
            this.PriorityLabel.Name = "PriorityLabel";
            this.PriorityLabel.Size = new System.Drawing.Size(52, 17);
            this.PriorityLabel.TabIndex = 22;
            this.PriorityLabel.Text = "Priority";
            // 
            // AppointmentCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PriorityLabel);
            this.Controls.Add(this.StartTimeLabel);
            this.Controls.Add(this.CelestialBodyLabel);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.SpectraCyberConfigLabel);
            this.Controls.Add(this.OrientationLabel);
            this.Controls.Add(this.EndTimeLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.TelescopeIDLabel);
            this.Controls.Add(this.PublicLabel);
            this.Controls.Add(this.UserLabel);
            this.Controls.Add(this.AddApptBtn);
            this.Controls.Add(this.TelescopeIdInput);
            this.Controls.Add(this.OrientationIdInput);
            this.Controls.Add(this.SpectraCyberConfigIdInput);
            this.Controls.Add(this.PriorityInput);
            this.Controls.Add(this.PublicInput);
            this.Controls.Add(this.CelestialBodyIdInput);
            this.Controls.Add(this.StatusInput);
            this.Controls.Add(this.TypeInput);
            this.Controls.Add(this.EndTimeInput);
            this.Controls.Add(this.StartTimeInput);
            this.Controls.Add(this.UserIdInput);
            this.Name = "AppointmentCreationForm";
            this.Text = "AppointmentCreationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UserIdInput;
        private System.Windows.Forms.TextBox StartTimeInput;
        private System.Windows.Forms.TextBox EndTimeInput;
        private System.Windows.Forms.TextBox TypeInput;
        private System.Windows.Forms.TextBox StatusInput;
        private System.Windows.Forms.TextBox CelestialBodyIdInput;
        private System.Windows.Forms.TextBox PublicInput;
        private System.Windows.Forms.TextBox PriorityInput;
        private System.Windows.Forms.TextBox SpectraCyberConfigIdInput;
        private System.Windows.Forms.TextBox OrientationIdInput;
        private System.Windows.Forms.TextBox TelescopeIdInput;
        private System.Windows.Forms.Button AddApptBtn;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.Label PublicLabel;
        private System.Windows.Forms.Label TelescopeIDLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label EndTimeLabel;
        private System.Windows.Forms.Label OrientationLabel;
        private System.Windows.Forms.Label SpectraCyberConfigLabel;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Label CelestialBodyLabel;
        private System.Windows.Forms.Label StartTimeLabel;
        private System.Windows.Forms.Label PriorityLabel;
    }
}