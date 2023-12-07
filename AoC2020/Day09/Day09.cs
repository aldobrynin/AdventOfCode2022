namespace AoC2020.Day09;

public class Day9
{
    public static void Solve(IEnumerable<string> input) {
        var preambleLength = AoCContext.IsSample ? 5 : 25;
        var numbers = input.Select(long.Parse).ToArray();
        var invalidNumber = Part1(numbers, preambleLength).Dump("Part1: ");
        Part2(numbers, invalidNumber).Dump("Part2: ");
    }

    private static long Part1(long[] numbers, int preambleLength) {
        var window = new Queue<long>(numbers.Take(preambleLength));
        foreach (var current in numbers.Skip(preambleLength)) {
            if (!IsSumOfTwo(current, window)) return current;
            window.Dequeue();
            window.Enqueue(current);
        }

        throw new Exception("no solution");
    }

    private static long Part2(long[] numbers, long target) {
        var sum = 0L;
        var window = new Queue<long>();
        foreach (var current in numbers) {
            sum += current;
            window.Enqueue(current);
            
            while (sum > target && window.TryDequeue(out var removed)) 
                sum -= removed;
            if (sum == target && window.Count > 1)
                break;
        }

        return window.Max() + window.Min();
    }

    private static bool IsSumOfTwo(long value, IReadOnlyCollection<long> arr) {
        var map = new HashSet<long>();
        foreach (var i in arr) {
            if (map.Contains(value - i)) return true;
            map.Add(i);
        }

        return false;
    }
}