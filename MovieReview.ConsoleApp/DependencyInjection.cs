using Microsoft.Extensions.DependencyInjection;

namespace MovieReview.ConsoleApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<MovieReviewApp>();
            return services;
        }
    }
}
