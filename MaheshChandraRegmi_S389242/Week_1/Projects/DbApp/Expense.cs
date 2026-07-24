using System.Linq.Expressions;

namespace DbApp {
    public class Expense
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }

    public interface IExpenseRepository
    {
        void Add(Expense expense);
        List<Expense> GetAll();
        List<Expense> GetWhere(Expression<Func<Expense, bool>> predicate);

        bool Delete(Guid id);

        decimal GetTotal();
        decimal GetAverage();
    };
}
