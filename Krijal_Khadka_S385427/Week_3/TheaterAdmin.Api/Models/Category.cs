using System.ComponentModel.DataAnnotations;

namespace TheaterAdmin.Api.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string Code { get; set; } = string.Empty;

    public ICollection<Movie> Movies { get; set; }
        = new List<Movie>();
}