// Update the folders that need to be deleted
var folderList = new List<string> { "bin", "obj", "TestResults", "packages" };

// Update the file extensions that need to be deleted here
var fileExtensionList = new List<string>
{
    "*.vssscc",
    "*.ncrunchproject",
    "*.user",
    "*.suo"
};


WriteInfo("Enter solution folder. Press enter for current folder.");
var solutionFolderPath = Console.ReadLine();
if (string.IsNullOrEmpty(solutionFolderPath?.Trim()))
{
    solutionFolderPath = Directory.GetCurrentDirectory();
}

WriteInfo("Starting operation clean solution...");
var solutionFolder = new DirectoryInfo(solutionFolderPath);
CleanSolution(solutionFolder);
WriteInfo("Operation completed");
Console.ReadKey();


void CleanSolution(DirectoryInfo rootFolder)
{
    foreach (var folder in folderList)
    {
        DeleteFolder(rootFolder, folder);
    }

    foreach (var file in fileExtensionList)
    {
        DeleteFileByExtension(rootFolder, file);
    }

    WriteInfo("Files/Folders deleted successfully....");
}

void DeleteFolder(DirectoryInfo rootFolder, string folderName)
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

        WriteInfo($"Deleting {rootFolder.FullName}");
        Directory.Delete(rootFolder.FullName, true);
        WriteSuccess($"Deleted {rootFolder.FullName}");
    }
    catch
    {
        WriteError($"Cannot delete: {folderName}");
    }
}

void DeleteFileByExtension(DirectoryInfo rootFolder, string fileExtension)
{
    try
    {
        var toDelete = rootFolder.GetFiles(fileExtension, SearchOption.AllDirectories);
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


static void WriteInfo(string message)
{
    Write(message);
}

static void WriteSuccess(string message)
{
    Write(message, ConsoleColor.Green);
}

static void WriteError(string message)
{
    Write(message, ConsoleColor.Red);
}

static void Write(string message, ConsoleColor consoleColor = ConsoleColor.White)
{
    Console.ForegroundColor = consoleColor;
    Console.WriteLine(message);
    Console.ResetColor();
}