using System;
using System.Windows.Forms;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Main
{
    public partial class EditScriptsForm : Form
    {
        public RadioTelescopeController rt_controller { get; set; }
        public ControlRoom controlRoom { get; set; }
        public double speed { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EditScriptsForm(ControlRoom new_controlRoom, int rtId)
        {
            InitializeComponent();

            // Set ControlRoom
            controlRoom = new_controlRoom;

            // Make rt_controller
            rt_controller = controlRoom.RadioTelescopeControllers[rtId - 1];

            // Update Text
            UpdateText("Edit Scripts in Control Form for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Set azSpeed
            // comboBox1.Text = "0.1 RPM";
            speed = 0.1;

            logger.Info(Utilities.GetTimeStamp() + ": Edit Script Form Initalized");
        }

        private void ManualControlForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Edit Script Form Closing");
            timer1.Enabled = false;
        }

        private void UpdateText(string text)
        {
            //errorLabel.Text = text;
        }

        private void NegButton_MouseDown(object sender, MouseEventArgs e)
        {
           // logger.Info(Utilities.GetTimeStamp() + ": Jog NegButton MouseDown");
            //UpdateText("Moving at -" + comboBox1.Text);

            // Start CCW Jog
            //rt_controller.StartRadioTelescopeAzimuthJog(azSpeed, false, );
        }

        private void NegButton_MouseUp(object sender, MouseEventArgs e)
        {
           // logger.Info(Utilities.GetTimeStamp() + ": Jog NegButton MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            //ExecuteCorrectStop();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

      //  private void PosButton_MouseDown(object sender, MouseEventArgs e)
      ////  {
      //      logger.Info(Utilities.GetTimeStamp() + ": Jog PosButton MouseDown");
      //     // UpdateText("Moving at " + comboBox1.Text);

//      // Start CW Jog
//      rt_controller.StartRadioTelescopeAzimuthJog(azSpeed, true);
// // }

//  private void PosButton_MouseUp(object sender, MouseEventArgs e)
//{
//  logger.Info(Utilities.GetTimeStamp() + ": Jog PosButton MouseUp");
//  UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

// Stop Move
//ExecuteCorrectStop();
//   }

// private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
// {
// if(comboBox1.Text == "2 RPM")
// {
//     logger.Info(Utilities.GetTimeStamp() + ": Speed set to 2 RPM");
//     azSpeed = 333333;
// }
//// else if(comboBox1.Text == "0.1 RPM")
// {
//     logger.Info(Utilities.GetTimeStamp() + ": Speed set to 0.1 RPM");
//     azSpeed = 16667;
// }

//else
//{
//    logger.Info(Utilities.GetTimeStamp() + ": Invalid Speed Selected");
//    throw new Exception();
//}
///

//private void ExecuteCorrectStop()
//{
//   // //if (ControlledButtonRadio.Checked)
//   // {
//   //     logger.Info(Utilities.GetTimeStamp() + ": Executed Controlled Stop");
//   //     rt_controller.ExecuteRadioTelescopeControlledStop();
//   // }
//   //// else if (radioButton2.Checked)
//   // {
//   //     logger.Info(Utilities.GetTimeStamp() + ": Executed Immediate Stop");
//   //     rt_controller.ExecuteRadioTelescopeImmediateStop();
//   // }
//    //else
//    //{
//    //    logger.Info(Utilities.GetTimeStamp() + ": Invalid Stop Selected");
//    //    throw new Exception();
//    //}
//}

//private void button1_Click(object sender, EventArgs e)
//{//
//  //  logger.Info(Utilities.GetTimeStamp() + ": Move Relative Button Clicked");
//    //int pos = (int)numericUpDown1.Value * (int)((166 + (2.0 / 3.0)) * 200);
//    //int pos = ConversionHelper.DegreesToSteps( (int)numericUpDown1.Value , MotorConstants.GEARING_RATIO_AZIMUTH );
//   // rt_controller.ExecuteMoveRelativeAzimuth(RadioTelescopeAxisEnum.AZIMUTH,azSpeed, pos);
//}

//private void timer1_Tick(object sender, EventArgs e)
//{
//   // Entities.Orientation currentOrienation = rt_controller.GetCurrentOrientation();
//   // SetActualAZText(currentOrienation.Azimuth.ToString("0.##"));
//  //  SetActualELText(currentOrienation.Elevation.ToString("0.##"));
//}

//  delegate void SetActualAZTextCallback(string text);
//private void SetActualAZText(string text)
//{
//    // InvokeRequired required compares the thread ID of the
//    // calling thread to the thread ID of the creating thread.
//    // If these threads are different, it returns true.
//   // if (ActualAZTextBox.InvokeRequired)
//    {
//        //SetActualAZTextCallback d = new SetActualAZTextCallback(SetActualAZText);
//        var d = new SetActualAZTextCallback(SetActualAZText);
//        try
//        {
//            var task = Task.Run( () => Invoke( d , new object[] { text } ));
//            if(!task.Wait( 300 ))
//                throw new Exception( "Timed out" );

//        }
//        catch  {

//        }
//    }
//    else
//    {
//     //   ActualAZTextBox.Text = text;
//    }
//}

//delegate void SetActualELTextCallback(string text);
//private void SetActualELText(string text)
//{ }
// InvokeRequired required compares the thread ID of the
// calling thread to the thread ID of the creating thread.
// If these threads are different, it returns true.
//    if (ActualELTextBox.InvokeRequired)
//    {
//        //SetActualELTextCallback d = new SetActualELTextCallback(SetActualELText);
//        var d = new SetActualELTextCallback(SetActualELText);
//        try
//        {
//            var task = Task.Run( () => Invoke( d , new object[] { text } ) );
//            if(!task.Wait( 300 ))
//                throw new Exception( "Timed out" );

//        }
//        catch { }
//    }
//    else
//    {
//        ActualELTextBox.Text = text;
//    }
//}

//        private void NegButton_Click(object sender, EventArgs e)
//        {

//        }

//        private void ManualControlForm_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}
