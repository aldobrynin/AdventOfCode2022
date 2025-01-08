namespace AoC2018.Day11;

public static partial class Day11 {
    public static void Solve(IEnumerable<string> input) {
        var serialNumber = input.Single().ToInt();

        const int gridSize = 300;
        var sums = new int[gridSize + 1, gridSize + 1];

        for (var x = 1; x <= gridSize; x++)
        for (var y = 1; y <= gridSize; y++) {
            var rackId = x + 10;
            var powerLevel = (rackId * y + serialNumber) * rackId / 100 % 10 - 5;
            sums[y, x] = powerLevel + sums[y, x - 1] + sums[y - 1, x] - sums[y - 1, x - 1];
        }

        FindBest(3, 3)
            .Apply(x => $"{x.X},{x.Y}")
            .Part1();

        FindBest(1, 300)
            .Apply(x => $"{x.X},{x.Y},{x.Size}")
            .Part2();

        (int X, int Y, int Size, int Sum) FindBest(int minSize, int maxSize) {
            int bestX = 0, bestY = 0, bestSize = 0, bestSum = int.MinValue;

            for (var size = minSize; size <= maxSize; size++)
            for (var x = 0; x < gridSize - size; x++)
            for (var y = 0; y < gridSize - size; y++) {
                var sum = sums[y + size, x + size] - sums[y + size, x] - sums[y, x + size] + sums[y, x];
                if (sum > bestSum)
                    (bestX, bestY, bestSum, bestSize) = (x, y, sum, size);
            }

            return (bestX + 1, bestY + 1, bestSize, bestSum);
        }
    }
}
