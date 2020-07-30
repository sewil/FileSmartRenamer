using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FileSmartRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: filesmartrenamer <path> <pattern> <replacement>");
                Exit(0);
            }

            string path = args[0];
            string pattern = args[1];
            string replacement = args[2];
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Console.WriteLine("Path does not exist.");
                Exit(0);
            }

            FileAttributes attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                // Get paths
                string[] files = Directory.GetFiles(path);
                HandlePaths(pattern, replacement, files);
            }
            else
            {
                HandlePaths(pattern, replacement, path);
            }
        }

        static void HandlePaths(string pattern, string replacement, params string[] paths)
        {
            foreach (string path in paths)
            {
                try
                {
                    var fileInfo = new FileInfo(path);
                    string newName = Regex.Replace(fileInfo.Name, pattern, replacement);
                    fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, newName));
                }
                catch (Exception e)
                {
                    LogError(path + " | " + e.Message);
                }
            }
        }

        static void LogError(string message)
        {
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.WriteLine(DateTime.Now.ToString() + "\t" + message);
            }
        }

        static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
