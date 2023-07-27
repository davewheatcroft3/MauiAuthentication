# DotNet MAUI OAuth/OpenID Authentication
After building the sister library:

https://github.com/davewheatcroft3/BlazorServerAuthentication

I needed to build an app using the sample authentication setup, and whilst less restricted than Blazor server, encountered quite a lot of setup involved in getting authentication working for MAUI as well.
Seems Microsoft in general doesnt want to make it easy for us regarding authentication!

The IdentityModel.OidcClient is of great help to minimize the complicate OAuth flow steps, but still requires a fair amount of setup and bits that new users will have to play about with to get it right. The goal of this project is so only a few lines of setup (and the unavoidable platform specific requirements... sigh) allows us to authenticate our app and http clients!

This library utilizes the concept of popup providers for authenticating and makes the process of using utilizing webviews or webauthenticator easier with little to no setup.

If you encounter any issues that might be related to your OAuth server setup, this is a great way to test everything works:

https://openidconnect.net/

This was a great reference for helping simplify my initial setup:

https://auth0.com/blog/add-authentication-to-dotnet-maui-apps-with-auth0/
https://auth0.com/blog/add-authentication-to-blazor-hybrid-apps-in-dotnet-maui/

NOTE: To run the sample project, ensure both the Api AND Blazor/MAUI app are running for api call to work. It easiest just to run them in sample Visual Studio instances as you can choose your setup for the mobile app when using multiple startup projects (or at least I dont know of a way!).

## Installation

### Required Steps
1. Install Nuget package (MAUI/MAUI-Blazor-Hybrid)
```
Install-Package BlazorLikeAuth.Maui
Install-Package BlazorHybridAuth.Maui
```

2. In your Program.cs add
```cs
builder.Services.AddMauiAuthentication(options =>
{
    options.UseIdTokenForHttpAuthentication = true;
    options.RefreshExpiryClockSkewInMinutes = 2;

    options.OAuthSettings.Authority = "<oauth provider authority>";
    options.OAuthSettings.Domain = "<oauth provider domain>";
    options.OAuthSettings.ClientId = "<oauth client id>";
    options.OAuthSettings.ClientSecret = "<oauth client secret>";
    options.OAuthSettings.Scope = "openid";
    options.OAuthSettings.ResponseType = "code";
    options.OAuthSettings.LogoutUrl = "<url to logout of your oauth provider>";
    options.OAuthSettings.CallbackScheme = "mauiauthapp://callback"; // Your callback uri for your app
});
```
(Or AddMauiBlazorAuthentication for the MAUI Blazor Hybrid version)

3. Ensure your http clients authenticate by setting them up via this helper extension:
```cs
builder.Services.AddAuthenticatedHttpClient<SomeApiClient>((sp, h) =>
{
    h.BaseAddress = new Uri("<your api base url>");
});
```

###Platform Specific Steps
**See the sample project for full code**<br/>
In the sample project search for "mauiauthapp" for callback scheme used in it.<br/>

1. Android
a. Add a WebAuthenticationCallbackActivity class referencing your callback scheme.
b. Add this to the AndroidManifest.xml
```xml
<queries>
    <intent>
    <action android:name="android.support.customtabs.action.CustomTabsService" />
    </intent>
</queries>
```

2. iOS + MacCatalyst
Add to Info.plist:
```xml
<key>CFBundleURLTypes</key>
<array>
    <dict>
        <key>CFBundleURLName</key>
        <string>Maui Auth App</string>
        <key>CFBundleURLSchemes</key>
        <array>
        <string>mauiauthapp</string>
        </array>
        <key>CFBundleTypeRole</key>
        <string>Editor</string>
    </dict>
</array>
```

3. Windows
Add to the Package.appxmanifest file:
```xml
<Extensions>
    <uap:Extension Category="windows.protocol">
    <uap:Protocol Name="mauiauthapp">
        <uap:DisplayName>MAUI Auth App</uap:DisplayName>
    </uap:Protocol>
    </uap:Extension>
</Extensions>
```

## Other Things Of Note
There is an AuthenticateView control for the MAUI library variant(mimicking Blazors equivalent).
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage ...
             xmlns:ma="clr-namespace:Maui.Authentication.Controls;assembly=MauiAuthentication"
             ...
...
<ma:AuthenticateView>
    <ma:AuthenticateView.Authenticated>
        
    </ma:AuthenticateView.Authenticated>
    <ma:AuthenticateView.NotAuthenticated>

    </ma:AuthenticateView.NotAuthenticated>
</ma:AuthenticateView>
```

Also may potentially add an Authenticate attribute on a per page level...

For MAUI Blazor Hybrid, make sure to use CascadingAuthenticationState in your route component:

```xml
@using Microsoft.AspNetCore.Components.Authorization

<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(Main).Assembly">
        <Found Context="routeData">
            <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)">
                <Authorizing>
                    <p>Authorizing...</p>
                </Authorizing>
                <NotAuthorized>
                    <p>Not Authorized</p>
                </NotAuthorized>
            </AuthorizeRouteView>
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>
        <NotFound>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>
</CascadingAuthenticationState>
```
