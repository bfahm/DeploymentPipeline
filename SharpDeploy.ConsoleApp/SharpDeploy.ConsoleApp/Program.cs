using Microsoft.Extensions.DependencyInjection;
using SharpDeploy.Core;
using System;
using System.Threading.Tasks;

namespace SharpDeploy.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            await Run(serviceProvider);

        }

        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<EntryPoint>();
            services.AddCore();
                
            return services.BuildServiceProvider();
        }

        static async Task Run(IServiceProvider serviceProvider)
        {
            var entryPoint = serviceProvider.GetService<EntryPoint>();
            var result = await entryPoint.Deploy();
            Console.WriteLine(result);
        }
    }
}
