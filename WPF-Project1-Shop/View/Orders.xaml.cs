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
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    ObservableCollection<OrderData> orders = new ObservableCollection<OrderData>();
    public MainWindow()
    {
      for(int i = 0;i < 40; i++)
      {
        orders.Add(new OrderData() { Name = $"Product#{i}" });
      }
      InitializeComponent();
    }

    private void ListOrderLoaded(object sender, RoutedEventArgs e)
    {
      this.ListOrder.ItemsSource = orders;
    }
  }
}
