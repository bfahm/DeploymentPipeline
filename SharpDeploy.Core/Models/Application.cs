using System.IO;

namespace SharpDeploy.Models
{
    public abstract class Application
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string LastDeployed { get; set; }
        public string ImagePath { get; set; }
        public string WorkingDirectory { protected get; set; }

        public string SourceCodeFolder { protected get; set; }
        public string SourceCodePath
        {
            get
            {
                return Path.Combine(WorkingDirectory, SourceCodeFolder);
            }
        }

        public string BackupFolder { protected get; set; }
        public string BackupPath 
        { 
            get 
            {
                return Path.Combine(WorkingDirectory, BackupFolder);
            } 
        }
       
        public string OutputFolder { protected get; set; }
        public string OutputPath
        {
            get
            {
                return Path.Combine(WorkingDirectory, OutputFolder);
            }
        }
        
        public string GitRemotePath { get; set; }
        public string GitLiveBranch { get; set; }
    }

    public class DotNetApplication : Application
    {
        public string EndpointFolder { get; set; }
        public string EndpointPath
        {
            get
            {
                return Path.Combine(SourceCodePath, EndpointFolder);
            }
        }

        public string SecretsFolder { protected get; set; }
        public string SecretsPath
        {
            get
            {
                return Path.Combine(WorkingDirectory, SecretsFolder);
            }
        }

        public string TargetFramework { get; set; }
        public string IISSiteName { get; set; }
    }
}
