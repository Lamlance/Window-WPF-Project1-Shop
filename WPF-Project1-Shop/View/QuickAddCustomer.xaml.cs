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
  /// Interaction logic for QuickAddCustomer.xaml
  /// </summary>
  public partial class QuickAddCustomer : UserControl
  {
    public delegate void OnConfirmCustomer(Customer customer);
    public event OnConfirmCustomer CustomerConfirmed;

    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");
    static CustomerViewModel customerViewModel = new CustomerViewModel();
    public QuickAddCustomer()
    {
      InitializeComponent();
    }
    private void SearchCustomerBtnClick(object sender, RoutedEventArgs e)
    {

      string? firstname = this.menuApplyCustomerNameFilter.IsChecked ? this.txtBoxFirstNameCustomerFilter.Text : null;
      string? middlename = this.menuApplyCustomerNameFilter.IsChecked ? this.txtBoxMiddleNameCustomerFilter.Text : null;
      string? lastname = this.menuApplyCustomerNameFilter.IsChecked ? this.txtBoxLastNameCustomerFilter.Text : null;

      string? phone = this.menuApplyCustomerInformationFilter.IsChecked ? this.txtBoxPhoneCustomer.Text : null;
      string? email = this.menuApplyCustomerInformationFilter.IsChecked ? this.txtBoxEmailCustomer.Text : null;
      customerViewModel.SearchCustomers(firstname, middlename, lastname, phone, email);
    }
    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }

    private void CustomerDataGridLoaded(object sender, RoutedEventArgs e)
    {
      this.DataGridCustomer.ItemsSource = customerViewModel.CustomersInPage;
    }

    private void AddCustomerClick(object sender, RoutedEventArgs e)
    {
      if(this.DataGridCustomer.SelectedItem is Customer)
      {
        CustomerConfirmed?.Invoke((Customer)this.DataGridCustomer.SelectedItem);
      }
    }
  }
}
