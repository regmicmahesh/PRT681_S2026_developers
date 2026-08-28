//Result<string> result = FindById(1);

//Console.WriteLine(result.Message);

//if (result.IsSuccess)
//{
//    Console.WriteLine($"Data: {result.Data}");
//}

//Console.WriteLine();

//Result<string> result2 = FindById(9);

//Console.WriteLine(result2.Message);

//if (result2.IsSuccess)
//{
//    Console.WriteLine($"Data: {result2.Data}");
//}

//static Result<string> FindById(int id)
//{
//    if (id == 1)
//    {
//        return Result<string>.Success("Shyam", "Name Found");
//    }

//    return Result<string>.Failure("Name not found.");
//}
//public class Result<T>
//{
//    public bool IsSuccess { get; private set; }
//    public string Message { get; private set; }
//    public T? Data { get; private set; }

//    private Result(bool isSuccess, string message, T? data)
//    {
//        IsSuccess = isSuccess;
//        Message = message;
//        Data = data;
//    }
//    public static Result<T> Success(T data, string message)
//    {
//        return new Result<T>(true, message, data);
//    }

//    public static Result<T> Failure(string message)
//    {
//        return new Result<T>(false, message, default);
//    }
    
//}










//Result<string> successResult = new Result<string>(true, "Name Found","Deepjan");

//Result<string> failureResult = new Result<string>(false, "Name not found", null);

//Console.WriteLine(successResult.IsSuccess);
//Console.WriteLine(successResult.Message);
//Console.WriteLine(successResult.Data);
//Console.WriteLine();
//Console.WriteLine(failureResult.IsSuccess);
//Console.WriteLine(failureResult.Message);
//Console.WriteLine(failureResult.Data);

//Console.WriteLine();
//static Result<string> FindById(int id)
//{
//    if(id == 1)
//    {
//        return new Result<string>(true, "Name Found", "Shyam");
//    }

//    return new Result<string>(
//        false, "Name not found.", null);
//}

//Result<string> result = FindById(1);

//Console.WriteLine(result.Message);

//if (result.IsSuccess)
//{
//    Console.WriteLine($"Data: {result.Data}");
//}




//Result<string> result2 = FindById(9);
//Console.WriteLine();
//Console.WriteLine(result2.Message);
//if (result2.IsSuccess)
//{
//    Console.Write($"Data: {result2.Data}");
//}
//public class Result<T>
//{
//    public bool IsSuccess { get; private set; }
//    public string Message { get; private set; }
//    public T? Data { get; private set; }

//    public Result(bool isSuccess, string message, T? data)
//    {
//        IsSuccess = isSuccess;
//        Message = message;
//        Data = data;
//    }
//}
