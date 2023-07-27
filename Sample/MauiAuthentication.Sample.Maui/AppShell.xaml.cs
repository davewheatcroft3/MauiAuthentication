namespace MauiAuthentication.Sample.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("Main", typeof(MainPage));
    }
}
