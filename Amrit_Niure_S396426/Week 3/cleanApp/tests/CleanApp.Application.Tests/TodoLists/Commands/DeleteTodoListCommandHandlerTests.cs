using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Tests.TestSupport;
using CleanApp.Application.TodoLists.Commands;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using Moq;

namespace CleanApp.Application.Tests.TodoLists.Commands;

public class DeleteTodoListCommandHandlerTests
{
    private readonly Mock<ITodoListRepository> _listRepository = new();
    private readonly Mock<ITodoItemRepository> _itemRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly UserId _ownerId = new(Guid.NewGuid());
    private readonly ICurrentUserService _currentUser;

    public DeleteTodoListCommandHandlerTests() => _currentUser = new CurrentUserServiceStub(_ownerId);

    [Fact]
    public async Task Handle_WhenListHasItems_ReturnsConflict()
    {
        var list = TodoList.Create(_ownerId, "Groceries").Value;
        _listRepository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(list);
        _itemRepository.Setup(r => r.AnyForListAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteTodoListCommandHandler(_listRepository.Object, _itemRepository.Object, _unitOfWork.Object, _currentUser);
        var result = await handler.Handle(new DeleteTodoListCommand(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        _listRepository.Verify(r => r.Remove(It.IsAny<TodoList>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenListBelongsToAnotherUser_ReturnsNotFound()
    {
        var list = TodoList.Create(_ownerId, "Groceries").Value;
        _listRepository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var otherUser = new CurrentUserServiceStub(new UserId(Guid.NewGuid()));
        var handler = new DeleteTodoListCommandHandler(_listRepository.Object, _itemRepository.Object, _unitOfWork.Object, otherUser);
        var result = await handler.Handle(new DeleteTodoListCommand(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        _listRepository.Verify(r => r.Remove(It.IsAny<TodoList>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenListIsEmpty_RemovesAndSaves()
    {
        var list = TodoList.Create(_ownerId, "Groceries").Value;
        _listRepository.Setup(r => r.GetByIdAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(list);
        _itemRepository.Setup(r => r.AnyForListAsync(list.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var handler = new DeleteTodoListCommandHandler(_listRepository.Object, _itemRepository.Object, _unitOfWork.Object, _currentUser);
        var result = await handler.Handle(new DeleteTodoListCommand(list.Id.Value), CancellationToken.None);

        Assert.True(result.IsSuccess);
        _listRepository.Verify(r => r.Remove(list), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
