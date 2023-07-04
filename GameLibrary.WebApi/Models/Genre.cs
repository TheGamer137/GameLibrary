namespace GameLibrary.WebApi.Models;

public class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; }
    public ICollection<GameGenre> GameGenres { get; set; }
}