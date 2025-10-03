using MovieApp.Application.DTO;

namespace MovieApp.Application.Interfaces
{
    public interface IMovieAggregatorService
    {
        Task<IEnumerable<MovieBaseDto>> GetAll();
        Task<MovieDetailDto> GetById(string id);
    }
}
