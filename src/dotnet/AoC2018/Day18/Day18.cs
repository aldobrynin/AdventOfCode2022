namespace AoC2018.Day18;

public static partial class Day18 {
    record State(HashSet<V> Trees, HashSet<V> Lumberyards) {
        public virtual bool Equals(State? other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Trees.SetEquals(other.Trees) && Lumberyards.SetEquals(other.Lumberyards);
        }

        public override int GetHashCode() {
            var hash = new HashCode();
            foreach (var tree in Trees) hash.Add(tree);
            foreach (var lumberyard in Lumberyards) hash.Add(lumberyard);
            return hash.ToHashCode();
        }

        public int ResourceValue => Trees.Count * Lumberyards.Count;
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var initial = new State(
            map.FindAll('|').ToHashSet(),
            map.FindAll('#').ToHashSet()
        );

        const int target = 1_000_000_000;
        var history = new Dictionary<State, int>();
        var elements = initial.GenerateSequence(Next)
            .WithIndex()
            .TakeUntil(x => !history.TryAdd(x.Element, x.Index))
            .ToArray();

        var (state, endOfCycle) = elements.Last();
        var startOfCycleIndex = history[state];
        var cycleLength = endOfCycle - startOfCycleIndex;

        var targetIndex = startOfCycleIndex + (target - startOfCycleIndex) % cycleLength;
        elements[10].Element.ResourceValue.Part1();
        elements[targetIndex].Element.ResourceValue.Part2();
        State Next(State current) {
            var lumberyards = new HashSet<V>();
            var trees = new HashSet<V>();

            var treesFreq = current.Trees.SelectMany(x => map.Area8(x)).CountFrequency();
            var lumberyardsFreq = current.Lumberyards.SelectMany(x => map.Area8(x)).CountFrequency();

            foreach (var v in map.Coordinates()) {
                if (current.Trees.Contains(v)) {
                    if (lumberyardsFreq.GetValueOrDefault(v) >= 3) lumberyards.Add(v);
                    else trees.Add(v);
                }
                else if (current.Lumberyards.Contains(v)) {
                    if (lumberyardsFreq.ContainsKey(v) && treesFreq.ContainsKey(v))
                        lumberyards.Add(v);
                }
                else {
                    if (treesFreq.GetValueOrDefault(v) >= 3) trees.Add(v);
                }
            }

            return new State(trees, lumberyards);
        }
    }
}
