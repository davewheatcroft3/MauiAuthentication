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
            var tcs = new TaskCompletionSource<BrowserResult>();

            var page = await _popupProvider.AddToViewAsync();

            var webView = new WebView();
            page.Content = webView;

            webView.Navigated += (sender, e) =>
            {
                if (e.Url.StartsWith(options.EndUrl) || cancellationToken.IsCancellationRequested)
                {
                    if (tcs.Task.Status != TaskStatus.RanToCompletion)
                    {
                        tcs.SetResult(new BrowserResult
                        {
                            ResultType = cancellationToken.IsCancellationRequested
                                ? BrowserResultType.UserCancel
                                : BrowserResultType.Success,
                            Response = e.Url.ToString()
                        });
                    }
                }
            };

            webView.Source = new UrlWebViewSource { Url = options.StartUrl };

            var result = await tcs.Task;

            await _popupProvider.RemoveFromViewAsync();

            return result;
        }
    }
}
