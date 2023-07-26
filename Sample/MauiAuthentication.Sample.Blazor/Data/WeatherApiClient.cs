using System.Net.Http.Json;

namespace MauiAuthentication.Sample.Blazor.Data
{
    public class WeatherApiClient
    {
        private readonly HttpClient _httpClient;

        public WeatherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
            var response = await _httpClient.GetAsync("/weatherforecast");

            response.EnsureSuccessStatusCode();

            var parsedContent = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

            return parsedContent ?? new WeatherForecast[0];
        }
    }
}