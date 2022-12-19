using Common;

namespace Solution;

public class Day8
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.Select(s => s.Select(c => c - '0').ToArray()).ToArray();
        Part1(lines).Dump("Part1: ");
        Part2(lines).Dump("Part2: ");
    }

    private static int Part2(int[][] grid)
        {
            int maxScore = 0;
            for (int i = 1; i < grid.Length - 1; i++)
            {
                for (int j = 1; j < grid[i].Length - 1; j++)
                {
                    var element = grid[i][j];
                    var visibleLeft = 0;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        visibleLeft++;
                        if (element <= grid[i][k])
                            break;
                    }

                    if (visibleLeft == 0)
                        continue;

                    var visibleRight = 0;
                    for (int k = j + 1; k < grid[i].Length; k++)
                    {
                        visibleRight++;
                        if (element <= grid[i][k])
                            break;
                    }

                    if (visibleRight == 0)
                        continue;
                    var visibleTop = 0;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        visibleTop++;
                        if (element <= grid[k][j])
                            break;
                    }

                    if (visibleTop == 0)
                        continue;

                    var visibleBottom = 0;
                    for (int k = i + 1; k < grid.Length; k++)
                    {
                        visibleBottom++;
                        if (element <= grid[k][j])
                            break;
                    }

                    if (visibleBottom == 0)
                        continue;
                    var score = visibleRight * visibleBottom * visibleLeft * visibleTop;
                    maxScore = Math.Max(score, maxScore);
                }
            }

            return maxScore;
        }

    private static int Part1(int[][] grid)
        {
            int notVisible = 0;
            for (int i = 1; i < grid.Length - 1; i++)
            {
                for (int j = 1; j < grid[i].Length - 1; j++)
                {
                    var element = grid[i][j];
                    var isVisibleLeft = true;
                    for (int k = 0; k < j; k++)
                    {
                        if (grid[i][k] >= element)
                        {
                            isVisibleLeft = false;
                            break;
                        }
                    }

                    if (isVisibleLeft)
                        continue;

                    var isVisibleRight = true;
                    for (int k = j + 1; k < grid[i].Length; k++)
                    {
                        if (grid[i][k] >= element)
                        {
                            isVisibleRight = false;
                            break;
                        }
                    }

                    if (isVisibleRight)
                        continue;
                    var isVisibleTop = true;
                    for (int k = 0; k < i; k++)
                    {
                        if (grid[k][j] >= element)
                        {
                            isVisibleTop = false;
                            break;
                        }
                    }

                    if (isVisibleTop)
                        continue;

                    var isVisibleBottom = true;
                    for (int k = i + 1; k < grid.Length; k++)
                    {
                        if (grid[k][j] >= element)
                        {
                            isVisibleBottom = false;
                            break;
                        }
                    }

                    if (isVisibleBottom)
                        continue;
                    notVisible++;
                }
            }

            return grid.Length * grid[0].Length - notVisible;
        }
}