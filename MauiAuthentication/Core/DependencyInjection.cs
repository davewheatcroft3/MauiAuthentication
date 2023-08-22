using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Handlers;
using Maui.Authentication.Core.Persistance;

namespace Maui.Authentication.Core
{
    public static class DependencyInjection
    {
        public static void AddMauiAuthenticationCore(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
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

            services.AddSingleton<TokenService>();

            services.AddScoped<HttpTokenHandler>();
        }

        public static void AddAuthenticatedHttpClient<T>(this IServiceCollection services, Action<IServiceProvider, HttpClient> options)
            where T : class
        {
            services.AddHttpClient<T>(options)
                .AddHttpMessageHandler<HttpTokenHandler>();
        }
    }
}
