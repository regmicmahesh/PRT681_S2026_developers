
string menu = @"Welcome!
[1] Convert Temperature. (Celsius to Fahrenheit)
[2] Calculate Grade Average
[3] Check if a Number is Prime
[4] Exit

Enter your option: ";

Console.Write(menu);

if (!int.TryParse(Console.ReadLine(), out int menuOption))
{
    Console.WriteLine("Invalid choice");
    return;
}

switch (menuOption)
{

    case 1:
        Console.Write("Enter temperature (in celsius): ");
        double temperatureCelsius = double.Parse(Console.ReadLine());
        double temperatureFahrenheit = (temperatureCelsius * 9 / 5) + 32;
        Console.WriteLine($"Temperature in Fahrenheit: {temperatureFahrenheit:F2}");
        break;
    case 2:
        Console.Write("How many grades do you want to calculate? ");
        int gradeCount = int.Parse(Console.ReadLine());
        double avgMarks = 0;
        for (int i = 0; i < gradeCount; i++)
        {
            Console.Write($"Grade[{i + 1}]: ");
            double marks = double.Parse(Console.ReadLine());
            avgMarks += marks / gradeCount;
        }
        Console.WriteLine($"Your Average: {avgMarks}");
        break;
    case 3:
        Console.Write("Enter your number: ");
        int number = int.Parse(Console.ReadLine());
        if (number < 2)
        {
            Console.WriteLine("Invalid Number");
            return;
        }

        bool isPrime = true;
        for (int i = 2; i < number / 2; i++)
        {
            if (number % i == 0)
            {
                isPrime = false;
                break;
            }
        }

        Console.WriteLine($"IsPrime: {isPrime}");
        break;

    default:
        Console.WriteLine("Invalid Option");
        return;

}
