using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Oidc;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Options;

namespace Maui.Authentication.Blazor.Oidc
{
    public class MauiBlazorAuthClient : AuthClient
    {
        private const string WebViewCallbackScheme = "http://localhost/callback";

        public MauiBlazorAuthClient(IOptions<MauiAuthenticationSettings> options) : base(options)
        {
        }

        public void InitializeWithWebView(BlazorWebView webView)
        {
            _oidcClient.Options.Browser = new BlazorWebViewBrowser(webView);
            _oidcClient.Options.RedirectUri = WebViewCallbackScheme;
        }

        public override async Task LogoutAsync()
        {
            await base.LogoutAsync();

            if (_oidcClient.Options.Browser is BlazorWebViewBrowser browser)
            {
                browser.EnsureNotVisible();
            }
        }
    }
}
