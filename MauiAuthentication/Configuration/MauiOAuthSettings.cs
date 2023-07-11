namespace Maui.Authentication.Configuration
{
    public class MauiOAuthSettings
    {
        public string Authority { get; set; } = null!;

        public string Domain { get; set; } = null!;

        public string ClientId { get; set; } = null!;

        public string ClientSecret { get; set; } = null!;

        public string RedirectUri { get; set; } = null!;

        public string LogoutUrl { get; set; } = null!;

        public string ResponseType { get; set; } = null!;

        public string Scope { get; set; } = null!;
    }
}
