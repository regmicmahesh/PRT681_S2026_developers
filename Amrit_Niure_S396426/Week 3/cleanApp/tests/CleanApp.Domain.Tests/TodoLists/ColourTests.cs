using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.Tests.TodoLists;

public class ColourTests
{
    [Theory]
    [InlineData("#FFAA00")]
    [InlineData("#FFF")]
    [InlineData("#000000")]
    public void Create_WithValidHex_Succeeds(string hex)
    {
        var result = Colour.Create(hex);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-a-colour")]
    [InlineData("#GGGGGG")]
    [InlineData("FFAA00")]
    public void Create_WithInvalidHex_Fails(string? hex)
    {
        var result = Colour.Create(hex);

        Assert.True(result.IsFailure);
        Assert.Equal(TodoListErrors.InvalidColour, result.Error);
    }

    [Fact]
    public void Create_NormalizesToUppercase()
    {
        var result = Colour.Create("#ffaa00");

        Assert.Equal("#FFAA00", result.Value.Code);
    }
}
