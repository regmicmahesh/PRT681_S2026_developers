using Microsoft.Data.Sqlite;

namespace Todo;

public sealed class TodoRepository : IDisposable
{
    private readonly SqliteConnection connection;

    public TodoRepository(string databasePath = "todos.db")
    {
        connection = new SqliteConnection($"Data Source={databasePath}");
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Todos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                IsCompleted INTEGER NOT NULL DEFAULT 0
            );
            """;
        command.ExecuteNonQuery();
    }

    public TodoItem Add(string title)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Todos (Title) VALUES ($title); SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$title", title);

        var id = Convert.ToInt32(command.ExecuteScalar());
        return new TodoItem(id, title);
    }

    public List<TodoItem> GetAll()
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Title, IsCompleted FROM Todos ORDER BY Id;";

        using var reader = command.ExecuteReader();
        var todos = new List<TodoItem>();
        while (reader.Read())
        {
            todos.Add(new TodoItem(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetBoolean(2)));
        }

        return todos;
    }

    public TodoItem? GetById(int id)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Title, IsCompleted FROM Todos WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        return reader.Read()
            ? new TodoItem(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2))
            : null;
    }

    public void MarkAsCompleted(int id)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "UPDATE Todos SET IsCompleted = 1 WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Todos WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    public void Dispose() => connection.Dispose();
}
