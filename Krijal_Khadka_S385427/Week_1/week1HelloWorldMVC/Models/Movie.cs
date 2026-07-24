using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace week1HelloWorldMVC.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(
        60,
        MinimumLength = 3,
        ErrorMessage = "Title must be between 3 and 60 characters."
    )]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = "Genre is required.")]
    [StringLength(
        30,
        ErrorMessage = "Genre cannot exceed 30 characters."
    )]
    [RegularExpression(
        @"^[A-Z][a-zA-Z\s]*$",
        ErrorMessage = "Genre must start with a capital letter and contain only letters and spaces."
    )]
    public string Genre { get; set; } = string.Empty;

    [Range(
        1,
        100,
        ErrorMessage = "Price must be between $1 and $100."
    )]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Rating is required.")]
    [StringLength(5)]
    [RegularExpression(
        @"^(G|PG|M|MA15\+|R18\+)$",
        ErrorMessage = "Select a valid rating: G, PG, M, MA15+ or R18+."
    )]
    public string Rating { get; set; } = string.Empty;
}