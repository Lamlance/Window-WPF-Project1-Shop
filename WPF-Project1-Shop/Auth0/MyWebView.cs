using IdentityModel.OidcClient.Browser;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WPF_Project1_Shop.Auth0
{
  public class MyWebView : IBrowser
  {
    private readonly Func<Window> _windowFactory;
    private MyWebView(Func<Window> windowFactory)
    {
      _windowFactory = windowFactory;
    }


    public static MyWebView NewInstace(string title = "Authenticating...", int width = 1024, int height = 768)
    {
      return new MyWebView(() =>
      new Window()
      {
        Name = "WebAuthentication",
        Title = title,
        Width = width,
        Height = height
      });
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
      var tcs = new TaskCompletionSource<BrowserResult>();

      var window = _windowFactory();
      var webView = new WebView2();

      webView.NavigationStarting += (sender, e) =>
      {
        if (e.Uri.StartsWith(options.EndUrl))
        {
          tcs.SetResult(new BrowserResult { ResultType = BrowserResultType.Success, Response = e.Uri.ToString() });
          window.Close();
        }
      };

      window.Closing += (sender, e) =>
      {
        if (!tcs.Task.IsCompleted)
        {
          tcs.SetResult(new BrowserResult { ResultType = BrowserResultType.UserCancel });
        }
      };


      window.Content = webView;
      window.Show();
      await webView.EnsureCoreWebView2Async();
      webView.CoreWebView2.Navigate(options.StartUrl);
      
      return await tcs.Task;
    }
  }
}
