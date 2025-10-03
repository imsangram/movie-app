using AutoMapper;
using MovieApp.Application.DTO;
using MovieApp.Application.Models;

namespace MovieApp.Application
{
    /// <summary>
    /// Map between Application Models and DTOs
    /// </summary>
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<MovieModel, MovieBaseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MovieId));
            CreateMap<MovieDetailModel, MovieDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MovieId));
        }
    }
}
