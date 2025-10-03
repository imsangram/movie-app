
using MovieApp.Application.Models;
using MovieApp.Core;

namespace MovieApp.Application.Interfaces
{
    /// <summary>
    /// For infrastructure services that call external endpoints to fetch movie data.
    /// </summary>
    public interface IExternalEndpointService
    {
        Task<Result<IEnumerable<MovieModel>>> GetAll();
        Task<Result<MovieDetailModel>> GetById(string id);
    }

    // Provider spefic interfaces for future extension

    public interface ICinemaProviderService : IExternalEndpointService { }

    public interface IFilmProviderService : IExternalEndpointService { }
}
