using System.Collections.Immutable;
using Common;

namespace AoC2020;

public class Day8
{
    public static void Solve(IEnumerable<string> input)
    {
        var commands = input
            .Select(line =>
            {
                var tokens = line.Split(' ');
                return (Command: tokens[0], Arg: int.Parse(tokens[1]));
            })
            .ToImmutableArray();
        Run(commands).Value.Dump("Part1: ");

        commands.Indices()
            .Where(i => commands[i].Command != "acc")
            .Select(i =>
                commands.SetItem(i, commands[i] with { Command = commands[i].Command == "jmp" ? "nop" : "jmp" }))
            .Select(Run)
            .First(x => x.Index == commands.Length)
            .Value
            .Dump("Part2: ");
    }

    private static (int Index, int Value) Run(ImmutableArray<(string Command, int Argument)> commands)
    {
        var value = 0;
        var visited = new HashSet<int>();

        var i = 0;
        while (i < commands.Length && visited.Add(i))
        {
            var (command, arg) = commands[i];
            if (command == "acc") value += arg;
            i += command switch
            {
                "nop" => 1,
                "acc" => 1,
                "jmp" => arg,
                _ => throw new ArgumentOutOfRangeException(command),
            };
        }

        return (i, value);
    }
}