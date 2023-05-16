using GameLibrary.WebApi.Data;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.WebApi.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _dbContext;
        public GameRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void SaveGame(Game game)
        {
            var gameExists = _dbContext.Games.Any(g => g.GameName.Contains(game.GameName));
            if (gameExists.Equals(true))
                throw new Exception("Такая игра уже есть в базе данных");
            var studioExists = _dbContext.DeveloperStudios.Any(g => g.Name.Contains(game.DeveloperStudio.Name));
            if (studioExists.Equals(true))
                game.DeveloperStudio = GetDeveloperStudioByName(game.DeveloperStudio.Name);
            var genresInGame = game.GameGenres.Select(g => g.Genre);
            foreach (var genre in genresInGame)
            {
                var genreExists =  _dbContext.Genres.Any(g => g.GenreName.Contains(genre.GenreName));
                if (genreExists.Equals(true))
                {
                    genre.GenreId = GetGenreByName(genre.GenreName).GenreId;
                    genre.GenreName = GetGenreByName(genre.GenreName).GenreName;
                }
            }
            if (game.Id > 0)
            {
                _dbContext.Update(game);
            }
            else
                _dbContext.Games.Add(game);
            _dbContext.SaveChanges();
        }

        private Genre GetGenreByName(string genreName) =>
            _dbContext.Genres.FirstOrDefault(g => g.GenreName == genreName);

        private DeveloperStudio GetDeveloperStudioByName(string studioName) => 
            _dbContext.DeveloperStudios.FirstOrDefault(d => d.Name == studioName);

        public void DeleteGame(Game game)
        {
            _dbContext.Games.Remove(game);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Game> GetAllGames() => _dbContext.Games.Include(g => g.DeveloperStudio)
            .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre);

        public Game? GetGameById(int id) => 
            _dbContext.Games.Include(g => g.DeveloperStudio).Include(g => g.GameGenres)
            .ThenInclude(gg => gg.Genre).FirstOrDefault(g => g.Id == id);

        public IEnumerable<Game> GetGamesByGenre(string genreName) => 
            _dbContext.Games.Where(g => g.GameGenres.Any(gg => gg.Genre.GenreName.ToLower() == genreName.ToLower()));

    }
}
