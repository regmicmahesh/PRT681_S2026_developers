using CleanApp.Application.TodoLists.Queries;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Application.Tests.TodoLists.Queries;

public class GetTodoListByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenListDoesNotExist_ReturnsNotFound()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new GetTodoListByIdQueryHandler(context, new CurrentUserServiceStub(new UserId(Guid.NewGuid())));

        var result = await handler.Handle(new GetTodoListByIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_WhenListBelongsToAnotherUser_ReturnsNotFound()
    {
        await using var context = TestDbContextFactory.Create();

        var list = TodoList.Create(new UserId(Guid.NewGuid()), "Groceries").Value;
        context.TodoLists.Add(list);
        await context.SaveChangesAsync();

        var handler = new GetTodoListByIdQueryHandler(context, new CurrentUserServiceStub(new UserId(Guid.NewGuid())));
        var result = await handler.Handle(new GetTodoListByIdQuery(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_WhenListExists_ReturnsListWithItems()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = new UserId(Guid.NewGuid());

        var list = TodoList.Create(ownerId, "Groceries").Value;
        var item = TodoItem.Create(ownerId, list.Id, "Buy milk").Value;
        context.TodoLists.Add(list);
        context.TodoItems.Add(item);
        await context.SaveChangesAsync();

        var handler = new GetTodoListByIdQueryHandler(context, new CurrentUserServiceStub(ownerId));
        var result = await handler.Handle(new GetTodoListByIdQuery(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Groceries", result.Value.Title);
        var responseItem = Assert.Single(result.Value.Items);
        Assert.Equal("Buy milk", responseItem.Title);
    }
}
