using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAuth.Dtos
{
    public record RegisterDto(
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [MinLength(6)]
        string Password
    );

    public record LoginDto(
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        string Password
    );

    public record AuthResponseDto(
        string Token,
        string Email,
        DateTime ExpiresAt
    );
}
