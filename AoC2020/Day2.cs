using Common;

namespace AoC2020;

public class Day2
{
    private record Entry(int First, int Second, char Symbol, string Password);

    public static void Solve(IEnumerable<string> input)
    {
        var arr = input.ToArray();
        FindValidPasswords(arr, Part1Rule).Count().Dump("Part1: ");
        FindValidPasswords(arr, Part2Rule).Count().Dump("Part2: ");
    }

    private static IEnumerable<string> FindValidPasswords(IEnumerable<string> input, Func<Entry, bool> isValid)
    {
        return input.Select(Parse).Where(isValid).Select(x => x.Password);
    }

    private static bool Part1Rule(Entry entry)
    {
        var cnt = entry.Password.Count(c => c == entry.Symbol);
        return entry.First <= cnt && cnt <= entry.Second;
    }

    private static bool Part2Rule(Entry entry)
    {
        return entry.Password
            .Where((_, i) => i + 1 == entry.First || i + 1 == entry.Second)
            .Count(c => c == entry.Symbol) == 1;
    }

    private static Entry Parse(string line)
    {
        var tokens = line.Split(new[] { ':', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
        var min = int.Parse(tokens[0]);
        var max = int.Parse(tokens[1]);
        var symbol = tokens[2].Single();
        var password = tokens[3];
        return new(min, max, symbol, password);
    }
}