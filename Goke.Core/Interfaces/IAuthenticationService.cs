using Goke.Core.Enums;
using Goke.Core.Models;
using System.Security.Claims;

namespace Goke.Core.Interfaces;

public interface IAuthenticationService
{
    LoginStatus LoginStatus { get; }
    string LoginFailureMessage { get; }
    string? CurrentEmail { get; }

    Task<bool> IsAuthenticatedAsync();
    Task<bool> IsInRoleAsync(string role);
    Task<bool> HasClaimAsync(string claimType, string? claimValue = null);
    Task<bool> HasClaimAsync(Predicate<Claim> predicate);
    Task<bool> AuthorizePolicyAsync(string policyName);
    Task<AccessTokenInfo?> GetAccessTokenInfoAsync();
    Task<AuthenticationResult> AuthenticateAsync(LoginRequest loginRequest);
    Task<AuthenticationResult> RegisterAsync(RegisterRequest registerRequest);
    void Logout();
}
