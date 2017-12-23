using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RawPrint;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Printing;

namespace TinhTien
{
    public partial class Summary : Form
    {

        int Num;
        public Summary(List<Food> mlist, double sum, int num)
        {
            Num = num;
            InitializeComponent();
            List<string> printerList = new List<string>();
            DataTable dataTable = ConvertListToDataTable(mlist, sum);
            dataGridView1.DataSource = dataTable;
            
            
            //foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    printerList.Add(printerName);

            //}
            //comboBox1.DataSource = printerList;
            
        }

        public Summary()
        {
            InitializeComponent();
        }

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

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = dataGridView1.Size;
            bmp = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(bmp);
            memoryGraphics.CopyFromScreen(new Point(this.Location.X + dataGridView1.Location.X, this.Location.Y+ dataGridView1.Location.Y + 30),new Point(dataGridView1.Location.X, dataGridView1.Location.Y),s );
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void Summary_Load(object sender, EventArgs e)
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

            printer.printDocument.DocumentName ="Bàn_" + Num + "_" + datrStr + ".pdf";
            printer.PrintDataGridView(dataGridView1);
            //Console.WriteLine("Print file name: {0}", printer.PrintSettings.PrintFileName);
            this.Close();
        }
    }
}
