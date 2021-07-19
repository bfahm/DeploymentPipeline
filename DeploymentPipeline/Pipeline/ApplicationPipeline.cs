using DeploymentPipeline.Models;
using System;

namespace DeploymentPipeline.Pipeline
{
    class ApplicationPipeline : BasePipeline
    {
        protected readonly Application _application;
        private readonly GitCredentials _gitCredentials;

        public ApplicationPipeline(Application application, GitCredentials gitCredentials)
        {
            _application = application;
            _gitCredentials = gitCredentials;
        }

        private void Backup()
        {
            var currentTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            var stampedBackupPath = $"{_application.BackupPath}_{currentTime}";
            FileManager.Move(_application.SourcePath, stampedBackupPath);
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
                var gitClient = new GitClient(_application.SourcePath,
                                          _application.GitLiveBranch,
                                          _gitCredentials);
                gitClient.PullLatest();
            });

            PerformStep("Deleting old files", () =>
            {
                DeleteOldFiles();
            });
        }
    }
}
