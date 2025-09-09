using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace WeatherApp
{
    public class WeatherAPI : IDisposable
    {
        private readonly string apiKey;
        private readonly string baseUrl;
        private readonly string forecastUrl;
        private readonly HttpClient httpClient;

        public WeatherAPI(string apiKey = Config.API_KEY, string baseUrl = Config.BASE_URL, string forecastUrl = Config.FORECAST_URL)
        {
            this.apiKey = apiKey;
            this.baseUrl = baseUrl;
            this.forecastUrl = forecastUrl;
            this.httpClient = new HttpClient();
        }

        public async Task<JsonElement?> GetWeatherAsync(string city)
        {
            var paramsDict = new Dictionary<string, string>
            {
                {"q", city},
                {"appid", apiKey},
                {"units", "imperial"}
            };

            try
            {
                var queryString = await new FormUrlEncodedContent(paramsDict).ReadAsStringAsync();
                var response = await httpClient.GetAsync($"{baseUrl}?{queryString}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(content);

                await WriteToFileAsync(city, data, "weather");
                return data;
            }
            catch (HttpRequestException httpErr)
            {
                Console.WriteLine($"HTTP error occurred: {httpErr.Message}");
            }
            catch (Exception err)
            {
                Console.WriteLine($"Other error occurred: {err.Message}");
            }

            return null;
        }

        public async Task<JsonElement?> GetForecastAsync(string city)
        {
            var paramsDict = new Dictionary<string, string>
            {
                {"q", city},
                {"appid", apiKey},
                {"units", "imperial"}
            };

            try
            {
                var queryString = await new FormUrlEncodedContent(paramsDict).ReadAsStringAsync();
                var response = await httpClient.GetAsync($"{forecastUrl}?{queryString}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(content);

                await WriteToFileAsync(city, data, "forecast");
                return data;
            }
            catch (HttpRequestException httpErr)
            {
                Console.WriteLine($"HTTP error occurred: {httpErr.Message}");
            }
            catch (Exception err)
            {
                Console.WriteLine($"Other error occurred: {err.Message}");
            }

            return null;
        }

        public async Task WriteToFileAsync(string city, JsonElement data, string type)
        {
            var directory = "data";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Sanitize city name to avoid invalid file names
            var safeCity = string.Concat(city.Split(Path.GetInvalidFileNameChars()));
            var filePath = Path.Combine(directory, $"{safeCity}_{type}_data.txt");
            await File.WriteAllTextAsync(filePath, data.ToString());
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}