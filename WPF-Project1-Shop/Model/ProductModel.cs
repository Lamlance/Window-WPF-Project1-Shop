using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string? Image { get; set; }

        public string? Name { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int? CategoryId { get; set; }

        public virtual CategoryModel? CategoryModel { get; set; }
    }
}
