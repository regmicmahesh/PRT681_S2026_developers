namespace ModernCs;

/// <summary>
/// Supported distance measures between two points/vectors.
/// </summary>
public enum DistanceMetric
{
    Euclidean,  // √((x₂−x₁)² + (y₂−y₁)²)  — "as the crow flies"
    Manhattan,  // |x₂−x₁| + |y₂−y₁|         — grid / city-block distance
    Chebyshev   // max(|x₂−x₁|, |y₂−y₁|)     — chess king move distance
}
