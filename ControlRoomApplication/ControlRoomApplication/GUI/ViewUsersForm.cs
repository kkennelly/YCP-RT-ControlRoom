using ControlRoomApplication.Entities;
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
    public partial class ViewUsersForm : Form
    {
        public ViewUsersForm()
        {
            InitializeComponent();

            InitializeDataGrid();

            LoadUsers();
        }
        
        private void InitializeDataGrid()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;
            dataGridView1.ColumnHeadersVisible = true;

            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Email";
            dataGridView1.Columns[3].Name = "Phone #";
        }

        private void LoadUsers()
        {
            List<User> users = Database.DatabaseOperations.GetAllUsers();

            foreach (User user in users)
            {
                string[] row = {
                    user.Id.ToString(),
                    user.first_name + " " + user.last_name,
                    user.email_address,
                    user.phone_number
                };

                dataGridView1.Rows.Add(row);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
