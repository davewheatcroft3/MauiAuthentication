using Maui.Authentication.Core;
using Maui.Authentication.Core.Configuration;
using Maui.Authentication.Core.Oidc;
using Maui.Authentication.Core.Oidc.Browser;

namespace Maui.Authentication.Maui
{
    public static class DependencyInjection
    {
        private const string WebViewCallbackScheme = "http://localhost/callback";

        public static void AddMauiAuthentication(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
            services.AddSingleton<AuthClient>();

            services.AddSingleton<TokenService>();

            services.AddSingleton<AuthenticationStateProvider>();

            services.AddMauiAuthenticationShared(options);
        }

        internal static void AddMauiAuthenticationShared(this IServiceCollection services, Action<MauiAuthenticationSettings> options)
        {
#if WINDOWS
            var optionsOverride = (MauiAuthenticationSettings settings) =>
            {
                options(settings);
                settings.OAuthSettings.CallbackScheme = WebViewCallbackScheme;
            };
            services.AddMauiAuthenticationCore(optionsOverride);

            services.AddTransient<MauiAuthenticatorPage>();
            services.AddTransient<IPopupProvider>(_ => new ModalPopupProvider(Application.Current!));
            services.AddTransient<IdentityModel.OidcClient.Browser.IBrowser, PopupWebViewBrowser>();
#else
            services.AddMauiAuthenticationCore(options);
            services.AddTransient<IdentityModel.OidcClient.Browser.IBrowser, WebAuthenticatorBrowser>();
#endif
        }
    }
}
