using System;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    public abstract class BasePipeline
    {
        public async Task PerformStepAsync(int stepNumber, string stepName, Func<Task> action)
        {
            PrintStepDeclaration(stepNumber, stepName);
            await action();
            PrintSplitter();
        }

        public void PerformStep(int stepNumber, string stepName, Action action)
        {
            PrintStepDeclaration(stepNumber, stepName);
            action();
            PrintSplitter();
        }

        private void PrintStepDeclaration(int stepNumber, string stepName) => Console.WriteLine($"PERFORMING STEP #{stepNumber}: {stepName}");
        private void PrintSplitter()
        {
            Console.WriteLine("");
            Console.WriteLine("=========================================");
        }
    }
}
