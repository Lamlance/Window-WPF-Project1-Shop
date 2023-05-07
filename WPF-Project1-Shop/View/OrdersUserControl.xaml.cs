using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Project1_Shop.EFModel;
using WPF_Project1_Shop.ViewModel;

namespace WPF_Project1_Shop.View
{
  /// <summary>
  /// Interaction logic for OrdersUserControl.xaml
  /// </summary>
  public partial class OrdersUserControl : UserControl
  {
    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");
    private OrderViewModel _orderViewModel = new OrderViewModel();

    ObservableCollection<string> pageDisplay = new ObservableCollection<string>();

    public OrderViewModel.MODIFY_MODE ModifyMode
    {
      get => _orderViewModel.ModifyMode;
      set
      {
        _orderViewModel.ModifyMode = value;
        this.btnCustomerAddOrderForm.Visibility = (value == OrderViewModel.MODIFY_MODE.ADD) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public OrdersUserControl()
    {
      InitializeComponent();
      _orderViewModel.OnDataSetReset += ResetComboPageBox;

      _orderViewModel.OrderAdded += (p, e) =>
      {
        Task.Run(() =>
        {
          if (p != null)
          {
            MessageBox.Show($"Added order");
          }
          else if (e != null)
          {
            MessageBox.Show(e.Message);
          }
          else
          {
            MessageBox.Show("Something happem");
          }

        });
      };

      _orderViewModel.OrderUpdated += (p, e) =>
      {
        Task.Run(() =>
        {
          if (p != null)
          {
            MessageBox.Show($"Added update");
          }
          else if (e != null)
          {
            MessageBox.Show(e.Message);
          }
          else
          {
            MessageBox.Show("Something happem");
          }
        });
      };

      _orderViewModel.OrderDeleted += (p, e) =>
      {
        Task.Run(() =>
        {
          if (p != null)
          {
            MessageBox.Show($"Added deleted");
          }
          else if (e != null)
          {
            MessageBox.Show(e.Message);
          }
          else
          {
            MessageBox.Show("Something happem");
          }
        });
      };

    }

    public void AddOrder(Order order)
    {
      _orderViewModel.AddOrder(order);
    }

    public void SearchOrder(DateTime? from, DateTime? to, string? address, string? email, string? phone, double? fromTotal, double? toTotal)
    {
      _orderViewModel.SearchOrders(from, to, address, email, phone, fromTotal, toTotal);
    }

    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }
    private void OrderFormBtnClick(object sender, RoutedEventArgs e)
    {
      if (ModifyMode == OrderViewModel.MODIFY_MODE.NONE)
      {
        Task.Run(() =>
        {
          MessageBox.Show("Please select a modify mode");
        });
        return;
      }

      if (ModifyMode == OrderViewModel.MODIFY_MODE.ADD)
      {
        Order order = new Order()
        {
          CreatedAt = DateOnly.FromDateTime(datePickerCreatedOrderForm.SelectedDate ?? DateTime.Today),
          UpdatedAt = (datePickerDeliveredOrderForm.SelectedDate == null) ? null : DateOnly.FromDateTime(datePickerDeliveredOrderForm.SelectedDate ?? DateTime.Today),
          CustomerId = _orderViewModel.SelectedOrderCustomer.Id == 0 ? null : _orderViewModel.SelectedOrderCustomer.Id,
          ShipAddress = txtBoxAddressOrderForm.Text,
          Status = ((ComboBoxItem)comboOrderForm.SelectedItem).Content.ToString(),
          Subtotal = decimal.ToDouble(txtBoxTotalOrderForm.Number),
          OrderItems = _orderViewModel.SelectedOrderItems.ToList()
        };
        AddOrder(order);
        return;
      }
      if (ModifyMode == OrderViewModel.MODIFY_MODE.EDIT && this.ListOrder.SelectedItem is Order)
      {
        Order order = (Order)this.ListOrder.SelectedItem;
        order.CreatedAt = DateOnly.FromDateTime(datePickerCreatedOrderForm.SelectedDate ?? DateTime.Today);
        order.UpdatedAt = (datePickerDeliveredOrderForm.SelectedDate == null) ? null : DateOnly.FromDateTime(datePickerDeliveredOrderForm.SelectedDate ?? DateTime.Today);
        order.ShipAddress = txtBoxAddressOrderForm.Text;
        order.Subtotal = decimal.ToDouble(txtBoxTotalOrderForm.Number);
        _orderViewModel.UpdateOrder(order);
        return;
      }
      if (ModifyMode == OrderViewModel.MODIFY_MODE.DELETE && this.ListOrder.SelectedItem is Order)
      {
        _orderViewModel.DeleteOrder((Order)this.ListOrder.SelectedItem);
      }
    }

    public void ApplyNewOrderItem(List<OrderItem> orderItems)
    {
      if (this.ListOrder.SelectedItem is Order)
      {
        ((Order)this.ListOrder.SelectedItem).OrderItems = orderItems;
      }
      _orderViewModel.SelectedOrderItems.Clear();
      foreach (var oi in orderItems)
      {
        _orderViewModel.SelectedOrderItems.Add(oi);
      }
      this.txtBoxTotalOrderForm.Number = (decimal)Math.Round(orderItems.Sum(oi => oi.Quantity * oi.Price) ?? 0, 0);
    }

    private void OrderUserControlLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = _orderViewModel;
      this.OrderPageComboBox.ItemsSource = pageDisplay;
      this.SelectedOrderItemDataGrid.ItemsSource = _orderViewModel.SelectedOrderItems;
      this.tabItemCustomerInfoOrderForm.DataContext = _orderViewModel.SelectedOrderCustomer;
    }

    private void OrderListClick(object sender, MouseButtonEventArgs e)
    {
      _orderViewModel.SelectedOrderItems.Clear();
      if (this.ListOrder.SelectedItem is Order)
      {
        Order order = (Order)this.ListOrder.SelectedItem;
        this.OrderModifyForm.DataContext = ListOrder.SelectedItem;
        this.txtBoxTotalOrderForm.Number = (decimal)Math.Round(order.Subtotal, 0);
        this.txtBoxAddressOrderForm.Text = order.ShipAddress;
        foreach (var oi in order.OrderItems)
        {
          _orderViewModel.SelectedOrderItems.Add(oi);
        }
        SetSelectedCustomer(order.Customer);
      }
    }

    private void SetSelectedCustomer(Customer? customer)
    {
      _orderViewModel.SelectedOrderCustomer.Id = customer == null ? 0 : customer.Id;

      _orderViewModel.SelectedOrderCustomer.FirstName = customer == null ? "NONE" : customer.FirstName;
      _orderViewModel.SelectedOrderCustomer.LastName = customer == null ? "NONE" : customer.LastName;
      _orderViewModel.SelectedOrderCustomer.MiddleName = customer == null ? "NONE" : customer.MiddleName;

      _orderViewModel.SelectedOrderCustomer.Phone = customer == null ? "NONE" : customer.Phone;
      _orderViewModel.SelectedOrderCustomer.Email = customer == null ? "NONE" : customer.Email;
      _orderViewModel.SelectedOrderCustomer.Address = customer == null ? "NONE" : customer.Address;

      this.txtBoxAddressOrderForm.Text = customer == null ? "NONE" : customer.Address;
    }

    public void ResetComboPageBox(int totalPage)
    {
      pageDisplay.Clear();
      for (int i = 0; i < totalPage; i++)
      {
        pageDisplay.Add($"Page {i + 1} / {totalPage}");
      }
    }

    private void PageComboBoxChange(object sender, SelectionChangedEventArgs e)
    {
      _orderViewModel.SetPage(this.OrderPageComboBox.SelectedIndex);
    }

    private void EditOrderItemBtnClick(object sender, RoutedEventArgs e)
    {
      if (this.ListOrder.SelectedItem is Order)
      {
        var qaP = new QuickAddProdct((Order)this.ListOrder.SelectedItem);
        qaP.OnOrderListConfrim += ApplyNewOrderItem;
        new Window()
        {
          Title = "Quick add product",
          Content = qaP
        }.ShowDialog();

      }
      else
      {
        var qaP = new QuickAddProdct(null, _orderViewModel.SelectedOrderItems.ToList());
        qaP.OnOrderListConfrim += ApplyNewOrderItem;
        new Window()
        {
          Title = "Quick add product",
          Content = qaP
        }.ShowDialog();
      }
      return;
    }

    private void CustomerAddOrderBtnClick(object sender, RoutedEventArgs e)
    {
      var quickAddCustomer = new QuickAddCustomer();
      quickAddCustomer.CustomerConfirmed += SetSelectedCustomer;
      new Window()
      {
        Content = quickAddCustomer
      }.ShowDialog();
    }
  }
}
