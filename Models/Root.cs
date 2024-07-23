using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public string name { get; set; }
        public Coord Coord { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public int Timezone { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }

    public class Coord
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class WeatherForecast
    {
        public int Dt { get; set; }
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public int Visibility { get; set; }
        public double Pop { get; set; }
        public Sys Sys { get; set; }
        public string DtTxt { get; set; }
        public Rain Rain { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double temperature => Math.Round(temp);
        
        public double FeelsLike { get; set; }
        public double temp_min { get; set; }
        public double TempMin => Math.Round(temp_min);
        public double temp_max { get; set; }
        public double TempMax => Math.Round(temp_max);
        public int Pressure { get; set; }
        public int SeaLevel { get; set; }
        public int GrndLevel { get; set; }
        public int Humidity { get; set; }
        public double TempKf { get; set; }
    }

    public class Rain
    {
        [JsonProperty("3h")]
        public double _3h { get; set; }
    }

    public class Root
    {
        public string Cod { get; set; }
        public int Message { get; set; }
        public int Cnt { get; set; }
        public List<WeatherForecast> List { get; set; }
        public City City { get; set; }
    }

    public class Sys
    {
        public string Pod { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double Gust { get; set; }
    }
}
