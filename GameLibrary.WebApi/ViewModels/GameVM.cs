using System.ComponentModel.DataAnnotations;
using GameLibrary.WebApi.Models;

namespace GameLibrary.WebApi.ViewModels
{
    public class GameVM
    {
        [Required]
        public string GameName { get; set; }

        [Required]
        public string DeveloperStudioName { get; set; }

        [Required]
        public HashSet<string> Genres { get; set; }
    }
}
