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
    public partial class TableListForm : Form
    {
        List<int> tableListNum;
        List<EntryTableControl> entries;
        public TableListForm()
        {
            InitializeComponent();
            tableListNum = new List<int>();
            entries = new List<EntryTableControl>();

            
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmNewTable frmNewTable = new FrmNewTable(tableListNum);
            frmNewTable.ShowDialog();
            int num = frmNewTable.Num;
            tableListNum.Add(num);
            EntryTableControl entryTableControl = new EntryTableControl(num);
            entries.Add(entryTableControl);
            flowLayoutPanel1.Controls.Add(entryTableControl);
            entryTableControl.ButtonClick += EntryTableControl_ButtonClick;
        }

        private void EntryTableControl_ButtonClick(object sender, EventArgs e)
        {
            DialogResult dialogResult= MessageBox.Show("Xóa Bàn Này?", "Chú Ý", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return;
            else
            {
                EntryTableControl entry = (EntryTableControl)sender;
                int pos = entries.IndexOf(entry);
                removeEntryAt(pos);
            }
        }

        private void removeEntryAt(int pos)
        {
            entries.RemoveAt(pos);
            flowLayoutPanel1.Controls.RemoveAt(pos);
        }
    }
}
