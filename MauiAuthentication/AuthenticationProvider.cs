using Maui.Authentication.Oidc;

namespace Maui.Authentication;

public class AuthenticationProvider
{
    private readonly OAuthClient _authClient;
    private readonly ITokenProvider _tokenProvider;
    private readonly TokenService _tokenService;

    public AuthenticationProvider(
        OAuthClient authClient,
        ITokenProvider tokenProvider,
        TokenService tokenService)
    {
        _authClient = authClient;
        _tokenProvider = tokenProvider;
        _tokenService = tokenService;

        _authClient.Initialize();

        _ = LoadInitialState();
    }

    public AuthenticationState State { get; private set; } = new AuthenticationState();

    public void UseWebView(WebView webView)
    {
        _authClient.Initialize(webView);
    }

    public async Task<bool> LoginAsync()
    {
        var result = await _authClient.LoginAsync();
        
        if (!result.IsError)
        {
            var claims = result.User.Claims
                .Select(x => (x.Type, x.Value))
                .ToList();

            await _tokenProvider.SetTokensAsync(new Tokens(
                result.IdentityToken,
                result.AccessToken,
                result.RefreshToken,
                result.AccessTokenExpiration));

            await _tokenProvider.SetClaimsAsync(claims);

            State = new AuthenticationState() { User = result.User };

            return true;
        }
        else
        {
            await _tokenProvider.ClearAllAsync();
            
            State = new AuthenticationState();

            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _authClient.LogoutAsync();

        await _tokenProvider.ClearAllAsync();

        State = new AuthenticationState();
    }

    public async Task<bool> RefreshAsync()
    {
        var tokens = await _tokenProvider.GetTokensAsync();
        if (tokens != null && tokens.RefreshToken != null)
        {
            var result = await _authClient.RefreshAsync(tokens.RefreshToken);
            if (!result.IsError)
            {
                await _tokenProvider.SetTokensAsync(new Tokens(
                    result.IdentityToken,
                    result.AccessToken,
                    result.RefreshToken,
                    result.AccessTokenExpiration));

                return true;
            }
        }

        return false;
    }

    private async Task LoadInitialState()
    {
        // This should refresh check should be based on decode jwt of id token
        if (await _tokenService.ShouldRefreshTokenAsync())
        {
            await RefreshAsync();
        }

        var claimsPrincipal = await _tokenService.GetClaimsAsync();

        State = new AuthenticationState()
        {
            User = claimsPrincipal
        };
    }
}