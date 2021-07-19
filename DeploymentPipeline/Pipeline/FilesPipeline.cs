using DeploymentPipeline.Models;
using System;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    class FilesPipeline : BasePipeline, IPipeline
    {
        // TODO: extract to a model
        private readonly string LivePath;
        private readonly string BackupPath;
        private readonly string OutputPath;
        private readonly string SecretsPath;
        private readonly string EndPointPath;
        private readonly string LiveGitBranch;
        private readonly string TargetFramework;
        private readonly GitCredentials GitCredentials;

        public FilesPipeline(string livePath, string backupPath, string outputPath, string secretsPath, string liveGitBranch, GitCredentials gitCredentials, string targetFramework, string endPointPath)
        {
            LivePath = livePath;
            BackupPath = backupPath;
            LiveGitBranch = liveGitBranch;
            GitCredentials = gitCredentials;
            SecretsPath = secretsPath;
            OutputPath = outputPath;
            TargetFramework = targetFramework;
            EndPointPath = endPointPath;
        }

        private void Backup()
        {
            var currentTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            var stampedBackupPath = $"{BackupPath}_{currentTime}";
            FileManager.Move(LivePath, stampedBackupPath);
        }

        private void RestoreSercrets() => FileManager.Move(SecretsPath, OutputPath);

        private void DeleteOldFiles() => FileManager.DeleteAllFilesIn(OutputPath);

        private void PublishSourceFiles()
        {
            var cmd = ShellClient.BuildDotNetPublishCommand(OutputPath, TargetFramework);
            ShellClient.Run(EndPointPath, cmd);
        }

        public Task Execute()
        {
            return Task.Run(() =>
            {
                try
                {
                    PerformStep(1, "Backup", () =>
                    {
                        Backup();
                    });

                    PerformStep(2, "Pulling Latest GIT Changes", () =>
                    {
                        var gitClient = new GitClient(LivePath,
                                                  LiveGitBranch,
                                                  GitCredentials);
                        gitClient.PullLatest();
                    });

                    PerformStep(4, "Deleting old files", () =>
                    {
                        DeleteOldFiles();
                    });

                    PerformStep(5, "Restoring secrets", () =>
                    {
                        RestoreSercrets();
                    });

                    PerformStep(6, "Publishing new source files", () =>
                    {
                        PublishSourceFiles();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured while executing {this}, execution terminated, below are the details.");
                    Console.WriteLine(ex.Message);
                }
            });

        }

        public override string ToString()
        {
            return $"Files Pipeline for: {LivePath}";
        }
    }
}
