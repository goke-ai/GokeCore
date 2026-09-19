using Goke.Core.Authentication;

namespace Goke.Core.Security;

public interface IAuthTokenStore
{
    Task<AccessTokenInfo?> GetTokenAsync();
    Task<AccessTokenInfo?> SaveTokenAsync(LoginResponse? loginResponse, string email);
    AccessTokenInfo? CreateTransientToken(LoginResponse? loginResponse, string email);
    void RemoveToken();
}