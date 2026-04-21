using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_EDP
{
    public partial class Form2 : Form
    {
        public string userName;
        public Form2()
        {

            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, btnLogout.Width, btnLogout.Height);


            btnLogout.Region = new System.Drawing.Region(path);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            {
                DialogResult result = MessageBox.Show("Do you want to logout?",
                    "logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                }
                frmLogin login = new frmLogin();

                login.Show();


                this.Hide();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegister registerForm = new frmRegister();
            registerForm.Show();          
        }
    }
}
