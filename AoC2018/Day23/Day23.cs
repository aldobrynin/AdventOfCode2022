namespace AoC2018.Day23;

public static partial class Day23 {
    record Robot(V3<int> Position, int Radius) {
        public static Robot Parse(string s) {
            var parts = s.Split(' ');
            var position = V3<int>.Parse(parts[0][5..^2]);
            var radius = parts[1][2..].ToInt();
            return new Robot(position, radius);
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var robots = input.Select(Robot.Parse).ToArray();

        var strongest = robots.MaxBy(x => x.Radius)!;
        robots
            .Count(x => x.Position.DistTo(strongest.Position) <= strongest.Radius)
            .Part1();
    }
}
