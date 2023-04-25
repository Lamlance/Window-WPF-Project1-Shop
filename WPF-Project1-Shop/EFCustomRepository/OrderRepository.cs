using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class OrderRepository : IDisposable
  {
    private RailwayContext dbContext;
    public OrderRepository(RailwayContext railwayContext)
    {
      dbContext = railwayContext;
    }

    public void Dispose()
    {
      dbContext.SaveChanges();
      dbContext.Dispose();
    }

    public IEnumerable<Order> GetManyOrders(int page = 1)
    {
      var orders = dbContext.Orders
        .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
        .Include(o => o.Customer)
        .OrderBy(o => o.CreatedAt)
        .Skip(page > 0 ? page - 1 : 0)
        .Take(500);
      return orders;
    }
    public Order AddOrder(Order data)
    {
      dbContext.Add(data);
      return data;
    }
  }
}
