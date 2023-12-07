namespace AoC2020.Day11;

public class Day11 {
    private const char Floor = '.';
    private const char Empty = 'L';
    private const char Occupied = '#';

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input.Select(s => s.ToCharArray()).ToArray());

        Simulate(map.Clone())
            .Select(s => s.Coordinates().Count(v => s[v] == Occupied))
            .Last()
            .Dump("Part1: ");

        SimulatePart2(map.Clone())
            .Select(s => s.Coordinates().Count(v => s[v] == Occupied))
            .Last()
            .Dump("Part2: ");
    }

    private static IEnumerable<Map<char>> Simulate(Map<char> map) {
        while (true) {
            var changedSeats =
                map.Coordinates()
                    .Where(x => map[x] != Floor)
                    .Where(x => map[x] == Empty && CanSeat(x) || map[x] == Occupied && ShouldLeave(x))
                    .ToArray();
            if (changedSeats.Length == 0) break;
            foreach (var coordinate in changedSeats) {
                map[coordinate] = map[coordinate] == Empty ? Occupied : Empty;
            }

            yield return map;
        }

        bool ShouldLeave(V x) {
            return map.Area8(x).Count(neighbor => map[neighbor] == Occupied) >= 4;
        }


        bool CanSeat(V x) {
            return map.Area8(x).All(neighbor => map[neighbor] != Occupied);
        }
    }

    private static IEnumerable<Map<char>> SimulatePart2(Map<char> map) {
        while (true) {
            var changedSeats =
                map.Coordinates()
                    .Where(x => map[x] != Floor)
                    .Where(x => map[x] == Empty && CanSeat(x) || map[x] == Occupied && ShouldLeave(x))
                    .ToArray();
            if (changedSeats.Length == 0) break;
            foreach (var coordinate in changedSeats) {
                map[coordinate] = map[coordinate] == Empty ? Occupied : Empty;
            }

            yield return map;
        }

        bool CanSeat(V x) {
            return EnumerateVisibleSeats(map, x).All(neighbor => map[neighbor] != Occupied);
        }

        bool ShouldLeave(V x) {
            return EnumerateVisibleSeats(map, x).Count(neighbor => map[neighbor] == Occupied) >= 5;
        }
    }

    private static IEnumerable<V> EnumerateVisibleSeats(Map<char> map, V x) {
        return V.Directions8
            .Select(direction => FirstVisibleSeatOrDefault(map, x, direction))
            .OfType<V>();
    }

    private static V? FirstVisibleSeatOrDefault(Map<char> map, V x, V direction) {
        for (var seat = x + direction; seat.IsInRange(map); seat += direction) {
            if (map[seat] == Floor) continue;
            return seat;
        }

        return null;
    }
}