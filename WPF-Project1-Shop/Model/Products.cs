using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    public class Product
    {
        private int _id;
        private string? _imagePath;
        private string? _brand;
        private string? _productName;
        private decimal _price;
        private int _quantity;

        public int Id { get => _id; set => _id = value; }
        public string? ImagePath { get => _imagePath; set => _imagePath = value; }
        public string? Brand { get => _brand; set => _brand = value; }
        public string? ProductName { get => _productName; set => _productName = value; }
        public decimal Price { get => _price; set => _price = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }

        //public int Id { get; set; }
        //public string? ImagePath { get; set; }
        //public string? Brand { get; set; }
        //public string? ProductName { get; set; }
        //public decimal Price { get; set; }
        //public int Quantity { get; set; }
    }

}
