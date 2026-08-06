using System.ComponentModel.DataAnnotations;
using TheaterAdmin.Api.Models;

namespace TheaterAdmin.Api.DTOs;

public class MovieRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Director { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [EnumDataType(typeof(MovieLanguage))]
    public MovieLanguage Language { get; set; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Please select a valid category.")]
    public int CategoryId { get; set; }
}