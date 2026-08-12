namespace Goke.Core.Authorization
{
    public interface IAuthorizationRequirement
    {
    }
}

namespace Goke.Core.Authorization
{
    public sealed class AuthorizationPolicy
    {
        public string Name { get; init; } = string.Empty;

        public IReadOnlyList<IAuthorizationRequirement> Requirements { get; init; } = [];
    }
}


namespace Goke.Core.Authorization
{
    public sealed class RolesRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> Roles { get; init; } = [];
    }

    public sealed class ClaimRequirement : IAuthorizationRequirement
    {
        public string Type { get; init; } = string.Empty;

        public IReadOnlyList<string> Values { get; init; } = [];
    }

    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> Permissions { get; init; } = [];
    }
}

