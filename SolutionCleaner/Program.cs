// Update the folders that need to be deleted
var folderList = new List<string> { "bin", "obj", "node_modules", "TestResults", "packages", ".idea", ".vs"  };

// Update the file extensions that need to be deleted here
var fileExtensionList = new List<string>
{
    "*.vssscc",
    "*.ncrunchproject",
    "*.user",
    "*.suo"
};


Console.WriteLine("Enter solution folder. Press enter for current folder.");

var solutionFolderPath = Console.ReadLine();
if (string.IsNullOrEmpty(solutionFolderPath?.Trim()))
{
    solutionFolderPath = Directory.GetCurrentDirectory();
}

WriteInfo("Starting operation clean solution...");
var solutionFolder = new DirectoryInfo(solutionFolderPath);
await CleanSolution(solutionFolder);
WriteInfo("Operation completed");
Console.ReadKey();


async Task CleanSolution(DirectoryInfo rootFolder)
{
    await Parallel.ForEachAsync(folderList, async (subFolder, _) =>
    {
        await DeleteFolder(rootFolder, subFolder);
    });

    Parallel.ForEach(fileExtensionList, (file, _) =>
    {
        DeleteFileByExtension(rootFolder, file);
    });

    WriteInfo("Files/Folders deleted successfully....");
}

async Task DeleteFolder(DirectoryInfo rootFolder, string folderName)
{
    try
    {
        var subfolders = rootFolder.GetDirectories();
        await Parallel.ForEachAsync(subfolders, async (subFolder, token) =>
        {
            await DeleteFolder(subFolder, folderName);
        });

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

static void Write(string message, ConsoleColor consoleColor = ConsoleColor.White, bool showTime = true)
{
    Console.ForegroundColor = consoleColor;
    Console.WriteLine(showTime ? $"{DateTime.Now.ToLongTimeString()}: {message}": message);
    Console.ResetColor();
}