using CleanApp.Application.TodoItems.Queries;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Application.Tests.TodoItems.Queries;

public class GetTodoItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithIsDoneFilter_ReturnsOnlyMatchingItems()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        var list = TodoList.Create(ownerId, "Groceries").Value;
        var pending = TodoItem.Create(ownerId, list.Id, "Buy milk").Value;
        var done = TodoItem.Create(ownerId, list.Id, "Buy eggs").Value;
        done.Complete();

        context.TodoLists.Add(list);
        context.TodoItems.AddRange(pending, done);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(new GetTodoItemsQuery(list.Id.Value, IsDone: true), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("Buy eggs", response.Title);
    }

    [Fact]
    public async Task Handle_WithPriorityFilter_ReturnsOnlyMatchingItems()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        var list = TodoList.Create(ownerId, "Groceries").Value;
        var low = TodoItem.Create(ownerId, list.Id, "Buy milk", priority: 1).Value;
        var high = TodoItem.Create(ownerId, list.Id, "Call plumber", priority: 3).Value;

        context.TodoLists.Add(list);
        context.TodoItems.AddRange(low, high);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(new GetTodoItemsQuery(list.Id.Value, Priority: 3), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("Call plumber", response.Title);
    }

    [Fact]
    public async Task Handle_WithoutFilter_ReturnsAllItemsForList()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        var list = TodoList.Create(ownerId, "Groceries").Value;
        var otherList = TodoList.Create(ownerId, "Chores").Value;
        var item1 = TodoItem.Create(ownerId, list.Id, "Buy milk").Value;
        var item2 = TodoItem.Create(ownerId, otherList.Id, "Vacuum").Value;

        context.TodoLists.AddRange(list, otherList);
        context.TodoItems.AddRange(item1, item2);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(new GetTodoItemsQuery(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("Buy milk", response.Title);
    }

    [Fact]
    public async Task Handle_WithPaging_ReturnsRequestedPageAndTotalCount()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());
        var list = TodoList.Create(ownerId, "Groceries").Value;

        context.TodoLists.Add(list);
        context.TodoItems.AddRange(
            TodoItem.Create(ownerId, list.Id, "Item A").Value,
            TodoItem.Create(ownerId, list.Id, "Item B").Value,
            TodoItem.Create(ownerId, list.Id, "Item C").Value);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(
            new GetTodoItemsQuery(list.Id.Value, Page: 2, PageSize: 2, SortBy: TodoItemSortBy.Title),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.TotalCount);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("Item C", response.Title);
    }

    [Fact]
    public async Task Handle_DoesNotReturnItemsBelongingToAnotherUser()
    {
        await using var context = TestDbContextFactory.Create();

        var otherOwnerId = new UserId(Guid.NewGuid());
        var list = TodoList.Create(otherOwnerId, "Groceries").Value;
        var item = TodoItem.Create(otherOwnerId, list.Id, "Buy milk").Value;
        context.TodoLists.Add(list);
        context.TodoItems.Add(item);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsQueryHandler(context, new CurrentUserServiceStub(new UserId(Guid.NewGuid())));
        var result = await handler.Handle(new GetTodoItemsQuery(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
    }
}
