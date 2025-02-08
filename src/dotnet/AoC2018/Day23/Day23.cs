namespace AoC2018.Day23;

public static partial class Day23 {
    record Robot(V3<int> Position, int Radius) {
        public static Robot Parse(string s) {
            var parts = s.Split(' ');
            var position = V3<int>.Parse(parts[0][5..^2]);
            var radius = parts[1][2..].ToInt();
            return new Robot(position, radius);
        }

        public bool Contains(V3<int> point) => Position.DistTo(point) <= Radius;
        public bool Overlaps(Robot other) => Position.DistTo(other.Position) <= Radius + other.Radius;
    }

    public static void Solve(IEnumerable<string> input) {
        var robots = input.Select(Robot.Parse).ToArray();

        var strongest = robots.MaxBy(x => x.Radius)!;
        robots
            .Count(x => strongest.Contains(x.Position))
            .Part1();

        var graph = new Dictionary<Robot, HashSet<Robot>>();
        foreach (var pair in robots.Combinations(2).Where(pair => pair[0].Overlaps(pair[1]))) {
            graph.GetOrAdd(pair[0], _ => []).Add(pair[1]);
            graph.GetOrAdd(pair[1], _ => []).Add(pair[0]);
        }

        graph.FindLargestClique()
            .Max(robot => robot.Position.DistTo(V3<int>.Zero) - robot.Radius)
            .Part2();
    }
}
