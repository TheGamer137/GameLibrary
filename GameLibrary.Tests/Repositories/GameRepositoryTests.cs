using FakeItEasy;
using GameLibrary.WebApi.Controllers;
using GameLibrary.WebApi.Data;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using GameLibrary.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Tests.Repositories
{
    public class GameRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        public GameRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("SqlServerConnection")
                .Options;
        }

        [Fact]
        public void GetAllGames_Success_Test()
        {
            //Arrange
            IGameRepository repository = new GameRepository(GetDbContext());
            //Act
            var result = repository.GetAllGames();
            //Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void SaveGame_CreatesNewGame()
        {
            // Arrange
            Game game = new Game
            {
                GameName = "test",
                DeveloperStudio = new DeveloperStudio { Name = "test" },
                GameGenres = new GameGenre[]
                {
                    new GameGenre
                    {
                        Genre = new Genre{GenreName = "test"}
                    }
                }
            };
            IGameRepository repository = new GameRepository(GetDbContext());

            // Act
            repository.SaveGame(game);
            var games = repository.GetAllGames();

            // Assert
            Assert.Equal(4, games.Count());
        }

        [Fact]
        public void SaveGame_GameAlreadyExists_ThrowException()
        {
            // Arrange
            Game game = new Game
            {
                GameName = "The Witcher 3",
                DeveloperStudio = new DeveloperStudio { Name = "test" },
                GameGenres = new GameGenre[]
                {
                    new GameGenre
                    {
                        Genre = new Genre{GenreName = "test"}
                    }
                }
            };
            IGameRepository repository = new GameRepository(GetDbContext());

            // Act
            Action act = () => repository.SaveGame(game);

            // Assert
            Exception exception = Assert.Throws<Exception>((act));
            Assert.Equal("Такая игра уже есть в базе данных", exception.Message);
        }

        [Fact]
        public void SaveGame_UpdatesGame()
        {
            // Arrange
            IGameRepository repository = new GameRepository(GetDbContext());
            var gameToUpdate = repository.GetGameById(3);
            gameToUpdate.GameName = "test";
            gameToUpdate.DeveloperStudio = new DeveloperStudio();
            gameToUpdate.DeveloperStudio.Name = "test";
            gameToUpdate.GameGenres = new List<GameGenre>();
            foreach (var gg in gameToUpdate.GameGenres)
            {
                gg.Genre.GenreName = "test";
            }
            // Act
            repository.SaveGame(gameToUpdate);

            // Assert
            Assert.Equal("test", repository.GetGameById(3).GameName);
        }

        [Fact]
        public void GetGameById_Success_Test()
        {
            // Arrange
            IGameRepository repository = new GameRepository(GetDbContext());

            // Act
            var game = repository.GetGameById(1);

            // Assert
            Assert.Equal("The Witcher 3", game.GameName);
        }

        [Fact]
        public void GetGameByGenre_Success_Test()
        {
            // Arrange
            IGameRepository repository = new GameRepository(GetDbContext());

            // Act
            var games = repository.GetGamesByGenre("rpg");

            // Assert
            Assert.Equal(2, games.Count());
        }

        [Fact]
        public void DeleteGame_Success_Test()
        {
            // Arrange
            IGameRepository repository = new GameRepository(GetDbContext());
            var gameToDelete = repository.GetGameById(3);

            // Act
            repository.DeleteGame(gameToDelete);
            var allGames = repository.GetAllGames();

            // Assert
            Assert.Equal(2, allGames.Count());
        }
        private AppDbContext GetDbContext()
        {
            AppDbContext context = new AppDbContext(_options);
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
                        Name = "CD Project RED",
                    },
                    GameGenres = new GameGenre[]
                    {
                       new GameGenre
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
                    GameGenres = new GameGenre[]
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
                        Name = "NetherRealm",
                    },
                    GameGenres = new GameGenre[]
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
}
