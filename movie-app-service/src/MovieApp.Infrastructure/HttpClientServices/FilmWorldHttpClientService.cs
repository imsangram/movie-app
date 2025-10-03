using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieApp.Application.Interfaces;
using MovieApp.Application.Models;
using MovieApp.Core;
using MovieApp.Infrastructure.ExternalDto;

namespace MovieApp.Infrastructure.HttpClientServices
{
    public class FilmWorldHttpClientService(HttpClient httpClient, ILogger<FilmWorldHttpClientService> logger, IMapper mapper) : IFilmProviderService
    {
        public async Task<Result<IEnumerable<MovieModel>>> GetAll()
        {
            var response = await httpClient.GetAsync("movies");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<MovieServiceDto>();
                return Result<IEnumerable<MovieModel>>.Success(mapper.Map<IEnumerable<MovieModel>>(content.Movies));
            }

            logger.LogError($"Error fetching movies from CinemaWorld. Status Code: {response.StatusCode}");
            return Result<IEnumerable<MovieModel>>.Failure(ErrorType.Unexpected);
        }

        public async Task<Result<MovieDetailModel>> GetById(string id)
        {
            var response = await httpClient.GetAsync($"movie/fw{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<MovieDetailServiceDto>();
                var result = mapper.Map<MovieDetailModel>(content);
                return Result<MovieDetailModel>.Success(result);
            }
            var errorType = response.StatusCode switch
            {
                HttpStatusCode.NotFound => ErrorType.NotFound,
                _ => ErrorType.Unexpected
            };
            logger.LogError($"Error fetching movie details from CinemaWorld for ID cw{id}. Status Code: {response.StatusCode}");
            return Result<MovieDetailModel>.Failure(errorType);
        }
    }
}
