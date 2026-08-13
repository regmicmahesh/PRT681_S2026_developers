
Console.WriteLine("\t\tWelcome to Delivery Calculator App\n\n");

Console.WriteLine("We offer two types of delivery: \n" +
    "1. Standard Delivery: \n" +
    "\t--> Delivery Rate for order below $100: $5\n" +
    "\t--> Delivery Rate for order above $100: $0\n" +
    "2. Express Delivery \n" +
    "\t--> Delivery Rate for order below 100: $15\n" +
    "\t--> Delivery Rate for order above 100: $10\n");

Console.Write("\nPlease enter the total bill Amount: ");
string? inputBillAmount = Console.ReadLine();
inputBillAmount = inputBillAmount.Trim();

if (string.IsNullOrWhiteSpace(inputBillAmount))
{ 
    Console.WriteLine("\n   Error: The Bill Amount cannot be zero or empty.");
    return;
}

if (!decimal.TryParse(inputBillAmount, out decimal billAmount))
{
    Console.WriteLine("\n   Error: The Bill Amount is not a valid number.");
    return;
}

if (billAmount < 0)
{
    Console.WriteLine("\n   Error: The Bill Amount cannot be negative. ");
    return;
}




Console.WriteLine("\nPlease press key 's' for standard delivery \nOr\n" +
    "Press 'E' for Express Delivery.");
Console.Write("\nNow please choose your method of delivery: ");
string? inputDeliveryType = Console.ReadLine();



IDeliveryFeeCalculator deliveryFeeCalc;

if (string.IsNullOrWhiteSpace(inputDeliveryType))
{
    Console.WriteLine("\n   Error: Delivery Type cannot be Empty.");
    return;
}
string cleanedinputDeliveryType = inputDeliveryType.Trim().ToLower();

if(cleanedinputDeliveryType!="s"&& cleanedinputDeliveryType != "e")
{
    Console.WriteLine("\n   Error: Invalid Delivery Type.");
    return;
}

if (cleanedinputDeliveryType == "s")
{
    deliveryFeeCalc = new StandardDeliveryRate();
}
else
    deliveryFeeCalc = new ExpressDeliveryRate();

DeliveryFeeCalculator deliveryFeeCalculat = new DeliveryFeeCalculator(deliveryFeeCalc);
deliveryFeeCalculat.DeliveryFeeCalculate(billAmount);


public interface IDeliveryFeeCalculator
{
    public decimal DeliveryFee(decimal billAmount);
    public string DeliveryType();
}

public class StandardDeliveryRate : IDeliveryFeeCalculator
{
    public decimal DeliveryFee(decimal billAmount)
    {
        if (billAmount >= 100)
        {
            return 0m;
        }else return 5m;
        
    }

    public string DeliveryType()
    {
        return $"Standard Delivery";
    }
}

public class ExpressDeliveryRate : IDeliveryFeeCalculator
{
    public decimal DeliveryFee(decimal billAmount)
    {
        if (billAmount >= 100)
        {
            return 10m;
        }
        else return 15m;

    }

    public string DeliveryType()
    {
        return $"Express Delivery";
    }
}

public class DeliveryFeeCalculator
{
    private readonly IDeliveryFeeCalculator _DeliveryFeeCalculator;

    public DeliveryFeeCalculator(IDeliveryFeeCalculator deliveryFeeCalculator)
    {
        _DeliveryFeeCalculator = deliveryFeeCalculator;
    }

    public void DeliveryFeeCalculate(decimal billAmount)
    {
        decimal totalBill = billAmount + _DeliveryFeeCalculator.DeliveryFee(billAmount);
        Console.Clear();
        Console.WriteLine(totalBill);
    }

}