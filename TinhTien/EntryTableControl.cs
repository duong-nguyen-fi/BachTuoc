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
        public event EventHandler deleteClick;
        public event EventHandler mergeClick;
        public int Num;
        public EntryTableControl(int num)
        {
            InitializeComponent();
            Num = num;
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
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close = true;
            if (this.deleteClick != null)
                this.deleteClick(this, e);
        }

        private void EntryTableControl_MouseEnter(object sender, EventArgs e)
        {
            
            
        }

        private void EntryTableControl_Enter(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void EntryTableControl_Leave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.None;
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            Close = true;
            if (this.mergeClick != null)
                this.mergeClick(this, e);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            table.ShowDialog();
        }
    }
}
