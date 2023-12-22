namespace AoC2023.Day22;

public static partial class Day22 {
    record Brick(int Index, Range3d Range) {
        public Brick MoveToZ(int z) {
            var diffZ = Range.Z.From - z;
            return this with { Range = Range with { Z = Range.Z - diffZ } };
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var bricks = input.Select((line, ind) => {
            var parts = line.Split('~').Select(V3.Parse).ToArray();
            return new Brick(ind, Range3d.FromMinMax(parts[0], parts[1]));
        }).ToArray();
        bricks = Stabilize(bricks);

        var bottomEdgeLookup = bricks.ToLookup(x => x.Range.Z.From);
        var topEdgeLookup = bricks.ToLookup(x => x.Range.Z.To);

        var brickToBricksBelow = bricks.ToDictionary(brick => brick.Index,
            brick => topEdgeLookup[brick.Range.Z.From]
                .Where(x => brick.Range.HasXYIntersection(x.Range))
                .Select(x => x.Index).ToArray());
        var brickToBricksAbove = bricks.ToDictionary(brick => brick.Index,
            brick => bottomEdgeLookup[brick.Range.Z.To]
                .Where(x => brick.Range.HasXYIntersection(x.Range))
                .Select(x => x.Index).ToArray());

        bricks.Count(CanRemove).Part1();
        bricks.Sum(CountFallen).Part2();

        bool CanRemove(Brick x) => brickToBricksAbove[x.Index].All(o => brickToBricksBelow[o].Length != 1);

        int CountFallen(Brick brickToRemove) {
            var fallenBricks = new HashSet<int> { brickToRemove.Index };
            foreach (var brick in bricks) {
                if (brickToBricksBelow[brick.Index].Any() && fallenBricks.IsSupersetOf(brickToBricksBelow[brick.Index]))
                    fallenBricks.Add(brick.Index);
            }

            return fallenBricks.Count - 1;
        }
    }

    private static Brick[] Stabilize(IEnumerable<Brick> bricks) {
        var stabilized = new List<Brick>();
        foreach (var brick in bricks.OrderBy(x => x.Range.Z.From)) {
            var maxZ = stabilized.Where(x => brick.Range.HasXYIntersection(x.Range))
                .MaxBy(x => x.Range.Z.To)?.Range.Z.To ?? 1;
            stabilized.Add(brick.MoveToZ(maxZ));
        }

        return stabilized.ToArray();
    }
}