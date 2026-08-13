namespace CleanApp.Presentation.TodoLists.Contracts;

public sealed record CreateTodoListRequest(string Title, string? Colour);

public sealed record RenameTodoListRequest(string Title);

public sealed record ChangeTodoListColourRequest(string Colour);
