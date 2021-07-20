using SharpDeploy.Core.Utils;
using System;
using System.Threading.Tasks;

namespace SharpDeploy.Pipeline
{
    public abstract class BasePipeline
    {
        private readonly InternalConsole _internalConsole;

        private int StepCounter = 0;

        protected BasePipeline(InternalConsole internalConsole)
        {
            _internalConsole = internalConsole;
        }

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

        private void PrintStepDeclaration(int stepNumber, string stepName) => _internalConsole.WriteLine($"PERFORMING STEP #{stepNumber}: {stepName}");

        private void PrintSplitter()
        {
            _internalConsole.WriteLine("");
            _internalConsole.WriteLine("=========================================");
        }
    }
}
