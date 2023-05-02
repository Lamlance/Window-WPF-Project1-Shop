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

        public IEnumerable<Product> GetManyProducts(int page = 1, int limit = 500)
        {
            var products = dbContext.Products
              .Include(p => p.Categories)
              .Skip(page > 0 ? page - 1 : 0)
              .Take(limit);
            return products;
        }

        public Product AddProduct(Product product)
        {
            List<Category> needAddingCategories = new List<Category>(product.Categories.ToList());
            product.Categories.Clear();
            dbContext.Products.Add(product);

            if (needAddingCategories.Count > 0)
            {
                dbContext.SaveChanges();
                var updatedProduct = dbContext.Products.SingleOrDefault(p => p.Id == product.Id);
                if (updatedProduct != null)
                {
                    var categories = dbContext.Categories
                      .Include(c => c.Products)
                      .Where(c => needAddingCategories.Contains(c));
                    foreach (Category category in categories)
                    {
                        category.Products.Add(updatedProduct);
                    }
                    dbContext.Categories.UpdateRange(categories);
                }

            }

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

            if (queryProduct != null)
            {
                HashSet<Category> oldCategories = new HashSet<Category>(queryProduct.Categories.ToList());

                if (!oldCategories.Equals(newCategories))
                {
                    foreach (var category in queryProduct.Categories.Where(c => newCategories.Contains(c) == false).ToList())
                    {
                        queryProduct.Categories.Remove(category);
                    }
                    dbContext.SaveChanges();
                }
                foreach (var c in newCategories)
                {
                    if (oldCategories.Contains(c) == false)
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

        public Product? RemoveProduct(Product product)
        {
            var deletingProduct = dbContext.Products
              .Include(p => p.OrderItems)
              .Include(p => p.Categories)
              .SingleOrDefault(p => product.Id == p.Id);

            if (deletingProduct != null)
            {
                foreach (var category in deletingProduct.Categories.ToList())
                {
                    deletingProduct.Categories.Remove(category);
                }

                foreach (var oi in deletingProduct.OrderItems.ToList())
                {
                    deletingProduct.OrderItems.Remove(oi);
                }

                dbContext.SaveChanges();

                dbContext.Products.Remove(deletingProduct);
                return deletingProduct;
            }
            return null;
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
