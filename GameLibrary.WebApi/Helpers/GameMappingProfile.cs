using AutoMapper;
using GameLibrary.WebApi.ViewModels;
using GameLibrary.WebApi.Models;

namespace GameLibrary.WebApi.Helpers
{
    public class GameMappingProfile : Profile
    {
        public GameMappingProfile()
        {
            CreateMap<GameVM, Game>()
                .ForMember(dest => dest.GameGenres, opt => opt.MapFrom(src =>
                    src.Genres.Select(genreName => new GameGenre { Genre = new Genre { GenreName = genreName } })))
                .ForMember(dest => dest.DeveloperStudio, opt => opt.MapFrom(src =>
                    new DeveloperStudio { Name = src.DeveloperStudioName })).ReverseMap();
            CreateMap<Game, GameVM>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(g => g.GameGenres.Select(gg => gg.Genre.GenreName)));
        }
    }
}
