using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.ViewModel
{
  public class OrderViewHodel
  {
    ObservableCollection<EFModel.Order> orders;
    OrderViewHodel()
    {
      orders = new ObservableCollection<EFModel.Order>();
      this.GetOrderAtPage();
    }
    private int _curPage = 1;
    private int _itemPerPage = 15;

    public void GetOrderAtPage()
    {
      using(EFCustomRepository.OrderRepository orderRepository = new EFCustomRepository.OrderRepository(new EFModel.RailwayContext()))
      {
        orders.Clear();
        orderRepository.GetOrderAtPage(_curPage, _itemPerPage).ToList().ForEach(o =>
        {
          orders.Add(o);
        });
      }
      
    }
  }
}
