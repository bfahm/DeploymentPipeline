using System.Diagnostics;

namespace SharpDeploy.Core.Clients
{
    public class ShellClient
    {
        public static string Run(string workingDirectory, string command, bool runAsAdmin = false)
        {
            Process process = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal; // todo change to hidden
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.Arguments = command;
            startInfo.Verb = runAsAdmin ? "runas" : startInfo.Verb;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        public static string BuildDotNetPublishCommand(string outputPath, string targetFramework)
            => $"/C dotnet publish -p:PublishProfile=FolderProfile -o {outputPath} -c Release -f {targetFramework} --self-contained false";

        public static string BuildIISStartCommand(string appName) => $"/C appcmd start sites {appName}";
        public static string BuildIISStopCommand(string appName) => $"/C appcmd stop sites {appName}";
    }
}
