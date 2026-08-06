using CleanApp.Application.TodoItems;

namespace CleanApp.Application.Admin;

public sealed record AdminTodoListResponse(
    Guid Id,
    string Title,
    string Colour,
    DateTime CreatedOnUtc,
    Guid OwnerId,
    string OwnerEmail,
    int ItemCount,
    int CompletedItemCount);

public sealed record AdminTodoListDetailResponse(
    Guid Id,
    string Title,
    string Colour,
    DateTime CreatedOnUtc,
    Guid OwnerId,
    string OwnerEmail,
    IReadOnlyList<TodoItemResponse> Items);
