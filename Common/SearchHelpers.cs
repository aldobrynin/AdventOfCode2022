namespace Common;

public static class SearchHelpers {
    public static IEnumerable<TState> Bfs<TState>(
        Func<TState, IEnumerable<TState>> getNextState,
        params TState[] initialStates
    ) {
        return Bfs(getNextState, maxDistance: null, initialStates)
            .Select(x => x.State);
    }

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

    public static IEnumerable<SearchPathItem<V>> Bfs<T>(this T[][] map, CanMove canMove, params V[] initial) {
        return Bfs(getNextState: state => {
                var pos = state;
                return pos.Area4().Where(v => v.IsInRange(map) && canMove(pos, v));
            }, maxDistance: null, initialStates: initial
        );
    }

    public static IEnumerable<SearchPathItem<V>> Bfs<T>(this Map<T> map, CanMove canMove, params V[] initial) {
        return Bfs(getNextState: state => {
                var pos = state;
                return pos.Area4().Where(v => v.IsInRange(map) && canMove(pos, v));
            }, maxDistance: null, initialStates: initial
        );
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