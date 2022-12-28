using Common;

namespace AoC2021;

public class Day10
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input as string[] ?? input.ToArray();
        array.Select(GetCorruptionScore).Sum().Dump("Part1: ");

        var nonCorrupted = array.Where(x => GetCorruptionScore(x) == 0).ToArray();
        nonCorrupted
            .Select(GetMissingChars)
            .Select(x => x.Aggregate(0L, (curr, next) => curr * 5 + AutocompleteScore(next)))
            .Order()
            .ElementAt((nonCorrupted.Length) / 2)
            .Dump("Part2: ");

    }

    private static int GetCorruptionScore(string line)
    {
        var stack = new Stack<char>();
        foreach (var current in line)
        {
            if (IsOpen(current))
                stack.Push(current);
            else if (GetPair(stack.Pop()) != current)
                return Score(current);
        }

        return 0;
    }

    private static IEnumerable<char> GetMissingChars(string line)
    {
        var stack = new Stack<char>();
        foreach (var current in line)
        {
            if (IsOpen(current))
                stack.Push(current);
            else
                stack.Pop();
        }

        return stack.Select(GetPair);
    }

    private static bool IsOpen(char c) => c is '{' or '(' or '<' or '[';

    private static char GetPair(char c) => c switch
    {
        '{' => '}',
        '(' => ')',
        '[' => ']',
        '<' => '>',
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
    
    private static int Score(char c)=> c switch
    {
        ')' =>3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
    
    private static int AutocompleteScore(char c)=> c switch
    {
        ')' =>1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}

