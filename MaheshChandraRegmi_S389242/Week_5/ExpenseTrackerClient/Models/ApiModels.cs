namespace ExpenseTrackerClient.Models
{
    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token, string Email, DateTime ExpiresAt);

    public record CreateExpenseRequest(string Description, decimal Amount, int CategoryId);
    public record ExpenseItem(
        Guid Id,
        string Description,
        decimal Amount,
        DateTime CreatedAt,
        int CategoryId,
        string CategoryName
    );
    public record ExpenseSummary(int Count, decimal Total, decimal Average);
    public record CategoryItem(int Id, string Name);
}
