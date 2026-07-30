using Domain.Entities;
using Domain.Enums;

namespace Application.TodoItems.Dtos;

public sealed record TodoItemDto(
    Guid Id,
    string Title,
    string? Description,
    Priority Priority,
    bool IsCompleted,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? CompletedAt)
{
    public static TodoItemDto FromEntity(TodoItem entity) => new(
        entity.Id,
        entity.Title.Value,
        entity.Description,
        entity.Priority,
        entity.IsCompleted,
        entity.DueDate,
        entity.CreatedAt,
        entity.CompletedAt);
}
