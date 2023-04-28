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

    public Order UpdateOrder(Order order)
    {
      dbContext.Orders.Update(order);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
      return order;
    }

    public IEnumerable<Order>? SearchOrders(DateTime? from, DateTime? to, string? address,string? email,string? phone, double? fromTotal, double? toTotal)
    {
      var result = dbContext.Orders
        .Where(o => 
          (from == null || to == null) ? true : 
          o.CreatedAt >= DateOnly.FromDateTime((DateTime)from) && ( o.CreatedAt <= DateOnly.FromDateTime((DateTime)to)) ||
          (o.UpdatedAt != null && o.UpdatedAt >= DateOnly.FromDateTime((DateTime)from) && o.UpdatedAt <= DateOnly.FromDateTime((DateTime)to) )
        )
        .Where( o =>
          (address == null || email == null || phone == null) ? true :
          ( 
            ( !(string.IsNullOrWhiteSpace(address) || o.ShipAddress == null) && EF.Functions.ILike(o.ShipAddress,$"%{address}%") ) ||
            ( !(string.IsNullOrWhiteSpace(email) || o.Customer == null || o.Customer.Email == null ) && EF.Functions.ILike(o.Customer.Email,email) ) || 
            ( !(string.IsNullOrEmpty(phone) || o.Customer == null ) && EF.Functions.ILike(o.Customer.Phone,phone) )
          )
        )
        .Where( o => (fromTotal == null || toTotal == null) ? true : o.Subtotal >= fromTotal && o.Subtotal <= toTotal)
        .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
        .Include(o => o.Customer)
        .OrderBy(o => o.CreatedAt)
        .Take(500);

      return result;
    }
  }
}
