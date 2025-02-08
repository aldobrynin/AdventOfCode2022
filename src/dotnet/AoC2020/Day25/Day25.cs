namespace AoC2020.Day25;

public partial class Day25 {
    public static void Solve(IEnumerable<string> input) {
        var lines = input.Select(long.Parse).ToArray();
        var cardPublicKey = lines[0];
        var doorPublicKey = lines[1];

        var cardLoopSize = GuessLoopSize(cardPublicKey);
        var doorLoopSize = GuessLoopSize(doorPublicKey);

        Transform(doorPublicKey).ElementAt(cardLoopSize).Part1();
        Transform(cardPublicKey).ElementAt(doorLoopSize).Part1();
    }

    private static IEnumerable<long> Transform(long subjectNumber) {
        const int mod = 20201227;
        var value = 1L;
        while (true) {
            yield return value;
            value = value * subjectNumber % mod;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private static int GuessLoopSize(long publicKey) {
        return Transform(7)
            .Select((key, ind) => (PublicKey: key, Iterations: ind))
            .First(x => x.PublicKey == publicKey)
            .Iterations;
    }
}