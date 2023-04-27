using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        .Skip(page > 0 ? page - 1 : 0)
        .Take(500);
      return products;
    }

    public Product AddProduct(Product product)
    {
      dbContext.Products.Add(product);
      return product;
    }

    public IEnumerable<Product>? SearchProduct(IEnumerable<Category>? categories, double? from, double? to, string? name)
    {
      /*
      foreach (Category c in categories)
      {

        longs.Add(c.Id);

      }

      var cates = dbContext.Categories
        .Where(c => longs.Contains(c.Id))
        .Where(c => c.Products.Count > 0)
        .Where(c => c.Products.Any(p => p.ProductName.Contains("Season") ) )
        .Include(c => c.Products).ToList();
      */
      HashSet<long> longs = new HashSet<long>();
      if(categories != null)
      {
        foreach(Category c in categories)
        {
          longs.Add(c.Id);
        }
      }
      int cateSize = longs.Count;

      var product = dbContext.Products
        .Where(p => (cateSize <= 0) || p.Categories.Any(c => longs.Contains(c.Id) ))
        .Where(p => (from == null || to == null) || (p.Price >= from && p.Price <= to))
        .Where(p => name == null || EF.Functions.ILike(p.ProductName,$"%{name}%") )
        .OrderBy(p => p.Id)
        .Take(500).ToList();
      //int size = product.Count();
      return product;
    }
  }
}
