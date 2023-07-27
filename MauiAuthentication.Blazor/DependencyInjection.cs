using Mau.Authentication.Blazor;
using Maui.Authentication.Core;
using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Oidc;
using Maui.Authentication.Core.Oidc.Browser;
using Microsoft.AspNetCore.Components.Authorization;

namespace Maui.Authentication.Blazor
{
    public static class DependencyInjection
    {
        private const string WebViewCallbackScheme = "http://localhost/callback";

        public static void AddMauiBlazorAuthentication(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
            services.AddAuthorizationCore();

            services.AddScoped<MauiBlazorAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<MauiBlazorAuthenticationStateProvider>());

            services.AddTransient<MauiAuthenticatorPage>();
            services.AddTransient<IPopupProvider>(_ => new ModalPopupProvider(Application.Current!));
            services.AddTransient<IdentityModel.OidcClient.Browser.IBrowser, PopupWebViewBrowser>();

            services.AddSingleton<AuthClient>();

            var optionsOverride = (MauiAuthenticationSettings settings) =>
            {
                options(settings);
                settings.OAuthSettings.CallbackScheme = WebViewCallbackScheme;
            };
            services.AddMauiAuthenticationCore(optionsOverride);
        }
    }
}
