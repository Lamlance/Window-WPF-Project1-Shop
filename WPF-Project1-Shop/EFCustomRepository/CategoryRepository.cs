using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class CategoryRepository:IDisposable
  {
    RailwayContext dbContext;
    public CategoryRepository(RailwayContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public void Dispose()
    {
      dbContext.SaveChanges();
      dbContext.Dispose();
    }

    public IEnumerable<Category> GetAllCategories()
    {
      return dbContext.Categories.
        Include(c => c.Products)
        .Select(c => new Category
        {
          Id = c.Id,
          CategoryName = c.CategoryName,
          Products =  c.Products.Select(p => new Product { Id = p.Id}).ToList()
        })
        .Take(500);
    }

    public Category? AddCategory(Category category)
    {
      dbContext.Categories.Add(category);
      return category;
    }

  }
}
