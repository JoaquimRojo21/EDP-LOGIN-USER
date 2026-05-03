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


            string sql = "INSERT INTO tbl_registration (firstname, middlename, lastname, email, address, birthdate, username) " +
                         "VALUES (@fn, @mn, @ln, @em, @ad, @bd, @un)";


            MySqlParameter[] parameters = {
                new MySqlParameter("@fn", txtFirstName.Text),
                new MySqlParameter("@mn", txtMiddleName.Text),
                new MySqlParameter("@ln", txtLastName.Text),
                new MySqlParameter("@em", txtEmail.Text),
                new MySqlParameter("@ad", txtAddress.Text),
                new MySqlParameter("@bd", dtpBirthDate.Value.ToString("yyyy-MM-dd")), // SQL Date format
                new MySqlParameter("@un", txtUsername.Text)
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
                    txtUsername.Text
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
            }

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
