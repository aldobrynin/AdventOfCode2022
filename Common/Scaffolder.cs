namespace Common;

public static class Scaffolder {
    private static readonly string[] FileNames = ["sample.txt", "input.txt"];

    public static void Scaffold(int year, int day) {
        var projectDirectory = GetProjectDirectory(year);
        var dayString = $"Day{day:00}";
        var dayDirectory = Path.Combine(projectDirectory, dayString);
        if (!Directory.Exists(dayDirectory)) Directory.CreateDirectory(dayDirectory);

        var filePath = Path.Combine(dayDirectory, $"{dayString}.cs");
        File.WriteAllText(filePath, $$"""
                                      namespace AoC{{year}}.{{dayString}};

                                      public class {{dayString}} {
                                          public static void Solve(IEnumerable<string> input) {
                                              input.Dump("Not implemented");
                                          }
                                      }
                                      """);
        var filesToCreate = FileNames
            .Select(fileName => Path.Combine(dayDirectory, fileName))
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