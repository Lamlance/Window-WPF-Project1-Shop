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
  /// Interaction logic for ProductsUserControl.xaml
  /// </summary>
  public partial class ProductsUserControl : UserControl
  {
    ProductViewModel viewModel;
    public enum MODIFY_MODE
    {
      NONE,ADD,EDIT,DELETE
    }

    MODIFY_MODE _modifyMode = MODIFY_MODE.ADD;

    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");

    public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }

    public ProductsUserControl()
    {
      InitializeComponent();
      this.viewModel = new ProductViewModel();
      viewModel.SelectedProduct.ProductName = "Example product";
      viewModel.SelectedProduct.ImagePath = "None";
      this.ProductDataForm.DataContext = viewModel.SelectedProduct;
    }

    private void ProductListViewLoaded(object sender, RoutedEventArgs e)
    {
      this.ProductListView.ItemsSource = viewModel.Products;
    }
    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }
    private void BrowseImageBtnClick(object sender, RoutedEventArgs e)
    {
      using(var dialog = new System.Windows.Forms.OpenFileDialog())
      {
        dialog.Filter = "Files | *.jpg; *.jpeg; *.png";
        var res = dialog.ShowDialog();
        if(res == System.Windows.Forms.DialogResult.OK)
        { 
          viewModel.SelectedProduct.ImagePath = dialog.FileName;
        }
      }
    }

    private void SaveProductBtnClick(object sender, RoutedEventArgs e)
    {
      if(_modifyMode == MODIFY_MODE.ADD)
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
  }
}
