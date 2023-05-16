using AutoMapper;
using FakeItEasy;
using GameLibrary.WebApi.Controllers;
using GameLibrary.WebApi.Interfaces;
using GameLibrary.WebApi.Models;
using GameLibrary.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GameLibrary.Tests.Controllers
{
    public class GameControllerTests
    {
       
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        public GameControllerTests() 
        {
            _gameRepository = A.Fake<IGameRepository>();
            _mapper = A.Fake<IMapper>();
        }
        #region GetGames
        [Fact]
        public void GetGames_Returns_OK()
        {
            //Arrange
            var games = new List<Game> { new Game() };
            A.CallTo(() => _gameRepository.GetAllGames()).Returns(games);
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.GetGames();
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void GetGames_Returns_NotFound()
        {
            //Arrange
            var emptyList = Enumerable.Empty<Game>();
            A.CallTo(() => _gameRepository.GetAllGames()).Returns(emptyList);
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.GetGames();
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
        #region CreateGame
        [Fact]
        public void Create_Returns_Ok()
        {
            //Arrange
            var mapGame = new GameVM();
            var newGame = new Game();
            A.CallTo(() => _gameRepository.SaveGame(newGame)).DoesNothing();
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.Create(mapGame);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Create_Returns_BadRequest()
        {
            //Arrange
            var gameVm = new GameVM();
            var controller = new GamesController(_gameRepository, _mapper);
            controller.ModelState.AddModelError("test", "test");
            //Act
            var result = controller.Create(gameVm);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion
        #region GetGamesByGenre
        [Fact]
        public void GetGamesByGenre_Returns_Ok()
        {
            //Arrange
            var genre = "";
            var games = new Game[]{new Game()};
            A.CallTo(() => _gameRepository.GetGamesByGenre(genre)).Returns(games);
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.GetGamesByGenre(genre);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void GetGamesByGenre_Returns_NotFound()
        {
            //Arrange
            var genre = "";
            A.CallTo(() => _gameRepository.GetGamesByGenre(genre)).Returns(Enumerable.Empty<Game>());
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.GetGamesByGenre(genre);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void GetGamesByGenre_Returns_BadRequest()
        {
            //Arrange
            var emptyList = Enumerable.Empty<Game>();
            A.CallTo(() => _gameRepository.GetGamesByGenre(null)).Returns(emptyList);
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.GetGamesByGenre(null);
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion
        #region Update
        [Fact]
        public void UpdateGame_Returns_Ok()
        {
            //Arrange
            int id = 1;
            var existingGame = new Game();
            var gameVM = new GameVM();
            A.CallTo(() => _gameRepository.GetGameById(id)).Returns(existingGame);
            A.CallTo(() => _gameRepository.SaveGame(existingGame)).DoesNothing();
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.Update(id, gameVM);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void Update_Returns_BadRequest()
        {
            //Arrange
            int id = 1;
            var controller = new GamesController(_gameRepository,  _mapper);
            controller.ModelState.AddModelError("test", "test");
            //Act
            var result = controller.Update(id, null);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Update_Returns_NotFound()
        {
            //Arrange
            int id = 1;
            var gameVM = new GameVM();
            A.CallTo(() => _gameRepository.GetGameById(id)).Returns(null);
            var controller = new GamesController(_gameRepository,  _mapper);
            //Act
            var result = controller.Update(id, gameVM);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
        #region Delete
        [Fact]
        public void Delete_Returns_Ok()
        {
            //Arrange
            int id = 1;
            var existingGame = new Game();
            A.CallTo(() => _gameRepository.GetGameById(id)).Returns(existingGame);
            A.CallTo(() => _gameRepository.DeleteGame(existingGame)).DoesNothing();
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.Delete(id);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void Delete_Returns_NotFound()
        {
            //Arrange
            int id = 1;
            A.CallTo(() => _gameRepository.GetGameById(id)).Returns(null);
            var controller = new GamesController(_gameRepository, _mapper);
            //Act
            var result = controller.Delete(id);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
    }
}
