using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.Tests.TodoLists;

public class TodoListTitleTests
{
    [Fact]
    public void Create_WithValidTitle_Succeeds()
    {
        var result = TodoListTitle.Create("Groceries");

        Assert.True(result.IsSuccess);
        Assert.Equal("Groceries", result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyTitle_Fails(string? title)
    {
        var result = TodoListTitle.Create(title);

        Assert.True(result.IsFailure);
        Assert.Equal(TodoListErrors.TitleEmpty, result.Error);
    }

    [Fact]
    public void Create_WithTitleTooLong_Fails()
    {
        var title = new string('a', TodoListTitle.MaxLength + 1);

        var result = TodoListTitle.Create(title);

        Assert.True(result.IsFailure);
        Assert.Equal(TodoListErrors.TitleTooLong, result.Error);
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        var result = TodoListTitle.Create("  Groceries  ");

        Assert.Equal("Groceries", result.Value.Value);
    }

    [Fact]
    public void TwoTitles_WithSameValue_AreEqual()
    {
        var a = TodoListTitle.Create("Groceries").Value;
        var b = TodoListTitle.Create("Groceries").Value;

        Assert.Equal(a, b);
        Assert.True(a == b);
    }
}
