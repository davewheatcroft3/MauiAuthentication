using Maui.Authentication.Core;
using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Oidc;

namespace Maui.Authentication
{
    public static class DependencyInjection
    {
        public static void AddMauiAuthentication(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
            services.AddMauiAuthenticationCore(options);

            services.AddSingleton<MauiAuthClient>();
            services.AddSingleton<AuthClient, MauiAuthClient>();

            services.AddSingleton<TokenService>();

            services.AddSingleton<AuthenticationStateProvider>();
        }
    }
}
