using System;
using System.Windows.Forms;

namespace Lab_04
{
    public partial class DeleteUserForm : Form
    {
        public string CNP
        {
            get { return CNPTextBox.Text.Trim(); }
        }

        public DeleteUserForm()
        {
            InitializeComponent();

            AcceptButton = DeleteButton;
            CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
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