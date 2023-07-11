namespace Maui.Authentication.Oidc
{
    public class BrowserFactory
    {
        public IdentityModel.OidcClient.Browser.IBrowser BuildWebViewBrowser(WebView webView)
        {
            return new WebViewBrowser(webView);
        }

        public IdentityModel.OidcClient.Browser.IBrowser BuildWebAuthenticatorBrowser()
        {
            return new WebAuthenticatorBrowser();
        }
    }
}
