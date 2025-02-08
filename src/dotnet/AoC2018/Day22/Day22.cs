namespace AoC2018.Day22;

public static partial class Day22 {
    public record State(V Position, Tool Tool);

    public enum Tool {
        None,
        Torch,
        ClimbingGear,
    }

    public static void Solve(IEnumerable<string> input) {
        var lines = input.ToArray();
        var depth = lines[0].Split(' ').Last().ToInt();
        var target = V.Parse(lines[1].Split(' ').Last());

        const long mod = 20183;
        var erosionLevelsMap = new Map<long>((int)target.X * 2, (int)target.Y * 2);
        foreach (var v in erosionLevelsMap.Coordinates()) {
            var geologicIndex = v switch {
                _ when v == target => 0,
                { X: 0 } => v.Y * 48271,
                { Y: 0 } => v.X * 16807,
                _ => erosionLevelsMap[v + V.Left] * erosionLevelsMap[v + V.Up],
            };
            erosionLevelsMap[v] = (geologicIndex + depth) % mod;
        }

        erosionLevelsMap
            .Coordinates()
            .Where(v => v.X <= target.X && v.Y <= target.Y)
            .Sum(RegionType)
            .Part1();

        SearchHelpers.Dijkstra(NextStates, initialStates: new State(V.Zero, Tool.Torch))
            .First(x => x.State.Position == target && x.State.Tool == Tool.Torch)
            .Distance
            .Part2();

        IEnumerable<SearchPathItem<State>> NextStates(SearchPathItem<State> current) {
            var (position, tool) = current.State;
            return erosionLevelsMap.Area4(position).Where(x => IsValidTool(x, tool))
                .Select(x => current.Next(new State(x, tool), distanceCost: 1))
                .Append(current.Next(new State(position, ChangeTool(position, tool)), distanceCost: 7));
        }

        int RegionType(V v) => (int)(erosionLevelsMap[v] % 3);

        bool IsValidTool(V v, Tool tool) => RegionType(v) switch {
            0 => tool is Tool.ClimbingGear or Tool.Torch,
            1 => tool is Tool.ClimbingGear or Tool.None,
            2 => tool is Tool.Torch or Tool.None,
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        };

        Tool ChangeTool(V v, Tool tool) => RegionType(v) switch {
            0 => tool == Tool.ClimbingGear ? Tool.Torch : Tool.ClimbingGear,
            1 => tool == Tool.ClimbingGear ? Tool.None : Tool.ClimbingGear,
            2 => tool == Tool.Torch ? Tool.None : Tool.Torch,
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        };
    }
}
