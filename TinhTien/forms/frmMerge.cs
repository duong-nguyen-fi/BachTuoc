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
    public partial class FrmMerge : Form
    {
        public int SrcNum, DesNum;
        private List<int> mList = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DesNum = Int16.Parse(comboBox1.SelectedItem.ToString());
                this.Close();
            }
            catch (Exception e1)
            { }
            
            
        }

        public FrmMerge(int srcNum, List<int> list )
        {
            InitializeComponent();
            SrcNum = srcNum;
            //mList = list;
            //MessageBox.Show(list.Count + " " );
            //mList.Remove(srcNum);
            label2.Text = "" + SrcNum;
            foreach (int i in list)
            {
                if (i != SrcNum)
                    mList.Add(i);
            }
            comboBox1.DataSource = mList;
        }
    }
}
