using Spectre.Console;

namespace Common;

public static class Scaffolder {
    private static readonly string[] FileNames = ["sample.txt"];
    
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

    public static async Task DownloadInput(int year, int day, string destinationFile) {
        var url = $"https://adventofcode.com/{year}/day/{day}/input";
        var client = new HttpClient();

        var session = GetAoCSession();
        client.DefaultRequestHeaders.Add("Cookie", "session=" + session);
        var input = await client.GetStreamAsync(url);
        await using var file = File.OpenWrite(destinationFile);
        await input.CopyToAsync(file);
    }

    private static string GetAoCSession() {
        var sessionFromEnv = Environment.GetEnvironmentVariable("AOC_SESSION");
        if (!string.IsNullOrEmpty(sessionFromEnv)) return sessionFromEnv;
        AnsiConsole.WriteLine("No session cookie was provided in AOC_SESSION environment variable");
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter session cookie:").Secret())
            .Replace("session=", string.Empty);
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