using Goke.Core.Authentication;
using System.Security.Claims;

namespace Goke.Core.Security;

public interface IAuthenticatedUserPrincipalFactory
{
    ClaimsPrincipal Create(AuthenticatedUserResponse user, string fallbackEmail, string authenticationType);
}
