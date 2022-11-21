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
    public partial class ViewAppointmentForm : Form
    {
        private int id;

        public ViewAppointmentForm(int id)
        {
            this.id = id;

            InitializeComponent();

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            List<Entities.Appointment> appts = Database.DatabaseOperations.GetListOfAppointmentsForRadioTelescope(id);

            foreach (Entities.Appointment appt in appts)
            {
                listBox1.Items.Add(appt.ToString());
            }
        }
    }
}
