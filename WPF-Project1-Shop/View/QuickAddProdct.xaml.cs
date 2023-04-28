using System;
using System.Collections.Generic;
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
  /// Interaction logic for QuickAddProdct.xaml
  /// </summary>
  public partial class QuickAddProdct : UserControl
  {
    CategoryViewModel _categoryViewModel = new CategoryViewModel();
    static ProductViewModel productViewModel = new ProductViewModel()
    {
      ItemPerPage = 100,
    };
    public QuickAddProdct()
    {
      InitializeComponent();
    }

    private void CategoriesListLoaded(object sender, RoutedEventArgs e)
    {
      this.ListRibbonCategoriesList.ItemsSource = _categoryViewModel.Categories;
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

      productViewModel.SearchProduct(categories, fromPrice, toPrice, name);
    }

    private void DataGridLoaded(object sender, RoutedEventArgs e)
    {
      this.ProductDataGrid.ItemsSource = productViewModel.ProductsInPage;
    }
  }
}
