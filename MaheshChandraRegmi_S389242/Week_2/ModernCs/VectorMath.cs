namespace ModernCs;

/// <summary>
/// Static helpers for distance calculations.
/// Demonstrates: switch expressions, pattern matching, LINQ, extension methods.
/// </summary>
public static class VectorMath
{
    public static double Distance(Vector2D a, Vector2D b, DistanceMetric metric = DistanceMetric.Euclidean)
    {
        var delta = a - b;

        // Switch expression on enum (modern alternative to switch statements)
        return metric switch
        {
            DistanceMetric.Euclidean => Math.Sqrt(delta.MagnitudeSquared),
            DistanceMetric.Manhattan => Math.Abs(delta.X) + Math.Abs(delta.Y),
            DistanceMetric.Chebyshev => Math.Max(Math.Abs(delta.X), Math.Abs(delta.Y)),
            _ => throw new ArgumentOutOfRangeException(nameof(metric), metric, "Unknown metric.")
        };
    }

    /// <summary>
    /// Pairwise distances from a reference point to many others (LINQ).
    /// </summary>
    public static IEnumerable<(Vector2D Point, double Distance)> DistancesFrom(
        Vector2D origin,
        IEnumerable<Vector2D> points,
        DistanceMetric metric = DistanceMetric.Euclidean)
    {
        return points.Select(p => (Point: p, Distance: Distance(origin, p, metric)));
    }

    /// <summary>
    /// Find the nearest neighbour to <paramref name="origin"/>.
    /// Returns null when the list is empty (list pattern + nullable).
    /// </summary>
    public static Vector2D? FindNearest(Vector2D origin, IReadOnlyList<Vector2D> points)
        => points is []
            ? null
            : points.MinBy(p => Distance(origin, p));
}

/// <summary>
/// Extension methods keep call sites readable: <c>a.DistanceTo(b)</c>.
/// </summary>
public static class Vector2DExtensions
{
    public static double DistanceTo(
        this Vector2D source,
        Vector2D target,
        DistanceMetric metric = DistanceMetric.Euclidean)
        => VectorMath.Distance(source, target, metric);

    public static bool IsNearlyEqual(this Vector2D a, Vector2D b, double epsilon = 1e-9)
        => a.DistanceTo(b) < epsilon;
}
