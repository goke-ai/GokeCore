using System.Net;

namespace Goke.Core.Authentication;

public interface ICookieProvider
{
    Task<CookieContainer?> CreateAuthCookies(CancellationToken cancellationToken = default);
}
