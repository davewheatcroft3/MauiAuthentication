using Maui.Authentication;

namespace MauiAuthentication.Sample.Mobile
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthenticationProvider _authProvider;

        public LoginPage(AuthenticationProvider authProvider)
        {
            _authProvider = authProvider;

            InitializeComponent();

            CheckInitialState();
#if WINDOWS
            _authProvider.UseWebView(WebViewInstance);
#endif
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

        private void CheckInitialState()
        {
            if (_authProvider.State.User?.Identity?.IsAuthenticated == true)
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
