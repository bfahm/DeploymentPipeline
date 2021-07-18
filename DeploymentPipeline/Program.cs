using DeploymentPipeline;
using DeploymentPipeline.Pipeline;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeploymentPipelineTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<IPipeline> pipelines = new List<IPipeline>
            {
                new FilesPipeline(Constants.PAYROLL_API_LIVE_PATH, Constants.PAYROLL_API_BACKUP_PATH),
                new FilesPipeline(Constants.PAYROLL_FE_LIVE_PATH, Constants.PAYROLL_FE_BACKUP_PATH)
            };

            List<Task> pipelineExecutionTasks = new();

            foreach(var pipeline in pipelines)
            {
                pipelineExecutionTasks.Add(pipeline.Execute());
            }

            await Task.WhenAll(pipelineExecutionTasks);
        }
    }
}
