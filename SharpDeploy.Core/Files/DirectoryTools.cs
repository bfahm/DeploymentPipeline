using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpDeploy.Core.Files
{
    // TODO: add unit tests
    public static class DirectoryTools
    {
        public static List<string> WalkDirectoryTree(DirectoryInfo root)
        {
            List<string> filesPaths = null;

            try
            {
                filesPaths = root.GetFiles("*.*").Select(f => f.FullName).ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                // TODO: Try to elevate your privileges and access the file again.
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (filesPaths != null)
            {
                DirectoryInfo[] subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    filesPaths.AddRange(WalkDirectoryTree(dirInfo));
                }
            }

            return filesPaths;
        }

        public static string PathCombine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }

        public static string ExtractAbsolutePath(string parentPath, string sourcePath) => sourcePath.Split(parentPath)[1];

        public static string GetDirectoryFromFile(string filePath)
        {
            var pathElements = filePath.Split("\\").ToList();
            pathElements.RemoveAt(pathElements.Count() - 1);

            return Path.Combine(pathElements.ToArray());
        }
    }
}
