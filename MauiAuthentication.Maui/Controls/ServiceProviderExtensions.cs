namespace Maui.Authentication.Controls
{
    /// <summary>
    /// Only way I could get view models binding nicely from DI for content views...
    /// https://github.com/dotnet/maui/discussions/8363
    /// </summary>
    internal static class ServiceProvider
    {
        public static TService? GetService<TService>()
            => Current.GetService<TService>();

        public static IServiceProvider Current
            =>
#if WINDOWS10_0_17763_0_OR_GREATER
			MauiWinUIApplication.Current.Services;
#elif ANDROID
                MauiApplication.Current.Services;
#elif IOS || MACCATALYST
			MauiUIApplicationDelegate.Current.Services;
#else
			throw new Exception("Unsupport platform");
#endif
    }
}
