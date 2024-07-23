using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public static class ApiService
    {
        private static readonly HttpClient httpClient = new();

        public static async Task<Root> GetWeather(double latitude, double longitude)
        {
            var response = await httpClient.GetStringAsync(
                $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&appid=18148711055497b32e0f5ea44f29491c");

            return JsonConvert.DeserializeObject<Root>(response) ?? throw new InvalidOperationException("Failed to deserialize weather data.");
        }

        public static async Task<Root> GetWeatherByCity(string city)
        {
            var response = await httpClient.GetStringAsync(
                $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid=18148711055497b32e0f5ea44f29491c");

            return JsonConvert.DeserializeObject<Root>(response) ?? throw new InvalidOperationException("Failed to deserialize weather data.");
        }
    }
}
