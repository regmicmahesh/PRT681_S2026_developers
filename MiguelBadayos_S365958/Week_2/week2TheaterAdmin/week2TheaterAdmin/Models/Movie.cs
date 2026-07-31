namespace week2TheaterAdmin.Models;

public class Movie
{
    public required int MovieId { get; set; }
    public required string MovieName { get; set; }
    public required DateOnly ReleaseDate { get; set; }
    public required string ContactEmailAddress { get; set; }
    public required Language Language { get; set; }
    public required Category Category { get; set; }
}
