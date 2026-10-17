namespace Goke.Core.Map
{
    public interface ILocationService
    {
        Task<Location?> GetCurrentLocationAsync();
    }
}