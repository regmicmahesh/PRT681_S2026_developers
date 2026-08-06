//using System.Runtime.Intrinsics.X86;
//using static System.Net.Mime.MediaTypeNames;

//Create a C# delivery checkout system.

//* Standard delivery fee: `5`
//*Express delivery fee: `15`

//Use an interface, two implementing classes, and constructor injection.

//`CheckoutService` should calculate:

//csharp
//finalTotal = orderAmount + deliveryFee;
//

//For an order of `100`, totals should be `105` or `115`.
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

Console.Write("\tWelcome to Delivery fee calculator app\n\n");

Console.Write("Enter the total order Amount: ");
string? amountInput = Console.ReadLine();

//checkNull
InputNullChecker nullChecker = new InputNullChecker(amountInput);
if (nullChecker.IsInputNull())
{
    Console.WriteLine("Error: Input is empty.");
    Console.ReadLine();
    return;
}

if (!decimal.TryParse(amountInput, out decimal orderAmount))
{
    Console.WriteLine("\nThe input is not a valid number");
    return;
}

Console.Clear();
Console.Write("\tWelcome to Delivery fee calculator app\n\n");
Console.WriteLine("\nWe Have two delivery system: You may choose one. \n");
Console.WriteLine("  1. Standard Delivery. Rate: $5 ");
Console.WriteLine("  2. Express Delivery. Rate: $15 ");

Console.WriteLine("\n\nFor Standard Devlivery press key: 'S' \n");
Console.WriteLine("For Express Devlivery press key: 'E' \n");

Console.Write("Please Enter your method of delivery:  ");

string?  deliveryInput= Console.ReadLine();
InputNullChecker isNull = new InputNullChecker(deliveryInput);
if (isNull.IsInputNull())
{
    Console.WriteLine("Error: Input is empty.");
    Console.ReadLine();
    return;
}
String cleanedInput = deliveryInput.Trim().ToLower();
Console.Clear();
Console.Write("\tWelcome to Delivery fee calculator app\n\n");
if (string.IsNullOrWhiteSpace(cleanedInput) || (cleanedInput != "s" && cleanedInput != "e"))//sorry for checking null twice i wanted to learn to use class..
{
    Console.WriteLine("\nThe input is not a valid delivery method");
    return;
}
else if (cleanedInput == "s")
{
    CheckOutService standardDelivery = new CheckOutService(new StandardDelivery());
    standardDelivery.checkout(orderAmount);
}
else if(cleanedInput == "e")
{
    CheckOutService expressDelivery = new CheckOutService(new ExpressDelivery());
    expressDelivery.checkout(orderAmount);
}

Console.ReadLine();

InputNullChecker deliverynullChecker = new InputNullChecker(deliveryInput);
if (deliverynullChecker.IsInputNull())
{
    Console.WriteLine("Error: Input is empty.");
    Console.ReadLine();
    return;
}




public class InputNullChecker
{
    private string _UserInput;

    public InputNullChecker(string userInput)
    {
        _UserInput = userInput;
    }
    public bool IsInputNull()
    {
        if (string.IsNullOrWhiteSpace(_UserInput))
        {
            return true;
        }
        else
            return false;
    }
} 

public interface IDeliveryFeeCalculator
{
    decimal CalculateFee(decimal orderAmount);
}

public class StandardDelivery:IDeliveryFeeCalculator
{
   
    public decimal CalculateFee(decimal orderAmount)
    {
        Console.WriteLine($"\nDelivery via Standard Delivery! \nRate: $5");
        return 5m;
    }
}

public class ExpressDelivery : IDeliveryFeeCalculator
{
    public decimal CalculateFee(decimal orderAmount)
    {
        Console.WriteLine($"\nDelivery via Express Delivery! \nRate: $15");
        return 15m;
    }
}

public class CheckOutService
{
    private IDeliveryFeeCalculator _deliveryFeeCalculator;

    public CheckOutService(IDeliveryFeeCalculator deliveryFeeCalculator)
    {
        _deliveryFeeCalculator = deliveryFeeCalculator;
    }

    public void checkout(decimal orderAmount)
    {
        decimal deliveryFee =  _deliveryFeeCalculator.CalculateFee(orderAmount);
        Console.WriteLine("\n\tDelivery Invoice");
        Console.WriteLine($"Total Order Amount: {orderAmount}");
        Console.WriteLine($"The delivery fee: {deliveryFee}");
        Console.WriteLine($"Final Amount: {orderAmount + deliveryFee}");
    }
}
