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
using WPF_Project1_Shop.Auth0Model;
using WPF_Project1_Shop.EFModel;
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
    UserInformation? user;

    public UserInformation User { get => user; set => user = value; }

    public OrdersWindow(UserInformation userInformation)
    {
      this.User = new UserInformation()
      {
        Nickname = userInformation.Nickname,
        Name = userInformation.Name,
        Email = userInformation.Email,
        PricturePath = userInformation.PricturePath
      };
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
      this.UserInfoContentControl.Content = new LoginUserControl(this.User);

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
      DateTime? to = menuApplyDateFilterOrder.IsChecked ? this.datePickerToOrderFilter.SelectedDate : null;

      double? fromSub = this.menuApplySumFilterOrder.IsChecked ? decimal.ToDouble(this.txtMoneyFromOderFilter.Number) : null;
      double? toSub = this.menuApplySumFilterOrder.IsChecked ? decimal.ToDouble(this.txtMoneyToOrderFilter.Number) : null;

      string? address = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxAddressOrderFilter.Text : null;
      string? email = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxEmailOrderFilter.Text : null;
      string? phone = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxPhoneOrderFilter.Text : null;

      ordersUserControl.SearchOrder(from, to, address, email, phone, fromSub, toSub);
    }

    private void CategoryChecked(object sender, RoutedEventArgs e)
    {
      if (sender is Fluent.CheckBox && ((Fluent.CheckBox)sender).Content is Category)
      {
        Category category = (Category)((Fluent.CheckBox)sender).Content;
        _categoryViewModel.SelectedCategories.Add(category);
        return;
      }
    }

    private void CategoryUnchecked(object sender, RoutedEventArgs e)
    {
      if (sender is Fluent.CheckBox && ((Fluent.CheckBox)sender).Content is Category)
      {
        Category category = (Category)((Fluent.CheckBox)sender).Content;
        _categoryViewModel.SelectedCategories.Remove(category);
        return;
      }
    }

    private void SearchProductBtnClick(object sender, RoutedEventArgs e)
    {
      IEnumerable<Category>? categories = this.menuApplyCategoriesProductFilter.IsChecked ? _categoryViewModel.SelectedCategories : null;
      string? name = this.menuApplyNameProductFilter.IsChecked ? this.txtBoxNameProductFilter.Text : null;
      double? fromPrice = this.menuApplyPriceProductFilter.IsChecked ? decimal.ToDouble(this.txtMoneyFromProductFilter.Number) : null;
      double? toPrice = this.menuApplyPriceProductFilter.IsChecked ? decimal.ToDouble(this.txtMoneyToProductFilter.Number) : null;

      productsUserControl.SearchProduct(categories, fromPrice, toPrice, name);
    }

    private void ProductEditModeChecked(object sender, RoutedEventArgs e)
    {
      productsUserControl.ModifyMode = ProductViewModel.MODIFY_MODE.EDIT;
    }

    private void ProductAddModeChecked(object sender, RoutedEventArgs e)
    {
      productsUserControl.ModifyMode = ProductViewModel.MODIFY_MODE.ADD;
    }

    private void ProductDeleteModeChecked(object sender, RoutedEventArgs e)
    {
      productsUserControl.ModifyMode = ProductViewModel.MODIFY_MODE.DELETE;
    }

    private void ImportProductFromAccessBtnClick(object sender, RoutedEventArgs e)
    {
      var userControl = new ImportProductFromAccess();
      userControl.OnDataImported += (p) =>
      {
        productsUserControl.AddManyProduct(p);
      };
      var window = new Window()
      {
        Title = "Import access data",
        Content = userControl
      };
      window.ShowDialog();
    }
  }
}
