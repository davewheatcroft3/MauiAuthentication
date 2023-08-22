using System.Security.Claims;

namespace Maui.Authentication.Maui;

public class AuthenticationState
{
    public ClaimsPrincipal? User { get; init; }

    internal AuthenticationState()
    {
    }
}