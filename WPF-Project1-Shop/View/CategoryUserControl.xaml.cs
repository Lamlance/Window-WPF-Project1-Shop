using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
  /// Interaction logic for CategoryUserControl.xaml
  /// </summary>
  public partial class CategoryUserControl : UserControl
  {
    static CategoryViewModel categoryViewModel = CategoryViewModel.NewInstance();
    public CategoryViewModel.MODIFY_MODE ModifyMode { get => categoryViewModel.ModifyMode; set => categoryViewModel.ModifyMode = value; }
    ObservableCollection<string> pageDisplay = new ObservableCollection<string>();

    public CategoryUserControl()
    {
      InitializeComponent();
    }

    private void CategoryListClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ListCategory.SelectedItem is Category)
      {
        this.CategoryModifyForm.DataContext = (Category)ListCategory.SelectedItem;
      }
    }

    public void ResetComboPageBox(int totalPage)
    {
      pageDisplay.Clear();
      for (int i = 0; i < totalPage; i++)
      {
        pageDisplay.Add($"Page: {i + 1} / {totalPage}");
      }
    }

    private void PageComboBoxChange(object sender, SelectionChangedEventArgs e)
    {
      //categoryViewModel.SetPage(this.CustomerPageComboBox.SelectedIndex);
    }

    private void CategoryFormBtnClick(object sender, RoutedEventArgs e)
    {
      if (ModifyMode == CategoryViewModel.MODIFY_MODE.NONE)
      {
        MessageBox.Show("Select a modify mode");
        return;
      }
      if (ModifyMode == CategoryViewModel.MODIFY_MODE.ADD)
      {
        Category category = new Category()
        {
          CategoryName = this.txtBoxCategoryName.Text,
        };
        categoryViewModel.AddCategory(category);
        return;
      }
    }

    //public void CustomerFormBtnClick(object sender, RoutedEventArgs e)
    //{
    //  if (ModifyMode == CustomerViewModel.MODIFY_MODE.NONE)
    //  {
    //    MessageBox.Show("Select a modify mode");
    //    return;
    //  }
    //  if (ModifyMode == CustomerViewModel.MODIFY_MODE.ADD)
    //  {
    //    Customer customer = new Customer()
    //    {
    //      FirstName = this.txtBoxFirstNameCustomer.Text,
    //      MiddleName = this.txtBoxMiddleNameCustomer.Text,
    //      LastName = this.txtBoxLastNameCustomer.Text,
    //      Phone = this.txtBoxPhone.Text,
    //      Email = this.txtBoxEmail.Text,
    //      Address = this.txtBoxAddress.Text
    //    };
    //    _customerViewModel.AddCustomer(customer);
    //    return;
    //  }
    //  if (ModifyMode == CustomerViewModel.MODIFY_MODE.EDIT && this.ListCustomer.SelectedItem is Customer)
    //  {
    //    Customer customer = (Customer)this.ListCustomer.SelectedItem;
    //    customer.FirstName = this.txtBoxFirstNameCustomer.Text;
    //    customer.MiddleName = this.txtBoxMiddleNameCustomer.Text;
    //    customer.LastName = this.txtBoxLastNameCustomer.Text;
    //    customer.Phone = this.txtBoxPhone.Text;
    //    customer.Email = this.txtBoxEmail.Text;
    //    customer.Address = this.txtBoxAddress.Text;
    //    _customerViewModel.UpdateCustomer(customer);
    //    return;
    //  }
    //  if (ModifyMode == CustomerViewModel.MODIFY_MODE.DELETE && this.ListCustomer.SelectedItem is Customer)
    //  {
    //    Customer customer = (Customer)this.ListCustomer.SelectedItem;
    //    customer.FirstName = this.txtBoxFirstNameCustomer.Text;
    //    customer.MiddleName = this.txtBoxMiddleNameCustomer.Text;
    //    customer.LastName = this.txtBoxLastNameCustomer.Text;
    //    customer.Phone = this.txtBoxPhone.Text;
    //    customer.Email = this.txtBoxEmail.Text;
    //    customer.Address = this.txtBoxAddress.Text;
    //    _customerViewModel.RemoveCustomer(customer);
    //    return;
    //  }
    //}

    private void CategoryUserControlLoaded(object sender, RoutedEventArgs e)
    {
      this.ListCategory.ItemsSource = categoryViewModel.Categories;
      this.DataContext = categoryViewModel;   
      this.CategoryPageComboBox.ItemsSource = pageDisplay;
    }

    private void ListCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    public void SearchCategory(string? name)
    {
      categoryViewModel.SearchCategories(name);
    }
  }
}
