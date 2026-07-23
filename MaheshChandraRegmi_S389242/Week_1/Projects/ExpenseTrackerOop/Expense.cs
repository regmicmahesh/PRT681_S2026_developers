
namespace ExpenseTrackerOop
{

    public record Expense(Guid Id, string Description, decimal Amount, DateTime CreatedDate);


    public interface IExpenseRepository
    {
        void Add(Expense expense);
        List<Expense> GetAll();
        List<Expense> GetWhere(Func<Expense, bool> predicate);

        bool Delete(Guid id);

        decimal GetTotal();
        decimal GetAverage();
    };

}
