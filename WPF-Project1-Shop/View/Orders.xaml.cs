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
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop.View
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class OrdersWindow : Window
  {
    //ObservableCollection<OrderData> orders = new ObservableCollection<OrderData>();
    private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");

    public OrdersWindow()
    {
      InitializeComponent();

      //for (int i = 0; i < 40; i++)
      //{
      //  orders.Add(new OrderData() { Name = $"Product#{i}" });
      //}
    }

    private void ListOrderLoaded(object sender, RoutedEventArgs e)
    {
      // this.ListOrder.ItemsSource = orders;
    }

    private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = _regexNumberOnly.IsMatch(e.Text);
    }
  }
}
