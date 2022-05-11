using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Input dialog box class that can be used for any string input 
/// </summary>
namespace ControlRoomApplication.GUI
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        public void setPromptLabel(string text)
        {
            promptLabel.Text = text;
        }
    }
}
