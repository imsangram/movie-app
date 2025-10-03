using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Interfaces;

namespace MovieApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieAggregatorService _movieAggregateService;

        public MoviesController(IMovieAggregatorService movieAggregateService)
        {
            _movieAggregateService = movieAggregateService;
        }

        // GET /Movie
        /// <summary>
        /// Returns all movies from both providers
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllMovies")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _movieAggregateService.GetAll();
            return Ok(result);
        }

        // GET /Movie/{id}
        /// <summary>
        /// Returns the movie with the specified ID, or a 404 if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMovieById")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _movieAggregateService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
