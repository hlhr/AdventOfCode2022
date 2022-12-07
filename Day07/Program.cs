using System.Diagnostics;

var lines = File.ReadAllLines("input.txt").ToArray();
var fileSystem = ExecuteCommands(lines);

var answer1 = fileSystem
    .Where(kv => kv.Value.Size <= 100000)
    .Sum(kv => kv.Value.Size);
Console.WriteLine($"Question 1: {answer1}");

var toFreeUp = 30000000 - (70000000 - fileSystem["/"].Size);
var answer2 =
    fileSystem
        .Where(dir => dir.Value.Size >= toFreeUp)
        .MinBy(dir => dir.Value.Size)
        .Value.Size;
Console.WriteLine($"Question 1: {answer2}");

Dictionary<string,DirectorySystemItem> ExecuteCommands(IReadOnlyList<string> commands)
{
    var currentPath = "";
    var directories = new Dictionary<string,DirectorySystemItem>();

    var i = 0;
    for ( ; i < commands.Count; i++)
    {
        var cmd = commands[i][..4];

        switch(cmd)
        {
            case "$ cd": RunCd(commands[i][5..]); break;

            case "$ ls": RunLs(); break;

            default: throw new UnreachableException();
        }
    }

    return directories;

    void RunCd(string path)
    {
        switch (path)
        {
            case "/": currentPath = "/"; break;

            case "..":
            {
                currentPath = currentPath[0..currentPath.LastIndexOf('/')];
                if (string.IsNullOrEmpty(currentPath))
                {
                    currentPath = "/";
                }
            }
            break;

            default: currentPath += "/" + path; break;
        }
    }

    void RunLs()
    {
        if (!directories.TryGetValue(currentPath, out var directoryItem))
        {
            directoryItem = new DirectorySystemItem
            {
                Name = currentPath
            };
            directories[currentPath] = directoryItem;
        }

        i++;
        for ( ; i < commands.Count && commands[i][0] != '$'; i++)
        {
            var cmd = commands[i];
            var fileItem = ParseFileItem(cmd);
            directoryItem.Items.Add(fileItem);

            if (fileItem is DirectorySystemItem)
            {
                directories[currentPath + "/"  + fileItem.Name] = (DirectorySystemItem)fileItem;
            }
        }
        i--;

        IFileSystemItem ParseFileItem(string cmd)
        {
            if (cmd.StartsWith("dir "))
            {
                return new DirectorySystemItem
                {
                    Name = cmd[4..]
                };
            }

            var parts = cmd.Split(' ');
            var fileSize = int.Parse(parts[0]);
            var fileName = parts[1];
            return new FileSystemItem(fileName, fileSize);
        }
    }
}

internal interface IFileSystemItem
{
    public string Name { get; }
    public int Size { get; }
}

internal record FileSystemItem(string Name, int Size) : IFileSystemItem;

internal class DirectorySystemItem : IFileSystemItem
{
    public required string Name { get; init; }

    public int Size => Items.Sum(x => x.Size);

    public List<IFileSystemItem> Items { get; } = new();
}