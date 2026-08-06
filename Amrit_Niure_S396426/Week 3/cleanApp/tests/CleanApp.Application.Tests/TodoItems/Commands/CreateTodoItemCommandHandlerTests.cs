using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Application.TodoItems.Commands;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using Moq;

namespace CleanApp.Application.Tests.TodoItems.Commands;

public class CreateTodoItemCommandHandlerTests
{
    private readonly Mock<ITodoListRepository> _listRepository = new();
    private readonly Mock<ITodoItemRepository> _itemRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly UserId _ownerId = new(Guid.NewGuid());
    private readonly ICurrentUserService _currentUser;

    public CreateTodoItemCommandHandlerTests() => _currentUser = new CurrentUserServiceStub(_ownerId);

    [Fact]
    public async Task Handle_WhenListDoesNotExist_ReturnsNotFound()
    {
        _listRepository.Setup(r => r.GetByIdAsync(It.IsAny<TodoListId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoList?)null);

        var handler = new CreateTodoItemCommandHandler(_listRepository.Object, _itemRepository.Object, _unitOfWork.Object, _currentUser);
        var command = new CreateTodoItemCommand(Guid.NewGuid(), "Buy milk", 1, null, null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        _itemRepository.Verify(r => r.Add(It.IsAny<TodoItem>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenListBelongsToAnotherUser_ReturnsNotFound()
    {
        var list = TodoList.Create(new UserId(Guid.NewGuid()), "Groceries").Value;
        _listRepository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var handler = new CreateTodoItemCommandHandler(_listRepository.Object, _itemRepository.Object, _unitOfWork.Object, _currentUser);
        var command = new CreateTodoItemCommand(list.Id.Value, "Buy milk", 1, null, null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        _itemRepository.Verify(r => r.Add(It.IsAny<TodoItem>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenListExists_AddsItemAndSaves()
    {
        var list = TodoList.Create(_ownerId, "Groceries").Value;
        _listRepository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var handler = new CreateTodoItemCommandHandler(_listRepository.Object, _itemRepository.Object, _unitOfWork.Object, _currentUser);
        var command = new CreateTodoItemCommand(list.Id.Value, "Buy milk", 1, "2%", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _itemRepository.Verify(
            r => r.Add(It.Is<TodoItem>(i => i.Title.Value == "Buy milk" && i.OwnerId == _ownerId)),
            Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
