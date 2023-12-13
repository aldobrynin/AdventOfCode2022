using System.Numerics;

namespace Common;

public static class MathHelpers {
    public static T Lcm<T>(T a, T b) where T : INumber<T> => a * b / Gcd(a, b);
    public static T Gcd<T>(T a, T b) where T : INumber<T> => b == T.Zero ? a : Gcd(b, a % b);

    public static T Mod<T>(this T value, T divisor) where T : INumber<T> {
        var result = value % divisor;
        return T.IsNegative(result) ? result + divisor : result;
    }
}