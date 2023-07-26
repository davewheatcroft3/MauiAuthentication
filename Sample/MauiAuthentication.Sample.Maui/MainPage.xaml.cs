using MauiAuthentication.Sample.Maui.Mobile.Data;
using System.Net;

namespace MauiAuthentication.Sample.Maui.Mobile;

public partial class MainPage : ContentPage
{
	private readonly WeatherApiClient _apiClient;
	private int count = 0;

	public MainPage(WeatherApiClient apiClient)
	{
		_apiClient = apiClient;

        InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private async void OnHttpClicked(object sender, EventArgs e)
    {
		try
		{
			AuthenticatedLabel.Text = "Loading...";

            var forecasts = await _apiClient.GetForecastAsync();
            AuthenticatedLabel.Text = $"Authenticated! {forecasts.Length} forecasts returned!";
        }
		catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                AuthenticatedLabel.Text = $"Not Authenticated! Sorry no forecasts for you.";
            }
			else
            {
                AuthenticatedLabel.Text = $"There was an error communicating with the API. Ensure its running!";
            }
        }
		catch (Exception)
        {
            AuthenticatedLabel.Text = $"Unknown error communicating with the API. This is a likely a problem with the sample app...";
        }

        SemanticScreenReader.Announce(AuthenticatedLabel.Text);
    }
}

