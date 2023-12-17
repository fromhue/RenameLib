class Program
{
    static void Main()
    {
        Console.Write("Enter the root directory path: ");
        string rootDirectory = Console.ReadLine();

        if (Directory.Exists(rootDirectory))
        {
            string oldProjectName, newProjectName;

            Console.Write("Enter the current project name: ");
            oldProjectName = Console.ReadLine();

            Console.Write("Enter the new project name: ");
            newProjectName = Console.ReadLine();

            Console.WriteLine("\nRenaming files and folders...");

            RenameFilesAndFolders(rootDirectory, oldProjectName, newProjectName);

            Console.WriteLine("\nUpdating file contents...");

            UpdateFileContents(rootDirectory, oldProjectName, newProjectName);

            Console.WriteLine("\nRenaming completed. Press any key to exit.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("The specified directory does not exist. Exiting the program.");
        }
    }

    static void RenameFilesAndFolders(string directory, string oldName, string newName)
    {
        foreach (var fileOrDir in Directory.GetFileSystemEntries(directory).OrderByDescending(Path.GetFileName))
        {
            string newPath = fileOrDir.Replace(oldName, newName);

            //Skip bin and obj directories
            if (fileOrDir.Contains("bin") || fileOrDir.Contains("obj"))
            {
                continue;
            }

            if (!fileOrDir.Equals(newPath, StringComparison.OrdinalIgnoreCase))
            {
                if (Directory.Exists(newPath) || File.Exists(newPath))
                {
                    Console.WriteLine($"Skipped: Destination path '{newPath}' already exists.");
                    continue;
                }
                Console.WriteLine($"Renaming: {fileOrDir} to {newPath}");
                Directory.Move(fileOrDir, newPath);
            }

            if (Directory.Exists(newPath))
            {
                RenameFilesAndFolders(newPath, oldName, newName);
            }
        }
    }

    static void UpdateFileContents(string directory, string oldName, string newName)
    {
        foreach (var filePath in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
        {
            string fileExtension = Path.GetExtension(filePath);
            if (fileExtension.Equals(".cs", StringComparison.OrdinalIgnoreCase) ||
                fileExtension.Equals(".sln", StringComparison.OrdinalIgnoreCase) ||
                fileExtension.Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                string fileContent = File.ReadAllText(filePath);
                string newContent = fileContent.Replace(oldName, newName);

                if (!fileContent.Equals(newContent, StringComparison.Ordinal))
                {
                    Console.WriteLine($"Updating content in: {filePath}");
                    File.WriteAllText(filePath, newContent);
                }
            }
        }
    }
}
