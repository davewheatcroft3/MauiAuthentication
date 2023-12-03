using Maui.Authentication.Blazor;
using Maui.Authentication.Core;
using MauiAuthentication.Sample.Blazor.Data;

namespace MauiAuthentication.Sample.Blazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		    builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddMauiBlazorAuthentication(options =>
            {
                options.UseIdTokenForHttpAuthentication = true;
                options.RefreshExpiryClockSkewInMinutes = 2;

                options.OAuthSettings.Authority = "https://dev-khkwv2u0z51n71ze.us.auth0.com";
                options.OAuthSettings.DiscoveryBaseUrl = "https://dev-khkwv2u0z51n71ze.us.auth0.com";
                options.OAuthSettings.ClientId = "ywc66xjPnPwnjDuZZWFgxINvuAsfEXiY";
                options.OAuthSettings.ClientSecret = "vdtMtrVWPtGWl9BbUWZ7hoksUoNldoryANueBqRwN9AJ_2xGC-lN0_U0Msbkmr5p";
                options.OAuthSettings.Scope = "openid";
                options.OAuthSettings.ResponseType = "code";
                options.OAuthSettings.LogoutUrl = "https://dev-khkwv2u0z51n71ze.us.auth0.com/oidc/logout";
                options.OAuthSettings.LogoutReturnUriQueryParameterName = "post_logout_redirect_uri";
                options.OAuthSettings.CallbackScheme = "mauiauthapp://callback";
            });

            builder.Services.AddAuthenticatedHttpClient<WeatherApiClient>((sp, h) =>
            {
                var apiBaseUrl = "https://localhost:7189";
                h.BaseAddress = new Uri(apiBaseUrl);
            });

            return builder.Build();
        }
    }
}