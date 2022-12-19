using Common;

namespace Solution;

public class Day9
{
    public static void Solve(IEnumerable<string> input)
    {
        var motions = input.Select(Motion.Parse).ToArray();
        EnumerateTailPositions(motions, 2).Distinct().Count().Dump("Part1: ");
        EnumerateTailPositions(motions, 10).Distinct().Count().Dump("Part2: ");
    }

    private static IEnumerable<V> EnumerateTailPositions(IEnumerable<Motion> motions, int knotsCount)
    {
        var knots = Enumerable.Repeat(V.Zero, knotsCount).ToArray();
        foreach (var move in motions.SelectMany(m => Enumerable.Repeat(m.Direction, m.Steps)))
        {
            knots[0] += move;
            for (var i = 0; i < knots.Length - 1; i++)
            {
                var diff = knots[i] - knots[i + 1];
                if (diff.CLen > 1) knots[i + 1] += diff.Signum();
            }

            yield return knots.Last();
        }
    }
}

public record Motion(V Direction, int Steps)
{
    public static Motion Parse(string input)
    {
        var segments = input.Split(' ');
        return new Motion(
            segments[0] switch
            {
                "R" => V.Right,
                "L" => V.Left,
                "U" => V.Up,
                "D" => V.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(input), segments[0])
            },
            int.Parse(segments[1])
        );
    }
}