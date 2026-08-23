namespace Todo;

public class TodoItem
{
    public int Id { get; }
    public string Title { get; }
    public bool IsCompleted { get; private set; }

    public TodoItem(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}
