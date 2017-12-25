using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhTien
{

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
    }
}
