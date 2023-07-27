using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;
using Maui.Authentication.Core.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MauiAuthentication.Blazor")]
[assembly: InternalsVisibleTo("MauiAuthentication.Maui")]
namespace Maui.Authentication.Core.Oidc
{
    public class AuthClient
    {
        protected readonly OidcClient _oidcClient;
        private readonly MauiAuthenticationSettings _settings;

        public AuthClient(
            IdentityModel.OidcClient.Browser.IBrowser browser,
            IOptions<MauiAuthenticationSettings> options)
        {
            _settings = options.Value;

            _oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = _settings.OAuthSettings.Authority,
                ClientId = _settings.OAuthSettings.ClientId,
                ClientSecret = _settings.OAuthSettings.ClientSecret,
                Scope = _settings.OAuthSettings.Scope,
                RedirectUri = _settings.OAuthSettings.CallbackScheme,
                Browser = browser
            });
            _oidcClient.Options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add(_settings.OAuthSettings.DiscoveryBaseUrl);
        }

        public async Task<LoginResult> LoginAsync()
        {
            return await _oidcClient.LoginAsync();
        }

        public async Task<RefreshTokenResult> RefreshAsync(string refreshToken)
        {
            return await _oidcClient.RefreshTokenAsync(refreshToken);
        }

        public async Task LogoutAsync()
        {
            var returnUriName = _settings.OAuthSettings.LogoutReturnUriQueryParameterName ?? "redirect_uri";
            var logoutParameters = new Dictionary<string, string>
            {
              {"client_id", _oidcClient.Options.ClientId },
              {returnUriName, _oidcClient.Options.RedirectUri },
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

            await _oidcClient.Options.Browser.InvokeAsync(browserOptions);
        }
    }
}
