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
    public partial class AddUserForm : Form
    {
        public string FirstName => FirstNameTextBox.Text.Trim();
        public string LastName => LastNameTextBox.Text.Trim();
        public string CNP => CNPTextBox.Text.Trim();
        public AddUserForm()
        {
            InitializeComponent();
            AcceptButton = SaveButton;
            CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show(
                    "Enter both first and last name.",
                    "Missing information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!Database.IsValidCnp(CNP))
            {
                MessageBox.Show(
                    "Enter a valid 13-digit CNP.",
                    "Invalid CNP",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                CNPTextBox.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
