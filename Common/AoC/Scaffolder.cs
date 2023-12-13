namespace Common.AoC;

public static class Scaffolder {
    public static string GetDayDirectory(int year, int day) => Path.Combine(GetProjectDirectory(year), $"Day{day:00}");

    public static async Task Scaffold(int year, int day) {
        var dayDirectory = GetDayDirectory(year, day);
        if (!Directory.Exists(dayDirectory)) Directory.CreateDirectory(dayDirectory);

        var dayString = $"Day{day:00}";
        await File.WriteAllTextAsync(Path.Combine(dayDirectory, $"{dayString}.cs"),
            $$"""
              namespace AoC{{year}}.{{dayString}};

              public static partial class {{dayString}} {
                  public static void Solve(IEnumerable<string> input) {
                      input.Dump("Not implemented: ");
                      
                      // 1.Part1();
                      // 2.Part1();
                  }
              }
              """);
        await File.WriteAllTextAsync(Path.Combine(dayDirectory, $"{dayString}.Sample.cs"),
            $$"""
              namespace AoC{{year}}.{{dayString}};

              public static partial class {{dayString}} {
                  public static IEnumerable<SampleInput> GetSamples() {
                      yield return SampleInput.ForInput("test");
                  }
              }
              """);
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