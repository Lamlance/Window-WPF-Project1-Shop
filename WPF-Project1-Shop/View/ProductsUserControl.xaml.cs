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



    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");

    public ProductViewModel.MODIFY_MODE ModifyMode { get => viewModel.ModifyMode; set => viewModel.ModifyMode = value; }

    public ProductsUserControl()
    {
      InitializeComponent();
      this.viewModel = new ProductViewModel();
      this.categoryViewModel = new CategoryViewModel();

      viewModel.OnDataAdd += (p) =>
      {
        Task.Run(() =>
        {
          MessageBox.Show($"Added {p.ProductName}");
        });
      };
      viewModel.OnDataUpdate += (p) =>
      {
        Task.Run(() =>
        {
          MessageBox.Show($"Update {p.ProductName}");
        });
      };

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
          this.txtBoxImgPath.Text = dialog.FileName;
          this.imageProductForm.Source = new BitmapImage(new Uri(dialog.FileName));
          //viewModel.SelectedProduct.ImagePath = dialog.FileName;
        }
      }
    }

    private void SaveProductBtnClick(object sender, RoutedEventArgs e)
    {
      if(ModifyMode == ProductViewModel.MODIFY_MODE.NONE)
      {
        MessageBox.Show("Select a modify mode");
        return;
      }

      if (ModifyMode == ProductViewModel.MODIFY_MODE.ADD)
      {
        Product product = new Product()
        {
          ProductName = txtBoxNameProductFrom.Text,
          Descriptions = txtBoxDescProductFrom.Text,
          ImagePath = Helper.CopyFileToApp.CopyImageToApp(this.txtBoxImgPath.Text),
          Price = decimal.ToDouble(txtCurrencyProductFrom.Number),
          Numbers = int.Parse(txtBoxAmountProductFrom.Text),
          CreatedAt = DateOnly.FromDateTime(DateTime.Now)
        };
        viewModel.AddProduct(product);
        return;
      }

      if(ModifyMode == ProductViewModel.MODIFY_MODE.EDIT && this.ProductListView.SelectedItem is Product)
      {
        Product p = (Product)this.ProductListView.SelectedItem;
        p.ProductName = txtBoxNameProductFrom.Text;
        p.Descriptions = txtBoxDescProductFrom.Text;
        p.Price = decimal.ToDouble(txtCurrencyProductFrom.Number);
        p.ImagePath = this.txtBoxImgPath.Text;
        p.Numbers = int.Parse(txtBoxAmountProductFrom.Text);
        viewModel.UpdateProduct(p);
        return;
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

        for(int i = 0; i < categoryViewModel.Categories.Count; i++)
        {
          categoryViewModel.Categories[i].IsChecked = p.Categories.Any(c => categoryViewModel.Categories[i].Id == c.Id);
        }
        
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
      if(sender is Fluent.CheckBox && ((Fluent.CheckBox)sender).Content is Category)
      {
        Category c = (Category)((Fluent.CheckBox)sender).Content;
        if(ProductListView.SelectedItem is Product)
        {
          ((Product)ProductListView.SelectedItem).Categories.Add(c);
        }
      }
    }

    private void CategoryUnchecked(object sender, RoutedEventArgs e)
    {
      if (sender is Fluent.CheckBox && ((Fluent.CheckBox)sender).Content is Category)
      {
        Category c = (Category)((Fluent.CheckBox)sender).Content;
        if (ProductListView.SelectedItem is Product)
        {
          ((Product)ProductListView.SelectedItem).Categories.Remove(c);
        }
      }
    }
  }
}
