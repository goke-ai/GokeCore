using Goke.Core.Authentication;
using Microsoft.Extensions.Logging;

namespace Goke.Core.Security;

public sealed class AccessTokenManager(
    AuthApiClient authApiClient,
    IAuthTokenStore tokenStore,
    ILogger<AccessTokenManager> logger) : IAccessTokenManager
{
    private const int TokenExpirationBufferMinutes = 30;

    private readonly SemaphoreSlim refreshLock = new(1, 1);
    private AccessTokenInfo? accessToken;
    private bool persistTokenToSecureStorage;

    public string? CurrentEmail => accessToken?.Email;

    public async Task<AccessTokenInfo?> GetValidTokenAsync()
    {
        try
        {
            if (accessToken is null)
            {
                accessToken = await tokenStore.GetTokenAsync();
                persistTokenToSecureStorage = accessToken is not null;
            }

            if (accessToken is null)
            {
                return null;
            }

            var refreshThreshold = DateTime.UtcNow.AddMinutes(TokenExpirationBufferMinutes);
            if (refreshThreshold < accessToken.AccessTokenExpiration)
            {
                return accessToken;
            }

            return await RefreshTokenAsync(accessToken.LoginResponse.RefreshToken, accessToken.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking the access token for validity.");
            return null;
        }
    }

    public async Task<AccessTokenInfo?> SetTokenAsync(LoginResponse? loginResponse, string email, bool persistToSecureStorage)
    {
        this.persistTokenToSecureStorage = persistToSecureStorage;

        accessToken = persistToSecureStorage
            ? await tokenStore.SaveTokenAsync(loginResponse, email)
            : tokenStore.CreateTransientToken(loginResponse, email);

        return accessToken;
    }

    public void Clear()
    {
        accessToken = null;
        persistTokenToSecureStorage = false;
        tokenStore.RemoveToken();
    }

    private async Task<AccessTokenInfo?> RefreshTokenAsync(string refreshToken, string email)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        await refreshLock.WaitAsync();

        try
        {
            if (accessToken is not null)
            {
                var refreshThreshold = DateTime.UtcNow.AddMinutes(TokenExpirationBufferMinutes);
                if (refreshThreshold < accessToken.AccessTokenExpiration)
                {
                    return accessToken;
                }
            }

            var refreshed = await authApiClient.RefreshTokenAsync(refreshToken);
            if (refreshed is null)
            {
                logger.LogWarning("Failed to refresh access token.");
                Clear();
                return null;
            }

            logger.LogInformation("Access token refreshed successfully.");

            accessToken = persistTokenToSecureStorage
                ? await tokenStore.SaveTokenAsync(refreshed, email)
                : tokenStore.CreateTransientToken(refreshed, email);

            return accessToken;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error refreshing the access token.");
            Clear();
            return null;
        }
        finally
        {
            refreshLock.Release();
        }
    }
}