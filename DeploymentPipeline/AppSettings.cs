namespace DeploymentPipeline
{
    public class AppSettings
    {
        public string PAYROLL_API_LIVE_PATH { get; } = "";
        public string PAYROLL_API_BACKUP_PATH { get; } = "";
        public string PAYROLL_API_GIT_LIVE_BRANCH { get; } = "";
        public string PAYROLL_FE_LIVE_PATH { get; } = "";
        public string PAYROLL_FE_BACKUP_PATH { get; } = "";
        public string PAYROLL_FE_GIT_LIVE_BRANCH { get; } = "";
        public GitCredentials GitCredentials { get; } = new();
    }

    public class GitCredentials
    {
        public string USERNAME { get; } = "";
        public string PASSWORD { get; } = "";
        public string MERGE_USERNAME { get; } = "";
        public string MERGE_USER_EMAIL { get; } = "";
    }
}
