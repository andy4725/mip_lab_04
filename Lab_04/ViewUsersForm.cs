using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_04
{
    public partial class ViewUsersForm : Form
    {
        public ViewUsersForm()
        {
            InitializeComponent();
            usersGridView.ReadOnly = true;
            usersGridView.AllowUserToAddRows = false;
            usersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersGridView.DataSource = Database.GetUsers();
        }

        private void usersGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
