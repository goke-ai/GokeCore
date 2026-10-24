using System.Security.Claims;

namespace Goke.Core.Authentication;

public interface IAuthenticatedUserPrincipalFactory
{
    ClaimsPrincipal Create(AuthenticatedUserResponse user, string fallbackEmail, string authenticationType);
}
