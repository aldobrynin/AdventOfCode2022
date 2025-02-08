namespace AoC2019.Day06;

public static partial class Day06 {
    public static void Solve(IEnumerable<string> input) {
        var objectsOrbits = input.Select(s => s.Split(')')).ToDictionary(x => x[1], x => x[0]);

        objectsOrbits.Keys.Sum(CountIndirect).Part1();

        if (objectsOrbits.TryGetValue("YOU", out var youOrbit)) {
            var adjMap = objectsOrbits
                .Concat(objectsOrbits.Select(x => new KeyValuePair<string, string>(x.Value, x.Key)))
                .ToLookup(x => x.Key, x => x.Value);
            var santaOrbit = objectsOrbits["SAN"];
            SearchHelpers.Bfs(cur => adjMap[cur], initialStates: youOrbit)
                .FirstOrDefault(x => x.State == santaOrbit)
                ?.Distance.Part2();
        }

        int CountIndirect(string o) {
            var count = 0;
            var current = objectsOrbits.GetValueOrDefault(o);
            while (current != null) {
                current = objectsOrbits.GetValueOrDefault(current);
                count++;
            }

            return count;
        }
    }
}