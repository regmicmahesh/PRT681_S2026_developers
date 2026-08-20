using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAuth.Dtos
{
    public record CreateExpenseDto(
        [Required]
        [MinLength(1)]
        string Description,

        [Range(0.01, 1_000_000)]
        decimal Amount,

        [Range(1, int.MaxValue)]
        int CategoryId
    );

    public record UpdateExpenseDto(
        [Required]
        [MinLength(1)]
        string Description,

        [Range(0.01, 1_000_000)]
        decimal Amount,

        [Range(1, int.MaxValue)]
        int CategoryId
    );

    public record ExpenseDto(
        Guid Id,
        string Description,
        decimal Amount,
        DateTime CreatedAt,
        int CategoryId,
        string CategoryName
    );

    public record SummaryDto(
        int Count,
        decimal Total,
        decimal Average
    );
}
