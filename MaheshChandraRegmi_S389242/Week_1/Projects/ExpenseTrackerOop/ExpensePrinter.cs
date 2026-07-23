
namespace ExpenseTrackerOop
{
    public static class ExpensePrinter
    {
        public static void PrintExpense(Expense exp)
        {
            if (exp == null) return;

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"ID: {exp.Id}");
            Console.WriteLine($"Description: {exp.Description}");
            Console.WriteLine($"Amount: {exp.Amount}");
            Console.WriteLine($"Created At: {exp.CreatedDate}");
            Console.WriteLine("-------------------------------");
        }
    };

}
