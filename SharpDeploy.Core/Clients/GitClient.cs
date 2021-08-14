using SharpDeploy.Models;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using SharpDeploy.Core.Utils;
using System.Linq;

namespace SharpDeploy.Core.Clients
{
    public class GitClient
    {
        private readonly InternalConsole _internalConsole;

        public GitClient(InternalConsole internalConsole)
        {
            _internalConsole = internalConsole;
        }

        private string Init(string workingDirectory) => Repository.Init(workingDirectory);

        private void SetOrigin(Repository repository, string remoteUrl)
        {
            try
            {
                Remote remote = repository.Network.Remotes.Add("origin", remoteUrl);

                repository.Branches.Update(repository.Head,
                    b => b.Remote = remote.Name,
                    b => b.UpstreamBranch = repository.Head.CanonicalName);
            }
            catch (Exception ex)
            {
                _internalConsole.WriteLine($"{ex.Message}");
            }
        }

        private FetchOptions SetFetchOptions(GitCredentials gitCredentials)
        {
            FetchOptions options = new FetchOptions();
            options.CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                 new UsernamePasswordCredentials()
                 {
                     Username = gitCredentials.UserName,
                     Password = gitCredentials.Password
                 });

            return options;
        }

        private void Fetch(Repository repository, GitCredentials gitCredentials)
        {
            string logMessage = "";
            var fetchOptions = SetFetchOptions(gitCredentials);

            var remote = repository.Network.Remotes["origin"];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
            Commands.Fetch(repository, remote.Name, refSpecs, fetchOptions, logMessage);

            _internalConsole.WriteLine($"{logMessage}");
        }

        private void ConfigureRemoteTracking(Repository repository, string deploymentBranch)
        {
            var trackingBranch = repository.Branches[$"refs/remotes/origin/{deploymentBranch}"];

            if (trackingBranch == null)
                throw new Exception("GitClient: Invalid branch name.");

            if (!trackingBranch.IsRemote)
                throw new Exception("GitClient: Provided branch is not a remote branch.");

            Branch localBranch = repository.Head;
            repository.Branches.Update(localBranch, b => b.TrackedBranch = trackingBranch.CanonicalName);

            _internalConsole.WriteLine($"Configured remote tracking, {trackingBranch.CanonicalName}");
        }

        private PullOptions SetPullOptions(GitCredentials gitCredentials)
        {
            // Credential information to fetch
            PullOptions options = new PullOptions();

            options.MergeOptions = new MergeOptions()
            {
                FastForwardStrategy = FastForwardStrategy.Default
            };

            options.FetchOptions = new FetchOptions();
            options.FetchOptions.CredentialsProvider = new CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = gitCredentials.UserName,
                        Password = gitCredentials.Password
                    });

            return options;
        }

        private Signature SetSignature(GitCredentials gitCredentials)
        {
            // User information to create a merge commit
            return new Signature(new Identity(gitCredentials.MergeUserName, 
                                              gitCredentials.MergeUserEmail), 
                                 DateTimeOffset.Now);
        }

        private void PullLatest(Repository repository, GitCredentials gitCredentials)
        {
            var options = SetPullOptions(gitCredentials);
            var signature = SetSignature(gitCredentials);

            try
            {
                var mergeResult = Commands.Pull(repository, signature, options);

                _internalConsole.WriteLine($"Pulled latest for current branch");
                _internalConsole.WriteLine($"Merge Status: {mergeResult.Status}");

                if (mergeResult.Commit != null)
                {
                    _internalConsole.WriteLine($"Latest Commit: {mergeResult.Commit.Message}");
                    _internalConsole.WriteLine($"Latest Commit by: {mergeResult.Commit.Author.Name}");
                }
            }
            catch (Exception ex)
            {
                _internalConsole.WriteLine($"An error occured while pulling: {ex.Message}");
                throw;
            }
        }

        public void DownloadSourceCode(string workingDirectory, string remotePath, string deploymentBranch, GitCredentials gitCredentials)
        {
            _internalConsole.WriteLine("");

            var rootedPath = Init(workingDirectory);

            using (var repo = new Repository(rootedPath))
            {
                SetOrigin(repo, remotePath);
                Fetch(repo, gitCredentials);
                ConfigureRemoteTracking(repo, deploymentBranch);
                PullLatest(repo, gitCredentials);
            }
        }
    }
}
