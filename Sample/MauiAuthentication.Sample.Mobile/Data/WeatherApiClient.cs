using System.Net.Http.Json;

namespace MauiAuthentication.Sample.Mobile.Data
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

            if (response.IsSuccessStatusCode)
            {
                var parsedContent = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

                return parsedContent ?? new WeatherForecast[0];
            }
            else
            {
                return new WeatherForecast[0];
            }
        }
    }
}