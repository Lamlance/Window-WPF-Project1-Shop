using System;
using System.Collections.Generic;
using System.Drawing;
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
  /// Interaction logic for ProductsUserControl.xaml
  /// </summary>
  public partial class ProductsUserControl : UserControl
  {
    ProductViewModel viewModel;
    CategoryViewModel categoryViewModel;
    public enum MODIFY_MODE
    {
      NONE, ADD, EDIT, DELETE
    }

    MODIFY_MODE _modifyMode = MODIFY_MODE.ADD;

    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");

    public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }

    public ProductsUserControl()
    {
      InitializeComponent();
      this.viewModel = new ProductViewModel();
      this.categoryViewModel = new CategoryViewModel();
    }

    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }

    private void BrowseImageBtnClick(object sender, RoutedEventArgs e)
    {
      using (var dialog = new System.Windows.Forms.OpenFileDialog())
      {
        dialog.Filter = "Files | *.jpg; *.jpeg; *.png";
        var res = dialog.ShowDialog();
        if (res == System.Windows.Forms.DialogResult.OK)
        {
          //viewModel.SelectedProduct.ImagePath = dialog.FileName;
        }
      }
    }

    private void SaveProductBtnClick(object sender, RoutedEventArgs e)
    {
      if (_modifyMode == MODIFY_MODE.ADD)
      {
        Product product = new Product()
        {
          ProductName = txtBoxNameProductFrom.Text,
          Descriptions = txtBoxDescProductFrom.Text,
          ImagePath = "Image/user.png",
          Price = decimal.ToDouble(txtCurrencyProductFrom.Number),
          Numbers = int.Parse(txtBoxAmountProductFrom.Text),
          CreatedAt = DateOnly.FromDateTime(DateTime.Now)
        };

        viewModel.AddProduct(product);
      }
    }

    private void UserControlLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = viewModel;
    }

    private void ProductListClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ProductListView.SelectedItem is Product)
      {
        Product p = (Product)this.ProductListView.SelectedItem;
        this.txtBoxNameProductFrom.Text = p.ProductName;
        this.txtCurrencyProductFrom.Number = (decimal)Math.Round(p.Price, 0);
        this.txtBoxAmountProductFrom.Text = p.Numbers.ToString();
        this.txtBoxImgPath.Text = p.ImagePath;
        this.txtBoxDescProductFrom.Text = p.Descriptions;
        string imageAbsolutePath = Helper.RelativeToAbsoluteConverter.ReletiveImagePathToAbsoule(p.ImagePath);
        this.imageProductForm.Source = new BitmapImage(new Uri(imageAbsolutePath));
      }
    }

    public void SearchProduct(IEnumerable<Category>? categories, double? from, double? to, string? name)
    {
      viewModel.SearchProduct(categories, from, to, name);
    }

    private void CategoriesListLoaded(object sender, RoutedEventArgs e)
    {
      this.CategoriesListProductForm.ItemsSource = categoryViewModel.Categories;
    }

    private void CategoryChecked(object sender, RoutedEventArgs e)
    {

    }

    private void CategoryUnchecked(object sender, RoutedEventArgs e)
    {

    }
  }
}
