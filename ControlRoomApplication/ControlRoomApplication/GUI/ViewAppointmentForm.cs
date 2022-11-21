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

            InitializeDataGrid();

            LoadAppointments();
        }
        
        private void InitializeDataGrid()
        {
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ColumnHeadersVisible = true;

            dataGridView1.Columns[0].Name = "User";
            dataGridView1.Columns[1].Name = "Status";
            dataGridView1.Columns[2].Name = "Start Time";
            dataGridView1.Columns[3].Name = "End Time";
        }

        private void LoadAppointments()
        {
            List<Entities.Appointment> appts = Database.DatabaseOperations.GetListOfAppointmentsForRadioTelescope(id);

            foreach (Entities.Appointment appt in appts)
            {
                string[] row = { appt.User.ToString(), appt.status.ToString(), appt.start_time.ToShortTimeString(), appt.end_time.ToShortTimeString() };

                dataGridView1.Rows.Add(row);
            }
        }
    }
}
