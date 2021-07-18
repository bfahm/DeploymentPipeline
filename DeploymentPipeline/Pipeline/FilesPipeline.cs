using DeploymentPipelineTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    class FilesPipeline : IPipeline
    {
        // ☑ Backup files
        // ☑ Fetch new files from remote git repo
        // ⬜ Replace files

        private readonly string LivePath;
        private readonly string BackupPath;
        private readonly string LiveGitBranch;
        private readonly GitCredentials GitCredentials;

        public FilesPipeline(string livePath, string backupPath, string liveGitBranch, GitCredentials gitCredentials)
        {
            LivePath = livePath;
            BackupPath = backupPath;
            LiveGitBranch = liveGitBranch;
            GitCredentials = gitCredentials;
        }

        private Task Backup()
        {
            return Task.Run(() =>
            {
                BackupTool.Backup(LivePath, BackupPath);
            });
        }

        public async Task Execute()
        {
            try
            {
                Console.WriteLine("PERFORMING STEP 1: BACKUP");
                await Backup();

                Console.WriteLine("");

                Console.WriteLine("PERFORMING STEP 2: PULLING LATEST GIT CHANGES");
                var gitClient = new GitClient(LivePath,
                                              LiveGitBranch,
                                              GitCredentials);
                gitClient.PullLatest();
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
