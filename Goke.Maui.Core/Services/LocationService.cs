using Goke.Core.Map;

namespace Goke.Maui.Core.Services
{
    public class LocationService : ILocationService
    {

        public async Task<Goke.Core.Map.Location?> GetCurrentLocationAsync()
        {
            var sensorLocation = await GetCurrentLocation(); // existing method
            if (sensorLocation == null)
                return null;

            return new Goke.Core.Map.Location
            {
                Latitude = sensorLocation.Latitude,
                Longitude = sensorLocation.Longitude,
                Altitude = sensorLocation.Altitude ?? 0,
                Accuracy = sensorLocation.Accuracy ?? 0,
                VerticalAccuracy = sensorLocation.VerticalAccuracy ?? 0,
                ReducedAccuracy = sensorLocation.IsFromMockProvider, // Assuming this property exists
                Speed = sensorLocation.Speed ?? 0,
                Heading = sensorLocation.Course ?? 0,

                Timestamp = sensorLocation.Timestamp
            };
        }

        private CancellationTokenSource? _cancelTokenSource;

                    
        protected async Task<Microsoft.Maui.Devices.Sensors.Location?> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request = new(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                return location;
            }
            catch (Exception)
            {
                // Unable to get location
            }
            finally
            {
            }
            return null;
        }

        protected void CancelRequest()
        {
            if (_cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }

    }

}
