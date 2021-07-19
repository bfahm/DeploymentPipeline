using System;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    class FilesPipeline : BasePipeline, IPipeline
    {
        private readonly string LivePath;
        private readonly string BackupPath;
        private readonly string SecretsPath;
        private readonly string LiveGitBranch;
        private readonly GitCredentials GitCredentials;

        public FilesPipeline(string livePath, string backupPath, string secretsPath, string liveGitBranch, GitCredentials gitCredentials)
        {
            LivePath = livePath;
            BackupPath = backupPath;
            LiveGitBranch = liveGitBranch;
            GitCredentials = gitCredentials;
            SecretsPath = secretsPath;
        }

        private Task Backup()
        {
            return Task.Run(() =>
            {
                FileManager.Move(LivePath, BackupPath);
            });
        }

        private Task RestoreSercrets()
        {
            return Task.Run(() =>
            {
                FileManager.Move(SecretsPath, LivePath);
            });
        }

        public async Task Execute()
        {
            try
            {
                await PerformStep(1, "Backup", () => 
                {
                    return Backup();
                });

                await PerformStep(2, "Pulling Latest GIT Changes", () =>
                {
                    var gitClient = new GitClient(LivePath,
                                              LiveGitBranch,
                                              GitCredentials);
                    gitClient.PullLatest();

                    return Task.CompletedTask;
                });

                await PerformStep(3, "Restoring secrets", () =>
                {
                    return RestoreSercrets();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while executing {this}, execution terminated, below are the details.");
                Console.WriteLine(ex.Message);
            }
        }

        public override string ToString()
        {
            return $"Files Pipeline for: {LivePath}";
        }
    }
}
