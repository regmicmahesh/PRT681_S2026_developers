using ExpenseTrackerAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAuth.Data
{
    public class ExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public List<Expense> GetAllForUser(string userId) =>
            [.. _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)];

        public Expense? GetByIdForUser(Guid id, string userId) =>
            _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefault(e => e.Id == id && e.UserId == userId);

        public List<Expense> GetWhereForUser(string userId, Func<Expense, bool> predicate) =>
            [.. GetAllForUser(userId).Where(predicate)];

        public bool Update(Expense expense)
        {
            _context.Expenses.Update(expense);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteForUser(Guid id, string userId)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == id && e.UserId == userId);
            if (expense is null)
            {
                return false;
            }

            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return true;
        }

        public decimal GetTotalForUser(string userId) =>
            _context.Expenses.Where(e => e.UserId == userId).Sum(e => (decimal?)e.Amount) ?? 0;

        public decimal GetAverageForUser(string userId)
        {
            var query = _context.Expenses.Where(e => e.UserId == userId);
            if (!query.Any())
            {
                return 0;
            }

            return query.Average(e => e.Amount);
        }

        public List<Category> GetCategories() => [.. _context.Categories.OrderBy(c => c.Id)];

        public Category? GetCategory(int id) => _context.Categories.Find(id);
    }
}
