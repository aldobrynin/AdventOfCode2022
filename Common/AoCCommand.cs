using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Common;

// ReSharper disable once ClassNeverInstantiated.Global
public class AoCCommand : AsyncCommand<AoCCommand.AoCCommandSettings> {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AoCCommandSettings : CommandSettings {
        [Description("Day to run. Defaults to last found solution")]
        [CommandOption("-d|--day")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int? Day { get; init; }

        [Description("Whether to run against sample input")]
        [CommandOption("-s|--sample")]
        [DefaultValue(false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool Sample { get; init; }


        [Description("Whether to run all solutions")]
        [CommandOption("-a|--all")]
        [DefaultValue(false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool All { get; init; }
        
        [Description("Whether to run check answers")]
        [CommandOption("-c|--check")]
        [DefaultValue(false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool CheckAnswers { get; init; }
    }

    private static int GetDayNumber(string typeName) {
        return int.TryParse(typeName.Replace("Day", string.Empty), out var day) ? day : -1;
    }

    private static async Task RunSolution((TypeInfo Type, int Day) dayType, bool sample, int year,
        bool allowDownloadAnswers) {
        AnsiConsole.MarkupLine(
            ":christmas_tree::christmas_tree::christmas_tree:Day [underline red]{0}[/] of AoC {1}:christmas_tree::christmas_tree::christmas_tree:",
            dayType.Day, year);
        var url = $"https://adventofcode.com/{year}/day/{dayType.Day}";
        AnsiConsole.MarkupLine("Problem description: [underline navy]{0}[/]", url);
        var method = dayType.Type.GetMethod("Solve", BindingFlags.Public | BindingFlags.Static)
                     ?? throw new Exception($"Solve method is not found in {dayType.Type.Name}");
        var dayDirectory = Path.Combine($"Day{dayType.Day:00}");

        var action = (Action<IEnumerable<string>>)Delegate.CreateDelegate(typeof(Action<IEnumerable<string>>), method);
        AoCContext.Year = year;
        AoCContext.Day = dayType.Day;
        AoCContext.IsSample = sample;
        if (sample) {
            var samples = (IEnumerable<SampleInput>?)
                          dayType.Type
                              .GetMethod("GetSamples", BindingFlags.Public | BindingFlags.Static)?
                              .Invoke(null, Array.Empty<object>())
                          ?? throw new Exception($"GetSamples method is not found in {dayType.Type.Name}");

            foreach (var (sampleInput, index) in samples.WithIndex()) {
                AoCContext.Answers = (sampleInput.PartOneAnswer, sampleInput.PartTwoAnswer);
                AnsiConsole.MarkupLine($"[underline navy]Sample input #{index}:[/]");
                action(sampleInput.MultilineInput());
            }
        }
        else {
            var inputFile = Path.Combine(dayDirectory, "input.txt");
            if (!File.Exists(inputFile)) {
                AnsiConsole.MarkupLine("Downloading input...");
                await AoCClient.DownloadInput(year, dayType.Day, inputFile);
            }

            AnsiConsole.MarkupLine("Using input from [underline navy]{0}[/]", inputFile);
            AoCContext.Answers = await DownloadAnswers(dayType, year, dayDirectory, allowDownloadAnswers);
            var input = File.ReadLines(inputFile);
            Measure.Time(() => action(input))
                .Dump("Finished in ", t => t.ToHumanTimeString());
        }
    }

    private static async Task<(string?, string?)> DownloadAnswers((TypeInfo Type, int Day) dayType, int year,
        string dayDirectory, bool allowDownloadAnswers) {
        if (File.Exists(Path.Combine(dayDirectory, "answer.part1.txt"))) {
            return (ReadFileIfExists(Path.Combine(dayDirectory, "answer.part1.txt")),
                ReadFileIfExists(Path.Combine(dayDirectory, "answer.part2.txt")));
        }

        if (!allowDownloadAnswers) return default;

        if (!AnsiConsole.Confirm("Would you like to download answers?")) {
            return default;
        }

        AnsiConsole.WriteLine("Downloading answers!");
        var page = await AoCClient.DownloadPage(year, dayType.Day);
        // Your puzzle answer was <code>9556896</code>.
        var regex = new Regex("Your puzzle answer was <code>(?<answer>.+)</code>.");
        var answers = regex.Matches(page).Select(x => x.Groups["answer"].Value).ToArray();
        foreach (var index in answers.Indices()) {
            var fileName = $"answer.part{index + 1}.txt";
            await File.WriteAllTextAsync(Path.Combine(dayDirectory, fileName), answers[index]);
        }

        return (answers.FirstOrDefault(), answers.Skip(1).FirstOrDefault());
    }

    public override async Task<int> ExecuteAsync(CommandContext context, AoCCommandSettings settings) {
        var entryAssembly = Assembly.GetEntryAssembly();
        var year = int.Parse(
            entryAssembly!.GetName().Name!.Replace("AoC", string.Empty, StringComparison.OrdinalIgnoreCase));
        var puzzlesToRun = FindSolutionToRun(settings, entryAssembly);

        if (puzzlesToRun.Length == 0 && settings.Day.HasValue) {
            return await TryScaffoldSolutionFiles(year, settings.Day.Value);
        }

        foreach (var tuple in puzzlesToRun) {
            await RunSolution(tuple, settings.Sample, year, settings.CheckAnswers);
        }

        return 0;
    }

    private static (TypeInfo Type, int Day)[] FindSolutionToRun(AoCCommandSettings settings, Assembly entryAssembly) {
        var dayClass = entryAssembly
            .DefinedTypes.Where(x => x.Name.StartsWith("Day"))
            .Select(x => (Type: x, Day: GetDayNumber(x.Name)))
            .Where(x => settings.Day == null || x.Day == settings.Day.Value);
        dayClass = settings.All
            ? dayClass.OrderBy(x => x.Day)
            : dayClass.OrderByDescending(x => x.Day).Take(1);
        return dayClass.ToArray();
    }

    private static async Task<int> TryScaffoldSolutionFiles(int year, int day) {
        AnsiConsole.MarkupLine("[red]No solutions found[/]");
        if (AnsiConsole.Confirm($"Would you like to create a new solution for day {day}?")) {
            await Scaffolder.Scaffold(year, day);
            return 0;
        }

        return 1;
    }

    private static string? ReadFileIfExists(string fileName) {
        return File.Exists(fileName) ? File.ReadAllText(fileName).Trim() : null;
    }
}