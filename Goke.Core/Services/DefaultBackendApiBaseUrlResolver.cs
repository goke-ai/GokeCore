
namespace Goke.Core.Services;

public sealed class DefaultBackendApiBaseUrlResolver : IBackendApiBaseUrlResolver
{
    public string Resolve(string baseUrl) => baseUrl;
}
