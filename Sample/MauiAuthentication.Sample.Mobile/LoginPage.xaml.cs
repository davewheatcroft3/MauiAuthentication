using Maui.Authentication;

namespace MauiAuthentication.Sample.Mobile
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthenticationStateProvider _authProvider;

        public LoginPage(AuthenticationStateProvider authProvider)
        {
            _authProvider = authProvider;

            InitializeComponent();

#if WINDOWS
            _authProvider.UseWebView(WebViewInstance);
#endif
        }

        protected override async void OnAppearing()
        {
            await CheckInitialState();

            base.OnAppearing();
        }

        private async void ButtonGoAnyway_Clicked(object sender, EventArgs e)
        {
            await AppShell.Current.GoToAsync("Main");
        }

        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            var result = await _authProvider.LoginAsync();

            if (result)
            {
                ResetToLoggedIn();
            }
            else
            {
                ResetToLoggedOut();
            }
        }

        private async void ButtonLogout_Clicked(object sender, EventArgs e)
        {
            await _authProvider.LogoutAsync();

            ResetToLoggedOut();
        }

        private async Task CheckInitialState()
        {
            var state = await _authProvider.GetStateAsync();
            if (state.User?.Identity?.IsAuthenticated == true)
            {
                ResetToLoggedIn();
            }
            else
            {
                ResetToLoggedOut();
            }
        }

        private void ResetToLoggedIn()
        {
            LabelLoginStatus.Text = $"Logged in";

            ButtonLogin.IsVisible = false;
            ButtonLogout.IsVisible = true;
        }

        private void ResetToLoggedOut()
        {
            LabelLoginStatus.Text = "Not logged in";

            ButtonLogin.IsVisible = true;
            ButtonLogout.IsVisible = false;
        }
    }
}
