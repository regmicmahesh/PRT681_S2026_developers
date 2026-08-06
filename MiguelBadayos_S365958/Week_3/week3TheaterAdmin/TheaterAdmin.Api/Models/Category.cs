using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheaterAdmin.Api.Models;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(10)]
    public required string CategoryCode { get; set; }

    [Required]
    [StringLength(100)]
    public required string CategoryName { get; set; }

    [ValidateNever]
    [JsonIgnore]
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

