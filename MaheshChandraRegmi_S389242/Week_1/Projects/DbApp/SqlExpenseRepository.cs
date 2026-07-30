using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DbApp
{
    public class SqlExpenseRepository(ExpenseDbContext context) : IExpenseRepository
    {

        private readonly ExpenseDbContext context = context;

        public void Add(Expense expense) {
            this.context.Expenses.Add(expense);
            this.context.SaveChanges();
        }

        public bool Delete(Guid id) => this.context.Expenses.Where(e => e.Id == id).ExecuteDelete() > 0;

        public List<Expense> GetAll() => [.. context.Expenses];

        public decimal GetAverage() => context.Expenses.Average(e => e.Amount);

        public decimal GetTotal() => context.Expenses.Sum(e => e.Amount);

        public List<Expense> GetWhere(Expression<Func<Expense, bool>> predicate) => [.. this.context.Expenses.Where(predicate)];
    }
}
