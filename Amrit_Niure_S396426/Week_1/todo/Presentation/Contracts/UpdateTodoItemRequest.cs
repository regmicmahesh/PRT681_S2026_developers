using Domain.Enums;

namespace Presentation.Contracts;

public sealed record UpdateTodoItemRequest(
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate);
