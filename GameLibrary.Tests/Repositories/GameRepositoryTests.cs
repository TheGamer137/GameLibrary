using FluentAssertions;
using GameLibrary.WebApi.Data;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using GameLibrary.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Tests.Repositories;

public class GameRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _options;

    public GameRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllGames_Success_Test()
    {
        //Arrange
        IGameRepository repository = new GameRepository(GetDbContext());
        //Act
        var result = await repository.GetAllGames();
        //Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task SaveGame_CreatesNewGame()
    {
        // Arrange
        var game = new Game
        {
            GameName = "test",
            DeveloperStudio = new DeveloperStudio { Name = "test" },
            GameGenres = new[]
            {
                new GameGenre()
                {
                    Genre = new Genre { GenreName = "test" }
                }
            }
        };
        IGameRepository repository = new GameRepository(GetDbContext());

        // Act
        repository.SaveGame(game);
        var games = await repository.GetAllGames();

        // Assert
        games.Should().HaveCount(4);
    }

    [Fact]
    public async Task SaveGame_UpdatesGame()
    {
        // Arrange
        IGameRepository repository = new GameRepository(GetDbContext());
        var gameToUpdate = await repository.GetGameById(2);
        gameToUpdate.GameName = "test";
        gameToUpdate.DeveloperStudio = new DeveloperStudio();
        gameToUpdate.DeveloperStudio.Name = "test";
        gameToUpdate.GameGenres = new List<GameGenre>()
        {
            new GameGenre
            {
                Genre = new Genre
                {
                    GenreName = "test"
                }
            }
        };
        // Act
        await repository.SaveGame(gameToUpdate);

        // Assert
        var updatedGame = await repository.GetGameById(2);
        updatedGame.GameName.Should().Be("test");
        updatedGame.GameGenres.Should().HaveCount(1);
        updatedGame.GameGenres.First().Genre.GenreName.Should().Be("test");
    }

    [Fact]
    public async Task GetGameById_Success_Test()
    {
        // Arrange
        IGameRepository repository = new GameRepository(GetDbContext());

        // Act
        var result = await repository.GetGameById(1);

        // Assert
        result.GameName.Should().Be("The Witcher 3");
    }

    [Fact]
    public async Task GetGameByGenre_Success_Test()
    {
        // Arrange
        IGameRepository repository = new GameRepository(GetDbContext());

        // Act
        var result = await repository.GetGamesByGenre("rpg");

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteGame_Success_Test()
    {
        // Arrange
        IGameRepository repository = new GameRepository(GetDbContext());
        var gameToDelete = await repository.GetGameById(3);

        // Act
        await repository.DeleteGame(gameToDelete);
        var result = await repository.GetAllGames();

        // Assert
        result.Should().HaveCount(2);
    }

    private AppDbContext GetDbContext()
    {
        var context = new AppDbContext(_options);
        context.Games.AddRange(GetGamesForTest());
        context.SaveChanges();
        return context;
    }

    private Game[] GetGamesForTest()
    {
        return new[]
        {
            new Game
            {
                GameName = "The Witcher 3",
                DeveloperStudio = new DeveloperStudio
                {
                    Name = "CD Project RED"
                },
                GameGenres = new[]
                {
                    new()
                    {
                        Genre = new Genre
                        {
                            GenreName = "RPG"
                        }
                    },
                    new GameGenre
                    {
                        Genre = new Genre
                        {
                            GenreName = "Action"
                        }
                    }
                }
            },
            new Game
            {
                GameName = "Mass Effect",
                DeveloperStudio = new DeveloperStudio
                {
                    Name = "BioWare"
                },
                GameGenres = new[]
                {
                    new GameGenre
                    {
                        GenreId = 1
                    },
                    new GameGenre
                    {
                        GenreId = 2
                    }
                }
            },
            new Game
            {
                GameName = "Mortal Kombat 11",
                DeveloperStudio = new DeveloperStudio
                {
                    Name = "NetherRealm"
                },
                GameGenres = new[]
                {
                    new GameGenre
                    {
                        Genre = new Genre
                        {
                            GenreName = "Fighting"
                        }
                    }
                }
            }
        };
    }
}