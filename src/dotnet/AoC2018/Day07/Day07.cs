using Common.AoC;

namespace AoC2018.Day07;

public static partial class Day07 {
    public static void Solve(IEnumerable<string> input) {
        var edges = input
            .Select(line => line.Split(' '))
            .Select(x => (From: x[1][0], To: x[7][0]))
            .ToArray();
        var workersCount = AoCContext.IsSample ? 2 : 5;
        var timePerStep = AoCContext.IsSample ? 0 : 60;

        var graph = new Dictionary<char, HashSet<char>>();
        var indegree = new Dictionary<char, int>();
        foreach (var (from, to) in edges) {
            graph.GetOrAdd(from, _ => []).Add(to);
            graph.GetOrAdd(to, _ => []);
            indegree.GetOrAdd(from, _ => 0);
            indegree[to] = indegree.GetValueOrDefault(to) + 1;
        }
        var roots = indegree
            .Where(x => x.Value == 0)
            .Select(x => x.Key)
            .ToArray();

        SortSteps().StringJoin(string.Empty).Part1();
        CalculateTimeToFinish().Part2();

        IEnumerable<char> SortSteps() {
            var indegreeLocal = indegree.ToDictionary();

            var queue = new PriorityQueue<char, char>(roots.Select(x => (x, x)));
            while (queue.TryDequeue(out var current, out _)) {
                yield return current;
                foreach (var next in graph[current].Where(next => --indegreeLocal[next] == 0)) {
                    queue.Enqueue(next, next);
                }
            }
        }

        int CalculateTimeToFinish() {
            var indegreeLocal = indegree.ToDictionary();

            var queue = new PriorityQueue<char, char>(roots.Select(x => (x, x)));
            var workersQueue = new PriorityQueue<Worker, int>(workersCount);

            var currentTime = 0;
            while (queue.Count > 0 || workersQueue.Count > 0) {
                if (workersQueue.TryPeek(out _, out var minTime))
                    currentTime = minTime;

                while (workersQueue.TryPeek(out _, out var finishTime) && finishTime <= currentTime) {
                    var nextSteps = graph[workersQueue.Dequeue().Step]
                        .Where(next => --indegreeLocal[next] == 0)
                        .Select(x => (x, x));
                    queue.EnqueueRange(nextSteps);
                }

                while (workersQueue.Count < workersCount && queue.TryDequeue(out var nextStep, out _)) {
                    var worker = new Worker(currentTime + TimeToFinish(nextStep), nextStep);
                    workersQueue.Enqueue(worker, worker.FinishTime);
                }
            }

            return currentTime;

        }

        int TimeToFinish(char step) => step - 'A' + 1 + timePerStep;
    }

    private record Worker(int FinishTime, char Step);
}
