using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Login_EDP
{
    public partial class frmUser : Form
    {
        MyDatabase db = new MyDatabase();
        public frmUser()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Stop! You must enter letters in all required fields.",
                                "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            string sql = "INSERT INTO tbl_registration (firstname, middlename, lastname, email, address, birthdate, username, password) " +
                         "VALUES (@fn, @mn, @ln, @em, @ad, @bd, @un, @pw)";

            MySqlParameter[] parameters = {
    new MySqlParameter("@fn", txtFirstName.Text),
    new MySqlParameter("@mn", txtMiddleName.Text),
    new MySqlParameter("@ln", txtLastName.Text),
    new MySqlParameter("@em", txtEmail.Text),
    new MySqlParameter("@ad", txtAddress.Text),
    new MySqlParameter("@bd", dtpBirthDate.Value.ToString("yyyy-MM-dd")),
    new MySqlParameter("@un", txtUsername.Text),
    new MySqlParameter("@pw", txtPassword.Text)
};

            try
            {
                db.ExecuteNonQuery(sql, parameters);

                dgvUsers.Rows.Add(
                    txtFirstName.Text,
                    txtMiddleName.Text,
                    txtLastName.Text,
                    txtEmail.Text,
                    txtAddress.Text,
                    dtpBirthDate.Value.ToShortDateString(),
                    txtUsername.Text,
                    txtPassword.Text
                );

                MessageBox.Show("Successfully saved to Database and Grid!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving to database: " + ex.Message);
            }
        }



        private void ClearAllFields()
        {
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            dtpBirthDate.Value = DateTime.Now;
            txtFirstName.Focus();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {

            if (dgvUsers.Columns.Count == 0)
            {
                dgvUsers.Columns.Add("colFN", "First Name");
                dgvUsers.Columns.Add("colMN", "Middle Name");
                dgvUsers.Columns.Add("colLN", "Last Name");
                dgvUsers.Columns.Add("colEmail", "Email");
                dgvUsers.Columns.Add("colAddress", "Address");
                dgvUsers.Columns.Add("colBirth", "Birth Date");
                dgvUsers.Columns.Add("colUser", "Username");
                dgvUsers.Columns.Add("colPass", "Password");
            }

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];


                txtFirstName.Text = row.Cells["colFN"].Value?.ToString();
                txtMiddleName.Text = row.Cells["colMN"].Value?.ToString();
                txtLastName.Text = row.Cells["colLN"].Value?.ToString();
                txtEmail.Text = row.Cells["colEmail"].Value?.ToString();
                txtAddress.Text = row.Cells["colAddress"].Value?.ToString();

                if (DateTime.TryParse(row.Cells["colBirth"].Value?.ToString(), out DateTime birthDate))
                {
                    dtpBirthDate.Value = birthDate;
                }

                txtUsername.Text = row.Cells["colUser"].Value?.ToString();
                txtPassword.Text = row.Cells["colPass"].Value?.ToString();


                txtUsername.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the grid to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username cannot be empty. Unable to locate record for update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = "UPDATE tbl_registration SET firstname=@fn, middlename=@mn, lastname=@ln, " +
                         "email=@em, address=@ad, birthdate=@bd, password=@pw WHERE username=@un";

            MySqlParameter[] parameters = {
        new MySqlParameter("@fn", txtFirstName.Text.Trim()),
        new MySqlParameter("@mn", txtMiddleName.Text.Trim()),
        new MySqlParameter("@ln", txtLastName.Text.Trim()),
        new MySqlParameter("@em", txtEmail.Text.Trim()),
        new MySqlParameter("@ad", txtAddress.Text.Trim()),
        new MySqlParameter("@bd", dtpBirthDate.Value.ToString("yyyy-MM-dd")),
        new MySqlParameter("@pw", txtPassword.Text),
        new MySqlParameter("@un", txtUsername.Text.Trim())
    };

            try
            {
                db.ExecuteNonQuery(sql, parameters);

                DataGridViewRow currentRow = dgvUsers.CurrentRow;

                currentRow.Cells["colFN"].Value = txtFirstName.Text;
                currentRow.Cells["colMN"].Value = txtMiddleName.Text;
                currentRow.Cells["colLN"].Value = txtLastName.Text;
                currentRow.Cells["colEmail"].Value = txtEmail.Text;
                currentRow.Cells["colAddress"].Value = txtAddress.Text;
                currentRow.Cells["colBirth"].Value = dtpBirthDate.Value.ToShortDateString();
                currentRow.Cells["colPass"].Value = txtPassword.Text;

                MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtUsername.Enabled = true;
                ClearAllFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialog = MessageBox.Show($"Are you sure you want to permanently delete {txtUsername.Text}?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialog == DialogResult.Yes)
            {
                string sql = "DELETE FROM tbl_registration WHERE username = @un";
                MySqlParameter[] parameters = { new MySqlParameter("@un", txtUsername.Text) };

                try
                {

                    db.ExecuteNonQuery(sql, parameters);

                    if (dgvUsers.CurrentRow != null)
                    {
                        dgvUsers.Rows.Remove(dgvUsers.CurrentRow);
                    }

                    MessageBox.Show("User has been permanently deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.Enabled = true;
                    ClearAllFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}