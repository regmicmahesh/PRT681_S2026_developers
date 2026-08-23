using ExpenseTrackerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseDbContext _context;

        public ExpenseRepository(ExpenseDbContext context)
        {
            _context = context;
        }

        public void Add(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public List<Expense> GetAll() =>
            [.. _context.Expenses.Include(e => e.Category).OrderByDescending(e => e.CreatedAt)];

        public Expense? GetById(Guid id) =>
            _context.Expenses.Include(e => e.Category).FirstOrDefault(e => e.Id == id);

        public List<Expense> GetWhere(Func<Expense, bool> predicate) =>
            [.. _context.Expenses.Include(e => e.Category).AsEnumerable().Where(predicate)];

        public bool Update(Expense expense)
        {
            if (_context.Expenses.Find(expense.Id) is null)
            {
                return false;
            }

            _context.Expenses.Update(expense);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Guid id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense is null)
            {
                return false;
            }

            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return true;
        }

        public decimal GetTotal() =>
            _context.Expenses.Sum(e => (decimal?)e.Amount) ?? 0;

        public decimal GetAverage()
        {
            if (!_context.Expenses.Any())
            {
                return 0;
            }

            return _context.Expenses.Average(e => e.Amount);
        }

        public List<Category> GetCategories() => [.. _context.Categories.OrderBy(c => c.Id)];

        public Category? GetCategory(int id) => _context.Categories.Find(id);
    }
}
