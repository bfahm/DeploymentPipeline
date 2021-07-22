using Microsoft.Extensions.DependencyInjection;

namespace SharpDeploy.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<Deployer>();
            
            return services;
        }
    }
}
