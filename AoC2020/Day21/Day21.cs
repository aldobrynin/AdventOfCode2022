namespace AoC2020.Day21;

public partial class Day21 {
    public record struct Food(HashSet<string> Ingredients, HashSet<string> Allergens) {
        public static Food Parse(string line) {
            var parts = line.Split(" (contains ");
            return new Food(
                parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet(),
                parts[1].Split(new[] { ',', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries).ToHashSet());
        }

        public override string ToString() {
            return $"{Ingredients.StringJoin(" ")} (contains {Allergens.StringJoin()})";
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var foods = input.Select(Food.Parse).ToArray();

        var allergenToIngredients = foods
            .SelectMany(food => food.Allergens.Select(allergen => (Allergen: allergen, food.Ingredients)))
            .GroupBy(x => x.Allergen)
            .ToDictionary(x => x.Key, x => x.Select(s => s.Ingredients).IntersectAll());

        var ingredientsWithAllergen = allergenToIngredients.SelectMany(x => x.Value).ToHashSet();

        foods.Sum(food => food.Ingredients.Count(ingredient => !ingredientsWithAllergen.Contains(ingredient)))
            .Part1();

        FindBijection(allergenToIngredients)
            .OrderBy(x => x.Value)
            .Select(x => x.Key)
            .StringJoin()
            .Part2();
    }

    private static Dictionary<string, string> FindBijection(Dictionary<string, HashSet<string>> candidates) {
        var visited = new HashSet<string>();
        while (visited.Count != candidates.Count) {
            var current = candidates.First(x => x.Value.Count == 1 && visited.Add(x.Key));
            foreach (var (otherAllergen, otherIngredients) in candidates) {
                if (current.Key != otherAllergen)
                    otherIngredients.Remove(current.Value.Single());
            }
        }

        return candidates.ToDictionary(x => x.Value.Single(), x => x.Key);
    }
}