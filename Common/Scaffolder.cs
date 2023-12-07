namespace Common;

public static class Scaffolder {
    private static readonly string[] FileNames = ["sample.txt", "input.txt"];

    public static void Scaffold(int year, int day) {
        var projectDirectory = GetProjectDirectory(year);
        var filePath = Path.Combine(projectDirectory, $"Day{day:00}.cs");
        File.WriteAllText(filePath, $$"""
                                      namespace AoC{{year}};

                                      public class Day{{day}} {
                                          public static void Solve(IEnumerable<string> input) {
                                              input.Dump("Not implemented");
                                          }
                                      }
                                      """);
        var inputsDir = Path.Combine(projectDirectory, "inputs", $"day{day}");
        if (!Directory.Exists(inputsDir)) Directory.CreateDirectory(inputsDir);
        var filesToCreate = FileNames
            .Select(fileName => Path.Combine(inputsDir, fileName))
            .Where(file => !File.Exists(file));
        foreach (var file in filesToCreate) {
            File.WriteAllText(file, string.Empty);
        }
    }

    private static IEnumerable<string> EnumerateParentDirectories(string directory) {
        var current = directory;
        while (current != null) {
            yield return current;
            current = Directory.GetParent(current)?.FullName;
        }
    }

    private static string GetProjectDirectory(int year) {
        var projectFile = $"AoC{year}.csproj";
        return EnumerateParentDirectories(Directory.GetCurrentDirectory())
                   .FirstOrDefault(d => File.Exists(Path.Combine(d, projectFile)))
               ?? throw new Exception("Could not find project directory");
    }
}