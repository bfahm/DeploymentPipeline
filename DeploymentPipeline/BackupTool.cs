using System;
using System.IO;
using System.Threading.Tasks;

namespace DeploymentPipelineTool
{
    class BackupTool
    {
        public static void Backup(string source, string destination)
        {
            var root = new DirectoryInfo(source);
            var sourceFilesPaths = DirectoryTools.WalkDirectoryTree(root);

            Parallel.ForEach(sourceFilesPaths, sourcePath =>
            {
                var fileAbsolutePath = DirectoryTools.ExtractAbsolutePath(source, sourcePath);
                var destFullPath = DirectoryTools.PathCombine(destination, fileAbsolutePath);

                var newDirectoryAbsolute = DirectoryTools.GetDirectoryFromFile(fileAbsolutePath);
                var newDirectory = Path.Combine(destination, newDirectoryAbsolute);

                Directory.CreateDirectory(newDirectory);
                File.Copy(sourcePath, destFullPath, true);

                Console.WriteLine(destFullPath);
            });
        }
    }
}
