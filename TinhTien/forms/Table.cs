using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using TinhTien.helper;


namespace TinhTien
{


    public partial class Table : Form
    {
        private string filePath;

        static int BASE_X = 100, BASE_Y = 50, GAP_X = 100, GAP_Y = 50; // used to position controls on the form


        public List<Food> TableFoodList = null;
        public List<Food> savedFoodList = null;
        public event EventHandler onPayedEvent = null;
        public bool isNew = true; 
        List<string> foodNames = new List<string>();
        List<double> foodPrices = new List<double>();
        public List<Label> lblNames;
        List<Label> lblPrices;
        public List<NumericUpDown> nbQuantities;
        public List<TextBox> txtcalPrices;
        
        private double sum;
        int Num;
        string serializeFile;
        DBConnection dBConnection;
        public long table_id;




        public Table(int num, long id)
        {

            //if debug
            filePath  = "../../Resources/btuoc.txt";
            if (!System.IO.File.Exists(filePath))
            {
                //if publish
                filePath = "Resources/btuoc.txt";
                //MessageBox.Show("File Not Exists");
            }

            table_id = id;
            //Console.WriteLine("Table id from Table:" + id);
            InitializeComponent();
            Num = num;
            serializeFile = "datas/" + Num +"_"+table_id+ ".bin";
            lblNum.Text = "Bàn " + num;
            LoginForm loginForm = new LoginForm();
            //loginForm.ShowDialog();

            //get the list of food
            getFoodList();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            InitComp();
        }

        private void InitComp()
        {
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

                nbQuantity.MouseWheel += NbQuantity_MouseWheel;
                nbQuantity.ValueChanged += NbQuantity_ValueChanged;

                calPrice.Validated += CalPrice_Validated;

                nbQuantity = null;


            } // end of for

            dBConnection = DBConnection.Instance();
            // create new table in DB


            //Button button = new Button() { Text = "Thanh Toán", Location = new Point(BASE_X + 4 * GAP_X, BASE_Y + GAP_Y * (foodNames.Count + 1)), Size = new Size(new Point(92, 60)), Font = new Font(Font.FontFamily, 15), };
            //panel1.Controls.Add(button);
            button.Click += Button_Click;

            //txtSum = new TextBox()
            //{
            //    Size = new Size(new Point(92, 35)),
            //    Font = new Font(Font.FontFamily, 13),
            //    Location = new Point(BASE_X + 4 * GAP_X, 20 + BASE_Y + GAP_Y * (foodNames.Count + 2))

            //};
            //panel1.Controls.Add(txtSum);

            //Button btnSave = new Button()
            //{ Text = "Lưu",
            //    Size = new Size(new Point(92, 45)),
            //    Font = new Font(Font.FontFamily, 15),
            //    Location = Location = new Point(BASE_X + 3 * GAP_X, BASE_Y + GAP_Y * (foodNames.Count + 1))

            //};
            //panel1.Controls.Add(btnSave);
            btnSave.Click += BtnSave_Click;
        }

        private void NbQuantity_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        //cause when calculated Price textbox value is changed - update the total sum
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

        private void BtnSave_Click(object sender, EventArgs e)
        {

            upTableFoodList();
            writeSerialize();
            
            saveToDB();
            UpdateDataGridView();
            //this.Close();
            MessageBox.Show("Đã Lưu");

        }

        public void writeSerialize()
        {
            upTableFoodList();
            try
            {

                using (Stream stream = File.Open(serializeFile, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, TableFoodList);
                }
                
                isNew = false;
            }
            catch (IOException e1)
            {
                Console.WriteLine(e1.ToString());
            }
        }

        //save data to the db
        private void saveToDB()
        {
            DataGridViewRowCollection rows = dataGridView1.Rows;
            foreach (Food item in TableFoodList)
            {
                string fname = item.Name;
                bool isOldNotServed = false;
                
                bool isnewItem = true;
                double totalQuan = item.Quantity;
                double sumQuan = 0;
                long id =-1;
                for (int i=0; i<rows.Count-1;i++)
                {
                    DataGridViewRow row = rows[i];
                    string mname = row.Cells[0].Value.ToString();
                    double quan = Double.Parse(row.Cells[1].Value.ToString());
                    string mserved = row.Cells[3].Value.ToString();
                    long mid = Int64.Parse(row.Cells[4].Value.ToString());

                    if (fname == mname)
                    {
                       // Console.WriteLine("Found {0}-{1}",fname,mname);
                        isnewItem = false;
                        
                        if (mserved.Trim().Length == 0 || mserved.Trim() == "")
                        {
                            id = mid;
                            isOldNotServed = true;
                      //      Console.WriteLine("Found {0} not serve", fname);
                            break;
                        }
                        else
                        {
                            sumQuan += quan;
                        }
                    }
                                            
                }//end of for

                int pos = foodNames.IndexOf(fname);
                nbQuantities[pos].Minimum = Decimal.Parse(sumQuan.ToString());

                if (isnewItem)
                {
                    //Console.WriteLine("Adding to DB" + fname);
                    dBConnection.addtoOrder(fname, totalQuan, item.Total, table_id);
                }
                else if (isOldNotServed)
                {
                    if (totalQuan > sumQuan)
                    {
                      //  Console.WriteLine("isOldNotServed" + fname);
                        dBConnection.updateOrder(id, totalQuan - sumQuan);
                    }
                    if (totalQuan == sumQuan)
                    {
                        dBConnection.deleteOrder(id);
                    }
                }
                else
                {
                    //Console.WriteLine("Else " + fname);

                    double quan = totalQuan - sumQuan;
                    double price = item.Total - item.Price * quan;
                    if (quan>0)
                    dBConnection.addtoOrder(fname, quan, price, table_id);
                }

            }//endof foreach
            
        }

        

        private void UpdateDataGridView()
        {
            if (dBConnection.IsConnect())
            {
                try
                {
                    dataGridView1.DataSource = dBConnection.thisTableOrders(table_id);
                    
                }
                catch (SocketException e)
                {
                    MessageBox.Show("Kết Nối Bị Lỗi - Thử Lại Sau");
                }
            }
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
            writeSerialize();
            Summary summary = new Summary(TableFoodList, sum, Num);
            //summary.Visible = false;
            summary.ShowDialog();
            //summary.Visible = true;

            DialogResult dialogResult = MessageBox.Show("Bàn Này Đã Được Thanh Toán?", "Chú Ý", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return;
            else
            {
                if (onPayedEvent != null)
                {
                    if (dBConnection.IsConnect())
                        dBConnection.updateTableTimeOutAndSum(table_id, sum);
                    onPayedEvent(this, e);
                    DisableButtons();
                    
                    this.Close();
                }
            }
        }

        public void DisableButtons()
        {
            button.Enabled = false;
            btnSave.Enabled = false;
            button1.Enabled = false;
            btnEdit.Visible = true;
        }

        public void EnableButtons()
        {
            button.Enabled = true;
            btnSave.Enabled = true;
            button1.Enabled = true;
            btnEdit.Visible = false;
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
            if (calPrice >0)
                txtcalPrices[pos].Text = calPrice.ToString();
            //System.Console.WriteLine("CalPrice:  {0}", calPrice);
            else
                if (calPrice==0)
                txtcalPrices[pos].Text = "000";

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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Xóa món này khỏi danh sách?", "Chú Ý", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return;
            else
            {
                var index = dataGridView1.CurrentRow.Index;
                var selectedRow = dataGridView1.Rows[index];
                long idCell = Int64.Parse(selectedRow.Cells["ID"].Value.ToString());
                Console.WriteLine("ID of SELECTED ROW: " + idCell);
                dBConnection.deleteOrder(idCell);
                dataGridView1.DataSource = dBConnection.thisTableOrders(table_id);
            }
        }

        private void button_Click_1(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            
                EnableButtons();
            
        }

        //add Food the list of price and name
        void addFood(string name, double price)
        {
            foodNames.Add(name);
            foodPrices.Add(price);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (!isNew)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(serializeFile);
                    if (fileInfo.Exists && fileInfo.Length > 0)
                        using (Stream stream = File.Open(serializeFile, FileMode.Open))
                        {
                            BinaryFormatter bin = new BinaryFormatter();

                            TableFoodList = (List<Food>)bin.Deserialize(stream);
                            foreach (Food food in TableFoodList)
                            {
                                int pos = foodNames.IndexOf(food.Name);
                                if (pos != -1)
                                {
                                    //Console.WriteLine("DESERIALIZING: Pos = {0}", pos);
                                    decimal value = Decimal.Parse(food.Quantity.ToString());
                                    //Console.WriteLine("DESERIALIZING: Pos = {0} Name={1} Value ={2} ", pos, lblNames[pos], value);
                                    nbQuantities[pos].Value = value;
                                }
                            }
                            savedFoodList = TableFoodList;
                        }
                    UpdateDataGridView();
                    dataGridView1.Columns["id"].Visible = false;
                }
                catch (IOException)
                {
                }
                
            }
            
        }

        void getFoodList()
        {
            List<helper.FoodList> list = Food.getFoodList(filePath);
            foreach (helper.FoodList item in list)
            {
                addFood(item.Name, item.Price);
            }
                

        }
        
    }
}
