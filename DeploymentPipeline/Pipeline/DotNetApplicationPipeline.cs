using DeploymentPipeline.Models;
using System;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    class DotNetApplicationPipeline : ApplicationPipeline, IPipeline
    {
        private readonly DotNetApplication dotnetApplication;

        public DotNetApplicationPipeline(DotNetApplication application, GitCredentials gitCredentials) : base(application, gitCredentials)
        {
            dotnetApplication = application;
        }

        private void RestoreSercrets() => FileManager.Move(((DotNetApplication)_application).SecretsPath, _application.OutputPath);

        private void PublishSourceFiles()
        {
            var cmd = ShellClient.BuildDotNetPublishCommand(dotnetApplication.OutputPath, dotnetApplication.TargetFramework);
            ShellClient.Run(dotnetApplication.EndpointPath, cmd);
        }

        public Task Execute()
        {
            return Task.Run(() =>
            {
                try
                {
                    // From Base..
                    ExecuteSteps();

                    PerformStep("Restoring secrets", () =>
                    {
                        RestoreSercrets();
                    });

                    PerformStep("Publishing new source files", () =>
                    {
                        PublishSourceFiles();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Execution terminated due to an exception:");
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
