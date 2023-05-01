using System;
using System.Collections;
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
  /// Interaction logic for QuickAddProdct.xaml
  /// </summary>
  public partial class QuickAddProdct : UserControl
  {
    public delegate void  OrderItemListCallback (List<OrderItem> ordersItem);
    public event OrderItemListCallback? OnOrderListConfrim;

    CategoryViewModel _categoryViewModel = CategoryViewModel.NewInstance();
    static ProductViewModel productViewModel = new ProductViewModel()
    {
      ItemPerPage = 100,
    };

    ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
    Order? parentOrder;

    public QuickAddProdct(Order? order, List<OrderItem>? prevItem = null)
    {
      InitializeComponent();
      parentOrder = order;
      if (order != null && order.OrderItems != null)
      {
        foreach (var oi in order.OrderItems)
        {
          orderItems.Add(oi);
        }
      }
      else if (prevItem != null)
      {
        foreach (var oi in prevItem)
        {
          orderItems.Add(oi);
        }
      }
      productViewModel.ClearSkipProduct();
      foreach(var oi in orderItems)
      {
        if(oi.Product != null)
        {
          productViewModel.AddSkipProduct(oi.Product);
        }
      }
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
      this.OrderItemDataGrid.ItemsSource = orderItems;
    }

    private void AddToOrderBtnClick(object sender, RoutedEventArgs e)
    {
      var selectedItems = this.ProductDataGrid.SelectedItems;
      if(selectedItems.Count > 0 && selectedItems[0] is Product)
      {
        var selectedProduct = new List<Product>();
        foreach (var item in selectedItems)
        {
          selectedProduct.Add((Product)item);
        }

        foreach(var p in selectedProduct)
        {
          Product product = (Product)p;
          productViewModel.AddSkipProduct(product);
          orderItems.Add(new OrderItem()
          {
            ProductId = product.Id,
            Product = product,
            OrderId = parentOrder == null ? 0 : parentOrder.Id,
            Price = product.Price,
            Quantity = 1,
            CreatedAt = DateOnly.FromDateTime(DateTime.Now)
          }); ;
        }
      }
    }

    private void RemoveFromOrderGtnClick(object sender, RoutedEventArgs e)
    {
      var selectdItem = this.OrderItemDataGrid.SelectedItems;
      if(selectdItem.Count > 0 && selectdItem[0] is OrderItem)
      {
        var selectedOrderItems = new List<OrderItem>();
        foreach(var oi in selectdItem)
        {
          selectedOrderItems.Add((OrderItem)oi);
        }

        foreach(var oi in selectedOrderItems)
        {
          orderItems.Remove(oi);
          if(oi.Product != null)
          {
            productViewModel.RemoveSkipProduct(oi.Product);
          }
          
        }
      }
    }

    private void ConfirmOrderItemClick(object sender, RoutedEventArgs e)
    {
      OnOrderListConfrim?.Invoke(orderItems.ToList());
    }
  }
}
