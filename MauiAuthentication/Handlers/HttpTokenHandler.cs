namespace Maui.Authentication.Handlers
{
    public class HttpTokenHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;
        private readonly AuthenticationStateProvider _authProvider;

        public HttpTokenHandler(TokenService tokenService, AuthenticationStateProvider authProvider)
        {
            _tokenService = tokenService;
            _authProvider = authProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetBearerTokenAsync();

            if (token != null)
            {
                if (await _tokenService.ShouldRefreshTokenAsync())
                {
                    if (await _authProvider.RefreshAsync())
                    {
                        token = await _tokenService.GetBearerTokenAsync();
                    }
                    else
                    {
                        await _authProvider.LogoutAsync();
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
