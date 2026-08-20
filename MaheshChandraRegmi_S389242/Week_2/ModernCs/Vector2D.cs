namespace ModernCs;

/// <summary>
/// Immutable 2D vector using a readonly record struct (value type + value equality).
/// Demonstrates: primary constructor, init-only props, deconstruction, operators.
/// </summary>
public readonly record struct Vector2D(double X, double Y)
{
    // Expression-bodied members
    public double Magnitude => Math.Sqrt(X * X + Y * Y);
    public double MagnitudeSquared => X * X + Y * Y;

    public Vector2D Normalize()
    {
        var mag = Magnitude;
        // Pattern matching with relational patterns
        return mag switch
        {
            0 => throw new InvalidOperationException("Cannot normalize the zero vector."),
            _ => new Vector2D(X / mag, Y / mag)
        };
    }

    // Operator overloading
    public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2D operator *(Vector2D v, double scalar) => new(v.X * scalar, v.Y * scalar);
    public static Vector2D operator *(double scalar, Vector2D v) => v * scalar;
    public static Vector2D operator -(Vector2D v) => new(-v.X, -v.Y);

    public double Dot(Vector2D other) => X * other.X + Y * other.Y;

    // Custom string representation
    public override string ToString() => $"({X:F2}, {Y:F2})";

    // Collection expression conversion helper (from array / span of 2 numbers)
    public static Vector2D From(params double[] components) => components switch
    {
        [var x, var y] => new(x, y),
        _ => throw new ArgumentException("Exactly two components required.", nameof(components))
    };
}
