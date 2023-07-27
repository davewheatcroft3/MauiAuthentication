namespace Maui.Authentication.Core.Oidc.Browser
{
    public interface IPopupProvider
    {
        Task<ContentPage> AddToViewAsync();

        Task RemoveFromViewAsync();
    }
}
