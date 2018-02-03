using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinhTien.forms;

namespace TinhTien
{
    public partial class FrmWelcome : Form
    {
        public FrmWelcome()
        {
            InitializeComponent();
        }

        private void btnTableList_Click(object sender, EventArgs e)
        {
            TableListForm tableListForm = new TableListForm();
            //this.Visible = false;
            tableListForm.ShowDialog();
            //this.Close();
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            FrmReview frmReview = new FrmReview();
            frmReview.ShowDialog();
        }
    }
}
