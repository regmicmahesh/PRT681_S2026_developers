using GameStore.DTOs;

namespace GameStore.Repositories
{
  public class GameRepository
  {
    private readonly List<GameDto> games =
    [
      new (1, "Street Fighters II", "Fighting", 56.74, "des 1", new DateOnly(1991, 1, 1)),
      new (2, "Super Mario World", "Platformer", 49.9, "des 2", new DateOnly(1990, 1, 1)),
      new (3, "The Legend of Zelda: A Link to the Past", "Action-Adventure", 59.9, "des 3", new DateOnly(1991, 1, 1))
    ];
    public List<GameDto> GetAllGames()
    {
      return games;
    }
    public GameDto GetGameById(int id)
    {
      return games.Find(game => game.Id == id);
    }
    public void AddGame(GameDto game)
    {
      games.Add(game);
    }
    public void UpdateGame(GameDto game, int id)
    {
      var existingGame = games.Find(g => g.Id == id);
      if (existingGame != null)
      {
        games.Remove(existingGame);
        games.Add(game);
      }
    }
    public void DeleteGame(int id)
    {
      var existingGame = games.Find(g => g.Id == id);
      if (existingGame != null)
      {
        games.Remove(existingGame);
      }
    }
  }
}