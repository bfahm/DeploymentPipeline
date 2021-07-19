using SharpDeploy.Models;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;

namespace SharpDeploy
{
    public class GitClient
    {
        private readonly string _repositoryPath;
        private readonly string _deploymentBranch;
        private readonly GitCredentials _gitCredentials;

        public GitClient(string repositoryPath, string deploymentBranch, GitCredentials gitCredentials)
        {
            _repositoryPath = repositoryPath;
            _deploymentBranch = deploymentBranch;
            _gitCredentials = gitCredentials;
        }

        private void Checkout()
        {
            using (var repo = new Repository(_repositoryPath))
            {
                var branch = repo.Branches[_deploymentBranch];

                if (branch == null)
                    return;

                Branch currentBranch = Commands.Checkout(repo, branch);
                Console.WriteLine($"Checked out {currentBranch.FriendlyName}");
            }
        }

        public void PullLatest()
        {
            Checkout();
            Console.WriteLine("");

            using (var repo = new Repository(_repositoryPath))
            {
                // Credential information to fetch
                PullOptions options = new PullOptions();
                options.FetchOptions = new FetchOptions();
                options.FetchOptions.CredentialsProvider = new CredentialsHandler(
                    (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = _gitCredentials.UserName,
                            Password = _gitCredentials.Password
                        });

                // User information to create a merge commit
                var signature = new Signature(new Identity(_gitCredentials.MergeUserName,
                                                           _gitCredentials.MergeUserEmail),
                                              DateTimeOffset.Now);

                // Pull
                try
                {
                    var mergeResult = Commands.Pull(repo, signature, options);

                    Console.WriteLine($"Pulled latest for current branch");
                    Console.WriteLine($"Merge Status: {mergeResult.Status}");

                    if (mergeResult.Commit != null)
                    {
                        Console.WriteLine($"Latest Commit: {mergeResult.Commit.Message}");
                        Console.WriteLine($"Latest Commit by: {mergeResult.Commit.Author.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured while pulling: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
