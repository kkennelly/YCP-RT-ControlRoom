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
            dataGridView1.ColumnCount = 5;
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
            dataGridView1.Columns[4].Name = "Role";
        }

        private void LoadUsers()
        {
            List<User> users = Database.DatabaseOperations.GetAllUsers();
            List<UserRole> userRoles = Database.DatabaseOperations.GetUserRoles();

            foreach (User user in users)
            {
                string[] row = new string[5];
                row[0] = user.Id.ToString();
                row[1] = user.first_name + " " + user.last_name;
                row[2] = user.email_address;
                row[3] = user.phone_number;

                try
                {
                    row[4] = userRoles.Find(userRole => userRole.user_id == user.Id).role;
                }
                catch (NullReferenceException ex)
                {
                    row[4] = "N/A";
                }

                dataGridView1.Rows.Add(row);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
