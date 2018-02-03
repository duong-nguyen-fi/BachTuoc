using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinhTien.helper;

namespace TinhTien
{
    [Serializable]
    public class Food
    {
        private string name;
        private double price, total, quantity;
        

        public Food(string name, double quantity, double price, double total)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.Price = price;
            this.Total = total;
        }

        public string Name { get => name; set => name = value; }
        public double Price { get => price; set => price = value; }
        public double Total { get => total; set => total = value; }
        public double Quantity { get => quantity; set => quantity = value; }

        public static string formatPrice (double num)
         {
            string returnStr = "";
            
            if (num %1 ==0)
                returnStr = String.Format("{0:0,0}", num);
            else
                returnStr = String.Format("{0:0,0.000}", num);
            returnStr = returnStr.Replace(',', ' ');

            return returnStr;
         }

        public static List<FoodList> getFoodList(string filePath)
        {
            List<FoodList> list = new List<FoodList>();
            try
            {
                string line;
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(filePath);

                while ((line = file.ReadLine()) != null)
                {
                    //Console.WriteLine("Reading {0}", line);
                    string[] values = line.Split(':');
                    FoodList foodList = new FoodList(values[0], Double.Parse(values[1]));
                    list.Add(foodList);
                }

                
            }
            catch (System.IO.IOException e1)
            {
                System.Windows.Forms.MessageBox.Show("Có lỗi, kiểm tra file: " + filePath);
                
                
            }

         // end of get food list

            return list;
        }
    }


}
