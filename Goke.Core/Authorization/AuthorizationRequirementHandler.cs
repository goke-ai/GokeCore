
using System.Security.Claims;

namespace Goke.Core.Authorization
{
    public interface IAuthorizationRequirementHandler
    {
        bool CanHandle(IAuthorizationRequirement requirement);

        Task<bool> EvaluateAsync(ClaimsPrincipal user, IAuthorizationRequirement requirement);
    }
}

namespace Goke.Core.Authorization
{
    public sealed class RolesRequirementHandler : IAuthorizationRequirementHandler
    {
        public bool CanHandle(IAuthorizationRequirement requirement)
            => requirement is RolesRequirement;

        public Task<bool> EvaluateAsync(ClaimsPrincipal user, IAuthorizationRequirement requirement)
        {
            var rolesRequirement = (RolesRequirement)requirement;

            var result = rolesRequirement.Roles.Count > 0 &&
                         rolesRequirement.Roles.Any(user.IsInRole);

            return Task.FromResult(result);
        }
    }

    public sealed class ClaimRequirementHandler : IAuthorizationRequirementHandler
    {
        public bool CanHandle(IAuthorizationRequirement requirement)
            => requirement is ClaimRequirement;

        public Task<bool> EvaluateAsync(ClaimsPrincipal user, IAuthorizationRequirement requirement)
        {
            var claimRequirement = (ClaimRequirement)requirement;

            var result = claimRequirement.Values.Count == 0
                ? user.HasClaim(c =>
                    string.Equals(c.Type, claimRequirement.Type, StringComparison.OrdinalIgnoreCase))
                : user.HasClaim(c =>
                    string.Equals(c.Type, claimRequirement.Type, StringComparison.OrdinalIgnoreCase) &&
                    claimRequirement.Values.Any(v =>
                        string.Equals(v, c.Value, StringComparison.OrdinalIgnoreCase)));

            return Task.FromResult(result);
        }
    }

    public sealed class PermissionRequirementHandler : IAuthorizationRequirementHandler
    {
        public bool CanHandle(IAuthorizationRequirement requirement)
            => requirement is PermissionRequirement;

        public Task<bool> EvaluateAsync(ClaimsPrincipal user, IAuthorizationRequirement requirement)
        {
            var permissionRequirement = (PermissionRequirement)requirement;

            var result = permissionRequirement.Permissions.Count > 0 &&
                         user.HasClaim(c =>
                             string.Equals(c.Type, "Permission", StringComparison.OrdinalIgnoreCase) &&
                             permissionRequirement.Permissions.Any(p =>
                                 string.Equals(p, c.Value, StringComparison.OrdinalIgnoreCase)));

            return Task.FromResult(result);
        }
    }


}

