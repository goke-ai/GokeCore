namespace Goke.Core.Authorization
{
    public sealed class AuthorizationPolicyBuilder
    {
        private readonly List<IAuthorizationRequirement> _requirements = [];

        internal AuthorizationPolicy Build(string name)
        {
            return new AuthorizationPolicy
            {
                Name = name,
                Requirements = _requirements.ToArray()
            };
        }

        public AuthorizationPolicyBuilder RequireRole(params string[] roles)
        {
            _requirements.Add(new RolesRequirement
            {
                Roles = roles
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray()
            });

            return this;
        }

        public AuthorizationPolicyBuilder RequireClaim(string claimType, params string[] values)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(claimType);

            _requirements.Add(new ClaimRequirement
            {
                Type = claimType,
                Values = values
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray()
            });

            return this;
        }

        public AuthorizationPolicyBuilder RequirePermission(params string[] permissions)
        {
            _requirements.Add(new PermissionRequirement
            {
                Permissions = permissions
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray()
            });

            return this;
        }
    }
}







