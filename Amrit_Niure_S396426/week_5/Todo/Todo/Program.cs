using Todo;

using var repository = new TodoRepository();

Console.WriteLine("=== My Todo App ===");

while (true)
{
    ShowMenu();
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1": AddTodo(); break;
        case "2": ListTodos(); break;
        case "3": CompleteTodo(); break;
        case "4": DeleteTodo(); break;
        case "0": Console.WriteLine("Goodbye!"); return;
        default: Console.WriteLine("Please choose a valid option."); break;
    }
}

void ShowMenu()
{
    Console.WriteLine();
    Console.WriteLine("1. Add todo");
    Console.WriteLine("2. List todos");
    Console.WriteLine("3. Complete todo");
    Console.WriteLine("4. Delete todo");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
}

void AddTodo()
{
    Console.Write("Enter a title: ");
    var title = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("A todo needs a title.");
        return;
    }

    var todo = repository.Add(title);
    Console.WriteLine($"Added todo #{todo.Id}: {title}");
}

void ListTodos()
{
    var todos = repository.GetAll();
    if (todos.Count == 0)
    {
        Console.WriteLine("There are no todos yet.");
        return;
    }

    foreach (var todo in todos)
    {
        var status = todo.IsCompleted ? "x" : " ";
        Console.WriteLine($"{todo.Id}. [{status}] {todo.Title}");
    }
}

void CompleteTodo()
{
    var todo = FindTodo("complete");
    if (todo is null) return;

    repository.MarkAsCompleted(todo.Id);
    Console.WriteLine($"Completed: {todo.Title}");
}

void DeleteTodo()
{
    var todo = FindTodo("delete");
    if (todo is null) return;

    repository.Delete(todo.Id);
    Console.WriteLine($"Deleted: {todo.Title}");
}

TodoItem? FindTodo(string action)
{
    Console.Write($"Enter the id of the todo to {action}: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var id))
    {
        Console.WriteLine("The id must be a number.");
        return null;
    }

    var todo = repository.GetById(id);
    if (todo is null) Console.WriteLine($"No todo found with id {id}.");

    return todo;
}
