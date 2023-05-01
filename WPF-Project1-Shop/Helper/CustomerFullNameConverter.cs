using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.Helper
{
  public class CustomerFullNameConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if(value is Customer)
      {
        Customer customer = (Customer)value;
        return $"{customer.FirstName ?? ""} {customer.MiddleName ?? ""} {customer.LastName ?? ""}";
      }
      return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
