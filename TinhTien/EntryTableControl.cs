using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace TinhTien
{
    public partial class EntryTableControl : UserControl
    {
        public Table table;
        public Boolean Close = false;
        public event EventHandler ButtonClick;
        public EntryTableControl(int num)
        {
            InitializeComponent();
            table= new Table(num);
            lblNum.Text = " " + num;
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("vi-VN");
            DateTime dateTime = DateTime.Now;

            lblDate.Text = dateTime.ToString(cultureinfo);


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            table.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close = true;
            if (this.ButtonClick != null)
                this.ButtonClick(this, e);
        }
    }
}
