using AutoMapper;
using MovieApp.Infrastructure.ExternalDto;

namespace MovieApp.Infrastructure
{
    /// <summary>
    /// Mapping profile for AutoMapper to map between service DTOs and application models.
    /// </summary>
    public class InfrastructureMappingProfile : Profile
    {
        public InfrastructureMappingProfile()
        {
            CreateMap<MovieBaseServiceDto, Application.Models.MovieModel>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Id.Remove(0, 2)));

            CreateMap<MovieDetailServiceDto, Application.Models.MovieDetailModel>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Id.Remove(0, 2)));
        }
    }
}
