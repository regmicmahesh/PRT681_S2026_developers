

static void PrintList<T>(List<T> items)
{
    foreach (T item in items)
    {
        Console.WriteLine($"{item}");
    }
}

List<string> names = new List<string> { "Deepjan", "Alex", "Sarah" };
List<int> ages = new List<int> { 25, 28, 30 };
List<decimal> salaries = new List<decimal> { 4000m, 5000m, 6000m };

PrintList(names);
Console.WriteLine();

PrintList(ages);
Console.WriteLine();

PrintList(salaries);




//Result<string> nameResult = new (true, "Name loaded successfully","Deepjan");

//Result<int> ageResult = new(true, "Age loaded successfully", 28);

//nameResult.ShowResult();
//ageResult.ShowResult();
//public class Result<T>
//{
//    public bool IsSuccess { get; private set; }
//    public string Message { get; private set; }
//    public T Data { get; private set; }
//    public Result(bool isSuccess, string message, T data)
//    {
//        IsSuccess = isSuccess;
//        Message = message;
//        Data = data;
//    }

//    public void ShowResult()
//    {
//        Console.WriteLine($"Success: {IsSuccess}");
//        Console.WriteLine($"Message: {Message}");
//        Console.WriteLine($"Data: {Data}");
//        Console.WriteLine();
//    }

//}

//String name = ReturnSameValue("Deepjan");
//int age = ReturnSameValue(28);
//decimal salary = ReturnSameValue(700m);

//Console.WriteLine(name);
//Console.WriteLine(age);
//Console.WriteLine(salary);
//static T ReturnSameValue<T> (T value)
//{
//    return value;
//}



//PrintItem("Deepjan");
//PrintItem(28);
//PrintItem(700m);
//PrintItem(true);
//static void PrintItem <T>( T item)
//{
//    Console.WriteLine($"Item: {item}");
//}





//Box<string> nameBox = new ("Deepjan");
//Box<int> ageBox = new (28);
//Box<decimal> salaryBox = new (700m);

//nameBox.ShowValue();
//ageBox.ShowValue();
//salaryBox.ShowValue();

//public class Box<T>
//{
//    public T Value { get; private set; }

//    public Box(T value)
//    {
//        Value = value;
//    }

//    public void ShowValue()
//    {
//        Console.WriteLine($"Stored value: {Value}");
//    }
//}

