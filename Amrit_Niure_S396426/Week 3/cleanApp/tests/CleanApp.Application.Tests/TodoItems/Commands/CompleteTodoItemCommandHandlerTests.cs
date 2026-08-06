using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Application.TodoItems.Commands;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using Moq;

namespace CleanApp.Application.Tests.TodoItems.Commands;

public class CompleteTodoItemCommandHandlerTests
{
    private readonly Mock<ITodoItemRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly UserId _ownerId = new(Guid.NewGuid());
    private readonly ICurrentUserService _currentUser;

    public CompleteTodoItemCommandHandlerTests() => _currentUser = new CurrentUserServiceStub(_ownerId);

    [Fact]
    public async Task Handle_WhenItemDoesNotExist_ReturnsNotFound()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem?)null);

        var handler = new CompleteTodoItemCommandHandler(_repository.Object, _unitOfWork.Object, _currentUser);
        var result = await handler.Handle(new CompleteTodoItemCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task Handle_WhenItemBelongsToAnotherUser_ReturnsNotFound()
    {
        var item = TodoItem.Create(new UserId(Guid.NewGuid()), TodoListId.New(), "Buy milk").Value;
        _repository.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        var handler = new CompleteTodoItemCommandHandler(_repository.Object, _unitOfWork.Object, _currentUser);
        var result = await handler.Handle(new CompleteTodoItemCommand(item.Id.Value), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task Handle_WhenAlreadyCompleted_ReturnsConflict()
    {
        var item = TodoItem.Create(_ownerId, TodoListId.New(), "Buy milk").Value;
        item.Complete();
        _repository.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);

        var handler = new CompleteTodoItemCommandHandler(_repository.Object, _unitOfWork.Object, _currentUser);
        var result = await handler.Handle(new CompleteTodoItemCommand(item.Id.Value), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNotCompleted_CompletesAndSaves()
    {
        var item = TodoItem.Create(_ownerId, TodoListId.New(), "Buy milk").Value;
        _repository.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);

        var handler = new CompleteTodoItemCommandHandler(_repository.Object, _unitOfWork.Object, _currentUser);
        var result = await handler.Handle(new CompleteTodoItemCommand(item.Id.Value), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(item.IsDone);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
