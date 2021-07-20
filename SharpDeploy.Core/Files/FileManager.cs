using SharpDeploy.Core.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SharpDeploy.Files
{
    class FileManager
    {
        public static void Move(string source, string destination)
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
            });
        }

        public static void DeleteAllFilesIn(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
                return;

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
