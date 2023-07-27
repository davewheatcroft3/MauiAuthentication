namespace Maui.Authentication.Core.Oidc.Browser
{
    public class MauiAuthenticatorPage : ContentPage
    {
        public MauiAuthenticatorPage()
        {
            Shell.SetPresentationMode(this, PresentationMode.Modal);
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
