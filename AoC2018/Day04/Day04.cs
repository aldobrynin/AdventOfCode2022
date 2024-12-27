using Range = Common.Range;

namespace AoC2018.Day04;

public static partial class Day04 {
    public abstract record LogEntry(DateTime Timestamp) {
        public record GuardChange(DateTime Timestamp, int GuardId) : LogEntry(Timestamp);

        public record FallsAsleep(DateTime Timestamp) : LogEntry(Timestamp);

        public record WakesUp(DateTime Timestamp) : LogEntry(Timestamp);

        public static LogEntry Parse(string input) {
            var parts = input.Split([' ', '[', ']', '#'], StringSplitOptions.RemoveEmptyEntries);
            var timestamp = DateTime.Parse(parts[0] + ' ' + parts[1]);
            return parts[2] switch {
                "Guard" => new GuardChange(timestamp, int.Parse(parts[3])),
                "falls" => new FallsAsleep(timestamp),
                _ => new WakesUp(timestamp)
            };
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var sleepRanges = input.Select(LogEntry.Parse)
            .OrderBy(x => x.Timestamp)
            .PartitionBy(x => x is LogEntry.GuardChange)
            .Where(x => x.Count > 1)
            .GroupBy(
                x => ((LogEntry.GuardChange)x[0]).GuardId,
                (guardId, entries) =>
                (
                    GuardId: guardId,
                    SleepMinutes: entries
                        .Flatten()
                        .Where(x => x is not LogEntry.GuardChange)
                        .Chunk(2)
                        .SelectMany(e => Range.FromStartAndEnd(e[0].Timestamp.Minute, e[1].Timestamp.Minute))
                        .ToArray()
                )
            )
            .Select(x =>
                (
                    x.GuardId,
                    TotalSleep: x.SleepMinutes.Length,
                    MostFrequentSleepMinute: x.SleepMinutes.CountFrequency().MaxBy(kv => kv.Value)
                )
            )
            .ToArray();

        sleepRanges
            .MaxBy(x => x.TotalSleep)
            .Apply(x => x.GuardId * x.MostFrequentSleepMinute.Key)
            .Part1();

        sleepRanges
            .MaxBy(x => x.MostFrequentSleepMinute.Value)
            .Apply(x => x.GuardId * x.MostFrequentSleepMinute.Key)
            .Part2();
    }
}
