using System.ComponentModel.DataAnnotations;

namespace GameLibrary.WebApi.ViewModels;

public class GameVM
{
    [Required] public string GameName { get; set; }

    [Required] public string DeveloperStudioName { get; set; }

    [Required] public HashSet<string> Genres { get; set; }
}