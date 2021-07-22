using Microsoft.Extensions.DependencyInjection;
using SharpDeploy.Core.Utils;

namespace SharpDeploy.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<InternalConsole>();
            services.AddScoped<Deployer>();
            
            return services;
        }
    }
}
