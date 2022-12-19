using System.ComponentModel;
using System.Reflection;
using Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Solution;

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
    }

    private static int GetDayNumber(string typeName)
    {
        return int.TryParse(typeName.Replace("Day", string.Empty), out var day) ? day : -1;
    }

    private static void RunSolution((TypeInfo Type, int Day) dayType, bool sample)
    {
        var method = dayType.Type.GetMethod("Solve", BindingFlags.Public | BindingFlags.Static)
                     ?? throw new Exception($"Solve method is not found in {dayType.Type.Name}");
        var dayInputDirectory = Path.Combine("inputs", $"day{dayType.Day}");
        var inputFile = Path.Combine(dayInputDirectory, sample ? "sample.txt" : "input.txt");
        AnsiConsole.MarkupLine("Using input from [underline navy]{0}[/]", inputFile);
        var input = File.ReadLines(inputFile);
        var action = (Action<IEnumerable<string>>)Delegate.CreateDelegate(typeof(Action<IEnumerable<string>>), method);
        action(input);
    }

#pragma warning disable CS8765
    public override int Execute(CommandContext context, AoCCommandSettings settings)
#pragma warning restore CS8765
    {
        var dayClass = Assembly.GetExecutingAssembly()
            .DefinedTypes.Where(x => x.Name.StartsWith("Day"))
            .Select(x => (Type: x, Day: GetDayNumber(x.Name)))
            .Where(x => settings.Day == null || x.Day == settings.Day.Value)
            .MaxBy(x => x.Day);
        AnsiConsole.MarkupLine(":christmas_tree::christmas_tree::christmas_tree:Day [underline red]{0}[/] of AoC 2022:christmas_tree::christmas_tree::christmas_tree:", dayClass.Day);
        Measure.Time(() => RunSolution(dayClass, settings.Sample)).Dump("Finished in ");
        return 0;
    }
}