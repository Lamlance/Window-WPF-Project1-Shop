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
      
      var curOrder = dbContext.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == order.Id);
      if (curOrder != null && !curOrder.OrderItems.Equals(order.OrderItems.ToList())) 
      {
        var orderItemOldInOrder =  new HashSet<OrderItem>(dbContext.OrderItems.Include(oi => oi.Product).Where(oi => oi.OrderId == order.Id).ToList());
        var orderItemNewInOrder = new List<OrderItem>(order.OrderItems.ToList());

        List<OrderItem> updateOrder = orderItemNewInOrder.Intersect(orderItemOldInOrder).ToList();

        //Remove
        foreach (var oldOderItem in orderItemOldInOrder)
        {
          if (!order.OrderItems.Contains(oldOderItem))
          {
            orderItemOldInOrder.Remove(oldOderItem);
          }
          else
          {
            var newOi = updateOrder.Find(o => o.Id == oldOderItem.Id);
            if (newOi != null && newOi.Quantity == oldOderItem.Quantity && newOi.Price == oldOderItem.Price && newOi.CreatedAt == oldOderItem.CreatedAt)
            {
              updateOrder.Remove(newOi);
            }
            else if(newOi != null)
            {
              orderItemOldInOrder.Add(newOi);
            }
          }
        }
        //Add

        foreach (var newOrderItem in orderItemNewInOrder)
        {
          if (newOrderItem.Id == 0)
          {
            newOrderItem.Product = null;
            dbContext.OrderItems.Add(newOrderItem);
          }
          
        }

        //Edit
        

        //Edit
        dbContext.UpdateRange(orderItemOldInOrder);

        dbContext.SaveChanges();
      }

      if(curOrder != null )
      {
        bool isDif = false;
        if(curOrder.Status != order.Status)
        {
          isDif = true;
          curOrder.Status = order.Status;
        }
        if( curOrder.Subtotal != order.Subtotal)
        {
          isDif = true;
          curOrder.Subtotal = curOrder.Subtotal;
        }
        if(curOrder.CreatedAt != order.CreatedAt)
        {
          isDif = true;
          curOrder.CreatedAt = order.CreatedAt;
        }
        if(curOrder.UpdatedAt != order.UpdatedAt)
        {
          isDif = true;
          curOrder.UpdatedAt = order.UpdatedAt;
        }
        if (isDif)
        {
          dbContext.Orders.Update(curOrder);
        }
      }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
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
