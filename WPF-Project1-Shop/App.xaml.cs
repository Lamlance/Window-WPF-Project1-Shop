﻿using Auth0.OidcClient;
using IdentityModel.OidcClient;
using Microsoft.VisualBasic;
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
    static readonly bool IS_DEV = true;
    protected void ApplicationStart(object sender, EventArgs e)
    {
      if(IS_DEV == true)
      {
        OpenMainWindowWithoutLogin();
      }
      else
      {
        Window window = new Window();
        WPF_Project1_Shop.View.LoginUserControl loginUserControl = new LoginUserControl(() =>
        {
          window.Close();
        });
        window.Content = loginUserControl;
        window.Show();
      }
      
    }
    private void OpenMainWindowWithoutLogin()
    {
      new MainWindow(null).Show();
    }
  }
}
