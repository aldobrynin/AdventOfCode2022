namespace AoC2021.Day05;

public class Day5
{
    private record Line(V From, V To)
    {
        public static Line Parse(string line)
        {
            var split = line.Split(new[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            return new Line(new V(split[0], split[1]), new V(split[2], split[3]));
        }
    }
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.Select(Line.Parse).ToArray();
        lines
            .Where(v => v.From.X == v.To.X || v.From.Y == v.To.Y)
            .SelectMany(x => x.From.LineTo(x.To))
            .GroupBy(x => x)
            .Count(x => x.Count() > 1)
            .Dump("Part1: ");
        
        lines
            .SelectMany(x => x.From.LineTo(x.To))
            .GroupBy(x => x)
            .Count(x => x.Count() > 1)
            .Dump("Part2: ");
    }
}