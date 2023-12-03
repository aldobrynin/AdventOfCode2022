namespace AoC2023;

public class Day2 {
    public static void Solve(IEnumerable<string> input) {
        var games = input.Select(line => {
                var segments = line.Replace("Game ", string.Empty).Split(": ");
                var id = segments[0].ToInt();
                var rounds = segments[1].Split("; ")
                    .Select(s => s.Split(", ")
                        .Select(r => r.Split(' '))
                        .ToDictionary(x => x[1], x => x[0].ToInt()))
                    .ToArray();
                return (Id: id, Rounds: rounds);
            })
            .ToArray();

        var limits = new (string Color, int Limit)[] {
            ("red", 12),
            ("green", 13),
            ("blue", 14),
        };

        games.Where(g => g.Rounds.All(IsValidRound))
            .Select(x => x.Id)
            .Sum()
            .Dump("Part1: ");

        games.Select(g => g.Rounds.SelectMany(kv => kv)
                .GroupBy(x => x.Key, x => x.Value)
                .Product(x => x.Max())
            )
            .Sum()
            .Dump("Part2: ");

        bool IsValidRound(IReadOnlyDictionary<string, int> round) {
            return limits.All(r => round.GetValueOrDefault(r.Color) <= r.Limit);
        }
    }
}