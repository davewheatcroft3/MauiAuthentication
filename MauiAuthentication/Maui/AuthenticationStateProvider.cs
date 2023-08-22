using Maui.Authentication.Core;
using Maui.Authentication.Core.Oidc;
using Maui.Authentication.Core.Persistance;

namespace Maui.Authentication.Maui;

public class AuthenticationStateProvider
{
    private readonly AuthClient _authClient;
    private readonly ITokenProvider _tokenProvider;
    private readonly TokenService _tokenService;

    public AuthenticationStateProvider(
        AuthClient authClient,
        ITokenProvider tokenProvider,
        TokenService tokenService)
    {
        _authClient = authClient;
        _tokenProvider = tokenProvider;
        _tokenService = tokenService;
    }

    private AuthenticationState? _state;

    public async Task<AuthenticationState> GetStateAsync()
    {
        if ( _state == null)
        {
            await Initialize();
        }

        return _state!;
    }

    public event EventHandler<AuthenticationState>? StateChanged;

    public async Task<bool> LoginAsync()
    {
        var result = await _authClient.LoginAsync();

        if (!result.IsError)
        {
            await _tokenProvider.SetTokensAsync(new Tokens(
                result.IdentityToken,
                result.AccessToken,
                result.RefreshToken,
                result.AccessTokenExpiration));

            var claims = result.User.Claims
                .Select(x => (x.Type, x.Value))
                .ToList();

            await _tokenProvider.SetClaimsAsync(claims);

            _state = new AuthenticationState() { User = result.User };

            TriggerStateChangedEvent();

            return true;
        }
        else
        {
            await _tokenProvider.ClearAllAsync();

            _state = new AuthenticationState();

            TriggerStateChangedEvent();

            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _authClient.LogoutAsync();

        await _tokenProvider.ClearAllAsync();

        _state = new AuthenticationState();

        TriggerStateChangedEvent();
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

    private void TriggerStateChangedEvent()
    {
        if (StateChanged != null)
        {
            StateChanged(this, _state!);
        }
    }

    private async Task Initialize()
    {
        var claimsPrincipal = await _tokenService.GetClaimsAsync();

        if (claimsPrincipal.Identity?.IsAuthenticated == true)
        {
            // This should refresh check should be based on decode jwt of id token
            if (await _tokenService.ShouldRefreshTokenAsync())
            {
                if (!await RefreshAsync())
                {
                    await _tokenProvider.ClearAllAsync();
                    _state = new AuthenticationState();
                    return;
                }
            }
        }

        _state = new AuthenticationState()
        {
            User = claimsPrincipal
        };
    }
}