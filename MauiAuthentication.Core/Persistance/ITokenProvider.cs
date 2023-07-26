namespace Maui.Authentication.Core.Persistance;

public record Tokens(
    string? IdToken,
    string? AccessToken,
    string? RefreshToken,
    DateTimeOffset? Expires);

public interface ITokenProvider
{
    Task<Tokens?> GetTokensAsync();

    Task<IEnumerable<(string Name, string Value)>> GetClaimsAsync();

    Task SetTokensAsync(Tokens tokens);

    Task SetClaimsAsync(IEnumerable<(string Name, string Value)> claims);

    Task ClearAllAsync();
}