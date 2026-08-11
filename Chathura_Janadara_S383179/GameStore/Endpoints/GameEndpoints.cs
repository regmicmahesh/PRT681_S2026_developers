using GameStore.Repositories;
using GameStore.DTOs;

namespace GameStore.Endpoints;

public static class GameEndpoints
{
  private static readonly GameRepository gameRepository = new GameRepository();
  public static void MapGameEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("/games");
    group.MapGet("/", () =>
    {
      var games = gameRepository.GetAllGames();
      return games is not null ? Results.Ok(games) : Results.NotFound();
    });
    group.MapGet("/{id}", (int id) =>
    {
      var game = gameRepository.GetGameById(id);
      return game is not null ? Results.Ok(game) : Results.NotFound();
    }).WithName("GetGameById");
    group.MapPost("/", (GameDto game) =>
    {
      gameRepository.AddGame(game);
      return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, game);
    });
    group.MapPut("/{id}", (int id, GameDto game) =>
    {
      gameRepository.UpdateGame(game, id);
      return Results.NoContent();
    });
    group.MapDelete("/{id}", (int id) =>
    {
      gameRepository.DeleteGame(id);
      return Results.NoContent();
    });
  }

}