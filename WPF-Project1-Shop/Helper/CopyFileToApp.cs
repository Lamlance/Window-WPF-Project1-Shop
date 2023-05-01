using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Helper
{
  public class CopyFileToApp
  {
    static readonly string appPath = AppDomain.CurrentDomain.BaseDirectory;
    public static string CopyImageToApp(string absoluteSrcPath)
    {
      if (!File.Exists(absoluteSrcPath))
      {
        return string.Empty;
      }
      string fileName = Path.GetFileName(absoluteSrcPath);
      string reletivePath = $"/Images/{fileName}";
      string absoluteDesPath = $"{appPath}{reletivePath}";
      if (File.Exists(absoluteDesPath))
      {
        return reletivePath;
      }
      try
      {
        File.Copy(absoluteSrcPath, absoluteDesPath);
      }catch(Exception)
      {
        return string.Empty;
      }
      return reletivePath;
    }
  }
}
