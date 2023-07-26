using IdentityModel.OidcClient.Browser;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace Maui.Authentication.Blazor.Oidc
{
    public class BlazorWebViewBrowser : IdentityModel.OidcClient.Browser.IBrowser
    {
        private readonly BlazorWebView _webView;

        public BlazorWebViewBrowser(BlazorWebView webView)
        {
            _webView = webView;
        }

        public void EnsureNotVisible()
        {
            _webView.WidthRequest = 0;
            _webView.HeightRequest = 0;
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();
            var navManager = _webView.Navigation as NavigationManager;
            navManager.LocationChanged += (sender, e) =>
            {
                if (e.Location.StartsWith(options.EndUrl))
                {
                    _webView.WidthRequest = 0;
                    _webView.HeightRequest = 0;
                    if (tcs.Task.Status != TaskStatus.RanToCompletion)
                    {
                        tcs.SetResult(new BrowserResult
                        {
                            ResultType = BrowserResultType.Success,
                            Response = e.Location.ToString()
                        });
                    }
                }

            };

            _webView.WidthRequest = 600;
            _webView.HeightRequest = 600;

            navManager.NavigateTo(options.StartUrl);

            return await tcs.Task;
        }
    }
}
