namespace AoC2024.Day21;

public static partial class Day21 {
    public static void Solve(IEnumerable<string> input) {
        var codes = input.ToArray();

        var numPad = Map.From([
            "789",
            "456",
            "123",
            " 0A",
        ]);

        var dirPad = Map.From([
            " ^A",
            "<v>",
        ]);
        var numPadPaths = BuildPathsMap(numPad);
        var directionalPadPaths = BuildPathsMap(dirPad);
        var memo = new Dictionary<(string Code, int DirectionalPadNumber), long>();

        codes
            .Sum(code => Complexity(code, FindMinLength(code, directionalPadsCount: 2)))
            .Part1();

        codes
            .Sum(code => Complexity(code, FindMinLength(code, directionalPadsCount: 25)))
            .Part2();

        long FindMinLength(string code, int directionalPadsCount) =>
            FindAllSequences(numPadPaths, code).Min(x => GetFinalLength(x, directionalPadsCount));

        long GetFinalLength(string code, int directionalPadsCount) {
            if (directionalPadsCount == 0) return code.Length;
            return memo.GetOrAdd(
                (code, directionalPadsCount),
                key => {
                    var (codePart, padNumber) = key;
                    var parts = codePart.Split('A').SkipLast(1).Select(x => x + "A").ToArray();
                    return parts.Length == 1
                        ? FindAllSequences(directionalPadPaths, parts.First())
                            .Min(x => GetFinalLength(x, padNumber - 1))
                        : parts.Select(p => GetFinalLength(p, padNumber)).Sum();
                }
            );
        }

        string[] FindAllSequences(Dictionary<(char, char), string[]> paths, string code) =>
            code
                .Prepend('A')
                .ZipWithNext((prev, next) => paths[(prev, next)])
                .Reduce((prev, next) => prev.SelectMany(p => next.Select(n => p + n)).ToArray());
    }

    private static Dictionary<(char From, char To), string[]> BuildPathsMap(Map<char> padMap) {
        return padMap
            .FindAll(x => x != ' ')
            .ToArray()
            .Variants(2)
            .ToDictionary(
                x => (From: padMap[x[0]], To: padMap[x[1]]),
                x => FindPossibleSequences(padMap, x[0], x[1]).Select(PathToInstruction).ToArray()
            );
    }

    private static IEnumerable<V[]> FindPossibleSequences(Map<char> map, V start, V end) {
        if (start == end) {
            yield return [end];
            yield break;
        }

        var movedToY = start with { Y = end.Y };
        var movedToX = start with { X = end.X };

        if (movedToX != movedToY && map[movedToY] != ' ') {
            yield return start.LineTo(movedToY).SkipLast(1).Concat(movedToY.LineTo(end)).ToArray();
        }

        if (movedToX != movedToY && map[movedToX] != ' ') {
            yield return start.LineTo(movedToX).SkipLast(1).Concat(movedToX.LineTo(end)).ToArray();
        }
    }

    private static string PathToInstruction(IReadOnlyCollection<V> path) =>
        path
            .ZipWithNext((from, to) => (to - from).ToArrow())
            .Append('A')
            .StringJoin(string.Empty);


    private static long Complexity(string code, long inputLength) =>
        inputLength * code.Replace("A", string.Empty).ToLong();
}
