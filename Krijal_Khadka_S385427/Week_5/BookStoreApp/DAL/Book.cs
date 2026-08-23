using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.DAL;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}