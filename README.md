# DotNet MAUI OAuth/OpenID Authentication
After building the sister library:

https://github.com/davewheatcroft3/BlazorServerAuthentication

I needed to build an app using the sample authentication setup, and whilst less restricted than Blazor server, encountered quite a lot of setup involved in getting authentication working for MAUI as well.
Seems Microsoft in general doesnt want to make it easy for us regarding authentication!

The IdentityModel.OidcClient is of great help to minimize the complicate OAuth flow steps, but still requires a fair amount of setup and bits that new users will have to play about with to get it right. The goal of this project is so only a few lines of setup (and the unavoidable platform specific requirements... sigh) allows us to authenticate our app and http clients!

This library uses the concepts from Blazor - AuthenticationState and AuthenticationStateProvider as well as an injectable ITokenProvider (in this case the in built MAUI preferences system is used - but you can inject your own). The sample project uses a sample OAuth client to show the flow.

If you encounter any issues that might be related to your OAuth server setup, this is a great way to test everything works:

https://openidconnect.net/

This was a great reference for helping simplify my initial setup:

https://auth0.com/blog/add-authentication-to-dotnet-maui-apps-with-auth0/

NOTE: To run the sample project, ensure both the Api AND Mobile app are running for api call to work. It easiest just to run them in sample Visual Studio instances as you can choose your setup for the mobile app when using multiple startup projects (or at least I dont know of a way!).

## Installation

### Required Steps
1. Install Nuget package
```
Install-Package Maui.Authentication
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
#if WINDOWS
    options.OAuthSettings.RedirectUri = "http://localhost/callback"; // Windows uses a webview with localhost
#else
    options.OAuthSettings.RedirectUri = "mauiauthapp://callback"; // Your callback uri for your app
#endif
});
```

3. Ensure your http clients authenticate by setting them up via this helper extension:
```cs
builder.Services.AddAuthenticatedHttpClient<SomeApiClient>((sp, h) =>
{
    h.BaseAddress = new Uri("<your api base url>");
});
```

4. Platform Specific Steps:
**See the sample project for full code**
In the sample project search for "mauiauthapp" for callback scheme used in it.

a. Android (1):
Add a WebAuthenticationCallbackActivity class referencing your callback scheme.
b. Android (2):
Add this to the AndroidManifest.xml
```xml
<queries>
    <intent>
    <action android:name="android.support.customtabs.action.CustomTabsService" />
    </intent>
</queries>
```
c. iOS
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
d. MacCatalyst
__NOTE: same as iOS__
e. Windows
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
There are plans to add an AuthenticateView or similar to further mimic Blazor Server...

Also potentially add an Authenticate attribute on a per page level...

