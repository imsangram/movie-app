using AutoMapper;
using FluentAssertions;
using Moq;
using MovieApp.Application.DTO;
using MovieApp.Application.Interfaces;
using MovieApp.Application.Models;
using MovieApp.Application.Services;
using MovieApp.Core;

namespace MovieApp.Tests.Application
{
    public class MovieAggregatorServiceTests
    {
        private readonly Mock<ICinemaProviderService> _cinemaServiceMock;
        private readonly Mock<IFilmProviderService> _filmServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly MovieAggregatorService _service;

        public MovieAggregatorServiceTests()
        {
            _cinemaServiceMock = new Mock<ICinemaProviderService>();
            _filmServiceMock = new Mock<IFilmProviderService>();
            _mapperMock = new Mock<IMapper>();
            _service = new MovieAggregatorService(
                _cinemaServiceMock.Object,
                _filmServiceMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAll_WhenBothProvidersSucceed_ShouldReturnDistinctMovies()
        {
            // Arrange
            var cinemaMovies = new List<MovieModel>
            {
                new() { MovieId = "1", Title = "Movie 1", Year = 2023 },
                new() { MovieId = "2", Title = "Movie 2", Year = 2023 }
            };
            var filmMovies = new List<MovieModel>
            {
                new() { MovieId = "1", Title = "Movie 1", Year = 2023 }, // Duplicate
                new() { MovieId = "3", Title = "Movie 3", Year = 2023 }
            };

            var expectedDtos = new List<MovieBaseDto>
            {
                new() { Id = "1", Title = "Movie 1", Year = 2023 },
                new() { Id = "2", Title = "Movie 2", Year = 2023 },
                new() { Id = "3", Title = "Movie 3", Year = 2023 }
            };

            _cinemaServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(Result<IEnumerable<MovieModel>>.Success(cinemaMovies));
            _filmServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(Result<IEnumerable<MovieModel>>.Success(filmMovies));
            _mapperMock.Setup(x => x.Map<List<MovieBaseDto>>(It.IsAny<IEnumerable<MovieModel>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _service.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            _mapperMock.Verify(x => x.Map<List<MovieBaseDto>>(It.IsAny<IEnumerable<MovieModel>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenOnlyOneProviderSucceeds_ShouldReturnResultsFromSuccessfulProvider()
        {
            // Arrange
            var cinemaMovies = new List<MovieModel>
            {
                new() { MovieId = "1", Title = "Movie 1", Year = 2023 }
            };

            var expectedDtos = new List<MovieBaseDto>
            {
                new() { Id = "1", Title = "Movie 1", Year = 2023 }
            };

            _cinemaServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(Result<IEnumerable<MovieModel>>.Success(cinemaMovies));
            _filmServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(Result<IEnumerable<MovieModel>>.Failure(ErrorType.Unexpected));
            _mapperMock.Setup(x => x.Map<List<MovieBaseDto>>(It.IsAny<IEnumerable<MovieModel>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _service.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAll_WhenAllProvidersFail_ShouldThrowApplicationException()
        {
            // Arrange
            _cinemaServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(Result<IEnumerable<MovieModel>>.Failure(ErrorType.Unexpected));
            _filmServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(Result<IEnumerable<MovieModel>>.Failure(ErrorType.Unexpected));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAll());
            exception.Message.Should().Contain("All the providers are down");
        }

        [Fact]
        public async Task GetById_WithValidId_WhenBothProvidersHaveMovie_ShouldReturnCheapestMovie()
        {
            // Arrange
            var movieId = "123";
            var cinemaMovie = new MovieDetailModel
            {
                MovieId = movieId,
                Title = "Movie 1",
                Price = 15.50m
            };
            var filmMovie = new MovieDetailModel
            {
                MovieId = movieId,
                Title = "Movie 1",
                Price = 12.99m
            };

            var expectedDto = new MovieDetailDto
            {
                Id = movieId,
                Title = "Movie 1",
                Price = 12.99m
            };

            _cinemaServiceMock.Setup(x => x.GetById(movieId))
                .ReturnsAsync(Result<MovieDetailModel>.Success(cinemaMovie));
            _filmServiceMock.Setup(x => x.GetById(movieId))
                .ReturnsAsync(Result<MovieDetailModel>.Success(filmMovie));
            _mapperMock.Setup(x => x.Map<MovieDetailDto>(filmMovie))
                .Returns(expectedDto);

            // Act
            var result = await _service.GetById(movieId);

            // Assert
            result.Should().NotBeNull();
            result.Price.Should().Be(12.99m);
            _mapperMock.Verify(x => x.Map<MovieDetailDto>(filmMovie), Times.Once);
        }

        [Fact]
        public async Task GetById_WithValidId_WhenOnlyOneProviderHasMovie_ShouldReturnThatMovie()
        {
            // Arrange
            var movieId = "123";
            var cinemaMovie = new MovieDetailModel
            {
                MovieId = movieId,
                Title = "Movie 1",
                Price = 15.50m
            };

            var expectedDto = new MovieDetailDto
            {
                Id = movieId,
                Title = "Movie 1",
                Price = 15.50m
            };

            _cinemaServiceMock.Setup(x => x.GetById(movieId))
                .ReturnsAsync(Result<MovieDetailModel>.Success(cinemaMovie));
            _filmServiceMock.Setup(x => x.GetById(movieId))
                .ReturnsAsync(Result<MovieDetailModel>.Failure(ErrorType.NotFound));
            _mapperMock.Setup(x => x.Map<MovieDetailDto>(cinemaMovie))
                .Returns(expectedDto);

            // Act
            var result = await _service.GetById(movieId);

            // Assert
            result.Should().NotBeNull();
            result.Price.Should().Be(15.50m);
        }

        [Fact]
        public async Task GetById_WhenAllProvidersAreDown_ShouldThrowApplicationException()
        {
            // Arrange
            var movieId = "123";

            _cinemaServiceMock.Setup(x => x.GetById(movieId))
                .ReturnsAsync(Result<MovieDetailModel>.Failure(ErrorType.Unexpected));
            _filmServiceMock.Setup(x => x.GetById(movieId))
                .ReturnsAsync(Result<MovieDetailModel>.Failure(ErrorType.Unexpected));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetById(movieId));
            exception.Message.Should().Contain("All the providers are down");
        }
    }
}
