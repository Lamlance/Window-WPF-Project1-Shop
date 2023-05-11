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

    }

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
