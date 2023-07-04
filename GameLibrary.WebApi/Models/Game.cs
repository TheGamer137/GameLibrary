namespace GameLibrary.WebApi.Models;

public class Game
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public DeveloperStudio DeveloperStudio { get; set; }
    public ICollection<GameGenre> GameGenres { get; set; }
}