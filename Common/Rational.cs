using System.Globalization;
using System.Numerics;

namespace Common;

public readonly struct Rational : INumber<Rational> {
    public Rational(long numerator, long denominator = 1)
        : this(BigInteger.CreateChecked(numerator), BigInteger.CreateChecked(denominator)) {
    }

    public Rational(BigInteger numerator, BigInteger denominator) {
        if (denominator == 0) throw new ArgumentException("Denominator cannot be zero", nameof(denominator));
        if (numerator == 0) {
            Numerator = BigInteger.Zero;
            Denominator = BigInteger.One;
        }
        else {
            var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
            Numerator = numerator / gcd;
            Denominator = denominator / gcd;
        }
    }

    public BigInteger Numerator { get; }
    public BigInteger Denominator { get; }

    public static Rational operator +(Rational a, Rational b) {
        return new Rational(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
    }

    public double ToDouble() {
        return (double)(Numerator / Denominator);
    }

    public static Rational operator -(Rational a, Rational b) {
        return new Rational(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
    }

    public static Rational operator *(Rational a, Rational b) {
        return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static Rational operator /(Rational a, Rational b) {
        return new Rational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }

    public int CompareTo(object? obj) {
        return obj switch {
            null => 1,
            Rational other => CompareTo(other),
            _ => throw new ArgumentException($"Object must be of type {nameof(Rational)}")
        };
    }

    public int CompareTo(Rational other) {
        return (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);
    }

    public bool Equals(Rational other) {
        if (Numerator.IsZero && other.Numerator.IsZero) return true;
        return Numerator == other.Numerator && Denominator == other.Denominator;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) {
        return $"{Numerator}/{Denominator}";
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) {
        var res = $"{Numerator}/{Denominator}".AsSpan();
        res.CopyTo(destination);
        charsWritten = res.Length;
        return true;
    }

    public static Rational Parse(string s, IFormatProvider? provider) {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj) {
        return obj is Rational other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Numerator, Denominator);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Rational result) {
        throw new NotImplementedException();
    }

    public static Rational Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rational result) {
        throw new NotImplementedException();
    }

    public static Rational AdditiveIdentity { get; } = new(0);

    public static bool operator ==(Rational left, Rational right) {
        return left.Equals(right);
    }

    public static bool operator !=(Rational left, Rational right) {
        return !(left == right);
    }

    public static bool operator >(Rational left, Rational right) {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Rational left, Rational right) {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <(Rational left, Rational right) {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Rational left, Rational right) {
        return left.CompareTo(right) <= 0;
    }

    public static Rational operator --(Rational value) {
        return new Rational(value.Numerator - value.Denominator, value.Denominator);
    }

    public static Rational operator ++(Rational value) {
        return new Rational(value.Numerator + value.Denominator, value.Denominator);
    }

    public static Rational operator %(Rational left, Rational right) {
        return left - right * (left / right);
    }

    public static Rational MultiplicativeIdentity { get; } = new(1);

    public static Rational operator -(Rational value) {
        return new Rational(-value.Numerator, value.Denominator);
    }

    public static Rational operator +(Rational value) {
        return value;
    }

    public static Rational Abs(Rational value) {
        return value.Numerator.Sign == 1 ? value : -value;
    }

    public static bool IsCanonical(Rational value) {
        return value.Denominator == 1 || value.Numerator.Sign == 0;
    }

    public static bool IsComplexNumber(Rational value) {
        return value.Denominator != 1 && value.Numerator.Sign != 0;
    }

    public static bool IsEvenInteger(Rational value) {
        return value.Numerator.IsEven;
    }

    public static bool IsFinite(Rational value) {
        return !value.Denominator.IsZero;
    }

    public static bool IsImaginaryNumber(Rational value) {
        throw new NotImplementedException();
    }

    public static bool IsInfinity(Rational value) {
        return value.Denominator.IsZero;
    }

    public static bool IsInteger(Rational value) {
        return value.Denominator == 1;
    }

    public static bool IsNaN(Rational value) {
        return value.Denominator.IsZero && value.Numerator.IsZero;
    }

    public static bool IsNegative(Rational value) {
        return value.Numerator.Sign == -1;
    }

    public static bool IsNegativeInfinity(Rational value) {
        return IsInfinity(value) && IsNegative(value);
    }

    public static bool IsNormal(Rational value) {
        throw new NotImplementedException();
    }

    public static bool IsOddInteger(Rational value) {
        throw new NotImplementedException();
    }

    public static bool IsPositive(Rational value) {
        return value.Numerator.Sign == 1;
    }

    public static bool IsPositiveInfinity(Rational value) {
        throw new NotImplementedException();
    }

    public static bool IsRealNumber(Rational value) {
        throw new NotImplementedException();
    }

    public static bool IsSubnormal(Rational value) {
        throw new NotImplementedException();
    }

    public static bool IsZero(Rational value) {
        return value.Numerator.IsZero;
    }

    public static Rational MaxMagnitude(Rational x, Rational y) {
        return x.CompareTo(y) > 0 ? x : y;
    }

    public static Rational MaxMagnitudeNumber(Rational x, Rational y) {
        return x.CompareTo(y) > 0 ? x : y;
    }

    public static Rational MinMagnitude(Rational x, Rational y) {
        return x.CompareTo(y) < 0 ? x : y;
    }

    public static Rational MinMagnitudeNumber(Rational x, Rational y) {
        return x.CompareTo(y) < 0 ? x : y;
    }

    public static Rational Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
        throw new NotImplementedException();
    }

    public static Rational Parse(string s, NumberStyles style, IFormatProvider? provider) {
        throw new NotImplementedException();
    }

    public static bool TryConvertFromChecked<TOther>(TOther value, out Rational result)
        where TOther : INumberBase<TOther> {
        if (TOther.TryConvertToChecked(value, out BigInteger numerator)) {
            result = new Rational(numerator, BigInteger.One);
            return true;
        }

        result = Zero;
        return false;
    }

    public static bool TryConvertFromSaturating<TOther>(TOther value, out Rational result)
        where TOther : INumberBase<TOther> {
        if (TOther.TryConvertToSaturating(value, out BigInteger numerator)) {
            result = new Rational(numerator, BigInteger.One);
            return true;
        }

        result = Zero;
        return false;
    }

    public static bool TryConvertFromTruncating<TOther>(TOther value, out Rational result)
        where TOther : INumberBase<TOther> {
        if (TOther.TryConvertToTruncating(value, out BigInteger numerator)) {
            result = new Rational(numerator, BigInteger.One);
            return true;
        }

        result = Zero;
        return false;
    }

    public static bool TryConvertToChecked<TOther>(Rational value, out TOther result)
        where TOther : INumberBase<TOther> {
        throw new NotImplementedException();
    }

    public static bool TryConvertToSaturating<TOther>(Rational value, out TOther result)
        where TOther : INumberBase<TOther> {
        throw new NotImplementedException();
    }

    public static bool TryConvertToTruncating<TOther>(Rational value, out TOther result)
        where TOther : INumberBase<TOther> {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out Rational result) {
        throw new NotImplementedException();
    }

    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Rational result) {
        throw new NotImplementedException();
    }

    public static Rational One { get; } = new(1);
    public static int Radix => 10;
    public static Rational Zero { get; } = new(0);
}