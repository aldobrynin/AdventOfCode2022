namespace AoC2024.Day09;

public static partial class Day09 {
    public record MemoryBlock(int Size, int? FileId);

    public static void Solve(IEnumerable<string> input) {
        var blocks = input.Single()
            .WithIndex()
            .Select(x => new MemoryBlock(x.Element - '0', x.Index % 2 == 0 ? x.Index / 2 : null))
            .ToArray();
        Part1(blocks).Part1();
        Part2(blocks).Part2();
    }

    private static long Part1(MemoryBlock[] memory) {
        var blocks = memory.SelectMany(x => Enumerable.Repeat(x.FileId ?? -1, x.Size)).ToArray();
        int left = Array.IndexOf(blocks, -1), right = blocks.Length;
        while (left < --right) {
            if (blocks[right] == -1) continue;
            (blocks[left], blocks[right]) = (blocks[right], blocks[left]);
            left = Array.IndexOf(blocks, -1, left + 1);
        }

        return CalcCheckSum(blocks);
    }

    private static long Part2(MemoryBlock[] memory) {
        var map = memory
            .Where(x => x.FileId.HasValue)
            .ToDictionary(x => x.FileId!.Value, x => x.Size);
        var priorityQueues = Enumerable.Range(0, 10).Select(_ => new PriorityQueue<int, int>()).ToArray();
        var index = 0;
        foreach (var element in memory) {
            if (element.FileId is null)
                priorityQueues[element.Size].Enqueue(element.Size, index);
            index += element.Size;
        }

        var blocks = memory
            .SelectMany(x => Enumerable.Repeat(x.FileId ?? -1, x.Size))
            .ToArray();

        var right = blocks.Length - 1;

        while (right > 0) {
            var endOfFile = FindRightmostEndOfFile(right);
            var fileId = blocks[endOfFile];
            var size = map[fileId];
            var startOfFile = endOfFile - size + 1;
            var queue = priorityQueues
                .Skip(size)
                .MinBy(q => q.TryPeek(out _, out var ind) ? ind : int.MaxValue);
            if (queue is not null &&
                queue.TryDequeue(out var freeSpaseSize, out var freeSpaceStart) &&
                freeSpaceStart < startOfFile) {
                Array.Fill(blocks, fileId, freeSpaceStart, size);
                Array.Fill(blocks, -1, startOfFile, size);

                if (freeSpaseSize > size) {
                    freeSpaceStart += size;
                    freeSpaseSize -= size;
                    priorityQueues[freeSpaseSize].Enqueue(freeSpaseSize, freeSpaceStart);
                }
            }

            right = startOfFile - 1;
        }

        return CalcCheckSum(blocks);

        int FindRightmostEndOfFile(int ind) {
            for (var i = ind; i >= 0; i--) {
                if (blocks[i] != -1) return i;
            }

            return -1;
        }
    }

    private static long CalcCheckSum(int[] blocks) =>
        blocks
            .WithIndex()
            .Sum(x => 1L * Math.Max(x.Element, 0) * x.Index);
}
