namespace Maui.Authentication.Controls;

public class AuthenticateView : ContentView, IDisposable
{
    private readonly AuthenticationProvider _authProvider;

    private View? _authenticated;
    private View? _notAuthenticated;

    public AuthenticateView()
    {
        _authProvider = Maui.Authentication.Controls.ServiceProvider.GetService<AuthenticationProvider>()
            ?? throw new Exception("You ensure the AddMauiAuthentication DI extension method has been called");

        _authProvider.StateChanged += AuthProvider_StateChanged;

        SetVisibility();
    }

    private void AuthProvider_StateChanged(object? sender, AuthenticationState e)
    {
        SetVisibility();
    }

    public View? Authenticated
    {
        get => _authenticated;
        set
        {
            _authenticated = value;
            SetVisibility();
        }
    }

    public View? NotAuthenticated
    {
        get => _notAuthenticated;
        set
        {
            _notAuthenticated = value;
            SetVisibility();
        }
    }

    public void Dispose()
    {
        _authProvider.StateChanged -= AuthProvider_StateChanged;
    }

    private async void SetVisibility()
    {
        var state = await _authProvider.GetStateAsync();
        var isAuthenticated = state?.User?.Identity?.IsAuthenticated == true;

        if (isAuthenticated)
        {
            if (Authenticated != null)
            {
                Content = Authenticated;
            }
            else
            {
                Content = null;
            }
        }
        else
        {
            if (NotAuthenticated != null)
            {
                Content = NotAuthenticated;
            }
            else
            {
                Content = null;
            }
        }
    }
}