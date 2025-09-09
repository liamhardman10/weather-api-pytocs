using System;
using System.Text.Json;
using System.Windows.Forms;


namespace WeatherApp
{
    public partial class WeatherAppForm : Form
    {
        private WeatherAPI weatherAPI;
        private JsonElement? currentWeatherData; // <-- Add this line

        public WeatherAppForm()
        {
            InitializeComponent();
            weatherAPI = new WeatherAPI();
        }

        private async void getWeatherButton_Click(object sender, EventArgs e)
        {
            await GetWeatherData();
        }

        private async void getForecastButton_Click(object sender, EventArgs e)
        {
            await GetForecastData();
        }

        private async void saveWeatherButton_Click(object sender, EventArgs e)
        {
            await SaveWeatherData();
        }

        private async Task GetWeatherData()
        {
            string city = cityTextBox.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Please enter a city name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var data = await weatherAPI.GetWeatherAsync(city);
                if (!data.HasValue)
                {
                    MessageBox.Show("Failed to get weather data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dataValue = data.Value;
                if (dataValue.TryGetProperty("cod", out JsonElement codElement))
                {
                    if ((codElement.ValueKind == JsonValueKind.Number && codElement.GetInt32() != 200) ||
                        (codElement.ValueKind == JsonValueKind.String && codElement.GetString() != "200"))
                    {
                        string message = dataValue.TryGetProperty("message", out JsonElement messageElement)
                            ? messageElement.GetString()
                            : "Failed to get weather data";
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                DisplayWeather(dataValue);
                currentWeatherData = dataValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task GetForecastData()
        {
            string city = cityTextBox.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Please enter a city name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var data = await weatherAPI.GetForecastAsync(city);
                if (!data.HasValue)
                {
                    MessageBox.Show("Failed to get forecast data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dataValue = data.Value;
                if (dataValue.TryGetProperty("cod", out JsonElement codElement))
                {
                    if ((codElement.ValueKind == JsonValueKind.Number && codElement.GetInt32() != 200) ||
                        (codElement.ValueKind == JsonValueKind.String && codElement.GetString() != "200"))
                    {
                        string message = dataValue.TryGetProperty("message", out JsonElement messageElement)
                            ? messageElement.GetString()
                            : "Failed to get forecast data";
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                DisplayForecast(dataValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayWeather(JsonElement weatherData)
        {
            weatherInfoTextBox.Clear();

            try
            {
                string weatherDesc = weatherData.GetProperty("weather")[0].GetProperty("description").GetString();
                double temp = weatherData.GetProperty("main").GetProperty("temp").GetDouble();
                int humidity = weatherData.GetProperty("main").GetProperty("humidity").GetInt32();
                double windSpeed = weatherData.GetProperty("wind").GetProperty("speed").GetDouble();
                int pressure = weatherData.GetProperty("main").GetProperty("pressure").GetInt32();

                string visibility = weatherData.TryGetProperty("visibility", out JsonElement visibilityElement)
                    ? visibilityElement.GetInt32().ToString()
                    : "N/A";

                int cloudiness = weatherData.GetProperty("clouds").GetProperty("all").GetInt32();
                long sunrise = weatherData.GetProperty("sys").GetProperty("sunrise").GetInt64();
                long sunset = weatherData.GetProperty("sys").GetProperty("sunset").GetInt64();
                string cityName = weatherData.GetProperty("name").GetString();

                string weatherInfoText = $"City: {cityName}{Environment.NewLine}" +
                                         $"Weather: {weatherDesc}{Environment.NewLine}" +
                                         $"Temperature: {temp}°F{Environment.NewLine}" +
                                         $"Humidity: {humidity}%{Environment.NewLine}" +
                                         $"Wind Speed: {windSpeed} m/s{Environment.NewLine}" +
                                         $"Pressure: {pressure} hPa{Environment.NewLine}" +
                                         $"Visibility: {visibility} meters{Environment.NewLine}" +
                                         $"Cloudiness: {cloudiness}%{Environment.NewLine}" +
                                         $"Sunrise: {DateTimeOffset.FromUnixTimeSeconds(sunrise).ToLocalTime():hh:mm:ss tt}{Environment.NewLine}" +
                                         $"Sunset: {DateTimeOffset.FromUnixTimeSeconds(sunset).ToLocalTime():hh:mm:ss tt}{Environment.NewLine}";

                weatherInfoTextBox.Text = weatherInfoText;
            }
            catch (Exception ex)
            {
                weatherInfoTextBox.Text = $"Error parsing weather data: {ex.Message}";
            }
        }

        private void DisplayForecast(JsonElement forecastData)
        {
            weatherInfoTextBox.Clear();

            try
            {
                var forecastList = forecastData.GetProperty("list");
                string forecastInfo = "";

                foreach (JsonElement forecast in forecastList.EnumerateArray())
                {
                    string dtTxt = forecast.GetProperty("dt_txt").GetString();
                    string weatherDesc = forecast.GetProperty("weather")[0].GetProperty("description").GetString();
                    double temp = forecast.GetProperty("main").GetProperty("temp").GetDouble();
                    int humidity = forecast.GetProperty("main").GetProperty("humidity").GetInt32();
                    double windSpeed = forecast.GetProperty("wind").GetProperty("speed").GetDouble();

                    forecastInfo += $"Date/Time: {dtTxt}{Environment.NewLine}" +
                                    $"Weather: {weatherDesc}{Environment.NewLine}" +
                                    $"Temperature: {temp}°F{Environment.NewLine}" +
                                    $"Humidity: {humidity}%{Environment.NewLine}" +
                                    $"Wind Speed: {windSpeed} m/s{Environment.NewLine}" +
                                    "-------------------------" + Environment.NewLine;
                }

                weatherInfoTextBox.Text = forecastInfo;
            }
            catch (Exception ex)
            {
                weatherInfoTextBox.Text = $"Error parsing forecast data: {ex.Message}";
            }
        }

        private async Task SaveWeatherData()
        {
            if (currentWeatherData.HasValue)
            {
                string city = cityTextBox.Text.Trim();
                await weatherAPI.WriteToFileAsync(city, currentWeatherData.Value, "weather");
                MessageBox.Show("Weather data saved to file", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No weather data to save. Please get weather data first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                weatherAPI?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}