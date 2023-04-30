using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public class OrderViewModel
  {
    public delegate void OnOrderDataModify(Order order);
    public event OnOrderDataModify OrderUpdated;
    public event OnOrderDataModify OrderAdded;
    public event OnOrderDataModify OrderDeleted;


    public enum MODIFY_MODE
    {
      NONE, ADD, EDIT, DELETE
    }

    public event ProductDataSetChanged? OnDataSetReset;


    ObservableCollection<EFModel.Order> ordersInPage;
    ObservableCollection<OrderItem> selectedOrderItems;

    List<EFModel.Order>? ordersSet;
    MODIFY_MODE _modifyMode = MODIFY_MODE.NONE;
    Dictionary<long, int> idToPos = new Dictionary<long, int>();

    public OrderViewModel()
    {
      ordersInPage = new ObservableCollection<EFModel.Order>();
      selectedOrderItems = new ObservableCollection<OrderItem>();
      Initialize();
    }
    private int _curPage = 1;
    private int _itemPerPage = 15;
    private bool _isSearching = false;
    public ObservableCollection<Order> OrdersInPage { get => ordersInPage; }

    public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }
    public ObservableCollection<OrderItem> SelectedOrderItems { get => selectedOrderItems; }

    public async Task Initialize()
    {
      await GetManyOrder();
      SetPage(1);
      OnDataSetReset?.Invoke((int)Math.Ceiling((double)(ordersSet != null ? ordersSet.Count() : 0) / _itemPerPage));
    }

    public void SetPage(int page)
    {
      if(ordersSet == null)
      {
        return;
      }

      _curPage = page > 0 ? page : 1;
      ordersInPage.Clear();
      idToPos.Clear();

      //var numberOfPages = Math.Floor( (double)((ordersSet.Count() + _itemPerPage - 1) / _itemPerPage));
      int start = (_curPage * _itemPerPage) - _itemPerPage;
      int end = Math.Min(start + _itemPerPage, ordersSet.Count());
      for (int i = start; i < end; i++)
      {
        idToPos.Add(ordersSet.ElementAt(i).Id, i);
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

    public async Task SearchOrders(DateTime? from, DateTime? to, string? address, string? email, string? phone, double? fromTotal, double? toTotal)
    {
      if (_isSearching)
      {
        return;
      }
      _isSearching = true;
      var result = await Task<List<Order>?>.Run(() =>
      {
        using (OrderRepository repository = new OrderRepository(new RailwayContext()))
        {
          return repository.SearchOrders(from, to, address, email, phone, fromTotal, toTotal)?.ToList();
        }
      });
      ordersSet = result;
      SetPage(1);
      _isSearching = false;
      OnDataSetReset?.Invoke((int)Math.Ceiling((double)(ordersSet != null ? ordersSet.Count() : 0) / _itemPerPage));

    }

    public async Task AddOrder(Order order)
    {
      var result = await Task<Order?>.Run(() =>
      {
        try
        {

          using (EFCustomRepository.OrderRepository orderRepository = new EFCustomRepository.OrderRepository(new EFModel.RailwayContext()))
          {
            return orderRepository.AddOrder(order);
          }
        }
        catch (Exception e)
        {
          return null;
        };
      });
      if(result != null)
      {
        ordersInPage.Insert(0, result);
        OrderAdded?.Invoke(result);
      }

    }

    public async Task UpdateOrder(Order data)
    {
      var result = await Task<Order?>.Run(() =>
      {
        try
        {
          using (OrderRepository repository = new OrderRepository(new RailwayContext()))
          {
            return repository.UpdateOrder(data);
          }
        }catch(Exception e)
        {
          return null;
        }
      });
      if(result != null)
      {
        OrderUpdated?.Invoke(data);
      }
    }

    public async Task DeleteOrder(Order order)
    {
      var result = await Task<Order?>.Run(() =>
      {
        try
        {
          using (OrderRepository repository = new OrderRepository(new RailwayContext()))
          {
            return repository.DeleteOrder(order);
          }
        }
        catch (Exception)
        {
          return null;
        }
      });

      if(result != null)
      {
        ordersInPage.Remove(result);
        ordersSet?.Remove(result);
        OrderDeleted?.Invoke(result);
      }

    }
  }
}
