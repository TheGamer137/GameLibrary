using AutoMapper;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using GameLibrary.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameLibrary.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public GamesController(IGameRepository gameRepository, IMapper mapper)
    {
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    [HttpGet("GetGames")]
    public async Task<IActionResult> GetGames()
    {
        var games = await _gameRepository.GetAllGames();

        if (games.Any())
            return Ok(_mapper.Map<IEnumerable<GameVM>>(games));

        return NotFound();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(GameVM gameVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var game = _mapper.Map<Game>(gameVM);
        await _gameRepository.SaveGame(game);
        return Ok(_mapper.Map<GameVM>(game));
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update(int gameId, GameVM gameVM)
    {
        var existingGame = await _gameRepository.GetGameById(gameId);
        if (existingGame == null)
            return NotFound();
        
        _mapper.Map(gameVM, existingGame);

        await _gameRepository.SaveGame(existingGame);
        return NoContent();
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var game = await _gameRepository.GetGameById(id);
        if (game == null)
            return NotFound();

        await _gameRepository.DeleteGame(game);

        return NoContent();
    }

    [HttpGet("GetGamesByGenre")]
    public async Task<IActionResult> GetGamesByGenre(string genreName)
    {
        var games = await _gameRepository.GetGamesByGenre(genreName);
        if (!games.Any())
            return NotFound();
        return Ok(games.Select(g => g.GameName));
    }
}