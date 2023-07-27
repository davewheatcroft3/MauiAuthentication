namespace Maui.Authentication.Core.Configuration
{
    public class MauiOAuthSettings
    {
        public string Authority { get; set; } = null!;

        public string DiscoveryBaseUrl { get; set; } = null!;

        public string ClientId { get; set; } = null!;

        public string ClientSecret { get; set; } = null!;

        public string CallbackScheme { get; set; } = null!;

        public string LogoutUrl { get; set; } = null!;

        public string? LogoutReturnUriQueryParameterName { get; set; } = null!;

        public string ResponseType { get; set; } = null!;

        public string Scope { get; set; } = null!;
    }
}
