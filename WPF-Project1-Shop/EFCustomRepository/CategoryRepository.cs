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

    public Category? UpdateCategory(Category category)
    {
      dbContext.Categories.Update(category);
      return category;
    }

    public IEnumerable<Category>? SearchCategories(string? name)
    {
      var result = dbContext.Categories.
        Include(c => c.Products)
        .Select(c => new Category
        {
          Id = c.Id,
          CategoryName = c.CategoryName,
          Products = c.Products.Select(p => new Product { Id = p.Id }).ToList()
        })
        .Where(c =>
          (name == null) ? true :
          (
            (!(string.IsNullOrEmpty(name) || c.CategoryName == null) && EF.Functions.ILike(c.CategoryName, $"%{name}%"))
          )
        )
        .Take(500);
      return result;
    }

    public Category? DeleteCategory(Category category)
    {
      var products = dbContext.Products
        .Include(p => p.Categories)
        .Where(p => p.Categories.Any(c => c.Id == category.Id)).ToList();
      if(products.Count() > 0)
      {
        for(int i = 0; i < products.Count(); i++)
        {
          products[i].Categories.Remove(category);
        }
        dbContext.UpdateRange(products);
        dbContext.SaveChanges();
      }

      var deleteCategory = dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
      if(deleteCategory != null)
      {
        dbContext.Categories.Remove(deleteCategory);
        return deleteCategory;
      }

      return null;
    }
  }
}
