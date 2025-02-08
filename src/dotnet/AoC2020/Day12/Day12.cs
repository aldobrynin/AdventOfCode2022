namespace AoC2020.Day12;

public partial class Day12 {


    public static void Solve(IEnumerable<string> input) {
        var commands = input.Select(Command.From).ToArray();
        ExecutePart1(commands).Last().MLen.Part1();
        ExecutePart2(commands).Last().MLen.Part2();
    }

    private static IEnumerable<V> ExecutePart1(IEnumerable<Command> commands) {
        var position = V.Zero;
        var direction = V.E;
        foreach (var command in commands) {
            switch (command.Type) {
                case 'L' or 'R':
                    direction = direction.Rotate(command.Type == 'L' ? command.Value : 360 - command.Value);
                    break;
                case 'F':
                    position += direction * command.Value;
                    break;
                default:
                    position += CompassToV[command.Type] * command.Value;
                    break;
            }

            yield return position;
        }
    }

    private static IEnumerable<V> ExecutePart2(IReadOnlyCollection<Command> commands) {
        var position = V.Zero;
        var waypointPosition = position + new V(10, 1);
        foreach (var command in commands) {
            switch (command.Type) {
                case 'L' or 'R':
                    var degrees = command.Type == 'L' ? command.Value : -command.Value;
                    waypointPosition = waypointPosition.RotateAround(degrees, position);
                    break;
                case 'F':
                    var relative = waypointPosition - position;
                    position += (waypointPosition - position) * command.Value;
                    waypointPosition = position + relative;
                    break;
                default:
                    waypointPosition += CompassToV[command.Type] * command.Value;
                    break;
            }

            yield return position;
        }
    }

    private record Command(char Type, int Value) {
        public static Command From(string s) {
            return new Command(s[0], int.Parse(s[1..]));
        }
    }

    private static readonly Dictionary<char, V> CompassToV = new() {
        ['N'] = V.S,
        ['S'] = V.N,
        ['E'] = V.E,
        ['W'] = V.W,
    };
}