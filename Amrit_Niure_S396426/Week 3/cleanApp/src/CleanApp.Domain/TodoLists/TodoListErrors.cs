using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoLists;

public static class TodoListErrors
{
    public static readonly Error TitleEmpty =
        Error.Validation("TodoList.TitleEmpty", "Title cannot be empty.");

    public static readonly Error TitleTooLong =
        Error.Validation("TodoList.TitleTooLong", $"Title cannot exceed {TodoListTitle.MaxLength} characters.");

    public static readonly Error InvalidColour =
        Error.Validation("TodoList.InvalidColour", "Colour must be a valid hex code, e.g. #FFAA00.");

    public static readonly Error CannotDeleteNonEmpty =
        Error.Conflict("TodoList.CannotDeleteNonEmpty", "Cannot delete a todo list that still has items.");

    public static Error NotFound(TodoListId id) =>
        Error.NotFound("TodoList.NotFound", $"Todo list with id '{id}' was not found.");
}
