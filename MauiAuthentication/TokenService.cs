using Maui.Authentication.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Maui.Authentication;

public class TokenService
{
    private readonly ITokenProvider _tokenProvider;
    private readonly MauiAuthenticationSettings _settings;

    public TokenService(ITokenProvider tokenProvider, IOptions<MauiAuthenticationSettings> settings)
    {
        _tokenProvider = tokenProvider;
        _settings = settings.Value;
    }
       
    public async Task<string?> GetBearerTokenAsync()
    {
        var tokens = await _tokenProvider.GetTokensAsync();

        var token = _settings.UseIdTokenForHttpAuthentication
                               ? tokens?.IdToken
                               : tokens?.AccessToken;

        return token;
    }

    public async Task<ClaimsPrincipal> GetClaimsAsync()
    {
        var claimPairs = await _tokenProvider.GetClaimsAsync();

        var claimsPrincipal = new ClaimsPrincipal();

        var claims = new List<Claim>();

        foreach (var claimPair in claimPairs)
        {
            claims.Add(new Claim(claimPair.Name, claimPair.Value));
        }

        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "none"));

        return claimsPrincipal;
    }

    public async Task<bool> ShouldRefreshTokenAsync()
    {
        var tokens = await _tokenProvider.GetTokensAsync();

        if (tokens == null || tokens.IdToken == null || tokens.RefreshToken == null)
        {
            return false;
        }

        var jwtService = new JwtService();
        var decodedToken = jwtService.DecodeToken(tokens.IdToken);

        if (decodedToken != null)
        {
            return ShouldRefreshToken(decodedToken.ValidTo);
        }

        return false;
    }

    private bool ShouldRefreshToken(DateTime expiresAt)
    {
        if (DateTime.UtcNow.AddMinutes(_settings.RefreshExpiryClockSkewInMinutes) >= expiresAt)
        {
            return true;
        }

        return false;
    }
}