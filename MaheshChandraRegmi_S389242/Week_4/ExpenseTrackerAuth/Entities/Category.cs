namespace ExpenseTrackerAuth.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<Expense> Expenses { get; set; } = [];
    }
}
