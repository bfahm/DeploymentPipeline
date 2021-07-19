using System;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    public abstract class BasePipeline
    {
        public async Task PerformStep(int stepNumber, string stepName, Func<Task> action)
        {
            Console.WriteLine($"PERFORMING STEP #{stepNumber}: {stepName}");
            await action();
            Console.WriteLine("");
            Console.WriteLine("=========================================");
        }
    }
}
