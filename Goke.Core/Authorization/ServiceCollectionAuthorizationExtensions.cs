using Microsoft.Extensions.DependencyInjection;

namespace Goke.Core.Authorization
{
    public static class ServiceCollectionAuthorizationExtensions
    {
        public static IServiceCollection AddAuthorization(
            this IServiceCollection services,
            Action<AuthorizationOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configure);

            var options = new AuthorizationOptions();
            configure(options);

            services.AddSingleton<IAuthorizationPolicyRegistry>(
                _ => new AuthorizationPolicyRegistry(options.Policies));

            services.AddSingleton<IAuthorizationPolicyEvaluator, AuthorizationPolicyEvaluator>();

            services.AddSingleton<IAuthorizationRequirementHandler, RolesRequirementHandler>();
            services.AddSingleton<IAuthorizationRequirementHandler, ClaimRequirementHandler>();
            services.AddSingleton<IAuthorizationRequirementHandler, PermissionRequirementHandler>();

            return services;
        }
    }
}