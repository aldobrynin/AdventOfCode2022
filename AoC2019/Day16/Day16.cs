namespace AoC2019.Day16;

public static partial class Day16 {
    public static void Solve(IEnumerable<string> input) {
        var signal = input.Single().Select(c => c - '0').ToArray();
        Part1(signal);
        Part2(signal);
    }

    private static void Part1(int[] signal) {
        int[] pattern = [0, 1, 0, -1];
        for (var phase = 0; phase < 100; phase++)
            signal = signal
                .Select((_, i) => signal.Select((t, j) => t * pattern[(j + 1) / (i + 1) % pattern.Length]).Sum())
                .Select(x => Math.Abs(x % 10))
                .ToArray();

        signal.Take(8).StringJoin("").Part1();
    }

    static void Part2(int[] signal) {
        var offset = signal[..7].StringJoin("").ToInt();

        signal = signal.Repeat(10000).Skip(offset).ToArray();

        for (var phase = 0; phase < 100; phase++)
        for (var i = signal.Length - 2; i >= 0; i--)
            signal[i] = (signal[i] + signal[i + 1]) % 10;

        signal.Take(8).StringJoin("").Part2();
    }
}