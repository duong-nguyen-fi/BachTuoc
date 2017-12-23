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
    public partial class FrmNewTable : Form
    {
        public List<int> mList;
        public int Num;
        public FrmNewTable(List<int> numList)
        {
            InitializeComponent();
            mList = numList;
            this.ActiveControl = textBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Num = Int16.Parse(textBox1.Text);
                if (mList.Contains(Num))
                    MessageBox.Show("Số này đã tồn tại!!");
                else
                {

                    this.Close();

                }
            }
            catch (Exception e1)
            { }
        }

        
    }
}
