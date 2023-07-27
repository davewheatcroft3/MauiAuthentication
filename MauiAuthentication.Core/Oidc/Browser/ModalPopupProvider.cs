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

        public Task<ContentPage> GetGeneratedPageAsync()
        {
            var page = new ContentPage();

            if (_application.MainPage is Shell)
            {
                Shell.SetPresentationMode(page, PresentationMode.Modal);
            }

            return Task.FromResult(page);
        }

        public async Task AddToViewAsync(ContentPage page)
        {
            if (_application.MainPage is Shell shell)
            {
                Routing.RegisterRoute(_route, typeof(ContentPage));

                await shell.GoToAsync(_route);
            }
            else if (_application.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PushModalAsync(page);
            }
            else
            {
                throw new Exception(
                    "You must implement either Shell or NavigationPage navigation to use this popup provider. " +
                    "You could implement your own IPopupProvider if, for example, you wanted to use a library like Mopups.");
            }
        }

        public async Task RemoveFromViewAsync(ContentPage page)
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
