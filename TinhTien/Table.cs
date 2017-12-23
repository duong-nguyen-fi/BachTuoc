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


    public partial class Table : Form
    {
        static string filePath = @"d:\btuoc.txt";
        static int BASE_X = 100, BASE_Y = 50, GAP_X = 100, GAP_Y = 50;
        List<Food> tableFoodList;
        List<string> foodNames = new List<string>();
        List<double> foodPrices = new List<double>();
        List<Label> lblNames;
        List<Label> lblPrices;
        List<NumericUpDown> nbQuantities;
        List<TextBox> txtcalPrices;
        TextBox txtSum;
        private double sum;
        int Num;
        public Table(int num)
        {
            InitializeComponent();
            Num = num;
            lblNum.Text = "Bàn " + num;
            LoginForm loginForm = new LoginForm();
            //loginForm.ShowDialog();

            getFoodList();
            
            lblNames = new List<Label>();
            lblPrices = new List<Label>();
            nbQuantities = new List<NumericUpDown>();
            txtcalPrices = new List<TextBox>();
            NumericUpDown nbQuantity;
            TextBox calPrice;
            for (int i = 0; i < foodNames.Count; i++)
            {
                Label lblName = new Label()
                {
                    Text = foodNames[i],
                    Location = new Point(BASE_X, BASE_Y + GAP_Y * (i + 1))
                };
                lblNames.Add(lblName);

                panel1.Controls.Add(lblName);

                Label lblPrice = new Label()
                {
                    Text = foodPrices[i].ToString(),
                    Location = new Point(BASE_X + GAP_X, BASE_Y + GAP_Y * (i + 1))
                };

                lblPrices.Add(lblPrice);

                panel1.Controls.Add(lblPrice);

                nbQuantity = new NumericUpDown()
                {
                    //Text = foodPrices[i].ToString(),
                    Location = new Point(BASE_X + 2 * GAP_X, BASE_Y + GAP_Y * (i + 1))
                };

                nbQuantities.Add(nbQuantity);

                panel1.Controls.Add(nbQuantity);

                calPrice = new TextBox() { Text = "0", Location = new Point(BASE_X + 4 * GAP_X, BASE_Y + GAP_Y * (i + 1)) };
                txtcalPrices.Add(calPrice);
                panel1.Controls.Add(calPrice);


                nbQuantity.ValueChanged += NbQuantity_ValueChanged;


                nbQuantity = null;
            }

            Button button = new Button() { Text = "Thanh Toán", Location = new Point(BASE_X + 4 * GAP_X, BASE_Y + GAP_Y * (foodNames.Count + 1)) };
            panel1.Controls.Add(button);
            button.Click += Button_Click;

            txtSum = new TextBox() { Location = new Point(BASE_X + 4 * GAP_X, BASE_Y + GAP_Y * (foodNames.Count + 2)) };
            panel1.Controls.Add(txtSum);
            Button btnPrint = new Button()
            { Text = "Lưu", Location = Location = new Point(BASE_X + 3 * GAP_X, BASE_Y + GAP_Y * (foodNames.Count + 1)) };
            panel1.Controls.Add(btnPrint);
            btnPrint.Click += BtnPrint_Click;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            upTableFoodList();
            



//            System.Console.WriteLine("List Size:  {0}", tableFoodList.Count);

 //          System.Console.ReadLine();
        }

        private void upTableFoodList()
        {
            tableFoodList = new List<Food>();
            for (int i = 0; i < lblPrices.Count; i++)
            {
                double total = Double.Parse(txtcalPrices[i].Text);
                if (total > 0)
                {
                    string name = lblNames[i].Text;
                    int quantity = Int16.Parse(nbQuantities[i].Value.ToString());
                    double price = Double.Parse(lblPrices[i].Text);
                    
                    Food mFood = new Food(name, quantity, price, total);
                    tableFoodList.Add(mFood);
                }


            }
        }

        private void Button_Click(object sender, EventArgs e)
        {

            

            //txtSum.Text = sum.ToString();

            upTableFoodList();
            Summary summary = new Summary(tableFoodList, sum, Num);
            //summary.Visible = false;
            summary.ShowDialog();
            //summary.Visible = true;

        }

        private void NbQuantity_ValueChanged(object sender, EventArgs e)
        {
            sum = 0;
            NumericUpDown obj = (NumericUpDown)sender;
            int pos = nbQuantities.IndexOf(obj);
            //System.Console.WriteLine("Position:  {0}", pos);
            double price = Double.Parse(lblPrices[pos].Text);
            //System.Console.WriteLine("Price:  {0}", price);
            int quantity = Int16.Parse(nbQuantities[pos].Value.ToString()); 
            //System.Console.WriteLine("Quantity:  {0}", quantity);
            double calPrice = price * quantity;
            txtcalPrices[pos].Text = calPrice.ToString();
            //System.Console.WriteLine("CalPrice:  {0}", calPrice);

            for (int i = 0; i < foodNames.Count; i++)
            {

                double calPrice1 = Double.Parse(txtcalPrices[i].Text);
                sum += calPrice1;
                
            }
            txtSum.Text = sum.ToString();
        }

        void addFood(string name, double price)
        {
            foodNames.Add(name);
            foodPrices.Add(price);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void getFoodList()
        {
            //addFood("Bach Tuoc", 18000);
            //addFood("Thit Nuong", 10000);
            //addFood("Canh Ga", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chan Ga", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chan Ga", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chim Cut", 15000);
            //addFood("Chim Cut", 15000);

            int counter = 0;
            string line;
            try
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    string[] values = line.Split(':');
                    addFood(values[0], Double.Parse(values[1]));
                    counter++;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Có lỗi, kiểm tra file: " +filePath );
            }
        } // end of get food list
        
    }
}
