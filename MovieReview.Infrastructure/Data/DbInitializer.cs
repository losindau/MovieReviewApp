using Microsoft.EntityFrameworkCore;
using MovieReview.Domain.Entities;
using MovieReview.Infrastructure.Commons.Constants;
using MovieReview.Infrastructure.Commons.Helpers;

namespace MovieReview.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                await SeedDataMovies(context);
                await SeedDataReviews(context);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task SeedDataMovies(ApplicationDbContext context)
        {
            if (await context.Movies.AnyAsync())
            {
                return;
            }

            string path = Path.Combine(JsonConstants.Folder, JsonConstants.MoviesFileName);

            List<Movie> movie = JsonHelper.LoadJsonFile<Movie>(path).ToList();

            context.Movies.AddRange(movie);
            await context.SaveChangesAsync();
        }

        public static async Task SeedDataReviews(ApplicationDbContext context)
        {
            if (await context.Reviews.AnyAsync())
            {
                return;
            }

            string path = Path.Combine(JsonConstants.Folder, JsonConstants.ReviewsFileName);

            List<Review> review = JsonHelper.LoadJsonFile<Review>(path).ToList();

            context.Reviews.AddRange(review);
            await context.SaveChangesAsync();
        }
    }
}
