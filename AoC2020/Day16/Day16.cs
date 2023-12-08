using System.Text.RegularExpressions;
using Range = Common.Range;

namespace AoC2020.Day16;

public class Day16 {
    public static void Solve(IEnumerable<string> input) {
        var inputArray = input as string[] ?? input.ToArray();
        var fieldDescriptions = inputArray
            .TakeWhile(s => !string.IsNullOrWhiteSpace(s))
            .Select(FieldDescription.Parse)
            .ToArray();

        var tickets = inputArray
            .SkipWhile(s => !string.IsNullOrWhiteSpace(s))
            .Where(s => !string.IsNullOrWhiteSpace(s) && char.IsDigit(s[0]))
            .Select(x => x.Split(',').Select(int.Parse).ToArray())
            .ToArray();

        tickets.SelectMany(fields => fields.Where(f => !fieldDescriptions.Any(r => r.IsValid(f))))
            .Sum().Dump("Part1: ");

        var validNearbyTickets = tickets
            .Where(ticket => ticket.All(field => fieldDescriptions.Any(r => r.IsValid(field))))
            .ToArray();

        var myTicket = tickets[0];
        var rangesMap = Decode(fieldDescriptions, validNearbyTickets);
        fieldDescriptions.Indices()
            .Where(rangeInd => fieldDescriptions[rangeInd].Name.StartsWith("departure"))
            .Select(rangeInd => rangesMap[rangeInd])
            .Select(fieldInd => (long)myTicket[fieldInd])
            .Product()
            .Dump("Part2: ");
    }

    private static Dictionary<int, int> Decode(FieldDescription[] fieldDescriptions, int[][] tickets) {
        var rangeToTicketField = new Dictionary<int, int>(fieldDescriptions.Length);
        var queue = new Queue<int>(fieldDescriptions.Length);
        var usedFields = new HashSet<int>();
        foreach (var index in fieldDescriptions.Indices()) queue.Enqueue(index);

        while (queue.TryDequeue(out var rangeIndex)) {
            var range = fieldDescriptions[rangeIndex];
            var matchingFields = Enumerable.Range(0, fieldDescriptions.Length)
                .Where(fieldInd => !usedFields.Contains(fieldInd))
                .Where(fieldInd => tickets.All(ticket => range.IsValid(ticket[fieldInd])))
                .ToArray();
            if (matchingFields.Length == 1) {
                rangeToTicketField.Add(rangeIndex, matchingFields[0]);
                usedFields.Add(matchingFields[0]);
            }
            else queue.Enqueue(rangeIndex);
        }

        return rangeToTicketField;
    }

    public record FieldDescription(string Name, Range<int>[] Ranges) {
        private static readonly Regex Regex = new(@"\d+\-\d+");

        public static FieldDescription Parse(string input) {
            var segments = input.Split(':', 2, StringSplitOptions.TrimEntries);
            var ranges = Regex.Matches(segments[1]).Select(s => ParseRange(s.Value)).ToArray();
            return new FieldDescription(segments[0], ranges);
        }

        private static Range<int> ParseRange(string str) {
            var edges = str.Split('-');
            return Range.FromStartAndEndInclusive(int.Parse(edges[0]), int.Parse(edges[1]));
        }

        public bool IsValid(int value) {
            return Ranges.Any(r => r.Contains(value));
        }
    }
}