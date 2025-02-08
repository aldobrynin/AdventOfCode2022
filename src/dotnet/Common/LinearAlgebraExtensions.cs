namespace Common;

public static class LinearAlgebraExtensions {
    /// <summary>
    ///     Solves linear equations system using Gaussian elimination
    /// </summary>
    /// <param name="matrix">
    ///     matrix of coefficients in form of augmented matrix:
    ///     <code>
    ///     a11 a12 a13 ... a1n b1
    ///     a21 a22 a23 ... a2n b2
    ///     ...
    ///     an1 an2 an3 ... ann bn
    ///     </code>
    /// </param>
    /// <returns>array of solutions</returns>
    public static Rational[] SolveGaussian(this Rational[][] matrix) {
        var m = matrix.Length;
        for (var k = 0; k < m; k++) {
            var pivotIndex = k.RangeUntil(m).MaxBy(j => Rational.Abs(matrix[j][k]));
            if (matrix[pivotIndex][k] == Rational.Zero) throw new InvalidOperationException("Matrix is singular");

            (matrix[pivotIndex], matrix[k]) = (matrix[k], matrix[pivotIndex]);

            for (var i = k + 1; i < m; i++) {
                var factor = matrix[i][k] / matrix[k][k];
                for (var j = k + 1; j <= m; j++) matrix[i][j] -= factor * matrix[k][j];
                matrix[i][k] = Rational.Zero;
            }
        }

        var result = new Rational[m];
        for (var i = m - 1; i >= 0; i--) {
            result[i] = matrix[i][m];
            for (var j = i + 1; j < m; j++) result[i] -= result[j] * matrix[i][j];
            result[i] /= matrix[i][i];
        }

        return result;
    }
}