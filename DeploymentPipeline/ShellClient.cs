using System.Diagnostics;

namespace DeploymentPipeline
{
    public class ShellClient
    {
        public static void Run(string workingDirectory, string command)
        {
            Process process = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal; // todo change to hidden
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        public static string BuildDotNetPublishCommand(string outputPath, string targetFramework)
            => $"/C dotnet publish -p:PublishProfile=FolderProfile -o {outputPath} -c Release -f {targetFramework} --self-contained false";
    }
}
