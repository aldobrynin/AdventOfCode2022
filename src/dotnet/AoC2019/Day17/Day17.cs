namespace AoC2019.Day17;

public static partial class Day17 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();
        var computer = new IntCodeComputer(program);
        var map = Map.From(computer.ReadAllOutputs()
            .ToBlockingEnumerable()
            .Select(o => (char)o)
            .SplitBy(o => o == '\n')
            .Where(x => x.Count > 0));

        var robot = map.Coordinates().Single(v => "^v<>".Contains(map[v]));

        map.Bfs((_, to) => map[to] == '#', robot)
            .SelectMany(x => map.Area4(x.State).Where(v => map[v] == '#'))
            .CountFrequency()
            .Where(x => x.Value == 4)
            .Sum(x => x.Key.X * x.Key.Y)
            .Part1();

        var instructions = BuildPath(robot, map).StringJoin();
        var movementFunctions =
            FindPatterns(instructions) ?? throw new InvalidOperationException("Cannot find patterns");

        var main = movementFunctions
            .SelectMany((pattern, index) => instructions.FindOccurrences(pattern)
                .Select(ind => (PatternIndex: index, StartIndex: ind)))
            .OrderBy(x => x.StartIndex)
            .Select(x => (char)(x.PatternIndex + 'A'))
            .StringJoin();

        List<string> inputs = [main, ..movementFunctions, "n"];
        var programInput = string.Concat(inputs.Select(s => s + "\n")).Select(x => (long)x).ToArray();

        program[0] = 2;
        new IntCodeComputer(program, programInput).ReadAllOutputs().ToBlockingEnumerable().Last().Part2();
    }

    private static List<string>? FindPatterns(string s, int maxPatterns = 3) {
        if (maxPatterns == 0) return s.Length == 0 ? [] : null;

        const int maxLength = 20;
        if (s.Length <= maxLength) return [s];

        for (var length = maxLength; length >= 2; length--) {
            if (length < s.Length && char.IsLetter(s[length])) continue;
            var pattern = s[..length].TrimEnd(',');
            var matches = s.FindOccurrences(pattern);
            if (matches.Count <= 1) continue;

            var nextS = s.Replace(pattern + ",", string.Empty).Replace(pattern, string.Empty);
            var nextPatterns = FindPatterns(nextS, maxPatterns - 1);
            if (nextPatterns != null) return nextPatterns.Prepend(pattern).ToList();
        }

        return null;
    }

    private static List<int> FindOccurrences(this string s, string pattern) {
        var count = new List<int>();
        var index = 0;
        while ((index = s.IndexOf(pattern, index, StringComparison.Ordinal)) != -1) {
            count.Add(index);
            index += pattern.Length;
        }

        return count;
    }

    private static IEnumerable<string?> BuildPath(V start, Map<char> map) {
        var pos = start;
        var dir = V.FromArrow(map[start]);
        var steps = 0;
        while (true) {
            var left = dir.Rotate(-90);
            var right = dir.Rotate(90);
            V[] dirs = [dir, left, right];
            var nextDir = dirs.FirstOrDefault(x => map.GetValueOrDefault(pos + x, '.') != '.');
            if (nextDir == dir) steps++;
            else {
                if (steps > 0) yield return steps.ToString();
                steps = 1;

                if (nextDir is null) yield break;
                if (nextDir == left) yield return "L";
                else yield return "R";
            }

            dir = nextDir;
            pos += dir;
        }
    }
}