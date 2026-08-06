using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Application.TodoLists.Commands;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using Moq;

namespace CleanApp.Application.Tests.TodoLists.Commands;

public class CreateTodoListCommandHandlerTests
{
    private readonly Mock<ITodoListRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly ICurrentUserService _currentUser = new CurrentUserServiceStub(new UserId(Guid.NewGuid()));

    [Fact]
    public async Task Handle_WithValidData_AddsListAndSavesChanges()
    {
        var handler = new CreateTodoListCommandHandler(_repository.Object, _unitOfWork.Object, _currentUser);
        var command = new CreateTodoListCommand("Groceries", "#33CC66");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _repository.Verify(
            r => r.Add(It.Is<TodoList>(l => l.Title.Value == "Groceries" && l.OwnerId == _currentUser.UserId)),
            Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidTitle_ReturnsFailureWithoutTouchingRepository()
    {
        var handler = new CreateTodoListCommandHandler(_repository.Object, _unitOfWork.Object, _currentUser);
        var command = new CreateTodoListCommand("", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(TodoListErrors.TitleEmpty, result.Error);
        _repository.Verify(r => r.Add(It.IsAny<TodoList>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
