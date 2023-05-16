using AutoMapper;
using GameLibrary.WebApi.ViewModels;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using GameLibrary.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace GameLibrary.WebApi.Controllers
{
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
        public IActionResult GetGames()
        {
            var games = _gameRepository.GetAllGames();

            if (games.Any())
                return Ok(_mapper.Map<IEnumerable<GameVM>>(games));

            return NotFound();
        }

        [HttpPost("Create")]
        public IActionResult Create(GameVM gameVM)
        {
            Game game = _mapper.Map<Game>(gameVM);
            if (ModelState.IsValid)
            {
                _gameRepository.SaveGame(game);
                return Ok(_mapper.Map<GameVM>(game));
            }
            return BadRequest(ModelState);
        }
        [HttpPut("Update")]
        public IActionResult Update(int gameId, GameVM gameVM)
        {
            var existingGame = _gameRepository.GetGameById(gameId);
            if (existingGame != null)
            {
                _mapper.Map(gameVM, existingGame);
                if (ModelState.IsValid)
                {
                    _gameRepository.SaveGame(existingGame);
                    return Ok(_mapper.Map<GameVM>(existingGame));
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var game = _gameRepository.GetGameById(id);
            if (game != null)
            {
                _gameRepository.DeleteGame(game);
                return Ok($"Игра {game.GameName} удалена");
            }
            return NotFound();
        }

        [HttpGet("GetGamesByGenre")]
        public IActionResult GetGamesByGenre(string genre)
        {
            if (genre!=null)
            {
                var games = _gameRepository.GetGamesByGenre(genre);
                if (games.Any())
                {
                    var map = _mapper.Map<IEnumerable<GameVM>>(games);
                    return Ok(map.Select(g=>g.GameName));
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
