using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using CleanApp.Domain.TodoLists.Events;

namespace CleanApp.Domain.Tests.TodoLists;

public class TodoListTests
{
    private static readonly UserId OwnerId = new(Guid.NewGuid());

    [Fact]
    public void Create_WithValidData_RaisesTodoListCreatedEvent()
    {
        var result = TodoList.Create(OwnerId, "Groceries", "#33CC66");

        Assert.True(result.IsSuccess);
        var list = result.Value;
        Assert.Equal(OwnerId, list.OwnerId);
        var domainEvent = Assert.Single(list.DomainEvents);
        var created = Assert.IsType<TodoListCreatedEvent>(domainEvent);
        Assert.Equal(list.Id, created.TodoListId);
        Assert.Equal("Groceries", created.Title);
    }

    [Fact]
    public void Create_WithoutColour_DefaultsToWhite()
    {
        var result = TodoList.Create(OwnerId, "Groceries");

        Assert.Equal(Colour.White, result.Value.Colour);
    }

    [Fact]
    public void Create_WithInvalidTitle_Fails()
    {
        var result = TodoList.Create(OwnerId, "");

        Assert.True(result.IsFailure);
        Assert.Equal(TodoListErrors.TitleEmpty, result.Error);
    }

    [Fact]
    public void Rename_WithValidTitle_UpdatesTitleAndRaisesEvent()
    {
        var list = TodoList.Create(OwnerId, "Groceries").Value;
        list.ClearDomainEvents();

        var result = list.Rename("Shopping");

        Assert.True(result.IsSuccess);
        Assert.Equal("Shopping", list.Title.Value);
        Assert.Single(list.DomainEvents);
    }

    [Fact]
    public void Rename_ToSameTitle_DoesNotRaiseEvent()
    {
        var list = TodoList.Create(OwnerId, "Groceries").Value;
        list.ClearDomainEvents();

        var result = list.Rename("Groceries");

        Assert.True(result.IsSuccess);
        Assert.Empty(list.DomainEvents);
    }

    [Fact]
    public void ChangeColour_WithInvalidHex_Fails()
    {
        var list = TodoList.Create(OwnerId, "Groceries").Value;

        var result = list.ChangeColour("not-a-colour");

        Assert.True(result.IsFailure);
        Assert.Equal(TodoListErrors.InvalidColour, result.Error);
    }
}
