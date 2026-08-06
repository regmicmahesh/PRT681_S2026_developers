namespace TodoApi.Dtos;

public record TodoItemDto(int Id, string Title, bool IsComplete, DateTime CreatedAt, DateTime? DueDate);

public record CreateTodoDto(string Title, DateTime? DueDate);

public record UpdateTodoDto(string Title, bool IsComplete, DateTime? DueDate);
