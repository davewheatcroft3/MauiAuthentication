using Maui.Authentication.Blazor.Oidc;
using Maui.Authentication.Core.Persistance;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Mau.Authentication.Blazor
{
    public class MauiBlazorAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly MauiBlazorAuthClient auth0Client;
        private readonly ITokenProvider _tokenProvider;

        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public MauiBlazorAuthenticationStateProvider(MauiBlazorAuthClient client, ITokenProvider tokenProvider)
        {
            auth0Client = client;
            _tokenProvider = tokenProvider;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(currentUser));

        public Task LoginAsync()
        {
            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LogInAsyncCore()
            {
                var user = await LoginWithAuth0Async();
                currentUser = user;

                return new AuthenticationState(currentUser);
            }
        }

        private async Task<ClaimsPrincipal> LoginWithAuth0Async()
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());
            var loginResult = await auth0Client.LoginAsync();

            if (!loginResult.IsError)
            {
                await _tokenProvider.SetTokensAsync(new Tokens(
                    loginResult.IdentityToken,
                    loginResult.AccessToken,
                    loginResult.RefreshToken,
                    loginResult.AccessTokenExpiration));

                authenticatedUser = loginResult.User;
            }
            return authenticatedUser;
        }

        public async Task LogoutAsync()
        {
            await auth0Client.LogoutAsync();

            currentUser = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
        }
    }
}
