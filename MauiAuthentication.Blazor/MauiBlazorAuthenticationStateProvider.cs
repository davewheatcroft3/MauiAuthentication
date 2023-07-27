using Maui.Authentication.Core.Oidc;
using Maui.Authentication.Core.Persistance;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Mau.Authentication.Blazor
{
    public class MauiBlazorAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthClient _authClient;
        private readonly ITokenProvider _tokenProvider;

        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public MauiBlazorAuthenticationStateProvider(AuthClient client, ITokenProvider tokenProvider)
        {
            _authClient = client;
            _tokenProvider = tokenProvider;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Initialize();

            return new AuthenticationState(_currentUser);
        }

        public async Task LoginAsync()
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());
            var loginResult = await _authClient.LoginAsync();

            if (!loginResult.IsError)
            {
                await _tokenProvider.SetTokensAsync(new Tokens(
                    loginResult.IdentityToken,
                    loginResult.AccessToken,
                    loginResult.RefreshToken,
                    loginResult.AccessTokenExpiration));

                var claims = loginResult.User.Claims
                    .Select(x => (x.Type, x.Value))
                    .ToList();

                await _tokenProvider.SetClaimsAsync(claims);

                authenticatedUser = loginResult.User;
            }

            _currentUser = authenticatedUser;

            var state = new AuthenticationState(_currentUser);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        public async Task LogoutAsync()
        {
            await _authClient.LogoutAsync();

            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        private async Task Initialize()
        {
            var claims = await _tokenProvider.GetClaimsAsync();

            if (claims != null)
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity(claims.Select(x => new Claim(x.Name, x.Value))));
            }
        }
    }
}
