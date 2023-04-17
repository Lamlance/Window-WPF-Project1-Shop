using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop.Repository
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public bool Add(CategoryModel categoryModel)
        {
            throw new NotImplementedException();
        }

        public bool Edit(CategoryModel categoryModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoryModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public CategoryModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public CategoryModel GetByModelName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
