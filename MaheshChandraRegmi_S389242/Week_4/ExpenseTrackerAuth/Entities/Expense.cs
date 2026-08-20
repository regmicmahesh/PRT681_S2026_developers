using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAuth.Entities
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // week 4: every expense belongs to a user
        public string UserId { get; set; } = String.Empty;
    }
}
