namespace Common;

public static class SearchHelpers {
    public static IEnumerable<TState> Bfs<TState>(
        Func<TState, IEnumerable<TState>> getNextState,
        params TState[] initialStates
    ) {
        return Bfs(getNextState, maxDistance: null, initialStates)
            .Select(x => x.State);
    }

    public static IEnumerable<BfsPathItem<TState>> Bfs<TState>(
        Func<TState, IEnumerable<TState>> getNextState,
        int? maxDistance = null,
        params TState[] initialStates
    ) {
        var queue = new Queue<BfsPathItem<TState>>();
        foreach (var state in initialStates) queue.Enqueue(new BfsPathItem<TState>(state, 0, null));
        var visited = initialStates.ToHashSet();

        while (queue.TryDequeue(out var item)) {
            if (item.Distance > maxDistance)
                continue;
            yield return item;
            foreach (var nextState in getNextState(item.State).Where(visited.Add))
                queue.Enqueue(new BfsPathItem<TState>(nextState, item.Distance + 1, item));
        }
    }

    public static IEnumerable<BfsPathItem<V>> Bfs<T>(this T[][] map, CanMove canMove, params V[] initial) {
        return Bfs(getNextState: state => {
                var pos = state;
                return pos.Area4().Where(v => v.IsInRange(map) && canMove(pos, v));
            }, maxDistance: null, initialStates: initial
        );
    }

    public static IEnumerable<BfsPathItem<V>> Bfs<T>(this Map<T> map, CanMove canMove, params V[] initial) {
        return Bfs(getNextState: state => {
                var pos = state;
                return pos.Area4().Where(v => v.IsInRange(map) && canMove(pos, v));
            }, maxDistance: null, initialStates: initial
        );
    }
}

public record BfsPathItem<TState>(TState State, int Distance, BfsPathItem<TState>? Prev)
{
    public IEnumerable<TState> BackToStart() => All().Select(x => x.State);

    public IEnumerable<BfsPathItem<TState>> All() {
        for (var cur = this; cur != null; cur = cur.Prev)
            yield return cur;
    }

    public IEnumerable<TState> FromStart() => BackToStart().Reverse();
    
    public BfsPathItem<TState> Next(TState state, int distance = 1) => new(state, Distance + distance, this);
}

public delegate bool CanMove(V from, V to);