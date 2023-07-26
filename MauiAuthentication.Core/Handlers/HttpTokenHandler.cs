using Maui.Authentication.Core.Oidc;

namespace Maui.Authentication.Core.Handlers
{
    public class HttpTokenHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;
        private readonly AuthClient _authClient;

        public HttpTokenHandler(TokenService tokenService, AuthClient authClient)
        {
            _tokenService = tokenService;
            _authClient = authClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetBearerTokenAsync();

            if (token != null)
            {
                if (await _tokenService.ShouldRefreshTokenAsync())
                {
                    var refreshToken = await _tokenService.GetRefreshTokenAsync();
                    if (refreshToken != null)
                    {
                        var result = await _authClient.RefreshAsync(refreshToken);
                        if (!result.IsError)
                        {
                            token = await _tokenService.GetBearerTokenAsync();
                        }
                    }
                    else
                    {
                        await _authClient.LogoutAsync();
                    }
                }
            }

            if (token != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
