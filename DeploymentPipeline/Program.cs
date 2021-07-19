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
            DotNetApplication payroll = new();      // Should be initialized
            GitCredentials gitCredentials = new();  // Should be initialized

            // Initialize Pipelines
            var payrollPipeline = new DotNetApplicationPipeline(payroll, gitCredentials);

            await RunPipelines(payrollPipeline);
        }

        static async Task RunPipelines(params IPipeline[] pipelines)
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
