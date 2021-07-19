using System;
using System.Threading.Tasks;

namespace SharpDeploy.Pipeline
{
    public abstract class BasePipeline
    {
        private int StepCounter = 0;
        public async Task PerformStepAsync(string stepName, Func<Task> action)
        {
            IncrementStepCounter();
            PrintStepDeclaration(StepCounter, stepName);
            await action();
            PrintSplitter();
        }

        public void PerformStep(string stepName, Action action)
        {
            IncrementStepCounter();
            PrintStepDeclaration(StepCounter, stepName);
            action();
            PrintSplitter();
        }

        private void IncrementStepCounter() => StepCounter += 1;

        private void PrintStepDeclaration(int stepNumber, string stepName) => Console.WriteLine($"PERFORMING STEP #{stepNumber}: {stepName}");

        private void PrintSplitter()
        {
            Console.WriteLine("");
            Console.WriteLine("=========================================");
        }
    }
}
