namespace SharpDeploy.Models
{
    public class GitCredentials
    {
        public GitCredentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
            MergeUserName = userName;
            MergeUserEmail = userName;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string MergeUserName { get; set; }
        public string MergeUserEmail { get; set; }
    }
}
