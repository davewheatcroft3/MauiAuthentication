using IdentityModel.OidcClient.Browser;

namespace Maui.Authentication.Core.Oidc.Browser
{
    public class PopupWebViewBrowser : IdentityModel.OidcClient.Browser.IBrowser
    {
        private readonly IPopupProvider _popupProvider;

        public PopupWebViewBrowser(IPopupProvider popupProvider)
        {
            _popupProvider = popupProvider;
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var page = await _popupProvider.GetGeneratedPageAsync();

            var webView = new WebView();
            page.Content = webView;

            var tcs = new TaskCompletionSource<BrowserResult>();

            webView.Navigated += (sender, e) =>
            {
                if (e.Url.StartsWith(options.EndUrl))
                {
                    if (tcs.Task.Status != TaskStatus.RanToCompletion)
                    {
                        tcs.SetResult(new BrowserResult
                        {
                            ResultType = BrowserResultType.Success,
                            Response = e.Url.ToString()
                        });
                    }
                }

            };

            await _popupProvider.AddToViewAsync(page);

            webView.Source = new UrlWebViewSource { Url = options.StartUrl };

            var result = await tcs.Task;

            _ = _popupProvider.RemoveFromViewAsync(page);

            return result;
        }
    }
}
