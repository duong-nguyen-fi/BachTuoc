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
        static string filePath = @"btuoc.txt";
        static int BASE_X = 100, BASE_Y = 50, GAP_X = 100, GAP_Y = 50; // used to position controls on the form
        public List<Food> TableFoodList;
        List<string> foodNames = new List<string>();
        List<double> foodPrices = new List<double>();
        public List<Label> lblNames;
        List<Label> lblPrices;
        public List<NumericUpDown> nbQuantities;
        public List<TextBox> txtcalPrices;
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

            //get the list of food
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
                    Location = new Point(BASE_X + 2 * GAP_X, BASE_Y + GAP_Y * (i + 1)),
                    DecimalPlaces = 1
                };

                nbQuantities.Add(nbQuantity);

                panel1.Controls.Add(nbQuantity);

                calPrice = new TextBox() { Text = "000", Location = new Point(BASE_X + 4 * GAP_X, BASE_Y + GAP_Y * (i + 1)) };
                txtcalPrices.Add(calPrice);
                panel1.Controls.Add(calPrice);


                nbQuantity.ValueChanged += NbQuantity_ValueChanged;

                calPrice.Validated += CalPrice_Validated;

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

        //cause when calculated Price textbox value is changed
        private void CalPrice_Validated(object sender, EventArgs e)
        {
            try
            {
                int pos = txtcalPrices.IndexOf((TextBox)sender);
                int size = ((TextBox)sender).TextLength;
                if (size > 0)
                {
                    double value = Double.Parse(txtcalPrices[pos].Text);
                    updateSum();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Xem lại giá trị vừa nhập");
            }
        }

        private void CalPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            upTableFoodList();
            



//            System.Console.WriteLine("List Size:  {0}", tableFoodList.Count);

 //          System.Console.ReadLine();
        }

        //get all of the food with value > 0
        private void upTableFoodList()
        {
            TableFoodList = new List<Food>();
            for (int i = 0; i < lblPrices.Count; i++)
            {
                double total = Double.Parse(txtcalPrices[i].Text);
                if (total > 0)
                {
                    string name = lblNames[i].Text;
                    double quantity = Double.Parse(nbQuantities[i].Value.ToString());
                    double price = Double.Parse(lblPrices[i].Text);
                    
                    Food mFood = new Food(name, quantity, price, total);
                    TableFoodList.Add(mFood);
                }


            }
        }

        private void Button_Click(object sender, EventArgs e)
        {

            

            //txtSum.Text = sum.ToString();

            upTableFoodList();
            Summary summary = new Summary(TableFoodList, sum, Num);
            //summary.Visible = false;
            summary.ShowDialog();
            //summary.Visible = true;

        }

        // numericupdown control value changed
        private void NbQuantity_ValueChanged(object sender, EventArgs e)
        {
            sum = 0;
            NumericUpDown obj = (NumericUpDown)sender;
            int pos = nbQuantities.IndexOf(obj);
            //System.Console.WriteLine("Position:  {0}", pos);
            double price = Double.Parse(lblPrices[pos].Text);
            //System.Console.WriteLine("Price:  {0}", price);
            double quantity = Double.Parse(nbQuantities[pos].Value.ToString()); 
            //System.Console.WriteLine("Quantity:  {0}", quantity);
            double calPrice = price * quantity;
            txtcalPrices[pos].Text = calPrice.ToString();
            //System.Console.WriteLine("CalPrice:  {0}", calPrice);

            updateSum();
        }

        //update the sum
        void updateSum()
        {
            sum = 0;
            for (int i = 0; i < foodNames.Count; i++)
            {

                double calPrice1 = Double.Parse(txtcalPrices[i].Text);
                sum += calPrice1;

            }
            txtSum.Text = Food.formatPrice(sum);
        }

        //add Food the list of price and name
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
                MessageBox.Show("Có lỗi, kiểm tra file: " + filePath);
                addFood("Bach Tuoc", 18000);
                addFood("Thit Nuong", 10000);
                addFood("Canh Ga", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chan Ga", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chan Ga", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chim Cut", 15000);
                addFood("Chim Cut", 15000);
            }

        } // end of get food list
        
    }
}
