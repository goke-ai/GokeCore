using Goke.Core.Authentication;

namespace Goke.Core.Security;

public interface IAccessTokenManager
{
    string? CurrentEmail { get; }
    Task<AccessTokenInfo?> GetValidTokenAsync();
    Task<AccessTokenInfo?> SetTokenAsync(LoginResponse? loginResponse, string email, bool persistToSecureStorage);
    void Clear();
}
