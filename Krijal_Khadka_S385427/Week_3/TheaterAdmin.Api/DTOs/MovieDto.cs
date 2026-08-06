using TheaterAdmin.Api.Models;

namespace TheaterAdmin.Api.DTOs;

public class MovieDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Director { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    public MovieLanguage Language { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string CategoryCode { get; set; } = string.Empty;
}