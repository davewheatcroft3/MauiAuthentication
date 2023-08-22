using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Oidc;
using Maui.Authentication.Core.Oidc.Browser;
using Maui.Authentication.Maui;

namespace Maui.Authentication.Blazor
{
    public static class DependencyInjection
    {
        public static void AddMauiBlazorAuthentication(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
            services.AddAuthorizationCore();

            services.AddScoped<MauiBlazorAuthenticationStateProvider>();
            services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>(sp => sp.GetRequiredService<MauiBlazorAuthenticationStateProvider>());

            services.AddTransient<MauiAuthenticatorPage>();
            services.AddTransient<IPopupProvider>(_ => new ModalPopupProvider(Application.Current!));
            services.AddTransient<IdentityModel.OidcClient.Browser.IBrowser, PopupWebViewBrowser>();

            services.AddSingleton<AuthClient>();

            services.AddMauiAuthenticationShared(options);
        }
    }
}
