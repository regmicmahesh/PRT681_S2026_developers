using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Application.TodoLists.Commands;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using Moq;

namespace CleanApp.Application.Tests.TodoLists.Commands;

public class RenameTodoListCommandHandlerTests
{
    private readonly Mock<ITodoListRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly UserId _ownerId = new(Guid.NewGuid());

    [Fact]
    public async Task Handle_WhenListDoesNotExist_ReturnsNotFound()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<TodoListId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoList?)null);

        var handler = new RenameTodoListCommandHandler(_repository.Object, _unitOfWork.Object, new CurrentUserServiceStub(_ownerId));
        var command = new RenameTodoListCommand(Guid.NewGuid(), "Shopping");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task Handle_WhenListBelongsToAnotherUser_ReturnsNotFound()
    {
        var list = TodoList.Create(_ownerId, "Groceries").Value;
        _repository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var otherUser = new CurrentUserServiceStub(new UserId(Guid.NewGuid()));
        var handler = new RenameTodoListCommandHandler(_repository.Object, _unitOfWork.Object, otherUser);
        var command = new RenameTodoListCommand(list.Id.Value, "Shopping");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenListExists_RenamesAndSaves()
    {
        var list = TodoList.Create(_ownerId, "Groceries").Value;
        _repository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var handler = new RenameTodoListCommandHandler(_repository.Object, _unitOfWork.Object, new CurrentUserServiceStub(_ownerId));
        var command = new RenameTodoListCommand(list.Id.Value, "Shopping");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Shopping", list.Title.Value);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
