using ExpenseTrackerApi.Entities;

namespace ExpenseTrackerApi.Data
{
    public interface IExpenseRepository
    {
        void Add(Expense expense);
        List<Expense> GetAll();
        Expense? GetById(Guid id);
        List<Expense> GetWhere(Func<Expense, bool> predicate);
        bool Update(Expense expense);
        bool Delete(Guid id);
        decimal GetTotal();
        decimal GetAverage();
        List<Category> GetCategories();
        Category? GetCategory(int id);
    }
}
