﻿namespace ControlRoomApplication.GUI
{
    partial class DiagnosticsForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblCurrentAzOrientation = new System.Windows.Forms.Label();
            this.lblCurrentElOrientation = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.startTimeTextBox = new System.Windows.Forms.TextBox();
            this.endTimeTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblAzimuthTemp = new System.Windows.Forms.Label();
            this.lblElevationTemp = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.fldAzTemp = new System.Windows.Forms.Label();
            this.fldElTemp = new System.Windows.Forms.Label();
            this.txtTemperature = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.warningLabel = new System.Windows.Forms.Label();
            this.fanLabel = new System.Windows.Forms.Label();
            this.btnAddOneTemp = new System.Windows.Forms.Button();
            this.btnAddFiveTemp = new System.Windows.Forms.Button();
            this.btnAddXTemp = new System.Windows.Forms.Button();
            this.lblShutdown = new System.Windows.Forms.Label();
            this.selectDemo = new System.Windows.Forms.CheckBox();
            this.btnSubtractOneTemp = new System.Windows.Forms.Button();
            this.btnSubtractFiveTemp = new System.Windows.Forms.Button();
            this.btnSubtractXTemp = new System.Windows.Forms.Button();
            this.txtCustTemp = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblElLimStatus2 = new System.Windows.Forms.Label();
            this.lblElLimStatus1 = new System.Windows.Forms.Label();
            this.lblAzLimStatus2 = new System.Windows.Forms.Label();
            this.lblAzLimStatus1 = new System.Windows.Forms.Label();
            this.lblElLimit2 = new System.Windows.Forms.Label();
            this.lblElLimit1 = new System.Windows.Forms.Label();
            this.lblAzLimit2 = new System.Windows.Forms.Label();
            this.lblAzLimit1 = new System.Windows.Forms.Label();
            this.lblEleProx2 = new System.Windows.Forms.Label();
            this.lblEleProx1 = new System.Windows.Forms.Label();
            this.lblElProx2 = new System.Windows.Forms.Label();
            this.lblElProx1 = new System.Windows.Forms.Label();
            this.lblAzProxStatus3 = new System.Windows.Forms.Label();
            this.lblAzProxStatus2 = new System.Windows.Forms.Label();
            this.lblAzProxStatus1 = new System.Windows.Forms.Label();
            this.lblAzProx3 = new System.Windows.Forms.Label();
            this.lblAzProx2 = new System.Windows.Forms.Label();
            this.lblAzProx1 = new System.Windows.Forms.Label();
            this.lblAbsEncoder = new System.Windows.Forms.Label();
            this.lblEncoderDegrees = new System.Windows.Forms.Label();
            this.lblAzEncoderDegrees = new System.Windows.Forms.Label();
            this.lblEncoderTicks = new System.Windows.Forms.Label();
            this.lblAzEncoderTicks = new System.Windows.Forms.Label();
            this.btnAddOneEncoder = new System.Windows.Forms.Button();
            this.btnAddFiveEncoder = new System.Windows.Forms.Button();
            this.btnAddXEncoder = new System.Windows.Forms.Button();
            this.btnSubtractOneEncoder = new System.Windows.Forms.Button();
            this.btnSubtractFiveEncoder = new System.Windows.Forms.Button();
            this.btnSubtractXEncoder = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCustEncoderVal = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.lblElEncoderTicks = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblElEncoderDegrees = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(9, 10);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(290, 178);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // lblCurrentAzOrientation
            // 
            this.lblCurrentAzOrientation.AutoSize = true;
            this.lblCurrentAzOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAzOrientation.Location = new System.Drawing.Point(319, 19);
            this.lblCurrentAzOrientation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentAzOrientation.Name = "lblCurrentAzOrientation";
            this.lblCurrentAzOrientation.Size = new System.Drawing.Size(132, 20);
            this.lblCurrentAzOrientation.TabIndex = 1;
            this.lblCurrentAzOrientation.Text = "Current Azimuth: ";
            // 
            // lblCurrentElOrientation
            // 
            this.lblCurrentElOrientation.AutoSize = true;
            this.lblCurrentElOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentElOrientation.Location = new System.Drawing.Point(319, 46);
            this.lblCurrentElOrientation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentElOrientation.Name = "lblCurrentElOrientation";
            this.lblCurrentElOrientation.Size = new System.Drawing.Size(139, 20);
            this.lblCurrentElOrientation.TabIndex = 2;
            this.lblCurrentElOrientation.Text = "Current Elevation: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(466, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(466, 46);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 211);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Current Appointment";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 243);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Start Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 296);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "End Time";
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(9, 262);
            this.startTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.startTimeTextBox.TabIndex = 8;
            // 
            // endTimeTextBox
            // 
            this.endTimeTextBox.Location = new System.Drawing.Point(9, 314);
            this.endTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.endTimeTextBox.Name = "endTimeTextBox";
            this.endTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.endTimeTextBox.TabIndex = 9;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(116, 262);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(102, 20);
            this.statusTextBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(112, 242);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 11;
            this.label8.Text = "Status";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblAzimuthTemp
            // 
            this.lblAzimuthTemp.AutoSize = true;
            this.lblAzimuthTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzimuthTemp.Location = new System.Drawing.Point(529, 19);
            this.lblAzimuthTemp.Name = "lblAzimuthTemp";
            this.lblAzimuthTemp.Size = new System.Drawing.Size(115, 20);
            this.lblAzimuthTemp.TabIndex = 14;
            this.lblAzimuthTemp.Text = "Azimuth Temp:";
            // 
            // lblElevationTemp
            // 
            this.lblElevationTemp.AutoSize = true;
            this.lblElevationTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElevationTemp.Location = new System.Drawing.Point(522, 49);
            this.lblElevationTemp.Name = "lblElevationTemp";
            this.lblElevationTemp.Size = new System.Drawing.Size(122, 20);
            this.lblElevationTemp.TabIndex = 15;
            this.lblElevationTemp.Text = "Elevation Temp:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(622, 89);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(59, 17);
            this.radioButton1.TabIndex = 16;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Celcius";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(526, 89);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(75, 17);
            this.radioButton2.TabIndex = 17;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Fahrenheit";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // fldAzTemp
            // 
            this.fldAzTemp.AutoSize = true;
            this.fldAzTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAzTemp.Location = new System.Drawing.Point(650, 19);
            this.fldAzTemp.Name = "fldAzTemp";
            this.fldAzTemp.Size = new System.Drawing.Size(31, 20);
            this.fldAzTemp.TabIndex = 18;
            this.fldAzTemp.Text = "0.0";
            // 
            // fldElTemp
            // 
            this.fldElTemp.AutoSize = true;
            this.fldElTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldElTemp.Location = new System.Drawing.Point(650, 49);
            this.fldElTemp.Name = "fldElTemp";
            this.fldElTemp.Size = new System.Drawing.Size(31, 20);
            this.fldElTemp.TabIndex = 19;
            this.fldElTemp.Text = "0.0";
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(833, 23);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(39, 20);
            this.txtTemperature.TabIndex = 20;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(823, 53);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(59, 34);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "Test Button";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.Color.Chartreuse;
            this.warningLabel.Location = new System.Drawing.Point(706, 46);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(100, 16);
            this.warningLabel.TabIndex = 22;
            this.warningLabel.Text = "warningLabel";
            this.warningLabel.Visible = false;
            // 
            // fanLabel
            // 
            this.fanLabel.AutoSize = true;
            this.fanLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fanLabel.ForeColor = System.Drawing.Color.Chartreuse;
            this.fanLabel.Location = new System.Drawing.Point(717, 23);
            this.fanLabel.Name = "fanLabel";
            this.fanLabel.Size = new System.Drawing.Size(68, 16);
            this.fanLabel.TabIndex = 23;
            this.fanLabel.Text = "fanLabel";
            this.fanLabel.Visible = false;
            // 
            // btnAddOneTemp
            // 
            this.btnAddOneTemp.Location = new System.Drawing.Point(521, 117);
            this.btnAddOneTemp.Name = "btnAddOneTemp";
            this.btnAddOneTemp.Size = new System.Drawing.Size(35, 35);
            this.btnAddOneTemp.TabIndex = 24;
            this.btnAddOneTemp.Text = "+1";
            this.btnAddOneTemp.UseVisualStyleBackColor = true;
            this.btnAddOneTemp.Click += new System.EventHandler(this.btnAddOneTemp_Click);
            // 
            // btnAddFiveTemp
            // 
            this.btnAddFiveTemp.Location = new System.Drawing.Point(574, 117);
            this.btnAddFiveTemp.Name = "btnAddFiveTemp";
            this.btnAddFiveTemp.Size = new System.Drawing.Size(35, 35);
            this.btnAddFiveTemp.TabIndex = 25;
            this.btnAddFiveTemp.Text = "+5";
            this.btnAddFiveTemp.UseVisualStyleBackColor = true;
            this.btnAddFiveTemp.Click += new System.EventHandler(this.btnAddFiveTemp_Click);
            // 
            // btnAddXTemp
            // 
            this.btnAddXTemp.Location = new System.Drawing.Point(622, 117);
            this.btnAddXTemp.Name = "btnAddXTemp";
            this.btnAddXTemp.Size = new System.Drawing.Size(35, 35);
            this.btnAddXTemp.TabIndex = 26;
            this.btnAddXTemp.Text = "+X";
            this.btnAddXTemp.UseVisualStyleBackColor = true;
            this.btnAddXTemp.Click += new System.EventHandler(this.btnAddXTemp_Click);
            // 
            // lblShutdown
            // 
            this.lblShutdown.AutoSize = true;
            this.lblShutdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShutdown.ForeColor = System.Drawing.Color.Chartreuse;
            this.lblShutdown.Location = new System.Drawing.Point(706, 71);
            this.lblShutdown.Name = "lblShutdown";
            this.lblShutdown.Size = new System.Drawing.Size(111, 16);
            this.lblShutdown.TabIndex = 27;
            this.lblShutdown.Text = "shutdownLabel";
            this.lblShutdown.Visible = false;
            // 
            // selectDemo
            // 
            this.selectDemo.AutoSize = true;
            this.selectDemo.Location = new System.Drawing.Point(720, 102);
            this.selectDemo.Name = "selectDemo";
            this.selectDemo.Size = new System.Drawing.Size(77, 17);
            this.selectDemo.TabIndex = 29;
            this.selectDemo.Text = "Run Demo";
            this.selectDemo.UseVisualStyleBackColor = true;
            // 
            // btnSubtractOneTemp
            // 
            this.btnSubtractOneTemp.Location = new System.Drawing.Point(521, 158);
            this.btnSubtractOneTemp.Name = "btnSubtractOneTemp";
            this.btnSubtractOneTemp.Size = new System.Drawing.Size(35, 35);
            this.btnSubtractOneTemp.TabIndex = 30;
            this.btnSubtractOneTemp.Text = "-1";
            this.btnSubtractOneTemp.UseVisualStyleBackColor = true;
            this.btnSubtractOneTemp.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSubtractFiveTemp
            // 
            this.btnSubtractFiveTemp.Location = new System.Drawing.Point(574, 158);
            this.btnSubtractFiveTemp.Name = "btnSubtractFiveTemp";
            this.btnSubtractFiveTemp.Size = new System.Drawing.Size(35, 35);
            this.btnSubtractFiveTemp.TabIndex = 31;
            this.btnSubtractFiveTemp.Text = "-5";
            this.btnSubtractFiveTemp.UseVisualStyleBackColor = true;
            this.btnSubtractFiveTemp.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSubtractXTemp
            // 
            this.btnSubtractXTemp.Location = new System.Drawing.Point(622, 158);
            this.btnSubtractXTemp.Name = "btnSubtractXTemp";
            this.btnSubtractXTemp.Size = new System.Drawing.Size(35, 35);
            this.btnSubtractXTemp.TabIndex = 32;
            this.btnSubtractXTemp.Text = "-X";
            this.btnSubtractXTemp.UseVisualStyleBackColor = true;
            this.btnSubtractXTemp.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtCustTemp
            // 
            this.txtCustTemp.Location = new System.Drawing.Point(673, 158);
            this.txtCustTemp.Name = "txtCustTemp";
            this.txtCustTemp.Size = new System.Drawing.Size(51, 20);
            this.txtCustTemp.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(663, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Custom Value";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.lblElLimStatus2);
            this.panel1.Controls.Add(this.lblElLimStatus1);
            this.panel1.Controls.Add(this.lblAzLimStatus2);
            this.panel1.Controls.Add(this.lblAzLimStatus1);
            this.panel1.Controls.Add(this.lblElLimit2);
            this.panel1.Controls.Add(this.lblElLimit1);
            this.panel1.Controls.Add(this.lblAzLimit2);
            this.panel1.Controls.Add(this.lblAzLimit1);
            this.panel1.Controls.Add(this.lblEleProx2);
            this.panel1.Controls.Add(this.lblEleProx1);
            this.panel1.Controls.Add(this.lblElProx2);
            this.panel1.Controls.Add(this.lblElProx1);
            this.panel1.Controls.Add(this.lblAzProxStatus3);
            this.panel1.Controls.Add(this.lblAzProxStatus2);
            this.panel1.Controls.Add(this.lblAzProxStatus1);
            this.panel1.Controls.Add(this.lblAzProx3);
            this.panel1.Controls.Add(this.lblAzProx2);
            this.panel1.Controls.Add(this.lblAzProx1);
            this.panel1.Location = new System.Drawing.Point(495, 223);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 344);
            this.panel1.TabIndex = 35;
            // 
            // lblElLimStatus2
            // 
            this.lblElLimStatus2.AutoSize = true;
            this.lblElLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus2.Location = new System.Drawing.Point(223, 309);
            this.lblElLimStatus2.Name = "lblElLimStatus2";
            this.lblElLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus2.TabIndex = 17;
            this.lblElLimStatus2.Text = "Inactive";
            // 
            // lblElLimStatus1
            // 
            this.lblElLimStatus1.AutoSize = true;
            this.lblElLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus1.Location = new System.Drawing.Point(223, 271);
            this.lblElLimStatus1.Name = "lblElLimStatus1";
            this.lblElLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus1.TabIndex = 16;
            this.lblElLimStatus1.Text = "Inactive";
            // 
            // lblAzLimStatus2
            // 
            this.lblAzLimStatus2.AutoSize = true;
            this.lblAzLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus2.Location = new System.Drawing.Point(223, 231);
            this.lblAzLimStatus2.Name = "lblAzLimStatus2";
            this.lblAzLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblAzLimStatus2.TabIndex = 15;
            this.lblAzLimStatus2.Text = "Inactive";
            this.lblAzLimStatus2.Click += new System.EventHandler(this.lblAzLimStatus2_Click);
            // 
            // lblAzLimStatus1
            // 
            this.lblAzLimStatus1.AutoSize = true;
            this.lblAzLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus1.Location = new System.Drawing.Point(223, 193);
            this.lblAzLimStatus1.Name = "lblAzLimStatus1";
            this.lblAzLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzLimStatus1.TabIndex = 14;
            this.lblAzLimStatus1.Text = "Inactive";
            // 
            // lblElLimit2
            // 
            this.lblElLimit2.AutoSize = true;
            this.lblElLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit2.Location = new System.Drawing.Point(14, 309);
            this.lblElLimit2.Name = "lblElLimit2";
            this.lblElLimit2.Size = new System.Drawing.Size(160, 15);
            this.lblElLimit2.TabIndex = 13;
            this.lblElLimit2.Text = "Elevation Limit Switch 2";
            // 
            // lblElLimit1
            // 
            this.lblElLimit1.AutoSize = true;
            this.lblElLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit1.Location = new System.Drawing.Point(14, 271);
            this.lblElLimit1.Name = "lblElLimit1";
            this.lblElLimit1.Size = new System.Drawing.Size(160, 15);
            this.lblElLimit1.TabIndex = 12;
            this.lblElLimit1.Text = "Elevation Limit Switch 1";
            // 
            // lblAzLimit2
            // 
            this.lblAzLimit2.AutoSize = true;
            this.lblAzLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit2.Location = new System.Drawing.Point(14, 231);
            this.lblAzLimit2.Name = "lblAzLimit2";
            this.lblAzLimit2.Size = new System.Drawing.Size(152, 15);
            this.lblAzLimit2.TabIndex = 11;
            this.lblAzLimit2.Text = "Azimuth Limit Switch 1";
            // 
            // lblAzLimit1
            // 
            this.lblAzLimit1.AutoSize = true;
            this.lblAzLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit1.Location = new System.Drawing.Point(14, 193);
            this.lblAzLimit1.Name = "lblAzLimit1";
            this.lblAzLimit1.Size = new System.Drawing.Size(152, 15);
            this.lblAzLimit1.TabIndex = 10;
            this.lblAzLimit1.Text = "Azimuth Limit Switch 1";
            // 
            // lblEleProx2
            // 
            this.lblEleProx2.AutoSize = true;
            this.lblEleProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEleProx2.Location = new System.Drawing.Point(223, 155);
            this.lblEleProx2.Name = "lblEleProx2";
            this.lblEleProx2.Size = new System.Drawing.Size(56, 15);
            this.lblEleProx2.TabIndex = 9;
            this.lblEleProx2.Text = "Inactive";
            // 
            // lblEleProx1
            // 
            this.lblEleProx1.AutoSize = true;
            this.lblEleProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEleProx1.Location = new System.Drawing.Point(223, 118);
            this.lblEleProx1.Name = "lblEleProx1";
            this.lblEleProx1.Size = new System.Drawing.Size(56, 15);
            this.lblEleProx1.TabIndex = 8;
            this.lblEleProx1.Text = "Inactive";
            // 
            // lblElProx2
            // 
            this.lblElProx2.AutoSize = true;
            this.lblElProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElProx2.Location = new System.Drawing.Point(14, 155);
            this.lblElProx2.Name = "lblElProx2";
            this.lblElProx2.Size = new System.Drawing.Size(190, 15);
            this.lblElProx2.TabIndex = 7;
            this.lblElProx2.Text = "Elevation Proximity Sensor 2";
            // 
            // lblElProx1
            // 
            this.lblElProx1.AutoSize = true;
            this.lblElProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElProx1.Location = new System.Drawing.Point(14, 118);
            this.lblElProx1.Name = "lblElProx1";
            this.lblElProx1.Size = new System.Drawing.Size(190, 15);
            this.lblElProx1.TabIndex = 6;
            this.lblElProx1.Text = "Elevation Proximity Sensor 1";
            // 
            // lblAzProxStatus3
            // 
            this.lblAzProxStatus3.AutoSize = true;
            this.lblAzProxStatus3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus3.Location = new System.Drawing.Point(223, 83);
            this.lblAzProxStatus3.Name = "lblAzProxStatus3";
            this.lblAzProxStatus3.Size = new System.Drawing.Size(56, 15);
            this.lblAzProxStatus3.TabIndex = 5;
            this.lblAzProxStatus3.Text = "Inactive";
            // 
            // lblAzProxStatus2
            // 
            this.lblAzProxStatus2.AutoSize = true;
            this.lblAzProxStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus2.Location = new System.Drawing.Point(223, 50);
            this.lblAzProxStatus2.Name = "lblAzProxStatus2";
            this.lblAzProxStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblAzProxStatus2.TabIndex = 4;
            this.lblAzProxStatus2.Text = "Inactive";
            // 
            // lblAzProxStatus1
            // 
            this.lblAzProxStatus1.AutoSize = true;
            this.lblAzProxStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus1.Location = new System.Drawing.Point(223, 18);
            this.lblAzProxStatus1.Name = "lblAzProxStatus1";
            this.lblAzProxStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzProxStatus1.TabIndex = 3;
            this.lblAzProxStatus1.Text = "Inactive";
            // 
            // lblAzProx3
            // 
            this.lblAzProx3.AutoSize = true;
            this.lblAzProx3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx3.Location = new System.Drawing.Point(14, 83);
            this.lblAzProx3.Name = "lblAzProx3";
            this.lblAzProx3.Size = new System.Drawing.Size(182, 15);
            this.lblAzProx3.TabIndex = 2;
            this.lblAzProx3.Text = "Azimuth Proximity Sensor 3";
            // 
            // lblAzProx2
            // 
            this.lblAzProx2.AutoSize = true;
            this.lblAzProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx2.Location = new System.Drawing.Point(14, 50);
            this.lblAzProx2.Name = "lblAzProx2";
            this.lblAzProx2.Size = new System.Drawing.Size(182, 15);
            this.lblAzProx2.TabIndex = 1;
            this.lblAzProx2.Text = "Azimuth Proximity Sensor 2";
            // 
            // lblAzProx1
            // 
            this.lblAzProx1.AutoSize = true;
            this.lblAzProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx1.Location = new System.Drawing.Point(14, 18);
            this.lblAzProx1.Name = "lblAzProx1";
            this.lblAzProx1.Size = new System.Drawing.Size(182, 15);
            this.lblAzProx1.TabIndex = 0;
            this.lblAzProx1.Text = "Azimuth Proximity Sensor 1";
            // 
            // lblAbsEncoder
            // 
            this.lblAbsEncoder.AutoSize = true;
            this.lblAbsEncoder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbsEncoder.Location = new System.Drawing.Point(32, 367);
            this.lblAbsEncoder.Name = "lblAbsEncoder";
            this.lblAbsEncoder.Size = new System.Drawing.Size(115, 15);
            this.lblAbsEncoder.TabIndex = 36;
            this.lblAbsEncoder.Text = "Azimuth Encoder";
            // 
            // lblEncoderDegrees
            // 
            this.lblEncoderDegrees.AutoSize = true;
            this.lblEncoderDegrees.Location = new System.Drawing.Point(32, 394);
            this.lblEncoderDegrees.Name = "lblEncoderDegrees";
            this.lblEncoderDegrees.Size = new System.Drawing.Size(47, 13);
            this.lblEncoderDegrees.TabIndex = 37;
            this.lblEncoderDegrees.Text = "Degrees";
            // 
            // lblAzEncoderDegrees
            // 
            this.lblAzEncoderDegrees.AutoSize = true;
            this.lblAzEncoderDegrees.Location = new System.Drawing.Point(96, 394);
            this.lblAzEncoderDegrees.Name = "lblAzEncoderDegrees";
            this.lblAzEncoderDegrees.Size = new System.Drawing.Size(13, 13);
            this.lblAzEncoderDegrees.TabIndex = 38;
            this.lblAzEncoderDegrees.Text = "0";
            // 
            // lblEncoderTicks
            // 
            this.lblEncoderTicks.AutoSize = true;
            this.lblEncoderTicks.Location = new System.Drawing.Point(31, 417);
            this.lblEncoderTicks.Name = "lblEncoderTicks";
            this.lblEncoderTicks.Size = new System.Drawing.Size(33, 13);
            this.lblEncoderTicks.TabIndex = 39;
            this.lblEncoderTicks.Text = "Ticks";
            // 
            // lblAzEncoderTicks
            // 
            this.lblAzEncoderTicks.AutoSize = true;
            this.lblAzEncoderTicks.Location = new System.Drawing.Point(96, 417);
            this.lblAzEncoderTicks.Name = "lblAzEncoderTicks";
            this.lblAzEncoderTicks.Size = new System.Drawing.Size(13, 13);
            this.lblAzEncoderTicks.TabIndex = 40;
            this.lblAzEncoderTicks.Text = "0";
            // 
            // btnAddOneEncoder
            // 
            this.btnAddOneEncoder.Location = new System.Drawing.Point(10, 437);
            this.btnAddOneEncoder.Name = "btnAddOneEncoder";
            this.btnAddOneEncoder.Size = new System.Drawing.Size(35, 35);
            this.btnAddOneEncoder.TabIndex = 41;
            this.btnAddOneEncoder.Text = "+1";
            this.btnAddOneEncoder.UseVisualStyleBackColor = true;
            this.btnAddOneEncoder.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnAddFiveEncoder
            // 
            this.btnAddFiveEncoder.Location = new System.Drawing.Point(54, 437);
            this.btnAddFiveEncoder.Name = "btnAddFiveEncoder";
            this.btnAddFiveEncoder.Size = new System.Drawing.Size(35, 35);
            this.btnAddFiveEncoder.TabIndex = 42;
            this.btnAddFiveEncoder.Text = "+5";
            this.btnAddFiveEncoder.UseVisualStyleBackColor = true;
            this.btnAddFiveEncoder.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnAddXEncoder
            // 
            this.btnAddXEncoder.Location = new System.Drawing.Point(96, 437);
            this.btnAddXEncoder.Name = "btnAddXEncoder";
            this.btnAddXEncoder.Size = new System.Drawing.Size(35, 35);
            this.btnAddXEncoder.TabIndex = 43;
            this.btnAddXEncoder.Text = "+X";
            this.btnAddXEncoder.UseVisualStyleBackColor = true;
            this.btnAddXEncoder.Click += new System.EventHandler(this.btnAddXEncoder_Click);
            // 
            // btnSubtractOneEncoder
            // 
            this.btnSubtractOneEncoder.Location = new System.Drawing.Point(9, 478);
            this.btnSubtractOneEncoder.Name = "btnSubtractOneEncoder";
            this.btnSubtractOneEncoder.Size = new System.Drawing.Size(35, 35);
            this.btnSubtractOneEncoder.TabIndex = 44;
            this.btnSubtractOneEncoder.Text = "-1";
            this.btnSubtractOneEncoder.UseVisualStyleBackColor = true;
            this.btnSubtractOneEncoder.Click += new System.EventHandler(this.btnSubtractOneEncoder_Click);
            // 
            // btnSubtractFiveEncoder
            // 
            this.btnSubtractFiveEncoder.Location = new System.Drawing.Point(54, 478);
            this.btnSubtractFiveEncoder.Name = "btnSubtractFiveEncoder";
            this.btnSubtractFiveEncoder.Size = new System.Drawing.Size(35, 35);
            this.btnSubtractFiveEncoder.TabIndex = 45;
            this.btnSubtractFiveEncoder.Text = "-5";
            this.btnSubtractFiveEncoder.UseVisualStyleBackColor = true;
            this.btnSubtractFiveEncoder.Click += new System.EventHandler(this.btnSubtractFiveEncoder_Click);
            // 
            // btnSubtractXEncoder
            // 
            this.btnSubtractXEncoder.Location = new System.Drawing.Point(95, 478);
            this.btnSubtractXEncoder.Name = "btnSubtractXEncoder";
            this.btnSubtractXEncoder.Size = new System.Drawing.Size(35, 35);
            this.btnSubtractXEncoder.TabIndex = 46;
            this.btnSubtractXEncoder.Text = "-X";
            this.btnSubtractXEncoder.UseVisualStyleBackColor = true;
            this.btnSubtractXEncoder.Click += new System.EventHandler(this.btnSubtractXEncoder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(137, 448);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "Custom Value";
            // 
            // txtCustEncoderVal
            // 
            this.txtCustEncoderVal.Location = new System.Drawing.Point(140, 478);
            this.txtCustEncoderVal.Name = "txtCustEncoderVal";
            this.txtCustEncoderVal.Size = new System.Drawing.Size(51, 20);
            this.txtCustEncoderVal.TabIndex = 48;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(378, 478);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 56;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(375, 448);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 55;
            this.label10.Text = "Custom Value";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 478);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 35);
            this.button1.TabIndex = 54;
            this.button1.Text = "-X";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(292, 478);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 35);
            this.button2.TabIndex = 53;
            this.button2.Text = "-5";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(247, 478);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 35);
            this.button3.TabIndex = 52;
            this.button3.Text = "-1";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(334, 437);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(35, 35);
            this.button4.TabIndex = 51;
            this.button4.Text = "+X";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(292, 437);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(35, 35);
            this.button5.TabIndex = 50;
            this.button5.Text = "+5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(248, 437);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(35, 35);
            this.button6.TabIndex = 49;
            this.button6.Text = "+1";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // lblElEncoderTicks
            // 
            this.lblElEncoderTicks.AutoSize = true;
            this.lblElEncoderTicks.Location = new System.Drawing.Point(341, 417);
            this.lblElEncoderTicks.Name = "lblElEncoderTicks";
            this.lblElEncoderTicks.Size = new System.Drawing.Size(13, 13);
            this.lblElEncoderTicks.TabIndex = 61;
            this.lblElEncoderTicks.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(276, 417);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 60;
            this.label14.Text = "Ticks";
            // 
            // lblElEncoderDegrees
            // 
            this.lblElEncoderDegrees.AutoSize = true;
            this.lblElEncoderDegrees.Location = new System.Drawing.Point(341, 394);
            this.lblElEncoderDegrees.Name = "lblElEncoderDegrees";
            this.lblElEncoderDegrees.Size = new System.Drawing.Size(13, 13);
            this.lblElEncoderDegrees.TabIndex = 59;
            this.lblElEncoderDegrees.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(277, 394);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 13);
            this.label16.TabIndex = 58;
            this.label16.Text = "Degrees";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(277, 367);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(123, 15);
            this.label17.TabIndex = 57;
            this.label17.Text = "Elevation Encoder";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 543);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Set Bits of Precision";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(30, 559);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(39, 20);
            this.textBox2.TabIndex = 63;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 582);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 13);
            this.label13.TabIndex = 64;
            this.label13.Text = "Set Error";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1158, 74);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 13);
            this.label15.TabIndex = 65;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(29, 598);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(40, 20);
            this.textBox3.TabIndex = 66;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(30, 637);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(39, 20);
            this.textBox4.TabIndex = 67;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(16, 621);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(63, 13);
            this.label18.TabIndex = 68;
            this.label18.Text = "Set Position";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(163, 341);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(98, 13);
            this.label19.TabIndex = 69;
            this.label19.Text = "Encoder Simulation";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(245, 534);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(95, 13);
            this.label21.TabIndex = 71;
            this.label21.Text = "Has Active Move?";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(346, 534);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 13);
            this.label22.TabIndex = 72;
            this.label22.Text = "True";
            // 
            // DiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 682);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblElEncoderTicks);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblElEncoderDegrees);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.txtCustEncoderVal);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSubtractXEncoder);
            this.Controls.Add(this.btnSubtractFiveEncoder);
            this.Controls.Add(this.btnSubtractOneEncoder);
            this.Controls.Add(this.btnAddXEncoder);
            this.Controls.Add(this.btnAddFiveEncoder);
            this.Controls.Add(this.btnAddOneEncoder);
            this.Controls.Add(this.lblAzEncoderTicks);
            this.Controls.Add(this.lblEncoderTicks);
            this.Controls.Add(this.lblAzEncoderDegrees);
            this.Controls.Add(this.lblEncoderDegrees);
            this.Controls.Add(this.lblAbsEncoder);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCustTemp);
            this.Controls.Add(this.btnSubtractXTemp);
            this.Controls.Add(this.btnSubtractFiveTemp);
            this.Controls.Add(this.btnSubtractOneTemp);
            this.Controls.Add(this.selectDemo);
            this.Controls.Add(this.lblShutdown);
            this.Controls.Add(this.btnAddXTemp);
            this.Controls.Add(this.btnAddFiveTemp);
            this.Controls.Add(this.btnAddOneTemp);
            this.Controls.Add(this.fanLabel);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtTemperature);
            this.Controls.Add(this.fldElTemp);
            this.Controls.Add(this.fldAzTemp);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lblElevationTemp);
            this.Controls.Add(this.lblAzimuthTemp);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.endTimeTextBox);
            this.Controls.Add(this.startTimeTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCurrentElOrientation);
            this.Controls.Add(this.lblCurrentAzOrientation);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(615, 249);
            this.Name = "DiagnosticsForm";
            this.Text = "DiagnosticsForm";
            this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblCurrentAzOrientation;
        private System.Windows.Forms.Label lblCurrentElOrientation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox startTimeTextBox;
        private System.Windows.Forms.TextBox endTimeTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblAzimuthTemp;
        private System.Windows.Forms.Label lblElevationTemp;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label fldAzTemp;
        private System.Windows.Forms.Label fldElTemp;
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label fanLabel;
        private System.Windows.Forms.Button btnAddOneTemp;
        private System.Windows.Forms.Button btnAddFiveTemp;
        private System.Windows.Forms.Button btnAddXTemp;
        private System.Windows.Forms.Label lblShutdown;
        private System.Windows.Forms.CheckBox selectDemo;
        private System.Windows.Forms.Button btnSubtractOneTemp;
        private System.Windows.Forms.Button btnSubtractFiveTemp;
        private System.Windows.Forms.Button btnSubtractXTemp;
        private System.Windows.Forms.TextBox txtCustTemp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblElLimStatus2;
        private System.Windows.Forms.Label lblElLimStatus1;
        private System.Windows.Forms.Label lblAzLimStatus2;
        private System.Windows.Forms.Label lblAzLimStatus1;
        private System.Windows.Forms.Label lblElLimit2;
        private System.Windows.Forms.Label lblElLimit1;
        private System.Windows.Forms.Label lblAzLimit2;
        private System.Windows.Forms.Label lblAzLimit1;
        private System.Windows.Forms.Label lblEleProx2;
        private System.Windows.Forms.Label lblEleProx1;
        private System.Windows.Forms.Label lblElProx2;
        private System.Windows.Forms.Label lblElProx1;
        private System.Windows.Forms.Label lblAzProxStatus3;
        private System.Windows.Forms.Label lblAzProxStatus2;
        private System.Windows.Forms.Label lblAzProxStatus1;
        private System.Windows.Forms.Label lblAzProx3;
        private System.Windows.Forms.Label lblAzProx2;
        private System.Windows.Forms.Label lblAzProx1;
        private System.Windows.Forms.Label lblAbsEncoder;
        private System.Windows.Forms.Label lblEncoderDegrees;
        private System.Windows.Forms.Label lblAzEncoderDegrees;
        private System.Windows.Forms.Label lblEncoderTicks;
        private System.Windows.Forms.Label lblAzEncoderTicks;
        private System.Windows.Forms.Button btnAddOneEncoder;
        private System.Windows.Forms.Button btnAddFiveEncoder;
        private System.Windows.Forms.Button btnAddXEncoder;
        private System.Windows.Forms.Button btnSubtractOneEncoder;
        private System.Windows.Forms.Button btnSubtractFiveEncoder;
        private System.Windows.Forms.Button btnSubtractXEncoder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCustEncoderVal;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label lblElEncoderTicks;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblElEncoderDegrees;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
    }
}