namespace Maui.Authentication.Core.Configuration
{
    public class MauiAuthenticationSettings
    {
        public bool UseIdTokenForHttpAuthentication { get; set; } = false;

        public int RefreshExpiryClockSkewInMinutes { get; set; } = 5;

        public MauiOAuthSettings OAuthSettings { get; set;} = new MauiOAuthSettings();
    }
}
