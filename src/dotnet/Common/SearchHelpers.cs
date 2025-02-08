using System.Numerics;
using System.Text;

namespace Common;

public static class SearchHelpers {
    public static IEnumerable<SearchPathItem<TState>> Dfs<TState>(
        Func<TState, IEnumerable<TState>> getNextState,
        TState start
    ) {
        var visited = new HashSet<TState> { start };
        var current = SearchPathItem.Start(start);
        while (current != null) {
            var next = getNextState(current.State).FirstOrDefault(visited.Add);
            if (next is null) {
                yield return current;
                current = current.Prev;
            }
            else current = current.Next(next);
        }
    }

    public static IEnumerable<SearchPathItem<V>> Dfs<T>(this Map<T> map, CanMove canMove, V start) {
        return Dfs(getNextState: from => from.Area4().Where(v => v.IsInRange(map) && canMove(from, v)), start);
    }

    public static IEnumerable<SearchPathItem<TState>> Bfs<TState>(
        Func<TState, IEnumerable<TState>> getNextState,
        int? maxDistance = null,
        params TState[] initialStates
    ) {
        var queue = new Queue<SearchPathItem<TState>>();
        foreach (var state in initialStates) queue.Enqueue(SearchPathItem.Start(state));
        var visited = initialStates.ToHashSet();

        while (queue.TryDequeue(out var item)) {
            if (item.Distance > maxDistance)
                continue;
            yield return item;
            foreach (var nextState in getNextState(item.State).Where(visited.Add))
                queue.Enqueue(item.Next(nextState));
        }
    }

    public static IEnumerable<SearchPathItem<V>> Bfs<T>(this Map<T> map, CanMove canMove, params V[] initial) {
        return Bfs(getNextState: state => {
                var pos = state;
                return pos.Area4().Where(v => v.IsInRange(map) && canMove(pos, v));
            }, maxDistance: null, initialStates: initial
        );
    }

    public static IEnumerable<SearchPathItem<TState>> Dijkstra<TState>(
        Func<SearchPathItem<TState>, IEnumerable<SearchPathItem<TState>>> getNextState,
        params TState[] initialStates
    ) where TState : notnull {
        var priorityQueue =
            new PriorityQueue<SearchPathItem<TState>, int>(initialStates.Select(x => (SearchPathItem.Start(x), 0)));
        var distanceMap = initialStates.ToDictionary(x => x, _ => 0);
        while (priorityQueue.TryDequeue(out var currentState, out _)) {
            yield return currentState;
            foreach (var nextState in getNextState(currentState)) {
                if (distanceMap.TryGetValue(nextState.State, out var currentDistance) &&
                    currentDistance <= nextState.Distance)
                    continue;
                distanceMap[nextState.State] = nextState.Distance;
                priorityQueue.Enqueue(nextState, nextState.Distance);
            }
        }
    }


    public static void Visualize<T, TS>(this Map<T> map, SearchPathItem<TS> pathItem,
        Func<TS, V> stateToCoordinate, Func<TS, (string, ConsoleColor?)> getPathStateFormat) {
        var path = pathItem.FromStart().ToArray();
        Console.Clear();
        foreach (var y in map.RowIndices) {
            foreach (var x in map.ColumnIndices)
                Console.Write(map[new V(x, y)]);
            Console.WriteLine();
        }

        Console.CursorVisible = false;

        foreach (var p in path) {
            var coordinate = stateToCoordinate(p);
            Console.SetCursorPosition((int)coordinate.X, (int)coordinate.Y);
            var (text, color) = getPathStateFormat(p);
            if (color != null) Console.ForegroundColor = color.Value;
            Console.Write(text);
            Console.ResetColor();
        }

        Console.CursorVisible = true;
        Console.WriteLine();
        path.Dump("Full path:\n", separator: " => ");
    }

    public static string BuildGraphViz<TNode>(TNode[] start, Func<TNode, IEnumerable<TNode>> getNextState) {
        var sb = new StringBuilder();
        sb.AppendLine("digraph G {");
        var visited = new HashSet<TNode>();
        var queue = new Queue<TNode>(start);
        while (queue.TryDequeue(out var current)) {
            if (!visited.Add(current)) continue;
            foreach (var next in getNextState(current)) {
                sb.AppendLine($"\t{current} -> {next};");
                queue.Enqueue(next);
            }
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    public static T BinarySearchLowerBound<T>(T left, T right, Func<T, bool> isLessOrEqual) where T : INumber<T> {
        while (left < right) {
            var mid = left + (right - left) / T.CreateChecked(2);
            if (isLessOrEqual(mid)) right = mid;
            else left = mid + T.One;
        }

        return left;
    }

    public static T BinarySearchUpperBound<T>(T left, T right, Func<T, bool> isLessOrEqual) where T : INumber<T> {
        while (left < right) {
            var mid = left + (right - left) / T.CreateChecked(2);
            if (isLessOrEqual(mid)) left = mid + T.One;
            else right = mid;
        }

        return left - T.One;
    }
}

public record SearchPathItem<TState>(TState State, int Distance, SearchPathItem<TState>? Prev) {
    public IEnumerable<TState> BackToStart() => All().Select(x => x.State);

    public IEnumerable<SearchPathItem<TState>> All() {
        for (var cur = this; cur != null; cur = cur.Prev)
            yield return cur;
    }

    public IEnumerable<TState> FromStart() => BackToStart().Reverse();

    public SearchPathItem<TState> Next(TState state, int distanceCost = 1) => new(state, Distance + distanceCost, this);
}

public static class SearchPathItem {
    public static SearchPathItem<TState> Start<TState>(TState state) => new(state, 0, null);
}

public delegate bool CanMove(V from, V to);
