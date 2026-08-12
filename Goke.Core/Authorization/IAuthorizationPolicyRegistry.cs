
using System.Security.Claims;

namespace Goke.Core.Authorization
{
    public interface IAuthorizationPolicyRegistry
    {
        bool TryGetPolicy(string policyName, out AuthorizationPolicy? policy);
    }

    public interface IAuthorizationPolicyEvaluator
    {
        Task<bool> EvaluateAsync(ClaimsPrincipal user, AuthorizationPolicy policy);
    }


    public sealed class AuthorizationPolicyRegistry : IAuthorizationPolicyRegistry
    {
        private readonly Dictionary<string, AuthorizationPolicy> _policies;

        public AuthorizationPolicyRegistry(IEnumerable<AuthorizationPolicy> policies)
        {
            _policies = policies
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGetPolicy(string policyName, out AuthorizationPolicy? policy)
        {
            var found = _policies.TryGetValue(policyName, out var result);
            policy = result;
            return found;
        }
    }

    public sealed class AuthorizationPolicyEvaluator(
        IEnumerable<IAuthorizationRequirementHandler> handlers)
        : IAuthorizationPolicyEvaluator
    {
        private readonly IAuthorizationRequirementHandler[] _handlers = handlers.ToArray();

        public async Task<bool> EvaluateAsync(ClaimsPrincipal user, AuthorizationPolicy policy)
        {
            if (user.Identity?.IsAuthenticated != true)
            {
                return false;
            }

            foreach (var requirement in policy.Requirements)
            {
                var handler = _handlers.FirstOrDefault(h => h.CanHandle(requirement));
                if (handler is null)
                {
                    return false;
                }

                if (!await handler.EvaluateAsync(user, requirement))
                {
                    return false;
                }
            }

            return true;
        }

       
    }

   
}