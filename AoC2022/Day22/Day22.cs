using System.Text.RegularExpressions;

namespace AoC2022.Day22;

public partial class Day22 {
    private record Instruction(char Direction, int Moves);

    private record State(V Position, V Direction) {
        public static implicit operator State((V Position, V Direction) tuple) =>
            new(tuple.Position, tuple.Direction);

        public long ToScore() {
            return 1000 * (Position.Y + 1) + 4 * (Position.X + 1) + Direction switch {
                _ when Direction == V.Right => 0,
                _ when Direction == V.Left => 2,
                _ when Direction == V.Up => 3,
                _ when Direction == V.Down => 1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public State NextState => this with { Position = Position + Direction };

        public State Rotate(char c) {
            return this with {
                Direction = c switch {
                    'R' => Direction.Rotate(90),
                    'L' => Direction.Rotate(-90),
                    _ => Direction,
                }
            };
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var (map, instructions) = ParseInput(input);
        var yMax = map.SizeY;
        var xMax = map.SizeX;
        var tileSize = Math.Max(yMax, xMax) / 4;
        var startX = 0;
        for (; startX < xMax && map[0, startX] != '.'; startX++) {
        }

        var initialState = new State(new V(startX, 0), V.Right);

        EnumeratePositions(map, initialState, instructions, Normalize)
            .Last()
            .ToScore().Part1();

        EnumeratePositions(map, initialState, instructions, Normalize2)
            .Last()
            .ToScore().Part2();

        State Normalize(State currentState) => currentState with { Position = currentState.Position.Mod(yMax, xMax) };

        State Normalize2(State currentState) => xMax > yMax ? Normalize22(currentState) : Normalize21(currentState);

        // #12
        // #3#
        // 45#
        // 6##
        State Normalize21(State currentState) {
            var (pos, currentDirection) = currentState;
            var (tileX, tileY) = pos / tileSize;
            if (pos.X >= xMax) {
                return tileY switch {
                    0 => (new V(2 * tileSize - 1, 3 * tileSize - 1 - pos.Y), V.Left), // 2 -> 5 (right => left)
                    1 => (new V(2 * tileSize + pos.Y % tileSize, tileSize - 1), V.Up), // 3 -> 2 (right => up)
                    2 => (new V(xMax - 1, tileSize - 1 - pos.Y % tileSize), V.Left), // 5 -> 2 (right => left)
                    3 => (new V(tileSize + pos.Y % tileSize, 3 * tileSize - 1), V.Up), // 6 -> 5 (right => up)
                    _ => throw new Exception($"unexpected tile index: {tileY}"),
                };
            }

            if (pos.X < 0) {
                return tileY switch {
                    0 => (new V(0, 3 * tileSize - 1 - pos.Y), V.Right), // 1 <- 4 (left => right)
                    1 => (new V(pos.Y % tileSize, 2 * tileSize), V.Down), // 3 <- 4 (right => down)
                    2 => (new V(tileSize, tileSize - 1 - pos.Y % tileSize), V.Right), // 4 -> 1 (left => right)
                    3 => (new V(tileSize + pos.Y % tileSize, 0), V.Down), // 6 -> 1 (left => down)
                    _ => throw new Exception($"unexpected tile index: {tileY}"),
                };
            }

            if (pos.Y < 0) {
                return tileX switch {
                    0 => (new V(tileSize, tileSize + pos.X), V.Right), // 4 -> 3 (up => right)
                    1 => (new(0, 3 * tileSize + pos.X % tileSize), V.Right), // 1 -> 6 (up => right)
                    2 => (new V(pos.X % tileSize, yMax - 1), V.Up), // 2 -> 6 (up => up)
                    _ => throw new Exception($"unexpected tile index: {tileX}"),
                };
            }

            if (pos.Y >= yMax) {
                return tileX switch {
                    0 => (new V(2 * tileSize + pos.X % tileSize, 0), V.Down), // 6 -> 2 (down => down)
                    1 => (new(tileSize - 1, 3 * tileSize + pos.X % tileSize), V.Left), // 5 -> 6 (down => left)
                    2 => (new V(2 * tileSize - 1, tileSize + pos.X % tileSize), V.Left), // 2 -> 3 (down => left)
                    _ => throw new Exception($"unexpected tile index: {tileX}"),
                };
            }

            return new(pos, currentDirection);
        }

        // ##1#
        // 234#
        // ##45
        State Normalize22(State currentState) {
            var (pos, currentDirection) = currentState;
            var (tileX, tileY) = pos / tileSize;

            if (pos.X >= xMax) {
                return tileY switch {
                    0 => new(new V(xMax - 1, yMax - 1 - pos.Y), V.Left), // 1 -> 6 (right => left)
                    1 => new(new V(xMax - 1 - pos.Y % tileSize, 2 * tileSize), V.Down), // 4 -> 6 (right => down)
                    2 => new(new V(3 * tileSize - 1, tileSize - 1 - pos.Y % tileSize),
                        V.Left), // 6 -> 1 (right => left)
                    _ => throw new Exception($"unexpected tile index: {tileY}"),
                };
            }

            if (pos.X < 0) {
                return tileY switch {
                    0 => new(new V(xMax - 1, yMax - 1 - pos.Y), V.Down), // 3 <- 1 (left => down)
                    1 => new(new V(xMax - 1 - pos.Y % tileSize, yMax), V.Up), // 6 <- 2 (left => up)
                    2 => new(new V(2 * tileSize - 1 - pos.Y % tileSize, tileSize * 2 - 1), V.Up),
                    _ => throw new Exception($"unexpected tile index: {tileY}"),
                };
            }

            if (pos.Y < 0) {
                return tileX switch {
                    0 => (new V(3 * tileSize - 1 - pos.X, 0), V.Down), // 2 -> 1 (up => down)
                    1 => (new(2 * tileSize, pos.X % tileSize), V.Right), // 3 -> 1 (up => right)
                    2 => (new V(tileSize - 1 - pos.X % tileSize, tileSize), V.Down), // 1 -> 2 (up => down)
                    3 => (new V(3 * tileSize - 1, 2 * tileSize - 1 - pos.X % tileSize), V.Left), // 6 -> 4 (up => left)
                    _ => throw new Exception($"unexpected tile index: {tileX}"),
                };
            }

            if (pos.Y >= yMax) {
                return tileX switch {
                    0 => (new V(3 * tileSize - 1 - pos.X, yMax - 1), V.Up), // 2 -> 5 (down => up)
                    1 => (new V(2 * tileSize, yMax - 1 - pos.X % tileSize), V.Right), // 3 -> 5 (down => right)
                    2 => (new V(tileSize - 1 - pos.X % tileSize, 2 * tileSize - 1), V.Up), // 5 -> 2 (down => up)
                    3 => (new V(0, 2 * tileSize - 1 - pos.X % tileSize), V.Left), // 6 -> 4 (up => left)
                    _ => throw new Exception($"unexpected tile index: {tileX}"),
                };
            }

            return new(pos, currentDirection);
        }
    }

    private static (Map<char> Map, Instruction[] Instructions) ParseInput(IEnumerable<string> input) {
        var blocks = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();
        var rawMap = blocks[0];
        var maxX = rawMap.Max(x => x.Length);
        var map = new Map<char>(maxX, rawMap.Count);
        foreach (var coordinate in map.Coordinates()) {
            map[coordinate] = coordinate.X < rawMap[(int)coordinate.Y].Length
                ? rawMap[(int)coordinate.Y][(int)coordinate.X]
                : ' ';
        }

        var instructions =
            Regex.Matches(blocks[1].Single(), @"((?<Direction>R|L)?(?<Moves>\d+))")
                .Select(x => {
                    var direction = x.Groups["Direction"];
                    var moves = int.Parse(x.Groups["Moves"].Value);
                    return new Instruction(direction.Success ? direction.ValueSpan[0] : 'F', moves);
                })
                .ToArray();
        return (map, instructions);
    }

    private static IEnumerable<State> EnumeratePositions(
        Map<char> map,
        State initialState,
        IEnumerable<Instruction> instructions,
        Func<State, State> handleWrap) {
        var state = initialState;
        foreach (var (direction, moves) in instructions) {
            state = state.Rotate(direction);
            for (var move = 0; move < moves; move++)
                yield return state = GetNextState(map, state, handleWrap);
        }
    }

    private static State GetNextState(Map<char> map, State currentState, Func<State, State> handleWrap) {
        char GetAt(V pos) => map.GetValueOrDefault(pos, ' ');

        var nextState = currentState;
        do nextState = handleWrap(nextState.NextState);
        while (GetAt(nextState.Position) == ' ');

        return GetAt(nextState.Position) switch {
            '#' => currentState,
            '.' => nextState,
            _ => throw new Exception($"WTF: '{GetAt(nextState.Position)}'"),
        };
    }
}