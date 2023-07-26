using Mau.Authentication.Blazor;
using Maui.Authentication.Blazor.Oidc;
using Maui.Authentication.Core;
using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Oidc;
using Microsoft.AspNetCore.Components.Authorization;

namespace Maui.Authentication.Blazor
{
    public static class DependencyInjection
    {
        public static void AddMauiAuthenticationBlazor(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
            services.AddAuthorizationCore();

            services.AddMauiAuthenticationCore(options);

            services.AddScoped<MauiBlazorAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, MauiBlazorAuthenticationStateProvider>();

            services.AddSingleton<MauiBlazorAuthClient>();
            services.AddSingleton<AuthClient, MauiBlazorAuthClient>();

        }
    }
}
