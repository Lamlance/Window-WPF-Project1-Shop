using Auth0.OidcClient;
using IdentityModel.OidcClient;
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

namespace WPF_Project1_Shop.View
{
  /// <summary>
  /// Interaction logic for LoginUserControl.xaml
  /// </summary>
  public partial class LoginUserControl : UserControl
  {
    public LoginUserControl()
    {
      InitializeComponent();
      Login();
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
    }
    private void DisplayResult(LoginResult loginResult)
    {
      if (loginResult.IsError)
      {
        MessageBox.Show(loginResult.Error);
        return;
      }
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("Tokens");
      sb.AppendLine("------");
      sb.AppendLine($"id_token: {loginResult.IdentityToken}");
      sb.AppendLine($"access_token: {loginResult.AccessToken}");
      sb.AppendLine($"refresh_token: {loginResult.RefreshToken}");
      sb.AppendLine();

      sb.AppendLine("Claims");
      sb.AppendLine("------");
      foreach (var claim in loginResult.User.Claims)
      {
        sb.AppendLine($"{claim.Type}: {claim.Value}");
      }
      LoginInfoTextBlock.Text = sb.ToString();
    }
  }
}
