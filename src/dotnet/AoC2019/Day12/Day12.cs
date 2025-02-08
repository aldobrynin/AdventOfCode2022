using System.Text.RegularExpressions;
using Common.AoC;

namespace AoC2019.Day12;

public static partial class Day12 {
    public static void Solve(IEnumerable<string> input) {
        var positions = input.Select(x => Regex.Matches(x, @"-?\d+")
                .Select(m => int.Parse(m.Value))
                .ToArray())
            .Select<int[], Moon>(x => new Moon(Position: new V3<int>(x), Velocity: V3<int>.Zero))
            .ToArray();

        var steps = AoCContext.IsSample ? 100 : 1000;
        Simulate3d(positions)
            .ElementAt(steps)
            .Sum(moon => moon.Position.MLen * moon.Velocity.MLen)
            .Part1();

        new Func<V3<int>, int>[] { v => v.X, v => v.Y, v => v.Z }
            .Select(coordinate => positions.Select(x => (coordinate(x.Position), coordinate(x.Velocity))).ToArray())
            .Select(initial =>
                (long)Simulate1d(initial).WithIndex().Skip(1).First(x => x.Element.SequenceEqual(initial)).Index)
            .Lcm()
            .Part2();
    }

    private static IEnumerable<Moon[]> Simulate3d(Moon[] positions) {
        while (true) {
            yield return positions;
            positions = positions.Select(ApplyVelocity).ToArray();
        }

        Moon ApplyVelocity(Moon state) {
            var velocity = positions.Aggregate(state.Velocity,
                (acc, cur) => acc + (cur.Position - state.Position).Signum());
            return new(state.Position + velocity, velocity);
        }
    }

    private static IEnumerable<(int Position, int Velocity)[]> Simulate1d((int Position, int Velocity)[] positions) {
        while (true) {
            yield return positions;
            positions = positions.Select(ApplyVelocity).ToArray();
        }

        (int Position, int Velocity) ApplyVelocity((int Position, int Velocity) state) {
            var newVelocity = positions.Aggregate(state.Velocity,
                (acc, cur) => acc + int.Sign(cur.Position - state.Position));
            return (state.Position + newVelocity, newVelocity);
        }
    }

    private record Moon(V3<int> Position, V3<int> Velocity) {
        public override string ToString() => $"pos={Position}, vel={Velocity}";
    }
}