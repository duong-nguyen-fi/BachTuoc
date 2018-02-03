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
        public event EventHandler viewClick;
        public event EventHandler payedEvent;
        public int Num;
        public long Table_id;
        private helper.DBConnection connection;
        public EntryTableControl(int num, long id)
        {
            InitializeComponent();
            Table_id = id;
            Num = num;
            Console.WriteLine("Table ID from entry" + id);
            table= new Table(num, Table_id);
            if (num>0)
            {
            
            lblNum.Text = " " + num;
            }
            else
            {
                lblNum.Text = "Về";
            }
            connection = helper.DBConnection.Instance();
            
            table.onPayedEvent += Table_onPayedEvent;
            
            BackColor = Color.LightGreen;
            getDate();
        }

        private void Table_onPayedEvent(object sender, EventArgs e)
        {
            if (payedEvent != null)
            {
                
                payedEvent(this, e);
                label3.Text = "Đã Thanh Toán";
                BackColor = Color.Empty;
                table.DisableButtons();
            }
            
        }

        

        private void getDate()
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("vi-VN");
            DateTime dateTime;
            string dt = "";
            
            if (connection.IsConnect())
            {
                dt = connection.getEntryDateTime(Table_id);
                
                if (dt != null && dt.Length > 0)
                {
                    dateTime = DateTime.Parse(dt);
                    string dt1 = dateTime.ToString(" HH:mm dd/MM/yyyy");
                    Console.WriteLine("dt1 = " + dt1);
                    lblDate.Text = dt1; 
                }
                string to = connection.getEntryTimeOut(Table_id);
                if (to !="aa" ||to.Length>2)
                {
                    label3.Text = "Đã Thanh Toán";
                    BackColor = Color.Empty;
                    table.DisableButtons();
                }
                
            }
            if (dt == null || dt.Length == 0)
            {
                dateTime = DateTime.Now;
                lblDate.Text = dateTime.ToString(cultureinfo);
            }
            
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
            
            
        }

        private void EntryTableControl_Leave(object sender, EventArgs e)
        {
            //this.BorderStyle = BorderStyle.None;
            
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            Close = true;
            if (this.mergeClick != null)
                this.mergeClick(this, e);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (viewClick != null)
                viewClick(this, e);
            //table.ShowDialog();
            
        }

        private void EntryTableControl_MouseLeave(object sender, EventArgs e)
        {
            
        }
    }
}
