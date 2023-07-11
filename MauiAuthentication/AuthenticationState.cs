using System.Security.Claims;

namespace Maui.Authentication;

public class AuthenticationState
{
    public ClaimsPrincipal? User { get; init; }

    internal AuthenticationState()
    {
    }
}