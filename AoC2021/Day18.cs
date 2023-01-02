using Common;

namespace AoC2021;

public class Day18
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input.Select(SnailfishNumber.Parse).ToArray();

        array.Skip(1)
            .Aggregate(array[0].Clone(), (sum, cur) => sum.Add(cur.Clone()).Reduce())
            .Magnitude().Dump("Part1: ");
        
        array.Indices().SelectMany(x => array.Indices().Select(y => (x, y)))
            .Where(tuple => tuple.y != tuple.x)
            .Select(tuple => array[tuple.x].Clone().Add(array[tuple.y].Clone()).Reduce())
            .Max(x => x.Magnitude())
            .Dump("Part2: ");
    }
    
    private class SnailfishNumberParsingException : Exception
    {
        public SnailfishNumberParsingException(string? message) : base(message)
        {
        }

        public static void ThrowIfNotExpectedSymbol(char actual, char expected)
        {
            if (actual != expected)
                throw new SnailfishNumberParsingException($"Unexpected symbol: got '{actual}', expected '{expected}'");
        }
    }

    private abstract class SnailfishNumber
    {
        public SnailfishPair? Parent { get; set; }
        public SnailfishNumber Reduce(int maxLevel = 4)
        {
            while (true)
            {
                var pairDepth = FindDeepestPair();
                if (pairDepth?.Depth >= maxLevel)
                {
                    pairDepth.Value.Pair.Explode();
                    continue;
                }

                var valueToSplit = FindLeftMostGreaterThan(10);
                if (valueToSplit != null)
                {
                    valueToSplit.Split();
                    continue;
                }
                
                return this;
            }
        }
        
        public static SnailfishNumber Parse(string source)
        {
            var offset = 0;
            return Parse(source, ref offset);
        }
        
        private static SnailfishNumber Parse(string source, ref int offset)
        {
            if (char.IsDigit(source[offset]))
                return new SnailfishValue(source[offset++] - '0');
        
            SnailfishNumberParsingException.ThrowIfNotExpectedSymbol(source[offset++], '[');
        
            var left = Parse(source, ref offset);
        
            SnailfishNumberParsingException.ThrowIfNotExpectedSymbol(source[offset++], ',');

            var right = Parse(source, ref offset);
        
            SnailfishNumberParsingException.ThrowIfNotExpectedSymbol(source[offset++], ']');
            return new SnailfishPair(left, right);
        }

        public SnailfishNumber Add(SnailfishNumber other)
        {
            if (Parent != null) throw new Exception("WTF?");
            if (other.Parent != null) throw new Exception("WTF??");
            var pair = new SnailfishPair(this, other);
            Parent = pair;
            other.Parent = pair;
            return pair;
        }

        public SnailfishValue? FindLeftMostLeaf()
        {
            if (this is SnailfishValue v) return v;
            if (this is not SnailfishPair p) throw new Exception("WTF");
            var cur = p.Left;
            while (true)
            {
                if (cur is SnailfishPair pair) cur = pair.Left;
                else return (SnailfishValue?)cur;
            }
        }

        public SnailfishValue? FindRightMostLeaf()
        {
            if (this is SnailfishValue v) return v;
            if (this is not SnailfishPair p) throw new Exception("WTF");
            var cur = p.Right;
            while (true)
            {
                if (cur is SnailfishPair pair) cur = pair.Right;
                else return (SnailfishValue?)cur;
            }
        }

        private (int Depth, SnailfishPair Pair)? FindDeepestPair()
        {
            if (this is SnailfishValue) return null;
            var pair = (SnailfishPair)this;
            if (pair is { Left: SnailfishValue, Right: SnailfishValue })
                return (0, pair);

            var left = pair.Left.FindDeepestPair();
            var right = pair.Right.FindDeepestPair();
            return (left, right) switch
            {
                (null, null) => null,
                (null, _) => (right.Value.Depth + 1, right.Value.Pair),
                (_, null) => (left.Value.Depth + 1, left.Value.Pair),
                _ => left.Value.Depth >= right.Value.Depth
                    ? (left.Value.Depth + 1, left.Value.Pair)
                    : (right.Value.Depth + 1, right.Value.Pair)
            };
        }

        private SnailfishValue? FindLeftMostGreaterThan(int minValue)
        {
            return this switch
            {
                SnailfishValue value => value.Value >= minValue ? value : null,
                SnailfishPair pair => pair.Left.FindLeftMostGreaterThan(minValue) ??
                                      pair.Right.FindLeftMostGreaterThan(minValue),
                _ => throw new Exception("WTF???"),
            };
        }
        
        public long Magnitude()
        {
            return this switch
            {
                SnailfishPair pair => pair.Left.Magnitude() * 3 + pair.Right.Magnitude() * 2,
                SnailfishValue value => value.Value,
                _ => throw new Exception("WTF???"),
            };
        }

        public SnailfishNumber Clone()
        {
            return this switch
            {
                SnailfishValue value => new SnailfishValue(value.Value),
                SnailfishPair pair => new SnailfishPair(pair.Left.Clone(), pair.Right.Clone()),
                _ => throw new Exception("WTF"),
            };
        }
    }
    
    private class SnailfishPair : SnailfishNumber
    {
        public SnailfishPair(SnailfishNumber left, SnailfishNumber right)
        {
            left.Parent = this;
            right.Parent = this;

            Left = left;
            Right = right;
        }

        public SnailfishNumber Left { get; set; }
        public SnailfishNumber Right { get; set; }

        public override string ToString() => $"[{Left},{Right}]";

        public void Explode()
        {
            var leftValue = ((SnailfishValue)Left).Value;
            var rightValue = ((SnailfishValue)Right).Value;
            for (var cur = this; cur.Parent != null; cur = cur.Parent)
            {
                if (cur.Parent.Right != cur) continue;
                var number = cur.Parent.Left.FindRightMostLeaf();
                if (number != null) number.Value += leftValue;
                break;
            }

            for (var cur = this; cur.Parent != null; cur = cur.Parent)
            {
                if (cur.Parent.Left != cur) continue;
                var number = cur.Parent.Right.FindLeftMostLeaf();
                if (number != null) number.Value += rightValue;
                break;
            }

            if (Parent?.Left == this)
                Parent.Left = new SnailfishValue(0) { Parent = Parent };
            if (Parent?.Right == this)
                Parent.Right = new SnailfishValue(0) { Parent = Parent };
        }
    }
    
    private class SnailfishValue :SnailfishNumber
    {
        public SnailfishValue(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
        public override string ToString() => Value.ToString();

        public void Split()
        {
            var newPair = new SnailfishPair(
                new SnailfishValue(Value / 2),
                new SnailfishValue(Value - Value / 2))
            {
                Parent = Parent,
            };

            if (Parent?.Left == this) Parent.Left = newPair;
            if (Parent?.Right == this) Parent.Right = newPair;
        }
    }
}