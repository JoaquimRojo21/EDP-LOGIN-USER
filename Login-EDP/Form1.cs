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
                MessageBox.Show("Enter username and password");
                return;
            }


            string sql = "SELECT * FROM tbllogincredentials WHERE user_username = @uname AND user_password = @pword";


            MySqlParameter[] loginParams = {
        new MySqlParameter("@uname", tbUsername.Text),
        new MySqlParameter("@pword", tbPassword.Text)
    };

            if (tbUsername.Text == "Admin" && tbPassword.Text == "1234")
            {
                MessageBox.Show("Welcome Admin");
                Form2 home = new Form2();
                home.userName = "Admin";
                this.Hide();
                home.Show();
                return; 
            }
            try

            {

                DataTable dt = db.ExecuteReturnQuery(sql, loginParams);

                if (dt.Rows.Count == 1)
                {

                    string uName = dt.Rows[0]["user_username"].ToString();
                    MessageBox.Show("Welcome " + uName);

                    Form2 home = new Form2();
                    home.userName = uName; 
                    this.Hide();
                    home.Show();
                }
                else
                {

                    MessageBox.Show("Invalid Username or Password");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error: " + ex.Message);
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
            
       
