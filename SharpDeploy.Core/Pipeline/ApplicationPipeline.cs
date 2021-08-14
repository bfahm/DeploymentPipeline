using SharpDeploy.Core.Clients;
using SharpDeploy.Core.Utils;
using SharpDeploy.Files;
using SharpDeploy.Models;
using System;
using System.IO;

namespace SharpDeploy.Pipeline
{
    class ApplicationPipeline : BasePipeline
    {
        protected readonly Application _application;
        private readonly GitCredentials _gitCredentials;
        private readonly InternalConsole _internalConsole;

        public ApplicationPipeline(Application application, GitCredentials gitCredentials, InternalConsole internalConsole) : base(internalConsole)
        {
            _application = application;
            _gitCredentials = gitCredentials;
            _internalConsole = internalConsole;
        }

        private void Backup()
        {
            var currentTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            var stampedBackupPath = $"{_application.BackupPath}_{currentTime}";

            if (Directory.Exists(_application.OutputPath))
                FileManager.Move(_application.OutputPath, stampedBackupPath);
            else
                _internalConsole.WriteLine("[WRN] No backup operation was made, output folder does not exist.");
        }

        private void DeleteOldFiles() => FileManager.DeleteAllFilesIn(_application.OutputPath);

        protected void ExecuteSteps()
        {
            PerformStep("Backup", () =>
            {
                Backup();
            });

            PerformStep("Pulling Latest GIT Changes", () =>
            {
                var gitClient = new GitClient(_internalConsole);
                gitClient.DownloadSourceCode(_application.SourceCodePath, 
                                          _application.GitRemotePath,
                                          _application.GitLiveBranch,
                                          _gitCredentials);
            });

            PerformStep("Deleting old files", () =>
            {
                DeleteOldFiles();
            });
        }
    }
}
