namespace AoC2018.Day12;

public static partial class Day12 {
    public record State(long Generation, HashSet<int> Pots) {
        public readonly int Min = Pots.Min();
        public readonly int Max = Pots.Max();

        public bool AreEqual(State other) =>
            Pots.Count == other.Pots.Count
            && other.Max - other.Min == Max - Min
            && Pots.All(x => other.Pots.Contains(x - Min + other.Min));
    }

    public static void Solve(IEnumerable<string> input) {
        var sections = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();

        var initialPots = sections.First().Single().Split(':').Last().Trim()
            .WithIndex()
            .Where(x => x.Element == '#')
            .Select(x => x.Index)
            .ToHashSet();
        var initialState = new State(Generation: 0, initialPots);

        var rules = sections
            .Last()
            .Select(x => x.Split(" => "))
            .ToDictionary(
                x => x.First().Select(c => c == '#' ? 1L : 0).Aggregate(0L, (acc, cur) => cur | acc << 1),
                x => x.Last() == "#"
            );

        Simulate(initialState)
            .ElementAt(20)
            .Pots
            .Sum()
            .Part1();

        const long target = 50_000_000_000;

        Simulate(initialState)
            .ZipWithNext()
            .Where(x => x.Prev.AreEqual(x.Next))
            .Select(x => (State: x.Prev, Rate: x.Next.Min - x.Prev.Min))
            .First()
            .Apply(x => x.State.Pots.Sum() + (target - x.State.Generation) * x.State.Pots.Count * x.Rate)
            .Part2();

        IEnumerable<State> Simulate(State initial) =>
            initial.GenerateSequence(current =>
                new State(
                    Generation: current.Generation + 1,
                    Pots: (current.Min - 2).RangeTo(current.Max + 2)
                    .Where(i => rules.GetValueOrDefault(GetPattern(current, i)))
                    .ToHashSet()
                )
            );

        long GetPattern(State current, int potNumber) =>
            Enumerable.Range(0, 5)
                .Select(x => current.Pots.Contains(x + potNumber - 2) ? 1L : 0L)
                .Aggregate(0L, (acc, cur) => cur | acc << 1);
    }
}
