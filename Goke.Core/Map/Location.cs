using Goke.Core.Engines;

namespace Goke.Core.Map
{
    public class Location
    {
        public string Name { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double?  Altitude { get; set; }
        
        public double? Accuracy { get; set; }

        public double? VerticalAccuracy { get; set; }

        public bool ReducedAccuracy { get; set; }
       
        public double? Speed { get; set; }

        public double? Heading { get; set; }

        public long Timestamp { get; set; } = 0;
        
        public DateTimeOffset CapturedAt = DateTimeOffset.UtcNow;
    }
}