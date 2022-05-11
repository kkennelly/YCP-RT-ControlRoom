namespace ControlRoomApplication.GUI
{
    partial class CustomOrientationInputDialog
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
            this.invalidInputLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // invalidInputLabel
            // 
            this.invalidInputLabel.AutoSize = true;
            this.invalidInputLabel.ForeColor = System.Drawing.Color.Red;
            this.invalidInputLabel.Location = new System.Drawing.Point(139, 141);
            this.invalidInputLabel.Name = "invalidInputLabel";
            this.invalidInputLabel.Size = new System.Drawing.Size(64, 13);
            this.invalidInputLabel.TabIndex = 4;
            this.invalidInputLabel.Text = "Invalid input";
            // 
            // CustomOrientationInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 237);
            this.Controls.Add(this.invalidInputLabel);
            this.Name = "CustomOrientationInputDialog";
            this.Text = "AzimuthInputDialog";
            this.Controls.SetChildIndex(this.promptLabel, 0);
            this.Controls.SetChildIndex(this.textBox, 0);
            this.Controls.SetChildIndex(this.okButton, 0);
            this.Controls.SetChildIndex(this.invalidInputLabel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label invalidInputLabel;
    }
}