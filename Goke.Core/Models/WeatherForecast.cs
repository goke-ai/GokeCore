namespace Goke.Core.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public string City { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public int FeelsLikeTemperatureC { get; set; }

        public int MinimumTemperatureC { get; set; }
        public int MaximumTemperatureC { get; set; }

        public int HumidityPercent { get; set; }
        public double DewPointC { get; set; }
        public int PressureHpa { get; set; }
        public double VisibilityKm { get; set; }

        public int AirQualityIndex { get; set; }
        public int UVIndex { get; set; }
        public int RainProbabilityPercent { get; set; }

        public double WindSpeedKph { get; set; }
        public double WindGustKph { get; set; }
        public int WindDirectionDegrees { get; set; }
        public string WindDirection { get; set; } = string.Empty;

        public double DaylightHours { get; set; }
        public string ActivityRecommendation { get; set; } = string.Empty;

        public string? Summary { get; set; }
        public string? SummaryIcon { get; set; }
    }
}