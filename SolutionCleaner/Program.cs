namespace SolutionCleaner
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Program
    {
        #region [ Private Fields ]
        private static readonly List<string> FolderList = new List<string> { "bin", "obj", "TestResults" };

        private static readonly List<string> FileExtensionList = new List<string>
                                                                     {
                                                                         "*.vssscc",
                                                                         "*.ncrunchproject",
                                                                         "*.user",
                                                                         "*.suo"
                                                                     };
        #endregion

        #region [ Private Methods - Main ]
        private static void Main()
        {
            WriteInfo("Enter solution folder. Press enter for current folder.");
            string solutionFolderPath = Console.ReadLine();
            if (string.IsNullOrEmpty(solutionFolderPath?.Trim()))
            {
                solutionFolderPath = Directory.GetCurrentDirectory();
            }

            var solutionFolder = new DirectoryInfo(solutionFolderPath);
            CleanSolution(solutionFolder);
            WriteInfo("Operation completed");
            Console.ReadKey();
        }
        #endregion

        #region [ Private Methods - Clean & Delete ]
        private static void CleanSolution(DirectoryInfo rootFolder)
        {
            foreach (var folder in FolderList)
            {
                DeleteFolder(rootFolder, folder);    
            }

            foreach (var file in FileExtensionList)
            {
                DeleteFileByExtention(rootFolder, file);
            }
            
            WriteInfo("Files/Folders deleted successfully....");
        }

        private static void DeleteFolder(DirectoryInfo rootFolder, string folderName)
        {
            try
            {
                var subfolders = rootFolder.GetDirectories();
                foreach (var subFolder in subfolders)
                {
                    DeleteFolder(subFolder, folderName);
                }

                var isSearchFolder = rootFolder.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase);
                if (!isSearchFolder)
                {
                    return;
                }

                WriteInfo($"Deleting {rootFolder.Name}");
                Directory.Delete(rootFolder.FullName, true);
                WriteSuccess($"Deleted {rootFolder.Name}");
            }
            catch
            {
                WriteError($"Could not delete: {folderName}");
            }
        }

        private static void DeleteFileByExtention(DirectoryInfo rootFolder, string fileExtention)
        {
            try
            {
                var toDelete = rootFolder.GetFiles(fileExtention, SearchOption.AllDirectories);
                foreach (var file in toDelete)
                {
                    WriteInfo($"Deleting {file.FullName}");
                    file.Delete();
                    WriteSuccess($"Deleted {file.FullName}");
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
        }
        #endregion

        #region [ Private Methods - Write ]
        private static void WriteInfo(string message)
        {
            Write(message);
        }

        private static void WriteSuccess(string message)
        {
            Write(message, ConsoleColor.Green);
        }

        private static void WriteError(string message)
        {
            Write(message, ConsoleColor.Red);
        }

        private static void Write(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion
    }
}