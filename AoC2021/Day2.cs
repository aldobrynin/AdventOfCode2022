using Common;

namespace AoC2021;

public class Day2
{
    public static void Solve(IEnumerable<string> input)
    {
        var commands = input.Select(line => line.Split(' '))
            .Select(x => (Command: x[0], Arg: int.Parse(x[1])))
            .ToArray();
        Part1(commands);
        Part2(commands);
    }

    private static void Part2(IEnumerable<(string Command, int Arg)> commands)
    {
        int pos = 0, aim = 0, depth = 0;
        foreach (var (command, arg) in commands)
        {
            if (command == "forward")
            {
                pos += arg;
                depth += aim * arg;
            }
            else if (command == "down")
                aim += arg;
            else
                aim -= arg;
        }

        (pos * depth).Dump("Part2: ");
    }

    private static void Part1(IEnumerable<(string Command, int Arg)> input)
    {
        var depth = 0;
        var pos = 0;
        foreach (var (command, arg) in input)
        {
            if (command == "forward")
                pos += arg;
            else if (command == "down")
                depth += arg;
            else
                depth -= arg;
        }

        (depth * pos).Dump("Part1: ");
    }
}

