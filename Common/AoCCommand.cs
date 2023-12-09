using System.ComponentModel;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Common;

// ReSharper disable once ClassNeverInstantiated.Global
public class AoCCommand : Command<AoCCommand.AoCCommandSettings>
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AoCCommandSettings : CommandSettings
    {
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
    }

    private static int GetDayNumber(string typeName)
    {
        return int.TryParse(typeName.Replace("Day", string.Empty), out var day) ? day : -1;
    }

    private static void RunSolution((TypeInfo Type, int Day) dayType, bool sample, int year) {
        AnsiConsole.MarkupLine(
            ":christmas_tree::christmas_tree::christmas_tree:Day [underline red]{0}[/] of AoC {1}:christmas_tree::christmas_tree::christmas_tree:",
            dayType.Day, year);
        var url = $"https://adventofcode.com/{year}/day/{dayType.Day}";
        AnsiConsole.MarkupLine("Problem description: [underline navy]{0}[/]", url);
        var method = dayType.Type.GetMethod("Solve", BindingFlags.Public | BindingFlags.Static)
                     ?? throw new Exception($"Solve method is not found in {dayType.Type.Name}");
        var dayDirectory = Path.Combine($"Day{dayType.Day:00}");
        var inputFile = Path.Combine(dayDirectory, sample ? "sample.txt" : "input.txt");
        AnsiConsole.MarkupLine("Using input from [underline navy]{0}[/]", inputFile);
        var input = File.ReadLines(inputFile);
        var action = (Action<IEnumerable<string>>)Delegate.CreateDelegate(typeof(Action<IEnumerable<string>>), method);
        AoCContext.IsSample = sample;
        action(input);
    }

    public override int Execute(CommandContext context, AoCCommandSettings settings) {
        var entryAssembly = Assembly.GetEntryAssembly();
        var year = int.Parse(
            entryAssembly!.GetName().Name!.Replace("AoC", string.Empty, StringComparison.OrdinalIgnoreCase));
        var puzzlesToRun = FindSolutionToRun(settings, entryAssembly);

        if (puzzlesToRun.Length == 0 && settings.Day.HasValue) {
            return TryScaffoldSolutionFiles(year, settings.Day.Value);
        }

        foreach (var tuple in puzzlesToRun) {
            Measure.Time(() => RunSolution(tuple, settings.Sample, year))
                .Dump("Finished in ", t => t.ToHumanTimeString());
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

    private static int TryScaffoldSolutionFiles(int year, int day) {
        AnsiConsole.MarkupLine("[red]No solutions found[/]");
        var scaffold = AnsiConsole.Prompt(
            new ConfirmationPrompt($"Would you like to create a new solution for day {day}?") {
                DefaultValue = true,
            });
        if (!scaffold) return 1;
        Scaffolder.Scaffold(year, day);
        return 0;
    }
}