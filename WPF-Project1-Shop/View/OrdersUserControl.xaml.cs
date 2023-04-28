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

    public OrderViewModel.MODIFY_MODE ModifyMode { get => _orderViewModel.ModifyMode; set => _orderViewModel.ModifyMode = value; }

    public OrdersUserControl()
    {
      InitializeComponent();
      _orderViewModel.OnDataSetReset += ResetComboPageBox;
    }
    public bool AddOrder(Order order)
    {
      return _orderViewModel.AddOrder(order);
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
      if (ModifyMode == OrderViewModel.MODIFY_MODE.ADD)
      {
        Order order = new Order()
        {
          CreatedAt = DateOnly.FromDateTime(datePickerCreatedOrderForm.SelectedDate ?? DateTime.Today),
          UpdatedAt = (datePickerDeliveredOrderForm.SelectedDate == null) ? null : DateOnly.FromDateTime(datePickerDeliveredOrderForm.SelectedDate ?? DateTime.Today),
          CustomerId = 1,
          ShipAddress = txtBoxAddressOrderForm.Text,
          Status = ((ComboBoxItem)comboOrderForm.SelectedItem).Content.ToString(),
          Subtotal = 10000
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

    }

    private void OrderUserControlLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = _orderViewModel;
      this.labelStatusText.Content = _orderViewModel.ModifyMode;
      this.OrderPageComboBox.ItemsSource = pageDisplay;
    }

    private void OrderListClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ListOrder.SelectedItem is Order)
      {
        this.OrderModifyForm.DataContext = (Order)ListOrder.SelectedItem;
      }
    }

    public void ResetComboPageBox(int totalPage)
    {
      pageDisplay.Clear();
      for (int i = 0; i < totalPage; i++)
      {
        pageDisplay.Add($"Page{i + 1} / {totalPage}");
      }
    }

    private void PageComboBoxChange(object sender, SelectionChangedEventArgs e)
    {
      _orderViewModel.SetPage(this.OrderPageComboBox.SelectedIndex);
    }
  }
}
