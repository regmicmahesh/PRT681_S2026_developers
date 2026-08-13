using System.Text.RegularExpressions;
using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoLists;

public sealed partial class Colour : ValueObject
{
    public static readonly Colour White = new("#FFFFFF");
    public static readonly Colour Red = new("#FF5733");
    public static readonly Colour Blue = new("#3366FF");
    public static readonly Colour Green = new("#33CC66");
    public static readonly Colour Yellow = new("#FFC300");

    private Colour(string code) => Code = code;

    public string Code { get; }

    [GeneratedRegex("^#(?:[0-9a-fA-F]{3}){1,2}$")]
    private static partial Regex HexPattern();

    public static Result<Colour> Create(string? code)
    {
        if (string.IsNullOrWhiteSpace(code) || !HexPattern().IsMatch(code))
            return Result.Failure<Colour>(TodoListErrors.InvalidColour);

        return Result.Success(new Colour(code.ToUpperInvariant()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Code;
}
