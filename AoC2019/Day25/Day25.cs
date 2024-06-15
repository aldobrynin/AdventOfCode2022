using System.Text;
using System.Text.RegularExpressions;

namespace AoC2019.Day25;

public static partial class Day25 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.First().ToLongArray();
        var computer = new IntCodeComputer(program);

        var door = Direction.Unknown;

        var game = new Game(computer);

        foreach (var line in ReadLine(computer).ToBlockingEnumerable()) {
            Console.WriteLine(line);
            if (game.ReadState == ReadState.None) {
                if (line.StartsWith("==")) {
                    var prevRoom = game.CurrentRoomName;
                    game.CurrentRoomName = line[3..^3];
                    Room current;

                    if (door != Direction.Unknown) {
                        var prev = game.Map[prevRoom];
                        current = prev.Doors[door];

                        if (current.Visited == false) {
                            current.Name = game.CurrentRoomName;
                            current.Visited = true;
                            current.Doors[door.Opposite()] = prev;
                            game.Map.Add(game.CurrentRoomName, current);
                        }
                    }
                    else {
                        current = new Room(game.CurrentRoomName) { Visited = true };
                        game.Map.Add(game.CurrentRoomName, current);
                    }
                }
                else if (line == "Command?") {
                    var current = game.CurrentRoom;
                    if (game.IsTooHeavy) {
                        var indexToDrop = Random.Shared.Next(game.Inventory.Count);
                        var item = game.Inventory[indexToDrop];
                        game.DropItem(item);
                        continue;
                    }

                    if (game.IsTooLight) {
                        var items = current.Items.ToArray();
                        var indexToPick = Random.Shared.Next(items.Length);
                        game.TakeItem(items[indexToPick]);
                        continue;
                    }

                    var unknownDirections = current.Doors
                        .Where(x => x.Value.Visited == false)
                        .Select(x => x.Key)
                        .ToArray();

                    if (current.IsSecurityCheckpoint) {
                        unknownDirections = unknownDirections.Where(x => x != Direction.West).ToArray();
                    }

                    if (unknownDirections.Any()) {
                        door = unknownDirections.First();
                    }
                    else if (game.AllRooms.FirstOrDefault(x => x is { IsSecurityCheckpoint: false, VisitedAllDoors: false }) is
                             { } roomWithUnvisitedDoor) {
                        door = PathTo(current, roomWithUnvisitedDoor).First();
                    }
                    else if (current.IsSecurityCheckpoint) {
                        door = Direction.West;
                    }
                    else if (game.Inventory.Count == game.AllItems.Count) {
                        door = PathTo(current, game.Map["Security Checkpoint"]).First();
                    }
                    else {
                        var itemRoom = game.Map.Values.First(x => !x.IsSecurityCheckpoint && x.Items.Any());
                        if (current == itemRoom) {
                            game.TakeItem(current.Items.First());
                            continue;
                        }

                        door = PathTo(current, itemRoom).First();
                    }

                    game.Computer.AddAsciiInput(door.ToString().ToLower());
                }
                else if (line == "Doors here lead:") game.ReadState = ReadState.Doors;
                else if (line == "Items here:") game.ReadState = ReadState.Items;
                else if (line.StartsWith("A loud, robotic voice says")) {
                    game.CurrentRoomName = "Security Checkpoint";
                    if (line.Contains("lighter")) game.IsTooHeavy = true;
                    else if (line.Contains("heavier")) game.IsTooLight = true;
                }
                else if (line.Contains("You should be able to get in by typing", StringComparison.OrdinalIgnoreCase)) {
                    Regex.Replace(line, "\\D", "").Part1();
                }
            }
            else {
                if (string.IsNullOrEmpty(line)) game.ReadState = ReadState.None;
                else if (game.ReadState == ReadState.Doors)
                    game.CurrentRoom.AddNeighbor(Enum.Parse<Direction>(line[2..], ignoreCase: true));
                else if (game.ReadState == ReadState.Items) {
                    var item = line[2..];
                    if (!game.BlackList.Contains(item)) {
                        game.CurrentRoom.Items.Add(item);
                        game.AllItems.Add(item);
                    }
                }
            }
        }
    }

    private static async IAsyncEnumerable<string> ReadLine(IntCodeComputer computer) {
        var sb = new StringBuilder();
        await foreach (var output in computer.ReadAllOutputs()) {
            if (output == 10) {
                yield return sb.ToString();
                sb.Clear();
            }
            else {
                sb.Append((char)output);
            }
        }
    }

    private static IEnumerable<Direction> PathTo(Room start, Room end) {
        return SearchHelpers.Bfs(
                current => current.Room.Doors.Select(s => (s.Key, s.Value)),
                maxDistance: null,
                (Direction: Direction.Unknown, Room: start)
            ).First(x => x.State.Room == end)
            .FromStart()
            .Skip(1)
            .Select(x => x.Direction);
    }

    enum Direction {
        Unknown,
        North,
        South,
        East,
        West,
    }

    enum ReadState {
        None,
        Doors,
        Items,
    }

    class Game(IntCodeComputer computer) {
        public IntCodeComputer Computer { get; } = computer;
        public Dictionary<string, Room> Map { get; } = new();
        public string CurrentRoomName { get; set; } = null!;
        public Room CurrentRoom => Map[CurrentRoomName];
        public IEnumerable<Room> AllRooms => Map.Values;
        public ReadState ReadState { get; set; }
        public List<string> Inventory { get; } = new();
        public bool IsTooLight { get; set; }
        public bool IsTooHeavy { get; set; }
        public HashSet<string> AllItems { get; } = new();
        public HashSet<string> BlackList { get; } = new() {
            "giant electromagnet",
            "infinite loop",
            "molten lava",
            "photons",
            "escape pod",
        };

        public void TakeItem(string item) {
            Inventory.Add(item);
            Map[CurrentRoomName].Items.Remove(item);
            Computer.AddAsciiInput($"take {item}");
            IsTooLight = false;
        }

        public void DropItem(string item) {
            Inventory.Remove(item);
            Map[CurrentRoomName].Items.Add(item);
            Computer.AddAsciiInput($"drop {item}");
            IsTooHeavy = false;
        }
    }

    class Room {
        public Room() : this("Unknown") {
        }

        public Room(string name) {
            Name = name;
        }

        public bool Visited { get; set; }
        public string Name { get; set; }
        public Dictionary<Direction, Room> Doors { get; } = new();
        public HashSet<string> Items { get; } = new();
        public bool VisitedAllDoors => Doors.All(d => d.Value.Visited);

        public void AddNeighbor(Direction direction) {
            if (Doors.ContainsKey(direction)) {
                return;
            }

            Doors.Add(direction, new Room());
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine($"Room: {Name}");
            if (Items.Any()) sb.AppendLine($"Items: {Items.StringJoin()}");
            sb.AppendLine("Doors:");
            foreach (var (direction, nextRoom) in Doors) {
                sb.AppendLine($"\t{direction} -> {nextRoom.Name} (Visited: {nextRoom.Visited})");
            }

            return sb.ToString();
        }

        public bool IsSecurityCheckpoint => Name == "Security Checkpoint";
    }

    private static Direction Opposite(this Direction direction) {
        return direction switch {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
