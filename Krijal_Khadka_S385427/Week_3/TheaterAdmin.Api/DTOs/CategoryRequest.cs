using System.ComponentModel.DataAnnotations;

namespace TheaterAdmin.Api.DTOs;

public class CategoryRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string Code { get; set; } = string.Empty;
}