namespace AoC2022.Day07;

public partial class Day07
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.ToArray();
        var root = new Directory("/", null, new(), new());
        var current = root;
        for (var index = 0; index < lines.Length;)
        {
            var tokens = lines[index++].Split(' ');
            if (tokens[1] == "cd")
            {
                current = tokens[2] switch
                {
                    ".." => current.Parent ?? current,
                    "/" => root,
                    var dirName => current.SubDirectories[dirName],
                };
            }
            else
            {
                while (index < lines.Length && !lines[index].StartsWith("$"))
                {
                    var str = lines[index++].Split(' ');
                    if (str[0] == "dir")
                        current.SubDirectories.TryAdd(str[1], new Directory(str[1], current, new(), new()));
                    else
                        current.Files.Add(new FileInfo(str[1], long.Parse(str[0])));
                }
            }
        }

        Find(root, x => x.Size() < 100_000)
            .Sum(x => x.Size())
            .Part1();
        
        const long TOTAL_SPACE = 70_000_000;
        const long MIN_REQUIRED_SPACE = 30_000_000;
        var freeSpace = TOTAL_SPACE - root.Size();
        var minCleanupSpace = MIN_REQUIRED_SPACE - freeSpace;
        Find(root, x => x.Size() > minCleanupSpace)
            .Select(x => x.Size())
            .Min().Part2();
    }

    private static IEnumerable<Directory> Find(Directory root, Func<Directory, bool> condition)
    {
        if (condition(root))
            yield return root;
        foreach (var directory in root.SubDirectories.SelectMany(dir => Find(dir.Value, condition)))
            yield return directory;
    }
}

public record Directory(string Name, Directory? Parent, Dictionary<string, Directory> SubDirectories, List<FileInfo> Files)
{
    public long Size() => Files.Sum(x => x.Size) + SubDirectories.Sum(x => x.Value.Size());
};
public record FileInfo(string Name, long Size);