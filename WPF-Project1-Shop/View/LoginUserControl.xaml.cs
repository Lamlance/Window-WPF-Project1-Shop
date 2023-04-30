using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using WPF_Project1_Shop.Auth0;
using WPF_Project1_Shop.Auth0Model;

namespace WPF_Project1_Shop.View
{
  /// <summary>
  /// Interaction logic for LoginUserControl.xaml
  /// </summary>
  public partial class LoginUserControl : UserControl
  {
    public delegate void OnLoginSuccess();
    public event OnLoginSuccess LoginSuccessed;
    public LoginUserControl(OnLoginSuccess loginSuccess)
    {
      this.LoginSuccessed += loginSuccess;
      InitializeComponent();
      Login();
    }

    UserInformation? User;
    public LoginUserControl(UserInformation user)
    {
      User = user;
      InitializeComponent();
      if(user == null)
      {
        Login();
      }
      else
      {
        this.txtBlockLog.Visibility = Visibility.Collapsed;
        this.DockPanelUserInfo.Visibility = Visibility.Visible;
        this.btnLogin.Visibility = Visibility.Collapsed;
        this.btnLogout.Visibility = Visibility.Visible;
      }
    }


    private MyAuth0Client? client;
    
    public async Task Login()
    {
      client = new MyAuth0Client(new Auth0ClientOptions
      {
        Domain = "dev-j07rhfbc.us.auth0.com",
        ClientId = "vr8qHAl19qNFnWDK4eXS0AasVwV0SNBK",
        Browser = MyWebView.NewInstace()
      });
      var extraParameters = new Dictionary<string, string>();
      extraParameters.Add("connection", "WPF-ShopUserDB");
      try
      {
        var result = await client.LoginAsync(extraParameters: extraParameters);
        DisplayResult(result);
      }
      catch (Exception e)
      {
        var a = e.Message;
      }
      btnLogin.Visibility = Visibility.Collapsed;
      btnLogout.Visibility = Visibility.Visible;
      LoginSuccessed?.Invoke();
    }

    public async Task Logout()
    {
      if(client != null)
      {
        client = new MyAuth0Client(new Auth0ClientOptions
        {
          Domain = "dev-j07rhfbc.us.auth0.com",
          ClientId = "vr8qHAl19qNFnWDK4eXS0AasVwV0SNBK",
          Browser = MyWebView.NewInstace()
        });
      }

      BrowserResultType browserResult = await client!.LogoutAsync();
      if (browserResult != BrowserResultType.Success)
      {
        Task.Run(() =>
        {
          MessageBox.Show(browserResult.ToString());
        });
        return;
      }
      btnLogin.Visibility = Visibility.Visible;
      btnLogout.Visibility = Visibility.Collapsed;
    }

    private void DisplayResult(LoginResult loginResult)
    {
      if (loginResult.IsError)
      {
        MessageBox.Show(loginResult.Error);
        return;
      }

      UserInformation user = new UserInformation();

      StringBuilder sb = new StringBuilder();
      foreach (var claim in loginResult.User.Claims)
      {
        switch (claim.Type){
          case "nickname":
            {
              user.Nickname = claim.Value;
              sb.AppendLine($"{claim.Type}: {claim.Value}");
              break;
            }
          case "name":
            {
              user.Name = claim.Value;
              sb.AppendLine($"{claim.Type}: {claim.Value}");
              break;
            }
          case "picture":
            {
              user.PricturePath = claim.Value;
              break ;
            }
          case "email":
            {
              user.Email = claim.Value;
              sb.AppendLine($"{claim.Type}: {claim.Value}");
              break;
            }
          case "email_verified":
            {
              if(user.IsEmailVerified)
              {
                user.IsEmailVerified = user.IsEmailVerified && (claim.Value == "True" || claim.Value == "true");
              }
              else
              {
                user.IsEmailVerified = (claim.Value == "True" || claim.Value == "true");
              }
              sb.AppendLine($"{claim.Type}: {claim.Value}");
              break;
            }
        }
      }
      this.txtBlockUserInfo.Text = sb.ToString();
      this.imgUserAvatar.Source = new BitmapImage(new Uri(user.PricturePath));
      if (user.IsEmailVerified)
      {
        new OrdersWindow(user)
        {
          User = user
        }.Show();
      }
    }

    private void LogOutBtnClicked(object sender, RoutedEventArgs e)
    {
      Logout();
    }

    private void DockPanelUserInfoLoaded(object sender, RoutedEventArgs e)
    {
      if(User == null)
      {
        return;
      }
      StringBuilder sb = new StringBuilder();
      sb.AppendLine($"Nick name: {User.Nickname}");
      sb.AppendLine($"Email: {User.Email}");
      sb.AppendLine($"Email is verified: {User.IsEmailVerified}");
      this.txtBlockUserInfo.Text = sb.ToString();
      this.imgUserAvatar.Source = new BitmapImage(new Uri(User.PricturePath));
    }
  }
}
