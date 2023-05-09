using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_Project1_Shop.Helper
{
  class OrderStatusTextColorConverter : IValueConverter
  {
    static Dictionary<string, SolidColorBrush> dict = new Dictionary<string, SolidColorBrush>
    {
      {"Waiting", new SolidColorBrush(Colors.Blue)},
      {"Shipping", new SolidColorBrush(Colors.Yellow)},
      {"Shipped", new SolidColorBrush(Colors.Green)},
      {"Canceled", new SolidColorBrush(Colors.Red)},
    };
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string)
      {
        string status = (string)value;
        return dict[status] ?? new SolidColorBrush(Colors.Purple);
      }
      return new SolidColorBrush(Colors.Purple);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }
  }
}
