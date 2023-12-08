namespace Common;

public static class MathHelpers {
    public static long Lcm(long a, long b) => a * b / Gcd(a, b);
    public static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);
}