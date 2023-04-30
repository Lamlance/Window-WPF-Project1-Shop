using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.Auth0;

namespace Auth0.OidcClient
{
  public class MyAuth0Client : Auth0ClientBase
  {
    public MyAuth0Client(Auth0ClientOptions options) : base(options, "wpf")
    {
      options.Browser = options.Browser ?? MyWebView.NewInstace();
    }
  }
}
