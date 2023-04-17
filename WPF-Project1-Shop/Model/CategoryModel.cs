using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
