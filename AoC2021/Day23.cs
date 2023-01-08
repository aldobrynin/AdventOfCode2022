using System.Collections.Immutable;
using System.Text;
using Common;

namespace AoC2021;

public class Day23
{
    private record State(
        ImmutableArray<string> Hallway,
        ImmutableArray<ImmutableArray<string>> Rooms,
        int Cost = 0,
        int RoomCapacity = 2)
    {
        public override string ToString()
        {
            var rooms = Rooms.Select(x => x.ToArray()).ToArray();

            var sb = new StringBuilder();
            sb.Append('#').Append($"Cost: {Cost,4} ").Append('#').AppendLine();
            sb.Append('#', 13).AppendLine();
            sb.Append('#').AppendJoin(string.Empty, Hallway.Select(x => x[0])).Append('#').AppendLine();

            for (var i = RoomCapacity - 1; i >= 0; i--)
            {
                var isFirstSeat = i == RoomCapacity - 1;
                sb.Append(' ', isFirstSeat ? 0 : 2).Append('#', isFirstSeat ? 3 : 1);
                sb.AppendJoin('#', rooms.Select(r => r.Length - 1 >= i ? r[i][0] : '.').ToArray());
                sb.Append('#', isFirstSeat ? 3 : 1).AppendLine();
            }

            sb.Append(' ', 2).Append('#', 9);
            return sb.ToString();
        }

        public bool IsComplete()
        {
            return Rooms.Indices()
                .Select(roomId => (Room: Rooms[roomId].Select(r => r[0]),
                    Expected: Enumerable.Repeat((char)('A' + roomId), RoomCapacity)))
                .All(t => t.Room.SequenceEqual(t.Expected));
        }

        public string GetCacheKey()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(string.Empty, Hallway);
            sb.AppendJoin(string.Empty,
                Rooms.SelectMany(room =>
                    Enumerable.Range(0, RoomCapacity).Select(i => room.Length > i ? room[i] : "."))
            );
            return sb.ToString();
        }

        public static State FromInput(char[][] map)
        {
            var numbers = new int[4];

            string ToId(char c)
            {
                var group = c - 'A';
                var number = numbers[group]++;
                return $"{c}{number}";
            }

            var rooms = map
                .Transpose()
                .Rows().Select(r => r.Select(ToId).Reverse().ToImmutableArray())
                .ToImmutableArray();

            return new State(Enumerable.Repeat(".", 11).ToImmutableArray(),
                rooms,
                Cost: 0,
                RoomCapacity: rooms.Max(x => x.Length)
            );
        }
    }

    public static void Solve(IEnumerable<string> input)
    {
        var rooms = input.Skip(2)
            .Select(line => line.Where(char.IsLetter).ToArray())
            .Where(x => x.Length > 0)
            .ToArray();
        FindSolution(rooms)
            .First(s => s.IsComplete())
            .Cost.Dump("Part1: ");

        var extraPod = new[]
        {
            new[] { 'D', 'C', 'B', 'A' },
            new[] { 'D', 'B', 'A', 'C' },
        };
        var part2Rooms = rooms.Take(1).Concat(extraPod).Concat(rooms.TakeLast(1))
            .ToArray();
        FindSolution(part2Rooms)
            .First(s => s.IsComplete())
            .Cost.Dump("Part2: ");
    }

    private static IEnumerable<State> FindSolution(char[][] map)
    {
        var state = State.FromInput(map);

        var queue = new PriorityQueue<State, long>();
        var visited = new Dictionary<string, int>();
        queue.Enqueue(state, state.Cost);
        visited.Add(state.GetCacheKey(), state.Cost);

        while (queue.TryDequeue(out var item, out _))
        {
            yield return item;
            foreach (var nextState in GetNextStates(item))
            {
                var stateKey = nextState.GetCacheKey();
                if (visited.GetValueOrDefault(stateKey, int.MaxValue) <= nextState.Cost) continue;
                visited[stateKey] = nextState.Cost;
                queue.Enqueue(nextState, nextState.Cost);
            }
        }
    }

    private static IEnumerable<State> GetNextStates(State current)
    {
        var canMoveIn = false;
        foreach (var state in GetMovesInRooms(current))
        {
            yield return state;
            canMoveIn = true;
        }

        if (canMoveIn) yield break;
        foreach (var state in GetMovesFromRooms(current))
            yield return state;
    }

    private static IEnumerable<State> GetMovesFromRooms(State current)
    {
        foreach (var roomId in current.Rooms.Indices().Where(roomId => CanLeaveRoom(current, roomId)))
        {
            var room = current.Rooms[roomId];
            var pod = room[^1];
            foreach (var (position, steps) in GetPossibleMovesToHall(current, roomId))
            {
                yield return new State(
                    current.Hallway.SetItem(position, pod),
                    current.Rooms.SetItem(roomId, room.RemoveAt(room.Length - 1)),
                    current.Cost + steps * GetEnergy(pod[0]),
                    RoomCapacity: current.RoomCapacity
                );
            }
        }
    }

    private static IEnumerable<State> GetMovesInRooms(State current)
    {
        foreach (var roomInd in current.Rooms.Indices().Where(roomId => CanLeaveRoom(current, roomId)))
        {
            var room = current.Rooms[roomInd];
            var pod = room[^1];
            var (targetRoomId, targetRoomEntrance) = GetRoomByPod(pod);
            var targetRoom = current.Rooms[targetRoomId];
            if (!CanJoinRoom(current, pod, targetRoom)) continue;

            var entranceToRoom = GetRoomEntrance(roomInd);
            if (!IsPassageFree(entranceToRoom, targetRoomEntrance, current.Hallway)) continue;

            var newRooms = current.Rooms
                .SetItem(roomInd, room.RemoveAt(room.Length - 1))
                .SetItem(targetRoomId, targetRoom.Add(pod));
            var distance = Math.Abs(entranceToRoom - targetRoomEntrance)
                           + GetDistanceToEntrance(current, roomInd)
                           + GetDistanceFromEntrance(current, targetRoomId);
            yield return current with { Rooms = newRooms, Cost = current.Cost + distance * GetEnergy(pod[0]) };
        }

        foreach (var hallwayPos in current.Hallway.Indices().Where(pos => current.Hallway[pos] != "."))
        {
            var pod = current.Hallway[hallwayPos];
            var (podRoomId, entranceToRoom) = GetRoomByPod(pod);
            var room = current.Rooms[podRoomId];
            if (!CanJoinRoom(current, pod, room)) continue;
            if (!IsPassageFree(hallwayPos, entranceToRoom, current.Hallway)) continue;

            var distance = Math.Abs(entranceToRoom - hallwayPos) + GetDistanceFromEntrance(current, podRoomId);
            yield return new State(
                current.Hallway.SetItem(hallwayPos, "."),
                current.Rooms.SetItem(podRoomId, room.Add(pod)),
                current.Cost + distance * GetEnergy(pod[0]),
                RoomCapacity: current.RoomCapacity
            );
        }
    }

    private static bool CanLeaveRoom(State current, int roomId)
    {
        var room = current.Rooms[roomId];
        var expectedPod = (char)('A' + roomId);
        return room.Any(r => r[0] != expectedPod);
    }

    private static bool CanJoinRoom(State state, string pod, ImmutableArray<string> room)
    {
        return room.Length != state.RoomCapacity && room.All(c => c[0] == pod[0]);
    }

    private static (int RoomId, int RoomEntrancePosition) GetRoomByPod(string pod)
    {
        var podRoomId = pod[0] - 'A';
        return (podRoomId, GetRoomEntrance(podRoomId));
    }

    private static IEnumerable<(int Position, int Steps)> GetPossibleMovesToHall(State current, int roomId)
    {
        var entrance = GetRoomEntrance(roomId);
        return
            GetReachableHallPositions(entrance, current.Hallway)
                .Select(hallPosition => (hallPosition,
                    Math.Abs(entrance - hallPosition) + GetDistanceToEntrance(current, roomId)));
    }

    private static int GetDistanceToEntrance(State state, int roomId) =>
        state.RoomCapacity + 1 - state.Rooms[roomId].Length;

    private static int GetDistanceFromEntrance(State state, int roomId) =>
        state.RoomCapacity - state.Rooms[roomId].Length;

    private static IEnumerable<int> GetReachableHallPositions(int start, IReadOnlyList<string> hallway)
    {
        for (var i = start + 1; i < hallway.Count && hallway[i] == "."; i++)
            if (!IsRoomEntrance(i))
                yield return i;

        for (var i = start - 1; i >= 0 && hallway[i] == "."; i--)
            if (!IsRoomEntrance(i))
                yield return i;
    }

    private static bool IsPassageFree(int start, int end, IReadOnlyList<string> hallway)
    {
        var incr = Math.Sign(end - start);
        for (var i = start + incr; i != end; i += incr)
            if (hallway[i] != ".")
                return false;

        return true;
    }

    private static int GetRoomEntrance(int roomId) => (roomId + 1) * 2;

    private static bool IsRoomEntrance(int position) => position != 0 && position != 10 && position % 2 == 0;

    private static int GetEnergy(char pod) => (int)Math.Pow(10, pod - 'A');
}