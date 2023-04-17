using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop.Repository
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public bool Add(ProductModel productModel)
        {
            throw new NotImplementedException();
        }

        public bool Edit(ProductModel productModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public ProductModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ProductModel GetByModelName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
