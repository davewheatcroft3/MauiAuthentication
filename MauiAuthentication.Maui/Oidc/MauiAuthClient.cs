using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Oidc;
using Microsoft.Extensions.Options;

namespace Maui.Authentication.Core.Oidc
{
    public class MauiAuthClient : AuthClient
    {
        private const string WebViewCallbackScheme = "http://localhost/callback";

        public MauiAuthClient(IOptions<MauiAuthenticationSettings> options) : base(options)
        {
        }

        public void InitializeWithWebView(WebView webView)
        {
            _oidcClient.Options.Browser = new WebViewBrowser(webView);
            _oidcClient.Options.RedirectUri = WebViewCallbackScheme;
        }

        public override async Task LogoutAsync()
        {
            await base.LogoutAsync();

            if (_oidcClient.Options.Browser is WebViewBrowser browser)
            {
                browser.EnsureNotVisible();
            }
        }
    }
}
