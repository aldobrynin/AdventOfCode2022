namespace AoC2023;

public class Day4 {
    public static void Solve(IEnumerable<string> input) {
        var cards = input.Select(line => {
            var parts = line.Split(": ");
            var id = parts[0].Replace("Card ", string.Empty).ToInt();
            var cardsSegments = parts[1].Split(" | ");

            return (id, MatchCount: cardsSegments[0].ToIntArray().Intersect(cardsSegments[1].ToIntArray()).Count());
        }).ToArray();
        cards
            .Where(x => x.MatchCount > 0)
            .Sum(x => 1 << (x.MatchCount - 1))
            .Dump("Part1: ");

        var counts = Enumerable.Repeat(1, cards.Length).ToArray();
        foreach (var (id, matchCount) in cards)
        foreach (var copy in Enumerable.Range(id + 1, matchCount))
            counts[copy] += counts[id];
        counts
            .Sum()
            .Dump("Part2: ");
    }
}