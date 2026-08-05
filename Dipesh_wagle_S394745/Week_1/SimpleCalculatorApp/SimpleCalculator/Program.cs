using System;

namespace SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Simple Calculator ===");
            Console.WriteLine("Enter 'exit' to quit\n");

            while (true)
            {
                Console.Write("Enter first number: ");
                if (!GetNumber(out double num1))
                    break;

                Console.Write("Enter operator (+, -, *, /): ");
                char op = Console.ReadLine()?.Trim()[0] ?? ' ';

                Console.Write("Enter second number: ");
                if (!GetNumber(out double num2))
                    break;

                double result = Calculate(num1, num2, op);
                Console.WriteLine($"Result: {num1} {op} {num2} = {result}\n");
            }

            Console.WriteLine("Calculator closed.");
        }

        static bool GetNumber(out double number)
        {
            string? input = Console.ReadLine();

            if (input?.ToLower() == "exit")
            {
                number = 0;
                return false;
            }

            if (double.TryParse(input, out number))
                return true;

            Console.WriteLine("Invalid input. Please enter a valid number.");
            return GetNumber(out number);
        }

        static double Calculate(double num1, double num2, char op)
        {
            return op switch
            {
                '+' => num1 + num2,
                '-' => num1 - num2,
                '*' => num1 * num2,
                '/' => num2 != 0 ? num1 / num2 : throw new InvalidOperationException("Cannot divide by zero"),
                _ => throw new InvalidOperationException($"Unknown operator: {op}")
            };
        }
    }
}
