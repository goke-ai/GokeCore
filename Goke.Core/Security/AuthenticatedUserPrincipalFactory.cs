using Goke.Core.Authentication;
using Goke.Core.Authorization;
using System.Security.Claims;

namespace Goke.Core.Security;

public sealed class AuthenticatedUserPrincipalFactory : IAuthenticatedUserPrincipalFactory
{
    public ClaimsPrincipal Create(AuthenticatedUserResponse user, string fallbackEmail, string authenticationType)
    {
        var claims = ClaimBuilder.BuildClaimFomUserInfo(user, fallbackEmail);
        var identity = new ClaimsIdentity(claims, authenticationType);
        return new ClaimsPrincipal(identity);
    }
}
