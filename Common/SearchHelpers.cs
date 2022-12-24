namespace Common;

public static class SearchHelpers
{
    public static IEnumerable<TState> Bfs<TState>(
        Func<TState, IEnumerable<TState>> getNextState,
        params TState[] initialStates
    )
    {
        var queue = new Queue<TState>();
        foreach (var state in initialStates) queue.Enqueue(state);
        var visited = initialStates.ToHashSet();

        while (queue.TryDequeue(out var state))
        {
            yield return state;
            foreach (var nextState in getNextState(state).Where(visited.Add))
                queue.Enqueue(nextState);
        }
    }

    public static IEnumerable<BfsState> Bfs<T>(this T[][] map, CanMove canMove, params V[] initial)
    {
        return Bfs(getNextState: state =>
            {
                var result = state;
                return result.Pos.Area4()
                    .Where(v => v.IsInRange(map) && canMove(result.Pos, v))
                    .Select(v => new BfsState(v, result.Steps + 1));
            }, initialStates: initial.Select(v => new BfsState(v, 0)).ToArray()
        );
    }
}

public record BfsState(V Pos, int Steps);
public delegate bool CanMove(V from, V to);