using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace TinhTien
{
    public partial class TableListForm : Form
    {
        List<int> tableListNum;
        List<EntryTableControl> entries;
        helper.DBConnection dBConnection;


        public TableListForm()
        {
            InitializeComponent();
            tableListNum = new List<int>();
            entries = new List<EntryTableControl>();

            //timer to update the datagridview
            

            dBConnection = helper.DBConnection.Instance();
            if (dBConnection.IsConnect())
            {
                dataGridView1.DataSource = dBConnection.currentOrderTable();
                dataGridView1.Invalidate();
            }
            else
            {
                MessageBox.Show("Ko ket noi dc voi du lieu");
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Columns["ID"].Visible = false;

        }



        //add new entryTable
        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmNewTable frmNewTable = new FrmNewTable(tableListNum);
            frmNewTable.ShowDialog();
            int num = frmNewTable.Num;
            long table_id;
            
            if (num >=0)
            if (dBConnection.IsConnect())
            {

                table_id = dBConnection.addToAllTables(num);
                //Console.WriteLine("table_id = {0}", table_id);
                dBConnection.addToCurrentTable(table_id);
                createNewEntryTable(num, true, table_id);
            }
            else
                createNewEntryTable(num, true, -1);




        }

        private void createNewEntryTable(int num, bool isNew, long id)
        {
            tableListNum.Add(num);
            Console.WriteLine("Creating new Entry:  Num = {0} ID={1}", num, id);
            EntryTableControl entryTableControl;
            if (id != -1)
            {
                entryTableControl = new EntryTableControl(num, id);
                entryTableControl.table.isNew = isNew;
            }
            else
            {
                
                entryTableControl = new EntryTableControl(num, -1);
                entryTableControl.table.isNew = isNew;
                
            }


            if (!System.IO.Directory.Exists("datas"))
                System.IO.Directory.CreateDirectory("datas/");

            //entries.Add(entryTableControl);
            entries.Insert(0, entryTableControl);
            flowLayoutPanel1.Controls.Add(entryTableControl);
            flowLayoutPanel1.Controls.SetChildIndex(entryTableControl, 0);
            flowLayoutPanel1.ResumeLayout();
            entryTableControl.deleteClick += EntryTableControl_delete;
            entryTableControl.mergeClick += EntryTableControl_mergeClick;
            entryTableControl.viewClick += EntryTableControl_viewClick;
            entryTableControl.payedEvent += EntryTableControl_payedEvent;

            // int tableNum = entryTableControl.Num;
            int tableNum = num;
            string filePath = "datas/" + tableNum + "_" + id + ".bin";

            if (!System.IO.File.Exists(filePath))
            {
                var myFile = System.IO.File.Create(filePath);
                myFile.Close();
            }

        }

        private void EntryTableControl_payedEvent(object sender, EventArgs e)
        {
            EntryTableControl entry = (EntryTableControl)sender;
            entry.BackColor = Color.Empty;
        }

        private void EntryTableControl_viewClick(object sender, EventArgs e)
        {
            EntryTableControl entry = (EntryTableControl)sender;
            entry.table.ShowDialog();
            if (dBConnection.IsConnect())
                dataGridView1.DataSource = dBConnection.currentOrderTable();
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

                //System.Console.WriteLine("Mon {3}  Source: {0}, Des {1} , Sum: {2}",quan1,quan2,sum, entries[srcPos].table.lblNames[i].Text);
                entries[srcPos].table.nbQuantities[i].Value = Decimal.Parse(sum.ToString());
                
            }

            entries[srcPos].table.writeSerialize();
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
                
                if (dBConnection.IsConnect())
                {
                    long table_id = entries[pos].Table_id;
                    dBConnection.deleteFromCurrentTable(table_id);
                }
                removeEntryAt(pos);
                dataGridView1.DataSource = dBConnection.currentOrderTable();
                
            }
        }

        private void removeEntryAt(int pos)
        {
            int tableNum = entries[pos].Num;
            
                long id = Int64.Parse(entries[pos].Table_id.ToString());
                
                string filePath = "datas/" + tableNum+"_"+ + id+".bin";
                //Console.WriteLine("DELETING FILE: " + filePath);
                System.IO.File.Delete(filePath);
            
            tableListNum.RemoveAt(pos);
            
            EntryTableControl entry = entries[pos];
            entries.RemoveAt(pos);
            flowLayoutPanel1.Controls.Remove(entry);
            
            
        }

        private void TableListForm_Load(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists("datas"))
            {
                System.Console.WriteLine("Folder exists");

                string[] files = System.IO.Directory.GetFiles("datas");
                foreach (string file in files)
                {
                    //System.Console.WriteLine("File: " +file);
                    string[] values = file.Split('.');
                    //System.Console.WriteLine("Value: " + values[0]);
                    string name = values[0].Split('\\')[1];
                    //System.Console.WriteLine("Name: " + name);
                    string[] name_ID = name.Split('_');
                    int num = Int16.Parse(name_ID[0]);
                    long id = Int64.Parse(name_ID[1]);

                    //Console.WriteLine("Table Num: {0}  Table_ID = {1}", num, id);
                    createNewEntryTable(num, false, id);
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateTable_Click(object sender, EventArgs e)
        {
            if (dBConnection.IsConnect())
            {
                dataGridView1.DataSource = dBConnection.currentOrderTable();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
                DialogResult dialogResult = MessageBox.Show("Đánh Dấu Món Này Đã Được Phục Vụ?", "Chú Ý", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                    return;
                else
                {
                    var index = dataGridView1.CurrentRow.Index;
                    var selectedRow = dataGridView1.Rows[index];
                    long idCell = Int64.Parse(selectedRow.Cells["ID"].Value.ToString());
                    //Console.WriteLine("ID of SELECTED ROW: " + idCell);
                    dBConnection.updateOrderServe(idCell);
                    dataGridView1.DataSource = dBConnection.currentOrderTable();
                }
            
        }
    }
}
