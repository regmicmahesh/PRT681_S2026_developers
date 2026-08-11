namespace GameStore.Models;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Genre? Genre { get; set; }
    public int? GenreId { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public DateOnly ReleaseDate { get; set; }
}
