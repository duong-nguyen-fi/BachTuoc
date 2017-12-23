using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinhTien
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string user = textBox1.Text;
                string password = textBox2.Text;
                if (user.Equals("admin") && password.Equals("admin"))
                {
                    this.Close();
                }
                else
                    MessageBox.Show("WRONG");
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            
        }
    }
}
