
Box<string> nameBox = new ("Deepjan");
Box<int> ageBox = new (28);
Box<decimal> salaryBox = new (700m);

nameBox.ShowValue();
ageBox.ShowValue();
salaryBox.ShowValue();

public class Box<T>
{
    public T Value { get; private set; }

    public Box(T value)
    {
        Value = value;
    }

    public void ShowValue()
    {
        Console.WriteLine($"Stored value: {Value}");
    }
}

