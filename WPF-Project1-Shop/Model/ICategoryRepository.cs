using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    public interface ICategoryRepository
    {
        bool Add(CategoryModel categoryModel);
        bool Edit(CategoryModel categoryModel);
        bool Remove(int id);
        CategoryModel GetById(int id);
        CategoryModel GetByModelName(string name);
        IEnumerable<CategoryModel> GetByAll();
    }
}
