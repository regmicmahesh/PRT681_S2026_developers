using System;

namespace DbApp
{

    public class ExpenseTrackerOop
    {

        private const string menu = """
            Expense Tracker Menu:
            [1] Add Expense
            [2] View All Expenses
            [3] View Total & Average
            [4] Filter Expenses
            [5] Delete Expense
            [6] Exit

            Enter your option:
            """;

        public static void Main()
        {

            ExpenseDbContext ctx = new();
            SqlExpenseRepository repository = new(context: ctx);

            while (true)
            {
                Console.Write(menu);
                if (!int.TryParse(Console.ReadLine(), out int menuChoice))
                {
                    Console.WriteLine("Invalid Menu Option");
                    continue;
                }

                switch (menuChoice)
                {
                    case 1:
                        Console.Write("Expense Name: ");
                        string expenseName = Console.ReadLine() ?? "";

                        Console.Write("Expense Amount: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal expenseAmount))
                        {
                            Console.WriteLine("Invalid Expense Amount");
                            continue;
                        }
                        Expense expense = new()
                        {
                            Description = expenseName,
                            Amount = expenseAmount,
                            CategoryId = 1 // this is bit hacky but maybe in future we can make this better.
                        };
                        repository.Add(expense);
                        Console.WriteLine("Expense Added Successfully!");
                        break;
                    case 2:
                        repository.GetAll().ForEach(ExpensePrinter.PrintExpense);
                        break;
                    case 3:
                        Console.WriteLine($"Total: ${repository.GetTotal()}");
                        Console.WriteLine($"Average: ${repository.GetAverage()}");
                        break;
                    case 4:
                        Console.Write("Enter your threshold amount: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal threshold))
                        {
                            Console.WriteLine("Invalid Threshold");
                            continue;
                        }
                        repository.GetWhere(el => el.Amount > threshold).ForEach(ExpensePrinter.PrintExpense);
                        break;

                    case 5:
                        Console.Write("Enter your Expense GUID: ");
                        Guid guid = Guid.Parse(Console.ReadLine() ?? "");
                        repository.Delete(guid);
                        break;
                    case 6:
                        Console.WriteLine("Exiting");
                        return;
                    default:
                        Console.WriteLine("Invalid Menu Option");
                        continue;
                }

            }


        }
    }

}
