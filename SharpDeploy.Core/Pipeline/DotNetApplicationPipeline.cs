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
        private const string IIS_WORKING_DIRECTORY = "c:\\windows\\system32\\inetsrv";
        private readonly DotNetApplication dotnetApplication;
        private readonly InternalConsole _internalConsole;

        public DotNetApplicationPipeline(DotNetApplication application, GitCredentials gitCredentials, InternalConsole internalConsole) : base(application, gitCredentials, internalConsole)
        {
            dotnetApplication = application;
            _internalConsole = internalConsole;
        }

        private void RestoreSercrets() => FileManager.Move(((DotNetApplication)_application).SecretsPath, _application.SourceCodePath);

        private void PublishSourceFiles()
        {
            var cmd = ShellClient.BuildDotNetPublishCommand(dotnetApplication.OutputPath, dotnetApplication.TargetFramework);
            var results = ShellClient.Run(dotnetApplication.EndpointPath, cmd);
            _internalConsole.WriteLine(results);
        }

        private void StartIIS()
        {
            var cmd = ShellClient.BuildIISStartCommand(dotnetApplication.IISSiteName);
            var results = ShellClient.Run(IIS_WORKING_DIRECTORY, cmd, true);
            _internalConsole.WriteLine(results);
        }

        private void StopIIS()
        {
            var cmd = ShellClient.BuildIISStopCommand(dotnetApplication.IISSiteName);
            var results = ShellClient.Run(IIS_WORKING_DIRECTORY, cmd, true);
            _internalConsole.WriteLine(results);
        }

        public Task Execute()
        {
            return Task.Run(() =>
            {
                try
                {
                    // From Base..
                    ExecuteSteps();

                    PerformStep("Stopping app in IIS", () =>
                    {
                        StopIIS();
                    });

                    PerformStep("Restoring secrets", () =>
                    {
                        RestoreSercrets();
                    });

                    PerformStep("Publishing new source files", () =>
                    {
                        PublishSourceFiles();
                    });

                    PerformStep("Starting app in IIS", () =>
                    {
                        StartIIS();
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
