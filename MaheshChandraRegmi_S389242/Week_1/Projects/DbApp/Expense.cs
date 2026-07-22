namespace DbApp {
    public class Expense {
        public Guid Id { get; set; }
        public string Description { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
