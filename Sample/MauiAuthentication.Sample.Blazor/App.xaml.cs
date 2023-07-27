namespace MauiAuthentication.Sample.Blazor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Switch these to see them working...!
            //MainPage = new NavigationPage(new MainPage());
            MainPage = new AppShell();
        }
    }
}