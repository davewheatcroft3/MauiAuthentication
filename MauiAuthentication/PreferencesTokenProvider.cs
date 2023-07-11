using System.Text.Json;

namespace Maui.Authentication;

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

internal class PreferencesTokenProvider : ITokenProvider
{
    private const string IdTokenKey = "MauiAuth_IdTokens";
    private const string AccessTokenKey = "MauiAuth_AccessToken";
    private const string RefreshTokenKey = "MauiAuth_RefreshToken";
    private const string ExpiresAtKey = "MauiAuth_ExpiresAt";
    private const string ClaimsKey = "MauiAuth_Claims";

    public Task<Tokens?> GetTokensAsync()
    {
        try
        {
            var idToken = Preferences.Get(IdTokenKey, null);
            var accessToken = Preferences.Get(AccessTokenKey, null);
            var refreshToken = Preferences.Get(RefreshTokenKey, null);
            var expiresAtSerialized = Preferences.Get(ExpiresAtKey, null);

            DateTimeOffset? expiresAt = expiresAtSerialized != null
                ? DateTimeOffset.Parse(expiresAtSerialized)
                : null;

            var tokens = new Tokens(idToken, accessToken, refreshToken, expiresAt);

            return Task.FromResult<Tokens?>(tokens);
        }
        catch
        {
            return Task.FromResult<Tokens?>(null);
        }
    }

    public Task<IEnumerable<(string Name, string Value)>> GetClaimsAsync()
    {
        try
        {
            var serialized = Preferences.Get(ClaimsKey, null);
            if (serialized == null)
            {
                return Task.FromResult<IEnumerable<(string Name, string Value)>>(new List<(string Name, string Value)>());
            }

            var deserialized = JsonSerializer.Deserialize<IEnumerable<(string Name, string Value)>>(serialized);

            return Task.FromResult(deserialized ?? new List<(string Name, string Value)>());
        }
        catch
        {
            return Task.FromResult<IEnumerable<(string Name, string Value)>>(new List<(string Name, string Value)>());
        }
    }

    public Task SetTokensAsync(Tokens tokens)
    {
        Preferences.Set(IdTokenKey, tokens.IdToken);
        Preferences.Set(AccessTokenKey, tokens.AccessToken);
        Preferences.Set(RefreshTokenKey, tokens.RefreshToken);
        Preferences.Set(ExpiresAtKey, tokens.Expires.HasValue ? tokens.Expires.Value.ToString() : null);

        return Task.CompletedTask;
    }

    public Task SetClaimsAsync(IEnumerable<(string Name, string Value)> claims)
    {
        var serializedClaims = JsonSerializer.Serialize(claims.Select(x => new ClaimPair() { Name = x.Name, Value = x.Value }));
        Preferences.Set(ClaimsKey, serializedClaims);

        return Task.CompletedTask;
    }

    public Task ClearAllAsync()
    {
        Preferences.Clear(IdTokenKey);
        Preferences.Clear(AccessTokenKey);
        Preferences.Clear(RefreshTokenKey);
        Preferences.Clear(ExpiresAtKey);
        Preferences.Clear(ClaimsKey);
        return Task.CompletedTask;
    }

    private class ClaimPair
    {
        public string Name { get; init; } = null!;
        public string Value { get; init; } = null!;
    }
}