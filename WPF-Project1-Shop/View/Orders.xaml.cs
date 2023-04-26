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
using WPF_Project1_Shop.ViewModel;

namespace WPF_Project1_Shop.View
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class OrdersWindow : Window
  {
    //ObservableCollection<OrderData> orders = new ObservableCollection<OrderData>();
    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");
    private static OrdersUserControl ordersUserControl = new OrdersUserControl();
    private static ProductsUserControl productsUserControl = new ProductsUserControl();
    private static CustomerUserControl customerUserControl = new CustomerUserControl();

    CategoryViewModel _categoryViewModel;

    public OrdersWindow()
    {
      InitializeComponent();
      _categoryViewModel = new CategoryViewModel();
    }

    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }

    private void MainWindowLoaded(object sender, RoutedEventArgs e)
    {
      this.datePickerFromOrderFilter.SelectedDate = DateTime.Today;
      this.datePickerToOrderFilter.SelectedDate = DateTime.Today;
      var screens = new ObservableCollection<TabItem>()
        {
          new TabItem(){Content = ordersUserControl },
          new TabItem(){Content = productsUserControl },
          new TabItem(){Content = customerUserControl}
        };
      this.MainTabControl.ItemsSource = screens;
    }

    private void CategoriesListLoaded(object sender, RoutedEventArgs e)
    {
      this.ListRibbonCategoriesList.ItemsSource = _categoryViewModel.Categories;
    }

    private void OrderEditModeChecked(object sender, RoutedEventArgs e)
    {
      ordersUserControl.ModifyMode = OrderViewModel.MODIFY_MODE.EDIT;
    }

    private void OrderAddModeChecked(object sender, RoutedEventArgs e)
    {
      ordersUserControl.ModifyMode = OrderViewModel.MODIFY_MODE.ADD;
    }

    private void OrderDeleteModeChecked(object sender, RoutedEventArgs e)
    {
      ordersUserControl.ModifyMode = OrderViewModel.MODIFY_MODE.DELETE;
    }

    private void SearchOrderBtnClick(object sender, RoutedEventArgs e)
    {
      DateTime? from = menuApplyDateFilterOrder.IsChecked ? this.datePickerFromOrderFilter.SelectedDate : null;
      DateTime? to = menuApplyDateFilterOrder.IsChecked ?  this.datePickerToOrderFilter.SelectedDate : null;
      
      double? fromSub = this.menuApplySumFilterOrder.IsChecked ? decimal.ToDouble(this.txtMoneyFromOderFilter.Number) : null;
      double? toSub = this.menuApplySumFilterOrder.IsChecked ? decimal.ToDouble(this.txtMoneyToOrderFilter.Number) : null;
      
      string? address = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxAddressOrderFilter.Text : null;
      string? email = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxEmailOrderFilter.Text : null;
      string? phone = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxPhoneOrderFilter.Text : null;

      ordersUserControl.SearchOrder(from, to, address, email, phone, fromSub, toSub);
    }
  }
}
