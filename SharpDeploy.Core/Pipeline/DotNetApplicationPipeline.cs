using SharpDeploy.Core.Clients;
using SharpDeploy.Core.Utils;
using SharpDeploy.Files;
using SharpDeploy.Models;
using System;
using System.Threading.Tasks;

namespace SharpDeploy.Pipeline
{
    class DotNetApplicationPipeline : ApplicationPipeline, IPipeline
    {
        private readonly DotNetApplication dotnetApplication;
        private readonly InternalConsole _internalConsole;

        public DotNetApplicationPipeline(DotNetApplication application, GitCredentials gitCredentials, InternalConsole internalConsole) : base(application, gitCredentials, internalConsole)
        {
            dotnetApplication = application;
            _internalConsole = internalConsole;
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
                    _internalConsole.WriteLine($"Execution terminated due to an exception:");
                    _internalConsole.WriteLine(ex.Message);
                }
            });
        }
    }
}
