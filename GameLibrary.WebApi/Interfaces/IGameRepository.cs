using GameLibrary.WebApi.Models;

namespace GameLibrary.WebApi.Interfaces
{
    public interface IGameRepository
    {
        /// <summary>
        /// Метод получает все игры из бд
        /// </summary>
        /// <returns></returns>
        IEnumerable<Game> GetAllGames();

        /// <summary>
        /// Метод ищет игру по её Id
        /// </summary>
        /// <param name="id">Id игры которую нужно найти</param>
        /// <returns>Игру из бд с Id</returns>
        Game? GetGameById(int id);

        /// <summary>
        /// Метод ищет игры по жанру
        /// </summary>
        /// <param name="genreName">Название жанра</param>
        /// <returns>Список игр с указанным жанром</returns>
        IEnumerable<Game> GetGamesByGenre(string genreName);

        /// <summary>
        /// Метод добавляет либо обновляет игру в бд
        /// </summary>
        /// <param name="game">Игра которую, нужно добавить</param>
        void SaveGame(Game game);

        /// <summary>
        /// Метод удаляет игру из бд
        /// </summary>
        /// <param name="game">Игра которую, нужно удалить</param>
        void DeleteGame(Game game);
    }
}
