using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Goke.Core.Authentication;
using Goke.Core.Services;
using Microsoft.Extensions.Logging;

namespace Goke.Core.Security;

public sealed class AuthApiClient(HttpClient httpClient, BackendApiEndpoints backend, ILogger<AuthApiClient> logger)
{
    public async Task<LoginResponse?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        return await PostAsync<LoginRequest, LoginResponse>(
            backend.LoginUri,
            request,
            "login",
            email,
            cancellationToken);
    }

    public async Task<string?> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(
                backend.RegisterUri,
                new RegisterRequest
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = password
                },
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            logger.LogWarning(
                "Remote registration failed for {Email} with status code {StatusCode}.",
                email,
                response.StatusCode);

            return await ExtractErrorMessageAsync(response, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("Remote registration canceled for {Email}.", email);
            return "Request canceled.";
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Remote registration request failed for {Email}.", email);
            return "Server error.";
        }
    }

    public async Task<AuthenticatedUserResponse?> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, backend.MeUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await SendAsync<AuthenticatedUserResponse>(
            request,
            "fetch current user",
            cancellationToken);
    }

    public async Task<bool> LogoutAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, backend.LogoutUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var result = await SendAsync<object>(request, "logout", cancellationToken, readBody: false);
        return result is not null;
    }

    public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await PostAsync<object, LoginResponse>(
            backend.RefreshUri,
            new { RefreshToken = refreshToken },
            "refresh token",
            email: null,
            cancellationToken);
    }

    private async Task<TResponse?> PostAsync<TRequest, TResponse>(
        Uri uri,
        TRequest payload,
        string operation,
        string? email,
        CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(uri, payload, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Remote {Operation} failed for {Email} with status code {StatusCode}.",
                    operation,
                    email ?? "<n/a>",
                    response.StatusCode);

                return default;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(
                "Remote {Operation} canceled for {Email}.",
                operation,
                email ?? "<n/a>");
            return default;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Remote {Operation} request failed for {Email}.",
                operation,
                email ?? "<n/a>");
            return default;
        }
    }

    private async Task<TResponse?> SendAsync<TResponse>(
        HttpRequestMessage request,
        string operation,
        CancellationToken cancellationToken,
        bool readBody = true)
    {
        try
        {
            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Remote {Operation} failed with status code {StatusCode}.",
                    operation,
                    response.StatusCode);

                return default;
            }

            if (!readBody)
            {
                return (TResponse?)(object?)new object();
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("Remote {Operation} was canceled.", operation);
            return default;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Remote {Operation} request failed.", operation);
            return default;
        }
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(content))
        {
            return "Registration failed. Please try again.";
        }

        try
        {
            using var document = JsonDocument.Parse(content);

            if (document.RootElement.TryGetProperty("detail", out var detailElement) &&
                detailElement.ValueKind == JsonValueKind.String &&
                !string.IsNullOrWhiteSpace(detailElement.GetString()))
            {
                return detailElement.GetString()!;
            }

            if (document.RootElement.TryGetProperty("title", out var titleElement) &&
                titleElement.ValueKind == JsonValueKind.String &&
                !string.IsNullOrWhiteSpace(titleElement.GetString()))
            {
                return titleElement.GetString()!;
            }
        }
        catch (JsonException)
        {
        }

        return "Registration failed. Please try again.";
    }
}