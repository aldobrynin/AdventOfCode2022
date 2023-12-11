namespace AoC2020.Day15;

public partial class Day15 {
    public static void Solve(IEnumerable<string> input) {
        var nums = input.Single().Split(',').Select(int.Parse).ToArray();
        Emulate(nums).ElementAt(2020 - 1).Part1();
        Emulate(nums).ElementAt(30000000 - 1).Part2();
    }

    private static IEnumerable<int> Emulate(int[] input) {
        var map = new Dictionary<int, int>();
        var lastSpoken = -1;
        for (var ind = 0;; ind++) {
            var current = ind < input.Length
                ? input[ind]
                : ind - map.GetValueOrDefault(lastSpoken, ind);
            if (lastSpoken != -1) map[lastSpoken] = ind;
            lastSpoken = current;
            yield return current;
        }
    }
}