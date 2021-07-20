namespace SharpDeploy.Models
{
    public abstract class Application
    {
        public string Id { get; set; }
        public string SourcePath { get; set; }
        public string BackupPath { get; set; }
        public string OutputPath { get; set; }
        public string GitLiveBranch { get; set; }
    }

    public class DotNetApplication : Application
    {
        public string EndpointPath { get; set; }
        public string TargetFramework { get; set; }
        public string SecretsPath { get; set; }
    }
}
