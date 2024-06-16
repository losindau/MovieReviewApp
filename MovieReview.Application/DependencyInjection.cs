using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using MovieReview.Application.Interfaces;
using MovieReview.Application.Services;
using System.Reflection;

namespace MovieReview.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IReviewService, ReviewService>();

            return services;
        }
    }
}