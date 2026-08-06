using CleanApp.Domain.TodoItems;

namespace CleanApp.Domain.Tests.TodoItems;

public class PriorityLevelTests
{
    [Theory]
    [InlineData(0, "None")]
    [InlineData(1, "Low")]
    [InlineData(2, "Medium")]
    [InlineData(3, "High")]
    public void FromValue_WithValidValue_ReturnsMatchingLevel(int value, string expectedName)
    {
        var result = PriorityLevel.FromValue(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedName, result.Value.Name);
    }

    [Fact]
    public void FromValue_WithInvalidValue_Fails()
    {
        var result = PriorityLevel.FromValue(99);

        Assert.True(result.IsFailure);
        Assert.Equal(TodoItemErrors.InvalidPriority, result.Error);
    }
}
