using CleanApp.Application.TodoLists.Queries;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Application.Tests.TodoLists.Queries;

public class GetTodoListsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsListsWithItemCounts()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        var list = TodoList.Create(ownerId, "Groceries").Value;
        var item1 = TodoItem.Create(ownerId, list.Id, "Buy milk").Value;
        var item2 = TodoItem.Create(ownerId, list.Id, "Buy eggs").Value;
        item2.Complete();

        context.TodoLists.Add(list);
        context.TodoItems.AddRange(item1, item2);
        await context.SaveChangesAsync();

        var handler = new GetTodoListsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(new GetTodoListsQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("Groceries", response.Title);
        Assert.Equal(2, response.ItemCount);
        Assert.Equal(1, response.CompletedItemCount);
    }

    [Fact]
    public async Task Handle_DoesNotReturnListsBelongingToAnotherUser()
    {
        await using var context = TestDbContextFactory.Create();

        var otherUsersList = TodoList.Create(new UserId(Guid.NewGuid()), "Someone Else's List").Value;
        context.TodoLists.Add(otherUsersList);
        await context.SaveChangesAsync();

        var handler = new GetTodoListsQueryHandler(context, new CurrentUserServiceStub(new UserId(Guid.NewGuid())));
        var result = await handler.Handle(new GetTodoListsQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
    }

    [Fact]
    public async Task Handle_WithTitleFilter_ReturnsOnlyMatchingLists()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        context.TodoLists.AddRange(
            TodoList.Create(ownerId, "Groceries").Value,
            TodoList.Create(ownerId, "Chores").Value);
        await context.SaveChangesAsync();

        var handler = new GetTodoListsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(new GetTodoListsQuery(TitleContains: "Groc"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("Groceries", response.Title);
    }

    [Fact]
    public async Task Handle_WithPaging_ReturnsRequestedPageAndTotalCount()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        context.TodoLists.AddRange(
            TodoList.Create(ownerId, "List A").Value,
            TodoList.Create(ownerId, "List B").Value,
            TodoList.Create(ownerId, "List C").Value);
        await context.SaveChangesAsync();

        var handler = new GetTodoListsQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(
            new GetTodoListsQuery(Page: 2, PageSize: 2, SortBy: TodoListSortBy.Title), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Equal(2, result.Value.TotalPages);
        var response = Assert.Single(result.Value.Items);
        Assert.Equal("List C", response.Title);
    }
}
