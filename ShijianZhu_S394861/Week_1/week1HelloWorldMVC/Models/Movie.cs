using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace week1HelloWorldMVC.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    [NotInFuture]
    public DateTime ReleaseDate { get; set; } = DateTime.Today;

    [Required]
    [StringLength(30)]
    public string Genre { get; set; } = string.Empty;

    [Range(0.01, 1000)]
    [Column(TypeName = "decimal(18,2)")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required]
    [RegularExpression(
        @"^(G|PG|PG-13|M|MA15\+|R18\+)$",
        ErrorMessage = "Rating must be G, PG, PG-13, M, MA15+, or R18+."
    )]
    public string Rating { get; set; } = string.Empty;
}

public sealed class NotInFutureAttribute : ValidationAttribute
{
    public NotInFutureAttribute()
        : base("Release date cannot be in the future.")
    {
    }

    public override bool IsValid(object? value)
    {
        return value is null || value is DateTime date && date.Date <= DateTime.Today;
    }
}
