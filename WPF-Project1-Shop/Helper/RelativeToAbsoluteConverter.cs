using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_Project1_Shop.Helper
{
  public class RelativeToAbsoluteConverter : IValueConverter
  {
    static readonly string folder = AppDomain.CurrentDomain.BaseDirectory;

    public static string ReletiveImagePathToAbsoule(string relative)
    {
      string absolute = $"{folder}{relative}";
      if (File.Exists(absolute))
      {
        return absolute;
      }
      return $"{folder}/Images/img_missing.png";

    }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string)
      {
        string relative = (string)value;
        ReletiveImagePathToAbsoule(relative);
      }
      

      return $"{folder}/Images/img_missing.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
