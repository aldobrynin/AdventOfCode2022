namespace AoC2018.Day13;

public static partial class Day13 {
    public record Cart(V Position, V Direction, int Intersections = 0) {
        public Cart Turn() => (Intersections % 3) switch {
            0 => this with { Direction = new V(Direction.Y, -Direction.X), Intersections = Intersections + 1 },
            2 => this with { Direction = new V(-Direction.Y, Direction.X), Intersections = Intersections + 1 },
            _ => this with { Intersections = Intersections + 1 },
        };
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var carts = map
            .FindAll(x => x is 'v' or '^' or '<' or '>')
            .Select(v => new Cart(v, V.FromArrow(map[v])))
            .ToArray();

        foreach (var cart in carts) {
            map[cart.Position] = cart.Direction.ToArrow() is '>' or '<' ? '-' : '|';
        }

        SimulateCollisions(carts, map)
            .First(x => x.Collisions.Length > 0)
            .Collisions
            .FirstOrDefault()
            .Apply(x => $"{x?.X},{x?.Y}")
            .Part1();

        SimulateCollisions(carts, map)

            .First(x => x.RemainingCarts.Length <= 1)
            .RemainingCarts
            .FirstOrDefault()
            .Apply(x => $"{x?.Position.X},{x?.Position.Y}")
            .Part2();
    }

    private static readonly Dictionary<(char, V), V> Turns = new() {
        [('/', V.Up)] = V.Right,
        [('/', V.Down)] = V.Left,
        [('/', V.Left)] = V.Down,
        [('/', V.Right)] = V.Up,
        [('\\', V.Up)] = V.Left,
        [('\\', V.Down)] = V.Right,
        [('\\', V.Left)] = V.Up,
        [('\\', V.Right)] = V.Down,
    };

    private static IEnumerable<(V[] Collisions, Cart[] RemainingCarts)> SimulateCollisions(Cart[] current,
        Map<char> map) =>
        (Collisions: Array.Empty<V>(), RemainingCarts: current)
        .GenerateSequence(prev => {
            var nextCarts = new List<Cart>();
            var collisions = new List<V>();
            var aliveCartsPositions = prev.RemainingCarts.Select(x => x.Position).ToHashSet();
            foreach (var cart in prev.RemainingCarts.OrderBy(x => x.Position.Y).ThenBy(x => x.Position.X)) {
                if (!aliveCartsPositions.Contains(cart.Position)) {
                    continue;
                }

                var next = cart with { Position = cart.Position + cart.Direction };
                next = map[next.Position] switch {
                    '-' or '|' => next,
                    '+' => next.Turn(),
                    _ => next with { Direction = Turns[(map[next.Position], cart.Direction)] },
                };
                aliveCartsPositions.Remove(cart.Position);
                if (aliveCartsPositions.Add(next.Position)) {
                    nextCarts.Add(next);
                }
                else {
                    aliveCartsPositions.Remove(next.Position);
                    collisions.Add(next.Position);
                }
            }

            return (collisions.ToArray(), nextCarts.Where(x => aliveCartsPositions.Contains(x.Position)).ToArray());
        });
}
