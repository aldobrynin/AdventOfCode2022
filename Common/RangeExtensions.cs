namespace Common;

public static class RangeExtensions {
    public static IEnumerable<Range> Merge(this IEnumerable<Range> ranges) {
        Range? current = null;
        foreach (var range in ranges.OrderBy(x => x.From)) {
            if (current == null)
                current = range;
            else if (current.To >= range.From)
                current = current with { To = Math.Max(current.To, range.To) };
            else {
                yield return current;
                current = range;
            }
        }

        if (current is not null) yield return current;
    }
}