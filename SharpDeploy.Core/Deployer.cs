using SharpDeploy.Constants;
using SharpDeploy.Core.Utils;
using SharpDeploy.Models;
using SharpDeploy.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpDeploy
{
    public class Deployer
    {

        private List<Application> StoredApplications;

        public Deployer()
        {
            StoredApplications = new List<Application>
            {
                new DotNetApplication
                {
                    Id = "PAYROLL_1234",
                    Name = "Backend-API",
                    LastDeployed = "1 hr ago",
                    Title = "Payroll",
                    SourcePath = "D:\\pipelinetest\\Payroll",
                    EndpointPath = "D:\\pipelinetest\\Payroll\\FlairsPayroll",
                    SecretsPath = "D:\\pipelinetest\\Secrets",
                    BackupPath = "D:\\pipelinetest\\Payroll-Backup",
                    OutputPath = "D:\\pipelinetest\\Payroll-Output",
                    GitLiveBranch = "master",
                    TargetFramework = DotNetTargetFrameworks.ThreePointOne.ToNameValue(),
                    IISSiteName = "PayrollAngular"
                }
            };
        }

        public async Task<string> DeployDotNet(string projectId, string gitUserName, string gitToken)
        {
            var application = (DotNetApplication)StoredApplications.FirstOrDefault(a => a.Id == projectId);

            if (application == null)
                return "Application Not Found";

            var gitCredentials = new GitCredentials(gitUserName, gitToken);

            // Initialize Pipelines
            var applicationConsole = new InternalConsole(application.Id);
            var applicationPipeline = new DotNetApplicationPipeline(application, gitCredentials, applicationConsole);

            await RunPipelines(applicationPipeline);

            return applicationConsole.ToString();
        }

        public List<Application> GetListOfApplications()
        {
            return StoredApplications;
        }

        private static async Task RunPipelines(params IPipeline[] pipelines)
        {
            // Support for running multiple pipelines in parallel

            List<Task> pipelineExecutionTasks = new();

            foreach (var pipeline in pipelines)
            {
                pipelineExecutionTasks.Add(pipeline.Execute());
            }

            await Task.WhenAll(pipelineExecutionTasks);
        }
    }
}
