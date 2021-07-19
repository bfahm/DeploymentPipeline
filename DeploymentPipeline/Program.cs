using DeploymentPipeline.Models;
using DeploymentPipeline.Pipeline;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeploymentPipeline
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppSettings appSettings = new();

            List<IPipeline> pipelines = new List<IPipeline>
            {
                new FilesPipeline(appSettings.PAYROLL_API_SOURCE_PATH,
                                  appSettings.PAYROLL_API_BACKUP_PATH,
                                  appSettings.PublishSettings.OutputPath,
                                  appSettings.PAYROLL_API_SECRETS_PATH,
                                  appSettings.PAYROLL_API_GIT_LIVE_BRANCH,
                                  appSettings.GitCredentials,
                                  appSettings.PublishSettings.TargetFramework,
                                  appSettings.PublishSettings.EndpointPath),
            };

            List<Task> pipelineExecutionTasks = new();

            foreach (var pipeline in pipelines)
            {
                pipelineExecutionTasks.Add(pipeline.Execute());
            }

            await Task.WhenAll(pipelineExecutionTasks);
        }
    }
}
