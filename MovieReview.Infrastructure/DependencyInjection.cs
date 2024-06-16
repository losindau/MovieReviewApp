using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieReview.Domain.Interfaces;
using MovieReview.Infrastructure.Commons.Constants;
using MovieReview.Infrastructure.Data;
using MovieReview.Infrastructure.Repositories;

namespace MovieReview.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(DbConstants.DbName)
            );

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            return services;
        }
    }
}