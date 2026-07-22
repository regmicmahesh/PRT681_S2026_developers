using System;

string menu = @"Expense Tracker Menu:
[1] Add Expense
[2] View All Expenses
[3] View Total & Average
[4] Filter Expenses
[5] Delete Expense
[6] Exit

Enter your option: ";

// The tuple are expense name and the amount.
List<(string, decimal)> expenses = [];


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
            string expenseName = Console.ReadLine();

            Console.Write("Expense Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal expenseAmount))
            {
                Console.WriteLine("Invalid Expense Amount");
                continue;
            }
            expenses.Add((expenseName, expenseAmount));
            Console.WriteLine("Expense Added Successfully!");
            break;

        case 2:
            foreach (var (name, amt) in expenses)
            {
                Console.WriteLine($"{name}: ${amt:F2}");
            }
            break;
        case 3:
            decimal total = 0;
            decimal avg = 0;
            foreach (var (_, amt) in expenses)
            {
                total += amt;
                avg += amt / expenses.Count;
            }
            Console.WriteLine($"Total: ${total}");
            Console.WriteLine($"Average: ${avg}");
            break;

        case 4:
            Console.Write("Enter your threshold amount: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal threshold))
            {
                Console.WriteLine("Invalid Threshold");
                continue;
            }

            foreach (var (name, amt) in expenses)
            {
                if (amt <= threshold) continue;
                Console.WriteLine($"{name}: ${amt:F2}");
            }
            break;

        case 5:

            Console.Write("Enter your expense name: ");
            string deleteName = Console.ReadLine();


            int c = expenses.RemoveAll(e => e.Item1 == deleteName);
            Console.WriteLine($"Removed ${c} items.");

            break;



        case 6:
            Console.WriteLine("Exiting");
            return;
        default:
            Console.WriteLine("Invalid Menu Option");
            continue;


    }

}
