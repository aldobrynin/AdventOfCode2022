namespace AoC2023.Day04;

public partial class Day04 {
    public static void Solve(IEnumerable<string> input) {
        var cards = input.Select(line => line.Split(": ")[1]
                .Split(" | ")
                .Select(x => x.ToIntArray())
                .IntersectAll()
                .Count)
            .ToArray();
        cards
            .Where(x => x > 0)
            .Sum(x => 1 << (x - 1))
            .Part1();

        var counts = Enumerable.Repeat(1, cards.Length).ToArray();
        for (var index = 0; index < cards.Length; index++)
            foreach (var copy in Enumerable.Range(index + 1, cards[index]))
                counts[copy] += counts[index];

        counts
            .Sum()
            .Part2();
    }
}