using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoItems.Events;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.Tests.TodoItems;

public class TodoItemTests
{
    private static readonly UserId OwnerId = new(Guid.NewGuid());
    private static readonly TodoListId ListId = TodoListId.New();

    [Fact]
    public void Create_WithValidData_RaisesTodoItemCreatedEvent()
    {
        var result = TodoItem.Create(OwnerId, ListId, "Buy milk", priority: 2);

        Assert.True(result.IsSuccess);
        var item = result.Value;
        Assert.Equal(OwnerId, item.OwnerId);
        var domainEvent = Assert.Single(item.DomainEvents);
        Assert.IsType<TodoItemCreatedEvent>(domainEvent);
        Assert.False(item.IsDone);
    }

    [Fact]
    public void Create_WithReminderInPast_Fails()
    {
        var result = TodoItem.Create(OwnerId, ListId, "Buy milk", reminderUtc: DateTime.UtcNow.AddMinutes(-1));

        Assert.True(result.IsFailure);
        Assert.Equal(TodoItemErrors.ReminderInPast, result.Error);
    }

    [Fact]
    public void Create_WithInvalidPriority_Fails()
    {
        var result = TodoItem.Create(OwnerId, ListId, "Buy milk", priority: 99);

        Assert.True(result.IsFailure);
        Assert.Equal(TodoItemErrors.InvalidPriority, result.Error);
    }

    [Fact]
    public void Complete_WhenNotDone_MarksDoneAndRaisesEvent()
    {
        var item = TodoItem.Create(OwnerId, ListId, "Buy milk").Value;
        item.ClearDomainEvents();

        var result = item.Complete();

        Assert.True(result.IsSuccess);
        Assert.True(item.IsDone);
        Assert.NotNull(item.CompletedOnUtc);
        var domainEvent = Assert.Single(item.DomainEvents);
        Assert.IsType<TodoItemCompletedEvent>(domainEvent);
    }

    [Fact]
    public void Complete_WhenAlreadyDone_Fails()
    {
        var item = TodoItem.Create(OwnerId, ListId, "Buy milk").Value;
        item.Complete();

        var result = item.Complete();

        Assert.True(result.IsFailure);
        Assert.Equal(TodoItemErrors.AlreadyCompleted, result.Error);
    }

    [Fact]
    public void Reopen_WhenDone_MarksNotDoneAndRaisesEvent()
    {
        var item = TodoItem.Create(OwnerId, ListId, "Buy milk").Value;
        item.Complete();
        item.ClearDomainEvents();

        var result = item.Reopen();

        Assert.True(result.IsSuccess);
        Assert.False(item.IsDone);
        Assert.Null(item.CompletedOnUtc);
    }

    [Fact]
    public void Reopen_WhenNotDone_Fails()
    {
        var item = TodoItem.Create(OwnerId, ListId, "Buy milk").Value;

        var result = item.Reopen();

        Assert.True(result.IsFailure);
        Assert.Equal(TodoItemErrors.NotCompleted, result.Error);
    }

    [Fact]
    public void UpdateDetails_WithValidData_UpdatesFields()
    {
        var item = TodoItem.Create(OwnerId, ListId, "Buy milk").Value;

        var result = item.UpdateDetails("Buy oat milk", 3, "2% preferred", null);

        Assert.True(result.IsSuccess);
        Assert.Equal("Buy oat milk", item.Title.Value);
        Assert.Equal(3, item.Priority.Value);
        Assert.Equal("2% preferred", item.Note);
    }
}
