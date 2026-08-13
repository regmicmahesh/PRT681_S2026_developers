namespace CleanApp.Application.TodoLists;

public sealed record TodoListResponse(
    Guid Id,
    string Title,
    string Colour,
    DateTime CreatedOnUtc,
    int ItemCount,
    int CompletedItemCount);

public sealed record TodoListDetailResponse(
    Guid Id,
    string Title,
    string Colour,
    DateTime CreatedOnUtc,
    IReadOnlyList<CleanApp.Application.TodoItems.TodoItemResponse> Items);
