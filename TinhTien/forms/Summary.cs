using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.Drawing.Printing;
using Microsoft.Reporting.WinForms;

namespace TinhTien
{
    public partial class Summary : Form
    {


        int Num;
        double sum;
        string staff;
        List<Food> list;
        DataTable dataTable;
        public Summary(List<Food> mlist, double sum, int num, string staff = " admin")
        {

            list = mlist;
            this.sum = sum;
            this.staff = staff;
            Num = num;
            InitializeComponent();
            
            List<string> printerList = new List<string>();
            dataTable = ConvertListToDataTable(mlist, sum);
            //dataGridView1.DataSource = dataTable;
            //dataGridView1.Visible = true;
           

            dataSet1BindingSource.DataSource = mlist;
            dataGridView1.Refresh();

            
        }

        public Summary()
        {
            InitializeComponent();
        }

        //use for DataGridView -- not in used
        static DataTable ConvertListToDataTable(List<Food> list, double sum)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            

            table.Columns.Add("Món", typeof(string));
            table.Columns.Add("Giá Tiền", typeof(string));
            table.Columns.Add("Số Lượng", typeof(string));
            table.Columns.Add("Thành Tiền", typeof(string));
            
            

            // Add rows.
            foreach (Food food in list)
            {
                //Food food = list[i];
                table.Rows.Add(food.Name, food.Price.ToString(), food.Quantity.ToString(), food.Total.ToString());
            }

            table.Rows.Add("Tổng Cộng", " "," ", sum);

            return table;
        }


        Bitmap bmp;
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = @"D:\bill.pdf";
            

            string fileName = "bill.pdf";
            //string printerName = comboBox1.SelectedItem.ToString();

            //CaptureScreen();
            //printDocument1.Print();

            

        }

        

        //in use
        private void Summary_Load(object sender, EventArgs e)
        {

            // set list as the dataSource named "ds" in RP
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ds", list));
            string msum = Food.formatPrice(sum);
            Microsoft.Reporting.WinForms.ReportParameter[] paras = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("rSum",msum),
                new Microsoft.Reporting.WinForms.ReportParameter("rDate",DateTime.Now.ToString("dd-MM-yy hh:mm")),
                new Microsoft.Reporting.WinForms.ReportParameter("rStaff",staff),
                new ReportParameter("rNum",Num.ToString())
            };
            
            reportViewer1.LocalReport.SetParameters(paras);
            //reportViewer1.PrinterSettings.PrinterName = "Foxit Reader PDF Printer";
            reportViewer1.PrinterSettings.MaximumPage = 1;
            reportViewer1.PrinterSettings.ToPage = 1;
            //reportViewer1.PrinterSettings.PrintRange = PrintRange.CurrentPage;

            //reportViewer1.RefreshReport();
            ////Controls.Add(reportViewer2);
            //this.reportViewer1.PrinterSettings.PrinterName = "Foxit Reader PDF Printer"; 
            //this.reportViewer1.PrinterSettings.MaximumPage = 2;
            //this.reportViewer1.PrinterSettings.FromPage = 1;
            //this.reportViewer1.PrinterSettings.ToPage = 1;
            this.reportViewer1.RefreshReport();
            
        }
#region old_print
        private void printPaper()
        {
            DGVPrinterHelper.DGVPrinter printer = new DGVPrinterHelper.DGVPrinter();
            printer.Title = "Quán 193";
            printer.SubTitle = "Bàn " + Num;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("Envelop", 4000, 3000);
            paperSize.RawKind = (int)PaperKind.Custom;

            DateTime dateTime = DateTime.Now;
            string datrStr = dateTime.ToString("dd-MM _ hh-mm");
            Console.WriteLine("Print something: ");
            //printer.PrintSettings.PrintFileName = Num+ "_" + datrStr + ".pdf";

            Console.ReadLine();
            printer.PorportionalColumns = true;
            printer.Footer = "193 Ung Văn Khiêm phường 25 quận Bình Thạnh";
            printer.FooterSpacing = 15;

            printer.printDocument.DocumentName = "Bàn_" + Num + "_" + datrStr + ".pdf";
            printer.PrintDataGridView(dataGridView1);
            //Console.WriteLine("Print file name: {0}", printer.PrintSettings.PrintFileName);
            this.Close();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = dataGridView1.Size;
            bmp = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(bmp);
            memoryGraphics.CopyFromScreen(new Point(this.Location.X + dataGridView1.Location.X, this.Location.Y + dataGridView1.Location.Y + 30), new Point(dataGridView1.Location.X, dataGridView1.Location.Y), s);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }


        #endregion old_print    

        private void Button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
