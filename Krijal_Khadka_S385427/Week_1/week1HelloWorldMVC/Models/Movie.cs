using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace week1HelloWorldMVC.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public string Genre { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 1000)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
}