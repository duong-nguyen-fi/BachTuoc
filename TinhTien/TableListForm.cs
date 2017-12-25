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

        //add new entryTable
        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmNewTable frmNewTable = new FrmNewTable(tableListNum);
            frmNewTable.ShowDialog();
            int num = frmNewTable.Num;
            tableListNum.Add(num);
            EntryTableControl entryTableControl = new EntryTableControl(num);
            entries.Add(entryTableControl);
            flowLayoutPanel1.Controls.Add(entryTableControl);
            entryTableControl.deleteClick += EntryTableControl_delete;
            entryTableControl.mergeClick += EntryTableControl_mergeClick;
        }

        private void EntryTableControl_mergeClick(object sender, EventArgs e)
        {
            EntryTableControl entry = (EntryTableControl)sender;
            int pos = entries.IndexOf(entry);
            
            if (entries.Count > 1)
            {
                int srcNum = entries[pos].Num;
                
                FrmMerge frmMerge = new FrmMerge(srcNum, tableListNum);
                frmMerge.ShowDialog();
                int desNum = frmMerge.DesNum;
                
                int srcPos = tableListNum.IndexOf(srcNum);
                int desPos = tableListNum.IndexOf(desNum);
                mergeTables(srcPos, desPos);
            }
        }

        private void mergeTables(int srcPos, int desPos)
        {
            Console.WriteLine("Source: " + srcPos + "Des: " + desPos);
            for (int i=0;i<entries[srcPos].table.lblNames.Count; i++)
            {
                double quan1 = Double.Parse(entries[srcPos].table.nbQuantities[i].Value.ToString());
                double quan2 = Double.Parse(entries[desPos].table.nbQuantities[i].Value.ToString());
                double sum = quan1 + quan2;
                entries[srcPos].table.nbQuantities[i].Value = Decimal.Parse(sum.ToString());

                Console.WriteLine("Quan1 = {0} + Quan2 = {1} Sum = {2}", quan1, quan2, sum);
               
            }
            removeEntryAt(desPos);
            
        }

        private void EntryTableControl_delete(object sender, EventArgs e)
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
            tableListNum.RemoveAt(pos);
            entries.RemoveAt(pos);
            flowLayoutPanel1.Controls.RemoveAt(pos);
        }
    }
}
