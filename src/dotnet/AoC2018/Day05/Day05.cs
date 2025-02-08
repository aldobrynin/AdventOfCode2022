namespace AoC2018.Day05;

public static partial class Day05 {
    public static void Solve(IEnumerable<string> input) {
        var polymer = input.Single();
        ApplyReactions(polymer).Length.Part1();

        polymer
            .Select(char.ToLower)
            .Distinct()
            .Select(x => polymer.Replace(x.ToString(), string.Empty).Replace(char.ToUpper(x).ToString(), string.Empty))
            .Select(ApplyReactions)
            .Min(x => x.Length)
            .Part2();
    }

    private static string ApplyReactions(string polymer) {
        var stack = new Stack<char>();
        foreach (var unit in polymer) {
            if (stack.TryPeek(out var top) && Math.Abs(top - unit) == 32) {
                stack.Pop();
            }
            else {
                stack.Push(unit);
            }
        }

        return new string(stack.Reverse().ToArray());
    }
}
