namespace ExpenseTrackerApi.Dtos
{
    public record SummaryDto(
        int Count,
        decimal Total,
        decimal Average
    );
}
