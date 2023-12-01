namespace Maui.Authentication.Core.Oidc.Browser
{
    public class ModalPopupProvider : IPopupProvider
    {
        private const string _route = "MauiAuthenticatcationPopup";

        public async Task<ContentPage> AddToViewAsync()
        {
            var application = Application.Current ?? throw new Exception("Access to current application required");
            if (application.MainPage is Shell shell)
            {
                Routing.RegisterRoute(_route, typeof(MauiAuthenticatorPage));
                await shell.GoToAsync(_route);

                return (shell.CurrentPage as ContentPage)!;
            }
            else if (application.MainPage is NavigationPage navigationPage)
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
            var application = Application.Current ?? throw new Exception("Access to current application required");
            if (application.MainPage is Shell shell)
            {
                Routing.UnRegisterRoute(_route);
                await shell.GoToAsync("..");
            }
            else if (application.MainPage is NavigationPage navigationPage)
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
