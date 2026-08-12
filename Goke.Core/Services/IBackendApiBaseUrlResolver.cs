namespace Goke.Core.Services;

public interface IBackendApiBaseUrlResolver
{
    string Resolve(string baseUrl);
}
