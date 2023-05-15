using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class ReportRepository : BaseCustomRepository
  {
    //RailwayContext dbContext;
    public ReportRepository(RailwayContext dbContext):base(dbContext)
    {
      //this.dbContext = dbContext;
    }

    public class OrderSumProfitGroupByTime
    {
      int date = 0;
      int month = 0;
      int year = 0;
      double sum = 0;

      public int Date { get => date; set => date = value; }
      public int Month { get => month; set => month = value; }
      public int Year { get => year; set => year = value; }
      public double Sum { get => sum; set => sum = value; }
    }

    public class OrderItemProductCountGroupByTime
    {
      int date = 0;
      int month = 0;
      int year = 0;
      string productName = "";
      int count = 0;

      public int Date { get => date; set => date = value; }
      public int Month { get => month; set => month = value; }
      public int Year { get => year; set => year = value; }
      public string ProductName { get => productName; set => productName = value; }
      public int Count { get => count; set => count = value; }
    }

    public List<OrderSumProfitGroupByTime>? OrderSubTotalByDate(DateOnly fromDate,DateOnly toDate)
    {
      var orderGrpByDate = dbContext.Orders
        .Select(o => new
        {
          o.CreatedAt,
          o.Subtotal
        })
        .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
        .GroupBy(o => o.CreatedAt)
        .Select(o => new OrderSumProfitGroupByTime
        {
          Date = o.Key.Day,
          Month = o.Key.Month,
          Year = o.Key.Year,
          Sum = o.Sum(g => g.Subtotal)
        }).ToList();
      return orderGrpByDate;
    }
    public List<OrderSumProfitGroupByTime>? OrderSubTotalByMonth(DateOnly fromDate, DateOnly toDate)
    {
      var orderGrpByMonth = dbContext.Orders
        .Select(o => new
        {
          o.CreatedAt,
          o.Subtotal
        })
        .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
        .GroupBy(o => new
        {
          MONTH = o.CreatedAt.Month,
          YEAR = o.CreatedAt.Year,
        })
        .Select(o => new OrderSumProfitGroupByTime
        {
          Month = o.Key.MONTH,
          Year = o.Key.YEAR,
          Sum = o.Sum(g => g.Subtotal)
        }).ToList();
      return orderGrpByMonth;
    }
    public List<OrderSumProfitGroupByTime>? OrderSubTotalByYear(DateOnly fromDate, DateOnly toDate)
    {
      var orderGrpByYear = dbContext.Orders
        .Select(o => new
        {
          o.CreatedAt,
          o.Subtotal
        })
        .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
        .GroupBy(o => new
        {
          YEAR = o.CreatedAt.Year,
        })
        .Select(o => new OrderSumProfitGroupByTime
        {
          Year = o.Key.YEAR,
          Sum = o.Sum(g => g.Subtotal)
        }).ToList();
      return orderGrpByYear;
    }


    public List<OrderItemProductCountGroupByTime>? OrderItemProductCountGroupByDate(DateOnly fromDate, DateOnly toDate)
    {
      var result = dbContext.OrderItems
        .Select(oi => new
        {
          oi.Quantity,
          oi.Product.ProductName,
          oi.Order.CreatedAt
        })
        .Where(oi => oi.CreatedAt >= fromDate && oi.CreatedAt <= toDate)
        .GroupBy(oi => new
        {
          oi.ProductName,
          DATE = oi.CreatedAt.Day,
          MONTH = oi.CreatedAt.Month,
          YEAR = oi.CreatedAt.Year
        })
        .Select(oi => new OrderItemProductCountGroupByTime
        {
          Date = oi.Key.DATE,
          Month = oi.Key.MONTH,
          Year = oi.Key.YEAR,
          Count = oi.Select(oi => oi.Quantity).Sum() ?? 0,
          ProductName = oi.Key.ProductName
        }).ToList();
      return result;
    }
    public List<OrderItemProductCountGroupByTime>? OrderItemProductCountGroupByMonth(DateOnly fromDate, DateOnly toDate)
    {
      var result = dbContext.OrderItems
        .Select(oi => new
        {
          oi.Quantity,
          oi.Product.ProductName,
          oi.Order.CreatedAt
        })
        .Where(oi => oi.CreatedAt >= fromDate && oi.CreatedAt <= toDate)
        .GroupBy(oi => new
        {
          oi.ProductName,
          MONTH = oi.CreatedAt.Month,
          YEAR = oi.CreatedAt.Year
        })
        .Select(oi => new OrderItemProductCountGroupByTime
        {
          Month = oi.Key.MONTH,
          Year = oi.Key.YEAR,
          Count = oi.Select(oi => oi.Quantity).Sum() ?? 0,
          ProductName = oi.Key.ProductName
        }).ToList();
      return result;
    }
    public List<OrderItemProductCountGroupByTime>? OrderItemProductCountGroupByYear(DateOnly fromDate, DateOnly toDate)
    {
      var result = dbContext.OrderItems
        .Select(oi => new
        {
          oi.Quantity,
          oi.Product.ProductName,
          oi.Order.CreatedAt
        })
        .Where(oi => oi.CreatedAt >= fromDate && oi.CreatedAt <= toDate)
        .GroupBy(oi => new
        {
          oi.ProductName,
          YEAR = oi.CreatedAt.Year
        })
        .Select(oi => new OrderItemProductCountGroupByTime
        {
          Year = oi.Key.YEAR,
          Count = oi.Select(oi => oi.Quantity).Sum() ?? 0,
          ProductName = oi.Key.ProductName
        }).ToList();
      return result;
    }


    public List<Order>? GetNewestOrder(int takeTop = 5)
    {
      var result = dbContext.Orders
        .Include(o => o.Customer)
        .OrderByDescending(o => o.CreatedAt)
        .Take(takeTop);
      return result?.ToList();
    }
    public List<Product>? GetTopSellingProduct(int takeTop = 5)
    {
      var result = dbContext.OrderItems
        .Include(oi => oi.Product)
        .Select(oi => new
        {
          oi.ProductId,
          oi.Product,
          oi.Quantity,
        })
        .GroupBy(oi => oi.ProductId)
        .Select(r => new Product
        {
          ProductName = r.First().Product.ProductName,
          Price = r.First().Product.Price,
          Numbers = r.Select(oi => oi.Quantity).Sum() ?? 0,
        })
        .OrderByDescending(r => r.Numbers)
        .Take(takeTop);
      return result.ToList();
    }
    public List<Product>? GetLowestRemainProduct(int takeTop = 5)
    {
      var result = dbContext.Products
        .OrderBy(p => p.Numbers)
        .Take(takeTop);
      return result.ToList();
    }
  }
}
