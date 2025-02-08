namespace AoC2018.Day15;

public static partial class Day15 {
    public class Unit(char type, V position, int damage) {
        public char Type { get; } = type;
        public V Position { get; set; } = position;
        public int HealthPoints { get; set; } = 200;
        public int Damage { get; } = damage;
        public bool IsAlive => HealthPoints > 0;
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        var units = map.FindAll(x => x is 'G' or 'E').Select(v => (Type: map[v], Position: v)).ToArray();

        foreach (var unit in units) {
            map[unit.Position] = '.';
        }

        Fight(map, units)
            .Apply(Score)
            .Part1();

        var minDamage = SearchHelpers.BinarySearchLowerBound(left: 4, right: 200,
            damage => Fight(map, units, damage).Winners.All(x => x is { Type: 'E', IsAlive: true }));

        Fight(map, units, minDamage)
            .Apply(Score)
            .Part2();

        int Score((int Round, Unit[] Winners) outcome) =>
            outcome.Winners.Where(x => x.IsAlive).Sum(x => x.HealthPoints) * outcome.Round;
    }

    private static (int Round, Unit[] Winners) Fight(Map<char> map, (char Type, V Position)[] unitsPositions, int elfDamage = 3) {
        var units = unitsPositions
            .Select(v => new Unit(v.Type, v.Position, v.Type is 'E' ? elfDamage : 3))
            .ToArray();

        V[] moves = [V.Up, V.Left, V.Right, V.Down];
        var teams = units.GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.ToDictionary(v => v.Position));

        var rounds = 0;
        while (true) {
            foreach (var unit in units.OrderBy(x => x.Position.Y).ThenBy(x => x.Position.X)) {
                if (unit.IsAlive == false) continue;

                var enemyType = unit.Type == 'G' ? 'E' : 'G';
                var allies = teams[unit.Type];
                var enemies = teams[enemyType];
                if (enemies.Count == 0) {
                    return (rounds, units.Where(x => x.Type == unit.Type).ToArray());
                }

                if (TryAttack(unit, enemies)) continue;

                var maxDistance = int.MaxValue;
                var closest = SearchHelpers
                    .Bfs(from => NextStates(from, allies, enemies), initialStates: unit.Position)
                    .Where(x => enemies.ContainsKey(x.State))
                    .Pipe(x => maxDistance = Math.Min(maxDistance, x.Distance))
                    .TakeWhile(x => x.Distance <= maxDistance)
                    .OrderBy(x => x.State.Y)
                    .ThenBy(x => x.State.X)
                    .FirstOrDefault();

                if (closest is null) continue;

                var nextMove = closest.FromStart().Skip(1).First();
                allies.Remove(unit.Position);
                unit.Position = nextMove;
                allies[unit.Position] = unit;

                TryAttack(unit, enemies);
            }
            rounds++;
        }

        IEnumerable<V> NextStates(V from, Dictionary<V, Unit> allies, Dictionary<V, Unit> enemies) =>
            enemies.ContainsKey(from)
                ? []
                : moves
                    .Select(move => from + move)
                    .Where(to => !allies.ContainsKey(to) && (enemies.ContainsKey(to) || map[to] == '.'));

        bool TryAttack(Unit attacker, Dictionary<V, Unit> enemies) {
            var enemyToAttack = attacker.Position.Area4()
                .Select(enemies.GetValueOrDefault)
                .OfType<Unit>()
                .OrderBy(x => x.HealthPoints)
                .ThenBy(x => x.Position.Y)
                .ThenBy(x => x.Position.X)
                .FirstOrDefault();
            if (enemyToAttack is null) return false;

            enemyToAttack.HealthPoints -= attacker.Damage;
            if (!enemyToAttack.IsAlive) {
                enemies.Remove(enemyToAttack.Position);
            }

            return true;
        }
    }
}
