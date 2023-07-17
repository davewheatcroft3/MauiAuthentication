using Maui.Authentication.Configuration;
using Maui.Authentication.Handlers;
using Maui.Authentication.Oidc;
using Microsoft.Extensions.DependencyInjection;

namespace Maui.Authentication
{
    public static class DependencyInjection
    {
        public static void AddMauiAuthentication(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
            var settings = new MauiAuthenticationSettings();
            options.Invoke(settings);

            services.Configure<MauiAuthenticationSettings>(options =>
            {
                options.RefreshExpiryClockSkewInMinutes = settings.RefreshExpiryClockSkewInMinutes;
                options.UseIdTokenForHttpAuthentication = settings.UseIdTokenForHttpAuthentication;
                options.OAuthSettings = settings.OAuthSettings ?? throw new Exception("Must set all OAuth properties");
            });

            services.AddScoped<ITokenProvider, PreferencesTokenProvider>();

            services.AddSingleton<OAuthClient>();
            services.AddSingleton<BrowserFactory>();

            services.AddSingleton<TokenService>();

            services.AddScoped<HttpTokenHandler>();

            services.AddSingleton<AuthenticationStateProvider>();
        }

        public static void AddAuthenticatedHttpClient<T>(this IServiceCollection services, Action<IServiceProvider, HttpClient> options)
            where T : class
        {
            services.AddHttpClient<T>(options)
                .AddHttpMessageHandler<HttpTokenHandler>();
        }
    }
}
