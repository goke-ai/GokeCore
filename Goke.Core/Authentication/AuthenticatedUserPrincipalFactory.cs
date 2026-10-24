using System.Security.Claims;

namespace Goke.Core.Authentication;

public sealed class AuthenticatedUserPrincipalFactory : IAuthenticatedUserPrincipalFactory
{
    public ClaimsPrincipal Create(AuthenticatedUserResponse user, string fallbackEmail, string authenticationType)
    {
        var claims = ClaimBuilder.BuildClaimFomUserInfo(user, fallbackEmail);
        var identity = new ClaimsIdentity(claims, authenticationType);
        return new ClaimsPrincipal(identity);
    }
}
