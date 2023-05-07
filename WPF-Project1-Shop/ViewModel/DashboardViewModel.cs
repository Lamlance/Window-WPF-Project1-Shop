using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public class DashboardViewModel : ViewModelBase
  {

    public List<Order>? RecentOrders { get; set; }
    public ObservableCollection<Order> RecentOrdersCollection { get; set; }
    public List<Product>? TopSellProducts { get; set; }
    public ObservableCollection<Product> TopSellProductsCollection { get; set; }
    public List<Product>? TopRunningOutProducts { get; set; }
    public ObservableCollection<Product> TopRunningOutProductsCollection { get; set; }

    private double _totalEarnings;
    private int _totalOrders;
    private int _totalProducts;


    public double TotalEarnings
    {
      get
      {
        return _totalEarnings;
      }
      set
      {
        _totalEarnings = value;
        OnPropertyChanged(nameof(TotalEarnings));
      }
    }
    public int TotalOrders
    {
      get
      {
        return _totalOrders;
      }
      set
      {
        _totalOrders = value;
        OnPropertyChanged(nameof(TotalOrders));
      }
    }
    public int TotalProducts
    {
      get
      {
        return _totalProducts;
      }
      set
      {
        _totalProducts = value;
        OnPropertyChanged(nameof(TotalProducts));
      }
    }

    //public class SaleReport
    //{
    //    public DateTime CreateAt { get; set; }
    //    public double TotalSum { get; set; }
    //}


    public DashboardViewModel()
    {
      Initialize();
    }


    private async Task Initialize()
    {
      RecentOrdersCollection = new ObservableCollection<Order>();
      TopSellProductsCollection = new ObservableCollection<Product>();
      TopRunningOutProductsCollection = new ObservableCollection<Product>();
      GetOrdersInformation();
      GetProductsInformation();
      GetTopSellingProduct();
    }


    private async Task GetTopSellingProduct()
    {
      var topSelling = await Task<List<Product>?>.Run(() =>
      {
        using (ReportRepository report = new ReportRepository(new RailwayContext()))
        {
          return report.GetTopSellingProduct();
        }
      });

      if (topSelling != null)
      {
        foreach (var t in topSelling)
        {
          TopSellProductsCollection.Add(t);
        }
      }
    }

    private async Task GetProductsInformation()
    {
      var result = await Task<List<Product>>.Run(() =>
      {
        using (ProductRepository repository = new ProductRepository(new RailwayContext()))
        {
          return repository.GetManyProducts().ToList();
        }
      });

      TotalProducts = result.Where(o => o.Numbers > 0).Count();

      TopRunningOutProducts = result.Where(o => o.Numbers > 0).OrderBy(o => o.Numbers).Take(5).ToList();
      if(TopRunningOutProducts != null)
      {
        foreach(var o in TopRunningOutProducts)
        {
          TopRunningOutProductsCollection.Add(o);
        }
      }
      
    }

    private async Task GetOrdersInformation()
    {
      var result = await Task<List<Order>>.Run(() =>
      {
        using (OrderRepository repository = new OrderRepository(new RailwayContext()))
        {
          return repository.GetManyOrders().ToList();
        }
      });

      TotalOrders = result.Count();
      TotalEarnings = result.Where(o => o.Status == "Shipped").Sum(o => o.Subtotal);
      RecentOrders = result.Take(5).ToList();
      if(RecentOrders != null)
      {
        foreach (var o in RecentOrders)
          RecentOrdersCollection.Add(o);
      }

    }
  }
}
