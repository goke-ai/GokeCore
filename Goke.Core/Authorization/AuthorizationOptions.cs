namespace Goke.Core.Authorization
{
    public sealed class AuthorizationOptions
    {
        private readonly List<AuthorizationPolicy> _policies = [];

        internal IReadOnlyList<AuthorizationPolicy> Policies => _policies;

        public void AddPolicy(string name, Action<AuthorizationPolicyBuilder> configure)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNull(configure);

            var builder = new AuthorizationPolicyBuilder();
            configure(builder);

            _policies.Add(builder.Build(name));
        }

        public void RemovePolicy(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var policy = _policies.FirstOrDefault(p => p.Name == name);
            if (policy != null)
            {
                _policies.Remove(policy);
            }
        }
        public void AddPolicy(AuthorizationPolicy policy)
        {
            ArgumentNullException.ThrowIfNull(policy);
            if (_policies.Any(p => p.Name == policy.Name))
            {
                throw new InvalidOperationException($"A policy with the name '{policy.Name}' already exists.");
            }
            _policies.Add(policy);

        }

        //get policy by name
        public AuthorizationPolicy? GetPolicy(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            return _policies.FirstOrDefault(p => p.Name == name);
        }


        }
}