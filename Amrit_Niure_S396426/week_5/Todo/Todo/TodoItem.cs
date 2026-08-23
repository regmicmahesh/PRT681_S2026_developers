namespace Todo;

public class TodoItem
{
    public int Id { get; }
    public string Title { get; }
    public bool IsCompleted { get; private set; }

    public TodoItem(int id, string title, bool isCompleted = false)
    {
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}
