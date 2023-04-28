using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class ProductRepository : IDisposable
  {
    private RailwayContext dbContext;
    public ProductRepository(RailwayContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public void Dispose()
    {
      dbContext.SaveChanges();
      dbContext.Dispose();
    }

    public IEnumerable<Product> GetManyProducts(int page = 1)
    {
      var products = dbContext.Products
        .Include(p => p.Categories)
        .Skip(page > 0 ? page - 1 : 0)
        .Take(500);
      return products;
    }

    public Product AddProduct(Product product)
    {
      dbContext.Products.Add(product);
      return product;
    }
    public List<Product> AddManyProduct(List<Product> products)
    {
      dbContext.Products.AddRange(products);
      return products;
    }

    public Product UpdateProduct(Product product)
    {
      HashSet<Category> newCategories = new HashSet<Category>(product.Categories.ToList());
      var queryProduct = dbContext.Products.Include(p => p.Categories).SingleOrDefault(p => p.Id == product.Id);

      if(queryProduct != null)
      {
        HashSet<Category> oldCategories = new HashSet<Category>(queryProduct.Categories.ToList());

        if (!oldCategories.Equals(newCategories))
        {
          foreach (var category in queryProduct.Categories.Where(c => newCategories.Contains(c) == false ).ToList())
          {
            queryProduct.Categories.Remove(category);
          }
          dbContext.SaveChanges();
        }
        foreach(var c in newCategories)
        {
          if(oldCategories.Contains(c) == false)
          {
            queryProduct.Categories.Add(c);
          }
        }
        queryProduct.ProductName = product.ProductName;
        queryProduct.ImagePath = product.ImagePath;
        queryProduct.CreatedAt = product.CreatedAt;
        queryProduct.Numbers = product.Numbers;

        dbContext.Products.Update(queryProduct);
      }
      return product;
    }

    public IEnumerable<Product>? SearchProduct(IEnumerable<Category>? categories, double? from, double? to, string? name)
    {
      HashSet<long> longs = new HashSet<long>();
      if (categories != null)
      {
        foreach (Category c in categories)
        {
          longs.Add(c.Id);
        }
      }
      int cateSize = longs.Count;

      var product = dbContext.Products
        .Include(p => p.Categories)
        .Where(p => (cateSize <= 0) || p.Categories.Any(c => longs.Contains(c.Id)))
        .Where(p => (from == null || to == null) || (p.Price >= from && p.Price <= to))
        .Where(p => name == null || EF.Functions.ILike(p.ProductName, $"%{name}%"))
        .OrderBy(p => p.Id)
        .Take(500).ToList();
      //int size = product.Count();
      return product;
    }
  }
}
