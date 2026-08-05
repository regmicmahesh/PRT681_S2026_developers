using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TheaterAdmin.Api.Models;

public class Movie
{
    public int MovieId { get; set; }

    [Required]
    [StringLength(200)]
    public required string MovieName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public required DateOnly ReleaseDate { get; set; }

    [Required]
    [StringLength(150)]
    public required string Director { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    [Display(Name = "Contact Email Address")]
    public required string ContactEmailAddress { get; set; }

    [Required]
    public required Language Language { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ValidateNever]
    public Category? Category { get; set; }
}

