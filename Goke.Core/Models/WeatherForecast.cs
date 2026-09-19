namespace Goke.Core.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public string City { get; set; } = string.Empty;
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public string? SummaryIcon { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}