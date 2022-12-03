namespace ControlRoomApplication.GUI
{
    partial class SpectraCyberConfigCreationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ModeInputList = new System.Windows.Forms.ComboBox();
            this.OffsetVoltageInput = new System.Windows.Forms.TextBox();
            this.IFGainInput = new System.Windows.Forms.TextBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.IntegrationTimeInputList = new System.Windows.Forms.ComboBox();
            this.DCGainInputList = new System.Windows.Forms.ComboBox();
            this.BandwidthInputList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Spectracyber Configuration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Integration Time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Offset Voltage";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "IF Gain";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "DC Gain";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(91, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Bandwidth";
            // 
            // ModeInputList
            // 
            this.ModeInputList.FormattingEnabled = true;
            this.ModeInputList.Location = new System.Drawing.Point(154, 58);
            this.ModeInputList.Name = "ModeInputList";
            this.ModeInputList.Size = new System.Drawing.Size(100, 21);
            this.ModeInputList.TabIndex = 7;
            // 
            // OffsetVoltageInput
            // 
            this.OffsetVoltageInput.Location = new System.Drawing.Point(154, 122);
            this.OffsetVoltageInput.Name = "OffsetVoltageInput";
            this.OffsetVoltageInput.Size = new System.Drawing.Size(100, 20);
            this.OffsetVoltageInput.TabIndex = 9;
            // 
            // IFGainInput
            // 
            this.IFGainInput.Location = new System.Drawing.Point(154, 154);
            this.IFGainInput.Name = "IFGainInput";
            this.IFGainInput.Size = new System.Drawing.Size(100, 20);
            this.IFGainInput.TabIndex = 10;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(191, 254);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 13;
            this.ButtonOK.Text = "Add";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(110, 254);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 14;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // IntegrationTimeInputList
            // 
            this.IntegrationTimeInputList.FormattingEnabled = true;
            this.IntegrationTimeInputList.Location = new System.Drawing.Point(154, 90);
            this.IntegrationTimeInputList.Name = "IntegrationTimeInputList";
            this.IntegrationTimeInputList.Size = new System.Drawing.Size(100, 21);
            this.IntegrationTimeInputList.TabIndex = 15;
            // 
            // DCGainInputList
            // 
            this.DCGainInputList.FormattingEnabled = true;
            this.DCGainInputList.Location = new System.Drawing.Point(154, 184);
            this.DCGainInputList.Name = "DCGainInputList";
            this.DCGainInputList.Size = new System.Drawing.Size(100, 21);
            this.DCGainInputList.TabIndex = 16;
            // 
            // BandwidthInputList
            // 
            this.BandwidthInputList.FormattingEnabled = true;
            this.BandwidthInputList.Location = new System.Drawing.Point(154, 211);
            this.BandwidthInputList.Name = "BandwidthInputList";
            this.BandwidthInputList.Size = new System.Drawing.Size(100, 21);
            this.BandwidthInputList.TabIndex = 17;
            // 
            // SpectraCyberConfigCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 289);
            this.Controls.Add(this.BandwidthInputList);
            this.Controls.Add(this.DCGainInputList);
            this.Controls.Add(this.IntegrationTimeInputList);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.IFGainInput);
            this.Controls.Add(this.OffsetVoltageInput);
            this.Controls.Add(this.ModeInputList);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SpectraCyberConfigCreationForm";
            this.Text = "SpectraCyberConfigCreationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ModeInputList;
        private System.Windows.Forms.TextBox OffsetVoltageInput;
        private System.Windows.Forms.TextBox IFGainInput;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.ComboBox IntegrationTimeInputList;
        private System.Windows.Forms.ComboBox DCGainInputList;
        private System.Windows.Forms.ComboBox BandwidthInputList;
    }
}