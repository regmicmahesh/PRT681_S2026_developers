namespace ExpenseTrackerOop
{
    public class InMemoryExpenseRepository : IExpenseRepository
    {

        private readonly List<Expense> expenses = [];

        public void Add(Expense expense) => this.expenses.Add(expense);

        public bool Delete(Guid id) => this.expenses.RemoveAll(e => e.Id == id) > 0;

        public List<Expense> GetAll() => [.. this.expenses];

        public decimal GetAverage() => this.expenses.Count == 0 ? 0 : this.GetTotal() / this.expenses.Count;

        public decimal GetTotal() => this.expenses.Sum(e => e.Amount);

        public List<Expense> GetWhere(Func<Expense, bool> predicate) => [.. this.expenses.Where(predicate)];
    }
}
