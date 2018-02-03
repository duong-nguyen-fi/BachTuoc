using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinhTien.forms
{
    public partial class FrmReview : Form
    {
        
        helper.DBConnection dBConnection;
        public FrmReview()
        {
            
            InitializeComponent();
            
            
            dBConnection = helper.DBConnection.Instance();
            //dateTimePicker1.MinDate = DateTime.Now.AddDays(-30);
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            
            
            dateTimePicker1.MaxDate = DateTime.Now;
            dateTimePicker2.MaxDate = DateTime.Now;
        }
        //new Name BtnShowByTable
        private void btnShowByFood_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null)
            {
                var index = dataGridView1.CurrentRow.Index;
                var selectedRow = dataGridView1.Rows[index];
                var num = Int64.Parse(selectedRow.Cells[0].Value.ToString());
                label1.Text = "Bàn Số " + num; 
                long idCell = Int64.Parse(selectedRow.Cells["ID"].Value.ToString());
                //Console.WriteLine("ID of SELECTED ROW: " + idCell);

                if (dBConnection.IsConnect())
                {

                    dataGridView2.DataSource = dBConnection.thisTableReview(idCell);
                    dataGridView2.Visible = true;
                }
            }
        }

        private void btnShowByDate_Click(object sender, EventArgs e)
        {
            string date1 = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            string date2 = dateTimePicker2.Value.ToString("dd/MM/yyyy");
            if (dBConnection.IsConnect())
            {
                //var foodName = comboBox1.SelectedText;
                dataGridView1.DataSource = dBConnection.TableByDate(date1, date2);
                dataGridView1.Columns["ID"].Visible = false;
                
                if (dataGridView1.Rows.Count > 0)
                {
                    MessageBox.Show("Lấy Kết Quả Thành Công");
                    btnShowByTableNum.Visible = true;
                    dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
                }
                else
                {
                    MessageBox.Show("Không Tìm Thấy Bàn Nào");
                }
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnShowByFood_Click(sender, e);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = dateTimePicker2.Value;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count<1)
            {
                btnShowByTableNum.Visible = false;
            }
        }
    }
}
