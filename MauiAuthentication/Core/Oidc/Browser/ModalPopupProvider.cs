namespace Maui.Authentication.Core.Oidc.Browser
{
    public class ModalPopupProvider : IPopupProvider
    {
        private readonly Application _application;

        private const string _route = "MauiAuthenticatcationPopup";

        public ModalPopupProvider(Application application)
        {
            _application = application;
        }

        public async Task<ContentPage> AddToViewAsync()
        {
            if (_application.MainPage is Shell shell)
            {
                Routing.RegisterRoute(_route, typeof(MauiAuthenticatorPage));
                await shell.GoToAsync(_route);

                return (shell.CurrentPage as ContentPage)!;
            }
            else if (_application.MainPage is NavigationPage navigationPage)
            {
                var page = new ContentPage();
                await navigationPage.Navigation.PushModalAsync(page);

                return page;
            }
            else
            {
                throw new Exception(
                    "You must implement either Shell or NavigationPage navigation to use this popup provider. " +
                    "You could implement your own IPopupProvider if, for example, you wanted to use a library like Mopups.");
            }

        }

        public async Task RemoveFromViewAsync()
        {
            if (_application.MainPage is Shell shell)
            {
                Routing.UnRegisterRoute(_route);
                await shell.GoToAsync("..");
            }
            else if (_application.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PopModalAsync();
            }
            else
            {
                throw new Exception(
                    "You must implement either Shell or NavigationPage navigation to use this popup provider. " +
                    "You could implement your own IPopupProvider if, for example, you wanted to use a library like Mopups.");
            }
        }
    }
}
