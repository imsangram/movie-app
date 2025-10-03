using Microsoft.Extensions.DependencyInjection;
using MovieApp.Application.Interfaces;
using MovieApp.Application.Services;

namespace MovieApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMovieAggregatorService, MovieAggregatorService>();
            services.AddAutoMapper(cfg => { }, typeof(ApplicationMappingProfile).Assembly);

            return services;
        }
    }
}
