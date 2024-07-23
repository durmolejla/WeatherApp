using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp
{
    public partial class Dnevna : ContentPage
    {
        public ObservableCollection<HourlyForecast> HourlyForecasts { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }

        public Dnevna()
        {
            InitializeComponent();
            HourlyForecasts = new ObservableCollection<HourlyForecast>();
            BindingContext = this;

            SearchIcon.Clicked += SearchIcon_Clicked;
            TxtCitySearch.Completed += TxtCitySearch_Completed;

            LoadWeatherForDefaultCity("Sarajevo");
        }

        private async void LoadWeatherForDefaultCity(string cityName)
        {
            var result = await ApiService.GetWeatherByCity(cityName);
            UpdateWeatherData(result);
            UpdateHourlyForecasts(result);
        }

        private async void SearchIcon_Clicked(object sender, EventArgs e)
        {
            if (TxtCitySearch.IsVisible)
            {
                await SearchForCity(TxtCitySearch.Text);
            }
            else
            {
                TxtCitySearch.IsVisible = true;
                TxtCitySearch.Focus();
            }
        }

        private async void TxtCitySearch_Completed(object sender, EventArgs e)
        {
            await SearchForCity(TxtCitySearch.Text);
        }

        private async Task SearchForCity(string cityName)
        {
            if (!string.IsNullOrWhiteSpace(cityName))
            {
                var result = await ApiService.GetWeatherByCity(cityName);
                UpdateWeatherData(result);
                UpdateHourlyForecasts(result);

                TxtCitySearch.IsVisible = false;
                TxtCitySearch.Text = string.Empty;
            }
        }

        private void UpdateWeatherData(Root result)
        {
            LblCity.Text = result.City.name;
            LblTemperature.Text = result.List[0].Main.temperature + "°C";

            // Update the main weather icon
            var currentWeatherDescription = result.List[0].Weather.FirstOrDefault()?.Description ?? string.Empty;
            ImgWeatherIcon.Source = GetWeatherIcon(currentWeatherDescription);

            // Calculate min and max temperatures
            MinTemperature = double.MaxValue;
            MaxTemperature = double.MinValue;
            foreach (var forecast in result.List)
            {
                if (forecast.Main.TempMin < MinTemperature)
                    MinTemperature = forecast.Main.TempMin;
                if (forecast.Main.TempMax > MaxTemperature)
                    MaxTemperature = forecast.Main.TempMax;
            }

            min.Text = "Min: " + MinTemperature + "°C";
            max.Text = "Max: " + MaxTemperature + "°C";
            LblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
        }

        private string GetWeatherIcon(string weatherDescription)
        {
            if (weatherDescription.Contains("rain", StringComparison.OrdinalIgnoreCase))
            {
                return "rain_icon.png";
            }
            if (weatherDescription.Contains("cloud", StringComparison.OrdinalIgnoreCase))
            {
                return "cloud_icon.png";
            }
            if (weatherDescription.Contains("sun", StringComparison.OrdinalIgnoreCase) || weatherDescription.Contains("clear", StringComparison.OrdinalIgnoreCase))
            {
                return "sun_icon.png";
            }
            return "default_icon.png";
        }

        private void UpdateHourlyForecasts(Root result)
        {
            HourlyForecasts.Clear();
            foreach (var forecast in result.List)
            {
                DateTime forecastTime = DateTimeOffset.FromUnixTimeSeconds(forecast.Dt).DateTime;
                var weatherDescription = forecast.Weather.FirstOrDefault()?.Description ?? string.Empty;
                var hourlyForecast = new HourlyForecast
                {
                    Time = forecastTime.ToString("HH:mm"),
                    Temperature = forecast.Main.temperature.ToString() + "°C",
                    WeatherIcon = GetWeatherIcon(weatherDescription)
                };
                HourlyForecasts.Add(hourlyForecast);
            }
        }

        private async void NazadButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    public class HourlyForecast
    {
        public string Time { get; set; }
        public string WeatherIcon { get; set; }
        public string Temperature { get; set; }
    }
}
