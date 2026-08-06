namespace ExpenseTrackerApi.Dtos
{
    public record ExpenseDto(
        Guid Id,
        string Description,
        decimal Amount,
        DateTime CreatedAt,
        int CategoryId,
        string CategoryName
    );
}
