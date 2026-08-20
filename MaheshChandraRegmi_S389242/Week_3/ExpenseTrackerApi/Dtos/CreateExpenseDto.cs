using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Dtos
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
}
