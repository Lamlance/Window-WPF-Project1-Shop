using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class ProductRepository: IDisposable
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

  }
}
