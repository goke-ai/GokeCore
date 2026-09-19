using System.Text.Json;
using Goke.Core.Authentication;
using Goke.Core.Security;
using Microsoft.Extensions.Logging;

namespace Goke.Maui.Core.Services;

public sealed class TokenStorage(ILogger<TokenStorage> logger) : IAuthTokenStore
{
    private const string StorageKeyName = "access_token";

    public void RemoveToken()
    {
        SecureStorage.Remove(StorageKeyName);
    }

    public async Task<AccessTokenInfo?> GetTokenAsync()
    {
        try
        {
            var tokenJson = await SecureStorage.GetAsync(StorageKeyName);

            if (string.IsNullOrWhiteSpace(tokenJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<AccessTokenInfo>(tokenJson);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unable to retrieve AccessTokenInfo from SecureStorage.");
            return null;
        }
    }

    public async Task<AccessTokenInfo?> SaveTokenAsync(LoginResponse? loginResponse, string email)
    {
        try
        {
            var accessToken = CreateTransientToken(loginResponse, email);
            if (accessToken is null)
            {
                return null;
            }

            await SecureStorage.SetAsync(StorageKeyName, JsonSerializer.Serialize(accessToken));
            return accessToken;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unable to save AccessTokenInfo to SecureStorage.");
            return null;
        }
    }

    public AccessTokenInfo? CreateTransientToken(LoginResponse? loginResponse, string email)
    {
        if (loginResponse is null || string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return new AccessTokenInfo
        {
            LoginResponse = loginResponse,
            Email = email,
            AccessTokenExpiration = DateTime.UtcNow.AddSeconds(loginResponse.ExpiresIn)
        };
    }
}