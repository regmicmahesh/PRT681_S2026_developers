using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.DAL;

public class Printer
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;
}