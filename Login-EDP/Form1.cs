using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_EDP
{
    public partial class frmLogin : Form
    {
        MyDatabase db = new MyDatabase();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(tbUsername.Text) || string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tbUsername.Text == "Admin" && tbPassword.Text == "1234")
            {
                MessageBox.Show("Welcome Admin!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form2 home = new Form2();
                home.userName = "Admin";
                this.Hide();
                home.Show();
                return;
            }

  
            string sql = "SELECT * FROM tbl_registration WHERE username = @uname AND password = @pword AND status = 'Active'";

            MySqlParameter[] loginParams = {
        new MySqlParameter("@uname", tbUsername.Text.Trim()),
        new MySqlParameter("@pword", tbPassword.Text)
    };

            try
            {
                DataTable dt = db.ExecuteReturnQuery(sql, loginParams);

                if (dt != null && dt.Rows.Count == 1)
                {
                    string uName = dt.Rows[0]["username"].ToString();

                    MessageBox.Show("Welcome " + uName + "!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form2 home = new Form2();
                    home.userName = uName;
                    this.Hide();
                    home.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Username, Password, or account is Inactive.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        


        private void frmLogin_Load(object sender, EventArgs e)
        {

            if (db.TestConnection() == true)
            {
                MessageBox.Show("Connected Successfully");
            }
            else
            {

                MessageBox.Show("NO: Cannot connect to database. Check XAMPP or DB Name.");
            }
        }
    }
}
            
       
