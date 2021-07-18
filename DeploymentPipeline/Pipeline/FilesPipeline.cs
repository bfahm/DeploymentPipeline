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
        // ⬜ Fetch new files from remote git repo
        // ⬜ Replace files

        private readonly string LivePath;
        private readonly string BackupPath;

        public FilesPipeline(string livePath, string backupPath)
        {
            LivePath = livePath;
            BackupPath = backupPath;
        }

        private Task Backup()
        {
            return Task.Run(() =>
            {
                BackupTool.Backup(LivePath, BackupPath);
            });
        }

        public Task Execute()
        {
            return Backup();
        }
    }
}
