SimpleStore<string> nameStore = new SimpleStore<string>();

nameStore.AddItem("Deepjan");
nameStore.AddItem("Alex");
nameStore.AddItem("Sarah");


nameStore.PrintAllItems();
Console.WriteLine();
SimpleStore<int> ageStore = new SimpleStore<int>();
ageStore.AddItem(28);
ageStore.AddItem(26);
ageStore.AddItem(21);

ageStore.PrintAllItems();

public class SimpleStore<T>
{
    private List<T> _items =new List<T>();

   

    public void AddItem(T item)
    {
        _items.Add(item);
    }
    public List<T> GetAllItems()
    {
        return _items;
    }

    public void PrintAllItems()
    {
        foreach(T item in _items)
        {
            Console.WriteLine(item);
        }
    }
}