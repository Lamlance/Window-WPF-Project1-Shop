using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    internal interface IProductRepository
    {
        bool Add(ProductModel productModel);
        bool Edit(ProductModel productModel);
        bool Remove(int id);
        ProductModel GetById(int id);
        ProductModel GetByModelName(string name);
        IEnumerable<ProductModel> GetByAll();
    }
}
