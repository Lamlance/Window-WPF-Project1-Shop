using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_Project1_Shop.Auth0;
using WPF_Project1_Shop.View;

namespace WPF_Project1_Shop
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected void ApplicationStart(object sender, EventArgs e)
    {
      new Window()
      {
        Content = new LoginUserControl()
      }.Show();
    }
    
  }
}
