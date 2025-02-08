using System.Collections.Immutable;

namespace AoC2021.Day12;

public partial class Day12
{
    public static void Solve(IEnumerable<string> input)
    {
        var connections = input.Select(x => x.Split('-'))
            .Select(x => (From: x[0], To: x[1]))
            .ToArray();

        var paths = connections.Concat(connections.Select(x => (From: x.To, To: x.From)))
            .Where(x => x.To != "start")
            .Where(x => x.From != "end")
            .ToLookup(x => x.From, x => x.To);

        FindPaths(paths, "start").Count().Part1();
        FindPaths(paths, "start", canRevisitSmallOnce: true).Count().Part2();
    }

    private record State(string Current, ImmutableList<string> Visited, bool CanRevisitSmall);

    private static IEnumerable<string> FindPaths(ILookup<string, string> paths, string initial, bool canRevisitSmallOnce = false)
    {
        var queue = new Queue<State>();

        queue.Enqueue(new State(initial, new List<string> { initial }.ToImmutableList(),
            CanRevisitSmall: canRevisitSmallOnce));
        while (queue.TryDequeue(out var item))
        {
            if (item.Current == "end")
                yield return item.Visited.StringJoin("->");

            foreach (var path in paths[item.Current])
            {
                if (char.IsUpper(path, 0))
                    queue.Enqueue(new State(path, item.Visited.Add(path), item.CanRevisitSmall));
                else
                {
                    switch (item.Visited.Count(x => x == path))
                    {
                        case 1 when item.CanRevisitSmall:
                            queue.Enqueue(new State(path, item.Visited.Add(path), CanRevisitSmall: false));
                            break;
                        case 0:
                            queue.Enqueue(new State(path, item.Visited.Add(path), item.CanRevisitSmall));
                            break;
                    }
                }
            }
        }
    }
}