﻿
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
            this.StatusInput = new System.Windows.Forms.TextBox();
            this.CelestialBodyIdInput = new System.Windows.Forms.TextBox();
            this.OrientationIdInput = new System.Windows.Forms.TextBox();
            this.TelescopeIdInput = new System.Windows.Forms.TextBox();
            this.AddApptBtn = new System.Windows.Forms.Button();
            this.UserLabel = new System.Windows.Forms.Label();
            this.TelescopeIDLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.EndTimeLabel = new System.Windows.Forms.Label();
            this.OrientationLabel = new System.Windows.Forms.Label();
            this.SpectraCyberConfigLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.CelestialBodyLabel = new System.Windows.Forms.Label();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.PriorityLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.StartTimeInput = new System.Windows.Forms.DateTimePicker();
            this.EndTimeInput = new System.Windows.Forms.DateTimePicker();
            this.UsernameInputList = new System.Windows.Forms.ComboBox();
            this.PublicInput = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PriorityInputList = new System.Windows.Forms.ComboBox();
            this.TypeInputList = new System.Windows.Forms.ComboBox();
            this.SpectraCyberConfigInputList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // StatusInput
            // 
            this.StatusInput.Enabled = false;
            this.StatusInput.Location = new System.Drawing.Point(92, 166);
            this.StatusInput.Margin = new System.Windows.Forms.Padding(2);
            this.StatusInput.Name = "StatusInput";
            this.StatusInput.Size = new System.Drawing.Size(76, 20);
            this.StatusInput.TabIndex = 4;
            // 
            // CelestialBodyIdInput
            // 
            this.CelestialBodyIdInput.Location = new System.Drawing.Point(433, 162);
            this.CelestialBodyIdInput.Margin = new System.Windows.Forms.Padding(2);
            this.CelestialBodyIdInput.Name = "CelestialBodyIdInput";
            this.CelestialBodyIdInput.Size = new System.Drawing.Size(102, 20);
            this.CelestialBodyIdInput.TabIndex = 5;
            // 
            // OrientationIdInput
            // 
            this.OrientationIdInput.Location = new System.Drawing.Point(433, 62);
            this.OrientationIdInput.Margin = new System.Windows.Forms.Padding(2);
            this.OrientationIdInput.Name = "OrientationIdInput";
            this.OrientationIdInput.Size = new System.Drawing.Size(103, 20);
            this.OrientationIdInput.TabIndex = 9;
            // 
            // TelescopeIdInput
            // 
            this.TelescopeIdInput.Enabled = false;
            this.TelescopeIdInput.Location = new System.Drawing.Point(92, 203);
            this.TelescopeIdInput.Margin = new System.Windows.Forms.Padding(2);
            this.TelescopeIdInput.Name = "TelescopeIdInput";
            this.TelescopeIdInput.Size = new System.Drawing.Size(76, 20);
            this.TelescopeIdInput.TabIndex = 10;
            // 
            // AddApptBtn
            // 
            this.AddApptBtn.Location = new System.Drawing.Point(300, 277);
            this.AddApptBtn.Margin = new System.Windows.Forms.Padding(2);
            this.AddApptBtn.Name = "AddApptBtn";
            this.AddApptBtn.Size = new System.Drawing.Size(110, 23);
            this.AddApptBtn.TabIndex = 11;
            this.AddApptBtn.Text = "Add Appointment";
            this.AddApptBtn.UseVisualStyleBackColor = true;
            this.AddApptBtn.Click += new System.EventHandler(this.AddApptBtn_Click);
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Location = new System.Drawing.Point(58, 62);
            this.UserLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(29, 13);
            this.UserLabel.TabIndex = 12;
            this.UserLabel.Text = "User";
            // 
            // TelescopeIDLabel
            // 
            this.TelescopeIDLabel.AutoSize = true;
            this.TelescopeIDLabel.Location = new System.Drawing.Point(17, 206);
            this.TelescopeIDLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TelescopeIDLabel.Name = "TelescopeIDLabel";
            this.TelescopeIDLabel.Size = new System.Drawing.Size(71, 13);
            this.TelescopeIDLabel.TabIndex = 14;
            this.TelescopeIDLabel.Text = "Telescope ID";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(51, 169);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.StatusLabel.TabIndex = 15;
            this.StatusLabel.Text = "Status";
            // 
            // EndTimeLabel
            // 
            this.EndTimeLabel.AutoSize = true;
            this.EndTimeLabel.Location = new System.Drawing.Point(36, 130);
            this.EndTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EndTimeLabel.Name = "EndTimeLabel";
            this.EndTimeLabel.Size = new System.Drawing.Size(52, 13);
            this.EndTimeLabel.TabIndex = 16;
            this.EndTimeLabel.Text = "End Time";
            // 
            // OrientationLabel
            // 
            this.OrientationLabel.AutoSize = true;
            this.OrientationLabel.Location = new System.Drawing.Point(357, 62);
            this.OrientationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OrientationLabel.Name = "OrientationLabel";
            this.OrientationLabel.Size = new System.Drawing.Size(72, 13);
            this.OrientationLabel.TabIndex = 17;
            this.OrientationLabel.Text = "Orientation ID";
            // 
            // SpectraCyberConfigLabel
            // 
            this.SpectraCyberConfigLabel.AutoSize = true;
            this.SpectraCyberConfigLabel.Location = new System.Drawing.Point(311, 95);
            this.SpectraCyberConfigLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpectraCyberConfigLabel.Name = "SpectraCyberConfigLabel";
            this.SpectraCyberConfigLabel.Size = new System.Drawing.Size(118, 13);
            this.SpectraCyberConfigLabel.TabIndex = 18;
            this.SpectraCyberConfigLabel.Text = "SpectraCyber Config ID";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(398, 130);
            this.TypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(31, 13);
            this.TypeLabel.TabIndex = 19;
            this.TypeLabel.Text = "Type";
            // 
            // CelestialBodyLabel
            // 
            this.CelestialBodyLabel.AutoSize = true;
            this.CelestialBodyLabel.Location = new System.Drawing.Point(342, 166);
            this.CelestialBodyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CelestialBodyLabel.Name = "CelestialBodyLabel";
            this.CelestialBodyLabel.Size = new System.Drawing.Size(87, 13);
            this.CelestialBodyLabel.TabIndex = 20;
            this.CelestialBodyLabel.Text = "Celestial Body ID";
            // 
            // StartTimeLabel
            // 
            this.StartTimeLabel.AutoSize = true;
            this.StartTimeLabel.Location = new System.Drawing.Point(33, 95);
            this.StartTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StartTimeLabel.Name = "StartTimeLabel";
            this.StartTimeLabel.Size = new System.Drawing.Size(55, 13);
            this.StartTimeLabel.TabIndex = 21;
            this.StartTimeLabel.Text = "Start Time";
            // 
            // PriorityLabel
            // 
            this.PriorityLabel.AutoSize = true;
            this.PriorityLabel.Location = new System.Drawing.Point(391, 203);
            this.PriorityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PriorityLabel.Name = "PriorityLabel";
            this.PriorityLabel.Size = new System.Drawing.Size(38, 13);
            this.PriorityLabel.TabIndex = 22;
            this.PriorityLabel.Text = "Priority";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(182, 277);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(110, 23);
            this.CancelBtn.TabIndex = 23;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // StartTimeInput
            // 
            this.StartTimeInput.Location = new System.Drawing.Point(92, 95);
            this.StartTimeInput.Name = "StartTimeInput";
            this.StartTimeInput.Size = new System.Drawing.Size(200, 20);
            this.StartTimeInput.TabIndex = 24;
            // 
            // EndTimeInput
            // 
            this.EndTimeInput.Location = new System.Drawing.Point(92, 130);
            this.EndTimeInput.Name = "EndTimeInput";
            this.EndTimeInput.Size = new System.Drawing.Size(200, 20);
            this.EndTimeInput.TabIndex = 25;
            // 
            // UsernameInputList
            // 
            this.UsernameInputList.FormattingEnabled = true;
            this.UsernameInputList.Location = new System.Drawing.Point(93, 59);
            this.UsernameInputList.Name = "UsernameInputList";
            this.UsernameInputList.Size = new System.Drawing.Size(148, 21);
            this.UsernameInputList.TabIndex = 26;
            // 
            // PublicInput
            // 
            this.PublicInput.AutoSize = true;
            this.PublicInput.Location = new System.Drawing.Point(260, 242);
            this.PublicInput.Name = "PublicInput";
            this.PublicInput.Size = new System.Drawing.Size(55, 17);
            this.PublicInput.TabIndex = 27;
            this.PublicInput.Text = "Public";
            this.PublicInput.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(229, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 20);
            this.label1.TabIndex = 28;
            this.label1.Text = "Add Appointment";
            // 
            // PriorityInputList
            // 
            this.PriorityInputList.FormattingEnabled = true;
            this.PriorityInputList.Location = new System.Drawing.Point(434, 198);
            this.PriorityInputList.Name = "PriorityInputList";
            this.PriorityInputList.Size = new System.Drawing.Size(101, 21);
            this.PriorityInputList.TabIndex = 29;
            // 
            // TypeInputList
            // 
            this.TypeInputList.FormattingEnabled = true;
            this.TypeInputList.Location = new System.Drawing.Point(433, 127);
            this.TypeInputList.Name = "TypeInputList";
            this.TypeInputList.Size = new System.Drawing.Size(102, 21);
            this.TypeInputList.TabIndex = 30;
            // 
            // SpectraCyberConfigInputList
            // 
            this.SpectraCyberConfigInputList.FormattingEnabled = true;
            this.SpectraCyberConfigInputList.Location = new System.Drawing.Point(433, 92);
            this.SpectraCyberConfigInputList.Name = "SpectraCyberConfigInputList";
            this.SpectraCyberConfigInputList.Size = new System.Drawing.Size(103, 21);
            this.SpectraCyberConfigInputList.TabIndex = 31;
            // 
            // AppointmentCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 321);
            this.Controls.Add(this.SpectraCyberConfigInputList);
            this.Controls.Add(this.TypeInputList);
            this.Controls.Add(this.PriorityInputList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PublicInput);
            this.Controls.Add(this.UsernameInputList);
            this.Controls.Add(this.EndTimeInput);
            this.Controls.Add(this.StartTimeInput);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.PriorityLabel);
            this.Controls.Add(this.StartTimeLabel);
            this.Controls.Add(this.CelestialBodyLabel);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.SpectraCyberConfigLabel);
            this.Controls.Add(this.OrientationLabel);
            this.Controls.Add(this.EndTimeLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.TelescopeIDLabel);
            this.Controls.Add(this.UserLabel);
            this.Controls.Add(this.AddApptBtn);
            this.Controls.Add(this.TelescopeIdInput);
            this.Controls.Add(this.OrientationIdInput);
            this.Controls.Add(this.CelestialBodyIdInput);
            this.Controls.Add(this.StatusInput);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AppointmentCreationForm";
            this.Text = "AppointmentCreationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox StatusInput;
        private System.Windows.Forms.TextBox CelestialBodyIdInput;
        private System.Windows.Forms.TextBox OrientationIdInput;
        private System.Windows.Forms.TextBox TelescopeIdInput;
        private System.Windows.Forms.Button AddApptBtn;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.Label TelescopeIDLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label EndTimeLabel;
        private System.Windows.Forms.Label OrientationLabel;
        private System.Windows.Forms.Label SpectraCyberConfigLabel;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Label CelestialBodyLabel;
        private System.Windows.Forms.Label StartTimeLabel;
        private System.Windows.Forms.Label PriorityLabel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.DateTimePicker StartTimeInput;
        private System.Windows.Forms.DateTimePicker EndTimeInput;
        private System.Windows.Forms.ComboBox UsernameInputList;
        private System.Windows.Forms.CheckBox PublicInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox PriorityInputList;
        private System.Windows.Forms.ComboBox TypeInputList;
        private System.Windows.Forms.ComboBox SpectraCyberConfigInputList;
    }
}