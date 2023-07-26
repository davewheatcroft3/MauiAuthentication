namespace MauiAuthentication.Sample.Maui.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("Main", typeof(MainPage));
    }
}
