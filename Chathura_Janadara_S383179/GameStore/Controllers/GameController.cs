
using GameStore.DTOs;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
   private readonly GameRepository gameRepository = new GameRepository();

   [HttpGet]
   public IActionResult GetAllGames()
  {
    List<GameDto> games = gameRepository.GetAllGames();
    return games is null ? NotFound() : Ok(games);
  }

  [HttpGet("{id}", Name = "GetGameById")]
  public IActionResult GetGameById(int id)
  {
    var game = gameRepository.GetGameById(id);

    return game is null ? NotFound() : Ok(game);
  }

  [HttpPost]
  public IActionResult CreateGame(GameDto game)
  {
    gameRepository.AddGame(game);
    return CreatedAtRoute(nameof(GetGameById), new { id = game.Id }, game);
  }

  [HttpPut("{id}")]
  public IActionResult UpdateGame(int id, GameDto game)
  {
    if (id != game.Id) return BadRequest();

    gameRepository.UpdateGame(game,id);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteGame(int id)
  {
    gameRepository.DeleteGame(id);
    return NoContent();
  }


}