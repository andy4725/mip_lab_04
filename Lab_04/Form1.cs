using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace Lab_04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Database.Initialize();
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            using(var dialog = new AddUserForm())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    Database.AddUser(dialog.FirstName, dialog.LastName, dialog.CNP);
                    MessageBox.Show("User added.");
                }
                catch(System.Data.SQLite.SQLiteException ex)
                {
                    MessageBox.Show("Could not add user. The CNP may already exist.\n" + ex.Message);
                }
            }
        }

        private void ViewUsersButton_Click(object sender, EventArgs e)
        {
            using (var form = new ViewUsersForm())
            {
                form.ShowDialog(this);
            }
        }

        private void DeleteUserButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new DeleteUserForm())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                DialogResult confirm = MessageBox.Show(
                    "Delete the user with this CNP?",
                    "Confirm deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes)
                    return;

                if (Database.DeleteUser(dialog.CNP))
                {
                    MessageBox.Show("User deleted.");
                }
                else
                {
                    MessageBox.Show("No user was found with that CNP.");
                }
            }
        }

        private void DeleteTableButton_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
        "This will permanently delete the Users table and all its data. Continue?",
        "Confirm table deletion",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                Database.DeleteUsersTable();
                MessageBox.Show("Users table deleted.");
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                MessageBox.Show("Could not delete the Users table.\n" + ex.Message);
            }
        }
    }
}
