using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MovieApp.Application.Interfaces;
using MovieApp.Infrastructure.HttpClientServices;

namespace MovieApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(InfrastructureMappingProfile).Assembly);

            services.AddHttpClient<ICinemaProviderService, CinemaWorldHttpClientService>((sp, httpClient) =>
            {
                var settings = sp.GetRequiredService<IOptions<ServiceProvideConfiguration>>().Value;
                httpClient.BaseAddress = new Uri(settings.CinemaServiceProviderUrl);
                httpClient.DefaultRequestHeaders.Add("x-access-token", settings.Token);
                httpClient.Timeout = TimeSpan.FromSeconds(settings.TimeOut);
            })
            .AddPolicyHandler(Policies.RetryPolicy);

            services.AddHttpClient<IFilmProviderService, FilmWorldHttpClientService>((sp, httpClient) =>
            {
                var settings = sp.GetRequiredService<IOptions<ServiceProvideConfiguration>>().Value;
                httpClient.BaseAddress = new Uri(settings.FilmServiceProviderUrl);
                httpClient.DefaultRequestHeaders.Add("x-access-token", settings.Token);
                httpClient.Timeout = TimeSpan.FromSeconds(settings.TimeOut);
            })
            .AddPolicyHandler(Policies.RetryPolicy);
            return services;
        }
    }
}
