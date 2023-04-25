using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public class OrderViewModel
  {
    ObservableCollection<EFModel.Order> ordersInPage;
    IEnumerable<EFModel.Order>? ordersSet;
    public OrderViewModel()
    {
      ordersInPage = new ObservableCollection<EFModel.Order>();
      //Initialize();
    }
    private int _curPage = 1;
    private int _itemPerPage = 15;

    public ObservableCollection<Order> OrdersInPage { get => ordersInPage; }

    public async Task Initialize()
    {
      await GetManyOrder();
      SetPage(1);
    }

    public void SetPage(int page)
    {
      if(ordersSet == null)
      {
        return;
      }

      _curPage = page > 0 ? page : 1;
      ordersInPage.Clear();

      //var numberOfPages = Math.Floor( (double)((ordersSet.Count() + _itemPerPage - 1) / _itemPerPage));
      int start = (page * _itemPerPage) - _itemPerPage;
      int end = Math.Min(start + _itemPerPage, ordersSet.Count());
      for (int i = start; i < end; i++)
      {
        ordersInPage.Add(ordersSet.ElementAt(i));
      }
    }

    public async Task GetManyOrder()
    {
      var result = await Task<List<Order>>.Run(() =>
      {
        using (EFCustomRepository.OrderRepository orderRepository = new EFCustomRepository.OrderRepository(new EFModel.RailwayContext()))
        {
          return orderRepository.GetManyOrders().ToList();
        }
      });
      ordersSet = result;
    }

    public bool AddOrder(Order order)
    {
      try
      {
        using (EFCustomRepository.OrderRepository orderRepository = new EFCustomRepository.OrderRepository(new EFModel.RailwayContext()))
        {
          orderRepository.AddOrder(order);
        }
        ordersInPage.Insert(0,order);
        return true;
      }catch(Exception e)
      {
        return false;
      };
      
    }

  }
}
