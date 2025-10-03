using AutoMapper;
using MovieApp.Application.DTO;
using MovieApp.Application.Interfaces;
using MovieApp.Core;
using static MovieApp.Core.Exceptions;

namespace MovieApp.Application.Services
{
    public class MovieAggregatorService(ICinemaProviderService cinemaService, IFilmProviderService filmService, IMapper mapper) : IMovieAggregatorService
    {
        public async Task<IEnumerable<MovieBaseDto>> GetAll()
        {
            var tasks = new[]
            {
                cinemaService.GetAll(),
                filmService.GetAll()
            };

            var results = await Task.WhenAll(tasks);

            // None of the providers are available
            if (results.All(r => !r.IsSuccess))
                throw new ApplicationException($"All the providers are down");

            var result = results.Where(r => r.IsSuccess)
                .SelectMany(x => x.Value).DistinctBy(m => m.MovieId);
            return mapper.Map<List<MovieBaseDto>>(result);
        }

        public async Task<MovieDetailDto> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var tasks = new[]
            {
                cinemaService.GetById(id),
                filmService.GetById(id)
            };

            var results = await Task.WhenAll(tasks);

            // None of the providers are available
            if (results.All(r => !r.IsSuccess && r.Error == ErrorType.NotFound))
                throw new NotFoundException($"Cannot find the movie with id {id}");

            if (results.All(r => !r.IsSuccess))
                throw new ApplicationException($"All the providers are down");

            // Sort by price and return the movie with the cheapest price
            var result = results.Where(r => r.IsSuccess)
                .OrderBy(r => r.Value.Price).First().Value;

            return mapper.Map<MovieDetailDto>(result);
        }
    }
}
