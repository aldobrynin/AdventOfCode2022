using System.Text.RegularExpressions;

namespace AoC2018.Day24;

public static partial class Day24 {
    public record ArmyGroup(
        string Army,
        int Index,
        int Damage,
        int HitPoints,
        int Initiative,
        string DamageType,
        string[] Weaknesses,
        string[] Immunities) {
        public static ArmyGroup Parse(string army, string line, int index) {
            var regex = new Regex(
                @"(?<Units>\d+) units each with (?<HitPoints>\d+) hit points (\((?<Modifiers>.*)\) )?with an attack that does (?<Damage>\d+) (?<DamageType>\w+) damage at initiative (?<Initiative>\d+)");
            var match = regex.Match(line);
            if (!match.Success) throw new FormatException($"Invalid input: {line}");

            var units = match.Groups["Units"].Value.ToInt();
            var hitPoints = match.Groups["HitPoints"].Value.ToInt();
            var initiative = match.Groups["Initiative"].Value.ToInt();
            var damage = match.Groups["Damage"].Value.ToInt();
            var damageType = match.Groups["DamageType"].Value;
            var modifiers = match.Groups["Modifiers"].Value
                .Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.Split(" to ", StringSplitOptions.TrimEntries))
                .ToDictionary(x => x[0], x => x[1].Split([',', ' '], StringSplitOptions.RemoveEmptyEntries));

            var weaknesses = modifiers.GetValueOrDefault("weak", []);
            var immunities = modifiers.GetValueOrDefault("immune", []);

            return new ArmyGroup(army, index + 1, damage, hitPoints, initiative, damageType, weaknesses, immunities) {
                Units = units
            };
        }

        public int Units { get; set; }

        public int EffectivePower => Units * Damage;

        public bool IsImmuneSystem => Army == "Immune System";

        public bool IsAlive => Units > 0;

        public int DamageTo(ArmyGroup other) {
            if (other.Immunities.Contains(DamageType)) return 0;
            if (other.Weaknesses.Contains(DamageType)) return EffectivePower * 2;
            return EffectivePower;
        }

        public virtual bool Equals(ArmyGroup? other) {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Army == other.Army && Index == other.Index;
        }

        public override int GetHashCode() => HashCode.Combine(Army, Index);

        public ArmyGroup WithBoost(int boost) => this with { Damage = Damage + boost };
    }

    public static void Solve(IEnumerable<string> input) {
        var armies = input
            .SplitBy(string.IsNullOrWhiteSpace)
            .SelectMany(lines =>
                lines.Skip(1).Select((x, i) => ArmyGroup.Parse(lines[0].TrimEnd(':'), x, i)))
            .ToArray();

        Fight(armies).Sum(x => x.Units).Part1();

        var minBoost = SearchHelpers.BinarySearchLowerBound(1,
            100000,
            boost => Fight(armies, boost).All(r => r.IsImmuneSystem)
        );
        Fight(armies, minBoost).Sum(x => x.Units).Part2();
    }

    private static ArmyGroup[] Fight(ArmyGroup[] armies, int boost = 0) {
        armies = armies.Select(a => a.WithBoost(a.IsImmuneSystem ? boost : 0)).ToArray();

        while (armies.Where(x => x.IsAlive).Select(x => x.Army).Distinct().Count() ==2) {
            var targets = new Dictionary<ArmyGroup, ArmyGroup>();
            var groupsToSelectTarget = armies
                .Where(g => g.IsAlive)
                .OrderByDescending(x => x.EffectivePower)
                .ThenByDescending(x => x.Initiative);
            foreach (var attacker in groupsToSelectTarget) {
                var target = armies
                    .Where(x => x.Army != attacker.Army && x.IsAlive && !targets.ContainsValue(x))
                    .OrderByDescending(x => attacker.DamageTo(x))
                    .ThenByDescending(x => x.EffectivePower)
                    .ThenByDescending(x => x.Initiative)
                    .FirstOrDefault();

                if (target is not null && attacker.DamageTo(target) > 0)
                    targets[attacker] = target;
            }

            var totalKilled = 0;
            foreach (var (attacker, target) in targets.OrderByDescending(x => x.Key.Initiative)) {
                var unitsKilled = Math.Min(target.Units, attacker.DamageTo(target) / target.HitPoints);
                target.Units -= unitsKilled;
                totalKilled += unitsKilled;
            }

            if (totalKilled == 0) break;
        }

        return armies.Where(x => x.Units > 0).ToArray();
    }
}
