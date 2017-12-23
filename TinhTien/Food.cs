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
        private  double price, total;
        private int quantity;

        public Food(string name, int quantity, double price, double total)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.Price = price;
            this.Total = total;
        }

        public string Name { get => name; set => name = value; }
        public double Price { get => price; set => price = value; }
        public double Total { get => total; set => total = value; }
        public int Quantity { get => quantity; set => quantity = value; }

    }
}
