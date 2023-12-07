using System.Text.RegularExpressions;

namespace Solution.Day05;

public class Day5
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.ToArray();
        var (stacks, endOfStack) = ParseStacks(lines);
        endOfStack.Dump("End of stacks: ");
        var instructions = lines.Skip(endOfStack + 2)
            .Select(Instruction.Parse)
            .ToArray();

        ApplyAndPeek(instructions, stacks, ApplyPart1Instruction).Dump("Part1: ");
        ApplyAndPeek(instructions, stacks, ApplyPart2Instruction).Dump("Part2: ");
    }

    private static string ApplyAndPeek(
        IEnumerable<Instruction> instructions,
        Stack<char>[] initialStacks,
        Action<Instruction, Stack<char>[]> action)
    {
        var stacks = initialStacks.Select(c => new Stack<char>(c.Reverse())).ToArray();
        foreach (var instruction in instructions) action(instruction, stacks);
        return new string(stacks.Select(s => s.Peek()).ToArray());
    }

    private static void ApplyPart1Instruction(Instruction instruction, Stack<char>[] stacks)
    {
        for (var i = 0; i < instruction.Count; i++)
            stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop());
    }

    private static void ApplyPart2Instruction(Instruction instruction, Stack<char>[] stacks)
    {
        var tmp = new char[instruction.Count];
        for (var i = 0; i < instruction.Count; i++)
            tmp[i] = stacks[instruction.From - 1].Pop();

        for (var i = instruction.Count - 1; i >= 0; i--)
            stacks[instruction.To - 1].Push(tmp[i]);
    }

    private static (Stack<char>[] stacks, int EndOfStack) ParseStacks(string[] lines)
    {
        var endOfStack = lines.Indices().First(s => char.IsDigit(lines[s][1]));

        var stacks = Enumerable.Range(0, (lines[endOfStack].Length + 1) / 4)
            .Select(_ => new Stack<char>())
            .ToArray();

        foreach (var line in lines.Take(endOfStack).Reverse())
        {
            foreach (var (index, stackIndex) in line.Indices()
                         .Where(index => index % 4 == 1)
                         .Where(index => char.IsLetter(line[index]))
                         .Select(index => (Index: index, StackIndex: (index-1) /4)))
            {
                stacks[stackIndex].Push(line[index]);
            }
        }

        return (stacks, endOfStack);
    }

    public record Instruction(int From, int To, int Count)
    {
        private static readonly Regex Regex = new(@"^move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)", RegexOptions.Compiled);

        public static Instruction Parse(string input)
        {
            var match = Regex.Match(input);
            return new Instruction(int.Parse(match.Groups["from"].Value),
                int.Parse(match.Groups["to"].Value),
                int.Parse(match.Groups["count"].Value)
            );
        }
    }
}