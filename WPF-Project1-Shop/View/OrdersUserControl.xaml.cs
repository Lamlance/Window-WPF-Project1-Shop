using System;
using System.Collections.Generic;
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
    public OrdersUserControl()
    {
      InitializeComponent();
    }
    public bool AddOrder(Order order)
    {
      return _orderViewModel.AddOrder(order);
    }
    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }
    private void OrderFormBtnClick(object sender, RoutedEventArgs e)
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
    }
  }
}
