using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;
using Maui.Authentication.Configuration;
using Microsoft.Extensions.Options;

namespace Maui.Authentication.Oidc
{
    public class OAuthClient
    {
        private readonly OidcClient _oidcClient;
        private readonly MauiAuthenticationSettings _settings;
        private readonly BrowserFactory _browserFactory;

        private const string WebViewCallbackScheme = "http://localhost/callback";

        public OAuthClient(IOptions<MauiAuthenticationSettings> options, BrowserFactory browserFactory)
        {
            _browserFactory = browserFactory;
            _settings = options.Value;

            _oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = _settings.OAuthSettings.Authority,
                ClientId = _settings.OAuthSettings.ClientId,
                ClientSecret = _settings.OAuthSettings.ClientSecret,
                Scope = _settings.OAuthSettings.Scope,
                RedirectUri = _settings.OAuthSettings.CallbackScheme
            });
            _oidcClient.Options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add(_settings.OAuthSettings.Domain);
        }

        public void Initialize(WebView webView)
        {
            _oidcClient.Options.Browser = _browserFactory.BuildWebViewBrowser(webView);
            _oidcClient.Options.RedirectUri = WebViewCallbackScheme;
        }

        public void Initialize()
        {
            _oidcClient.Options.Browser = _browserFactory.BuildWebAuthenticatorBrowser();
            _oidcClient.Options.RedirectUri = _settings.OAuthSettings.CallbackScheme;
        }

        public async Task<LoginResult> LoginAsync()
        {
            return await _oidcClient.LoginAsync();
        }

        public async Task<RefreshTokenResult> RefreshAsync(string refreshToken)
        {
            return await _oidcClient.RefreshTokenAsync(refreshToken);
        }

        public Task LogoutAsync()
        {
            var logoutParameters = new Dictionary<string, string>
            {
              {"client_id", _oidcClient.Options.ClientId },
              {"redirect_uri", _oidcClient.Options.RedirectUri },
              {"response_type", _settings.OAuthSettings.ResponseType },
              {"scope", _settings.OAuthSettings.Scope }
            };

            var logoutRequest = new LogoutRequest();
            var endSessionUrl = new RequestUrl(_settings.OAuthSettings.LogoutUrl)
              .Create(new Parameters(logoutParameters));
            var browserOptions = new BrowserOptions(endSessionUrl, _oidcClient.Options.RedirectUri)
            {
                Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
                DisplayMode = DisplayMode.Hidden
            };

            // NOTE: since we dont care about this as long as it succeeded (arguably either way as far as the app/user is concerned)
            // We dont wait for this to finish. For OAuth providers ive seen it will redirect back to the login page anyway, which the
            // app user might not want
            _ = _oidcClient.Options.Browser.InvokeAsync(browserOptions);

            if (_oidcClient.Options.Browser is WebViewBrowser browser)
            {
                browser.EnsureNotVisible();
            }

            return Task.CompletedTask;
        }
    }
}
