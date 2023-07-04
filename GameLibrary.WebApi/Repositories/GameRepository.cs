using GameLibrary.WebApi.Data;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.WebApi.Repositories;

public class GameRepository : IGameRepository
{
    private readonly AppDbContext _dbContext;

    public GameRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveGame(Game game)
    {
        try
        {
            var existingStudio = await GetDeveloperStudioByName(game.DeveloperStudio.Name);
            if (existingStudio != null)
                game.DeveloperStudio = existingStudio;
            foreach (var gameGenre in game.GameGenres)
            {
                var existingGenre = await GetGenreByName(gameGenre.Genre.GenreName);
                if (existingGenre != null) gameGenre.Genre = existingGenre;
            }

            if (game.Id > 0)
                _dbContext.Update(game);
            else
                _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось сохранить игру", ex);
        }
    }

    public async Task DeleteGame(Game game)
    {
        _dbContext.Games.Remove(game);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Game>> GetAllGames()
    {
        var games = await _dbContext.Games.ToListAsync();
        foreach (var game in games)
        {
            await _dbContext.Entry(game)
                .Reference(g => g.DeveloperStudio)
                .LoadAsync();

            await _dbContext.Entry(game)
                .Collection(g => g.GameGenres)
                .Query()
                .Include(gg => gg.Genre)
                .LoadAsync();
        }

        return games;
    }

    public async Task<Game?> GetGameById(int id)
    {
        var game = await _dbContext.Games.FindAsync(id);
        if (game != null)
        {
            await _dbContext.Entry(game)
                .Reference(g => g.DeveloperStudio)
                .LoadAsync();

            await _dbContext.Entry(game)
                .Collection(g => g.GameGenres)
                .Query()
                .Include(gg => gg.Genre)
                .LoadAsync();
        }

        return game;
    }

    public async Task<IEnumerable<Game>?> GetGamesByGenre(string genreName)
    {
        return await _dbContext.Games
            .Where(g => g.GameGenres.Any(gg => gg.Genre.GenreName.ToLower() == genreName.ToLower()))
            .ToListAsync();
    }

    private async Task<Genre?> GetGenreByName(string genreName)
    {
        return await _dbContext.Genres.FirstOrDefaultAsync(g => g.GenreName == genreName);
    }

    private async Task<DeveloperStudio?> GetDeveloperStudioByName(string studioName)
    {
        return await _dbContext.DeveloperStudios.FirstOrDefaultAsync(d => d.Name == studioName);
    }
}