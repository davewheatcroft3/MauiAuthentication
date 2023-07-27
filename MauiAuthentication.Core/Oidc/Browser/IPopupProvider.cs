namespace Maui.Authentication.Core.Oidc.Browser
{
    public interface IPopupProvider
    {
        Task<ContentPage> GetGeneratedPageAsync();

        Task AddToViewAsync(ContentPage page);

        Task RemoveFromViewAsync(ContentPage page);
    }
}
