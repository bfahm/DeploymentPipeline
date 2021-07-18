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

                try
                {
                    Directory.CreateDirectory(newDirectory);
                    File.Copy(sourcePath, destFullPath, true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"An error occured while trying to backup your files: {ex.Message}");
                    throw;
                }
            });
        }
    }
}
