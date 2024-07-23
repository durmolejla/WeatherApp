using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using WeatherApp.Services;
using WeatherApp.Models;

namespace WeatherApp
{
    public partial class Sedmodnevna : ContentPage
    {
        public Sedmodnevna()
        {
            InitializeComponent();
            SearchIcon.Clicked += SearchIcon_Clicked;
            TxtCitySearch.Completed += TxtCitySearch_Completed;

            // Load default weather for Sarajevo
            LoadWeatherForCity("Sarajevo");
        }

        private async void SearchIcon_Clicked(object sender, EventArgs e)
        {
            if (TxtCitySearch.IsVisible)
            {
                await SearchForCity();
            }
            else
            {
                TxtCitySearch.IsVisible = true;
                TxtCitySearch.Focus();
            }
        }

        private async void TxtCitySearch_Completed(object sender, EventArgs e)
        {
            await SearchForCity();
        }

        private async Task SearchForCity()
        {
            string cityName = TxtCitySearch.Text;
            if (!string.IsNullOrWhiteSpace(cityName))
            {
                await LoadWeatherForCity(cityName);
                TxtCitySearch.IsVisible = false;
                TxtCitySearch.Text = string.Empty;
            }
        }

        private async Task LoadWeatherForCity(string cityName)
        {
            try
            {
                var sevenDayForecast = await ApiService.GetWeatherByCity(cityName);
                UpdateWeatherData(sevenDayForecast);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to retrieve weather data: " + ex.Message, "OK");
            }
        }

        private void UpdateWeatherData(Root sevenDayForecast)
        {
            LblCity.Text = sevenDayForecast.City.name;

            var dailyForecasts = sevenDayForecast.List
                .Where(f => DateTimeOffset.FromUnixTimeSeconds(f.Dt).DayOfWeek != DayOfWeek.Sunday)
                .Take(7)
                .ToList();

            int dayIndex = 1;
            foreach (var dailyForecast in dailyForecasts)
            {
                double minTemp = double.MaxValue;
                double maxTemp = double.MinValue;

                foreach (var forecast in sevenDayForecast.List)
                {
                    if (DateTimeOffset.FromUnixTimeSeconds(forecast.Dt).Date == DateTimeOffset.FromUnixTimeSeconds(dailyForecast.Dt).Date)
                    {
                        if (forecast.Main.TempMin < minTemp)
                            minTemp = forecast.Main.TempMin;
                        if (forecast.Main.TempMax > maxTemp)
                            maxTemp = forecast.Main.TempMax;
                    }
                }

                var tempLabel = this.FindByName<Label>($"TempLbl{dayIndex}");
                if (tempLabel != null)
                {
                    tempLabel.Text = $"max {maxTemp}° - min {minTemp}°";
                }

                dayIndex++;
            }
        }

        private async void NazadButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
