namespace AoC2018.Day14;

public static partial class Day14 {
    public static void Solve(IEnumerable<string> input) {
        var inputValue = input.Single();

        Simulate()
            .Skip(inputValue.ToInt())
            .Take(10)
            .StringJoin(string.Empty)
            .Part1();

        var digits = inputValue.Select(x => x - '0').ToArray();
        Simulate().IndexOf(digits).Part2();

        IEnumerable<int> Simulate() {
            var recipes = new List<int>([3, 7]);
            int elf1 = 0, elf2 = 1;
            yield return 3;
            yield return 7;

            while (true) {
                var sum = recipes[elf1] + recipes[elf2];
                if (sum >= 10) {
                    yield return Add(1);
                    yield return Add(sum - 10);
                }
                else yield return Add(sum);

                elf1 = (elf1 + recipes[elf1] + 1) % recipes.Count;
                elf2 = (elf2 + recipes[elf2] + 1) % recipes.Count;
            }

            int Add(int value) {
                recipes.Add(value);
                return value;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
