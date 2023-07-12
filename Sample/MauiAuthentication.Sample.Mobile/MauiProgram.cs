using Maui.Authentication;
using MauiAuthentication.Sample.Mobile.Data;

namespace MauiAuthentication.Sample.Mobile;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddMauiAuthentication(options =>
        {
            options.UseIdTokenForHttpAuthentication = true;
            options.RefreshExpiryClockSkewInMinutes = 2;

            options.OAuthSettings.Authority = "https://dev-khkwv2u0z51n71ze.us.auth0.com";
            options.OAuthSettings.Domain = "https://dev-khkwv2u0z51n71ze.us.auth0.com";
            options.OAuthSettings.ClientId = "ywc66xjPnPwnjDuZZWFgxINvuAsfEXiY";
            options.OAuthSettings.ClientSecret = "vdtMtrVWPtGWl9BbUWZ7hoksUoNldoryANueBqRwN9AJ_2xGC-lN0_U0Msbkmr5p";
            options.OAuthSettings.Scope = "openid";
            options.OAuthSettings.ResponseType = "code";
            options.OAuthSettings.LogoutUrl = "https://dev-khkwv2u0z51n71ze.us.auth0.com/logout";
            options.OAuthSettings.CallbackScheme = "mauiauthapp://callback";
        });

        builder.Services.AddAuthenticatedHttpClient<WeatherApiClient>((sp, h) =>
        {
            var apiBaseUrl = "https://localhost:7189";
            h.BaseAddress = new Uri(apiBaseUrl);
        });

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
	}
}
